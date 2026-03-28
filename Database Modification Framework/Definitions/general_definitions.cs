using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Database_Modification_Framework.Definitions
{
    public static class Directories
    {
        public static readonly string mainDatabase = Path.Combine(
            Application.streamingAssetsPath, "GameData", "Database"
            );
        public static readonly string backupDatabase = Path.Combine(
            Application.streamingAssetsPath, "GameData", "Database", "BackUp"
            );
        public static readonly string databaseDir = Path.Combine(
            "Tunguska_Data", "StreamingAssets", "GameData", "Database"
            );
    }

    internal static class Files
    {
        public const string AI = "AI";
        public const string Main = "Main";
        public const string Chinese = "MainCH";
        public const string French = "MainFR";
        public const string FrenchOld = "MainFR_old";
        public const string German = "MainGER";
        public const string GermanOld = "MainGer_old";
        public const string Italian = "MainITA";
        public const string Russian = "MainRUS";
        public const string Spanish = "MainSPA";
        public const string Ukrainian = "MainUA";
        public const string NonRegional = "NonRegional";
        public const string Translation = "Translation";
        public const string TranslationOld = "Translation_old";
    }

    public interface ISQLItem
    {
        string Database { get; }
        string ModId { get; }
        IDbCommand GetSqlCommand();
    }
    public abstract class SQLItem : ISQLItem
    {
        public string ModId { get; private set; }
        internal Enums.Databases _database { get; set; }
        public string Database { get => _database.ToString(); }
        internal string _sql { get; set; }
        public SQLItem()
        {
            ModId = FrameworkUtils._activeModId.Value;
        }
        public virtual ISQLItem Create(IDataReader reader)
        {
            return null;
        }
        public virtual void Sync() {
            Framework.QueueSQL(this);
        }
        public virtual IDbCommand GetSqlCommand()
        {
            return new SqliteCommand(_sql);
        }
    }
    public sealed class RawSQLItem : SQLItem
    {
        public RawSQLItem(Enums.Databases db, string sql)
        {
            _database = db;
            _sql = sql;
        }
    }
    public sealed class ItemAttributes : List<IGAttribute>
    {
        public readonly string RawAttributes;
        public ItemAttributes(string attributesString)
        {
            RawAttributes = attributesString;
            foreach (string line in attributesString.Split('\n'))
            {
                Add(AttributeReader.ReadAttribute(line.Trim()));
            }
        }
        public string GetSql() => this.Count == 0 ? string.Empty :
            string.Join("\n", this.Select(item => item.ToGlobalAttributeLine()));
    }
    public abstract class BaseItem : SQLItem
    {
        public virtual string DbTable { get; }
        public string Id { get; set; }
        public string PrefabName { get; set; }
        public string SpriteName { get; set; }
        public float Weight { get; set; }
        public ItemType Type { get; set; }
        public short GridCols { get; set; }
        public short GridRows { get; set; }
        public int MaxStackSize { get; set; }
        public short Tier { get; set; }
        public float BasePrice { get; set; }
        public int MaxDurability { get; set; }
        public float Durability { get; set; }
        public short UseLimit { get; set; }
        public bool IsUsable { get; set; }
        public ItemAttributes GlobalAttributes { get; set; }
        public bool NotForSale { get; set; }
        public int ChallengeLevel { get; set; }
        protected internal static object GetPropValue(object obj, string propName)
        {
            return obj?.GetType().GetProperty(propName)?.GetValue(obj);
        }
    }

}
