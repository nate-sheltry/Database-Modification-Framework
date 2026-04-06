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
        public static List<LocBaseItem> GetAllBaseItems(Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.base_items.ToString()
                ),
                r => new LocBaseItem(r, database)
            );
        }
        public static List<LocGlobalResp> GetAllGlobalResponses(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.global_dialogue_response.ToString()
                ),
                r => new LocGlobalResp(r, database)
            );
        }
        public static List<LocJrnlEntry> GetAllJournalEntries(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.journal_entries.ToString()
                ),
                r => new LocJrnlEntry(r, database)
            );
        }
        public static List<LocFactRel> GetAllFactionRelationships(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.faction_relationships.ToString()
                ),
                r => new LocFactRel(r, database)
            );
        }
        public static List<LocNotePaper> GetAllNotePapers(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.note_papers.ToString()
                ),
                r => new LocNotePaper(r, database)
            );
        }
        public static List<LocEnvSounds> GetAllEnvSoundsets(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.environment_sounds.ToString()
                ),
                r => new LocEnvSounds(r, database)
            );
        }
        public static List<LocTaskData> GetAllTaskDatas(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.task_data.ToString()
                ),
                r => new LocTaskData(r, database)
            );
        }
        public static List<LocInitSquads> GetAllInitSquads(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
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
        //            database,
        //            ConvertEnumsToStrings(values),
        //            Enums.LocalizationTables.preset_characters.ToString()
        //        ),
        //        r => new LocPresetChar(r, database)
        //    );
        //}
        public static List<LocConversation> GetAllConversations(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.conversation.ToString()
                ),
                r => new LocConversation(r, database)
            );
        }
        public static List<LocNewsEntry> GetAllNewsEntries(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.news_entries.ToString()
                ),
                r => new LocNewsEntry(r, database)
            );
        }
        public static List<LocStoryConItem> GetAllStoryConItems(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.story_conditions_item.ToString()
                ),
                r => new LocStoryConItem(r, database)
            );
        }
        public static List<LocStoryConTrigger> GetAllStoryConTriggers(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.story_conditions_trigger.ToString()
                ),
                r => new LocStoryConTrigger(r, database)
            );
        }
        public static List<LocSkillPerks> GetAllSkillPerks(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.skill_perks.ToString()
                ),
                r => new LocSkillPerks(r, database)
            );
        }
        public static List<LocStash> GetAllStashes(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.stashes.ToString()
                ),
                r => new LocStash(r, database)
            );
        }
        public static List<LocRecipe> GetAllRecipes(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.recipes.ToString()
                ),
                r => new LocRecipe(r, database)
            );
        }
        public static List<LocFaction> GetAllFactions(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.factions.ToString()
                ),
                r => new LocFaction(r, database)
            );
        }
        public static List<LocLvlData> GetAllLevelDatas(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.level_data.ToString()
                ),
                r => new LocLvlData(r, database)
            );
        }
        public static List<LocTutorials> GetAllTutorials(
            Enums.Databases database)
        {
            return ReaderToItems(
                GetAllLocSQLDataByTable(
                    database,
                    Enums.LocalizationTables.tutorials.ToString()
                ),
                r => new LocTutorials(r, database)
            );
        }
    }
}
