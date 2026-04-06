using BepInEx;
using Database_Modification_Framework.Definitions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace DynamicTestMod
{
    internal static class Mod
    {
        public static DynamicTest inst;
    }

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    [BepInDependency(
    FrameworkGUID,
    BepInDependency.DependencyFlags.HardDependency
)]
    public class DynamicTest : DatbaseModificationPlugin
    {
        private const string MyGUID = "tunguska.natesheltry.harmony_tester.mod";
        private const string PluginName = "Framework Harmony Test Mod";
        private const string VersionString = "0.0.5";
        public static Harmony harmony;

        public void Awake()
        {
            Mod.inst = this;
            harmony = new Harmony(MyGUID);
            harmony.PatchAll();
            Utils.LogMessage("Loaded Dynamic mod.");
        }
    }

    [HarmonyPatch(typeof(PlayerSurvival), "CompleteResting", MethodType.Normal)]
    internal static class Patch1
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            var rnd = new System.Random();
            var locItem = Mod.inst.Data.Loc.GetBaseItem("huntingshotgun", Mod.inst.Runtime.Language);
            locItem.Name = $"{locItem.Name}{rnd.Next(1,9)}";
            var item = locItem.GetNonRegionalData();
            item.Weight += 1;
            HumanCharacter humChar = GameManager.Inst.PlayerControl.Party.Members[0];
            Mod.inst.Runtime.UpdateItem(
                Mod.inst.Runtime.SearchAllSpawnedItems(item.Id), 
                item,
                locItem
                );
            //List<Item> items = DynamicTest.Data.Runtime.SearchAllItems(item.Id);
            //foreach (Item x in items)
            //{
            //    if (item == null)
            //    {
            //        DynamicTest.Utils.LogWarning("Found null item.");
            //        continue;
            //    }
            //    x.Weight += 1;
            //}
        }
    }

}
