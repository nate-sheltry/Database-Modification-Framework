using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Diagnostics;
using Database_Modification_Framework.Definitions;
using System.IO;
using System.Linq.Expressions;

namespace Database_Modification_Framework
{
    internal static class Setup
    {
        public static Dictionary<string, string> initialize()
        {
            //exit will clean up any previous runtime leftovers in the case
            //we failed to clean up properly when the executable closed.
            exit();
            //For testing, will log our directory for manual verification.
            Utils.Log.LogMessage(Directories.mainDatabase);
            Dictionary<string, string> databases = new Dictionary<string, string>();
            try
            {
                Directory.CreateDirectory(Directories.backupDatabase);
                string[] files = Directory.GetFiles(Directories.mainDatabase) ??
                    throw new Exception(
                        $"Unable to attain database files in: {Directories.mainDatabase}"
                    );
                foreach (string f in files)
                {
                    File.Copy(f, Path.Combine(Directories.backupDatabase, Path.GetFileName(f)));
                    string file = Path.GetFileName(f);
                    databases.Add(file.Replace(".db",""), file);
                    //For testing
                    Utils.Log.LogMessage(file.Replace(".db", ""));
                }
                return databases;
            }
            catch (Exception ex)
            {
                Utils.Log.LogError($"Failed to initialize Database Framework: {ex.Message}");
            }
            return null;
        }

        public static void exit()
        {
            //If there is no backup database directory, then the cleanup succeeded
            if (!Directory.Exists(Directories.backupDatabase))
                return;
            try
            {
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
            }
            catch (Exception ex)
            {
                Utils.Log.LogError($"Failed to cleanup database: {ex.Message}");
            }
        }
    }
}
