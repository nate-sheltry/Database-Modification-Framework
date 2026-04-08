using System.Collections.Generic;

namespace Database_Modification_Framework.Definitions
{
    public abstract partial class DatbaseModificationPlugin
    {
        public partial class DataClass
        {
            public class NonRegClass
            {
                internal DatbaseModificationPlugin root;
                internal DataClass parent;
                public NonRegClass(DataClass dc)
                {
                    root = dc.root;
                    parent = dc;
                }
                // Items
                public NonRegionalItem GetItem(string id) =>
                    root.GetThing(Database.DbNonRegional.GetItem, id);
                public List<NonRegionalItem> GetItems(
                    List<(Enums.NonRegItem, object)> values)
                    => root.GetThings(Database.DbNonRegional.GetItems, values);
                public List<NonRegionalItem> GetAllItems() =>
                    root.GetThings(Database.DbNonRegional.GetAllItems);
                // Enemies
                public SlaughterhouseEnemy GetEnemy(string id) =>
                    root.GetThing(Database.DbNonRegional.GetEnemy, id);
                public List<SlaughterhouseEnemy> GetEnemies
                    (List<(Enums.NonRegEnemy, object)> values)
                    => root.GetThings(Database.DbNonRegional.GetEnemies, values);
                public List<SlaughterhouseEnemy> GetAllEnemies() =>
                    root.GetThings(Database.DbNonRegional.GetAllEnemies);
                // Props
                public SlaughterhouseProp GetProp(string id) =>
                    root.GetThing(Database.DbNonRegional.GetProp, id);
                public List<SlaughterhouseProp> GetProps(
                    List<(Enums.NonRegProp, object)> values)
                    => root.GetThings(Database.DbNonRegional.GetProps, values);
                public List<SlaughterhouseProp> GetAllProps() =>
                    root.GetThings(Database.DbNonRegional.GetAllProps);
            }
        }
    }
}
