using BepInEx;
using BepInEx.Logging;
using Database_Modification_Framework;
using Database_Modification_Framework.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Framework_Tester
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    [BepInDependency(
        "tunguska.natesheltry.database_modification_framework.modplugin",
        BepInDependency.DependencyFlags.HardDependency
    )]
    public partial class Main : DatbaseModificationPlugin
    {
        //Plugin Information
        private const string MyGUID = "tunguska.natesheltry.db_mod_framework_tester.mod";
        private const string PluginName = "Framework Tester";
        private const string VersionString = "0.0.5";
        internal bool? recoveryTest = null;
        internal bool?[] tests = new bool?[]{null, null, null};
        internal static bool Pass1 = false;
        internal static bool Pass2 = false;
        internal static bool Pass3 = false;


        public void Awake()
        {
            if (FrameworkUtils.GetLogLevel < 2)
                Utils.LogError("Framework Config must be Set to 2 or higher (2 or 3 is recommended) to view test results.");
            Utils.LogMessage("\n\nYou should see no Red (errors) while the test is running, except at the end.\n"+
                "Yellow are warnings and may be appropriate.\n"+
                "To fully verify the test was successful you must have your DebugLevel to 4 in the config\n"+
                "And have Info Logging enabled in BepInEx.");
            Thread.Sleep(10000);
        }
        public void Update()
        {
            if (tests[0] == false && Framework.CheckQueueAmount() == 0)
            {
                tests[0] = true;
            }
            if (tests[0] == null && Framework.CheckQueueAmount() == 0)
            {
                Pass1 = NonRegionalTests();
                if (!Pass1)
                    Utils.LogError($"Failed NonRegional Test On Get/Client side: {!Pass1}");
                else
                    Utils.LogMessage($"Passed NonRegional TestOn Get/Client side: {Pass1}");
                tests[0] = false;
            }
            if (tests[1] == false && Framework.CheckQueueAmount() == 0)
            {
                tests[1] = true;
            }
            if (tests[1] == null && Framework.CheckQueueAmount() == 0)
            {
                Pass2 = LocalizationTests();
                if (!Pass2)
                    Utils.LogError($"Failed Localization Test On Get/Client side: {!Pass2}");
                else
                    Utils.LogMessage($"Passed Localization TestOn Get/Client side: {Pass2}");
                tests[1] = false;
            }
            if (tests[2] == false && Framework.CheckQueueAmount() == 0)
            {
                tests[2] = true;
            }
            if (tests[2] == null && Framework.CheckQueueAmount() == 0)
            {
                Pass3 = NullTest();
                if (!Pass3)
                    Utils.LogError($"Failed Null Test On Get/Client side: {!Pass3}");
                else
                    Utils.LogMessage($"Passed Null TestOn Get/Client side: {Pass3}");
                tests[2] = false;
            }
            if (recoveryTest == false && Framework.CheckQueueAmount() == 0)
            {
                if ((bool)tests[0] && Pass1)
                    Utils.LogMessage("Non Regional Test: PASSED");
                if ((bool)tests[1] && Pass2)
                    Utils.LogMessage("Localization Test: PASSED");
                if ((bool)tests[2] && Pass3)
                    Utils.LogMessage("Null Test: PASSED");
                Utils.LogMessage($"Recovery Test: PASSED");
                recoveryTest = true;
            }
            if (recoveryTest == null && Framework.CheckQueueAmount() == 0)
            {
                Utils.LogMessage($"Running Recovery Tests - Errors will be Logged.\n"+
                    ">>> Should be 1 error followed by an Info Log.\n");
                RecoveryTests();
                recoveryTest = false;
            }
        }
        public bool NonRegionalTests()
        {
            return NonRegSingleTest() && NonRegMultiTest() && NonRegAllTest();
        }
        public bool NonRegSingleTest()
        {
            List<bool> conditions = new List<bool>();
            List<ISQLItem> items = new List<ISQLItem>();
            items.Add(ReadInItem(Data.NonReg.GetItem, "huntingshotgun"));
            items.Add(ReadInItem(Data.NonReg.GetEnemy, "Mutant1"));
            items.Add(ReadInItem(Data.NonReg.GetProp, "ammo762_39"));
            foreach (ISQLItem item in items)
            {
                conditions.Add(TestNonRegItem(item));
            }
            var item1 = Data.NonReg.GetItem("huntingshotgun");
            conditions.Add(item1 != null);
            var locitem = item1.GetLocalizationData(Enums.Databases.Main);
            conditions.Add(locitem != null);
            return !conditions.Contains(false);
        }
        public bool NonRegMultiTest()
        {
            List<bool> conditions = new List<bool>();
            var items = Data.NonReg.GetItems(new List<(Enums.NonRegItem, object)>{ 
                (Enums.NonRegItem.max_stack_size, 40),
                (Enums.NonRegItem.tier, 2)
            });
            foreach (ISQLItem item in items)
            { conditions.Add(TestNonRegItem(item)); }
            var enemies = Data.NonReg.GetEnemies(new List<(Enums.NonRegEnemy, object)>{
                (Enums.NonRegEnemy.tier, 2),
            });
            foreach (ISQLItem item in enemies)
            { conditions.Add(TestNonRegItem(item)); }
            var props = Data.NonReg.GetProps(new List<(Enums.NonRegProp, object)>{
                (Enums.NonRegProp.type, ItemType.Ammo.ToString()),
                (Enums.NonRegProp.challenge_level, 2),
                (Enums.NonRegProp.tier, 2),
            });
            foreach (ISQLItem item in enemies)
            { conditions.Add(TestNonRegItem(item)); }
            return !conditions.Contains(false);
        }
        public bool NonRegAllTest()
        {
            List<bool> conditions = new List<bool>();
            var items = Data.NonReg.GetAllItems();
            foreach (ISQLItem item in items)
            { conditions.Add(TestNonRegItem(item)); }
            var enemies = Data.NonReg.GetAllEnemies();
            foreach (ISQLItem enemy in enemies)
            { conditions.Add(TestNonRegItem(enemy)); }
            var props = Data.NonReg.GetAllProps();
            foreach (ISQLItem prop in props)
            { conditions.Add(TestNonRegItem(prop)); }
            return !conditions.Contains(false);
        }
        public bool LocalizationTests()
        {
            var conditions = new List<bool>();
            conditions.Add(LocSingleTest());
            conditions.Add(LocMultiTest());
            conditions.Add(LocAllTest());
            return !conditions.Contains(false);
        }
        public bool LocSingleTest()
        {
            var conditions = new List<bool>();
            conditions.Add(TestAllLocDbs(Data.Loc.GetBaseItem, "huntingshotgun"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetConverseration, 2));
            conditions.Add(TestAllLocDbs(Data.Loc.GetEnvSoundset, "Wilderness", "Day", "Clear"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetFactionRel, 0, 2));
            conditions.Add(TestAllLocDbs(Data.Loc.GetFaction, (int)Faction.Player));
            // Test the Enum overload.
            conditions.Add(Data.Loc.GetFaction(Faction.BorderPatrol, Enums.Databases.Main)!=null);
            conditions.Add(TestAllLocDbs(Data.Loc.GetGlobalResp, "response1"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetInitialSquad, "zsk_sidorovich"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetJournalEntry, 1));
            conditions.Add(TestAllLocDbs(Data.Loc.GetLevelData, "Village"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetNewsEntry, 1));
            conditions.Add(TestAllLocDbs(Data.Loc.GetNotePaper, "zsk_ivan"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetRecipe, "serum_hr1"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetSkillPerkset, "RifleHandling"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetStash, "MillStashHandle"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetStoryCondItem, "hastomatoseeds"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetStoryCondTrigger, "zsk_village_gate_open"));
            conditions.Add(TestAllLocDbs(Data.Loc.GetTask, 0));
            conditions.Add(TestAllLocDbs(Data.Loc.GetTutorial, 1));
            return !conditions.Contains(false);
        }
        public bool LocMultiTest()
        {
            var conditions = new List<bool>();
            conditions.Add(TestAllLocDbs(
                Data.Loc.GetBaseItems,
                new List<(Enums.LocBaseItems, object)>{(Enums.LocBaseItems.grid_cols, 2) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetConverserations, 
                new List<(Enums.LocConvos, object)> {(Enums.LocConvos.is_enabled, 1)}
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetEnvSoundsets,
                new List<(Enums.LocEnvSounds, object)> { (Enums.LocEnvSounds.location, "Wilderness") }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetFactionRels,
                new List<(Enums.LocFactionRel, object)> { (Enums.LocFactionRel.faction_id, (int)Faction.Animals) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetFactions,
                new List<(Enums.LocFactions, object)> { (Enums.LocFactions.character_type, 1) }
                ));

            conditions.Add(TestAllLocDbs(Data.Loc.GetGlobalResponses,
                new List<(Enums.LocGlobResp, object)> { (Enums.LocGlobResp.id, 10) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetInitialSquads,
                new List<(Enums.LocInitSquads, object)> { (Enums.LocInitSquads.tier, 2) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetJournalEntries,
                new List<(Enums.LocJrnlEntries, object)> { (Enums.LocJrnlEntries.id, 10) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetLevelDatas,
                new List<(Enums.LocLvlData, object)> { (Enums.LocLvlData.main_level_name, "Zernaskaya") }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetNewsEntries,
                new List<(Enums.LocNewsEntries, object)> { (Enums.LocNewsEntries.pay, 300) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetNotePapers,
                new List<(Enums.LocNotePapers, object)> { (Enums.LocNotePapers.id, "recipe_hr1") }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetRecipes,
                new List<(Enums.LocRecipes, object)> { (Enums.LocRecipes.quantity, 2) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetSkillPerksets,
                new List<(Enums.LocSkillPerks, object)> { (Enums.LocSkillPerks.skill_id, "Rifle") }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetStashes,
                new List<(Enums.LocStashes, object)> { (Enums.LocStashes.stash_name, "S11Stash2Handle") }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetStoryCondItems,
                new List<(Enums.LocStoryConItems, object)> { (Enums.LocStoryConItems.is_type, 1) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetStoryCondTriggers,
                new List<(Enums.LocStoryConTriggers, object)> { (Enums.LocStoryConTriggers.is_active, 0) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetTasks,
                new List<(Enums.LocTaskData, object)> { (Enums.LocTaskData.journal_id, -1) }
                ));
            conditions.Add(TestAllLocDbs(Data.Loc.GetTutorials,
                new List<(Enums.LocTutorials, object)> { (Enums.LocTutorials.id, 4) }
                ));
            return !conditions.Contains(false);
        }
        public bool LocAllTest()
        {
            var conditions = new List<bool>();
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllBaseItems));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllConverserations));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllEnvSoundsets));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllFactionRels));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllFactions));
            // Test the Enum overload.
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllGlobalResponses));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllInitialSquads));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllJournalEntries));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllLevelDatas));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllNewsEntries));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllNotePapers));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllRecipes));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllSkillPerksets));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllStashes));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllStoryCondItems));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllStoryCondTriggers));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllTasks));
            conditions.Add(TestAllLocDbs(Data.Loc.GetAllTutorials));
            return !conditions.Contains(false);
        }
        public bool NullTest()
        {
            List<bool> conditions = new List<bool>();
            conditions.Add(NullSingleTests());
            conditions.Add(NullMultiTests());
            return !conditions.Contains(false);
        }
        public bool NullSingleTests()
        {
            // Some functions cannot be null tested,
            // This is because their parameters are not nullable.
            // specifically the int parameters.
            List<bool> conditions = new List<bool>();
            conditions.Add(Data.NonReg.GetItem(null) == null);
            conditions.Add(Data.NonReg.GetEnemy(null) == null);
            conditions.Add(Data.NonReg.GetProp(null) == null);
            conditions.Add(Data.Loc.GetBaseItem(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetEnvSoundset(null, null, null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetGlobalResp(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetInitialSquad(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetLevelData(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetNotePaper(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetRecipe(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetSkillPerkset(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetStash(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetStoryCondItem(null, Enums.Databases.Main) == null);
            conditions.Add(Data.Loc.GetStoryCondTrigger(null, Enums.Databases.Main) == null);
            return !conditions.Contains(false);
        }
        public bool NullMultiTests()
        {
            // Some functions cannot be null tested,
            // This is because their parameters are not nullable.
            // specifically the int parameters.
            List<bool> conditions = new List<bool>();
            conditions.Add(Data.NonReg.GetItems(null).Count == 0);
            conditions.Add(Data.NonReg.GetEnemies(null).Count == 0);
            conditions.Add(Data.NonReg.GetProps(null).Count == 0);
            conditions.Add(Data.NonReg.GetItems(new List<(Enums.NonRegItem, object)>
                {(Enums.NonRegItem.global_attributes, null),
                 (Enums.NonRegItem.id, null)}
                ).Count == 0);
            conditions.Add(Data.NonReg.GetEnemies(new List<(Enums.NonRegEnemy, object)>
                {(Enums.NonRegEnemy.character_type, null)}
                ).Count == 0);
            conditions.Add(Data.NonReg.GetProps(new List<(Enums.NonRegProp, object)>
                {(Enums.NonRegProp.id, null),
                 (Enums.NonRegProp.type, null),
                 (Enums.NonRegProp.price, null)}
                ).Count == 0);
            conditions.Add(Data.Loc.GetBaseItems(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetEnvSoundsets(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetGlobalResponses(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetInitialSquads(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetLevelDatas(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetNotePapers(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetRecipes(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetSkillPerksets(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetStashes(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetStoryCondItems(Enums.Databases.Main, null).Count == 0);
            conditions.Add(Data.Loc.GetStoryCondTriggers(Enums.Databases.Main, null).Count == 0);
            return !conditions.Contains(false);
        }
        public bool RecoveryTests()
        {
            List<bool> conditions = new List<bool>();
            //Get Errors
                conditions.Add(Data.NonReg.GetProps(new List<(Enums.NonRegProp, object)>
                    { }
                    ).Count == 0);
            Utils.LogInfo("Get Error occurred, Flow not interrupted.");
                conditions.Add(Data.Loc.GetBaseItems(Enums.Databases.Main,
                    new List<(Enums.LocBaseItems, object)>{ }).Count == 0);
            Utils.LogInfo("Get Error occurred, Flow not interrupted.");
            // Wrong Database Test on LocItem
                conditions.Add(Data.Loc.GetBaseItems(Enums.Databases.NonRegional,
                    new List<(Enums.LocBaseItems, object)> { }).Count == 0);
            Utils.LogInfo("Get Error occurred, Flow not interrupted.");

            // Framework Execute Errors
                new RawSQLItem(Enums.Databases.NonRegional, "sdsdsdsdsd").Sync();
                new RawSQLItem(Enums.Databases.Main, "sdsdsdsdsd").Sync();
                new RawSQLItem(Enums.Databases.MainITA, "sdsdsdsdsd").Sync();

            // Proper Execute Queues (Should run after erroring)
            {            
                var item = Data.NonReg.GetProp("ammo762_39");
                conditions.Add(item != null);
                item.Sync();
            }
            {
                var item = Data.Loc.GetBaseItem("huntingshotgun", Enums.Databases.Main);
                conditions.Add(item != null);
                item.Sync();
            }
            {
                var item = Data.Loc.GetBaseItem("huntingshotgun", Enums.Databases.MainITA);
                conditions.Add(item != null);
                item.Sync();
            }
            return !conditions.Contains(false);
        }
    }
}
