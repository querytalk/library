#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        private static bool ParameterizationCriteria(BuildContext buildContext, Parameterization p = Parameterization.None)
        {
            if (buildContext == null)
            {
                return false;
            }

            if (p == Parameterization.Param)
            {
                return true;
            }

            var root = buildContext.Root;

            return
                Admin.IsValueParameterizationOn                             
                    &&
                (root.CompilableType.IsProcOrSnipOrMapper() || 
                    (root.CompilableType == Compilable.ObjectType.View &&
                        (buildContext.ParamRoot.CompilableType.IsProcOrSnipOrMapper()))
                )
                    &&
                // do not parameterize non-parameterizable arguments (DeclareChainer, SetChainer, RaiserrorChainer)
                !(buildContext.Current is INonParameterizable);
        }

        // create the parameter from the inline value argument
        private static string Parameterize(ParameterArgument argument, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            QueryTalkException exception;
            var root = buildContext.ConcatRoot;

            // infer param from argument
            var param = Variable.InferParam(root, argument, out exception);

            TryThrow(exception);
            root.TryAddParamOrThrow(param, true);      
            argument.BuildArgument(root, param.Name);
            TryThrow(argument.Exception);

            if (p == Parameterization.Value)
            {
                param.SetParameterizedValue(argument);     
            }

            return param.Name;
        }

        internal static string Parameterize(this System.Boolean value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Byte value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Byte[] value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.DateTime value, BuildContext buildContext, DataType dataType, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(new Value(value, p, dataType));
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.DateTimeOffset value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Decimal value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Double value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Guid value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Int16 value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Int32 value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Int64 value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Single value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.TimeSpan value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(value);
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.String value, BuildContext buildContext, DataType dataType, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(new Value(value, p, dataType));
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Object value, BuildContext buildContext, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(new Value(value));
            return Parameterize(argument, buildContext, p);
        }

        internal static string Parameterize(this System.Object value, BuildContext buildContext, DataType dataType, Parameterization p = Parameterization.Value)
        {
            if (!ParameterizationCriteria(buildContext, p))
            {
                return null;
            }

            var argument = new ParameterArgument(new Value(value, dataType));
            return Parameterize(argument, buildContext, p);
        }

        // for functions only
        internal static string Parameterize(this View view, BuildContext buildContext)
        {
            var argument = new ParameterArgument(view);
            return Parameterize(argument, buildContext, Parameterization.Value);
        }

    }
}
