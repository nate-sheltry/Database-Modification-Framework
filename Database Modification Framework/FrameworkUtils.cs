using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Database_Modification_Framework
{
    //Static class for globally accessing instance plugin data.
    public static class FrameworkUtils
    {
        public static ManualLogSource Log = null;
        internal static Main Instance = null;
        public static IReadOnlyDictionary<string, string> Databases = null;
        private static int maxTx = 0;
        public static int MAX_TX { get => maxTx; }
        private static short? FrameworkLogLevel = null;
        public static short GetLogLevel { get => (short)FrameworkLogLevel; }
        internal static readonly AsyncLocal<string> _activeModId = new AsyncLocal<string>();
        public static void RegisterMod(string modId) => _activeModId.Value = modId;
        public static void CleanMod() => _activeModId.Value = null;
        private static bool ShouldLog(LogLevel num)
        {
            // 0 Errors and Fatal only,
            // 1 +Warnings,
            // 2 +Messages,
            // 3 +Debugs
            switch (FrameworkLogLevel)
            {
                case 0: 
                    return (int)num <= (int)LogLevel.Error;
                case 1: 
                    return (int)num <= (int)LogLevel.Warning;
                case 2: 
                    return (int)num <= (int)LogLevel.Message;
                case 3: 
                    return (int)num == (int)LogLevel.Debug || (int)num <= (int)LogLevel.Message;
                case 4: return true;
                default: return false;
            }
        }
        internal static void InternalLog(LogLevel level, object data, string modIdOverride = "Framework")
        {
            if (!ShouldLog(level)) return;
            Log.Log(level, $"- [{_activeModId.Value ?? modIdOverride}] {data}");
        }
        internal static void ExternalLog(LogLevel level, object data, string modIdOverride)
        {
            if (!ShouldLog(level)) return;
            Log.Log(level, $"- [{modIdOverride}] {data}");
        }
        internal static void LoadConfig()
        {
            var iniFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "DatabaseModificationFramework.cfg"), true);
            ConfigEntry<int> config_int = iniFile.Bind(
                "General",
                "DebugLevel",
                1,
                "Determines Logging Level for Framework and it's dependant mods.\n" +
                "0 = General and Fatal Errors.\n1 = Errors and Warnings (Default)\n" +
                "2 = Errors, Warnings and Messages\n3 = Errors, Warnings, Messages, and Debug\n"+
                "4 = All Logs, Include Informational logs.\n"+
                "Info logs are used as additional verbose logging, and will not often ever be needed."
            );
            config_int.Value = Math.Max(0, Math.Min(4, config_int.Value));
            ConfigEntry<int> configTx = iniFile.Bind(
                "General",
                "MaxTxPerFrame",
                0,
                "0 = Auto detect. (Will guestimate based off some system info.)\n"+
                "Anything above 0 will be a manual overwrite of the autodetected value.\n"+
                ""
            );
            configTx.Value = Math.Max(0, configTx.Value);
            FrameworkLogLevel = (short)config_int.Value;
            maxTx = configTx.Value;
        }
        public static void DetermineMaxTX()
        {
            if (maxTx == 0)
            {
                // lp is for Logical Processors, aka threads.
                int lp = SystemInfo.processorCount;
                maxTx = lp <= 2 ? 1 :
                        lp <= 4 ? 3 :
                        lp <= 6 ? 5 :
                        lp <= 8 ? 8 :
                        lp <= 12 ? 10 :
                        lp <= 16 ? 12 :
                        15;
            }
            InternalLog(LogLevel.Debug, $"Max Transactions Per Frame Detected: {MAX_TX}");
        }
        internal static List<(string, object)> ConvertEnumsToStrings<Tenum>(List<(Tenum, object)> items)
            => items.Select(x => (x.Item1.ToString(), x.Item2)).ToList();
    }
}
