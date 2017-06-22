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
        /// Datepart of a date.
        /// </summary>
        public enum PartOfDate : int
        {
            /// <summary>
            /// A year.
            /// </summary>
            Year = 0,
            /// <summary>
            /// A quarter of a year.
            /// </summary>
            Quarter = 1,
            /// <summary>
            /// A month.
            /// </summary>
            Month = 2,
            /// <summary>
            /// A day of a year.
            /// </summary>
            DayOfYear = 3,
            /// <summary>
            /// A day.
            /// </summary>
            Day = 4,
            /// <summary>
            /// A week.
            /// </summary>
            Week = 5,
            /// <summary>
            /// A day in a week.
            /// </summary>
            Weekday = 6,
            /// <summary>
            /// An hour.
            /// </summary>
            Hour = 7,
            /// <summary>
            /// A minute.
            /// </summary>
            Minute = 8,
            /// <summary>
            /// A second.
            /// </summary>
            Second = 9,
            /// <summary>
            /// A milisecond.
            /// </summary>
            Milisecond = 10,
            /// <summary>
            /// A microsecond.
            /// </summary>
            Microsecond = 11,
            /// <summary>
            /// A nanosecond.
            /// </summary>
            Nanosecond = 12
        }
    }
}