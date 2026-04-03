using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Modification_Framework.Definitions
{
    public abstract partial class DatbaseModificationPlugin
    {
        public static partial class Data
        {
            public static class NonReg
            {
                // Items
                public static NonRegionalItem GetItem(string id) =>
                    GetThing(Database.DbNonRegional.GetItem, id);
                public static List<NonRegionalItem> GetItems(
                    List<(Enums.NonRegItem, object)> values)
                    => GetThings(Database.DbNonRegional.GetItems, values);
                public static List<NonRegionalItem> GetAllItems() =>
                    GetEverything(Database.DbNonRegional.GetAllItems);
                // Enemies
                public static SlaughterhouseEnemy GetEnemy(string id) =>
                    GetThing(Database.DbNonRegional.GetEnemy, id);
                public static List<SlaughterhouseEnemy> GetEnemies
                    (List<(Enums.NonRegEnemy, object)> values)
                    => GetThings(Database.DbNonRegional.GetEnemies, values);
                public static List<SlaughterhouseEnemy> GetAllEnemies() =>
                    GetEverything(Database.DbNonRegional.GetAllEnemies);
                // Props
                public static SlaughterhouseProp GetProp(string id) =>
                    GetThing(Database.DbNonRegional.GetProp, id);
                public static List<SlaughterhouseProp> GetProps(
                    List<(Enums.NonRegProp, object)> values)
                    => GetThings(Database.DbNonRegional.GetProps, values);
                public static List<SlaughterhouseProp> GetAllProps() =>
                    GetEverything(Database.DbNonRegional.GetAllProps);
            }
        }
    }
}
