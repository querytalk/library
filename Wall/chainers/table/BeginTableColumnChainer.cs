#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginTableColumnChainer : Chainer, IStatement,
        IBeginTableColumn,
        IBeginTablePrimaryKey,
        IBeginTableUniqueKey,
        IEndTable
    {
        internal override string Method
        {
            get
            {
                return Text.Method.TableColumn;
            }
        }

        internal Nullable<bool> IsNullable { get; set; }    

        internal TableIdentity Identity { get; set; }      

        internal Expression Check { get; set; }           

        internal Value Default { get; set; }      

        internal BeginTableColumnChainer(Chainer prev, NonSelectColumnArgument column, DataType dataTypeDef)
            : base(prev)
        {
            Initialize(column, dataTypeDef);
        }

        internal BeginTableColumnChainer(Chainer prev, NonSelectColumnArgument column, Type type)
            : base(prev)
        {
            Type clrType;
            var typeMatch = Mapping.CheckClrCompliance(type, out clrType, out chainException);
            if (typeMatch != Mapping.ClrTypeMatch.ClrMatch)
            {
                TryThrow();
            }

            var dataTypeDef = Mapping.ClrMapping[clrType].DefaultDataType;
            Initialize(column, dataTypeDef);
        }

        private void Initialize(NonSelectColumnArgument column, DataType dataTypeDef)
        {
            IsNullable = null;
            CheckNullAndThrow(Arg(() => column, column));
            TryThrow(dataTypeDef.Exception);
            Prev.GetPrev<BeginTableChainer>().SetHasColumns();

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(300);
                sql.AppendLine();
                sql.Append(Text.TwoSpaces);

                if (!(Prev is BeginTableChainer && !((BeginTableChainer)Prev).IsDesignedByType))
                {
                    sql.Append(Text.Comma);
                }

                sql.Append(column.Build(buildContext, buildArgs)).S()
                    .Append(dataTypeDef.Build());

                TryThrow(column.Exception);

                if (Identity != null)
                {
                    sql.S().Append(Text.Identity)
                        .EncloseLeft()
                        .Append(Identity.Seed)
                        .AppendComma()
                        .Append(Identity.Increment)
                        .EncloseRight();
                }

                if (IsNullable == true)
                {
                    sql.S().Append(Text.Null);
                }
                else if (IsNullable == false)
                {
                    sql.S().Append(Text.NotNull);
                }

                if (Check != null)
                {
                    sql.S().Append(Text.Check)
                        .EncloseLeft().Append(Check.Build(buildContext, buildArgs)).EncloseRight();
                    TryThrow(buildContext);
                }

                if (Default != null)
                {
                    sql.S().Append(Text.Default)
                        .EncloseLeft().Append(Default.Build(buildContext, buildArgs)).EncloseRight();
                    TryThrow(buildContext);
                }

                return sql.ToString();
            };
        }

    }
}
