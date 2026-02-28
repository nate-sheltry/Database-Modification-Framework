using BepInEx;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

    public static class DatatableInfo
    {
        public static Dictionary<string, int> NonRegional;

        public static void Initialize()
        {
            NonRegional = new Dictionary<string, int>{
                { "id", 0},
                { "prefab_name", 1},
                { "sprite_name", 2},
                { "weight", 3},
                { "type", 4},
                { "grid_cols", 5},
                { "grid_rows", 6},
                { "max_stack_size", 7},
                { "tier", 8},
                { "base_price", 9},
                { "max_durability", 10},
                { "durability", 11},
                { "use_limit", 12},
                { "is_usable", 13},
                { "global_attributes", 14},
                { "not_for_sale", 15},
                { "challenge_level", 16},
            };
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
        public const string Ammo = "Ammo";
        public const string Armor = "Armor";
        public const string ArmorInsertion = "ArmorInsertion";
        public const string Attachment = "Attachment";
        public const string BackpackUp = "BackpackUp";
        public const string Battery = "Battery";
        public const string Casing = "Casing";
        public const string Fertilizer = "Fertilizer";
        public const string Food = "Food";
        public const string FoodIngredient = "FoodIngredient";
        public const string Fuel = "Fuel";
        public const string HeadGear = "HeadGear";
        public const string Helmet = "Helmet";
        public const string Misc = "Misc";
        public const string Ingredient = "Ingredient";
        public const string IngredientJar = "IngredientJar";
        public const string Key = "Key";
        public const string Keychain = "Keychain";
        public const string Medicine = "Medicine";
        public const string Medkit = "Medkit";
        public const string Money = "Money";
        public const string MutantWeapon = "MutantWeapon";
        public const string Note = "Note";
        public const string Poison = "Poison";
        public const string PrimaryWeapon = "PrimaryWeapon";
        public const string SideArm = "SideArm";
        public const string Recipe = "Recipe";
        public const string RecipeBook = "RecipeBook";
        public const string Repair = "Repair";
        public const string Seed = "Seed";
        public const string Serum = "Serum";
        public const string SleepingBag = "SleepingBag";
        public const string Solvent = "Solvent";
        public const string SupplyPack = "SupplyPack";
        public const string Thrown = "Thrown";
        public const string Tool = "Tool";
        public const string EasterEggs = "EasterEggs";
    }

    public struct ItemAttributes
    {
        public readonly string RawAttributes;
        public ItemAttributes(string reader)
        {
            this.RawAttributes = reader;
        }
    }

    // The Non Regional Item Table Datatype
    public struct Item
    {
        public string Id;
        public string Name;
        public string PrefabName;
        public string SpriteName;
        public float Weight;
        public string Type;
        public byte GridCols;
        public byte GridRows;
        public int MaxStackSize;
        public byte Tier;
        public float BasePrice;
        public int MaxDurability;
        public float Durability;
        public byte UseLimit;
        public bool IsUsable;
        public ItemAttributes GlobalAttributes;
        public bool NotForSale;
        public int ChallengeLevel;

        //public Item(IDataReader reader)
        //{
        //    this.Id = reader.GetString(DatatableInfo.NonRegional["id"]);

        //}
    }

    public struct PrimaryWeapon
    {

    }

}
