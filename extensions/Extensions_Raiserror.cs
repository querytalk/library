#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Generates an error message and initiates error processing for the session. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="message">Is a user-defined message.</param>
        /// <param name="severity">
        /// Is the user-defined severity level associated with this message.
        /// </param>
        /// <param name="state">
        /// Is an integer from 0 through 255.
        /// </param>
        public static RaiserrorChainer Raiserror(this IAny prev, string message, int severity, byte state)
        {
            return new RaiserrorChainer((Chainer)prev, message, severity, state);
        }
    }
}
