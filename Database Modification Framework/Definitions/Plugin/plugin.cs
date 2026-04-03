using BepInEx;
using Database_Modification_Framework.Database;
using System;
using System.Collections.Generic;

namespace Database_Modification_Framework.Definitions
{
    // Implements a child Unity Plugin class allowing users to use static classses
    // for easy data retrieval and manipulation.
    public abstract partial class DatbaseModificationPlugin : BaseUnityPlugin
    {
        private static string ModId;
        public DatbaseModificationPlugin() {
            ModId = Info.Metadata.Name; 
        }
        public static partial class Data
        {
            private static List<T> GetEverything<T>(Func<List<T>> func)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(); }
                catch { return new List<T>(); }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static List<T> GetEverything<T>(
                Func<string, string, List<T>> func, string db, string table)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(db, table); }
                catch { return new List<T>(); }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static T GetThing<T>(Func<string, T> func, string value) 
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(value); }
                catch { return null; }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static T GetThing<T, DEnum>(Func<string, DEnum, T> func, string value, DEnum db)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(value, db); }
                catch { return null; }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static T GetThing<T, DEnum>(Func<int, DEnum, T> func, int value, DEnum db)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(value, db); }
                catch { return null; }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static T GetThing<T, DEnum>(
                Func<string, string, DEnum, T> func, string value1, string value2, DEnum db)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(value1, value2, db); }
                catch { return null; }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static T GetThing<T, DEnum>(
                Func<int, int, DEnum, T> func, int value1, int value2, DEnum db)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(value1, value2, db); }
                catch { return null; }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static T GetThing<T, DEnum>(
                Func<string, string, string, DEnum, T> func, string value1, string value2, string value3, DEnum db)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(value1, value2, value3, db); }
                catch { return null; }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static List<T> GetThings<T, TEnum>(Func<List<(TEnum, object)>, List<T>> func, List<(TEnum, object)> values)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(values); }
                catch { return new List<T>(); }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static List<T> GetThings<T, TEnum>(
                Func<Enums.Databases, List<(TEnum, object)>, List<T>> func, 
                Enums.Databases db, List<(TEnum, object)> values)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(db, values); }
                catch { return new List<T>(); }
                finally { FrameworkUtils.CleanMod(); }
            }
            private static List<T> GetThings<T>(
                Func<Enums.Databases, List<T>> func,
                Enums.Databases db)
            where T : class
            {
                FrameworkUtils.RegisterMod(ModId);
                try { return func(db); }
                catch { return new List<T>(); }
                finally { FrameworkUtils.CleanMod(); }
            }
        }

        public static class Utils
        {
            public static void LogDebug(string message) => 
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Debug, message, ModId);
            public static void LogInfo(string message) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Info, message, ModId);
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
