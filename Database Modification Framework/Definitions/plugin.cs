using BepInEx;
using System;
using System.Collections.Generic;

namespace Database_Modification_Framework.Definitions
{
    // Implements a child Unity Plugin class allowing users to use static classses
    // for easy data retrieval and manipulation.
    public abstract class DatbaseModificationPlugin : BaseUnityPlugin
    {
        private static string ModId;
        public DatbaseModificationPlugin() {
            ModId = Info.Metadata.Name; 
        }
        public static class Data
        {
            private static T GetThing<T>(Func<string, T> func, string value) 
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(value); }
                catch { return null; }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static List<T> GetThings<T, TEnum>(Func<TEnum, string, List<T>> func, TEnum num, string value)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(num, value); }
                catch { return null; }
                finally { FrameworkUtils.CleanMod(); }
            }
            public static NonRegionalItem GetItem(string id) => 
                GetThing(Database.DbNonRegional.GetItem, id);
            public static List<NonRegionalItem> GetItems(Enums.NonRegItem num, string id) => 
                GetThings(Database.DbNonRegional.GetItems, num, id);
            public static SlaughterhouseEnemy GetEnemy(string id) => 
                GetThing(Database.DbNonRegional.GetEnemy, id);
            public static List<SlaughterhouseEnemy> GetEnemies(Enums.NonRegEnemy num, string id) =>
                GetThings(Database.DbNonRegional.GetEnemies, num, id);
            public static SlaughterhouseProp GetProp(string id) => 
                GetThing(Database.DbNonRegional.GetProp, id);
            public static List<SlaughterhouseProp> GetProps(Enums.NonRegProp num, string id) =>
                GetThings(Database.DbNonRegional.GetProps, num, id);
        }

        public static class Utils
        {
            public static void LogDebug(string message) => 
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Debug, message, ModId);
            public static void LogMessage(string message) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Message, message, ModId);
            public static void LogWarning(string message, Exception ex = null) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Warning, $"{message}{(ex!= null ? $"\n{ex}":"")}", ModId);
            public static void LogError(string message, Exception ex = null) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Error, $"{message}{(ex != null ? $"\n{ex}" : "")}", ModId);
            public static void LogError(Exception ex) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Error, ex, ModId);
            public static void LogFatal(string message, Exception ex = null) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Fatal, $"{message}{(ex != null ? $"\n{ex}" : "")}", ModId);
            public static void LogFatal(Exception ex) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Fatal, ex, ModId);
        }

    }
}
