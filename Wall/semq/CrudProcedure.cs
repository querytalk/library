#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;

namespace QueryTalk.Wall
{
    internal class CrudProcedure
    {
        internal DB3 NodeID { get; private set; }
        internal CrudProcedureType ProcedureType { get; private set; }
        internal Procedure Procedure { get; private set; }

        internal CrudProcedure(DB3 nodeID, CrudProcedureType procedureType, Procedure procedure)
        {
            NodeID = nodeID;
            ProcedureType = procedureType;
            Procedure = procedure;
        }

        internal bool Equals(CrudProcedure other)
        {
            return NodeID.Equals(other.NodeID) && ProcedureType == other.ProcedureType;
        }

        internal bool Equals(DB3 nodeID, CrudProcedureType procedureType)
        {
            return NodeID.Equals(nodeID) && ProcedureType == procedureType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(CrudProcedure))
            {
                return false;
            }

            return Equals((CrudProcedure)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + NodeID.GetHashCode();
                hash = hash * 23 + ProcedureType.GetHashCode();
                return hash;
            }
        }
    }

    internal class CrudProcedureEqualityComparer : IEqualityComparer<CrudProcedure>
    {
        public bool Equals(CrudProcedure o1, CrudProcedure o2)
        {
            if (o1 == null || o2 == null)
            {
                return false;
            }

            return (o1.NodeID.Equals(o2.NodeID) && o1.ProcedureType == o2.ProcedureType);
        }

        public int GetHashCode(CrudProcedure o)
        {
            if (o == null)
            {
                return 0;
            }

            return o.GetHashCode();
        }
    }

}
