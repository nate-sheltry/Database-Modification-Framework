using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using Unity.VisualScripting;
using BepInEx;
using BepInEx.Logging;
using Mono.Data.Sqlite;
using Unity.VisualScripting;

namespace Database_Modification_Framework
{
    //Static class for globally accessing instance plugin data.
    public static class Utils
    {
        public static ManualLogSource Log = null;
        public static Main Instance = null;
        public static IReadOnlyDictionary<string, string> Databases = null;
    }

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class Main : BaseUnityPlugin
    {
        //Plugin Information
        private const string MyGUID = "tunguska.natesheltry.database_modification_framework.modplugin";
        private const string PluginName = "Database Modification Framework";
        private const string VersionString = "0.0.1";

        private static Dictionary<string, string> _databases;

        //Runs on Executable launch
        public void Awake()
        {
            //Set our Utility references
            Utils.Log = Logger;
            Utils.Instance = this;
            _databases = Setup.initialize();
            Utils.Databases = _databases;
            //Subscribe to our listener for implementing SQL changes.
            Framework.SqlListener += SqlEvent;
        }

        //Code for running the SQL sent via mods.
        private void SqlEvent(Framework.SQLItem sqlItem)
        {
            Utils.Log.LogMessage("Framework performing SQL process.");
            Database.UpdateBaseItemByID(sqlItem.database, sqlItem.sql);
        }

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
