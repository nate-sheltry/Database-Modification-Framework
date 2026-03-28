using BepInEx;

namespace Database_Modification_Framework
{
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
