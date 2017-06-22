#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// The mapping data of a parameter. (This class is intended to be used by the mapper application.)
    /// </summary>
    public sealed class ParamMap : Map
    {
        /// <summary>
        /// A name of a parameter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A data type.
        /// </summary>
        public DataType DataType { get; private set; }

        /// <summary>
        /// A flag to indicate whether a parameter is an output parameter.
        /// </summary>
        public bool IsOutput { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ParamMap class.
        /// </summary>
        /// <param name="id">A DB3 identifier of a parameter.</param>
        /// <param name="name">A name of a parameter.</param>
        /// <param name="dataType">A data type.</param>
        /// <param name="isOutput">A flag to indicate whether a parameter is an output parameter.</param>
        public ParamMap(DB3 id, string name, DataType dataType, bool isOutput = false)
            : base(id)
        {
            Name = name;
            DataType = dataType;
            IsOutput = isOutput;
        }
    }
}
