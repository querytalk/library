#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents an exception object of the QueryTalk library.
    /// </summary>
    [Serializable]
    public sealed class QueryTalkException : System.Exception
    {
        private QueryTalkExceptionType _exceptionType = QueryTalkExceptionType.UnknownQueryTalkException;
        /// <summary>
        /// The exception type.
        /// </summary>
        public QueryTalkExceptionType ExceptionType
        {
            get
            {
                return _exceptionType;
            }
        }

        private string _objectName = null;
        /// <summary>
        /// The name of a QueryTalk object which was being created or executed in the operation that has thrown the exception.
        /// </summary> 
        public string ObjectName
        {
            get
            {
                return _objectName;
            }

            internal set
            {
                _objectName = value;
            }
        }

        internal QueryTalkException SetObjectName(string name)
        {
            _objectName = name;
            return this;
        }

        private string _method = null;
        /// <summary>
        /// A chain method that has thrown the exception.
        /// </summary>
        public string Method
        {
            get
            {
                return _method;
            }

            internal set
            {
                _method = value;
            }
        }

        private string _arguments = Text.NotAvailable;
        /// <summary>
        /// The exception argument(s) that caused the exception.
        /// </summary>
        public string Arguments
        {
            get
            {
                return _arguments;
            }

            internal set
            {
                _arguments = value;
            }
        }

        private string _extra = null;
        /// <summary>
        /// Extra exception information that is added later in the exception report building process.
        /// </summary>
        public string Extra
        {
            get
            {
                return _extra;
            }

            internal set
            {
                _extra = value;
            }
        }
        internal QueryTalkException SetExtra(string extra)
        {
            _extra = extra;
            return this;
        }

        private string _tip = Text.NotAvailable;
        /// <summary>
        /// A tip that helps to resolve the exception.
        /// </summary>
        public string Tip
        {
            get
            {
                return _tip;
            }
        }

        private bool _hasReport = false;
        private string _message;
        private System.Exception _clrException;

        /// <summary>
        /// A CLR exception caught by the QueryTalk library.
        /// </summary>
        public System.Exception ClrException
        {
            get
            {
                return _clrException;
            }
            internal set
            {
                _clrException = value;
            }
        }

        /// <summary>
        /// The original ERROR_NUMBER() value of a SQL error.
        /// </summary>
        internal int SqlErrorNumber
        {
            get
            {
                if (_clrException != null && _clrException is System.Data.SqlClient.SqlException)
                {
                    var sqlException = (System.Data.SqlClient.SqlException)_clrException;

                    // try extract ERROR_NUMBER() from ERROR_MESSAGE() - provided by QueryTalk in format <num>|<message> 
                    var reg = new Regex(@"^(\d+)(?=\|)");
                    var match = reg.Match(sqlException.Message);
                    if (match != null && match.Length > 0)
                    {
                        int errorNumber;
                        if (int.TryParse(match.Value, out errorNumber))
                        {
                            return errorNumber;
                        }
                        else
                        {
                            return 0;
                        }
                    }

                    return 50000;  // user custom error
                }

                return 0;
            }
        }

        private ExceptionAttribute _exceptionAttribute =
            QueryTalkExceptionType.UnknownQueryTalkException.GetAttributeOfType<ExceptionAttribute>();

        #region Internal

        internal QueryTalkException(
            object exceptionCreator,
            QueryTalkExceptionType exceptionType,
            string arguments,
            string method = null)
        {
            _hasReport = true;
            _exceptionType = exceptionType;
            _exceptionAttribute = exceptionType.GetAttributeOfType<ExceptionAttribute>();
            _arguments = arguments ?? Text.NotAvailable;
            _tip = _exceptionAttribute.Tip;
            _method = method;
        }

        internal QueryTalkException(
            object exceptionCreator,
            QueryTalkExceptionType exceptionType,
            string objectName,
            string method,
            string arguments)
        {
            _hasReport = true;
            _exceptionType = exceptionType;
            _exceptionAttribute = exceptionType.GetAttributeOfType<ExceptionAttribute>();
            _objectName = objectName ?? Text.NotAvailable;
            _method = method ?? Text.NotAvailable;
            _arguments = arguments ?? Text.NotAvailable;
            _tip = _exceptionAttribute.Tip;
        }

        #endregion

        #region Public

        /// <summary>
        /// Initializes a new instance of the QueryTalkException class.
        /// </summary>
        public QueryTalkException()
        { }

        /// <summary>
        /// Initializes a new instance of the QueryTalkException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public QueryTalkException(string message)
            : base(message)
        {
            _message = message;
        }

        /// <summary>
        /// Initializes a new instance of the QueryTalkException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        public QueryTalkException(string message, System.Exception inner)
            : base(message, inner)
        {
            _message = message;
        }

        /// <summary>
        /// Initializes a new instance of the QueryTalkException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        private QueryTalkException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                _message = info.GetString("_message");
                _objectName = info.GetString("_objectName");
                _method = info.GetString("_method");
                _arguments = info.GetString("_arguments");
                _extra = info.GetString("_extra");
            }
        }

        /// <summary>
        /// Provide the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue("_message", Message);
                info.AddValue("_objectName", _objectName);
                info.AddValue("_method", _method);
                info.AddValue("_arguments", _arguments);
                info.AddValue("_extra", _extra);
            }
        }

        #endregion

        #region Report

        /// <summary>
        /// The description of the exception in a structured report message. 
        /// </summary>
        public string Report
        {
            get
            {
                if (_hasReport)
                {
                    var report = new StringBuilder(300);
                    string exceptionType = _exceptionType.ToString();

                    report.AppendFormat("QueryTalkException: {0}   {1}{2}", Environment.NewLine, exceptionType, Environment.NewLine);
                    report.AppendFormat("Message: {0}   {1}{2}", Environment.NewLine, _message ?? _exceptionAttribute.Message, Environment.NewLine);

                    var clrMessage = GetClrException();
                    if (clrMessage != null)
                    {
                        report.AppendFormat("ClrException: {0}   {1}{2}", Environment.NewLine, clrMessage, Environment.NewLine);
                    }

                    // get client
                    var client = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
                    var clientName = Text.NotAvailable;
                    if (client != null)
                    {
                        clientName = client.GetName().Name;
                    }
                    if (clientName == "QueryTalk")
                    {
                        clientName = Text.NotAvailable;
                    }

                    report.AppendFormat("Object: {0}   {1}{2}", Environment.NewLine, _objectName ?? Text.NotAvailable, Environment.NewLine);
                    report.AppendFormat("Method: {0}   {1}{2}", Environment.NewLine, _method ?? Text.NotAvailable, Environment.NewLine);
                    report.AppendFormat("Argument(s): {0}   {1}{2}", Environment.NewLine, _arguments, Environment.NewLine);
                    if (_extra != null)
                    {
                        report.AppendFormat("Extra: {0}   {1}{2}", Environment.NewLine, _extra, Environment.NewLine);
                    }
                    report.AppendFormat("Tip: {0}   {1}{2}", Environment.NewLine, _tip ?? Text.NotAvailable, Environment.NewLine);

                    return report.ToString();
                }
                else
                {
                    return _message;
                }
            }
        }

        private string GetClrException()
        {
            var dump = new StringBuilder();
            if (_clrException != null)
            {
                int ei = 1;
                dump.AppendFormat("[{0}] {1}: {2} ", ei++, _clrException.GetType(), _clrException.Message);

                var innerException = _clrException.InnerException;
                while (innerException != null)
                {
                    dump.AppendFormat("[{0}] {1}: {2} ", ei++, innerException.GetType(), innerException.Message);
                    innerException = innerException.InnerException;
                }
            }

            if (dump.Length == 0)
            {
                return null;
            }
            else
            {
                // remove line feeds
                return Regex.Replace(dump.ToString(), @"\s{2,}", " ");
            }
        }

        #endregion

        /// <summary>
        /// The message that describes the exception.
        /// </summary>
        public override string Message
        {
            get
            {
                if (_clrException != null)
                {
                    return _clrException.Message;
                }
                else
                {
                    return _message ?? _exceptionAttribute.Message;
                }
            }
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return _exceptionAttribute.Message;
        }
    }
}
