using BepInEx;
using BepInEx.Logging;
using Database_Modification_Framework.Database;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static Database_Modification_Framework.Definitions.Enums;
using static Database_Modification_Framework.FrameworkUtils;


namespace Database_Modification_Framework.Definitions
{
    public static class Directories
    {
        public static readonly string mainDatabase = Path.Combine(
            Application.streamingAssetsPath, "GameData", "Database"
            );
        public static readonly string workingDatabase = Path.Combine(
            Application.streamingAssetsPath, "GameData", "Database", "Working"
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
    public struct DatabaseTable
    {
        private string _stringTable;
        private object _enumTable;

        public DatabaseTable(string dbTable)
        {
            _stringTable = dbTable; _enumTable = null;
        }
        public DatabaseTable(Enum dbTable)
        {
            _stringTable = null; _enumTable = dbTable;
        }
        public static implicit operator DatabaseTable(string value) => new DatabaseTable(value);
        public static implicit operator DatabaseTable(Enum value) => new DatabaseTable(value);
        public static implicit operator string(DatabaseTable value) => value.ToString();
        public override string ToString()
        {
            return _stringTable != null ? _stringTable : _enumTable.ToString();
        }
    }
    public interface ISQLItem
    {
        string Database { get; }
        string DbTable { get; }
        string ModId { get; }
        IDbCommand GetSqlCommand();
        void Sync();
    }
    public abstract class SQLItem : ISQLItem
    {
        protected internal DatabaseTable _table { get; set;}
        public string DbTable { get => _table; }
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
        public abstract IDbCommand GetSqlCommand();
    }
    public sealed class RawSQLItem : SQLItem
    {
        public RawSQLItem(Enums.Databases db, string sql)
        {
            _database = db;
            _sql = sql;
        }
        public string GetData()
        {
            return SqlExecutor.GetSQLDataRaw(_database, _sql);
        }
        public override IDbCommand GetSqlCommand()
        {
            return new SqliteCommand(_sql);
        }
    }
    public sealed class ItemAttributes : List<IGAttribute>
    {
        public readonly string RawAttributes;
        public ItemAttributes(string attributesString)
        {
            RawAttributes = attributesString;
            if (string.IsNullOrWhiteSpace(attributesString))
                return;
            foreach (string line in attributesString.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(line.Trim()))
                {
                    try
                    {
                        Add(AttributeReader.ReadAttribute(line.Trim()));
                    } catch (Exception ex)
                    {
                        FrameworkUtils.InternalLog(BepInEx.Logging.LogLevel.Warning,
                            $"Failed reading in Attribute line, line will be skipped.\n"+
                            $"Line: {line}\n"+$"Entire Source Column:{attributesString}\n"+
                            $"{ex}");
                    }
                }
            }
        }
        public string GetSql() => this.Count == 0 ? string.Empty :
            string.Join("\n", this.Select(item => item.ToGlobalAttributeLine()));
    }
    public abstract class BaseItem : SQLItem
    {
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
        public float Durability { get; set; }
        public short UseLimit { get; set; }
        public bool IsUsable { get; set; }
        public ItemAttributes GlobalAttributes { get; set; }
        public bool NotForSale { get; set; }
        public int ChallengeLevel { get; set; }

        public LocBaseItem GetLocalizationData(Enums.Databases database)
        {
            return DbLocalization.GetBaseItem(Id, database);
        }
        public NonRegionalItem GetNonRegionalData()
        {
            return DbNonRegional.GetItem(Id);
        }
    }

}
