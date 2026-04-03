using BepInEx.Logging;
using Database_Modification_Framework.Definitions;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

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
            if (FrameworkUtils.Databases is null)
            {
                // Fatal, because without Databases the framework cannot 
                // achieve any functionality.
                FrameworkUtils.InternalLog(
                    LogLevel.Fatal,
                    $"Failed to intiialize databases."
                );
                return null;
            }
            FrameworkUtils.Databases.TryGetValue(key, out dbPath);
            if (dbPath == null)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to find {key} database in Game Database Directory."
                );
                return null;
            }
            dbPath = Path.Combine(Directories.databaseDir, dbPath);
            FrameworkUtils.InternalLog(
                LogLevel.Info,
                dbPath
            );
            return new SqliteConnectionStringBuilder { DataSource = dbPath }.ConnectionString;
        }
        private static IDbConnection EstablishConnection(string dbName)
        {
            string connectionString = GetDatabaseConnectionString(dbName);
            if (connectionString == null)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to get connection string to {dbName} database."
                );
                return null;
            }
            try
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Debug,
                    connectionString
                );
                IDbConnection dbConnection = new SqliteConnection(connectionString);
                return dbConnection;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to open connection to {dbName} database. {ex.Message}"
                );
                return null;
            }
        }
        public static void InitializeDb()
        {
            // Get all the database files
            List<string> otherDbs = FrameworkUtils.Databases.Keys.ToList();
            // Go through our predetermined most often used databases
            foreach (string key in freqDb)
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
                    if (key == Files.NonRegional)
                        FrameworkUtils.InternalLog(
                            LogLevel.Fatal,
                            $"Failed to open Non Regional Database: {key}"
                        );
                    else
                        FrameworkUtils.InternalLog(
                            LogLevel.Error,
                            $"Failed to open Database: {key}"
                        );
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
                    FrameworkUtils.InternalLog(
                        LogLevel.Error,
                        $"Failed to add Database: {key}"
                    );
                }
            }
            FrameworkUtils.InternalLog(
                    LogLevel.Info,
                    "Databases Finished Initializing."
            );
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
                        FrameworkUtils.InternalLog(
                            LogLevel.Error,
                            $"Failed to close Database: {kvp.Key}"
                        );
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
}
