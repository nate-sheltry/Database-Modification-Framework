# Database Modification Framework - For Tunguska: The Visitation
A [BepInEx](https://github.com/BepInEx/BepInEx) plugin for *[Tunguska: The Visitation](https://store.steampowered.com/app/1601970/Tunguska_The_Visitation__Final_Cut/)*, designed to modify database data at startup and during runtime.

## Definitions

- **Program**
    This framework, implemented as a BepInEx plugin.

- **Framework plugin** 
    A BepInEx plugin that uses this framework to modify database and/or runtime data.

## Summary
The Program uses the BepInEx framework to hook into the third-party application (*Tunguska: The Visitation*) and starts automatically when it launches.

On startup, the Program:
- Backs up the original SQLite database files.
- Creates a working copy of those files for modification.

The Program then exposes a framework that allows other mods or components to modify both runtime data and the contents of the working database. All changes to the working database are batched and applied to the third-party application's currently used database files, ensuring that only valid, consistent changes are ever written to the live database.

When the third-party application exits or closes, the Program:
- Deletes the working database files.
- Restores the original database files from the backup.

If the restoration fails at exit, the Program performs the restore operation at the third-party application's next startup before any other Program work begins.

This architecture provides high compatibility between different database-modifying components and guarantees the database's original or "starting" state is always preserved, so users can safely revert in the event of a fatal failure.

## Program Flow

- **Startup**
    - Create backup database
    - Create working database
    - Third-party application's in-use database
- **Runtime**
    - Framework plugin uses Program calls to retrieve database data
    - Framework plugin modifies retrieved data and queue database changes
    - The Program executes queued modifications on the working database
    - The Program batches and executes modifications on the in-use database
- **Exit**
    - Delete working database
    - Overwrite in-use database with backup database

### Making a BepInEx plugin utilizing the Framework

I recommend using Visual Studio for an IDE. Your project will need the following references.

**References**
- `DatabaseModificationFramework.dll`
- `Assembly-CSharp.dll`
- `BepInEx.dll`
- `BepInEx.Preloader.dll`
- `Mono.Data.Sqlite.dll`
- `Newtonsoft.Json.dll`
- `System.dll`
- `System.Data.dll`
- `UnityEngine.dll`
- `UnityEngine.CoreModule.dll`

### Applicable Directories

These assemblies can typically be found within the following directories:

- `Tunguska The Visitation/Tunguska_Data/Managed/`
- `BepInEx/core/`

For a simple example on implementing the framework into your own BepInEx plugins, check the [Template](https://github.com/nate-sheltry/Database-Modification-Framework/blob/Graduation/Template.md) file.
