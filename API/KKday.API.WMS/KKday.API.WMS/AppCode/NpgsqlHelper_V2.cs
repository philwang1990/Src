using System;
using System.Data;
using System.Xml;
using Npgsql;
using System.Collections;

/// <summary>
/// Author:					Joseph Hill
/// Created:				2/16/2005
/// Last Modified:			2/16/2005
/// 
/// This is a helper class for Npgsql based on the functionality of the
/// MS Application Blocks for Data familiar to .NET developers
/// working with MS SQL
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 
/// </summary>
public sealed class NpgsqlHelper
{
    #region private utility methods & constructors

    // Since this class provides only static methods, make the default constructor private to prevent 
    // instances from being created with "new NpgsqlHelper()"
    private NpgsqlHelper() { }

    /// <summary>
    /// This method is used to attach array of NpgsqlParameters to a NpgsqlCommand.
    /// 
    /// This method will assign a value of DbNull to any parameter with a direction of
    /// InputOutput and a value of null.  
    /// 
    /// This behavior will prevent default values from being used, but
    /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
    /// where the user provided no input value.
    /// </summary>
    /// <param name="command">The command to which the parameters will be added</param>
    /// <param name="commandParameters">An array of NpgsqlParameters to be added to command</param>
    private static void AttachParameters(NpgsqlCommand command, NpgsqlParameter[] commandParameters)
    {
        if (command == null) throw new ArgumentNullException("command");
        if (commandParameters != null)
        {
            foreach (NpgsqlParameter p in commandParameters)
            {
                if (p != null)
                {
                    // Check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput ||
                        p.Direction == ParameterDirection.Input) &&
                        (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    command.Parameters.Add(p);
                }
            }
        }
    }

    /// <summary>
    /// This method assigns dataRow column values to an array of NpgsqlParameters
    /// </summary>
    /// <param name="commandParameters">Array of NpgsqlParameters to be assigned values</param>
    /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values</param>
    private static void AssignParameterValues(NpgsqlParameter[] commandParameters, DataRow dataRow)
    {
        if ((commandParameters == null) || (dataRow == null))
        {
            // Do nothing if we get no data
            return;
        }

        int i = 0;
        // Set the parameters values
        foreach (NpgsqlParameter commandParameter in commandParameters)
        {
            // Check the parameter name
            if (commandParameter.ParameterName == null ||
                commandParameter.ParameterName.Length <= 1)
                throw new Exception(
                    string.Format(
                    "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                    i, commandParameter.ParameterName));
            if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
            i++;
        }
    }

    /// <summary>
    /// This method assigns an array of values to an array of NpgsqlParameters
    /// </summary>
    /// <param name="commandParameters">Array of NpgsqlParameters to be assigned values</param>
    /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
    private static void AssignParameterValues(NpgsqlParameter[] commandParameters, object[] parameterValues)
    {
        if ((commandParameters == null) || (parameterValues == null))
        {
            // Do nothing if we get no data
            return;
        }

        // We must have the same number of values as we pave parameters to put them in
        if (commandParameters.Length != parameterValues.Length)
        {
            throw new ArgumentException("Parameter count does not match Parameter Value count.");
        }

        // Iterate through the NpgsqlParameters, assigning the values from the corresponding position in the 
        // value array
        for (int i = 0, j = commandParameters.Length; i < j; i++)
        {
            // If the current array value derives from IDbDataParameter, then assign its Value property
            if (parameterValues[i] is IDbDataParameter)
            {
                IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                if (paramInstance.Value == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = paramInstance.Value;
                }
            }
            else if (parameterValues[i] == null)
            {
                commandParameters[i].Value = DBNull.Value;
            }
            else
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }
    }

    /// <summary>
    /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
    /// to the provided command
    /// </summary>
    /// <param name="command">The NpgsqlCommand to be prepared</param>
    /// <param name="connection">A valid NpgsqlConnection, on which to execute this command</param>
    /// <param name="transaction">A valid NpgsqlTransaction, or 'null'</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of NpgsqlParameters to be associated with the command or 'null' if no parameters are required</param>
    /// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
    private static void PrepareCommand(
        NpgsqlCommand command,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        CommandType commandType,
        string commandText,
        NpgsqlParameter[] commandParameters,
        out bool mustCloseConnection)
    {
        if (command == null) throw new ArgumentNullException("command");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

        // If the provided connection is not open, we will open it
        if (connection.State != ConnectionState.Open)
        {
            mustCloseConnection = true;
            connection.Open();
        }
        else
        {
            mustCloseConnection = false;
        }

        // Associate the connection with the command
        command.Connection = connection;

        // Set the command text (stored procedure name or SQL statement)
        command.CommandText = commandText;

        // If we were provided a transaction, assign it
        if (transaction != null)
        {
            if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            command.Transaction = transaction;
        }

        // Set the command type
        command.CommandType = commandType;

        // Attach the command parameters if they are provided
        if (commandParameters != null)
        {
            AttachParameters(command, commandParameters);
        }
        return;
    }

    #endregion private utility methods & constructors

    #region ExecuteNonQuery

    /// <summary>
    /// Execute a NpgsqlCommand (that returns no resultset and takes no parameters) against the database specified in 
    /// the connection string
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>An int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteNonQuery(connectionString, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using the provided parameters
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

        // Create & open a NpgsqlConnection, and dispose of it after we are done
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Call the overload that takes a connection in place of the connection string
            return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
        }
    }
    

    /// <summary>
    /// Execute a NpgsqlCommand (that returns no resultset and takes no parameters) against the provided NpgsqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>An int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(NpgsqlConnection connection, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteNonQuery(connection, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns no resultset) against the specified NpgsqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(NpgsqlConnection connection, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, connection, (NpgsqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

        // Finally, execute the command
        int retval = cmd.ExecuteNonQuery();

        // Detach the NpgsqlParameters from the command object, so they can be used again
        cmd.Parameters.Clear();
        if (mustCloseConnection)
            connection.Close();
        return retval;
    }
    
