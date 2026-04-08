using Database_Modification_Framework.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Database_Modification_Framework
{
    internal static class RuntimeData
    {
        internal static void GetBodyItems(CharacterInventory charInv, ref List<Item> list)
        {
            try
            {
                list.Add(charInv.HeadSlot);
                list.Add(charInv.GearSlot);
                list.Add(charInv.ArmorSlot);
                list.Add(charInv.RifleSlot);
                list.Add(charInv.SideArmSlot);
                list.Add(charInv.SideArmSlot2);
                list.Add(charInv.ToolSlot);
                list.Add(charInv.ToolSlot2);
                list.Add(charInv.ThrowSlot);
                list.RemoveAll(x => x == null);
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    $"Failed to get CharacterInventory Body Slots.\n{ex}");
            }
        }
        internal static void SearchBodyItems(string id, CharacterInventory charInv, ref List<Item> list)
        {
            try
            {
                if(charInv.HeadSlot?.ID == id)
                    list.Add(charInv.HeadSlot);
                if(charInv.GearSlot?.ID == id)
                    list.Add(charInv.GearSlot);
                if(charInv.ArmorSlot?.ID == id)
                    list.Add(charInv.ArmorSlot);
                if (charInv.RifleSlot?.ID == id)
                    list.Add(charInv.RifleSlot);
                if (charInv.SideArmSlot?.ID == id)
                    list.Add(charInv.SideArmSlot);
                if (charInv.SideArmSlot2?.ID == id)
                    list.Add(charInv.SideArmSlot2);
                if(charInv.ToolSlot?.ID == id)
                        list.Add(charInv.ToolSlot);
                if(charInv.ToolSlot2?.ID == id)
                    list.Add(charInv.ToolSlot2);
                if(charInv.ThrowSlot?.ID == id)
                    list.Add(charInv.ThrowSlot);
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    $"Failed to get CharacterInventory Body Slots.\n{ex}");
            }
        }
        internal static List<Item> GetCharItems(CharacterInventory charInv)
        {
            List<Item> itemDatas = new List<Item>();
            try
            {
                itemDatas.AddRange(charInv.Backpack.Select(x => x != null ? x.Item : null));
                itemDatas.AddRange(charInv.Pockets.Select(x => x != null ? x.Item : null));
                itemDatas.RemoveAll(x => x == null);
                GetBodyItems(charInv, ref itemDatas);
                return itemDatas;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    $"Failed To Get All CharacterInventoryItems.\n{ex}"
                );
                return new List<Item>();
            }
        }
        internal static List<Item> SearchCharItems(string id, CharacterInventory charInv)
        {
            List<Item> itemDatas = new List<Item>();
            try
            {
                itemDatas.AddRange(charInv.FindItemsInBackpack(id).Select(x => x != null ? x.Item : null));
                itemDatas.AddRange(charInv.FindItemsInPocket(id).Select(x => x != null ? x.Item : null));
                itemDatas.RemoveAll(x => x == null);
                SearchBodyItems(id, charInv, ref itemDatas);
                return itemDatas;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    $"Failed To Search All CharacterInventoryItems.\n{ex}"
                );
                return new List<Item>();
            }
        }

        internal static List<Item> GetAllSpawnedItems()
        {
            List<Item> itemDatas = new List<Item>();
            try
            {
                List<Character> chars = new List<Character>();
                chars = GameManager.Inst.NPCManager.AllCharacters;
                chars.RemoveAll(x => x == null);
                foreach (Character character in chars)
                {
                    if (character == null)
                    {
                        FrameworkUtils.InternalLog(BepInEx.Logging.LogLevel.Warning, "null character");
                        continue;
                    }
                    itemDatas.AddRange(GetCharItems(character.Inventory));
                }
                itemDatas.AddRange(GetAllPlayerItems().Where(x => !itemDatas.Any(y => object.ReferenceEquals(x, y))));
                return itemDatas;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    $"Failed To Get All Spawned Items.\n{ex}"
                );
                return new List<Item>();
            }
        }
        internal static List<Item> SearchAllSpawnedItems(string id)
        {
            List<Item> items = new List<Item>();
            try
            {
                List<Character> chars = new List<Character>();
                chars.AddRange(GameManager.Inst.NPCManager.AllCharacters);
                foreach (Character character in chars)
                {
                    if (character == null)
                    {
                        FrameworkUtils.InternalLog(BepInEx.Logging.LogLevel.Warning, "Null character found. Skipped.");
                    }
                    items.AddRange(SearchCharItems(id, character.Inventory));
                }
                items.AddRange(SearchCharItems(id,
                GameManager.Inst.PlayerControl.Party.Members[0].Inventory));
                return items;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    "Failed to Search All Spawned items.");
                return new List<Item>();
            }
        }

        internal static List<Item> GetAllPlayerItems()
        {
            List<Item> items = new List<Item>();
            HumanCharacter humChar = GameManager.Inst.PlayerControl.Party.Members[0];
            SaveGame curSave = GameManager.Inst.SaveGameManager.CurrentSave;
            CharacterInventory charInv = humChar.Inventory;
            try
            {
                items.AddRange(charInv.Backpack.Select(x => x != null ? x.Item : null));
                items.AddRange(charInv.Pockets.Select(x => x != null ? x.Item : null));
                GetBodyItems(charInv, ref items);
                return items;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    $"Failed To Get All CharacterInventoryItems.\n{ex}"
                );
                return new List<Item>();
            }
        }
        internal static List<Item> SearchAllPlayerItems(string id)
        {
            List<Item> items = new List<Item>();
            try
            {
                items.AddRange(SearchCharItems(id, GameManager.Inst.PlayerControl.Party.Members[0].Inventory));
                foreach (var item in items)
                {
                    FrameworkUtils.InternalLog(BepInEx.Logging.LogLevel.Message, $"{item.Name} : {item.ID}");
                }
                return items;
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    $"Failed To Get All CharacterInventoryItems.\n{ex}"
                );
                return new List<Item>();
            }
        }
        internal static void UpdateItem(Item runtimeItem, LocBaseItem locItem, NonRegionalItem dbItem = null)
            => UpdateItem(runtimeItem, null, locItem);
        internal static void UpdateItem(Item runtimeItem, NonRegionalItem dbItem, LocBaseItem locItem = null)
        {
            if (dbItem != null)
            {
                UpdateItemNonReg(runtimeItem, dbItem);
                dbItem.Sync();
            }
            if (locItem != null)
            {
                UpdateItemLocalization(runtimeItem, locItem);
                locItem.Sync();
            }
        }
        internal static void UpdateItem(List<Item> runtimeItems, LocBaseItem locItem, NonRegionalItem dbItem = null)
            => UpdateItem(runtimeItems, dbItem, locItem);
        internal static void UpdateItem(List<Item> runtimeItems, NonRegionalItem dbItem, LocBaseItem locItem = null)
        {
            foreach(var ri in runtimeItems)
            {
                if (dbItem != null)
                {
                    if (ri.ID != dbItem.Id)
                        continue;
                    UpdateItemNonReg(ri, dbItem);
                }
                if (locItem != null)
                {
                    if (ri.ID != locItem.Id)
                        continue;
                    UpdateItemLocalization(ri, locItem);
                }
            }
            if (dbItem != null)
                dbItem.Sync();
            if (locItem != null)
                locItem.Sync();
        }
        internal static void UpdateItems(List<Item> runtimeItems, List<(NonRegionalItem, LocBaseItem)> dbAndLocItems)
        {
            // Sub Lists
            var Lists = runtimeItems.GroupBy(g => g.ID).ToDictionary(g => g.Key, g => g.ToList());
            foreach(var tuple in dbAndLocItems)
            {
                if(!Lists.ContainsKey(tuple.Item1.Id))
                {
                    FrameworkUtils.InternalLog(BepInEx.Logging.LogLevel.Warning,
                        "Found Database Item that was not present in runtimeItems.");
                    continue;

                }
                UpdateItem(Lists[tuple.Item1.Id], tuple.Item1, tuple.Item2);
            }
        }
        internal static void UpdateItemNonReg(Item runtimeItem, NonRegionalItem dbItem)
        {
            if(runtimeItem.ID != dbItem.Id)
            {
                FrameworkUtils.InternalLog(BepInEx.Logging.LogLevel.Error,
                    $"{runtimeItem.ID} is not the same as {dbItem.Id}");
                return;
            }
            List<ItemAttribute> dbAttributes = dbItem.GlobalAttributes
                .Select(x =>
                {
                    var att = x.ToItemAttribute();
                    if (runtimeItem.AttributeIndex.ContainsKey(att.Name))
                    {
                        att.IsPercentage = runtimeItem.Attributes[runtimeItem.AttributeIndex[att.Name]].IsPercentage;
                    }
                    return att;
                })
                .ToList();
            List<ItemAttribute> runtimeAttributes = runtimeItem.Attributes;
            runtimeItem.Weight = dbItem.Weight;
            runtimeItem.SpriteName = dbItem.SpriteName;
            runtimeItem.PrefabName = dbItem.PrefabName;
            runtimeItem.Type = dbItem.Type;
            runtimeItem.GridCols = dbItem.GridCols;
            runtimeItem.GridRows = dbItem.GridRows;
            runtimeItem.MaxStackSize = dbItem.MaxStackSize;
            runtimeItem.BasePrice = dbItem.BasePrice;
            runtimeItem.Tier = dbItem.Tier;
            runtimeItem.IsUsable = dbItem.IsUsable;
            runtimeItem.UseLimit = dbItem.UseLimit;
            runtimeItem.MaxDurability = dbItem.MaxDurability;
            runtimeItem.Durability = dbItem.Durability;
            runtimeItem.Attributes = new List<ItemAttribute>();
            foreach (ItemAttribute attribute in dbAttributes)
            {
                object value = attribute.Value;
                if(attribute.Name == "_LoadedAmmos")
                {
                    value = runtimeItem.AttributeIndex
                    .TryGetValue("_LoadedAmmos", out var ammoKey)
                    ? runtimeAttributes[ammoKey].Value : attribute.Value;
                }
                ItemAttribute item2 = new ItemAttribute(attribute.Name, value)
                {
                    IsPercentage = attribute.IsPercentage, 
                };
                runtimeItem.Attributes.Add(item2);
            }
            runtimeItem.AttributeIndex = new Dictionary<string, int>();
            for (int i = 0; i < runtimeItem.Attributes.Count; i++)
            {
                if (runtimeItem.Attributes[i] != null && !runtimeItem.AttributeIndex.ContainsKey(runtimeItem.Attributes[i].Name))
                {
                    runtimeItem.AttributeIndex.Add(runtimeItem.Attributes[i].Name, i);
                }
            }
        }
        internal static void UpdateItemLocalization(Item runtimeItem, LocBaseItem dbItem)
        {
            try
            {
                runtimeItem.Name = dbItem.Name;
                runtimeItem.Description = dbItem.Description;
            } catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    BepInEx.Logging.LogLevel.Error,
                    $"Failed to update Item {runtimeItem.ID}'s Localization data.\n{ex}"
                );
            }
        }
    }
}
