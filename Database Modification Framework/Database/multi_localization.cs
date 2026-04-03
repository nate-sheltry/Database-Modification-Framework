using BepInEx.Logging;
using Database_Modification_Framework.Definitions;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using static Database_Modification_Framework.Framework;
using static Database_Modification_Framework.FrameworkUtils;
using static Database_Modification_Framework.SqlExecutor;

namespace Database_Modification_Framework.Database
{
    public static partial class DbLocalization
    {
        // Multi Functions
        public static List<LocBaseItem> GetBaseItems(
            Enums.Databases database, List<(Enums.LocBaseItems, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.base_items.ToString()
                ),
                r => new LocBaseItem(r, database)
            );
        }
        public static List<LocGlobalResp> GetGlobalResponses(
            Enums.Databases database, List<(Enums.LocGlobResp, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.global_dialogue_response.ToString()
                ),
                r => new LocGlobalResp(r, database)
            );
        }
        public static List<LocJrnlEntry> GetJournalEntries(
            Enums.Databases database, List<(Enums.LocJrnlEntries, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.journal_entries.ToString()
                ),
                r => new LocJrnlEntry(r, database)
            );
        }
        public static List<LocFactRel> GetFactionRelationships(
            Enums.Databases database, List<(Enums.LocFactionRel, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.faction_relationships.ToString()
                ),
                r => new LocFactRel(r, database)
            );
        }
        public static List<LocNotePaper> GetNotePapers(
            Enums.Databases database, List<(Enums.LocNotePapers, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.note_papers.ToString()
                ),
                r => new LocNotePaper(r, database)
            );
        }
        public static List<LocEnvSounds> GetEnvSoundsets(
            Enums.Databases database, List<(Enums.LocEnvSounds, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.environment_sounds.ToString()
                ),
                r => new LocEnvSounds(r, database)
            );
        }
        public static List<LocTaskData> GetTaskDatas(
            Enums.Databases database, List<(Enums.LocTaskData, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.task_data.ToString()
                ),
                r => new LocTaskData(r, database)
            );
        }
        public static List<LocInitSquads> GetInitSquads(
            Enums.Databases database, List<(Enums.LocInitSquads, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.initial_squads.ToString()
                ),
                r => new LocInitSquads(r, database)
            );
        }
        //public static List<LocPresetChar> GetPresetChars(
        //    Enums.Databases database, List<(Enums.LocPresetChar, object)> values)
        //{
        //    return ReaderToItems(
        //        GetSQLDataByParams(
        //            database.ToString(),
        //            ConvertEnumsToStrings(values),
        //            Enums.LocalizationTables.preset_characters.ToString()
        //        ),
        //        r => new LocPresetChar(r, database)
        //    );
        //}
        public static List<LocConversation> GetConversations(
            Enums.Databases database, List<(Enums.LocConvos, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.conversation.ToString()
                ),
                r => new LocConversation(r, database)
            );
        }
        public static List<LocNewsEntry> GetNewsEntries(
            Enums.Databases database, List<(Enums.LocNewsEntries, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.news_entries.ToString()
                ),
                r => new LocNewsEntry(r, database)
            );
        }
        public static List<LocStoryConItem> GetStoryConItems(
            Enums.Databases database, List<(Enums.LocStoryConItems, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.story_conditions_item.ToString()
                ),
                r => new LocStoryConItem(r, database)
            );
        }
        public static List<LocStoryConTrigger> GetStoryConTriggers(
            Enums.Databases database, List<(Enums.LocStoryConTriggers, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.story_conditions_trigger.ToString()
                ),
                r => new LocStoryConTrigger(r, database)
            );
        }
        public static List<LocSkillPerks> GetSkillPerks(
            Enums.Databases database, List<(Enums.LocSkillPerks, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.skill_perks.ToString()
                ),
                r => new LocSkillPerks(r, database)
            );
        }
        public static List<LocStash> GetStashes(
            Enums.Databases database, List<(Enums.LocStashes, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.stashes.ToString()
                ),
                r => new LocStash(r, database)
            );
        }
        public static List<LocRecipe> GetRecipes(
            Enums.Databases database, List<(Enums.LocRecipes, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.recipes.ToString()
                ),
                r => new LocRecipe(r, database)
            );
        }
        public static List<LocFaction> GetFactions(
            Enums.Databases database, List<(Enums.LocFactions, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.factions.ToString()
                ),
                r => new LocFaction(r, database)
            );
        }
        public static List<LocLvlData> GetLevelDatas(
            Enums.Databases database, List<(Enums.LocLvlData, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.level_data.ToString()
                ),
                r => new LocLvlData(r, database)
            );
        }
        public static List<LocTutorials> GetTutorials(
            Enums.Databases database, List<(Enums.LocTutorials, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    database.ToString(),
                    ConvertEnumsToStrings(values),
                    Enums.LocalizationTables.tutorials.ToString()
                ),
                r => new LocTutorials(r, database)
            );
        }
    }
}
