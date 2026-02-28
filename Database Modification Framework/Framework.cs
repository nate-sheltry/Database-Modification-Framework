using BepInEx;
using BepInEx.Bootstrap;
using Database_Modification_Framework.Definitions;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Modification_Framework
{
    public static class Framework
    {
        public struct SQLItem
        {
            public SQLItem(string db, string sqlString)
            {
                database = db;
                sql = sqlString;
            }
            public string database { get; }
            public string sql { get; }
        }

        // Perhaps make a specialized container of queues
        // We'll sort SQL by databases in order to maximize performance.
        //public class SQLQueue
        //{
        //    public SQLQueue()
        //    {

        //    }
        //}

        public static event Action<SQLItem> SqlListener;
        public static event Action OnDbModified;
        public static Queue<SQLItem> SqlList = new Queue<SQLItem>();

        public static void QueueSQL(string database, string sql)
        {
            SqlList.Enqueue(
                new SQLItem(database, sql)
                );
        }

        public static void InvokeSQL()
        {
            if (SqlList.Count == 0) return;

            SqlListener?.Invoke(SqlList.Dequeue());
        }

        //public class Manager
        //{
        //    public void RegisterSQL(string modGUID, string id, string sql)
        //    {

        //    }

        //}

    }
}
