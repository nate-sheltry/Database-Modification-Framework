using BepInEx.Logging;
using Database_Modification_Framework.Definitions;
using Mono.Data.Sqlite;
using MonoMod.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Database_Modification_Framework
{
    public static class SqlExecutor
    {
        private static readonly ConcurrentDictionary<string, object> _dbLocks =
           new ConcurrentDictionary<string, object>();
        internal static Dictionary<string, List<ISQLItem>> incomingCommands = new Dictionary<string, List<ISQLItem>>{
            { Files.NonRegional, new List<ISQLItem>() },
            { Files.Main, new List<ISQLItem>() },
            { Files.Chinese, new List<ISQLItem>() },
            { Files.French, new List<ISQLItem>() },
            { Files.German, new List<ISQLItem>() },
            { Files.Italian, new List<ISQLItem>() },
            { Files.Russian, new List<ISQLItem>() },
            { Files.Spanish, new List<ISQLItem>() },
            { Files.Ukrainian, new List<ISQLItem>() },
        }
        ;
        internal static Dictionary<string, List<ISQLItem>> outgoingCommands = new Dictionary<string, List<ISQLItem>>{
            { Files.NonRegional, new List<ISQLItem>() },
            { Files.Main, new List<ISQLItem>() },
            { Files.Chinese, new List<ISQLItem>() },
            { Files.French, new List<ISQLItem>() },
            { Files.German, new List<ISQLItem>() },
            { Files.Italian, new List<ISQLItem>() },
            { Files.Russian, new List<ISQLItem>() },
            { Files.Spanish, new List<ISQLItem>() },
            { Files.Ukrainian, new List<ISQLItem>() },
        };
        private static object GetDbLock(string dbKey) =>
            _dbLocks.GetOrAdd(dbKey, _ => new object());
        // Framework handles the SQL error just fine if it's invalid.
        // Additionally, there could be an edge case in the future where this is valid but rare.
        private static bool CheckForLocDatabase(Enums.Databases database)
        {
            if (!Enums.LocalizationDatabases.Contains(database))
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Warning,
                    "Passed a non Localization database when attempting to retrieve localization data."
                    );
                return false;
            }
            return true;
        }
        internal static IDataReader GetAllLocSQLDataByTable(Enums.Databases database, string table)
        {
            CheckForLocDatabase(database);
            return GetAllSQLDataByTable(database, table);
        }
        internal static IDataReader GetAllSQLDataByTable(DatabaseTable database, string table)
        {
            IDbConnection connection = null;
            DatabaseManager.WorkingConnections.TryGetValue(database, out connection);
            if (connection is null)
            {
                // We are using a Fatal error here, since the user can only
                // recover from this by restarting.
                FrameworkUtils.InternalLog(
                    LogLevel.Fatal,
                    $"Database:{database} Connection does not exist."
                );
                return null;
            }
            if (connection?.State != ConnectionState.Open)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Warning,
                    $"Database: {connection.Database} - Connection was not opened, opening."
                );
                connection.Open();
                // infrequent connections which are opened here are closed
                // in the parent function -> Framework.SQLQueue.ProcessQueues
            }
            IDbCommand cmd = connection.CreateCommand();
            IDataReader reader = null;
            try
            {
                cmd.CommandText = $@"SELECT * FROM {table};";
                reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to get. {ex}"
                );
                return null;
            }
            finally
            {
                cmd?.Dispose();
            }
        }
        internal static IDataReader GetLocSQLDataByParam(Enums.Databases database, string param, object value, string table)
        {
            CheckForLocDatabase(database);
            return GetSQLDataByParam(database, param, value, table);
        }
        internal static IDataReader GetSQLDataByParam(DatabaseTable database, string param, object value, string table)
        {
            IDbConnection connection = null;
            DatabaseManager.WorkingConnections.TryGetValue(database, out connection);
            if (connection is null)
            {
                // We are using a Fatal error here, since the user can only
                // recover from this by restarting.
                FrameworkUtils.InternalLog(
                    LogLevel.Fatal,
                    $"Database:{database} Connection does not exist."
                );
                return null;
            }
            if (connection?.State != ConnectionState.Open)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Warning,
                    $"Database: {connection.Database} - Connection was not opened, opening."
                );
                connection.Open();
                // infrequent connections which are opened here are closed
                // in the parent function -> Framework.SQLQueue.ProcessQueues
            }
            IDbCommand cmd = connection.CreateCommand();
            IDataReader reader = null;
            try
            {
                if(value == null)
                {
                    cmd.CommandText = $@"SELECT * FROM {table} WHERE {param} IS NULL;";
                }
                else
                {
                    cmd.Parameters.Add(new SqliteParameter("value", value));
                    cmd.CommandText = $@"SELECT * FROM {table} WHERE {param} = @value;";
                }
                reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to retrieve Item data. {ex}"
                );
                return null;
            }
            finally
            {
                cmd?.Dispose();
            }
        }
        internal static IDataReader GetLocSQLDataByParams(Enums.Databases database, List<(string, object)> paramValues, string table)
        {
            CheckForLocDatabase(database);
            return GetSQLDataByParams(database, paramValues, table);
        }
        internal static IDataReader GetSQLDataByParams(DatabaseTable database, List<(string, object)> paramValues, string table)
        {
            IDbConnection connection = null;
            DatabaseManager.WorkingConnections.TryGetValue(database, out connection);
            if (connection is null)
            {
                // We are using a Fatal error here, since the user can only
                // recover from this by restarting.
                FrameworkUtils.InternalLog(
                    LogLevel.Fatal,
                    $"Database:{database} Connection does not exist."
                );
                return null;
            }
            if (connection?.State != ConnectionState.Open)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Warning,
                    $"Database: {database} - Connection was not opened, opening."
                );
                connection.Open();
                // infrequent connections which are opened here are closed
                // in the parent function -> Framework.SQLQueue.ProcessQueues
            }
            IDbCommand cmd = connection.CreateCommand();
            IDataReader reader = null;
            try
            {
                cmd.CommandText = $@"SELECT * FROM {table} WHERE";
                List<string> conditions = new List<string>();
                for(int i = 0; i < paramValues.Count;i++)
                {
                    if (paramValues[i].Item2 == null)
                    {
                        conditions.Add($@" {paramValues[i].Item1} IS NULL ");
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqliteParameter($"value{i}", paramValues[i].Item2));
                        conditions.Add($@" {paramValues[i].Item1} = @value{i} ");
                    }
                }
                cmd.CommandText += string.Join("AND", conditions) + ";";
                reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to retrieve Item data.\n{ex}"
                );
                return null;
            }
            finally
            {
                cmd?.Dispose();
            }
        }
        internal static T ReaderToItem<T>(IDataReader dataReader, Func<IDataReader, T> factory)
        where T : class
        {
            try
            {
                if (dataReader == null)
                    return null;
                if (dataReader.Read())
                    return factory(dataReader);
                return null;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to read in SQL data into {typeof(T).Name} object.\n{ex}"
                );
                return null;
            }
            finally
            {
                dataReader?.Dispose();
            }
        }
        internal static List<T> ReaderToItems<T>(IDataReader dataReader, Func<IDataReader, T> factory)
        where T : class
        {
            List<T> items = new List<T>();
            try
            {
                if (dataReader == null)
                    return items;
                while (dataReader.Read())
                {
                    items.Add(factory(dataReader));
                }
            } catch (Exception ex)
            {
                FrameworkUtils.InternalLog(LogLevel.Error,
                    $"Error occurred while reading in items.");
            }
            finally
            {
                dataReader?.Dispose();
            }
            return items;
        }
        internal static string GetSQLDataRaw(DatabaseTable database, string _sql)
        {
            IDbConnection connection = null;
            DatabaseManager.WorkingConnections.TryGetValue(database, out connection);
            if (connection is null)
            {
                // We are using a Fatal error here, since the user can only
                // recover from this by restarting.
                FrameworkUtils.InternalLog(
                    LogLevel.Fatal,
                    $"Database:{database} Connection does not exist."
                );
                return null;
            }
            if (connection?.State != ConnectionState.Open)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Warning,
                    $"Database: {connection.Database} - Connection was not opened, opening."
                );
                connection.Open();
                // infrequent connections which are opened here are closed
                // in the parent function -> Framework.SQLQueue.ProcessQueues
            }
            IDbCommand cmd = connection.CreateCommand();
            IDataReader reader = null;
            try
            {
                cmd.CommandText = _sql;
                reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetString(0);
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to retrieve Item data. {ex}"
                );
                return null;
            }
            finally
            {
                cmd?.Dispose();
            }
        }
        internal static bool IsConnectionBusy(IDbConnection conn)
        {
            try
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "BEGIN IMMEDIATE";
                cmd.CommandTimeout = 0;  // Fail instantly
                cmd.ExecuteNonQuery();
                cmd.CommandText = "ROLLBACK";  // Release test lock
                cmd.ExecuteNonQuery();
                FrameworkUtils.InternalLog(LogLevel.Message, $"Connection: {conn.Database} Not Busy");
                return false;  // Free
            }
            catch (SqliteException ex) when (ex.ErrorCode == SQLiteErrorCode.Busy || ex.ErrorCode == SQLiteErrorCode.Locked)
            {
                return true;  // SQLITE_BUSY or SQLITE_LOCKED
            }
        }

        internal static void ExecuteCommand(List<ISQLItem> items)
        {
            if (items?.Count == 0)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    "Database commands Failed to prepare to execute."
                );
                return;
            }
            foreach (ISQLItem item in items)
            {
                object dbLock = GetDbLock(item.Database);
                lock (dbLock)
                {

                    int rowsChanged = 0;
                    IDbConnection connection = null;
                    DatabaseManager.WorkingConnections.TryGetValue(item.Database, out connection);
                    if (connection is null)
                    {
                        FrameworkUtils.InternalLog(
                            BepInEx.Logging.LogLevel.Error,
                            $"Database:{item.Database} Connection does not exist."
                        );
                        continue;
                    }
                    if (connection?.State != ConnectionState.Open)
                    {
                        FrameworkUtils.InternalLog(
                            BepInEx.Logging.LogLevel.Warning,
                            $"Database: {item.Database} - Connection was not opened, opening."
                        );
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
                        command.CommandTimeout = 5;
                        command.Prepare();
                        rowsChanged = command.ExecuteNonQuery();
                        transaction.Commit();
                        incomingCommands[item.Database].Add(item);
                        command.Dispose();
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        FrameworkUtils.InternalLog(
                            BepInEx.Logging.LogLevel.Error,
                            $"Failed to execute SQLite Command for {item.Database} database.\n{ex}",
                            item.ModId
                        );
                    }
                    finally
                    {
                        transaction?.Dispose();
                        command?.Dispose();
                    }
                }
            }
        }
        internal static void UpdateGameDatabases()
        {
            foreach(string key in incomingCommands.Keys)
            {
                outgoingCommands[key].AddRange(incomingCommands[key]);
                incomingCommands[key].Clear();
            }
            foreach (string key in outgoingCommands.Keys)
            {
                int rowsChanged = 0;
                if (outgoingCommands[key].Count == 0)
                    continue;

                IDbConnection connection = null;
                DatabaseManager.WorkingConnections.TryGetValue(key, out connection);
                if (connection is null)
                {
                    FrameworkUtils.InternalLog(
                        BepInEx.Logging.LogLevel.Error,
                        $"Database:{key} Connection does not exist."
                    );
                    continue;
                }
                if (connection?.State != ConnectionState.Open)
                {
                    FrameworkUtils.InternalLog(
                        BepInEx.Logging.LogLevel.Warning,
                        $"Database: {key} - Connection was not opened, opening."
                    );
                    connection.Open();
                    // infrequent connections which are opened here are closed
                    // in the parent function -> Framework.SQLQueue.ProcessQueues
                }
                IDbTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction();
                    foreach(var item in outgoingCommands[key])
                    {
                        var command = item.GetSqlCommand();
                        try
                        {
                        
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandTimeout = 1;
                        rowsChanged += command.ExecuteNonQuery();
                        } catch (Exception ex)
                        {
                            throw;
                        }
                        finally
                        {
                            command?.Dispose();
                        }
                    }
                    transaction.Commit();
                    FrameworkUtils.ExternalLog(
                                BepInEx.Logging.LogLevel.Debug,
                                $"Game Database was Updated. Rows Changed: {rowsChanged}",
                                "Database Sync"
                            );
                    outgoingCommands[key].Clear();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    FrameworkUtils.ExternalLog(
                        BepInEx.Logging.LogLevel.Error,
                        $"Failed to execute SQLite Command for {key} database.\n{ex}",
                        "Database Sync"
                    );
                }
                finally
                {
                    transaction?.Dispose();
                }
            }
        }
    }
}
