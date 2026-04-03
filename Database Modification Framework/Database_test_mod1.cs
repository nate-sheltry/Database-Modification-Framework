//using BepInEx;
//using Database_Modification_Framework;
//using Database_Modification_Framework.Definitions;
//using System;
//using System.Collections.Generic;

//namespace Database_Test_Mod1
//{
//    [BepInPlugin(MyGUID, PluginName, VersionString)]
//    [BepInDependency(
//        "tunguska.natesheltry.database_modification_framework.modplugin",
//        BepInDependency.DependencyFlags.HardDependency
//    )]
//    public class Main : DatbaseModificationPlugin
//    {
//        //Plugin Information
//        private const string MyGUID = "tunguska.natesheltry.database_test_mod1.mod";
//        private const string PluginName = "Database Test Mod 1";
//        private const string VersionString = "0.0.2";

//        public void Awake()
//        {
//            FrameworkUtils.RegisterMod(MyGUID);
//            this.Logger.LogMessage("Sending SQL");
//            var shotgun = (NonRegionalItem)ReadInItem(Data.NonReg.GetItem, "huntingshotgun");
//            if (shotgun != null)
//            {
//                shotgun.Weight = 10;
//                Utils.LogMessage($"ItemType: {shotgun.Type.ToString()}");
//                shotgun.Sync();
//            }
//            var enemy = (SlaughterhouseEnemy)ReadInItem(Data.NonReg.GetEnemy, "Mutant1");
//            Utils.LogMessage($"Comments: {enemy.Comments}");
//            enemy.Sync();
//            var prop = (SlaughterhouseProp)ReadInItem(Data.NonReg.GetProp, "ammo762_39");
//            Utils.LogMessage($"price: {prop.Price}");
//            prop.Sync();
//            // Get Function for retrieving single item.
//            var item = Data.NonReg.GetItem("ammo762_39");
//            // Log's item info to verify.
//            Utils.LogMessage(item.PrefabName);
//        }
//        public void NonRegionalTests()
//        {
//            NonRegSingleTest();
//            NonRegMultiTest();
//            NonRegAllTest();
//        }
//        public void NonRegSingleTest()
//        {
//            List<ISQLItem> items = new List<ISQLItem>();
//            items.Add(ReadInItem(Data.NonReg.GetItem, "huntingshotgun"));
//            items.Add(ReadInItem(Data.NonReg.GetEnemy, "Mutant1"));
//            items.Add(ReadInItem(Data.NonReg.GetProp, "ammo762_39"));
//            foreach(ISQLItem item in items)
//            {
//                TestNonRegItem(item);
//            }
//        }
//        public void NonRegMultiTest()
//        {

//        }
//        public void NonRegAllTest()
//        {
//            var items = Data.NonReg.GetAllItems();
//            foreach(ISQLItem item in items)
//                { TestNonRegItem(item); }
//            var enemies = Data.NonReg.GetAllEnemies();
//            foreach (ISQLItem enemy in enemies)
//                { TestNonRegItem(enemy); }
//            var props = Data.NonReg.GetAllProps();
//            foreach (ISQLItem prop in props)
//                { TestNonRegItem(prop); }
//        }

//        public void TestNonRegItem(ISQLItem item)
//        {
//            switch (item)
//            {
//                case NonRegionalItem nr:
//                    Utils.LogDebug("Successfully casted item as NonRegional Item.");
//                    return;
//                case SlaughterhouseEnemy enemy:
//                    Utils.LogDebug("Successfully casted item as Slaughterhouse Enemy.");
//                    return;
//                case SlaughterhouseProp prop:
//                    Utils.LogDebug("Successfully casted item as Slaughterhouse Prop.");
//                    return;
//                default:
//                    Utils.LogError("Failed to cast item to any NonRegional Type.");
//                    return;
//            }
//        }
//        public void TestLocSqlItem(ISQLItem item)
//        {
//            switch (item)
//            {
//                case LocBaseItem x:
//                    Utils.LogDebug("Successfully casted item as LocBaseItem.");
//                    return;
//                case LocGlobalResp x:
//                    Utils.LogDebug("Successfully casted item as LocGlobalResp .");
//                    return;
//                case LocJrnlEntry x:
//                    Utils.LogDebug("Successfully casted item as LocJrnlEntry.");
//                    return;
//                case LocFactRel x:
//                    Utils.LogDebug("Successfully casted item as LocFactRel.");
//                    return;
//                case LocStoryConTrigger x:
//                    Utils.LogDebug("Successfully casted item as LocStoryConTrigger .");
//                    return;
//                case LocNotePaper x:
//                    Utils.LogDebug("Successfully casted item as LocNotePaper.");
//                    return;
//                case LocEnvSounds x:
//                    Utils.LogDebug("Successfully casted item as LocEnvSounds.");
//                    return;
//                case LocTaskData x:
//                    Utils.LogDebug("Successfully casted item as LocTaskData .");
//                    return;
//                case LocInitSquads x:
//                    Utils.LogDebug("Successfully casted item as LocInitSquads.");
//                    return;
//                case LocPresetChar x:
//                    Utils.LogDebug("Successfully casted item as LocPresetChar.");
//                    return;
//                case LocConversation x:
//                    Utils.LogDebug("Successfully casted item as LocConversation .");
//                    return;
//                case LocNewsEntry x:
//                    Utils.LogDebug("Successfully casted item as LocNewsEntry.");
//                    return;
//                case LocStoryConItem x:
//                    Utils.LogDebug("Successfully casted item as LocStoryConItem .");
//                    return;
//                case LocStash x:
//                    Utils.LogDebug("Successfully casted item as LocStash.");
//                    return;
//                case LocRecipe x:
//                    Utils.LogDebug("Successfully casted item as LocRecipe.");
//                    return;
//                case LocSkillPerks x:
//                    Utils.LogDebug("Successfully casted item as LocSkillPerks .");
//                    return;
//                case LocFaction x:
//                    Utils.LogDebug("Successfully casted item as LocFaction.");
//                    return;
//                case LocLvlData x:
//                    Utils.LogDebug("Successfully casted item as LocLvlData .");
//                    return;
//                case LocTutorials x:
//                    Utils.LogDebug("Successfully casted item as LocTutorials.");
//                    return;
//                default:
//                    Utils.LogError("Failed to cast item to any NonRegional Type.");
//                    return;
//            }
//        }

//        public ISQLItem ReadInItem(Func<string, SQLItem> func, string id)
//        {
//            var item = func(id);
//            if (item == null)
//            {
//                Utils.LogWarning($"Failed to get, {id}");
//            }
//            return item;
//        }
//    }
//}
