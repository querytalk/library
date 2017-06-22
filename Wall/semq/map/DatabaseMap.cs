#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// The mapping data of a database. (This class is intended to be used by the mapper application.)
    /// </summary>
    public sealed class DatabaseMap : Map
    {
        /// <summary>
        /// A name of a database.
        /// </summary>
        public Identifier Name { get; private set; }

        /// <summary>
        /// The creation date of a mapping assembly.
        /// </summary>
        public DateTime MappingDate { get; private set; }

        /// <summary>
        /// A flag to indicate whether all database objects have been mapped.
        /// </summary>
        public bool IsWhole { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DatabaseMap class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mappingDate"></param>
        /// <param name="isWhole"></param>
        public DatabaseMap(Identifier name, DateTime mappingDate, bool isWhole)
            : base(DB3.Get(DbMapping.NewDatabaseID))
        {
            Name = name;
            MappingDate = mappingDate;
            IsWhole = isWhole;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("DatabaseID={0};Name={1};MappingDate={2};IsWhole={3}", ID, Name, MappingDate, IsWhole);
        }
    }
}
