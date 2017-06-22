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
    public class DeclareChainer : EndChainer, IBegin, INonParameterizable,
        IDeclareSet
    {
        internal override string Method 
        { 
            get 
            { 
                return Text.Method.Declare; 
            } 
        }

        private string _variableName;
        internal string VariableName
        {
            get
            {
                return _variableName;
            }
        }

        internal DeclareChainer(Chainer prev, string variableName, DataType dbt)
            : base(prev)
        {
            _variableName = variableName;
            Initialize(dbt);
        }

        internal DeclareChainer(Chainer prev, string variableName, DT dt, TableArgument udt)
            : base(prev)
        {
            _variableName = variableName;
            Initialize(dt, udt);
        }

        internal DeclareChainer(Chainer prev, string variableName, Type type)
            : base(prev)
        {
            _variableName = variableName;

            Type clrType;
            var typeMatch = Mapping.CheckClrCompliance(type, out clrType, out chainException);
            if (typeMatch != Mapping.ClrTypeMatch.ClrMatch)
            {
                TryThrow();
            }

            var dataTypeDef = Mapping.ClrMapping[clrType].DefaultDataType;
            Initialize(dataTypeDef);
        }

        internal DeclareChainer(Chainer prev, string variableName, DeclareArgument value)
            : base(prev)
        {
            if (Value.IsNull(value) || Value.IsNull(value.Original))
            {
                Throw(QueryTalkExceptionType.ArgumentNull,
                    String.Format("variable = {0}{1}   value = null/NULL", variableName, Environment.NewLine),
                    Text.Method.Declare);
            }

            _variableName = variableName;
            Initialize(value);
        }

        private void Initialize(DataType dataType)
        {
            var root = GetRoot();
            chainMethod = Text.Method.Declare;  

            CheckAndThrow();
            TryThrow(dataType.Exception);

            var variable = new Variable(0, _variableName, dataType, IdentifierType.SqlVariable); 
            root.TryAddVariableOrThrow(variable, chainMethod, false);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(30)
                    .NewLine(Text.Declare).S()
                    .Append(_variableName).S()
                    .Append(Text.As).S()
                    .Append(dataType.Build());
                sql.Terminate();

                return sql.ToString();
            };
        }

        // initalize by value
        private void Initialize(DeclareArgument value)
        {
            var root = GetRoot();
            chainMethod = Text.Method.Declare;  

            CheckAndThrow();

            var dataType = Mapping.ClrMapping[value.ArgType].DefaultDataType;
            var variable = new Variable(0, _variableName, dataType, IdentifierType.SqlVariable); 
            root.TryAddVariableOrThrow(variable, chainMethod, false);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(50)
                    .NewLine(Text.Declare).S()
                    .Append(_variableName).S()
                    .Append(Text.As).S()
                    .Append(dataType.Build())
                    .Terminate();

                sql.NewLine(Text.Set).S()
                    .Append(_variableName)
                    .Append(Text._Equal_)
                    .Append(value.Build(buildContext, buildArgs));

                sql.Terminate();

                return sql.ToString();
            };
        }

        private void Initialize(DT dt, TableArgument sysObject)
        {
            var root = GetRoot();
            chainMethod = Text.Method.Declare;  

            CheckAndThrow();
            CheckNullAndThrow(Arg(() => sysObject, sysObject));
            TryThrow(sysObject.Exception);

            var param = new Variable(0, _variableName, dt, sysObject, IdentifierType.SqlVariable); 
            root.TryAddVariableOrThrow(param, chainMethod, false);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(30)
                    .NewLine(Text.Declare).S()
                    .Append(_variableName).S()
                    .Append(Text.As).S()
                    .Append(sysObject.Build(buildContext, buildArgs))
                    .Terminate();

                TryThrow(buildContext);

                return sql.ToString();
            };
        }

        // check the variable name validity
        private void CheckAndThrow()
        {
            CheckNullAndThrow(Arg(() => _variableName, _variableName));

            bool check = Variable.TryValidateName(_variableName, out chainException);
            TryThrow();

            if (!check)
            {
                Throw(QueryTalkExceptionType.InvalidVariableName, ArgVal(() => _variableName, _variableName));
            }
        }

    }
}
