using System.Collections.Generic;

namespace Database_Modification_Framework.Definitions
{
    public abstract partial class DatbaseModificationPlugin
    {
        public partial class DataClass
        {
            public class LocClass
            {
                internal DatbaseModificationPlugin root;
                internal DataClass parent;
                public LocClass(DataClass dc)
                {
                    root = dc.root;
                    parent = dc;
                }
                // Single Gets
                public  LocBaseItem GetBaseItem(string id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetBaseItem, id, db);
                public  LocConversation GetConverseration(int id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetConversation, id, db);
                public  LocEnvSounds GetEnvSoundset(
                    string location, string time, string weather, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetEnvSoundset, location, time, weather, db);
                public  LocFaction GetFaction(int id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetFaction, id, db);
                //Nice overload
                public  LocFaction GetFaction(Faction id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetFaction, (int)id, db);
                public  LocFactRel GetFactionRel(
                    int factionId, int targetFactionId, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetFactionRel,
                        factionId, targetFactionId, db);
                public  LocGlobalResp GetGlobalResp(string id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetGlobalResp, id, db);
                public  LocInitSquads GetInitialSquad(string id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetInitSquad, id, db);
                public  LocJrnlEntry GetJournalEntry(int id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetJounralEntry, id, db);
                public  LocLvlData GetLevelData(string id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetLevelData, id, db);
                public  LocNewsEntry GetNewsEntry(int id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetNewsEntry, id, db);
                public  LocNotePaper GetNotePaper(string id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetNotePaper, id, db);
                // Preset Character Table is empty, thus not being utilized.
                //public  LocPresetChar GetPresetChar(string id, Enums.Databases db) =>
                //    root.GetThing(Database.DbLocalization.GetPresetChar, id, db);
                public  LocRecipe GetRecipe(string serumId, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetRecipe, serumId, db);
                public  LocSkillPerks GetSkillPerkset(string skillId, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetSkillPerk, skillId, db);
                public  LocStash GetStash(string stashName, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetStash, stashName, db);
                public  LocStoryConItem GetStoryCondItem(string id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetStoryConItem, id, db);
                public  LocStoryConTrigger GetStoryCondTrigger(string id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetStoryConTrigger, id, db);
                public  LocTaskData GetTask(int id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetTaskData, id, db);
                public  LocTutorials GetTutorial(int id, Enums.Databases db) =>
                    root.GetThing(Database.DbLocalization.GetTutorial, id, db);

                // Multi Gets
                public  List<LocBaseItem> GetBaseItems(Enums.Databases db,
                    List<(Enums.LocBaseItems, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetBaseItems, db, values);
                public  List<LocConversation> GetConverserations(Enums.Databases db,
                    List<(Enums.LocConvos, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetConversations, db, values);
                public  List<LocEnvSounds> GetEnvSoundsets(
                    Enums.Databases db, List<(Enums.LocEnvSounds, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetEnvSoundsets, db, values);
                public  List<LocFaction> GetFactions(Enums.Databases db,
                    List<(Enums.LocFactions, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetFactions, db, values);
                public  List<LocFactRel> GetFactionRels(Enums.Databases db,
                    List<(Enums.LocFactionRel, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetFactionRelationships, db, values);
                public  List<LocGlobalResp> GetGlobalResponses(Enums.Databases db,
                    List<(Enums.LocGlobResp, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetGlobalResponses, db, values);
                public  List<LocInitSquads> GetInitialSquads(Enums.Databases db,
                    List<(Enums.LocInitSquads, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetInitSquads, db, values);
                public  List<LocJrnlEntry> GetJournalEntries(Enums.Databases db,
                    List<(Enums.LocJrnlEntries, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetJournalEntries, db, values);
                public  List<LocLvlData> GetLevelDatas(Enums.Databases db,
                    List<(Enums.LocLvlData, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetLevelDatas, db, values);
                public  List<LocNewsEntry> GetNewsEntries(Enums.Databases db,
                    List<(Enums.LocNewsEntries, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetNewsEntries, db, values);
                public  List<LocNotePaper> GetNotePapers(Enums.Databases db,
                    List<(Enums.LocNotePapers, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetNotePapers, db, values);
                //public  List<LocPresetChar> GetPresetChars(Enums.Databases db,
                //    List<(Enums.LocPresetChar, object)> values) =>
                //    root.GetThings(Database.DbLocalization.GetPresetChars, db, values);
                public  List<LocRecipe> GetRecipes(Enums.Databases db,
                    List<(Enums.LocRecipes, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetRecipes, db, values);
                public  List<LocSkillPerks> GetSkillPerksets(Enums.Databases db,
                    List<(Enums.LocSkillPerks, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetSkillPerks, db, values);
                public  List<LocStash> GetStashes(Enums.Databases db,
                    List<(Enums.LocStashes, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetStashes, db, values);
                public  List<LocStoryConItem> GetStoryCondItems(Enums.Databases db,
                    List<(Enums.LocStoryConItems, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetStoryConItems, db, values);
                public  List<LocStoryConTrigger> GetStoryCondTriggers(Enums.Databases db,
                    List<(Enums.LocStoryConTriggers, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetStoryConTriggers, db, values);
                public  List<LocTaskData> GetTasks(Enums.Databases db,
                    List<(Enums.LocTaskData, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetTaskDatas, db, values);
                public  List<LocTutorials> GetTutorials(Enums.Databases db,
                    List<(Enums.LocTutorials, object)> values) =>
                    root.GetThings(Database.DbLocalization.GetTutorials, db, values);
                // Get Everything
                public  List<LocBaseItem> GetAllBaseItems(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllBaseItems, db);
                public  List<LocConversation> GetAllConverserations(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllConversations, db);
                public  List<LocEnvSounds> GetAllEnvSoundsets(
                    Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllEnvSoundsets, db);
                public  List<LocFaction> GetAllFactions(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllFactions, db);
                public  List<LocFactRel> GetAllFactionRels(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllFactionRelationships, db);
                public  List<LocGlobalResp> GetAllGlobalResponses(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllGlobalResponses, db);
                public  List<LocInitSquads> GetAllInitialSquads(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllInitSquads, db);
                public  List<LocJrnlEntry> GetAllJournalEntries(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllJournalEntries, db);
                public  List<LocLvlData> GetAllLevelDatas(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllLevelDatas, db);
                public  List<LocNewsEntry> GetAllNewsEntries(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllNewsEntries, db);
                public  List<LocNotePaper> GetAllNotePapers(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllNotePapers, db);
                //public  List<LocPresetChar> GetPresetChars(Enums.Databases db,
                //    List<(Enums.LocPresetChar, object)> values) =>
                //    root.GetThings(Database.DbLocalization.GetPresetChars, db, values);
                public  List<LocRecipe> GetAllRecipes(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllRecipes, db);
                public  List<LocSkillPerks> GetAllSkillPerksets(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllSkillPerks, db);
                public  List<LocStash> GetAllStashes(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllStashes, db);
                public  List<LocStoryConItem> GetAllStoryCondItems(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllStoryConItems, db);
                public  List<LocStoryConTrigger> GetAllStoryCondTriggers(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllStoryConTriggers, db);
                public  List<LocTaskData> GetAllTasks(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllTaskDatas, db);
                public  List<LocTutorials> GetAllTutorials(Enums.Databases db) =>
                    root.GetThings(Database.DbLocalization.GetAllTutorials, db);
            }
        }
    }
}
