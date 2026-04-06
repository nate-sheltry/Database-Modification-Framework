using BepInEx.Logging;
using Database_Modification_Framework.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using static Database_Modification_Framework.Framework;

namespace Database_Modification_Framework
{
    internal static class Setup
    {
        internal static void initialize()
        {
            FrameworkUtils.LoadConfig();
            FrameworkUtils.DetermineMaxTX();
            //exit will clean up any previous runtime leftovers in the case
            //we failed to clean up properly when the executable closed.
            exit();
            //For testing, will log our directory for manual verification.
            FrameworkUtils.InternalLog(
                LogLevel.Debug, 
                Directories.mainDatabase
            );
            Dictionary<string, string> databases = new Dictionary<string, string>();
            try
            {
                Directory.CreateDirectory(Directories.backupDatabase);
                Directory.CreateDirectory(Directories.workingDatabase);
                string[] files = Directory.GetFiles(Directories.mainDatabase) ??
                    throw new Exception(
                        $"Unable to attain database files in: {Directories.mainDatabase}"
                    );
                foreach (string f in files)
                {
                    File.Copy(f, Path.Combine(Directories.backupDatabase, Path.GetFileName(f)));
                    File.Copy(f, Path.Combine(Directories.workingDatabase, Path.GetFileName(f)));
                    string file2 = Path.GetFileName(f);
                    databases.Add(file2.Replace(".db", ""), file2);
                    //For testing
                    FrameworkUtils.InternalLog(
                        LogLevel.Info,
                        file2.Replace(".db", "")
                    );
                }
                FrameworkUtils.Databases = databases;
                DatabaseManager.InitializeDb();
                SQLQueue.Initialize();
                return;
            }
            catch (Exception ex)
            {
                // Fatal because failure to initialize results in all
                // functionality lost.
                FrameworkUtils.InternalLog(
                    LogLevel.Fatal, 
                    $"Failed to initialize Database Framework: {ex.Message}"
                );
            }
            FrameworkUtils.InternalLog(
                LogLevel.Fatal, 
                "Setup Failed: Encountered Fatal Error during Setup process."
            );
            throw new Exception("Setup Failed: Encountered Fatal Error during Setup process.");
        }

        public static void exit()
        {
            //If there is no backup database directory, then the cleanup succeeded
            if (!Directory.Exists(Directories.backupDatabase) && !Directory.Exists(Directories.workingDatabase))
                return;
            try
            {
                DatabaseManager.CloseConnections();
                string[] files = Directory.GetFiles(Directories.backupDatabase) ??
                    throw new Exception(
                        $"Was unable to attain database files in: {Directories.backupDatabase}"
                    );
                foreach (string f in files)
                {
                    File.Copy(f, Path.Combine(
                        Directories.mainDatabase, Path.GetFileName(f)), 
                        overwrite: true);
                    File.Delete(f);
                }
                Directory.Delete(Directories.backupDatabase);
                string[] files2 = Directory.GetFiles(Directories.workingDatabase) ??
                    throw new Exception(
                        $"Was unable to attain database files in: {Directories.workingDatabase}"
                    );
                foreach (string f in files2)
                {
                    File.Delete(f);
                }
                Directory.Delete(Directories.workingDatabase);
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to cleanup database: {ex.Message}"
                );
            }
        }
    }
}
