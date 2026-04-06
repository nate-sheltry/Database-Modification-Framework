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
        public const string FrameworkGUID = Main.MyGUID;
        internal string _modId { get; set; }
        public string ModId { get => _modId; }
        public UtilsClass Utils;
        public RuntimeClass Runtime;
        public DataClass Data;
        public DatbaseModificationPlugin() {
            _modId = Info.Metadata.Name;
            Utils = new UtilsClass(this);
            Runtime = new RuntimeClass(this);
            Data = new DataClass(this);
        }

        public partial class DataClass
        {
            protected internal DatbaseModificationPlugin root;
            public LocClass Loc;
            public NonRegClass NonReg;
            public DataClass(DatbaseModificationPlugin plugin)
            {
                root = plugin;
                Loc = new LocClass(this);
                NonReg = new NonRegClass(this);

            }
        }

        public class UtilsClass
        {
            private string ModId;
            public UtilsClass(DatbaseModificationPlugin parent)
            {
                ModId = parent.ModId;
            }
            public void LogDebug(string message) => 
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Debug, message, ModId);
            public void LogInfo(string message) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Info, message, ModId);
            public void LogMessage(string message) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Message, message, ModId);
            public void LogWarning(string message, Exception ex = null) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Warning, $"{message}{(ex!= null ? $"\n{ex}":"")}", ModId);
            public void LogError(string message, Exception ex = null) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Error, $"{message}{(ex != null ? $"\n{ex}" : "")}", ModId);
            public void LogError(Exception ex) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Error, ex, ModId);
            public void LogFatal(string message, Exception ex = null) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Fatal, $"{message}{(ex != null ? $"\n{ex}" : "")}", ModId);
            public void LogFatal(Exception ex) =>
                FrameworkUtils.ExternalLog(BepInEx.Logging.LogLevel.Fatal, ex, ModId);
        }

    }
}
