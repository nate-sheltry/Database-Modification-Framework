using BepInEx.Logging;
using Database_Modification_Framework.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database_Modification_Framework
{
    public static class Framework
    {
        private static List<bool> hasErroredThisCycle = new List<bool>();
        public static bool AnyErrorThisCycle { get {
                bool temp = !hasErroredThisCycle.TrueForAll(x => x == false);
                hasErroredThisCycle.Clear();
                return temp;
            } }

        //Perhaps make a specialized container of queues
        //We'll sort SQL by databases in order to maximize performance.
        public static long CheckQueueAmount()
        {
            return SQLQueue.Count;
        }
        internal static class SQLQueue
        {
            private static Dictionary<string, Queue<ISQLItem>> dict = new Dictionary<string, Queue<ISQLItem>> { };
            private static long _count = 0;
            public static long Count { get => _count; }

            public static void Initialize()
            {
                foreach(string db in DatabaseManager.WorkingConnections.Keys)
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
                        LogLevel.Error, $"Failed to add item to Queue.\n{ex}"
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
                            $"Failed to add item to Queue.\n{ex}"
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
                bool error = false;
                while (lists.Count < FrameworkUtils.MAX_TX)
                {
                    // select non-regional queued items first for queued operations.
                    if (dict.TryGetValue(Files.NonRegional, out nonRegional))
                    {
                        //IDbConnection con = null;
                        //DatabaseManager.Connections.TryGetValue(Enums.Databases.NonRegional.ToString(), out con);
                        //con.
                        while(nonRegional.Count > 0 && lists.Count < FrameworkUtils.MAX_TX)
                            lists.Add(Dequeue(nonRegional));
                    }
                    // select all other database items for queueing.
                    foreach (string key in keys)
                    {
                        Queue<ISQLItem> queue;
                        if(!dict.TryGetValue(key, out queue))
                        {
                            error = true;
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
                try 
                {
                    if (!SqlExecutor.ExecuteCommand(lists))
                        error = true;
                } catch { error = true; }
                // Infrequent connections are closed here to avoid
                // repeated and unnecessary database connection opens.
                if (Count < 1)
                {
                    FrameworkUtils.InternalLog(
                        BepInEx.Logging.LogLevel.Info,
                        $"Finished modifying working database."
                    );
                    foreach (string key in DatabaseManager.WorkingConnections.Keys.ToList())
                    {
                        if (!DatabaseManager.freqDb.Contains(key))
                            DatabaseManager.WorkingConnections[key].Close();
                    }
                }
                hasErroredThisCycle.Add(error);
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
