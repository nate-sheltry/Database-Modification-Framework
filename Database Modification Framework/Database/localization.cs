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
using static Database_Modification_Framework.SqlExecutor;

namespace Database_Modification_Framework.Database
{
    public static class DbLocalization
    {
        public static LocBaseItem GetLocItem(string id, Enums.Databases database)
        {
            return ReaderToItem(
                GetSQLDataByParam(
                    database.ToString(),
                    Enums.LocBaseItems.id.ToString(),
                    id,
                    Enums.LocalizationTables.base_items.ToString()
                ),
                r => new LocBaseItem(r, database)
            );
        }
    }
}
