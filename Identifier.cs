#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a SQL identifier. (This class has no public constructor. Use create method instead.)
    /// </summary>
    public sealed class Identifier : Chainer, INonPredecessor, IColumnName,
        IScalar,
        IAs
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Identifier;
            }
        }

        private string _sql;
        internal string Sql
        {
            get
            {
                return _sql;
            }
        }

        internal string Part1 { get; private set; }

        internal string Part2 { get; private set; }

        internal string Part3 { get; private set; }

        internal string Part4 { get; private set; }

        private int _numberOfParts;
        internal int NumberOfParts
        {
            get
            {
                return _numberOfParts;
            }
        }

        #region IColumnName

        string IColumnName.ColumnName
        {
            get
            {
                return Part4 ?? (Part3 ?? (Part2 ?? Part1));
            }
        }

        #endregion

        internal Identifier(string part1, bool splitByDot)
            : base(null)
        {
            CheckNullAndThrow(Arg(() => part1, part1));

            Part1 = part1;
            _numberOfParts = 1;

            if (splitByDot)
            {
                _sql = Filter.DelimitColumnMultiPart(part1, out chainException);
                TryThrow();
            }
            else
            {
                _sql = Filter.DelimitNonAsterix(part1);
            }

            Build = (buildContext, buildArgs) =>
            {
                return _sql;
            };
        }

        internal Identifier(string part1, string part2)
            : base(null)
        {
            CheckNullAndThrow(Arg(() => part1, part1));
            CheckNullAndThrow(Arg(() => part2, part2));

            Part1 = part1;
            Part2 = part2;
            _numberOfParts = 2;

            _sql = Text.GenerateSql(20)
                .Append(Filter.DelimitNonAsterix(part1))
                .AppendDot()
                .Append(Filter.DelimitNonAsterix(part2))
                .ToString();

            Build = (buildContext, buildArgs) =>
            {
                return _sql;
            };
        }

        internal Identifier(string part1, string part2, string part3)
            : this(part1, part2)
        {
            CheckNullAndThrow(Arg(() => part1, part1));
            CheckNullAndThrow(Arg(() => part2, part2));
            CheckNullAndThrow(Arg(() => part3, part3));

            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
            _numberOfParts = 3;

            _sql = Text.GenerateSql(30)
                .Append(Filter.DelimitNonAsterix(part1))
                .AppendDot()
                .Append(Filter.DelimitNonAsterix(part2))
                .AppendDot()
                .Append(Filter.DelimitNonAsterix(part3))
                .ToString();

            Build = (buildContext, buildArgs) =>
            {
                return _sql;
            };
        }

        internal Identifier(string part1, string part2, string part3, string part4)
            : this(part1, part2, part3)
        {
            CheckNullAndThrow(Arg(() => part1, part1));
            CheckNullAndThrow(Arg(() => part2, part2));
            CheckNullAndThrow(Arg(() => part3, part3));
            CheckNullAndThrow(Arg(() => part4, part4));

            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
            Part4 = part4;
            _numberOfParts = 4;

            _sql = Text.GenerateSql(40)
                .Append(Filter.DelimitNonAsterix(part1))
                .AppendDot()
                .Append(Filter.DelimitNonAsterix(part2))
                .AppendDot()
                .Append(Filter.DelimitNonAsterix(part3))
                .AppendDot()
                .Append(Filter.DelimitNonAsterix(part4))
                .ToString();

            Build = (buildContext, buildArgs) =>
            {
                return _sql;
            };
        }

        #region Asterisk (ALL columns)

        private Identifier()
            : base(null)
        {
            Part1 = Text.Asterisk;
            _sql = Part1;

            Build = (buildContext, buildArgs) =>
            {
                return _sql;
            };
        }

        internal static Identifier GetAsterisk()
        {
            return new Identifier();
        }

        #endregion

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return _sql;
        }
    }
}
