using BepInEx.Logging;
using Database_Modification_Framework.Definitions;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Database_Modification_Framework
{
    public class SqlExecutor
    {
        internal static IDataReader GetSQLDataByParam(string database, string param, object value, string table)
        {
            IDbConnection connection = null;
            DatabaseManager.Connections.TryGetValue(database, out connection);
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
        internal static T ReaderToItem<T>(IDataReader dataReader, Func<IDataReader, T> factory)
        where T : class
        {
            if (dataReader.Read())
            {
                T item = factory(dataReader);
                dataReader.Dispose();
                return item;
            }
            return null;
        }
        internal static List<T> ReaderToItems<T>(IDataReader dataReader, Func<IDataReader, T> factory)
        where T : class
        {
            List<T> items = new List<T>();
            while (dataReader.Read())
            {
                items.Add(factory(dataReader));
            }
            dataReader.Dispose();
            return items;
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
                int rowsChanged = 0;
                IDbConnection connection = null;
                DatabaseManager.Connections.TryGetValue(item.Database, out connection);
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
                        $"Database: {connection.Database} - Connection was not opened, opening."
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
                    rowsChanged = command.ExecuteNonQuery();
                    FrameworkUtils.InternalLog(
                        BepInEx.Logging.LogLevel.Debug,
                        $"Executed SQLite Command - Rows Changed: {rowsChanged}",
                        item.ModId
                    );
                    transaction.Commit();
                    command.Dispose();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    FrameworkUtils.InternalLog(
                        BepInEx.Logging.LogLevel.Error,
                        $"Failed to execute SQLite Command\n{ex}",
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
}
