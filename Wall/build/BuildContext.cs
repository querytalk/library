#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class BuildContext
    {

        #region Properties

        // A creator is a compilable object that creates the instance of a BuildContext.
        private Compilable _creator;

        // Current node of the compilable object that is being built.
        internal Chainer Current { get; set; }

        // If given then this value prevails in the IsCurrentStringAsValue getter.
        private Nullable<bool> _isCurrentStringAsValue;
        internal void ResetCurrentStringAsValueFlag()
        {
            _isCurrentStringAsValue = null;
        }

        internal bool IsCurrentStringAsValue
        {
            get
            {
                if (Current == null)
                {
                    return false;
                }

                if (_isCurrentStringAsValue != null)
                {
                    return (bool)_isCurrentStringAsValue;
                }

                if (Current is IStringAsValue || Current.UseStringAsValue)
                {
                    return true;
                }

                return false;
            }
            set
            {
                _isCurrentStringAsValue = value;
            }
        }

        internal string BuildString(string arg)
        {
            if (Variable.Detect(arg))
            {
                return arg;
            }
            else
            {      
                return arg.Parameterize(this) ?? Mapping.BuildCast(arg);
            }
        }

        internal Query Query
        {
            get
            {
                if (Current != null && Current.IsQuery)
                {
                    return Current.Query;
                }
                else
                {
                    return null;
                }
            }
        }

        // attention: 
        //   Use it only where the Master is defined (in query statements). No Master check is done.
        internal Query Master
        {
            get
            {
                return Current.Query.Master;
            }
        }

        // Root of the compilable object that is currently being compiled or the root of the caller,
        // if the object is argument (also an inlined procedure of an .Exec that has to be compiled with delay).
        internal Designer Root 
        {
            get
            {
                return Current.TryGetRoot() ?? _creator.GetRoot();
            }
        }

        // If the param root is different than the root of the compilable object,
        // then ParamRoot will be used for storing the parameters and variables.
        private Designer _paramRoot;
        internal Designer ParamRoot 
        {
            get
            {
                return _paramRoot ?? Root;
            }
            set
            {
                _paramRoot = value;
            }
        }

        internal Designer ConcatRoot
        {
            get
            {
                // ConcatRoot (of the Master) is the priority root
                if (Current.IsQuery && Master.IsConcatenated)
                {
                    return Master.ConcatRoot;
                }
                // then ParamRoot
                // and lastly the regular Root
                else
                {
                    return _paramRoot ?? Root;
                }
            }
        }

        internal void TryAddParamToConcatRoot(Variable param)
        {
            if (param != null                   // null check since the extension methods are used on variable object
                && Current.IsQuery              // statement must be a Query
                && Master.IsConcatenated)       // master query must be a concatenated query
            {
                if (Master.IsConcatenated)
                {
                    if (Master.ConcatRoot.ParamOrVariableExists(param.Name) == false)
                    {
                        Master.ConcatRoot.TryAddParamOrThrow(param, true);  
                    }
                }
            }
        }

        private QueryTalkException _exception;
        internal QueryTalkException Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                if (_exception == null && value != null)
                {
                    _exception = value;
                }
            }
        }

        #endregion

        #region Constructors

        // regular ctor
        internal BuildContext(Compilable creator, Designer paramRoot = null)
        {
            _creator = creator;
            _paramRoot = paramRoot;
        }

        // ctor for mapper
        internal BuildContext(Mapper creator)
        {
            _creator = creator;
            Current = creator;  // mapper is THE ONLY chain object in Navigation Querying
        }

        internal static BuildContext CreateAdHoc()
        {
            return new BuildContext(new Mapper(new InternalRoot()));
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("BuildContext ({0}/{1})", Root.Name, ParamRoot.Name);
        }

        #endregion

        #region TryGetVariable

        // try get variable from build context
        // Note:
        //   The snippet object must be handled on different root (ParamRoot) than the procedure
        //   since the params are not declared on the snippet's root but rather on the master proc's root.
        internal Variable TryGetVariable(
            string variableName,
            out QueryTalkException exception,
            Variable.SearchType searchType = Variable.SearchType.Any)
        {
            if (Root.CompilableType.IsViewOrSnippet())
            {
                return ParamRoot.TryGetVariable(variableName, out exception, searchType);
            }
            else
            {
                return Root.TryGetVariable(variableName, out exception, searchType);
            }
        }

        #endregion

    }
}
