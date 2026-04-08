using Database_Modification_Framework.Definitions;
using System.Collections.Generic;
using static Database_Modification_Framework.Framework;
using static Database_Modification_Framework.SqlExecutor;

namespace Database_Modification_Framework.Database
{
    public static partial class DbLocalization
    {
        // Single Functions
        public static LocBaseItem GetBaseItem(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocBaseItems.id.ToString(),
                    id,
                    Enums.LocalizationTables.base_items.ToString()
                ),
                r => new LocBaseItem(r, database)
            );
        }
        public static LocGlobalResp GetGlobalResp(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocGlobResp.id.ToString(),
                    id,
                    Enums.LocalizationTables.global_dialogue_response.ToString()
                ),
                r => new LocGlobalResp(r, database)
            );
        }
        public static LocJrnlEntry GetJounralEntry(int id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocJrnlEntries.id.ToString(),
                    id,
                    Enums.LocalizationTables.journal_entries.ToString()
                ),
                r => new LocJrnlEntry(r, database)
            );
        }
        public static LocFactRel GetFactionRel(int factionId, int targetFactId, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParams(
                    database, new List<(string, object)>{
                        (Enums.LocFactionRel.faction_id.ToString(), factionId),
                        (Enums.LocFactionRel.target_faction_id.ToString(), targetFactId)
                    },
                    Enums.LocalizationTables.faction_relationships.ToString()
                ),
                r => new LocFactRel(r, database)
            );
        }
        public static LocStoryConTrigger GetStoryConTrigger(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocStoryConTriggers.id.ToString(), id,
                    Enums.LocalizationTables.story_conditions_trigger.ToString()
                ),
                r => new LocStoryConTrigger(r, database)
            );
        }
        public static LocNotePaper GetNotePaper(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocNotePapers.id.ToString(), id,
                    Enums.LocalizationTables.note_papers.ToString()
                ),
                r => new LocNotePaper(r, database)
            );
        }
        public static LocEnvSounds GetEnvSoundset(string location, string time, 
            string weather, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParams(
                    database, new List<(string, object)>{
                    (Enums.LocEnvSounds.location.ToString(), location),
                    (Enums.LocEnvSounds.time.ToString(), time),
                    (Enums.LocEnvSounds.weather.ToString(), weather),
                    },
                    Enums.LocalizationTables.environment_sounds.ToString()
                ),
                r => new LocEnvSounds(r, database)
            );
        }
        public static LocTaskData GetTaskData(int id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocTaskData.id.ToString(), id,
                    Enums.LocalizationTables.task_data.ToString()
                ),
                r => new LocTaskData(r, database)
            );
        }
        public static LocInitSquads GetInitSquad(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocInitSquads.id.ToString(), id,
                    Enums.LocalizationTables.initial_squads.ToString()
                ),
                r => new LocInitSquads(r, database)
            );
        }
        // Preset Table is empty
        //public static LocPresetChar GetPresetChar(string id, Enums.Databases database)
        //{
        //    return ReaderToItem(
        //        GetLocSQLDataByParam(
        //            database,
        //            Enums.LocPresetChar.id.ToString(), id,
        //            Enums.LocalizationTables.preset_characters.ToString()
        //        ),
        //        r => new LocPresetChar(r, database)
        //    );
        //}
        public static LocConversation GetConversation(int id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocConvos.id.ToString(), id,
                    Enums.LocalizationTables.conversation.ToString()
                ),
                r => new LocConversation(r, database)
            );
        }
        public static LocNewsEntry GetNewsEntry(int id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocNewsEntries.id.ToString(), id,
                    Enums.LocalizationTables.news_entries.ToString()
                ),
                r => new LocNewsEntry(r, database)
            );
        }
        public static LocStoryConItem GetStoryConItem(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocStoryConItems.id.ToString(), id,
                    Enums.LocalizationTables.story_conditions_item.ToString()
                ),
                r => new LocStoryConItem(r, database)
            );
        }
        public static LocStash GetStash(string stashName, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocStashes.stash_name.ToString(), stashName,
                    Enums.LocalizationTables.stashes.ToString()
                ),
                r => new LocStash(r, database)
            );
        }
        public static LocRecipe GetRecipe(string serumId, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocRecipes.serum_id.ToString(), serumId,
                    Enums.LocalizationTables.recipes.ToString()
                ),
                r => new LocRecipe(r, database)
            );
        }
        public static LocSkillPerks GetSkillPerk(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocSkillPerks.id.ToString(), id,
                    Enums.LocalizationTables.skill_perks.ToString()
                ),
                r => new LocSkillPerks(r, database)
            );
        }
        public static LocFaction GetFaction(int id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocFactions.id.ToString(), id,
                    Enums.LocalizationTables.factions.ToString()
                ),
                r => new LocFaction(r, database)
            );
        }
        public static LocLvlData GetLevelData(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocLvlData.id.ToString(), id,
                    Enums.LocalizationTables.level_data.ToString()
                ),
                r => new LocLvlData(r, database)
            );
        }
        public static LocTutorials GetTutorial(int id, Enums.Databases database)
        {
            return ReaderToItem(
                GetLocSQLDataByParam(
                    database,
                    Enums.LocTutorials.id.ToString(), id,
                    Enums.LocalizationTables.tutorials.ToString()
                ),
                r => new LocTutorials(r, database)
            );
        }
    }
}
