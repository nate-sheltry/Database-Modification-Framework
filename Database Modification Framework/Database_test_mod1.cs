using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Database_Modification_Framework;

namespace Database_Test_Mod1
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    [BepInDependency(
        "tunguska.natesheltry.database_modification_framework.modplugin",
        BepInDependency.DependencyFlags.HardDependency)]
    public class Main : BaseUnityPlugin
    {
        //Plugin Information
        private const string MyGUID = "tunguska.natesheltry.database_test_mod1.mod";
        private const string PluginName = "Database Test Mod 1";
        private const string VersionString = "0.0.2";

        public void Awake()
        {
            this.Logger.LogMessage("Sending SQL");
            Framework.QueueSQL("NonRegional", @"UPDATE item_attributes SET weight = 10 WHERE id = 'huntingshotgun';");
            // Get Function for retrieving single item.
            var item = Database.DbNonRegional.GetItem("ammo762_39");
            // Log's item info to verify.
            Utils.Log.LogMessage(item.PrefabName);
        }
    }
}
