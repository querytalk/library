#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace QueryTalk.Wall
{
    internal class Admin
    {

        #region Internal Settings

        // Indicates if the inline values are parameterized.
        internal const bool IsValueParameterizationOn = true;

        // Indicates if the .NET names are capitalized.
        internal const bool IsCapitalizationOn = true;

        // Indicates if the minimum DateTime correction is done in specific occasions to avoid SQL Server error
        // when converting 1/1/0001 to datetime or smalldatetime.
        internal const bool IsDateTimeMinCorrectionOn = true;

        // Indicates if the loader is cached.
        internal const bool IsLoaderCachingOn = true;

        // Defines how many rows that are shown in the testing environment when simple table query is used.
        internal const int TestSelectTopRows = 1000;

        // Defines the synchronization timeout for the connection.
        internal const int Async1Timeout = 45000;

        // Defines the synchronization timeout between the reader and loader process.
        internal const int Async2Timeout = 3000;

        #endregion

        private static object _locker = new object();

        #region Client Assembly Dependent Settings

        // client settings with the assembly scope
        internal class ClientSettings
        {
            // A delegate method that returns the connection string by the connection key.
            internal Func<ConnectionKey, ConnectionData> ConnectionFunc { get; set; }

            // Indicates if the testing environment is shut down.
            internal bool IsTestingEnvironmentShutDown { get; set; }

            // Indicates if the testing environment is activated.
            private bool _isTestingEnvironmentOn = true;   
            internal bool IsTestingEnvironmentOn
            {
                get
                {
                    return _isTestingEnvironmentOn;
                }
                set
                {
                    _isTestingEnvironmentOn = value;
                }
            }

            // Indicates if the connection function has a global scope (overriding the individual client settings).
            internal bool IsConnectionFuncGlobal { get; set; }

            // // Indicates if the testing environment is shut down globally (overriding the individual client settings).
            internal bool IsTestingEnvironmentShutDownGlobal { get; set; }
        }

        private static ConcurrentDictionary<Assembly, ClientSettings> _clientSettings =
            new ConcurrentDictionary<Assembly, ClientSettings>();

        internal static ClientSettings GetSettings(Assembly client)
        {
            if (client == null)
            {
                throw new QueryTalkException("Admin.GetSettings", QueryTalkExceptionType.NullClientInnerException, "client = null");
            }

            ClientSettings settings;

            lock (_locker)
            {
                if (!_clientSettings.TryGetValue(client, out settings))
                {
                    settings = new ClientSettings();
                    _clientSettings.TryAdd(client, settings);
                }
            }

            return settings;
        }

        internal static void ShutTestingEnvironmentDown(Assembly client, bool @global)
        {
            var settings = Admin.GetSettings(client);
            settings.IsTestingEnvironmentShutDown = true;

            if (@global && CheckTestingEnvironmentGlobalSetting())
            {
                settings.IsTestingEnvironmentShutDownGlobal = true;
            }
        }

        internal static void SetTestingEnvironment(Assembly client, bool value)
        {
            var settings = Admin.GetSettings(client);

            if (_clientSettings.Values.Where(s => s.IsTestingEnvironmentShutDownGlobal).Any() || settings.IsTestingEnvironmentShutDown)
            {
                return;
            }

            settings.IsTestingEnvironmentOn = value;
        }

        internal static bool IsTestingEnvironmentOff(Assembly client)
        {
            if (_clientSettings.Values.Where(s => s.IsTestingEnvironmentShutDownGlobal).Any())
            {
                return true;
            }

            var settings = Admin.GetSettings(client);

            return settings.IsTestingEnvironmentShutDown || !settings.IsTestingEnvironmentOn;
        }

        #endregion

        #region Global Settings

        internal static bool CheckConnectionFuncGlobalSetting()
        {
            return _clientSettings.Values.Where(s => s.IsConnectionFuncGlobal).Count() == 0;
        }

        internal static Func<ConnectionKey, ConnectionData> GetGlobalConnectionFunc()
        {
            var settings = _clientSettings.Values.Where(s => s.IsConnectionFuncGlobal).FirstOrDefault();
            if (settings != null)
            {
                return settings.ConnectionFunc;
            }

            return null;
        }

        internal static bool CheckTestingEnvironmentGlobalSetting()
        {
            return _clientSettings.Values.Where(s => s.IsTestingEnvironmentShutDownGlobal).Count() == 0;
        }

        #endregion

    }
}
