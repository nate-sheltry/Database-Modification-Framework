using BepInEx;
using BepInEx.Bootstrap;
using Database_Modification_Framework.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Modification_Framework
{
    public static class Framework
    {
        public static event Action<string, string> SqlListener;
        public static event Action OnDbModified;

        public static void QueueSQL(string database, string sql)
        {
            SqlListener?.Invoke(database, sql);
        }

        //public class Manager
        //{
        //    public void RegisterSQL(string modGUID, string id, string sql)
        //    {

        //    }

        //}

    }
}
