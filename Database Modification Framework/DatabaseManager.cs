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
        private static Dictionary<string, IDbConnection> _workingConnections = new Dictionary<string, IDbConnection>();
        private static Dictionary<string, IDbConnection> _gameConnections = new Dictionary<string, IDbConnection>();
        // Use Hashset for Frequent Database list because we can afford
        // the overhead for setup and the reduce computation for lookup during runtime.
        internal static HashSet<string> freqDb = new HashSet<string>{
            Files.NonRegional,
        };
        public static IReadOnlyDictionary<string, IDbConnection> WorkingConnections => _workingConnections;
        public static IReadOnlyDictionary<string, IDbConnection> GameConnections => _gameConnections;
        private static string GetDatabaseConnectionString(string key, string dir)
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
            dbPath = Path.Combine(dir, dbPath);
            FrameworkUtils.InternalLog(
                LogLevel.Info,
                dbPath
            );
            return new SqliteConnectionStringBuilder { DataSource = dbPath }.ConnectionString+";";
        }
        private static IDbConnection EstablishConnection(string dbName, string dir)
        {
            string connectionString = GetDatabaseConnectionString(dbName, dir);
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
                    _workingConnections.Add(key, EstablishConnection(key, Directories.workingDatabase));
                    _workingConnections[key].Open();

                    _gameConnections.Add(key, EstablishConnection(key, Directories.mainDatabase));
                    _gameConnections[key].Open();
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
                    _workingConnections.Add(key, EstablishConnection(key, Directories.workingDatabase));
                    _gameConnections.Add(key, EstablishConnection(key, Directories.mainDatabase));
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
                foreach (KeyValuePair<string, IDbConnection> kvp in WorkingConnections)
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
                foreach (KeyValuePair<string, IDbConnection> kvp in GameConnections)
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
