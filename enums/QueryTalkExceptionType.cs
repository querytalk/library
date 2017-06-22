#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Types of a QueryTalk exception.
    /// </summary>
    public enum QueryTalkExceptionType : int
    {       
        /// <summary>
        /// Exception of type 'QueryTalk.QueryTalkException' was thrown.
        /// </summary>
        [Exception(
            "Exception of type 'QueryTalk.QueryTalkException' was thrown.",
            null)]
        UnknownQueryTalkException,
        
        /// <summary>
        /// The casting is invalid.
        /// </summary>
        [Exception(
            "The casting is invalid.",
            "The casting between the Value object and the CLR type cannot be done. Note that the CLR type of the value inside the Value object must be the same as the CLR type to cast to. Use the GetType() method on Value object to check its embedded type.")]
        InvalidCast,
        
        /// <summary>
        /// Null cannot cast to a value type.
        /// </summary>
        [Exception(
            "Null cannot cast to a value type.",
            "When implicitly casting a Value object to a CLR value type, make sure that the Value object does not contain a null value. Otherwise cast to a nullable CLR value type."
            )]
        NullCannotCast,
      
        /// <summary>
        /// The argument is null.
        /// </summary>
        [Exception(
            "The argument is null.",
            "Provide a non-null argument.")]
        ArgumentNull,
     
        /// <summary>
        /// The .With is invalid.
        /// </summary>
        [Exception(
            "The .With is invalid.",
            "The subject of the .With method and the .With argument must be a table. Synonyms are not allowed.")]
        InvalidWith,
        
        /// <summary>
        /// The predecessor chain object is null.
        /// </summary>
        [Exception(
            "The predecessor chain object is null.",
            "It is not intended to use the chain object outside the method chaining.")]
        PredecessorNull,
    
        /// <summary>
        /// The inliner argument is null.
        /// </summary>
        [Exception(
            "The inliner argument is null.",
            "Provide a non-null inliner argument.")]
        InlinerArgumentNull,
        
        /// <summary>
        /// Alias is not allowed to be null or empty.
        /// </summary>
        [Exception(
            "Alias is not allowed to be null or empty.",
            "Provide a valid alias.")]
        AliasNullOrEmpty,
        
        /// <summary>
        /// The target table is not found.
        /// </summary>
        [Exception(
            "The target table is not found.",
            "The alias in UPDATE/DELETE statement is missing or does not point to the existing table.")]
        TargetTableNotFound,
   
        /// <summary>
        /// Collection argument is not allowed to have a null value or to be empty.
        /// </summary>
        [Exception(
            "Collection argument is not allowed to be null or empty.",
            "Provide a valid collection argument.")]
        CollectionNullOrEmpty,
     
        /// <summary>
        /// The parameter argument is null.
        /// </summary>
        [Exception(
            "The parameter argument is null.",
            "Provide a non-null parameter argument.")]
        ParamArgumentNull,
        
        /// <summary>
        /// Optional parameter's default argument is not allowed to have a null value.
        /// </summary>
        [Exception(
            "Optional parameter's default argument is not allowed to have a null value.",
            "Provide a non-null default argument."
            )]
        OptionalParamDefaultArgumentNull,
        
        /// <summary>
        /// The table-valued parameter is invalid.
        /// </summary>
        [Exception(
            "The table-valued parameter is invalid.",
            "The table-valued parameter of a table variable or temp table accepts View argument. Bulk table parameter accepts System.Data.DataTable argument. It is not possible to pass a variable in the nested calls.")]
        InvalidTableArgument,
        
        /// <summary>
        /// Nested bulk insert is not allowed.
        /// </summary>
        [Exception(
            "Nested bulk insert is not allowed.",
            "Bulk insert is only allowed in the most outer procedure.")]
        NestedBulkInsertDisallowed,
      
        /// <summary>
        /// The variable or parameter name is invalid.
        /// </summary>
        [Exception(
            "The variable or parameter name is invalid.",
            "The variable/parameter name should follow the rules: " + 
            "\r\n   1. The first character must be the at-sign (@)." +
            "\r\n   2. The second character must not be the underscore character (_), the number, or at sign (@)." +
            "\r\n   3. Subsequent characters can include letters as defined in the Unicode Standard 3.2., decimal numbers from either Basic Latin or other national scripts, the at-sign (@), dollar sign ($), number sign (#), or underscore (_)." +
            "\r\n   4. Embedded spaces or special characters are not allowed." +
            "\r\n   5. Supplementary characters are not allowed."
            )]
        InvalidVariableName,
        
        /// <summary>
        /// The temporary table name is invalid.
        /// </summary>
        [Exception(
            "The temporary table name is invalid.",
            "A parameter name for the temporary table should follow the rules for temporary table names: " +
            "\r\n   1. The first character must be the number sign (#)." +
            "\r\n   2. The second character must not be the number sign (#)." +
            "\r\n   3. Subsequent characters can include letters as defined in the Unicode Standard 3.2., decimal numbers from either Basic Latin or other national scripts, the at-sign (@), dollar sign ($), number sign (#), or underscore (_)." +
            "\r\n   4. Embedded spaces or special characters are not allowed." +
            "\r\n   5. Supplementary characters are not allowed."
            )]
        InvalidTempTableName,
     
        /// <summary>
        /// The alias is invalid.
        /// </summary>
        [Exception(
            "The alias is invalid.",
            "Alias should follow the rules:" +
            "\r\n   1. Alias name should not be undefined or empty string." +
            "\r\n   2. The first character must not be the underscore (_), at-sign (@) or number sign (#)." +
            "\r\n   3. Subsequent characters can include letters as defined in the Unicode Standard 3.2., decimal numbers from either Basic Latin or other national scripts, the at-sign (@), dollar sign ($), number sign (#), or underscore (_)." +
            "\r\n   4. Embedded spaces or special characters are not allowed." +
            "\r\n   5. Supplementary characters are not allowed."
            )]
        InvalidAlias,
       
        /// <summary>
        /// The expression string is invalid.
        /// </summary>
        [Exception(
            "The expression string is invalid.",
            "Expression string cannot be empty or contain the semicolon (;) or the sequence --."
            )]
        InvalidExpressionString,
        
        /// <summary>
        /// The cursor block is invalid.
        /// </summary>
        [Exception(
            "The cursor block is invalid.",
            "A valid cursor block consists of .BeginCursor, .FetchNext, and .EndCursor methods."
            )]
        InvalidCursorBlock,
      
        /// <summary>
        /// The .FetchNext method is missing.
        /// </summary>
        [Exception(
            "The .FetchNext method is missing.",
            "Provide a missing .FetchNext method inside the cursor block."
            )]
        MissingFetchNextMethod,
      
        /// <summary>
        /// The concatenator is not allowed.
        /// </summary>
        [Exception(
            "The concatenator is not allowed.",
            "Concatenator is allowed only in the SELECT queries."
            )]
        ConcatenatorDisallowed,
       
        /// <summary>
        /// The DB type size exceeds the maximum.
        /// </summary>
        [Exception(
            "The DB type size exceeds the maximum.",
            "The DB type declaration contains a size definition that exceeds the maximum allowed for the given type. Provide a DB type declaration with the size definition within the permitted range."
            )]
        DbTypeOversize,
      
        /// <summary>
        /// The DB type is invalid.
        /// </summary>
        [Exception(
            "The DB type is invalid.",
            "The DB type declaration contains either a negative value of length, precision, scale, or has the scale less than or equal to the precision. It may also be that the definition of length, precision, or scale was used on a type that do not expect such definition. Provide a valid DB type declaration."
            )]
        InvalidDbTypeDeclaration,
       
        /// <summary>
        /// The parameter or variable is not declared.
        /// </summary>
        [Exception(
            "The parameter or variable has not been declared.",
            "Declare the parameter/variable before use. Pay attention to case sensitivity."
            )]
        ParamOrVariableNotDeclared,

        /// <summary>
        /// The specified CLR type is not QueryTalk compliant or does not match the required type.
        /// </summary>
        [Exception(
            "The specified type is not QueryTalk compliant or does not match the required type.",
            "The CLR type of every inlined data, scalar argument, QueryTalk.Value argument, System.Object value or data class property should be a QueryTalk compliant CLR type. This exception can also be thrown if the argument type does not match the parameter type."
            )]
        NonCompliantClrType,

        /// <summary>
        /// Argument type does not match the parameter type.
        /// </summary>
        [Exception(
            "Argument type does not match the parameter type.",
            "Pass the argument that matches the required parameter type."
            )]
        ParamArgumentTypeMismatch,

        /// <summary>
        /// The specified parameter or variable has already been declared.
        /// </summary>
        [Exception(
            "The specified parameter or variable has already been declared.",
            "Make sure that parameter and variable names are unique within a QueryTalk procedure."
            )]
        ParamOrVariableAlreadyDeclared,

        /// <summary>
        /// The label is invalid.
        /// </summary>
        [Exception(
            "The label is invalid.",
            "Provide the label name that follows the rules for regular SQL identifiers."
            )]
        InvalidLabel,

        /// <summary>
        /// The label has already been declared.
        /// </summary>
        [Exception(
            "The label has already been declared.",
            "Make sure that label names are unique within a QueryTalk procedure."
            )]
        LabelAlreadyDeclared,

        /// <summary>
        /// The temporary table is not known.
        /// </summary>
        [Exception(
            "The temporary table is not known.",
            "Make sure that a temporary table is either declared as a parameter or created by the .CreateTempTable method. Note that QueryTalk checks the existence of a temp table in the scope of the procedure and not in the scope of the current transaction."
            )]
        UnknownTempTable,

        /// <summary>
        /// The parameter argument has not been passed.
        /// </summary>
        [Exception(
            "The parameter argument has not been passed.",
            "Pass the required argument."
            )]
        ParamArgumentNotPassed,

        /// <summary>
        /// The number of passed arguments do not match the number of parameters.
        /// </summary>
        [Exception(
            "The number of passed arguments do not match the number of parameters.",
            "Make sure that you correctly pass all the required arguments."
            )]
        ArgumentCountMismatch,

        /// <summary>
        /// The parameter has not been declared as an output parameter.
        /// </summary>
        [Exception(
            "The parameter has not been declared as an output parameter.",
            "It is not allowed to pass an argument by reference (using .Output method) if the parameter is not declared as an output parameter. Pass the argument without reference or declare the parameter as an output parameter."
            )]
        NonOutputParam,

        /// <summary>
        /// The optional parameter has the invalid position.
        /// </summary>
        [Exception(
            "The optional parameter has the invalid position.",
            "All optional parameters should be placed after the sequence of the required parameters."
            )]
        OptionalParamInvalidPosition,

        /// <summary>
        /// Argument length, scale or precision is greater than the declared one.
        /// </summary>
        [Exception(
            "Argument length, scale or precision is greater than the declared one.",
            "Make sure that you pass the argument whose length, scale or precision does not exceed the parameter's scope."
            )]
        InvalidArgumentSize,

        /// <summary>
        /// The view is invalid.
        /// </summary>
        [Exception(
            "The view is invalid.",
            "A View object can consist of one SELECT statement only, does not support common table expressions (CTE), data view arguments and cannot be parameterized. Note that variables are allowed in views."
            )]
        InvalidView,

        /// <summary>
        /// Inliner is invalid.
        /// </summary>
        [Exception(
            "The inliner is invalid.",
            "This exception was thrown because the inliner cannot be used or is not allowed in the specified method."
            )]
        InvalidInliner,

        /// <summary>
        /// The inliner is not found.
        /// </summary>
        [Exception(
            "The inliner is not found.",
            "A specified variable does not match any given inliner."
            )]
        InlinerNotFound,

        /// <summary>
        /// Parameter argument has already been passed.
        /// </summary>
        [Exception(
            "Parameter argument has already been passed.",
            "Make sure that each parameter argument is passed only once."
            )]
        ParamArgumentAlreadyPassed,

        /// <summary>
        /// Null cannot be passed as an output variable.
        /// </summary>
        [Exception(
            "Null cannot be passed as an output variable.",
            "Provide a non-null value."
            )]
        NullReferenceOutput,

        /// <summary>
        /// The table alias is invalid.
        /// </summary>
        [Exception(
            "The table alias is invalid.",
            "Numbers are not allowed as table aliases. They are reserved for the QueryTalk library internal use. Provide a valid table alias."
            )]
        InvalidTableAlias,

        /// <summary>
        /// The result set is invalid.
        /// </summary>
        [Exception(
            "The result set is invalid.",
            "It is not recommended to interfere with the result sets of a Result object. Use the Bag property to add user customized data."
            )]
        InvalidResultSet,

        /// <summary>
        /// The table alias has already been used.
        /// </summary>
        [Exception(
            "The table alias has already been used.",
            "Provide a unique table alias in the entire query statement."
            )]
        TableAliasDuplicate,

        /// <summary>
        /// The identifier is invalid.
        /// </summary>
        [Exception(
            "The identifier is invalid.",
            "Identifiers cannot have more than 4 parts."
            )]
        InvalidIdentifier,

        /// <summary>
        /// The table design is invalid.
        /// </summary>
        [Exception(
            "The table design is invalid.",
            "Table columns are missing. Provide at least one column in table definition using .Column method."
            )]
        InvalidTableDesign,

        /// <summary>
        /// The table identifier is invalid.
        /// </summary>
        [Exception(
            "The table identifier is invalid.",
            "Provide a valid SQL name for the table variable or temporary table: " +
            "\r\n   1. The first character must be the at-sign @ (for table variables) or number sign # (for temporary tables)." +
            "\r\n   2. The second character must not be the at-sign (@) or number sign (#)." +
            "\r\n   3. Subsequent characters can include letters as defined in the Unicode Standard 3.2., decimal numbers from either Basic Latin or other national scripts, the at sign (@), dollar sign ($), number sign (#), or underscore (_)." +
            "\r\n   4. Embedded spaces or special characters are not allowed." +
            "\r\n   5. Supplementary characters are not allowed."
            )]
        InvalidTableIdentifier,

        /// <summary>
        /// The column identifier is invalid.
        /// </summary>
        [Exception(
            "The column identifier is invalid.",
            "Provide a column identifier that have one or two parts. Column identifiers cannot have more than two parts."
            )]
        InvalidColumnIdentifier,

        /// <summary>
        /// The identifier is invalid.
        /// </summary>
        [Exception(
            "The identifier is invalid.",
            "If Identifier argument is used in .By method, it must be a 2-part identifier: alias and column."
            )]
        InvalidByIdentifier,

        /// <summary>
        /// The common table expression is invalid.
        /// </summary>
        [Exception(
            "The common table expression is invalid.",
            "When using successive .From methods, then every non-last .From method represents a common table expression (CTE). This exception was thrown because: \r\n" +
            "   - CTE expects a view object as an argument which was not provided, \r\n" +
            "   - CTE can be positioned only in the very beginning of the query (not as part of the UNION clause)."
            )]
        InvalidCte,

        /// <summary>
        /// SQL data source does not contain the target column.
        /// </summary>
        [Exception(
            "SQL data source does not contain the target column.",
            "Check the column in your data class that does not match the data source. Keep in mind that the QueryTalk library is case-sensitive when assigning the property names."
            )]
        MismatchedTargetColumn,

        /// <summary>
        /// The property's get/set accessor is not public.
        /// </summary>
        [Exception(
            "The property's get and set accessors are not public.",
            "Make sure that the property has both accessors defined as public."
            )]
        NonAccessableProperty,

        /// <summary>
        /// The DataTable cannot be used with .Enumerate method.
        /// </summary>
        [Exception(
            "The DataTable cannot be used with .Enumerate method.",
            "Do not use the .Enumerate method with the DataTable class or change the DataTable class with any other QueryTalk compliant data class."
            )]
        DataTableCannotEnumerate,

        /// <summary>
        /// The data class is invalid.
        /// </summary>
        [Exception(
            "The data class is invalid.",
            "The data class is not a QueryTalk compliant data storage:" +
            "\r\n    - It should be a class." +
            "\r\n    - It should have a public parameterless constructor." +
            "\r\n    - It should have at least one public property which is a QueryTalk CLR compliant type." +
            "\r\n    - QueryTalk compliant properies should have getters and setters publicly accesible." +
            "\r\n    - At least one data class property and data source column should match in name and type.")]
        InvalidDataClass,

        /// <summary>
        /// The DbRow class is invalid.
        /// </summary>
        [Exception(
            "The DbRow class is invalid.",
            "The DbRow base class is not intended for public use."
            )]
        InvalidDbRow,

        /// <summary>
        /// The number of child levels has exceeded the allowed maximum.
        /// </summary>
        [Exception(
            "The number of child levels has exceeded the allowed maximum.",
            "This exception has been thrown because the number of child nested levels of the root node has exceeded the allowed maximum. You may need to increase the value of the argument maxLevels. Check also if there are the cross relations in the database which cause circular calls."
            )]
        MaxLevelsExceeded,

        /// <summary>
        /// The data table is invalid.
        /// </summary>
        [Exception(
            "The data table is invalid.",
            "The data table does not contain any columns."
            )]
        InvalidDataTable,

        /// <summary>
        /// SQL data source has delivered all available tables.
        /// </summary>
        [Exception(
            "SQL data source has delivered all available tables.",
            "No more data is available in the data source. Check again, what resultset should be returned by the procedure and adapt the .Load method accordingly."
            )]
        NoMoreResultset,

        /// <summary>
        /// The resultset is empty.
        /// </summary>
        [Exception(
            "The resultset is empty.",
            ".ToValue method cannot return a value."
            )]
        EmptyResultset,

        /// <summary>
        /// Row data class has more columns that the result set.
        /// </summary>
        [Exception(
            "Row data class has more columns that the result set.",
            "When using the Row data class, make sure that every Row column matches the column in a data source by data type and ordinal position."
            )]
        NoMoreColumns,

        /// <summary>
        /// The data class column type does not match the type of a result set column.
        /// </summary>
        [Exception(
            "The data class column type does not match the type of a result set column.",
            "Note that a data class column type must match the CLR type of the result set column (provided by the ADO data reader). Make sure that all columns match by type."
            )]
        ColumnTypeMismatch,

        /// <summary>
        /// The TimeSpan value exceeds the limits of the SQL time type.
        /// </summary>
        [Exception(
            "The TimeSpan value exceeds the range of the SQL time type.",
            "The TimeSpan CLR type is mapped to the SQL time type which has limited range between 00:00:00.0000000 and 23:59:59.9999999. Do not use greater TimeSpan values."
            )]
        TimeOutOfRange,

        /// <summary>
        /// Name prefix is reserved.
        /// </summary>
        [Exception(
            "Name prefix is reserved.",
            "The variable or parameter name contains the underscore prefix (_) which is the reserved name prefix of the QueryTalk library and cannot be used by clients."
            )]
        ReservedNamePrefix,

        /// <summary>
        /// Result set contains columns with identical CLR names.
        /// </summary>
        [Exception(
            "Result set contains columns with identical CLR names.",
            "When using classes for storing data make sure that every column in the data source has a unique name. Note that QueryTalk checks the names after the conversion into CLR name is done (e.g. [Col] and [Co.l] produces the same CLR name: Col). Use dynamic or DataTable storage to load columns with ambiguous CLR names."
            )]
        ColumnNameDuplicate,

        /// <summary>
        /// Operation cancelled by the user.
        /// </summary>
        [Exception(
            "Operation canceled by the user.",
            "The operation was canceled by the user. The exception was thrown during the process of reading/displaying data. The results, if any, should be discarded."
            )]
        OperationCanceledByUser,

        /// <summary>
        /// It is not allowed to pass a client variable to the inner procedure by reference.
        /// </summary>
        [Exception(
            "It is not allowed to pass a client variable to the inner procedure by reference.",
            "The client cannot fetch the output variable directly from the inner procedure. The output values of the inner procedures should be returned to the calling procedure using SQL variables. Only the outermost call can collect the output values and pass it to the CLR environment."
            )]
        OutputVariableDisallowed,

        /// <summary>
        /// The variable count does not match the column count.
        /// </summary>
        [Exception(
            "The variable count does not match the column count.",
            "When storing column values into variables make sure that the number of variables equals the number of columns."
            )]
        VariableColumnCountMismatch,

        /// <summary>
        /// The column count is invalid.
        /// </summary>
        [Exception(
            "The column count is invalid.",
            "Make sure that the number of columns in the .Columns method equals the number of values in the .Select/ThenSelect method. Also pay attention that every set of values in the .ThenSelect method contains the same number of values as previous set of values."
            )]
        InvalidColumnCount,

        /// <summary>
        /// It is not allowed to store aliased column into a variable.
        /// </summary>
        [Exception(
            "It is not allowed to store aliased column into a variable.",
            "When storing a column value into a variable, make sure that the column does not have an alias."
            )]
        AliasedColumnDisallowed,

        /// <summary>
        /// It is not allowed to use undefined columns.
        /// </summary>
        [Exception(
            "It is not allowed to use undefined columns.",
            "Do explicitly enumerate the columns in the .Select method when followed by the .IntoVars or .IntoColumns method."
            )]
        UndefinedColumnsDisallowed,

        /// <summary>
        /// If block is not closed properly.
        /// </summary>
        [Exception(
            "If block is not closed properly.",
            "Make sure that every .If method matches the corresponding .EndIf method."
            )]
        InvalidIfBlock,

        /// <summary>
        /// While block is not closed properly.
        /// </summary>
        [Exception(
            "While block is not closed properly.",
            "Make sure that every .While method matches the corresponding .EndWhile method."
            )]
        InvalidWhileBlock,

        /// <summary>
        /// TryCatch block is not closed properly.
        /// </summary>
        [Exception(
            "TryCatch block is not closed properly.",
            "Make sure that every TryCatch block consists of .Try, .Catch and .EndTryCatch method."
            )]
        InvalidTryCatchBlock,

        /// <summary>
        /// The transaction name is invalid.
        /// </summary>
        [Exception(
            "The transaction name is invalid.",
            "Provide a transaction name or variable that follows the rules for regular SQL identifiers, must not exceed 32 characters and must not begin with the underscore character."
            )]
        InvalidTransactionName,

        /// <summary>
        /// Begin transaction is missing.
        /// </summary>
        [Exception(
            "Begin transaction is missing.",
            "The .CommitTransaction and the .RollbackTransaction methods require the preceding .BeginTransaction method which was not provided."
            )]
        BeginTransactionMissing,

        /// <summary>
        /// The index name is invalid.
        /// </summary>
        [Exception(
            "The index name is invalid.",
            "Provide an index name that follows the rules for regular SQL identifiers and do not begin with the underscore character."
            )]
        InvalidIndexName,

        /// <summary>
        /// Transactional interference in the transactional object is not allowed.
        /// </summary>
        [Exception(
            "Transactional interference in the transactional object is not allowed.",
            "The .BeginTransaction, .SaveTransaction, .CommitTransaction, and .RollbackTransaction methods are not allowed to be used inside the QueryTalk transactional object."
            )]
        TransactionalInterference,

        /// <summary>
        /// The data cannot be serialized.
        /// </summary>
        [Exception(
            "The data cannot be serialized.",
            "Check the type of the data object. The following types are allowed: " +
            "\r\n   - QueryTalk CLR compliant scalar types " +
            "\r\n   - QueryTalk.Value " +
            "\r\n   - QueryTalk compliant data classes " +
            "\r\n   - any generic collection that contains QueryTalk compliant items " +
            "\r\n   - QueryTalk.Result" +
            "\r\n   - System.DataTable "
            )]
        InvalidSerializationData,

        /// <summary>
        /// The dynamic result is null or empty.
        /// </summary>
        [Exception(
            "The dynamic result is null or empty.",
            "When serializing the dynamic result it must contain a non-empty result set."
            )]
        EmptyDynamicResult,

        /// <summary>
        /// The data view is invalid.
        /// </summary>
        [Exception(
            "The data view is invalid.",
            "This exception was thrown because the parameter expects a data view argument which was not provided. Pay attention that a data view can only be created by .ToView (or d.ToView) method. Provide a valid data view."
            )]
        InvalidDataView,
        
        /// <summary>
        /// The data view is missing a user-defined data type definition.
        /// </summary>
        [Exception(
            "The data view is missing a user-defined data type definition.",
            "A view passed to the stored procedure using d.Exec method is missing a user-defined data type definition. Provide a valid UDT identifier using <view>.ForUdt method."
            )]
        MissingDataViewUdt,

        /// <summary>
        /// Connection function is not defined.
        /// </summary>
        [Exception(
            "Connection function is not defined.",
            "Use the d.SetConnection method before the first execution.\r\n   Note that by default the connection has the scope of an assembly (from where the connection method was called). If you want to extend the connection scope to all assemblies in the AppDomain, use global parameter in the connection method.")]
        MissingConnectionFunc,

        /// <summary>
        /// The identity column contains invalid increment.
        /// </summary>
        [Exception(
            "The identity column contains invalid increment.",
            "Provide a non-zero increment value."
            )]
        InvalidIdentityIncrement,

        /// <summary>
        /// The snippet is invalid.
        /// </summary>
        [Exception(
            "The snippet is invalid.",
            "The snippet object contains the .Param method which is not allowed. Note that snippet is headerless, cannot have an embedded transaction and cannot be parameterized.")]
        InvalidSnippet,

        /// <summary>
        /// Table variable is not allowed.
        /// </summary>
        [Exception(
            "Table variable is not allowed.",
            "Table variables cannot be used in concatenated queries. Use a user-defined table type or temporary table instead.")]
        TableVariableDisallowed,

        /// <summary>
        /// The variable is not a table variable.
        /// </summary>
        [Exception(
            "The variable is not a table variable.",
            "Scalar variables cannot be used for representing the tables. Provide a table variable.")]
        TableVariableMissing,

        /// <summary>
        /// The concatenation is not allowed.
        /// </summary>
        [Exception(
            "The concatenation is not allowed.",
            "This exception was thrown because the concatenation is not allowed in the specified chain method. Note that in general the concatenation is not allowed in expressions."
            )]
        ConcatenationDisallowed,

        /// <summary>
        /// The comment is too long.
        /// </summary>
        [Exception(
            "The comment is too long.",
            "A comment line is not allowed to have more than 100 characters."
            )]
        CommentTooLong,

        /// <summary>
        /// The subject is null.
        /// </summary>
        [Exception(
            "The subject is null.",
            "A subject in SEMQ sentence cannot be null. Provide a non-null subject."
            )]
        SubjectNull,

        /// <summary>
        /// A link between the two tables is not found.
        /// </summary>
        [Exception(
            "A link between the two tables is not found.",
            "The two tables are not directly related to each other. The SEMQ attempt to find an intermediate table has failed. Provide the tables that are either directly related or exists a single intermediate table through which a link between the two tables can be established. Note that a table is not in relation with itself unless it has its own foreign key.")]
        LinkNotFound,

        /// <summary>
        /// Link between the tables is ambiguous.
        /// </summary>
        [Exception(
            "The link between the two tables is ambiguous.",
            "The two tables are not directly related to each other. The SEMQ attempt to find an intermediate table has failed due to the link ambiguity. There is more than a single link between the tables. Provide the tables that are either directly related or exists a single intermediate table through which the link between the two tables can be established.")]
        LinkAmbiguity,

        /// <summary>
        /// A foreign key is missing.
        /// </summary>
        [Exception(
            "A foreign key is missing.",
            "Note that the foreign key column must be specified using .By method when the link between the two tables contains more than one relation.")]
        MissingForeignKey,

        /// <summary>
        /// The intermediate table is not allowed.
        /// </summary>
        [Exception(
            "The intermediate table is not allowed.",
            "The link between the tables exists via an intermediate table, but it is not allowed to be used by the calling method. Intermediate tables are allowed only in the SEMQ predicate methods.")]
        IntermediateTableDisallowed,

        /// <summary>
        /// The related node is not found.
        /// </summary>
        [Exception(
            "The related node is not found.",
            ".With method accepts only nodes that are related directly to the subject.")]
        WithRelatedNotFound,

        /// <summary>
        /// The foreign key is not found.
        /// </summary>
        [Exception(
            "The foreign key is not found.",
            "The foreign key provided by .By method does not exist in the relation. Provide a valid foreign key.")]
        ForeignKeyNotFound,

        /// <summary>
        /// The quantifier is not allowed.
        /// </summary>
        [Exception(
            "The quantifier is not allowed.",
            "It is not allowed to use the quantifier (except d.AtLeastOne and d.None) when the relation between the subject and the predicate is many/one to one or when the argument of a predicate is a simple column expression.")]
        QuantifierDisallowed,

        /// <summary>
        /// The predicate is invalid.
        /// </summary>
        [Exception(
            "The predicate is invalid.",
            "The argument of a predicate must be a semantic sentence. If the query is used it must be converted into a subject using .As(synonym) method.")]
        InvalidPredicate,

        /// <summary>
        /// The .HavingMax/Min predicate has invalid argument.
        /// </summary>
        [Exception(
            "The .HavingMax/Min predicate has invalid argument.",
            "Note that only a simple column graph is allowed. Make sure that a the column graph table corresponds to the subject of the .HavingMax/Min predicate.")]
        InvalidHavingPredicateArgument,

        /// <summary>
        /// The .HavingMax/Min predicate has invalid position.
        /// </summary>
        [Exception(
            "The .HavingMax/Min predicate has invalid position.",
            "The .HavingMax/Min predicate has to be the last predicate of a given subject. Predicate grouping cannot be applied to the .HavingMax/Min predicate.")]
        InvalidHavingPredicatePosition,

        /// <summary>
        /// The predicate grouping is invalid.
        /// </summary>
        [Exception(
            "The predicate grouping is invalid.",
            "Make sure that every .Block method matches the corresponding .EndBlock method. Note that the nested grouping of predicates is not allowed.")]
        InvalidPredicateGrouping,

        /// <summary>
        /// The synonym is invalid.
        /// </summary>
        [Exception(
            "The synonym is invalid.",
            "1. A synonym must be a table." +
            "\r\n   2. When applying .UseAs method on a graph, a synonym table must exist in the graph." +
            "\r\n   3. When applying .UseAs method on a query, make sure the query consists of a single SELECT statement.")]
        InvalidSynonym,

        /// <summary>
        /// The logical operator Or has not been used properly.
        /// </summary>
        [Exception(
            "The logical operator Or has not been used properly.",
            "It is not allowed to place the Or operator before the first predicate or after the Not operator.")]
        InvalidOr,

        /// <summary>
        /// .GoAt method is invalid.
        /// </summary>
        [Exception(
            ".GoAt method is invalid.",
            "Use .GoAt method on the root node only.")]
        InvalidGoAt,

        /// <summary>
        /// Node or sentence is not allowed to be reused.
        /// </summary>
        [Exception(
            "Node or sentence is not allowed to be reused.",
            "Nodes or sentences are not intended to be reused as it causes the SEMQ query not to function properly. Storing the node or sentence reference into the variable is strongly discouraged.")]
        NodeReuseDisallowed,

        /// <summary>
        /// The designer root object is not allowed to be reused.
        /// </summary>
        [Exception(
            "The designer root object is not allowed to be reused.",
            "Storing the root reference into the reusable variable is strongly discouraged.")]
        RootReuseDisallowed,

        /// <summary>
        /// Function arguments are missing.
        /// </summary>
        [Exception(
            "Function arguments are missing.",
            "The function expects the arguments which have not been passed. Use the .Pass method to pass the arguments to the function."
            )]
        MissingFunctionArguments,

        /// <summary>
        /// The target table to join with is not found.
        /// </summary>
        [Exception(
            "The target table to join with is not found.",
            "The target table specified by the alias does not exist. When using the .By method, make sure that the specified alias points to the existing table in the query (positioned before the table to join). Note that the alias check is case-sensitive.")]
        JoinTargetNotFound,

        /// <summary>
        /// The target table to join with is not a mapped table.
        /// </summary>
        [Exception(
            "The target table to join with is not a mapped table.",
            "When using the .By method with the alias argument only, make sure that the target table is a mapped table.")]
        JoinTargetNotMapped,

        /// <summary>
        /// AutoJoin could not find the target table to join with.
        /// </summary>
        [Exception(
            "AutoJoin could not find the target table to join with.",
            "AutoJoin failed because the joining table is not related to any of the previous tables. Specify the aliased foreign key (using the .By method on the joining table) if there are multiple relations between the two tables.")]
        AutoJoinFailed,

        /// <summary>
        /// The join table is not valid.
        /// </summary>
        [Exception(
            "The join table is not valid.",
            "If the .By method is omitted, or it specifies the alias or the aliased foreign key, then both joining tables have to be mapped tables.")]
        InvalidJoinTable,

        /// <summary>
        /// Join is invalid.
        /// </summary>
        [Exception(
            "Join is invalid.",
            "The .Join method failed due to the use of the predicates. Note that the nodes along the graph can be joined only if no predicate is used.")]
        InvalidJoin,

        /// <summary>
        /// The specified column cannot be bound to the mapped object.
        /// </summary>
        [Exception(
            "The specified column cannot be bound to the mapped object.",
            "Please consider the following: " +
            "\r\n   - A table to bound with must be the mapped one. " +
            "\r\n   - When specifying an alias, make sure that there is a table that corresponds to the specified alias. Note that the alias check is case-sensitive. " +
            "\r\n   - When alias is not specified, make sure that there is a table that partially or entirely corresponds to the column graph. " +
            "\r\n   - A table corresponds partially to the column graph if its node corresponds to the first node in the column graph. Note that the rest of the nodes in the column graph that do not correspond to the table graph will be injected in the FROM clause. " +
            "\r\n   - A table corresponds entirely if there is a perfect match between the column graph and the table graph.")]
        ColumnNotBound,

        /// <summary>
        /// The specified column cannot be found.
        /// </summary>
        [Exception(
            "The specified column cannot be found.",
            "Provide a column that exists in the mapped object.")]
        ColumnNotFound,
        
        /// <summary>
        /// The column graph is not allowed.
        /// </summary>
        [Exception(
            "The column graph is not allowed.",
            "The column graph is allowed only in the .Select method."
            )]
        ColumnGraphDisallowed,

        /// <summary>
        /// The argument cannot be passed by the inner call.
        /// </summary>
        [Exception(
            "The argument cannot be passed by the inner call.",
            "It is not allowed to pass a table-valued argument to the stored procedure in the inner call. Pass the value using the variable.")]
        CannotInnerPass,

        /// <summary>
        /// The database action failed due to the concurrency violation.
        /// </summary>
        [Exception(
            "The database action failed due to the concurrency violation.",
            "An attempt to update, delete, or reload a row failed because it has been modified in the database after it was loaded by the application. " + 
            "\r\n   The concurrency violation issue can be resolved by reloading the row or by omitting the mirroring option.")]
        ConcurrencyViolation,

        /// <summary>
        /// The reload action failed.
        /// </summary>
        [Exception(
            "The reload action failed.",
            "An attempt to reload a row failed because a tie between the row in the database and the row in the application has been broken due to the modification of a row key in the database (PK, UK, or all columns if no PK or UK).")]
        ReloadFailed,

        /// <summary>
        /// The mirroring is invalid.
        /// </summary>
        [Exception(
            "The mirroring is invalid.",
            "This exception was thrown because the row is not updatable. In order to be able to force the mirroring the row must be loaded from the database in the first place.")]
        InvalidMirroring,

        /// <summary>
        /// The connection key is not allowed to have a negative value.
        /// </summary>
        [Exception(
            "The connection key is not allowed to have a negative value.",
            "Negative values are reserved for the database mapping. Provide a non-negative value. Use <db>.Map.ConnectionKey to refer to the database connection key.")]
        ConnectionKeyDisallowed,

        /// <summary>
        /// The connection string is not valid.
        /// </summary>
        [Exception(
            "The connection string is not valid.",
            "Provide a valid connection string.")]
        InvalidConnectionString,

        /// <summary>
        /// The .Await method failed.
        /// </summary>
        [Exception(
            "The .Await method failed.",
            "A connection timeout occured."
            )]
        Await1Failed,

        /// <summary>
        /// The .Await method failed.
        /// </summary>
        [Exception(
            "The .Await method failed.",
            "We are sorry but the synchronization cannot be done. Try to avoid using the .Await method inside the loop."
            )]
        Await2Failed,

        /// <summary>
        /// The asynchronous operation is invalid.
        /// </summary>
        [Exception(
            "The asynchronous operation is invalid.",
            "It is not allowed to use asynchronous methods inside the ambient transaction."
            )]
        InvalidAsyncOperation,

        /// <summary>
        /// The .Pack method is invalid.
        /// </summary>
        [Exception(
            "The .Pack method is invalid.",
            "The .Pack method cannot execute due to the invalid class type or non-existing property match.\r\n" +
            "   - Value types and interfaces cannot be used in the class conversion.\r\n" +
            "   - Every class should have at least one publicly accessible property.\r\n" +
            "   - Every property to convert must match in name and type.\r\n" +
            "   - There should be at least one match between the classes."
            )]
        InvalidPack,

        /// <summary>
        /// The properties used by the .Pack method do not match by type.
        /// </summary>
        [Exception(
            "The properties used by the .Pack method do not match by type.",
            "The two classes match by the propery name but not by their type. Make sure that all matching properties share the same type."
            )]
        PackPropertyMismatch,

        /// <summary>
        /// The conversion from JSON has failed.
        /// </summary>
        [Exception(
            "The conversion from JSON has failed.",
            "Make sure that the JSON string can be converted into the specified type."
            )]
        CannotConvertFromJson,

        /// <summary>
        /// The CRUD operation is not allowed.
        /// </summary>
        [Exception(
            "The CRUD operation is not allowed.",
            "This exception was thrown because a specified CRUD method was used with the data that do not belong to a table. Note that only data that belong to tables can be used in CRUD operations."
            )]
        InvalidCrudOperationException,

        /// <summary>
        /// The row operation is not allowed.
        /// </summary>
        [Exception(
            "The row operation is not allowed.",
            "This exception was thrown because a specified method was used with the newly created row which. Note that only rows that have been loaded from the database can be used in .ConnectBy and .With methods."
            )]
        NewRowException,

        /// <summary>
        /// The result is invalid.
        /// </summary>
        [Exception(
            "The result is invalid.",
            "This exception was thrown because the Result object contains a table that is not valid. Modifying the Result object strongly discouraged."
            )]
        InvalidResult,


        /// <summary>
        /// The name is reserved.
        /// </summary>
        [Exception(
            "The name is reserved.",
            "Ther specified name is reserved and cannot be used by the client."
            )]
        QueryTalkReservedName,

        /// <summary>
        /// The SQL operation is not allowed.
        /// </summary>
        [Exception(
            "The SQL operation is not allowed.",
            "This exception was thrown because a specific SQL data type (xml, text, ntext, image) was used in an operation that is not allowed by the SQL Server.\r\n" +
            "   If a table has a column with a specific SQL data type (xml, text, ntext, image):\r\n" +
            "   - do not use the optimistic concurrency check in .GoUpdate, .GoDelete, .GoUpdateRows, .GoDeleteRows methods,\r\n" +
            "   - do not use .GoReload method (which internally uses the optimistic concurrency check),\r\n" +
            "   - ensure that a table has a primary key or unique key.\r\n" +
            "   Note that you can use the optimistic concurrency check if a table has a rowversion (timestamp) column."
            )]
        InvalidSqlOperationException,
 
        /// <summary>
        /// CLR exception was thrown.
        /// </summary>
        [Exception(
            "CLR exception was thrown.",
            "An exception that was caught by QueryTalk has been thrown by the CLR. Please check the CLR exception."
            )]
        ClrException,

        /// <summary>
        /// CLR conversion has failed.
        /// </summary>
        [Exception(
            "CLR conversion has failed.",
            "Conversion from one CLR type to another has failed."
            )]
        ClrConversionFailed,

        /// <summary>
        /// The value is infinite.
        /// </summary>
        [Exception(
            "The value is infinite.",
            "Infinite values cannot be processed. Please provide a finite value."
            )]
        InfiniteValueException,
   
        /// <summary>
        /// Client assembly is null.
        /// </summary>
        [Exception(
            "Client assembly is null.", Text.Free.ReportExceptionToQueryTalk)]
        NullClientInnerException,

        /// <summary>
        /// Node cannot be found.
        /// </summary>
        [Exception(
            "Node cannot be found.", Text.Free.ReportExceptionToQueryTalk)]
        NodeNotFoundInnerException,

        /// <summary>
        /// Invoker cannot be found.
        /// </summary>
        [Exception(
            "Invoker cannot be found.", Text.Free.ReportExceptionToQueryTalk)]
        InvokerNotFoundInnerException,

        /// <summary>
        /// Column cannot be found.
        /// </summary>
        [Exception(
            "Column cannot be found.", Text.Free.ReportExceptionToQueryTalk)]
        ColumnNotFoundInnerException,

        /// <summary>
        /// Parameter cannot be found.
        /// </summary>
        [Exception(
            "Parameter cannot be found.", Text.Free.ReportExceptionToQueryTalk)]
        ParamNotFoundInnerException,

        /// <summary>
        /// XML parameter is invalid.
        /// </summary>
        [Exception(
            "XML parameter is invalid.", Text.Free.ReportExceptionToQueryTalk)]
        InvalidXmlParamInnerException,

        /// <summary>
        /// DeleteCascade has failed to build the execution code.
        /// </summary>
        [Exception(
            "DeleteCascade has failed to build the execution code.", Text.Free.ReportExceptionToQueryTalk)]
        DeleteCascadeInnerException,

        /// <summary>
        /// Expression operator is invalid.
        /// </summary>
        [Exception(
            "Expression operator is invalid.", Text.Free.ReportExceptionToQueryTalk)]
        InvalidExpressionOperatorInnerException,

        /// <summary>
        /// Executable is null.
        /// </summary>
        [Exception(
            "Executable is null.", Text.Free.ReportExceptionToQueryTalk)]
        NullExecutableInnerException,

        /// <summary>
        /// Argument is null.
        /// </summary>
        [Exception(
            "Argument is null.", Text.Free.ReportExceptionToQueryTalk)]
        NullArgumentInnerException,

        /// <summary>
        /// Root is invalid.
        /// </summary>
        [Exception(
            "Root is invalid.", Text.Free.ReportExceptionToQueryTalk)]
        InvalidRootInnerException,
    }
}
