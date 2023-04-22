using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxPowerGSTUpdateBulkReminderMails
{
    public class SQLDataAccess
    {

        public const string sqlConnectionString = @"Server=3.108.243.34,15435,4578;Database=TaxPowerCRM;User id=TaxPower; password=mTwQ459B%4z@lny;Trusted_Connection=False;MultipleActiveResultSets=true";
        //public const string sqlConnectionString = @"Data Source=.\TAXPOWER;Initial Catalog=TaxPowerCRM;Integrated Security=False;User Id=sa;Password=TaxPower123;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        //public const string sqlConnectionString = @"Data Source=.\TAXPOWER;Initial Catalog=TaxPowerCRM;Integrated Security=False;User Id=sa;Password=TaxPower123;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"; //For Local


        //SQL Command (ExecuteNonQuery)
        public static int SQLExecuteNonQuery(string commandText, Action<SqlCommand>? addParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(commandText, sqlConn))
                {
                    try
                    {
                        sqlConn.Open();
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandText = commandText;
                        sqlCmd.CommandTimeout = 0;

                        if (addParameters != null)
                        {
                            addParameters(sqlCmd);
                        }

                        return sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public static void SQLTransaction(string query, Action<SqlCommand>? addParameters)
        {
            SqlTransaction? sqlTran = null;

            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConn.Open();
                    sqlTran = sqlConn.BeginTransaction();
                    SqlCommand? sqlCmd = null;

                    if (!string.IsNullOrEmpty(query))
                    {
                        sqlCmd = new SqlCommand(query, sqlConn, sqlTran);
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandTimeout = 0;

                        if (addParameters != null)
                        {
                            addParameters(sqlCmd);
                        }

                        sqlCmd.ExecuteNonQuery();
                    }

                    sqlTran.Commit();

                }
                catch (Exception)
                {
                    if (sqlTran != null)
                    {
                        sqlTran.Rollback();
                    }

                    throw;
                }

            }
        }

        public static async Task SQLTransactionAsync(string query, Action<SqlCommand>? addParameters)
        {
            SqlTransaction? sqlTran = null;

            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConn.Open();
                    sqlTran = sqlConn.BeginTransaction();
                    SqlCommand? sqlCmd = null;

                    if (!string.IsNullOrEmpty(query))
                    {
                        sqlCmd = new SqlCommand(query, sqlConn, sqlTran);
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandTimeout = 0;

                        if (addParameters != null)
                        {
                            addParameters(sqlCmd);
                        }

                        await sqlCmd.ExecuteNonQueryAsync();
                    }

                    sqlTran.Commit();

                }
                catch (Exception)
                {
                    if (sqlTran != null)
                    {
                        sqlTran.Rollback();
                    }

                    throw;
                }

            }
        }

        //SQL Reader
        public static SqlDataReader SQLExecuteReader(string commandText, Action<SqlCommand>? AddParameters)
        {
            SqlConnection sqlConn = new SqlConnection(sqlConnectionString);

            using (SqlCommand sqlCmd = new SqlCommand(commandText, sqlConn))
            {
                try
                {
                    sqlConn.Open();

                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandTimeout = 0;

                    if (AddParameters != null)
                    {
                        AddParameters(sqlCmd);
                    }

                    return sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        //SQL ExecuteScalar
        public static object? SQLExecuteScalar(string commandText, Action<SqlCommand>? AddParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(commandText, sqlConn))
                {
                    try
                    {
                        sqlConn.Open();
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandText = commandText;
                        sqlCmd.CommandTimeout = 0;

                        if (AddParameters != null)
                        {
                            AddParameters(sqlCmd);
                        }

                        object? tempResult = null;
                        tempResult = sqlCmd.ExecuteScalar();

                        if (tempResult != null && !Convert.IsDBNull(tempResult))
                        {
                            return tempResult;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public static DataSet SQLFillDataSet(string commandText, Action<SqlCommand>? AddParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(commandText, sqlConn))
                {
                    using (DataSet dataset = new DataSet())
                    {
                        try
                        {
                            sqlConn.Open();
                            adapter.SelectCommand.CommandTimeout = 0;

                            if (AddParameters != null)
                            {
                                AddParameters(adapter.SelectCommand);
                            }

                            adapter.Fill(dataset);

                            return dataset;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        public static DataTable SQLFillDataTable(string commandText, Action<SqlCommand>? AddParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(commandText, sqlConn))
                {
                    using (DataTable table = new DataTable())
                    {
                        try
                        {
                            sqlConn.Open();
                            adapter.SelectCommand.CommandTimeout = 0;

                            if (AddParameters != null)
                            {
                                AddParameters(adapter.SelectCommand);
                            }

                            adapter.Fill(table);

                            return table;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        public static SqlDataAdapter SQLDataAdapter(string commandText, Action<SqlCommand>? AddParameters)
        {
            SqlConnection sqlConn = new SqlConnection(sqlConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(commandText, sqlConn);
            SqlCommandBuilder sqlCmdbl = new SqlCommandBuilder(adapter);

            try
            {
                sqlConn.Open();
                adapter.SelectCommand.CommandTimeout = 0;

                if (AddParameters != null)
                {
                    AddParameters(adapter.SelectCommand);
                }

                sqlConn.Close();

                return adapter;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static void SQLBulkCopyXML(string sourceXMLFile, string destinationTableName, Action<SqlBulkCopyColumnMappingCollection> AddColumns)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (DataSet ds = new DataSet())
                {
                    using (SqlBulkCopy bc = new SqlBulkCopy(sqlConn))
                    {
                        try
                        {
                            sqlConn.Open();

                            ds.ReadXml(sourceXMLFile);
                            DataTable dt = ds.Tables[0];

                            bc.DestinationTableName = destinationTableName;
                            if (AddColumns != null)
                            {
                                AddColumns(bc.ColumnMappings);
                            }

                            bc.WriteToServer(dt);

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        public static void SQLBulkCopyDataTable(DataTable dataTable, string destTableName, SqlBulkCopyOptions sqlBulkCopyOptions)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConn.Open();

                    using (SqlTransaction sqlTransaction = sqlConn.BeginTransaction())
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn, sqlBulkCopyOptions, sqlTransaction))
                        {
                            //Set the database table name
                            bulkCopy.DestinationTableName = destTableName;

                            //[OPTIONAL]: Map the DataTable columns with that of the database table
                            foreach (DataColumn c in dataTable.Columns)
                                bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);

                            try
                            {
                                bulkCopy.WriteToServerAsync(dataTable);
                                sqlTransaction.Commit();
                            }
                            catch (Exception)
                            {
                                sqlTransaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async static Task SQLBulkCopyDataTableAsync(DataTable dataTable, string destTableName, SqlBulkCopyOptions sqlBulkCopyOptions)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConn.Open();

                    using (SqlTransaction sqlTransaction = sqlConn.BeginTransaction())
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn, sqlBulkCopyOptions, sqlTransaction))
                        {
                            //Set the database table name
                            bulkCopy.DestinationTableName = destTableName;

                            //[OPTIONAL]: Map the DataTable columns with that of the database table
                            foreach (DataColumn c in dataTable.Columns)
                                bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);

                            try
                            {
                                await bulkCopy.WriteToServerAsync(dataTable);
                                sqlTransaction.Commit();
                            }
                            catch (Exception)
                            {
                                sqlTransaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static void UpdateGridData(string commandText, Action<SqlCommand> AddParameters, DataTable sqlDataTable)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(commandText, sqlConn))
                {
                    using (SqlCommandBuilder sqlCmd = new SqlCommandBuilder(adapter))
                    {
                        try
                        {
                            sqlConn.Open();
                            adapter.SelectCommand.CommandTimeout = 0;

                            if (AddParameters != null)
                            {
                                AddParameters(adapter.SelectCommand);
                            }


                            adapter.Update(sqlDataTable);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        public static async Task UpdateGridDataAsync(string commandText, Action<SqlCommand> AddParameters, DataTable sqlDataTable)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(commandText, sqlConn))
                {
                    using (SqlCommandBuilder sqlCmd = new SqlCommandBuilder(adapter))
                    {
                        try
                        {
                            sqlConn.Open();
                            adapter.SelectCommand.CommandTimeout = 0;

                            if (AddParameters != null)
                            {
                                AddParameters(adapter.SelectCommand);
                            }


                            await Task.Run(() => adapter.Update(sqlDataTable)); ;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        public static async Task<DataTable> SQLFillDataTableAsync(string commandText, Action<SqlCommand>? AddParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(commandText, sqlConn))
                {
                    using (DataTable table = new DataTable())
                    {
                        try
                        {
                            await sqlConn.OpenAsync();
                            adapter.SelectCommand.CommandTimeout = 0;

                            if (AddParameters != null)
                            {
                                AddParameters(adapter.SelectCommand);
                            }

                            await Task.Run(() => { adapter.Fill(table); });

                            return table;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }


        public static async Task<SqlDataReader> SQLExecuteReaderAsync(string commandText, Action<SqlCommand>? AddParameters)
        {
            SqlConnection sqlConn = new SqlConnection(sqlConnectionString);

            using (SqlCommand sqlCmd = new SqlCommand(commandText, sqlConn))
            {
                try
                {
                    await sqlConn.OpenAsync();

                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandTimeout = 0;

                    if (AddParameters != null)
                    {
                        AddParameters(sqlCmd);
                    }

                    return await sqlCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }


        public static async Task<int> SQLExecuteNonQueryAsync(string commandText, Action<SqlCommand> addParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(commandText, sqlConn))
                {
                    try
                    {
                        await sqlConn.OpenAsync();

                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandText = commandText;
                        sqlCmd.CommandTimeout = 0;

                        if (addParameters != null)
                        {
                            addParameters(sqlCmd);
                        }

                        return await sqlCmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public static async Task<int> SQLExecuteNonQueryChangeDatabaseAsync(string databaseName, string commandText, Action<SqlCommand> addParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(commandText, sqlConn))
                {
                    try
                    {
                        await sqlConn.OpenAsync();

                        sqlConn.ChangeDatabase(databaseName);

                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandText = commandText;
                        sqlCmd.CommandTimeout = 0;

                        if (addParameters != null)
                        {
                            addParameters(sqlCmd);
                        }

                        return await sqlCmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        //SQL ExecuteScalar
        public static async Task<object?> SQLExecuteScalarAsync(string commandText, Action<SqlCommand>? AddParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(commandText, sqlConn))
                {
                    try
                    {
                        await sqlConn.OpenAsync();
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandText = commandText;
                        sqlCmd.CommandTimeout = 0;

                        if (AddParameters != null)
                        {
                            AddParameters(sqlCmd);
                        }

                        object? tempResult = null;
                        tempResult = await sqlCmd.ExecuteScalarAsync();

                        if (tempResult != null && !Convert.IsDBNull(tempResult))
                        {
                            return tempResult;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }



        public static async Task<int> SQLExecuteStoredProcedureAsync(string databaseName, string storedProcedureName, Action<SqlCommand> addParameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(storedProcedureName, sqlConn))
                {
                    try
                    {
                        await sqlConn.OpenAsync();

                        sqlConn.ChangeDatabase(databaseName);

                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 0;

                        if (addParameters != null)
                        {
                            addParameters(sqlCmd);
                        }

                        return await sqlCmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }


    }
}
