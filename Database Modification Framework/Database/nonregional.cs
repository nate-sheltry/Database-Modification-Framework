using Database_Modification_Framework.Definitions;
using System.Collections.Generic;
using System.Data;
using static Database_Modification_Framework.SqlExecutor;

namespace Database_Modification_Framework.Database
{
    public static class DbNonRegional
    {
        private static IDataReader GetNonRegionalDataByParam(string param, object value, string table)
        {
            return GetSQLDataByParam(Files.NonRegional, param, value, table);
        }
        public static NonRegionalItem GetItem(string Id)
        {
            return ReaderToItem(
                GetNonRegionalDataByParam(
                    Enums.NonRegItem.id.ToString(),
                    Id,
                    Enums.NonRegionalTables.item_attributes.ToString()
                ),
                r => new NonRegionalItem(r)
            );
        }
        public static List<NonRegionalItem> GetItems(List<(Enums.NonRegItem, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    Enums.Databases.NonRegional.ToString(),
                    FrameworkUtils.ConvertEnumsToStrings(values),
                    Enums.NonRegionalTables.item_attributes.ToString()
                ),
                r => new NonRegionalItem(r)
            );
        }
        public static List<NonRegionalItem> GetAllItems()
        {
            return ReaderToItems(
                GetAllSQLDataByTable(
                    Enums.Databases.NonRegional.ToString(),
                    Enums.NonRegionalTables.item_attributes.ToString()
                ),
                r => new NonRegionalItem(r)
            );
        }
        public static SlaughterhouseEnemy GetEnemy(string Id)
        {
            return ReaderToItem(
                GetNonRegionalDataByParam(
                    Enums.NonRegEnemy.character_id.ToString(),
                    Id,
                    Enums.NonRegionalTables.slaughterhouse_enemies.ToString()
                ),
                r => new SlaughterhouseEnemy(r)
            );
        }
        public static List<SlaughterhouseEnemy> GetEnemies(List<(Enums.NonRegEnemy, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    Enums.Databases.NonRegional.ToString(),
                    FrameworkUtils.ConvertEnumsToStrings(values),
                    Enums.NonRegionalTables.slaughterhouse_enemies.ToString()
                ),
                r => new SlaughterhouseEnemy(r)
            );
        }
        public static List<SlaughterhouseEnemy> GetAllEnemies()
        {
            return ReaderToItems(
                GetAllSQLDataByTable(
                    Enums.Databases.NonRegional.ToString(),
                    Enums.NonRegionalTables.slaughterhouse_enemies.ToString()
                ),
                r => new SlaughterhouseEnemy(r)
            );
        }
        public static SlaughterhouseProp GetProp(string Id)
        {
            return ReaderToItem(
                GetNonRegionalDataByParam(
                    Enums.NonRegProp.id.ToString(),
                    Id,
                    Enums.NonRegionalTables.slaughterhouse_props.ToString()
                ),
                r => new SlaughterhouseProp(r)
            );
        }
        public static List<SlaughterhouseProp> GetProps(List<(Enums.NonRegProp, object)> values)
        {
            return ReaderToItems(
                GetSQLDataByParams(
                    Enums.Databases.NonRegional.ToString(),
                    FrameworkUtils.ConvertEnumsToStrings(values),
                    Enums.NonRegionalTables.slaughterhouse_props.ToString()
                ),
                r => new SlaughterhouseProp(r)
            );
        }
        public static List<SlaughterhouseProp> GetAllProps()
        {
            return ReaderToItems(
                GetAllSQLDataByTable(
                    Enums.Databases.NonRegional.ToString(),
                    Enums.NonRegionalTables.slaughterhouse_props.ToString()
                ),
                r => new SlaughterhouseProp(r)
            );
        }
    }
}
