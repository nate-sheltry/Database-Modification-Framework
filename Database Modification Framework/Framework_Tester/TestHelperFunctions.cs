using Database_Modification_Framework.Definitions;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework_Tester
{
    public partial class Main
    {
        public bool TestNonRegItem(ISQLItem item)
        {
            switch (item)
            {
                case NonRegionalItem x:
                    new NonRegionalItem(x);
                    Utils.LogInfo("Successfully casted item as NonRegional Item.");
                    return true;
                case SlaughterhouseEnemy x:
                    new SlaughterhouseEnemy(x);
                    Utils.LogInfo("Successfully casted item as Slaughterhouse Enemy.");
                    return true;
                case SlaughterhouseProp x:
                    new SlaughterhouseProp(x);
                    Utils.LogInfo("Successfully casted item as Slaughterhouse Prop.");
                    return true;
                default:
                    Utils.LogError("Failed to cast item to any NonRegional Type.");
                    return false;
            }
        }
        public bool TestLocSqlItem(ISQLItem item)
        {
            switch (item)
            {
                case LocBaseItem x:
                    new LocBaseItem(x);
                    Utils.LogInfo("Successfully casted item as LocBaseItem.");
                    return true;
                case LocGlobalResp x:
                    new LocGlobalResp(x);
                    Utils.LogInfo("Successfully casted item as LocGlobalResp .");
                    return true;
                case LocJrnlEntry x:
                    Utils.LogInfo("Successfully casted item as LocJrnlEntry.");
                    return true;
                case LocFactRel x:
                    Utils.LogInfo("Successfully casted item as LocFactRel.");
                    return true;
                case LocStoryConTrigger x:
                    Utils.LogInfo("Successfully casted item as LocStoryConTrigger .");
                    return true;
                case LocNotePaper x:
                    Utils.LogInfo("Successfully casted item as LocNotePaper.");
                    return true;
                case LocEnvSounds x:
                    Utils.LogInfo("Successfully casted item as LocEnvSounds.");
                    return true;
                case LocTaskData x:
                    Utils.LogInfo("Successfully casted item as LocTaskData .");
                    return true;
                case LocInitSquads x:
                    Utils.LogInfo("Successfully casted item as LocInitSquads.");
                    return true;
                case LocPresetChar x:
                    Utils.LogInfo("Successfully casted item as LocPresetChar.");
                    return true;
                case LocConversation x:
                    Utils.LogInfo("Successfully casted item as LocConversation .");
                    return true;
                case LocNewsEntry x:
                    Utils.LogInfo("Successfully casted item as LocNewsEntry.");
                    return true;
                case LocStoryConItem x:
                    Utils.LogInfo("Successfully casted item as LocStoryConItem .");
                    return true;
                case LocStash x:
                    Utils.LogInfo("Successfully casted item as LocStash.");
                    return true;
                case LocRecipe x:
                    Utils.LogInfo("Successfully casted item as LocRecipe.");
                    return true;
                case LocSkillPerks x:
                    Utils.LogInfo("Successfully casted item as LocSkillPerks .");
                    return true;
                case LocFaction x:
                    Utils.LogInfo("Successfully casted item as LocFaction.");
                    return true;
                case LocLvlData x:
                    Utils.LogInfo("Successfully casted item as LocLvlData .");
                    return true;
                case LocTutorials x:
                    Utils.LogInfo("Successfully casted item as LocTutorials.");
                    return true;
                default:
                    Utils.LogError("Failed to cast item to any NonRegional Type.");
                    return false;
            }
        }

        public ISQLItem ReadInItem(Func<string, ISQLItem> func, string id)
        {
            var item = func(id);
            if (item == null)
            {
                Utils.LogWarning($"Failed to get, {id}");
            }
            item.Sync();
            return item;
        }
        private void TestLocItems<Titem>(List<Titem> items, ref List<bool> conditions)
        {
            foreach (ISQLItem item in items)
            {
                if (item == null)
                {
                    conditions.Add(false);
                    Utils.LogError("Got null Item getting Localized data,");
                    continue;
                }
                if (TestLocSqlItem(item))
                {
                    item.Sync();
                    conditions.Add(item != null);
                }
                else
                {
                    conditions.Add(false);
                    Utils.LogError("Could not cast LocItem to any class type.");
                }
            }
        }
        public bool TestAllLocDbs(Func<string, Enums.Databases, ISQLItem> func, string value)
        {
            var conditions = new List<bool>();
            List<ISQLItem> locItems = new List<ISQLItem>();
            locItems.Add(func(value, Enums.Databases.Main));
            locItems.Add(func(value, Enums.Databases.MainCH));
            locItems.Add(func(value, Enums.Databases.MainFR));
            locItems.Add(func(value, Enums.Databases.MainGER));
            locItems.Add(func(value, Enums.Databases.MainITA));
            locItems.Add(func(value, Enums.Databases.MainRUS));
            locItems.Add(func(value, Enums.Databases.MainSPA));
            locItems.Add(func(value, Enums.Databases.MainUA));
            TestLocItems(locItems, ref conditions);
            return !conditions.Contains(false);
        }
        public bool TestAllLocDbs(
            Func<string, string, string, Enums.Databases, ISQLItem> func,
            string value1, string value2, string value3)
        {
            var conditions = new List<bool>();
            List<ISQLItem> locItems = new List<ISQLItem>();
            locItems.Add(func(value1, value2, value3, Enums.Databases.Main));
            locItems.Add(func(value1, value2, value3, Enums.Databases.MainCH));
            locItems.Add(func(value1, value2, value3, Enums.Databases.MainFR));
            locItems.Add(func(value1, value2, value3, Enums.Databases.MainGER));
            locItems.Add(func(value1, value2, value3, Enums.Databases.MainITA));
            locItems.Add(func(value1, value2, value3, Enums.Databases.MainRUS));
            locItems.Add(func(value1, value2, value3, Enums.Databases.MainSPA));
            locItems.Add(func(value1, value2, value3, Enums.Databases.MainUA));
            TestLocItems(locItems, ref conditions);
            return !conditions.Contains(false);
        }
        public bool TestAllLocDbs(
            Func<int, int, Enums.Databases, ISQLItem> func,
            int value1, int value2)
        {
            var conditions = new List<bool>();
            List<ISQLItem> locItems = new List<ISQLItem>();
            locItems.Add(func(value1, value2, Enums.Databases.Main));
            locItems.Add(func(value1, value2, Enums.Databases.MainCH));
            locItems.Add(func(value1, value2, Enums.Databases.MainFR));
            locItems.Add(func(value1, value2, Enums.Databases.MainGER));
            locItems.Add(func(value1, value2, Enums.Databases.MainITA));
            locItems.Add(func(value1, value2, Enums.Databases.MainRUS));
            locItems.Add(func(value1, value2, Enums.Databases.MainSPA));
            locItems.Add(func(value1, value2, Enums.Databases.MainUA));
            TestLocItems(locItems, ref conditions);
            return !conditions.Contains(false);
        }
        public bool TestAllLocDbs(Func<int, Enums.Databases, ISQLItem> func, int value)
        {
            var conditions = new List<bool>();
            List<ISQLItem> locItems = new List<ISQLItem>();
            locItems.Add(func(value, Enums.Databases.Main));
            locItems.Add(func(value, Enums.Databases.MainCH));
            locItems.Add(func(value, Enums.Databases.MainFR));
            locItems.Add(func(value, Enums.Databases.MainGER));
            locItems.Add(func(value, Enums.Databases.MainITA));
            locItems.Add(func(value, Enums.Databases.MainRUS));
            locItems.Add(func(value, Enums.Databases.MainSPA));
            locItems.Add(func(value, Enums.Databases.MainUA));
            TestLocItems(locItems, ref conditions);
            return !conditions.Contains(false);
        }
        public bool TestAllLocDbs<Tenum, Titem>(
            Func<Enums.Databases, List<(Tenum, object)>, List<Titem>> func,
            List<(Tenum, object)> values) where Titem : ISQLItem
        {
            var conditions = new List<bool>();
            List<Titem> locItems = new List<Titem>();
            locItems.AddRange(func( Enums.Databases.Main, values));
            locItems.AddRange(func( Enums.Databases.MainCH, values));
            locItems.AddRange(func( Enums.Databases.MainFR, values));
            locItems.AddRange(func( Enums.Databases.MainGER, values));
            locItems.AddRange(func( Enums.Databases.MainITA, values));
            locItems.AddRange(func( Enums.Databases.MainRUS, values));
            locItems.AddRange(func( Enums.Databases.MainSPA, values));
            locItems.AddRange(func( Enums.Databases.MainUA, values));
            TestLocItems(locItems, ref conditions);
            return !conditions.Contains(false);
        }
        public bool TestAllLocDbs<Titem>(
            Func<Enums.Databases, List<Titem>> func) where Titem : ISQLItem
        {
            var conditions = new List<bool>();
            List<Titem> locItems = new List<Titem>();
            locItems.AddRange(func(Enums.Databases.Main));
            locItems.AddRange(func(Enums.Databases.MainCH));
            locItems.AddRange(func(Enums.Databases.MainFR));
            locItems.AddRange(func(Enums.Databases.MainGER));
            locItems.AddRange(func(Enums.Databases.MainITA));
            locItems.AddRange(func(Enums.Databases.MainRUS));
            locItems.AddRange(func(Enums.Databases.MainSPA));
            locItems.AddRange(func(Enums.Databases.MainUA));
            TestLocItems(locItems, ref conditions);
            return !conditions.Contains(false);
        }
    }
}
