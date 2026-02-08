using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Modification_Framework
{
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
