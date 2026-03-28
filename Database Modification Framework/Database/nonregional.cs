using Database_Modification_Framework.Definitions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static List<NonRegionalItem> GetItems(Enums.NonRegItem field, string value)
        {
            return ReaderToItems(
                GetNonRegionalDataByParam(
                    field.ToString(),
                    value,
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
        public static List<SlaughterhouseEnemy> GetEnemies(Enums.NonRegEnemy field, string value)
        {
            return ReaderToItems(
                GetNonRegionalDataByParam(
                    field.ToString(),
                    value,
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
        public static List<SlaughterhouseProp> GetProps(Enums.NonRegProp field, string value)
        {
            return ReaderToItems(
                GetNonRegionalDataByParam(
                    field.ToString(), value,
                    Enums.NonRegionalTables.slaughterhouse_props.ToString()
                ),
                r => new SlaughterhouseProp(r)
            );
        }
    }
}
