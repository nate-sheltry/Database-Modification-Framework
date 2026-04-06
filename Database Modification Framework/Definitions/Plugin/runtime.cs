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
        public class RuntimeClass
        {
            internal DatbaseModificationPlugin root;
            public RuntimeClass(DatbaseModificationPlugin plugin)
            {
                root = plugin;
            }
            public Enums.Databases Language { get {
                    try
                    {
                        Enums.Databases db;
                        Enum.TryParse($"Main{GameManager.Inst.Language}", out db);
                        return db;
                    } catch (Exception ex)
                    {
                        root.Utils.LogError($"Failed to get runtime language. Returning Non Regional Database.\n{ex}");
                        return Enums.Databases.NonRegional;
                    }
                } }
            public List<Item> SearchAllSpawnedItems(string id)
                => root.GetThings(RuntimeData.SearchAllSpawnedItems, id);
            public List<Item> SearchAllPlayerItems(string id)
                => root.GetThings(RuntimeData.SearchAllPlayerItems, id);
            public List<Item> SearchCharacterItems(string id, Character character)
                => root.GetThings(RuntimeData.SearchCharItems, id, character.Inventory);
            public List<Item> SearchCharacterItems(string id, HumanCharacter character)
                => root.GetThings(RuntimeData.SearchCharItems, id, character.Inventory);
            public List<Item> SearchCharacterItems(string id, CharacterInventory charInv)
                => root.GetThings(RuntimeData.SearchCharItems, id, charInv);
            public List<Item> GetAllSpawnedItems()
                => root.GetThings(RuntimeData.GetAllSpawnedItems);
            public List<Item> GetAllPlayerItems()
                => root.GetThings(RuntimeData.GetAllPlayerItems);
            public List<Item> GetCharacterItems(Character character)
                => root.GetThings(RuntimeData.GetCharItems, character.Inventory);
            public List<Item> GetCharacterItems(HumanCharacter character)
                => root.GetThings(RuntimeData.GetCharItems, character.Inventory);
            public List<Item> GetCharacterItems(CharacterInventory charInv) 
                => root.GetThings(RuntimeData.GetCharItems, charInv); 
            public void UpdateItem(Item itemInstance, NonRegionalItem dbItem, LocBaseItem locItem = null)
                => root.DoThing(RuntimeData.UpdateItem, itemInstance, dbItem, locItem);
            public void UpdateItem(Item itemInstance, LocBaseItem locItem, NonRegionalItem dbItem = null)
                => root.DoThing(RuntimeData.UpdateItem, itemInstance, locItem, dbItem);
            public void UpdateItem(List<Item> itemInstances, NonRegionalItem dbItem, LocBaseItem locItem = null)
                => root.DoThing(RuntimeData.UpdateItem,itemInstances, dbItem, locItem);
            public void UpdateItem(List<Item> itemInstances, LocBaseItem locItem, NonRegionalItem dbItem = null)
                => root.DoThing(RuntimeData.UpdateItem, itemInstances, locItem, dbItem);
            public void UpdateItems(List<Item> itemInstances, List<(NonRegionalItem, LocBaseItem)> dbItems)
                => root.DoThing(RuntimeData.UpdateItems, itemInstances, dbItems);
        }
    }
}
