using Database_Modification_Framework.Definitions;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using static Database_Modification_Framework.Framework;

namespace Database_Modification_Framework
{

    internal static class DatabaseManager
    {
        private static Dictionary<string, IDbConnection> _connections = new Dictionary<string, IDbConnection>();
        // Use Hashset for Frequent Database list because we can afford
        // the overhead for setup and the reduce computation for lookup during runtime.
        public static readonly HashSet<string> freqDb = new HashSet<string>{
            Files.NonRegional,
        };
        public static IReadOnlyDictionary<string, IDbConnection> Connections => _connections;
        private static string GetDatabaseConnectionString(string key)
        {
            string dbPath = null;
            if (Utils.Databases is null)
            {
                Utils.Log.LogError($"Failed to intiialize databases");
                return null;
            }
            Utils.Databases.TryGetValue(key, out dbPath);
            if (dbPath == null)
            {
                Utils.Log.LogError($"Failed to find {key} database in Game Database Directory.");
                return null;
            }
            dbPath = Path.Combine(Directories.databaseDir, dbPath);
            Utils.Log.LogMessage(dbPath);
            return new SqliteConnectionStringBuilder { DataSource = dbPath }.ConnectionString;
        }
        private static IDbConnection EstablishConnection(string dbName)
        {
            string connectionString = GetDatabaseConnectionString(dbName);
            if (connectionString == null)
            {
                Utils.Log.LogError($"Failed to get connection string to {dbName} database.");
                return null;
            }
            try
            {
                Utils.Log.LogMessage(connectionString);
                IDbConnection dbConnection = new SqliteConnection(connectionString);
                return dbConnection;
            }
            catch (Exception ex)
            {
                Utils.Log.LogError($"Failed to open connection to {dbName} database. {ex.Message}");
                return null;
            }
        }
        public static void InitializeDb()
        {
            // Get all the database files
            List<string> otherDbs = Utils.Databases.Keys.ToList();
            // Go through our predetermined most often used databases
            foreach(string key in freqDb)
            {
                try
                {
                    _connections.Add(key, EstablishConnection(key));
                    // Establish a open connection and maintain it to reduce
                    // computational cost during runtime for some slight overhead.
                    _connections[key].Open();
                    // Removes thes from our all database files list.
                    otherDbs.Remove(key);
                }
                catch (Exception ex)
                {
                    Utils.Log.LogError($"Failed to open Database: {key}");
                }
            }
            // Now we create connection objects for the other databases.
            // However we will open them dynamically as needed.
            foreach (string key in otherDbs)
            {
                try
                {
                    _connections.Add(key, EstablishConnection(key));
                }
                catch (Exception ex)
                {
                    Utils.Log.LogError($"Failed to add Database: {key}");
                }
            }
            Utils.Log.LogMessage("Databases Finished Initializing.");
        }
        public static void CloseConnections()
        {
            {
                foreach (KeyValuePair<string, IDbConnection> kvp in Connections)
                {
                    try
                    {
                        if (kvp.Value == null) continue;
                        kvp.Value.Close();
                        kvp.Value.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Utils.Log.LogError($"Failed to close Database: {kvp.Key}");
                    }
                }
            }
        }

