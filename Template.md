### Example Template Code

The following is some template/example code for utilizing the Framework for developing your own mods and plugins.

```csharp
using BepInEx;
using Database_Modification_Framework.Definitions;

namespace YourModsName
{
    // I call this "Mod", but you can call this anything.
    // This is a static class for referencing your plugin
    // in order for performing database and runtime operations 
    // outside of the standard BepInEx plugin lifecycle.
    // e.g., in Harmony Patches.
    internal static class Mod
    {
        public static YourModName Inst {get; set;}
    }
    // FrameworkGUID is defined by DatabaseModificationPlugin
    [BepInPlugin(
        MyGUID, 
        PluginName, 
        VersionString
    )]
    [BepInDependency(
        FrameworkGUID, 
        BepInDependency.DependencyFlags.HardDependency
    )]
    public class YourModName : DatabaseModificationPlugin
    {
        private const string MyGUID = "tunguska.author.modName.mod";
        private const string PluginName = "Your Plugin's Name";
        private const string VersionString = "x.x.x";
        public void Awake()
        {
            Mod.Inst = this;
            Example();
        }
        public void Example()
        {
            // Example Database Modifications within YourModName.
            // Runs during Awake; runtime data is not available yet.
            NonRegionalItem shotgun;
            shotgun = Data.NonReg.GetItem("huntingshotgun");

            // Modify item and add it to the Framework's queue.
            int index;
            index = shotgun.GlobalAttributes.FindIndex(x => x.name == "Damage");
            GenericFloat dmg = (GenericFloat)shotgun.GlobalAttributes[index];
            dmg.Value = 20;
            shotgun.GlobalAttributes[index] = new GenericFloat(dmg.Name, dmg.Value);
            shotgun.Sync();
        }
    }

    public class ExampleClass
    {
        // Runtime data is for modifying data after a save has been loaded.
        // To ensure live objects match the database, you must update them
        // afterward.
        public void ExampleRuntimeFunction()
        {
            // Reference your plugin via the static Mod class.
            NonRegionalItem shotgun;
            shotgun = Mod.Inst.Data.NonReg.GetItem("huntingshotgun");
            int index;
            index = shotgun.GlobalAttributes.FindIndex(x => x.name == "Damage");
            GenericFloat dmg = (GenericFloat)shotgun.GlobalAttributes[index];
            dmg.Value = 20;
            shotgun.GlobalAttributes[index] = new GenericFloat(dmg.Name, dmg.Value);

            // Runtime.UpdateItem
            // 1st arg - the scope of where the item is being updated.
            // 2nd arg - the NonRegional item or Localization item.
            // 3rd arg - the Localization item or NonRegional item (optional).
            Mod.Inst.Runtime.UpdateItem(
                Mod.Inst.Runtime.SearchAllSpawnedItems(shotgun.id),
                shotgun
            );
        }
    }
}
```