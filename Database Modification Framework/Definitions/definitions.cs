using BepInEx;
using Mono.Data.Sqlite;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Mono.Security.X509.X520;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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

    public static class Files
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
    public static class Enums
    {
        public enum NonRegionalTables
        {
            slaughterhouse_enemies = 0,
            slaughterhouse_props = 1,
            item_attributes = 2,

        }
        public enum NonRegionalItem
        {
            id = 0,
            prefab_name = 1,
            sprite_name = 2,
            weight = 3,
            type = 4,
            grid_cols = 5,
            grid_rows = 6,
            max_stack_size = 7,
            tier = 8,
            base_price = 9,
            max_durability = 10,
            durability = 11,
            use_limit = 12,
            is_usable = 13,
            global_attributes = 14,
            not_for_sale = 15,
            challenge_level = 16,
        }
        public enum NonRegionalEnemy
        {
            character_id = 0,
            tier = 1,
            character_type = 2,
            Comments = 3,
        }
        public enum NonRegionalProp
        {
            id = 0,
            tier = 1,
            price = 2,
            type = 3,
            challenge_level = 4,
        }
    }

    //public class QueuedMod
    //{
    //    public BaseUnityPlugin mod { get; }
    //    public int LoadOrder { get;  }
    //    public QueuedMod(BaseUnityPlugin mod, int LoadOrder = 1000)
    //    {
    //        this.mod = mod;
    //        this.LoadOrder = LoadOrder;
    //    }
    //}

    public static class ItemType
    {
        public static readonly string[] TypeList = new string[]{
            "Ammo","Armor","ArmorInsertion","Attachment",
            "BackpackUp","Battery","Casing","Fertilizer",
            "Food","FoodIngredient","Fuel","HeadGear",
            "Helmet","Misc","Ingredient","IngredientJar",
            "Key","Keychain","Medicine","Medkit",
            "Money","MutantWeapon","Note","Poison",
            "PrimaryWeapon","SideArm","Recipe","RecipeBook",
            "Repair","Seed","Serum","SleepingBag",
            "Solvent","SupplyPack","Thrown","Tool",
            "EasterEggs",
        };
        public static string Ammo { get => TypeList[0]; }
        public static string Armor { get => TypeList[1]; }
        public static string ArmorInsertion { get => TypeList[2]; }
        public static string Attachment { get => TypeList[3]; }
        public static string BackpackUp { get => TypeList[4]; }
        public static string Battery { get => TypeList[5]; }
        public static string Casing { get => TypeList[6]; }
        public static string Fertilizer { get => TypeList[7]; }
        public static string Food { get => TypeList[8]; }
        public static string FoodIngredient { get => TypeList[9]; }
        public static string Fuel { get => TypeList[10]; }
        public static string HeadGear { get => TypeList[11]; }
        public static string Helmet { get => TypeList[12]; }
        public static string Misc { get => TypeList[13]; }
        public static string Ingredient { get => TypeList[14]; }
        public static string IngredientJar { get => TypeList[15]; }
        public static string Key { get => TypeList[16]; }
        public static string Keychain { get => TypeList[17]; }
        public static string Medicine { get => TypeList[18]; }
        public static string Medkit { get => TypeList[19]; }
        public static string Money { get => TypeList[20]; }
        public static string MutantWeapon { get => TypeList[21]; }
        public static string Note { get => TypeList[22]; }
        public static string Poison { get => TypeList[23]; }
        public static string PrimaryWeapon { get => TypeList[24]; }
        public static string SideArm { get => TypeList[25]; }
        public static string Recipe { get => TypeList[26]; }
        public static string RecipeBook { get => TypeList[27]; }
        public static string Repair { get => TypeList[28]; }
        public static string Seed { get => TypeList[29]; }
        public static string Serum { get => TypeList[30]; }
        public static string SleepingBag { get => TypeList[31]; }
        public static string Solvent { get => TypeList[32]; }
        public static string SupplyPack { get => TypeList[33]; }
        public static string Thrown { get => TypeList[34]; }
        public static string Tool { get => TypeList[35]; }
        public static string EasterEggs { get => TypeList[36]; }
    }

    public interface ISQLItem
    {
        string Database { get; }
        IDbCommand GetSqlCommand();

    }
    public abstract class SQLItem : ISQLItem
    {
        internal string _database { get; set; }
        public string Database { get => _database; }
        internal string _sql { get; set; }
        public virtual IDbCommand GetSqlCommand()
        {
            return new SqliteCommand(_sql);
        }
    }
    public class SlaughterhouseEnemy : SQLItem
    {
        public string character_id;
        public short tier;
        public short character_type;
        public string Comments;
        public SlaughterhouseEnemy(IDataReader reader)
        {
            _database = Files.NonRegional;
            character_id = reader.GetString((int)Enums.NonRegionalEnemy.character_id);
            tier = reader.GetInt16((int)Enums.NonRegionalEnemy.tier);
            character_type = reader.GetInt16((int)Enums.NonRegionalEnemy.character_type);
            Comments = reader.GetString((int)Enums.NonRegionalEnemy.Comments);
        }
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("cid", character_id),
                new SqliteParameter("tr", tier),
                new SqliteParameter("ct", character_type),
                new SqliteParameter("com", Comments),
            });
            cmd.CommandText = $@"
                UPDATE {Enums.NonRegionalTables.slaughterhouse_enemies} 
                SET 
                    {Enums.NonRegionalEnemy.tier.ToString()} = @tr,
                    {Enums.NonRegionalEnemy.character_type.ToString()} = @ct,
                    {Enums.NonRegionalEnemy.Comments.ToString()} = @com
                WHERE {Enums.NonRegionalEnemy.character_id.ToString()} = @cid;";
            return cmd;
        }
    }
    public class SlaughterhouseProp : SQLItem
    {
        public string id;
        public short tier;
        public int price;
        public string type;
        public short challenge_level;
        public SlaughterhouseProp(IDataReader reader)
        {
            _database = Files.NonRegional;
            id = reader.GetString((int)Enums.NonRegionalProp.id);
            tier = reader.GetInt16((int)Enums.NonRegionalProp.tier);
            price = reader.GetInt32((int)Enums.NonRegionalProp.price);
            type = reader.GetString((int)Enums.NonRegionalProp.type);
            challenge_level = reader.GetInt16((int)Enums.NonRegionalProp.challenge_level);
        }
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("id", id),
                new SqliteParameter("tr", tier),
                new SqliteParameter("pc", price),
                new SqliteParameter("tp", type),
                new SqliteParameter("cl", challenge_level),
            });
            cmd.CommandText = $@"
                UPDATE {Enums.NonRegionalTables.slaughterhouse_props} 
                SET 
                    {Enums.NonRegionalProp.tier.ToString()} = @tr,
                    {Enums.NonRegionalProp.price.ToString()} = @pc,
                    {Enums.NonRegionalProp.type.ToString()} = @tp,
                    {Enums.NonRegionalProp.challenge_level.ToString()} = @cl
                WHERE {Enums.NonRegionalProp.id.ToString()} = @id;";
            return cmd;
        }
    }
    public class RawSQLItem : SQLItem
    {
        public RawSQLItem(string dbName, string sql)
        {
            _database = dbName;
            _sql = sql;
        }
    }
    public static class AttributeReader
    {
        // Performance could be better here, so this will need to be revisited at some point.
        internal static readonly HashSet<string> stringBools = new HashSet<string>
        {
            "_ReloadToUnjam", "_NoHeadShots", "_Cookable",
            "_IsRanged", "_hideHats", "_IsFull",
        };

        internal static readonly HashSet<string> numericBools = new HashSet<string>
        {
            "_AlwaysKnockBack", "_IsGrenade", "_IsIllustration",
            "_convertsDamage", "_returnsDamage", "_removeSpeedPenalty",
            "_NoHelmet", "_IsHeavy", "_NoJump",
            "_nightVision", "_NoSprint", "_boostArmStrength",
            "_MaxMoveSpeed",
        };

        internal static readonly HashSet<string> genericInts = new HashSet<string>
        {
            "_Radiation","_PoisonDuration","_CausticDuration",
            "_visitationProtection","_gasProtection","_Toxin",
            "_Length","Blunt Damage","_DamageHigh",
            "Padding","_AccuracyMult","_Caustic",
            "_PoisonDamage","Poison Hits","Radiation",
            "_StaminaRestoreBoost","Armor","Charges",
            "_LengthSec","_Duration","_MoreCarryWeight",
            "_NoiseLevel","_Damage","Radius",
            "_Muzzle Velocity","Penetration","_ExtraRange",
            "Capacity","Electrical","Calories",
            "_carryWeight","_Electric","Sharp Damage",
        };

        internal static readonly HashSet<string> genericFloats = new HashSet<string>
        {
            "_Rarity","Rate of Fire","_reduceNoise",
            "_reduceNightVisibility","Battery Drain","_staminaConversion",
            "Swing Speed","_RequireStrength","_Bleeding",
            "Recoil","_DamageDrop","_restoreHealth",
            "Accuracy","Detection Radius","Speed",
            "Handling","Coverage","_ReloadSpeed",
            "_Encumbrance","Growth Period","Damage",
        };

        internal static readonly HashSet<string> specialNumerics = new HashSet<string>
        {
            "Pockets", "_NewRows", "_CurrentFireMode",
            "Magazine Size", "_numberOfProjectiles",
            "_ThrowType", "_LoadedAmmos",
        };

        public static IGAttribute ReadAttribute(string line)
        {
            string[] breakdown = line.Split('=');
            string key, value;
            (key, value) = (breakdown[0], breakdown[1]);
            if (key.Contains("Repair Amount"))
                return new RepairNumeric(key);
            else if (stringBools.Contains(key))
                return new StringBool(line);
            else if (numericBools.Contains(key))
                return new NumericBool(line);
            else if (genericInts.Contains(key))
                return new GenericInt(line);
            else if (genericFloats.Contains(key))
                return new GenericFloat(line);
            else if (specialNumerics.Contains(key))
                return new SpecialNumeric(line);
            else
                return new GenericAttribute(line);
        }
    }
    public class ItemAttributes : List<IGAttribute>
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

    // The Non Regional Item Table Datatype
    public class Item : SQLItem
    {
        public string Id { get; set; }
        public string PrefabName { get; set; }
        public string SpriteName { get; set; }
        public float Weight { get; set; }
        public string Type { get; set; }
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
        public Item(object data)
        {
            try
            {
                _database = Files.NonRegional;
                Id = (string)GetPropValue(data, "Id");
                PrefabName = (string)GetPropValue(data, "PrefabName");
                SpriteName = (string)GetPropValue(data, "SpriteName");
                Weight = (float)GetPropValue(data, "Weight");
                Type = (string)GetPropValue(data, "Type");
                GridCols = (short)GetPropValue(data, "GridCols");
                GridRows = (short)GetPropValue(data, "GridRows");
                MaxStackSize = (int)GetPropValue(data, "MaxStackSize");
                Tier = (short)GetPropValue(data, "Tier");
                BasePrice = (float)GetPropValue(data, "BasePrice");
                MaxDurability = (int)GetPropValue(data, "MaxDurability");
                Durability = (float)GetPropValue(data, "Durability");
                UseLimit = (short)GetPropValue(data, "UseLimit");
                IsUsable = (bool)GetPropValue(data, "IsUsable");
                GlobalAttributes = (ItemAttributes)GetPropValue(data, "GlobalAttributes");
                NotForSale = (bool)GetPropValue(data, "NotForSale");
                ChallengeLevel = (short)GetPropValue(data, "ChallengeLevel");
            } catch (Exception ex)
            {
                Utils.Log.LogError($"Failed creating a Non Regional Item object. {ex}");
            }

        }
        public Item(IDataReader reader)
        {
            _database = Files.NonRegional;
            Id = reader.GetString((int)Enums.NonRegionalItem.id);
            PrefabName = reader.GetString((int)Enums.NonRegionalItem.prefab_name);
            SpriteName = reader.GetString((int)Enums.NonRegionalItem.sprite_name);
            Weight = reader.GetFloat((int)Enums.NonRegionalItem.weight);
            Type = reader.GetString((int)Enums.NonRegionalItem.type);
            GridCols = reader.GetInt16((int)Enums.NonRegionalItem.grid_cols);
            GridRows = reader.GetInt16((int)Enums.NonRegionalItem.grid_rows);
            MaxStackSize = reader.GetInt32((int)Enums.NonRegionalItem.max_stack_size);
            Tier = reader.GetInt16((int)Enums.NonRegionalItem.tier);
            BasePrice = reader.GetFloat((int)Enums.NonRegionalItem.base_price);
            MaxDurability = reader.GetInt32((int)Enums.NonRegionalItem.max_durability);
            Durability = reader.GetFloat((int)Enums.NonRegionalItem.durability);
            UseLimit = reader.GetInt16((int)Enums.NonRegionalItem.use_limit);
            IsUsable = reader.GetBoolean((int)Enums.NonRegionalItem.is_usable);
            GlobalAttributes = new ItemAttributes(reader.GetString((int)Enums.NonRegionalItem.global_attributes));
            NotForSale = reader.GetBoolean((int)Enums.NonRegionalItem.not_for_sale);
            ChallengeLevel = reader.GetInt32((int)Enums.NonRegionalItem.challenge_level);
        }
        private static object GetPropValue(object obj, string propName)
        {
            return obj?.GetType().GetProperty(propName)?.GetValue(obj);
        }

        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{ 
                new SqliteParameter("id", Id),
                new SqliteParameter("prefab_name", PrefabName),
                new SqliteParameter("sprite_name", SpriteName),
                new SqliteParameter("weight", Weight),
                new SqliteParameter("type", Type),
                new SqliteParameter("grid_cols", GridCols),
                new SqliteParameter("grid_rows", GridRows),
                new SqliteParameter("max_stack_size", MaxStackSize),
                new SqliteParameter("tier", Tier),
                new SqliteParameter("base_price", BasePrice),
                new SqliteParameter("max_durability", MaxDurability),
                new SqliteParameter("durability", Durability),
                new SqliteParameter("use_limit", UseLimit),
                new SqliteParameter("is_usable", IsUsable ? 1 : 0),
                new SqliteParameter("global_attributes", GlobalAttributes.GetSql()),
                new SqliteParameter("not_for_sale", NotForSale ? 1 : 0),
                new SqliteParameter("challenge_level", ChallengeLevel),
            });
            cmd.CommandText = $@"
                UPDATE {Enums.NonRegionalTables.item_attributes.ToString()} 
                SET 
                    prefab_name = @prefab_name,
                    sprite_name = @sprite_name,
                    weight = @weight,
                    type = @type,
                    grid_cols = @grid_cols,
                    grid_rows = @grid_rows,
                    max_stack_size = @max_stack_size,
                    tier = @tier,
                    base_price = @base_price,
                    max_durability = @max_durability,
                    durability = @durability,
                    use_limit = @use_limit,
                    is_usable = @is_usable,
                    global_attributes = @global_attributes,
                    not_for_sale = @not_for_sale,
                    challenge_level = @challenge_level
                WHERE id = @id";
            return cmd;
        }

        //public Item(IDataReader reader)
        //{
        //    this.Id = reader.GetString(DatatableInfo.NonRegional["id"]);

        //}
    }

}
