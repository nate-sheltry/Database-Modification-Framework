using BepInEx;
using Database_Modification_Framework;
using Database_Modification_Framework.Definitions;
using System;

namespace Database_Test_Mod1
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    [BepInDependency(
        "tunguska.natesheltry.database_modification_framework.modplugin",
        BepInDependency.DependencyFlags.HardDependency)]
    public class Main : DatbaseModificationPlugin
    {
        //Plugin Information
        private const string MyGUID = "tunguska.natesheltry.database_test_mod1.mod";
        private const string PluginName = "Database Test Mod 1";
        private const string VersionString = "0.0.2";

        public void Awake()
        {
            FrameworkUtils.RegisterMod(MyGUID);
            this.Logger.LogMessage("Sending SQL");
            var shotgun = (NonRegionalItem)ReadInItem(Data.GetItem, "huntingshotgun");
            if (shotgun != null)
            {
                shotgun.Weight = 10;
                Utils.LogMessage($"ItemType: {shotgun.Type.ToString()}");
                shotgun.Sync();
            }
            var enemy = (SlaughterhouseEnemy)ReadInItem(Data.GetEnemy, "Mutant1");
            Utils.LogMessage($"Comments: {enemy.Comments}");
            enemy.Sync();
            var prop = (SlaughterhouseProp)ReadInItem(Data.GetProp, "ammo762_39");
            Utils.LogMessage($"price: {prop.price}");
            prop.Sync();
            // Get Function for retrieving single item.
            var item = Data.GetItem("ammo762_39");
            // Log's item info to verify.
            Utils.LogMessage(item.PrefabName);
        }

        public ISQLItem ReadInItem(Func<string, SQLItem> func, string id)
        {
            var item = func(id);
            if (item == null)
            {
                Utils.LogWarning($"Failed to get, {id}");
            }
            return item;
        }
    }
}
