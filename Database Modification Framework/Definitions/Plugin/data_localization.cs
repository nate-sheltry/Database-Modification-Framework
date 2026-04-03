using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Modification_Framework.Definitions
{
    public abstract partial class DatbaseModificationPlugin
    {
        public static partial class Data
        {
            public static class Loc
            {
                // Single Gets
                public static LocBaseItem GetBaseItem(string id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetBaseItem, id, db);
                public static LocConversation GetConverseration(int id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetConversation, id, db);
                public static LocEnvSounds GetEnvSoundset(
                    string location, string time, string weather, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetEnvSoundset, location, time, weather, db);
                public static LocFaction GetFaction(int id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetFaction, id, db);
                //Nice overload
                public static LocFaction GetFaction(Faction id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetFaction, (int)id, db);
                public static LocFactRel GetFactionRel(
                    int factionId, int targetFactionId, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetFactionRel,
                        factionId, targetFactionId, db);
                public static LocGlobalResp GetGlobalResp(string id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetGlobalResp, id, db);
                public static LocInitSquads GetInitialSquad(string id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetInitSquad, id, db);
                public static LocJrnlEntry GetJournalEntry(int id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetJounralEntry, id, db);
                public static LocLvlData GetLevelData(string id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetLevelData, id, db);
                public static LocNewsEntry GetNewsEntry(int id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetNewsEntry, id, db);
                public static LocNotePaper GetNotePaper(string id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetNotePaper, id, db);
                // Preset Character Table is empty, thus not being utilized.
                //public static LocPresetChar GetPresetChar(string id, Enums.Databases db) =>
                //    GetThing(Database.DbLocalization.GetPresetChar, id, db);
                public static LocRecipe GetRecipe(string serumId, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetRecipe, serumId, db);
                public static LocSkillPerks GetSkillPerkset(string skillId, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetSkillPerk, skillId, db);
                public static LocStash GetStash(string stashName, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetStash, stashName, db);
                public static LocStoryConItem GetStoryCondItem(string id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetStoryConItem, id, db);
                public static LocStoryConTrigger GetStoryCondTrigger(string id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetStoryConTrigger, id, db);
                public static LocTaskData GetTask(int id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetTaskData, id, db);
                public static LocTutorials GetTutorial(int id, Enums.Databases db) =>
                    GetThing(Database.DbLocalization.GetTutorial, id, db);

                // Multi Gets
                public static List<LocBaseItem> GetBaseItems(Enums.Databases db,
                    List<(Enums.LocBaseItems, object)> values) =>
                    GetThings(Database.DbLocalization.GetBaseItems, db, values);
                public static List<LocConversation> GetConverserations(Enums.Databases db,
                    List<(Enums.LocConvos, object)> values) =>
                    GetThings(Database.DbLocalization.GetConversations, db, values);
                public static List<LocEnvSounds> GetEnvSoundsets(
                    Enums.Databases db, List<(Enums.LocEnvSounds, object)> values) =>
                    GetThings(Database.DbLocalization.GetEnvSoundsets, db, values);
                public static List<LocFaction> GetFactions(Enums.Databases db,
                    List<(Enums.LocFactions, object)> values) =>
                    GetThings(Database.DbLocalization.GetFactions, db, values);
                public static List<LocFactRel> GetFactionRels(Enums.Databases db,
                    List<(Enums.LocFactionRel, object)> values) =>
                    GetThings(Database.DbLocalization.GetFactionRelationships, db, values);
                public static List<LocGlobalResp> GetGlobalResponses(Enums.Databases db,
                    List<(Enums.LocGlobResp, object)> values) =>
                    GetThings(Database.DbLocalization.GetGlobalResponses, db, values);
                public static List<LocInitSquads> GetInitialSquads(Enums.Databases db,
                    List<(Enums.LocInitSquads, object)> values) =>
                    GetThings(Database.DbLocalization.GetInitSquads, db, values);
                public static List<LocJrnlEntry> GetJournalEntries(Enums.Databases db,
                    List<(Enums.LocJrnlEntries, object)> values) =>
                    GetThings(Database.DbLocalization.GetJournalEntries, db, values);
                public static List<LocLvlData> GetLevelDatas(Enums.Databases db,
                    List<(Enums.LocLvlData, object)> values) =>
                    GetThings(Database.DbLocalization.GetLevelDatas, db, values);
                public static List<LocNewsEntry> GetNewsEntries(Enums.Databases db,
                    List<(Enums.LocNewsEntries, object)> values) =>
                    GetThings(Database.DbLocalization.GetNewsEntries, db, values);
                public static List<LocNotePaper> GetNotePapers(Enums.Databases db,
                    List<(Enums.LocNotePapers, object)> values) =>
                    GetThings(Database.DbLocalization.GetNotePapers, db, values);
                //public static List<LocPresetChar> GetPresetChars(Enums.Databases db,
                //    List<(Enums.LocPresetChar, object)> values) =>
                //    GetThings(Database.DbLocalization.GetPresetChars, db, values);
                public static List<LocRecipe> GetRecipes(Enums.Databases db,
                    List<(Enums.LocRecipes, object)> values) =>
                    GetThings(Database.DbLocalization.GetRecipes, db, values);
                public static List<LocSkillPerks> GetSkillPerksets(Enums.Databases db,
                    List<(Enums.LocSkillPerks, object)> values) =>
                    GetThings(Database.DbLocalization.GetSkillPerks, db, values);
                public static List<LocStash> GetStashes(Enums.Databases db,
                    List<(Enums.LocStashes, object)> values) =>
                    GetThings(Database.DbLocalization.GetStashes, db, values);
                public static List<LocStoryConItem> GetStoryCondItems(Enums.Databases db,
                    List<(Enums.LocStoryConItems, object)> values) =>
                    GetThings(Database.DbLocalization.GetStoryConItems, db, values);
                public static List<LocStoryConTrigger> GetStoryCondTriggers(Enums.Databases db,
                    List<(Enums.LocStoryConTriggers, object)> values) =>
                    GetThings(Database.DbLocalization.GetStoryConTriggers, db, values);
                public static List<LocTaskData> GetTasks(Enums.Databases db,
                    List<(Enums.LocTaskData, object)> values) =>
                    GetThings(Database.DbLocalization.GetTaskDatas, db, values);
                public static List<LocTutorials> GetTutorials(Enums.Databases db,
                    List<(Enums.LocTutorials, object)> values) =>
                    GetThings(Database.DbLocalization.GetTutorials, db, values);
                // Get Everything
                public static List<LocBaseItem> GetAllBaseItems(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllBaseItems, db);
                public static List<LocConversation> GetAllConverserations(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllConversations, db);
                public static List<LocEnvSounds> GetAllEnvSoundsets(
                    Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllEnvSoundsets, db);
                public static List<LocFaction> GetAllFactions(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllFactions, db);
                public static List<LocFactRel> GetAllFactionRels(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllFactionRelationships, db);
                public static List<LocGlobalResp> GetAllGlobalResponses(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllGlobalResponses, db);
                public static List<LocInitSquads> GetAllInitialSquads(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllInitSquads, db);
                public static List<LocJrnlEntry> GetAllJournalEntries(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllJournalEntries, db);
                public static List<LocLvlData> GetAllLevelDatas(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllLevelDatas, db);
                public static List<LocNewsEntry> GetAllNewsEntries(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllNewsEntries, db);
                public static List<LocNotePaper> GetAllNotePapers(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllNotePapers, db);
                //public static List<LocPresetChar> GetPresetChars(Enums.Databases db,
                //    List<(Enums.LocPresetChar, object)> values) =>
                //    GetThings(Database.DbLocalization.GetPresetChars, db, values);
                public static List<LocRecipe> GetAllRecipes(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllRecipes, db);
                public static List<LocSkillPerks> GetAllSkillPerksets(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllSkillPerks, db);
                public static List<LocStash> GetAllStashes(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllStashes, db);
                public static List<LocStoryConItem> GetAllStoryCondItems(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllStoryConItems, db);
                public static List<LocStoryConTrigger> GetAllStoryCondTriggers(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllStoryConTriggers, db);
                public static List<LocTaskData> GetAllTasks(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllTaskDatas, db);
                public static List<LocTutorials> GetAllTutorials(Enums.Databases db) =>
                    GetThings(Database.DbLocalization.GetAllTutorials, db);
            }
        }
    }
}
