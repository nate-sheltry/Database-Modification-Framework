using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace Database_Modification_Framework
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    internal class Main : BaseUnityPlugin
    {
        //Plugin Information
        internal const string MyGUID = "tunguska.natesheltry.database_modification_framework.modplugin";
        private const string PluginName = "Database Modification Framework";
        private const string VersionString = "0.0.4";
        private float _syncTimer = 0f;
        const float SyncInterval = 2f;

        //Runs on Executable launch
        public void Awake()
        {
            //Set our Utility references
            FrameworkUtils.Log = this.Logger;
            FrameworkUtils.Instance = this;
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
        public void Start()
        {
            // Add game's current language to frequent database table.
            DatabaseManager.freqDb.Add($"Main{GameManager.Inst.Language}");
        }
        public void Update()
        {
            Framework.InvokeSQL();
            _syncTimer += Time.deltaTime;
            if(_syncTimer >= SyncInterval)
            {
                _syncTimer = 0f;
                SqlExecutor.UpdateGameDatabases();
            }
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
