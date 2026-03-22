using BepInEx;
using Database_Modification_Framework;
using Database_Modification_Framework.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //Framework.QueueRawSQL(Enums.Databases.NonRegional, @"UPDATE item_attributes SET weight = 10 WHERE id = 'huntingshotgun';");
            var shotgun = (NonRegionalItem)ReadInItem(Database.DbNonRegional.GetItem, "huntingshotgun");
            if (shotgun != null)
            {
                shotgun.Weight = 10;
                Utils.Log.LogMessage($"ItemType: {shotgun.Type.ToString()}");
                shotgun.Sync();
            }
            var enemy = (SlaughterhouseEnemy)ReadInItem(Database.DbNonRegional.GetEnemy, "Mutant1");
            Utils.Log.LogMessage($"Comments: {enemy.Comments}");
            enemy.Sync();
            var prop = (SlaughterhouseProp)ReadInItem(Database.DbNonRegional.GetProp, "ammo762_39");
            Utils.Log.LogMessage($"price: {prop.price}");
            prop.Sync();
            // Get Function for retrieving single item.
            var item = Database.DbNonRegional.GetItem("ammo762_39");
            // Log's item info to verify.
            Utils.Log.LogMessage(item.PrefabName);
        }

        public ISQLItem ReadInItem(Func<string, SQLItem> func, string id)
        {
            var item = func(id);
            if (item == null)
            {
                Utils.Log.LogError($"Failed to get, {id}");
            }
            return item;
        }
    }
}
