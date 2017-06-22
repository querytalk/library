#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data;
using System.Reflection;

namespace QueryTalk.Wall
{
    internal static partial class Reader
    {

        #region GetConnectable

        internal static Connectable GetConnectable(Assembly client, Chainer prev, ConnectionData connectionData)
        {
            if (prev is Connectable)
            {
                return (Connectable)prev;
            }

            Connectable connectable;
            Compilable compilable = null;
            Executable executable = null;

            if (prev is Compilable)
            {
                compilable = (Compilable)prev;
                executable = new Executable(compilable);
            }
            else if (prev is IExecutable)
            {
                executable = ((Chainer)prev).Executable;
            }
            else if (prev is Connectable)
            {
                connectable = (Connectable)prev;
                connectable.SetTimeout(connectionData.CommandTimeout);
            }
            // root:
            else
            {
                compilable = new Procedure(prev);
                executable = new Executable(compilable);
            }

            connectable = new Connectable(client, executable, connectionData);
            return connectable;
        }

        internal static Connectable GetConnectable(Assembly client, Chainer prev)
        {
            if (prev is Connectable)
            {
                return (Connectable)prev;
            }

            return GetConnectable(client, prev,
                ConnectionManager.GetConnectionData(client, null, null, prev));
        }

        internal static Connectable GetConnectable(Assembly client, DbNode node, Chainer prev)
        {
            if (prev is Connectable)
            {
                return (Connectable)prev;
            }

            return GetConnectable(client, prev, 
                ConnectionManager.GetConnectionData(client, null, node, prev));
        }

        internal static Connectable GetConnectable(Assembly client, Chainer prev, ConnectBy connectBy)
        {
            if (prev is Connectable)
            {
                return (Connectable)prev;
            }

            return GetConnectable(client, prev,
                ConnectionManager.GetConnectionData(client, connectBy, null, prev));
        }

        internal static Connectable GetConnectable(Assembly client, DbRow row, Chainer prev, ConnectBy connectBy)
        {
            if (prev is Connectable)
            {
                return (Connectable)prev;
            }

            if (connectBy == null)
            {
                connectBy = row.GetConnectBy();
            }

            return GetConnectable(client, prev,
                ConnectionManager.GetConnectionData(client, connectBy, null, prev));
        }

        #endregion

        #region Output row

        private static int ReadOutputData(IDataReader reader, Connectable connectable)
        {
            object output;
            int returnValue = 0;

            try
            {
                bool found = false;
                do
                {
                    while (reader.Read())
                    {
                        if (reader.GetName(0) == Text.Reserved.ReturnValueColumnName)
                        {
                            found = true;
                            break;  // last datasource table always exists
                        }
                    }

                } while (!found && reader.NextResult());

                if (found)
                {
                    int count = reader.FieldCount;
                    var outputArguments =
                        ParameterArgument.GetOutputArguments(connectable.Executable.Arguments);

                    for (int i = 0; i < count; ++i)
                    {
                        if (reader.IsDBNull(i))
                        {
                            output = null;  
                        }
                        else
                        {
                            output = reader.GetValue(i);
                        }

                        // output values:
                        if (i >= 1)
                        {
                            outputArguments[i - 1].SetOutput(output);
                        }
                        // return value:
                        else
                        {
                            if (output != null && output.GetType() == typeof(System.Int32))
                            {
                                int.TryParse(output.ToString(), out returnValue);
                                connectable.ReturnValue = returnValue; 
                            }
                        }
                    }
                    connectable.OutputArguments = outputArguments; 
                }

                return returnValue;
            }
            catch (System.Exception ex)
            {
                throw ClrException(ex, "ReaderOutputData", connectable, null);
            }
        }

        private static int ReadOutputDataOnFound(IDataReader reader, Connectable connectable)
        {
            Value[] values = Common.DefaultReturnValue;

            // double check
            if (!reader.Read())
            {
                return 0;
            }

            try
            {
                int count = reader.FieldCount;
                values = new Value[count];
                var outputArguments =
                    ParameterArgument.GetOutputArguments(connectable.Executable.Arguments);

                for (int i = 0; i < count; ++i)
                {
                    Value outputData = new Value(reader.GetValue(i));
                    values[i] = outputData;

                    // output values:
                    if (i >= 1)
                    {
                        outputArguments[i - 1].SetOutput(outputData.Original);
                    }
                }

                connectable.OutputArguments = outputArguments;  

                int returnValue = 0;
                if (values[0] != null)   
                {
                    int retval;
                    if (int.TryParse(values[0].ToString(), out retval))
                    {
                        returnValue = retval;
                    }
                    else
                    {
                        ThrowQueryTalkReservedNameException();
                    }
                }
                connectable.ReturnValue = returnValue;
                return returnValue;
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw ClrException(ex, "ReadOutputDataOnFound", connectable, null);
            }
        }

        #endregion

        #region Exceptions

        private static QueryTalkException TypeMismatchException(
            string objectName,
            string method,
            Type tableType,
            Row<string, string, string> args,
            int tableIndex = 0)
        {
            return new QueryTalkException("Reader.TypeMismatchException",
                QueryTalkExceptionType.ColumnTypeMismatch,
                objectName,
                method,
                String.Format("table = {0}{1}   table index = {2}{3}   column = {4}{5}   invalid type = {6}{7}   source db type = {8}",
                    tableType,
                    Environment.NewLine,
                    tableIndex + 1,
                    Environment.NewLine,
                    args.Column1 ?? Text.NotAvailable,
                    Environment.NewLine,
                    args.Column2 ?? Text.NotAvailable,
                    Environment.NewLine,
                    args.Column3 != null ? args.Column3.ToLower() : Text.NotAvailable));
        }

        private static QueryTalkException NoMoreResultsetException<T>(Connectable connectable)
        {
            return new QueryTalkException(
                "Reader.NoMoreResultsetException",
                QueryTalkExceptionType.NoMoreResultset,
                ((IName)connectable.Executable).Name,
                Text.Method.Go,
                String.Format("superfluous table = {0}", typeof(T)));
        }

        internal static QueryTalkException ClrException(
            System.Exception ex,
            string readerName,
            Connectable connectable,
            string method,
            string arguments = null)
        {
            QueryTalkException exception = new QueryTalkException(String.Format("{0}.{1}", Text.Free.Reader, readerName),
                QueryTalkExceptionType.ClrException, ((IName)connectable).Name,
                method, arguments);
            exception.ClrException = ex;
            return exception;
        }

        internal static void ThrowOperationCancelledByUserException<T>(Connectable connectable, int rowix)
        {
            throw new QueryTalkException(
                "Reader.ThrowOperationCancelledByUserException",
                QueryTalkExceptionType.OperationCanceledByUser,
                ((IName)connectable.Executable).Name,
                null,
                String.Format("terminated table = {0}{1}   at row = {2}",
                    typeof(T), Environment.NewLine, rowix));
        }

        internal static void ThrowQueryTalkReservedNameException()
        {
            throw new QueryTalkException("Loader", QueryTalkExceptionType.QueryTalkReservedName,
                String.Format("name = {0}", Text.Reserved.ReturnValueColumnName), null)
                    .SetExtra("A reserved name was used as a column name.");
        }

        #endregion
    }
}
