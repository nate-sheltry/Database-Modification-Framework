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
                    return true;
                case SlaughterhouseEnemy x:
                    new SlaughterhouseEnemy(x);
                    return true;
                case SlaughterhouseProp x:
                    new SlaughterhouseProp(x);
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
                    return true;
                case LocGlobalResp x:
                    new LocGlobalResp(x);
                    return true;
                case LocJrnlEntry x:
                    return true;
                case LocFactRel x:
                    return true;
                case LocStoryConTrigger x:
                    return true;
                case LocNotePaper x:
                    return true;
                case LocEnvSounds x:
                    return true;
                case LocTaskData x:
                    return true;
                case LocInitSquads x:
                    return true;
                case LocPresetChar x:
                    return true;
                case LocConversation x:
                    return true;
                case LocNewsEntry x:
                    return true;
                case LocStoryConItem x:
                    return true;
                case LocStash x:
                    return true;
                case LocRecipe x:
                    return true;
                case LocSkillPerks x:
                    return true;
                case LocFaction x:
                    return true;
                case LocLvlData x:
                    return true;
                case LocTutorials x:
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
