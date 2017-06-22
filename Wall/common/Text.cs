#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace QueryTalk.Wall
{
    internal static class Text
    {

        #region General

        internal const string ClrNull = "null";
        internal const string DbNull = "NULL";
        internal const string Terminator = ";";
        internal const char TerminatorChar = ';';
        internal const string Comma = ",";
        internal const string CommaWithSpace = ", ";
        internal const string Dot = ".";
        internal const string ThreeDots = "...";
        internal const char DotChar = '.';
        internal const string Asterisk = "*";
        internal const string SemiColon = ":";
        internal const string LeftBracket = "(";
        internal const string RightBracket = ")";
        internal const string EmptyBrackets = "()";
        internal const string LeftSquareBracket = "[";
        internal const string RightSquareBracket = "]";
        internal const string SingleQuote = "'";
        internal const char Space = ' ';
        internal const string OneSpace = " ";
        internal const string TwoSpaces = "  ";
        internal const string TwoHyphens = "--";
        internal const string ZeroAlias = "0";
        internal const string FirstAlias = "1";
        internal const string TargetAlias = "T";
        internal const string SourceAlias = "S";
        internal const string DelimitedTargetAlias = "[T]";
        internal const string DelimitedTargetAlias2 = "[T2]";
        internal const string DelimitedSourceAlias = "[S]";
        internal const string PivotAlias = "P";
        internal const string DelimitedPivotAlias = "[P]";
        internal const string DelimitedFirstAlias = "[1]";
        internal const string Column = "Column";
        internal const string ColumnShortName = "C";
        internal const string SingleColumnName = "Column1";
        internal const string SingleColumnShortName = "C1";
        internal const string Equal = "=";
        internal const string _Equal_ = " = ";
        internal const string Underscore = "_";
        internal const string N = "N";
        internal const string NotAvailable = "N/A";
        internal const string Unknown = "Unknown";
        internal const string Zero = "0";
        internal const string BinaryPrefix = "0x";
        internal const string _Plus_ = " + ";
        internal const string OPrefix = "o";
        internal const string Count = "Count";

        #endregion

        #region Syntax

        internal const string _And_                           = " AND ";
        internal const string Abs                             = "ABS";
        internal const string And                             = "AND";
        internal const string Any                             = "ANY";
        internal const string As                              = "AS";
        internal const string _As_                            = " AS ";
        internal const string Asc                             = "ASC";
        internal const string Begin                           = "BEGIN";
        internal const string BeginCatch                      = "BEGIN CATCH";
        internal const string BeginTransaction                = "BEGIN TRANSACTION";
        internal const string BeginTry                        = "BEGIN TRY";
        internal const string Between                         = "BETWEEN";
        internal const string Break                           = "BREAK";
        internal const string Case                            = "CASE";
        internal const string Cast                            = "CAST";
        internal const string Check                           = "CHECK";
        internal const string Close                           = "CLOSE";
        internal const string Clustered                       = "CLUSTERED";
        internal const string ClusteredUnique                 = "UNIQUE CLUSTERED";
        internal const string Coalesce                        = "COALESCE";
        internal const string Collate                         = "COLLATE";
        internal const string CollateDefault                  = "DATABASE_DEFAULT";
        internal const string CommitTransaction               = "COMMIT TRANSACTION";
        internal const string Continue                        = "CONTINUE";
        internal const string Convert                         = "CONVERT";
        internal const string Create                          = "CREATE";
        internal const string CreateTable                     = "CREATE TABLE";
        internal const string CrossApply                      = "CROSS APPLY";
        internal const string CrossJoin                       = "CROSS JOIN";
        internal const string CurrentTimestamp                = "CURRENT_TIMESTAMP";
        internal const string CurrentUser                     = "CURRENT_USER";
        internal const string CursorLocalFastForward          = "CURSOR LOCAL FAST_FORWARD";
        internal const string CursorFor                       = "CURSOR FOR";
        internal const string Deallocate                      = "DEALLOCATE";
        internal const string Declare                         = "DECLARE";
        internal const string Default                         = "DEFAULT";
        internal const string DefaultValues                   = "DEFAULT VALUES";
        internal const string Delete                          = "DELETE";
        internal const string DeleteFrom                      = "DELETE FROM";
        internal const string DenseRankOver                   = "DENSE_RANK() OVER (";
        internal const string Desc                            = "DESC";
        internal const string DropIndex                       = "DROP INDEX";
        internal const string DropTempTable                   = "DROP TABLE";
        internal const string Distinct                        = "DISTINCT";          
        internal const string Else                            = "ELSE";
        internal const string ElseIf                          = "ELSE IF";
        internal const string ElseBegin                       = "ELSE BEGIN";
        internal const string End                             = "END";
        internal const string EndCatch                        = "END CATCH";
        internal const string EndTry                          = "END TRY";
        internal const string Escape                          = "ESCAPE";
        internal const string Except                          = "EXCEPT";
        internal const string Exec                            = "EXEC";
        internal const string ExecuteAsUser                   = "EXECUTE AS USER";
        internal const string Exists                          = "EXISTS";
        internal const string FetchNext                       = "FETCH NEXT";
        internal const string For                             = "FOR";
        internal const string From                            = "FROM";
        internal const string FullJoin                        = "FULL JOIN";
        internal const string Goto                            = "GOTO";
        internal const string GroupBy                         = "GROUP BY";
        internal const string Having                          = "HAVING";
        internal const string Identity                        = "IDENTITY";
        internal const string If                              = "IF";
        internal const string In                              = "IN";
        internal const string Index                           = "INDEX";
        internal const string InnerJoin                       = "INNER JOIN";
        internal const string Insert                          = "INSERT";
        internal const string InsertInto                      = "INSERT INTO";
        internal const string Intersect                       = "INTERSECT";
        internal const string Into                            = "INTO";
        internal const string LeftOuterJoin                   = "LEFT OUTER JOIN";
        internal const string Like                            = "LIKE";
        internal const string RightOuterJoin                  = "RIGHT OUTER JOIN";
        internal const string Merge                           = "MERGE";
        internal const string Null                            = "NULL";
        internal const string Nonclustered                    = "NONCLUSTERED";
        internal const string NonclusteredUnique              = "UNIQUE NONCLUSTERED";
        internal const string Not                             = "NOT";
        internal const string Not_                            = "NOT ";
        internal const string NotNull                         = "NOT NULL";
        internal const string NotExists                       = "NOT EXISTS";
        internal const string IsNull                          = "IS NULL";
        internal const string IsNotNull                       = "IS NOT NULL";
        internal const string NTile                           = "NTILE";
        internal const string On                              = "ON";
        internal const string OnWithIndent                    = "  ON";
        internal const string Off                             = "OFF";
        internal const string Or                              = "OR";
        internal const string OrderBy                         = "ORDER BY";
        internal const string Open                            = "OPEN";
        internal const string Output                          = "OUTPUT";
        internal const string Over                            = "OVER";
        internal const string OuterApply                      = "OUTER APPLY";
        internal const string PartitionBy                     = "PARTITION BY";
        internal const string Percent                         = "PERCENT";
        internal const string PrimaryKey                      = "PRIMARY KEY";
        internal const string PrimaryKeyNonclustered          = "PRIMARY KEY NONCLUSTERED";
        internal const string Pivot                           = "PIVOT";
        internal const string RankOver                        = "RANK() OVER (";
        internal const string Raiserror                       = "RAISERROR";
        internal const string ReadCommitted                   = "READ COMMITTED";
        internal const string ReadUncommitted                 = "READ UNCOMMITTED";
        internal const string RepeatableRead                  = "REPEATABLE READ";
        internal const string RowNumberOver                   = "ROW_NUMBER() OVER (";
        internal const string Serializable                    = "SERIALIZABLE";
        internal const string Snapshot                        = "SNAPSHOT";
        internal const string Readonly                        = "READONLY";
        internal const string RollbackTransaction             = "ROLLBACK TRANSACTION";
        internal const string Return                          = "RETURN";
        internal const string Revert                          = "REVERT";
        internal const string SaveTransaction                 = "SAVE TRANSACTION";
        internal const string Select                          = "SELECT";
        internal const string SelectDistinct                  = "SELECT DISTINCT";
        internal const string Set                             = "SET";
        internal const string SetIdentityInsert               = "SET IDENTITY_INSERT";
        internal const string SetTransactionIsolationLevel    = "SET TRANSACTION ISOLATION LEVEL";
        internal const string Table                           = "TABLE";
        internal const string TruncateTable                   = "TRUNCATE TABLE";
        internal const string Then                            = "THEN";
        internal const string ThenDelete                      = "THEN DELETE";
        internal const string ThenInsert                      = "THEN INSERT";
        internal const string ThenUpdateSet                   = "THEN UPDATE SET";
        internal const string Top                             = "TOP";
        internal const string TopZero                         = "TOP(0)";
        internal const string TopOne                          = "TOP(1)";
        internal const string Unique                          = "UNIQUE";
        internal const string Union                           = "UNION";
        internal const string UnionAll                        = "UNION ALL";
        internal const string Unpivot                         = "UNPIVOT";
        internal const string Update                          = "UPDATE";
        internal const string User                            = "USER";
        internal const string Using                           = "USING";
        internal const string Values                          = "VALUES";
        internal const string WaitforDelay                    = "WAITFOR DELAY";
        internal const string WhenMatched                     = "WHEN MATCHED";
        internal const string WhenMatchedAnd                  = "WHEN MATCHED AND";
        internal const string WhenNotMatched                  = "WHEN NOT MATCHED";
        internal const string WhenNotMatchedAnd               = "WHEN NOT MATCHED AND";
        internal const string WhenNotMatchedBySource          = "WHEN NOT MATCHED BY SOURCE";
        internal const string WhenNotMatchedBySourceAnd       = "WHEN NOT MATCHED BY SOURCE AND";
        internal const string When                            = "WHEN";
        internal const string Where                           = "WHERE";
        internal const string While                           = "WHILE";
        internal const string With                            = "WITH";
        internal const string WithCube                        = "WITH CUBE";
        internal const string WithRollup                      = "WITH ROLLUP";
        internal const string WithTies                        = "WITH TIES";

        #endregion

        internal static class Reserved
        {
            internal const string ReservedPreffix = "_";
            internal const string ReturnValue = "RETURN VALUE";
            internal const string ReturnValueOuterParam = "@_ro"; 
            internal const string ReturnValueInnerParam = "@_ri";
            internal const string ConcatVar = "@_cc"; 
            internal const string CursorNameBody = "_CUR";
            internal const string QtTransactionCount = "@_tc";
            internal const string QtTransactionSave = "_ts";
            internal const string TestTransactionCount = "@_tct";
            internal const string TestTransactionSave = "@_tst";
            internal const string CountColumnReserved = "COUNT";
            internal const string ReturnValueColumnName = "__QTReturnValue";
            internal static readonly string QtRowIDColumnName = "Z" + Regex.Replace(Guid.NewGuid().ToString(), "-", "");  // use unique name
            internal const string QtRowID = "RowID";
        }

        internal static class Free
        {
            internal const string Variable = "variable";
            internal const string Row = "row";
            internal const string Sentence = "sentence";
            internal const string Subject = "subject";
            internal const string Graph = "graph";
            internal static readonly string QueryTalkCode = String.Format("-- QueryTalk code:{0}", Environment.NewLine);
            internal static readonly string EndQueryTalkCode = String.Format("-- end of QueryTalk code{0}", Environment.NewLine);
            internal const string DefaultFontFamily = "Calibri";
            internal const string ConsolasFontFamily = "Consolas";
            internal const string AsyncDef = "Asynchronous Processing";
            internal const string SetAsyncDef = ";Asynchronous Processing=True;";
            internal const string Column = "column";
            internal const string Table = "Table";
            internal const string TableBulkInsert = "Table (bulk insert)";
            internal const string InlinerType = "Inliner";
            internal const string NameNullExtra = "Name argument is null. Use f.Name method to create a name object.";
            internal const string UdttNameArgumentExtra = "Check the userDefinedTableType argument of the .TableParam method.";
            internal const string TempTableParamExtra = "Check the table parameter of the .TableParam method.";
            internal const string ScalarViewNullExtra = "Check the View argument.";
            internal const string ViewNullExtra = "Check the View argument.";
            internal const string DataClassNoConstructorExtra = "Check if the data class has a parameterless constructor. It should be provided.";
            internal const string ExpressionNullExtra = "Make sure that the list of values passed to the .In method contains scalar values only. Data classes are not allowed.";
            internal const string AliasNullExtra = "Alias argument is null. Use .As method to create an alias object.";
            internal const string ChainObjectNullExtra = "Note that the chain objects are constructed by the chain methods and are not intended to be used directly by the code.";
            internal const string XmlToViewExtra = "Check the column argument of the .X method.";
            internal const string EnclosedInt = "[int]";
            internal const string EnclosedNVarcharMax = "[nvarchar](MAX)";
            internal const string ExecSpExecutesql = "EXEC sp_executesql";
            internal const string ExecSpExecutesql_N = "EXEC sp_executesql N'";
            internal const string CommaNSingleQuote = ",N'";
            internal const string Params = "-- Params:";
            internal const string OutputValues = "OutputValues";
            internal const string ReturningValues = "Returning Values";
            internal const string IfXactStateIsNotZero = "IF XACT_STATE() <> 0";
            internal const string ForXmlPathDefault = "FOR XML PATH('row'), ROOT('root'), ELEMENTS XSINIL";
            internal const string DataGridErrorMessage = "A cell value cannot be displayed.";
            internal const string UnexpectedError = "Unexpected error";
            internal const string UnexpectedException = "An unexpected exception has been thrown.";
            internal const string QtTesting = "QueryTalk Testing Environment";
            internal const string RestoreSQL = "Restore original SQL";
            internal const string Reader = "Reader";
            internal const string StoredProcTestMessage = "Only the stored procedure identifier can be edited in the SQL editor.\r\nAny additional SQL code will cause an error.";
            internal const string Stop = "stop";
            internal const string XmlToScalarViewSelect = "SELECT T.n.value('({0}/text())[1]', '[nvarchar](MAX)') AS {0}";
            internal const string XmlToScalarViewFrom = "FROM {0}.nodes('/root/row') AS T(n)";
            internal const string TruncateBulkTable = "TRUNCATE TABLE {0};";
            internal const string ReportExceptionToQueryTalk = "We are sorry for the inconvenience. If you believe that this exception is a library bug please report it to report@querytalk.com.";
            internal const string Warning = "Warning";
            internal const string Raiserror1 = "DECLARE @_e nvarchar(MAX),@_v int,@_s int;";
            internal const string Raiserror2 = "SELECT @_e = CAST(ERROR_NUMBER() AS nvarchar(10)) + N'|' + ERROR_MESSAGE(),@_v = ERROR_SEVERITY(),@_s = ERROR_STATE();";
            internal const string Raiserror3 = "RAISERROR(@_e, @_v, @_s);";
            internal const string RaiserrorS = "IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;DECLARE @_e nvarchar(MAX),@_v int,@_s int;SELECT @_e = CASE WHEN ERROR_NUMBER() != 50000 THEN CAST(ERROR_NUMBER() AS nvarchar(10)) + N'|' ELSE N'' END + ERROR_MESSAGE(),@_v = ERROR_SEVERITY(),@_s = ERROR_STATE(); RAISERROR(@_e, @_v, @_s);";
            internal static readonly string ReturnWithCommit = String.Format("IF {0} = 0 AND XACT_STATE() = 1 COMMIT TRANSACTION;RETURN", Reserved.QtTransactionCount);
            internal static readonly string ReturnValue = String.Format("ISNULL({0}, 0)", Reserved.ReturnValueOuterParam);
            internal const string RollbackTransactionCond = "IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;";
            internal const string BeginTestTransaction = "DECLARE @_ttc AS [int] = @@TRANCOUNT;IF @_ttc > 0 SAVE TRANSACTION _tts;ELSE BEGIN TRANSACTION;";
            internal const string EndTestTransaction = "IF @_ttc = 0 BEGIN;IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;END;ELSE IF XACT_STATE() = 1 ROLLBACK TRANSACTION _tts;";
            internal const string WhileFetchStatus = "WHILE @@FETCH_STATUS = 0";
            internal const string RowCountZero = "@@ROWCOUNT = 0";
            internal const string CastConcatenator = "CAST({0} AS [nvarchar](MAX))";
            internal const string StoredProcRootName = "stored procedure";
            internal const string SqlBatchRootName = "SQL batch";
            internal const string DbScalarFunctionRootName = "db scalar function";
            internal const string DbTableFunctionRootName = "db table function";
            internal const string DbViewRootName = "db view";
            internal const string GraphComment = "graph:";
            internal const string GraphTablePrefix = "@T";
            internal const string NotSupportedDataSourceType = "SQL data source contains a type that is not supported by the QueryTalk.";
            internal const string Inserted = "Inserted.*";
            internal const string Deleted = "Deleted.*";
            internal const string Renamed = "Renamed";
            internal const string Error547Check = "ERROR_NUMBER() = 547";
            internal const string DebuggingValueNotAvailable = "The debugging value is not available.";
            internal const string JsonSchemaNull = "Column1:o";
            internal const string JsonWithDefaultSchemaFormat = "{{\r\n  \"$json\": \"{0}\",\r\n  \"schema\": \"{1}\",\r\n  \"data\": {2}\r\n}}";
        }

        internal static class Class
        {
            internal const string Designer = "QueryTalk.d";
            internal const string ResultSet = "ResultSet`1";
        }

        internal static class Method
        {
            internal const string Ctor = "ctor";
            internal const string As = ".As";
            internal const string Await = ".Await";
            internal const string BeginTransaction = ".BeginTransaction";
            internal const string SaveTransaction = ".SaveTransaction";
            internal const string RollbackTransaction = ".RollbackTransaction";
            internal const string CommitTransaction = ".CommitTransaction";
            internal const string Select = ".Select";
            internal const string SelectCount = ".SelectCount";
            internal const string SelectDistinct = ".SelectDistinct";
            internal const string Collect = ".Collect";
            internal const string ThenCollect = ".ThenCollect";
            internal const string Declare = ".Declare/DeclareTable";
            internal const string Return = ".Return";
            internal const string Where = ".Where";
            internal const string WhereNot = ".WhereNot";
            internal const string WhereExists = ".WhereExists";
            internal const string WhereNotExists = ".WhereNotExists";
            internal const string And = ".And";
            internal const string AndNot = ".AndNot";
            internal const string AndExists = ".AndExists";
            internal const string AndNotExists = ".AndNotExists";
            internal const string Or = ".Or";
            internal const string OrNot = ".OrNot";
            internal const string OrExists = ".OrExists";
            internal const string OrNotExists = ".OrNotExists";
            internal const string End = ".End";
            internal const string EndBlock = ".EndBlock";
            internal const string While = ".While";
            internal const string C = ".C";
            internal const string E = ".E";
            internal const string ToView = ".ToView";
            internal const string ToJson = ".ToJson";
            internal const string FromJson = ".FromJson";
            internal const string ToList = ".ToList<T>";
            internal const string ToDataTable = ".ToDataTable";
            internal const string ToValue = ".ToValue";
            internal const string DefaultValues = ".Insert.DefaultValues";
            internal const string On = ".On";
            internal const string OnNot = ".OnNot";
            internal const string OnParity = ".OnParity";
            internal const string OnOrWhere = ".On/Where";
            internal const string InlinerTable = "Sys.InlineTable";
            internal const string InlinerColumn = "Sys.InlineColumn";
            internal const string InlinerSql = "Sys.InlineSql";
            internal const string InlinerExpression = "Sys.InlineExpression";
            internal const string InlinerSnippet = "Sys.InlineSnippet";
            internal const string InlinerData = "Sys.InlineData";
            internal const string Update = ".Update";
            internal const string By = ".By";
            internal const string UpdateFromBy = ".UpdateFrom.By";
            internal const string UpdateFrom = ".UpdateFrom";
            internal const string InsertFrom = ".InsertFrom";
            internal const string Insert = ".Insert";
            internal const string EndView = ".EndView";
            internal const string EndProc = ".EndProc";
            internal const string EndSnippet = ".EndSnippet";
            internal const string From = ".From";
            internal const string FromSelect = ".FromSelect";
            internal const string Run = ".Run";
            internal const string ConnectBy = ".ConnectBy";
            internal const string NameAs = "Root.As";
            internal const string ConnectByOrGo = ".Go";
            internal const string Param = ".Param";
            internal const string TableParam = ".TableParam";
            internal const string ParamOrTableParam = ".Param/TableParam";
            internal const string Pass = ".Pass";
            internal const string PassOrExec = ".Pass/Exec";
            internal const string DesignTable = ".DesignTable";
            internal const string TableColumn = ".DesignTable.Column";
            internal const string TableCheck = ".DeclareTable/CreateTempTable.Check";
            internal const string TableIdentity = ".DesignTable.Identity";
            internal const string TablePrimaryKey = ".DesignTable.PrimaryKey";
            internal const string TableUniqueKey = ".DesignTable.UniqueKey";
            internal const string CreateTempTableIndex = ".CreateTempTableIndex";
            internal const string DropTempTableIndex = ".DropTempTableIndex";
            internal const string DropTempTable = ".DropTempTable";
            internal const string Inject = ".Inject";
            internal const string Exec = ".Exec";
            internal const string Set = ".Set";
            internal const string New = ".New";
            internal const string Pack = ".Pack";
            internal const string NewDbRow = "DbRow<T>.ctor";
            internal const string SetIdentityInsert = ".SetIdentityInsert";
            internal const string AllowResources = ".AllowResources";
            internal const string SnippetResources = "QtSnippet.Resources";
            internal const string Go = ".Go";
            internal const string ExecGo = ".ExecGo";
            internal const string ExecGoAsync = ".ExecGoAsync";
            internal const string GoAsync = ".GoAsync";
            internal const string GoOrGoAsync = ".Go/GoAsync";
            internal const string GoAt = ".GoAt";
            internal const string ExistsGo = ".ExistsGo";
            internal const string Pivot = ".Pivot";
            internal const string PivotAs = ".Pivot/Unpivot.As";
            internal const string Unpivot = ".Unpivot";
            internal const string ImplicitConversion = "implicit conversion from Value to value type";
            internal const string IntoVars = ".IntoVars";
            internal const string IntoTempTable = ".IntoTempTable";
            internal const string Columns = ".Columns";
            internal const string ExecIntoReturnValue = ".Exec.IntoReturnValue";
            internal const string TryCatch = ".Try/Catch";
            internal const string Block = ".Block";
            internal const string BeginCursor = ".BeginCursor";
            internal const string BeginCursorWithVars = ".WithVars";
            internal const string BeginCursorFetchNext = ".FetchNext";
            internal const string EndCursor = ".EndCursor";
            internal const string If = ".If";
            internal const string ElseIf = ".ElseIf";
            internal const string IfExists = ".IfExists";
            internal const string ElseIfExists = ".ElseIfExists";
            internal const string Case = "Sys.Case";
            internal const string CaseValue = "Sys.Case.Value";
            internal const string CaseWhen = "Sys.Case.When";
            internal const string CaseElse = "Sys.Case.Else";
            internal const string CaseThen = "Sys.Case.Then";
            internal const string Expression = "expression";
            internal const string Identifier = "d.Identifier";
            internal const string I = ".I";
            internal const string AsAscDesc = ".AsAsc/AsDesc";
            internal const string In = ".In/NotIn";
            internal const string InXml = ".InXml/NotInXml";
            internal const string ParamOptional = ".Param.Optional";
            internal const string ParamNotNull = ".Param.NotNull";
            internal const string InlineParam = ".Param";
            internal const string AsUdf = ".AsUdf";
            internal const string AsUdt = ".AsUdt";
            internal const string Sys = "Sys.*";
            internal const string SysUdt = "Sys.Udt";
            internal const string TruncateTable = ".TruncateTable";
            internal const string Out = ".Out";
            internal const string Output = ".Output";
            internal const string OutputInto = "Output.Into";
            internal const string ToTable = ".ToTable";
            internal const string ApplyAs = ".Apply.As";
            internal const string FromAs = ".From.As";
            internal const string JoinAs = ".Join.As";
            internal const string ViewAs = "View.As";
            internal const string SysRankingOrderBy = "<Ranking>.OrderBy";
            internal const string SysRankingPartitionBy = "<Ranking>.PartitionBy";
            internal const string Raiserror = ".Raiserror";
            internal const string Of = ".Of";
            internal const string OrderBy = ".OrderBy";
            internal const string SysConvert = "Sys.Convert";
            internal const string SysCast = "Sys.Cast";
            internal const string FullJoin = ".FullJoin";
            internal const string Goto = ".Goto";
            internal const string Label = ".Label";
            internal const string LeftOuterJoin = ".LeftOuterJoin";
            internal const string RightOuterJoin = ".RightOuterJoin";
            internal const string OuterApply = ".OuterApply";
            internal const string CrossApply = ".CrossApply";
            internal const string CrossJoin = ".CrossJoin";
            internal const string InnerJoin = ".InnerJoin";
            internal const string Join = ".Join";
            internal const string JoinSynonym = ".Join(synonym)";
            internal const string UseAs = ".UseAs";
            internal const string Cte = ".Cte";
            internal const string Delete = ".Delete";
            internal const string DeleteFrom = ".DeleteFrom";
            internal const string GroupBy = ".GroupBy";
            internal const string Having = ".Having";
            internal const string InsertColumns = ".Insert.Columns";
            internal const string InsertValues = ".Insert.Values";
            internal const string FromMany = ".FromMany";
            internal const string Top = ".Top";
            internal const string Union = ".Union";
            internal const string UnionAll = ".UnionAll";
            internal const string Intersect = ".Intersect";
            internal const string Except = ".Except";
            internal const string UpdateSet = ".Update.Set";
            internal const string WithRollup = ".WithRollup";
            internal const string WithCube = ".WithCube";
            internal const string Value = "Value..ctor";
            internal const string QtSql = "QtSql..ctor";
            internal const string Mapper = "Mapper..ctor";
            internal const string Async = "Asynchronous call";
            internal const string Concat = "f.Concat";
            internal const string SysNameUnsafe = "Sys.NameUnsafe";
            internal const string Comment = ".Comment";
            internal const string Delay = ".Delay";
            internal const string Test = ".Test";
            internal const string IsGlobalCachingOn = "set_IsGlobalCachingOn";
            internal const string GlobalCacheSize = "set_GlobalCacheSize";
            internal const string GlobalCacheQueryTimeout = "set_GlobalCacheQueryTimeout";
            internal const string SetConnection = ".SetConnection";
            internal const string SetTestingEnvironmentOff = ".SetTestingEnvironmentOff";
            internal const string NodeChainJoin = "<node chain>.Join()";
            internal const string HavingMaxMin = ".HavingMax/Min";
            internal const string PredicateMethod = "predicate method";
            internal const string PredicateGrouping = ".Block/EndBlock";
            internal const string With = ".With";
            internal const string ReloadGo = ".ReloadGo";
            internal const string DeleteGo = ".DeleteGo";
            internal const string DeleteGoAsync = ".DeleteGoAsync";
            internal const string DeleteGoAndAsync = ".UpdateGo/Async";
            internal const string UpdateGo = ".UpdateGo";
            internal const string UpdateGoAsync = ".UpdateGoAsync";
            internal const string UpdateGoAndAsync = ".UpdateGo/Async";
            internal const string InsertGo = ".InsertGo";
            internal const string InsertGoAsync = ".InsertGoAsync";
            internal const string InsertGoAndAsync = ".InsertGo/Async";
            internal const string InsertCascadeGo = ".InsertCascadeGo";
            internal const string InsertRowsGo = ".InsertRowsGo";
            internal const string UpdateRowsGo = ".UpdateRowsGo";
            internal const string DeleteRowsGo = ".DeleteRowsGo";
            internal const string CrudRowsGo = ".***RowsGo";
            internal const string DeleteCascadeGo = ".DeleteCascadeGo";
            internal const string BulkInsertGo = ".BulkInsertGo";
            internal const string InvokeConnectionManager = ".InvokeConnectionManager";
            internal const string AddRow = ".AddRow";
        }

        #region StringBuilder Helper/Extension Methods

        internal static StringBuilder GenerateSql(int capacity)
        {
            return new StringBuilder(capacity);
        }

        internal static StringBuilder NewLine(this StringBuilder builder)
        {
            builder.Append(Environment.NewLine);
            return builder;
        }

        internal static StringBuilder NewLine(this StringBuilder builder, string text)
        {
            builder.Append(Environment.NewLine);
            builder.Append(text);
            return builder;
        }

        internal static StringBuilder Indent(this StringBuilder builder, int indentLevel = 1)
        {
            for (int i = 0; i < indentLevel; ++i)
            {
                builder.Append(TwoSpaces);
            }

            return builder;
        }

        internal static StringBuilder Indent(this StringBuilder builder, string text, int indentLevel = 1)
        {
            builder.Indent(indentLevel);
            builder.Append(text);
            return builder;
        }

        internal static StringBuilder NewLineIndent(this StringBuilder builder, int indentLevel = 1)
        {
            builder.Append(Environment.NewLine);
            builder.Indent(indentLevel);
            return builder;
        }

        internal static StringBuilder NewLineIndent(this StringBuilder builder, string text, int indentLevel = 1)
        {
            builder.Append(Environment.NewLine);
            builder.Indent(indentLevel);
            builder.Append(text);
            return builder;
        }

        internal static StringBuilder S(this StringBuilder builder)
        {
            builder.Append(OneSpace);
            return builder;
        }

        internal static StringBuilder TrimEnd(this StringBuilder builder)
        {
            if (builder.Length > 0)
            {
                if (Char.IsWhiteSpace(builder[builder.Length - 1]))
                {
                    TrimEnd(builder.Remove(builder.Length - 1, 1));
                }
            }

            return builder;
        }

        internal static StringBuilder EncloseLeft(this StringBuilder builder)
        {
            builder.Append(Text.LeftBracket);
            return builder;
        }

        internal static StringBuilder EncloseRight(this StringBuilder builder)
        {
            builder.Append(Text.RightBracket);
            return builder;
        }

        internal static StringBuilder AppendComma(this StringBuilder builder)
        {
            builder.Append(Text.Comma);
            return builder;
        }

        internal static StringBuilder AppendDot(this StringBuilder builder)
        {
            builder.Append(Text.Dot);
            return builder;
        }

        internal static StringBuilder Terminate(this StringBuilder builder)
        {
            builder.TrimEnd();  // assure that semi-colon is appended without a space
            builder.Append(Terminator);
            return builder;
        }

        // terminates batch with a single terminator
        internal static StringBuilder TerminateSingle(this StringBuilder builder)
        {
            if (builder.Length == 0)
            {
                return builder;
            }

            builder.TrimEnd();
            _TerminateSingle(builder);
            builder.Append(Terminator);
            return builder;
        }

        private static StringBuilder _TerminateSingle(this StringBuilder builder)
        {
            if (builder.Length > 0)
            {
                if (builder[builder.Length - 1] == Text.TerminatorChar)
                {
                    builder.Length--;
                }
            }

            return builder;
        }

        #endregion

    }
}
