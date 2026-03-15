using BepInEx;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Logging;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting;
using UnityEngine;
using static Database_Modification_Framework.Framework;

namespace Database_Modification_Framework
{
    //Static class for globally accessing instance plugin data.
    public static class Utils
    {
        public static ManualLogSource Log = null;
        public static Main Instance = null;
        public static IReadOnlyDictionary<string, string> Databases = null;
        private static int maxTx = 5;
        public static int MAX_TX { get => maxTx; }
        public static void DetermineMaxTX()
        {
            // lp is for Logical Processors, aka threads.
            int lp = SystemInfo.processorCount;
            maxTx = lp <= 2 ? 1 :
                    lp <= 4 ? 3 :
                    lp <= 6 ? 5 :
                    lp <= 8 ? 8 :
                    lp <= 12 ? 10 :
                    lp <= 16 ? 15 :
                    20;                          
        }
    }

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class Main : BaseUnityPlugin
    {
        //Plugin Information
        private const string MyGUID = "tunguska.natesheltry.database_modification_framework.modplugin";
        private const string PluginName = "Database Modification Framework";
        private const string VersionString = "0.0.1";

        //Runs on Executable launch
        public void Awake()
        {
            //Set our Utility references
            Utils.Log = Logger;
            Utils.Instance = this;
            Setup.initialize();
            //Subscribe to our listener for implementing SQL changes.
            Framework.SqlListener += Framework.SQLQueue.ProcessQueue;
        }

        //Code for running the SQL sent via mods.
        //private void SqlEvent(Framework.SQLItem sqlItem)
        //{
        //    Utils.Log.LogMessage("Framework performing SQL process.");
        //    Database.UpdateItemByID(sqlItem.Database, sqlItem.GetSQLString());
        //}

        public void Update()
        {
            Framework.InvokeSQL();
        }

        //Runs on Executable's exit/close
        public void OnApplicationQuit()
        {
            Setup.exit();
        }

        //Function for testing plugin.
        public void Testing()
        {

        }
    }
}
