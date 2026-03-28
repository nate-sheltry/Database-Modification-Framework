using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using Database_Modification_Framework.Definitions;
using Mono.Data.Sqlite;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Modification_Framework
{
    public static class Framework
    {
        //public struct SQLItem
        //{
        //    public SQLItem(string db, string sqlString)
        //    {
        //        database = db;
        //        sql = sqlString;
        //    }
        //    public string database { get; }
        //    public string sql { get; }
        //    public string GetSQL;
        //}

        //Perhaps make a specialized container of queues
        //We'll sort SQL by databases in order to maximize performance.
        internal static class SQLQueue
        {
            private static Dictionary<string, Queue<ISQLItem>> dict = new Dictionary<string, Queue<ISQLItem>> { };
            private static long _count = 0;
            public static long Count { get => _count; }

            public static void Initialize()
            {
                foreach(string db in DatabaseManager.Connections.Keys)
                {
                    dict.Add(db, new Queue<ISQLItem>());
                }
                FrameworkUtils.InternalLog(
                    LogLevel.Message,
                    $"SQLQueue finished loading Keys: {dict.Keys}"
                );
            }

            public static void Enqueue(ISQLItem item)
            {
                try
                {
                    dict[item.Database].Enqueue(item);
                    _count += 1;
                }
                catch (Exception ex)
                {
                    FrameworkUtils.InternalLog(
                        LogLevel.Error, ex
                    );
                }
            }
            private static ISQLItem Dequeue(Queue<ISQLItem> queue)
            {
                // Although we could avoid using a variable for potential memory
                // savings. Doing things this way ensures our item was removed
                // from the queue before changing the count variable.
                ISQLItem item = queue.Dequeue();
                _count -= 1;
                return item;
            }
            public static void Enqueue(IEnumerable<ISQLItem> items)
            {
                foreach (ISQLItem item in items)
                {
                    try
                    {
                        dict[item.Database].Enqueue(item);
                        _count += 1;
                    }
                    catch (Exception ex)
                    {
                        FrameworkUtils.InternalLog(
                            LogLevel.Error, 
                            ex
                        );
                    }
                }
            }
            internal static void ProcessQueue()
            {
                List<ISQLItem> lists = new List<ISQLItem>();
                List<string> keys = dict.Keys.ToList();
                // Remove NonRegional from the List, because we want to prioritize
                // non-regional modifications.
                keys.Remove(Files.NonRegional);

                Queue<ISQLItem> nonRegional;
                while(lists.Count < FrameworkUtils.MAX_TX)
                {
                    // select non-regional queued items first for batched operations.
                    if (dict.TryGetValue(Files.NonRegional, out nonRegional))
                    {
                        while(nonRegional.Count > 0 && lists.Count < FrameworkUtils.MAX_TX)
                            lists.Add(Dequeue(nonRegional));
                    }
                    // select all other database items for batching.
                    foreach (string key in keys)
                    {
                        Queue<ISQLItem> queue;
                        if(!dict.TryGetValue(key, out queue))
                        {
                            FrameworkUtils.InternalLog(
                                LogLevel.Error, 
                                $"Failed to acces commands for {key} database."
                            );
                            continue;
                        }
                        while(queue.Count > 0 && lists.Count < FrameworkUtils.MAX_TX)
                            lists.Add(Dequeue(queue));
                    }
                    break;
                }
                SqlExecutor.ExecuteCommand(lists);
                // Infrequent connections are closed here to avoid
                // repeated and unnecessary database connection opens.
                if (Count < 1)
                {
                    foreach(string key in DatabaseManager.Connections.Keys.ToList())
                    {
                        if (!DatabaseManager.freqDb.Contains(key))
                            DatabaseManager.Connections[key].Close();
                    }
                }
            }
        }

        public static event Action SqlListener;
        public static void QueueSQL(ISQLItem item)
        {
            SQLQueue.Enqueue(item);
        }
        public static void QueueRawSQL(Enums.Databases database, string sql)
        {
            SQLQueue.Enqueue(
                new RawSQLItem(database, sql)
                );
        }

        public static void InvokeSQL()
        {
            if (SQLQueue.Count < 1) return;
            SqlListener?.Invoke();
        }

        public static void ExecuteQueuedCommands()
        {
            SQLQueue.ProcessQueue();
        }

    }
}
