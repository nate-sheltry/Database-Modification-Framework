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

namespace DTMod
{
    internal static class Mod
    {
        public static DTMod inst { get; set; }
    }

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    [BepInDependency(
    FrameworkGUID,
    BepInDependency.DependencyFlags.HardDependency
)]
    public class DTMod : DatbaseModificationPlugin
    {
        private const string MyGUID = "tunguska.natesheltry.harmony_tester2.mod";
        private const string PluginName = "Framework Harmony Test Mod 2";
        private const string VersionString = "0.0.5";
        public static Harmony harmony;

        public void Awake()
        {
            Mod.inst = this;
            DTMod.harmony = new Harmony(MyGUID);
            DTMod.harmony.PatchAll();
            Utils.LogMessage("Loaded DT Mod.");
            
        }
    }

    [HarmonyPatch(typeof(PlayerSurvival), "CompleteResting", MethodType.Normal)]
    internal static class Patch1
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            var rnd = new System.Random();
            var item = Mod.inst.Data.NonReg.GetItem("ammo12shot");
            var locItem = item.GetLocalizationData(Mod.inst.Runtime.Language);
            locItem.Name = $"{locItem.Name}{rnd.Next(1, 9)}";
            int atrIndex = item.GlobalAttributes.FindIndex(x => x.Name == "Damage");
            GenericFloat dmg = (GenericFloat)item.GlobalAttributes[atrIndex];
            dmg.Value = dmg.Value+1;
            item.GlobalAttributes[atrIndex] = new GenericFloat(dmg.Name, dmg.Value);
            Mod.inst.Runtime.UpdateItem(
                Mod.inst.Runtime.SearchAllPlayerItems(item.Id),
                locItem,
                item
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