    /// <summary>
    /// Execute a NpgsqlCommand (that returns no resultset and takes no parameters) against the provided NpgsqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>An int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(NpgsqlTransaction transaction, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteNonQuery(transaction, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns no resultset) against the specified NpgsqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(NpgsqlTransaction transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // Finally, execute the command
        int retval = cmd.ExecuteNonQuery();

        // Detach the NpgsqlParameters from the command object, so they can be used again
        cmd.Parameters.Clear();
        return retval;
    }

    #endregion ExecuteNonQuery

    #region ExecuteDataset

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>A dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteDataset(connectionString, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>A dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

        // Create & open a NpgsqlConnection, and dispose of it after we are done
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Call the overload that takes a connection in place of the connection string
            return ExecuteDataset(connection, commandType, commandText, commandParameters);
        }
    }
     

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the provided NpgsqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>A dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(NpgsqlConnection connection, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteDataset(connection, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// This overload added by Joe Audette 12/3/2003 to accomodate long running queries when needed
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connection">a valid NpgsqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandTimeout">time in seconds to allow before timing out</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>a dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(NpgsqlConnection connection, CommandType commandType, string commandText, int commandTimeout, 
        params NpgsqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, connection, (NpgsqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

        cmd.CommandTimeout = commandTimeout;

        // Create the DataAdapter & DataSet
        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
        {
            DataSet ds = new DataSet();

            // Fill the DataSet using default values for DataTable names, etc
            da.Fill(ds);

            // Detach the NpgsqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            // Return the dataset
            return ds;
        }
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>A dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(NpgsqlConnection connection, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, connection, (NpgsqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

        // Create the DataAdapter & DataSet
        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
        {
            DataSet ds = new DataSet();

            // Fill the DataSet using default values for DataTable names, etc
            da.Fill(ds);

            // Detach the NpgsqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            // Return the dataset
            return ds;
        }
    }    

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the provided NpgsqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>A dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(NpgsqlTransaction transaction, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteDataset(transaction, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>A dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteDataset(NpgsqlTransaction transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // Create the DataAdapter & DataSet
        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
        {
            DataSet ds = new DataSet();

            // Fill the DataSet using default values for DataTable names, etc
            da.Fill(ds);

            // Detach the NpgsqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            // Return the dataset
            return ds;
        }
    }

    #endregion ExecuteDataset

    #region ExecuteSpRefCursorDataset

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlConnection to access Stored Procedure
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>A dataset containing the resultset generated by the command</returns>
    public static DataSet ExecuteSpRefCursorDataset(NpgsqlConnection connection, string spName, params NpgsqlParameter[] commandParameters)
    {
        NpgsqlTransaction trans = connection.BeginTransaction();
        var ds = ExecuteSpRefCursorDataset(trans, spName, commandParameters);
        trans.Commit();
        return ds;
    }

    public static DataSet ExecuteSpRefCursorDataset(NpgsqlTransaction transaction, string spName, params NpgsqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, transaction.Connection, transaction, CommandType.StoredProcedure, spName, commandParameters, out mustCloseConnection);

        // Create the DataAdapter & DataSet
        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
        {
            DataSet ds = new DataSet();

            // Fill the DataSet using default values for DataTable names, etc
            da.Fill(ds);

            // Detach the NpgsqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            // ===============================================

            // FETCH ALL IN <unnamed portal 1>
            NpgsqlCommand command = transaction.Connection.CreateCommand();
            
            command.CommandText = string.Format("fetch all in \"{0}\"", ds.Tables[0].Rows[0][0]);
            command.CommandType = CommandType.Text;

            var _da = new NpgsqlDataAdapter(command);
            var _ds = new DataSet();

            // Fill the DataSet using default values for DataTable names, etc
            _da.Fill(_ds);

            // Detach the NpgsqlParameters from the command object, so they can be used again
            command.Parameters.Clear();

            // Return the dataset
            return _ds;
        }
    }

    #endregion ExecuteSpRefCursorDataset


    #region ExecuteReader

    /// <summary>
    /// This enum is used to indicate whether the connection was provided by the caller, or created by NpgsqlHelper, so that
    /// we can set the appropriate CommandBehavior when calling ExecuteReader()
    /// </summary>
    private enum NpgsqlConnectionOwnership
    {
        /// <summary>Connection is owned and managed by NpgsqlHelper</summary>
        Internal,
        /// <summary>Connection is owned and managed by the caller</summary>
        External
    }

    /// <summary>
    /// Create and prepare a NpgsqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
    /// </summary>
    /// <remarks>
    /// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
    /// 
    /// If the caller provided the connection, we want to leave it to them to manage.
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection, on which to execute this command</param>
    /// <param name="transaction">A valid NpgsqlTransaction, or 'null'</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of NpgsqlParameters to be associated with the command or 'null' if no parameters are required</param>
    /// <param name="connectionOwnership">Indicates whether the connection parameter was provided by the caller, or created by NpgsqlHelper</param>
    /// <returns>NpgsqlDataReader containing the results of the command</returns>
    private static NpgsqlDataReader ExecuteReader(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        CommandType commandType,
        string commandText,
        NpgsqlParameter[] commandParameters,
        NpgsqlConnectionOwnership connectionOwnership)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        bool mustCloseConnection = false;
        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();
        try
        {
            PrepareCommand(
                cmd,
                connection,
                transaction,
                commandType,
                commandText,
                commandParameters,
                out mustCloseConnection);

            // Create a reader
            NpgsqlDataReader dataReader;

            // Call ExecuteReader with the appropriate CommandBehavior
            if (connectionOwnership == NpgsqlConnectionOwnership.External)
            {
                dataReader = cmd.ExecuteReader();
            }
            else
            {
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            // Detach the NpgsqlParameters from the command object, so they can be used again.
            // HACK: There is a problem here, the output parameter values are fletched 
            // when the reader is closed, so if the parameters are detached from the command
            // then the SqlReader cant set its values. 
            // When this happen, the parameters cant be used again in other command.
            bool canClear = true;
            foreach (NpgsqlParameter commandParameter in cmd.Parameters)
            {
                if (commandParameter.Direction != ParameterDirection.Input)
                    canClear = false;
            }

            if (canClear)
            {
                cmd.Parameters.Clear();
            }

            return dataReader;
        }
        catch
        {
            if (mustCloseConnection)
                connection.Close();
            throw;
        }
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  NpgsqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>A NpgsqlDataReader containing the resultset generated by the command</returns>
    public static NpgsqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteReader(connectionString, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  NpgsqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>A NpgsqlDataReader containing the resultset generated by the command</returns>
    public static NpgsqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        NpgsqlConnection connection = null;
        try
        {
            connection = new NpgsqlConnection(connectionString);
            connection.Open();

            // Call the private overload that takes an internally owned connection in place of the connection string
            return ExecuteReader(connection, null, commandType, commandText, commandParameters, NpgsqlConnectionOwnership.Internal);
        }
        catch
        {
            // If we fail to return the SqlDatReader, we need to close the connection ourselves
            if (connection != null) connection.Close();
            throw;
        }

    }     

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the provided NpgsqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  NpgsqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>A NpgsqlDataReader containing the resultset generated by the command</returns>
    public static NpgsqlDataReader ExecuteReader(NpgsqlConnection connection, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteReader(connection, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  NpgsqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>A NpgsqlDataReader containing the resultset generated by the command</returns>
    public static NpgsqlDataReader ExecuteReader(NpgsqlConnection connection, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        // Pass through the call to the private overload using a null transaction value and an externally owned connection
        return ExecuteReader(connection, (NpgsqlTransaction)null, commandType, commandText, commandParameters, NpgsqlConnectionOwnership.External);
    }   

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the provided NpgsqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  NpgsqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>A NpgsqlDataReader containing the resultset generated by the command</returns>
    public static NpgsqlDataReader ExecuteReader(NpgsqlTransaction transaction, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteReader(transaction, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///   NpgsqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>A NpgsqlDataReader containing the resultset generated by the command</returns>
    public static NpgsqlDataReader ExecuteReader(NpgsqlTransaction transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        // Pass through to private overload, indicating that the connection is owned by the caller
        return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, NpgsqlConnectionOwnership.External);
    }
    
    #endregion ExecuteReader

    #region ExecuteScalar

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteScalar(connectionString, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        // Create & open a NpgsqlConnection, and dispose of it after we are done
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Call the overload that takes a connection in place of the connection string
            return ExecuteScalar(connection, commandType, commandText, commandParameters);
        }
    }
   

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided NpgsqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(NpgsqlConnection connection, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteScalar(connection, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a 1x1 resultset) against the specified NpgsqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(NpgsqlConnection connection, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");

        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();

        bool mustCloseConnection = false;
        PrepareCommand(cmd, connection, (NpgsqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

        // Execute the command & return the results
        object retval = cmd.ExecuteScalar();

        // Detach the NpgsqlParameters from the command object, so they can be used again
        cmd.Parameters.Clear();

        if (mustCloseConnection)
            connection.Close();

        return retval;
    }
    
    /// <summary>
    /// Execute a NpgsqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided NpgsqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(NpgsqlTransaction transaction, CommandType commandType, string commandText)
    {
        // Pass through the call providing null for the set of NpgsqlParameters
        return ExecuteScalar(transaction, commandType, commandText, (NpgsqlParameter[])null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a 1x1 resultset) against the specified NpgsqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
    public static object ExecuteScalar(NpgsqlTransaction transaction, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        // Create a command and prepare it for execution
        NpgsqlCommand cmd = new NpgsqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // Execute the command & return the results
        object retval = cmd.ExecuteScalar();

        // Detach the NpgsqlParameters from the command object, so they can be used again
        cmd.Parameters.Clear();
        return retval;
    }
    
    #endregion ExecuteScalar

    #region ExecuteXmlReader
    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the provided NpgsqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
    /// <returns>An XmlReader containing the resultset generated by the command</returns>
    //public static XmlReader ExecuteXmlReader(NpgsqlConnection connection, CommandType commandType, string commandText)
    //{
    //    // Pass through the call providing null for the set of NpgsqlParameters
    //    return ExecuteXmlReader(connection, commandType, commandText, (NpgsqlParameter[])null);
    //}

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command using "FOR XML AUTO"</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An XmlReader containing the resultset generated by the command</returns>
    //public static XmlReader ExecuteXmlReader(NpgsqlConnection connection, CommandType commandType, string commandText, params NpgsqlParameter[] commandParameters)
    //{
    //    if( connection == null ) throw new ArgumentNullException( "connection" );

    //    bool mustCloseConnection = false;
    //    // Create a command and prepare it for execution
    //    NpgsqlCommand cmd = new NpgsqlCommand();
    //    try
    //    {
    //        PrepareCommand(cmd, connection, (NpgsqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

    //        // Create the DataAdapter & DataSet
    //        throw new Exception("Npgsql.NpgsqlCommand does not contain a definition for XmlReaderExecuteXmlReader()");
    //        XmlReader retval = null;
    //        //XmlReader retval = cmd.ExecuteXmlReader();

    //        // Detach the NpgsqlParameters from the command object, so they can be used again
    //        cmd.Parameters.Clear();

    //        return retval;
    //    }
    //    catch
    //    {	
    //        if( mustCloseConnection )
    //            connection.Close();
    //        throw;
    //    }
    //}

    

    #endregion ExecuteXmlReader

    #region FillDataset
    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the database specified in 
    /// the connection string. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
    /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
    /// by a user defined name (probably the actual table name)</param>
    public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (dataSet == null) throw new ArgumentNullException("dataSet");

        // Create & open a NpgsqlConnection, and dispose of it after we are done
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Call the overload that takes a connection in place of the connection string
            FillDataset(connection, commandType, commandText, dataSet, tableNames);
        }
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
    /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
    /// by a user defined name (probably the actual table name)
    /// </param>
    public static void FillDataset(string connectionString, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params NpgsqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (dataSet == null) throw new ArgumentNullException("dataSet");
        // Create & open a NpgsqlConnection, and dispose of it after we are done
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Call the overload that takes a connection in place of the connection string
            FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
        }
    }

   
    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the provided NpgsqlConnection. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
    /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
    /// by a user defined name (probably the actual table name)
    /// </param>    
    public static void FillDataset(NpgsqlConnection connection, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames)
    {
        FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
    /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
    /// by a user defined name (probably the actual table name)
    /// </param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    public static void FillDataset(NpgsqlConnection connection, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params NpgsqlParameter[] commandParameters)
    {
        FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
    }    

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset and takes no parameters) against the provided NpgsqlTransaction. 
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
    /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
    /// by a user defined name (probably the actual table name)
    /// </param>
    public static void FillDataset(NpgsqlTransaction transaction, CommandType commandType,
        string commandText,
        DataSet dataSet, string[] tableNames)
    {
        FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
    }

    /// <summary>
    /// Execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlTransaction
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
    /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
    /// by a user defined name (probably the actual table name)
    /// </param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    public static void FillDataset(NpgsqlTransaction transaction, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params NpgsqlParameter[] commandParameters)
    {
        FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
    }
    

    /// <summary>
    /// Private helper method that execute a NpgsqlCommand (that returns a resultset) against the specified NpgsqlTransaction and NpgsqlConnection
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new NpgsqlParameter("prodid", 24));
    /// </remarks>
    /// <param name="connection">A valid NpgsqlConnection</param>
    /// <param name="transaction">A valid NpgsqlTransaction</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="dataSet">A dataset wich will contain the resultset generated by the command</param>
    /// <param name="tableNames">This array will be used to create table mappings allowing the DataTables to be referenced
    /// by a user defined name (probably the actual table name)
    /// </param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    private static void FillDataset(NpgsqlConnection connection, NpgsqlTransaction transaction, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params NpgsqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (dataSet == null) throw new ArgumentNullException("dataSet");

        // Create a command and prepare it for execution
        NpgsqlCommand command = new NpgsqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // Create the DataAdapter & DataSet
        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command))
        {

            // Add the table mappings specified by the user
            if (tableNames != null && tableNames.Length > 0)
            {
                string tableName = "Table";
                for (int index = 0; index < tableNames.Length; index++)
                {
                    if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                    dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                    tableName += (index + 1).ToString();
                }
            }

            // Fill the DataSet using default values for DataTable names, etc
            dataAdapter.Fill(dataSet);

            // Detach the NpgsqlParameters from the command object, so they can be used again
            command.Parameters.Clear();
        }

        if (mustCloseConnection)
            connection.Close();
    }
    #endregion

    #region UpdateDataset
    /// <summary>
    /// Executes the respective command for each inserted, updated, or deleted row in the DataSet.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
    /// </remarks>
    /// <param name="insertCommand">A valid transact-SQL statement or stored procedure to insert new records into the data source</param>
    /// <param name="deleteCommand">A valid transact-SQL statement or stored procedure to delete records from the data source</param>
    /// <param name="updateCommand">A valid transact-SQL statement or stored procedure used to update records in the data source</param>
    /// <param name="dataSet">The DataSet used to update the data source</param>
    /// <param name="tableName">The DataTable used to update the data source.</param>
    public static void UpdateDataset(NpgsqlCommand insertCommand, NpgsqlCommand deleteCommand, NpgsqlCommand updateCommand, DataSet dataSet, string tableName)
    {
        if (insertCommand == null) throw new ArgumentNullException("insertCommand");
        if (deleteCommand == null) throw new ArgumentNullException("deleteCommand");
        if (updateCommand == null) throw new ArgumentNullException("updateCommand");
        if (tableName == null || tableName.Length == 0) throw new ArgumentNullException("tableName");

        // Create a NpgsqlDataAdapter, and dispose of it after we are done
        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter())
        {
            // Set the data adapter commands
            dataAdapter.UpdateCommand = updateCommand;
            dataAdapter.InsertCommand = insertCommand;
            dataAdapter.DeleteCommand = deleteCommand;

            // Update the dataset changes in the data source
            dataAdapter.Update(dataSet, tableName);

            // Commit all the changes made to the DataSet
            dataSet.AcceptChanges();
        }
    }
    #endregion  
}

