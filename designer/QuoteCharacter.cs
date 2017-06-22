#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// Quote character used as an argument of a .Quotename method.
        /// </summary>
        public enum QuoteCharacter : int
        {
            /// <summary>
            /// Quotation is enclosed by square brackets. 
            /// </summary>
            Brackets = 0,

            /// <summary>
            /// Quotation is enclosed by single quotation mark.
            /// </summary>
            SingleQuotationMark = 1,

            /// <summary>
            /// Quotation is enclosed by double quotation mark.
            /// </summary>
            DoubleQuotationMark = 2
        }
    }
}