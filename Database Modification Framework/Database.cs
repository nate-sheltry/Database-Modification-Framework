using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Modification_Framework.Definitions;
using Unity.VisualScripting;
using System.Collections.ObjectModel;

namespace Database_Modification_Framework
{

    internal static class DatabaseManager
    {
        private static Dictionary<string, IDbConnection> _connections = new Dictionary<string, IDbConnection>{ 
            { Files.Main , null },
            { Files.AI, null },
            { Files.NonRegional, null }
        };
        public static IReadOnlyDictionary<string, IDbConnection> Connections => _connections;
        private static string GetDatabaseConnectionString(string key)
        {
            string dbPath;
            if (Utils.Databases == null)
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
            return $@"URI=file:{dbPath}";
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
            foreach(KeyValuePair<string, IDbConnection> kvp in _connections)
            {
                try
                {
                    _connections[kvp.Key] = EstablishConnection(kvp.Key);
                    _connections[kvp.Key].Open();
                }
                catch (Exception ex)
                {
                    Utils.Log.LogError($"Failed to open Database: {kvp.Key}");
                }
            }
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

    internal class Database
    {
        internal static string GetDatabaseConnectionString(string key)
        {
            string dbPath;
            if(Utils.Databases == null)
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
            dbPath = Path.Combine(Definitions.Directories.databaseDir, dbPath);
            Utils.Log.LogMessage(dbPath);
            return $@"URI=file:{dbPath}";
        }

        internal static IDbConnection GetDatabase(string key)
        {
            string connectionString = GetDatabaseConnectionString(key);
            if(connectionString == null)
            {
                Utils.Log.LogError($"Failed to get connection string to {key} database.");
                return null;
            }
            try
            {
                Utils.Log.LogMessage(connectionString);
                IDbConnection dbConnection = new SqliteConnection(connectionString);
                dbConnection.Open();
                return dbConnection;
            }
            catch (Exception ex)
            {
                Utils.Log.LogError($"Failed to open connection to {key} database. {ex.Message}");
                return null;
            }
        }

        public static void UpdateBaseItemByID(string database, string query)
        {
            Utils.Log.LogMessage($"SQL received {query}");
            IDbConnection connection = GetDatabase(database);

            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Transaction = transaction;

                int rowsAffected = command.ExecuteNonQuery();
                transaction.Commit();

                Utils.Log.LogMessage($"Rows Affected: {rowsAffected}");
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                Utils.Log.LogError("Failed to modify SQL Database.");
            }
            connection.Close();
        }
    }
}
