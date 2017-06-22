#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// <para>ERROR_MESSAGE built-in function.</para>
        /// <para>Returns the message text of the error that caused the CATCH block of a TRY…CATCH construct to be run.</para>
        /// </summary>
        public static SysFn ErrorMessage()
        {
            return new SysFn("ERROR_MESSAGE()");
        }

        /// <summary>
        /// <para>ERROR_LINE built-in function.</para>
        /// <para>Returns the line number at which an error occurred that caused the CATCH block of a TRY…CATCH construct to be run.</para>
        /// </summary>
        public static SysFn ErrorLine()
        {
            return new SysFn("ERROR_LINE()");
        }

        /// <summary>
        /// <para>ERROR_NUMBER built-in function.</para>
        /// <para>Returns the error number of the error that caused the CATCH block of a TRY…CATCH construct to be run.</para>
        /// </summary>
        public static SysFn ErrorNumber()
        {
            return new SysFn("ERROR_NUMBER()");
        }

        /// <summary>
        /// <para>ERROR_PROCEDURE built-in function.</para>
        /// <para>Returns the name of the stored procedure or trigger where an error occurred that caused the CATCH block of a TRY…CATCH construct to be run.</para>
        /// </summary>
        public static SysFn ErrorProcedure()
        {
            return new SysFn("ERROR_PROCEDURE()");
        }

        /// <summary>
        /// <para>ERROR_SEVERITY built-in function.</para>
        /// <para>Returns the severity of the error that caused the CATCH block of a TRY…CATCH construct to be run.</para>
        /// </summary>
        public static SysFn ErrorSeverity()
        {
            return new SysFn("ERROR_SEVERITY()");
        }

        /// <summary>
        /// <para>ERROR_STATE built-in function.</para>
        /// <para>Returns the state number of the error that caused the CATCH block of a TRY…CATCH construct to be run.</para>
        /// </summary>
        public static SysFn ErrorState()
        {
            return new SysFn("ERROR_STATE()");
        }
    }
}