        private static int ExecuteSQL(IDbConnection connection, string sql)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            int changes = command.ExecuteNonQuery();
            command.Dispose();
            return changes;
        }
    }

    public class Database
    {
        //internal static string GetDatabaseConnectionString(string key)
        //{
        //    string dbPath;
        //    if(Utils.Databases == null)
        //    {
        //        Utils.Log.LogError($"Failed to intiialize databases");
        //        return null;
        //    }
        //    Utils.Databases.TryGetValue(key, out dbPath);
        //    if (dbPath == null)
        //    {
        //        Utils.Log.LogError($"Failed to find {key} database in Game Database Directory.");
        //        return null;
        //    }
        //    dbPath = Path.Combine(Definitions.Directories.databaseDir, dbPath);
        //    Utils.Log.LogMessage(dbPath);
        //    return $@"URI=file:{dbPath}";
        //}

        //internal static IDbConnection GetDatabase(string key)
        //{
        //    string connectionString = GetDatabaseConnectionString(key);
        //    if(connectionString == null)
        //    {
        //        Utils.Log.LogError($"Failed to get connection string to {key} database.");
        //        return null;
        //    }
        //    try
        //    {
        //        Utils.Log.LogMessage(connectionString);
        //        IDbConnection dbConnection = new SqlConnection(connectionString);
        //        dbConnection.Open();
        //        return dbConnection;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.Log.LogError($"Failed to open connection to {key} database. {ex.Message}");
        //        return null;
        //    }
        //}

        //public static void UpdateItemByID(string database, string query)
        //{
        //    Utils.Log.LogMessage($"SQL received {query}");
        //    IDbConnection connection = GetDatabase(database);

        //    IDbTransaction transaction = null;
        //    IDbCommand command = null;
        //    try
        //    {
        //        transaction = connection.BeginTransaction();
        //        command = connection.CreateCommand();
        //        command.CommandText = query;
        //        command.Transaction = transaction;

        //        int rowsAffected = command.ExecuteNonQuery();
        //        transaction.Commit();

        //        Utils.Log.LogMessage($"Rows Affected: {rowsAffected}");
        //    }
        //    catch(Exception ex)
        //    {
        //        transaction.Rollback();
        //        Utils.Log.LogError("Failed to modify SQLite Database.");
        //    }
        //    finally
        //    {
        //        transaction?.Dispose();
        //        command?.Dispose();
        //    }
        //    connection.Close();
        //}

        internal static void ExecuteCommand(List<ISQLItem> items)
        {
            if (items?.Count == 0)
            {
                Utils.Log.LogError("Database commands Failed to prepare to execute.");
                return;
            }
            foreach (ISQLItem item in items)
            {
                int rowsChanged = 0;
                IDbConnection connection = null;
                DatabaseManager.Connections.TryGetValue(item.Database, out connection);
                if(connection is null)
                {
                    Utils.Log.LogError($"Database:{item.Database} Connection does not exist.");
                    continue;
                }    
                if (connection?.State != ConnectionState.Open)
                {
                    Utils.Log.LogWarning($"Database: {connection.Database} - Connection was not opened, opening.");
                    connection.Open();
                    // infrequent connections which are opened here are closed
                    // in the parent function -> Framework.SQLQueue.ProcessQueues
                }
                IDbCommand command = item.GetSqlCommand();
                IDbTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    rowsChanged = command.ExecuteNonQuery();
                    Utils.Log.LogMessage($"Executed SQLite Command - Rows Changed: {rowsChanged}");
                    transaction.Commit();
                    command.Dispose();
                } catch (Exception ex)
                {
                    transaction?.Rollback();
                    Utils.Log.LogError($"Failed to execute SQLite Command\n {ex.Message}");
                }
                finally
                {
                    transaction?.Dispose();
                    command?.Dispose();
                }
            }
        }

        public static class DbNonRegional
        {
            public static NonRegionalItem GetItem(string Id)
            {
                return ReaderToItem(
                    GetSQLDataByParam(
                        Enums.NonRegionalItem.id.ToString(),
                        Id,
                        Enums.NonRegionalTables.item_attributes.ToString()
                    ),
                    r => new NonRegionalItem(r)
                );
            }
            public static List<NonRegionalItem> GetItems(Enums.NonRegionalItem field, string value)
            {
                return ReaderToItems(
                    GetSQLDataByParam(field.ToString(), value, 
                        Enums.NonRegionalTables.item_attributes.ToString()
                    ), 
                    r => new NonRegionalItem(r)
                );
            }
            public static SlaughterhouseEnemy GetEnemy(string Id)
            {
                return ReaderToItem(
                    GetSQLDataByParam(
                        Enums.NonRegionalEnemy.character_id.ToString(),
                        Id,
                        Enums.NonRegionalTables.slaughterhouse_enemies.ToString()
                    ),
                    r => new SlaughterhouseEnemy(r)
                );
            }
            public static List<SlaughterhouseEnemy> GetEnemies(Enums.NonRegionalEnemy field, string value)
            {
                return ReaderToItems(
                    GetSQLDataByParam(field.ToString(), value,
                        Enums.NonRegionalTables.slaughterhouse_enemies.ToString()
                    ),
                    r => new SlaughterhouseEnemy(r)
                );
            }
            public static SlaughterhouseProp GetProp(string Id)
            {
                return ReaderToItem(
                    GetSQLDataByParam(
                        Enums.NonRegionalProp.id.ToString(),
                        Id,
                        Enums.NonRegionalTables.slaughterhouse_props.ToString()
                    ),
                    r => new SlaughterhouseProp(r)
                );
            }
            public static List<SlaughterhouseProp> GetProps(Enums.NonRegionalProp field, string value)
            {
                return ReaderToItems(
                    GetSQLDataByParam(field.ToString(), value,
                        Enums.NonRegionalTables.slaughterhouse_props.ToString()
                    ),
                    r => new SlaughterhouseProp(r)
                );
            }
            private static T ReaderToItem<T>(IDataReader dataReader, Func<IDataReader, T> factory) where T : class
            {
                if (dataReader.Read())
                    return factory(dataReader);
                return null;
            }
            private static List<T> ReaderToItems<T>(IDataReader dataReader, Func<IDataReader, T> factory) where T : class
            {
                List<T> items = new List<T>();
                while(dataReader.Read())
                {
                    items.Add(factory(dataReader));
                }
                return items;
            }

            private static IDataReader GetSQLDataByParam(string param, object value, string table)
            {
                IDbConnection connection = null;
                DatabaseManager.Connections.TryGetValue(Files.NonRegional, out connection);
                if (connection is null)
                {
                    Utils.Log.LogError($"Database:{Files.NonRegional} Connection does not exist.");
                    return null;
                }
                IDbCommand cmd = connection.CreateCommand();
                IDataReader reader = null;
                try
                {
                    cmd.Parameters.Add(new SqliteParameter("value", value));
                    cmd.CommandText = $@"SELECT * FROM {table} WHERE {param} = @value";
                    reader = cmd.ExecuteReader();
                    return reader;
                }
                catch (Exception ex)
                {
                    Utils.Log.LogError($"Failed to retrieve Item data. {ex}");
                    return null;
                }
                finally
                {
                    cmd?.Dispose();
                }
            }
        }
    }
}
