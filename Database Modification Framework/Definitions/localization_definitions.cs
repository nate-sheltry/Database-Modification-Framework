using BepInEx.Logging;
using Database_Modification_Framework.Database;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityStandardAssets.Water;
using static Mono.Security.X509.X520;
using static Database_Modification_Framework.FrameworkUtils;

namespace Database_Modification_Framework.Definitions
{
    public abstract class LocSQLItem : SQLItem
    {
        internal virtual void SetUpObject(object data)
        {
            try
            {
                _database = GetPropValue<Enums.Databases>(data, "_database");
                if(!Enums.LocalizationDatabases.Contains(_database))
                {
                    throw new ArgumentException("Wrong DatabaseType, must be a \"Main\" database value.");
                }    
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to read in _database when creating Localization class\n{ex}"
                );
            }
        }
        internal virtual void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            try
            {
                _database = db;
                if (!Enums.LocalizationDatabases.Contains(_database))
                {
                    throw new ArgumentException("Wrong DatabaseType, must be a Main database value.");
                }
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to read in _database when creating Localization class\n{ex}"
                );
            }
        }
        protected internal void CreateInstance(Action action)
        {
            try
            {
                action();
            } catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed to create {GetType().Name}\n{ex}"
                );
            }
        }
    }
    public class LocGlobalResp : LocSQLItem
    {
        public string Id { get; set; }
        public string Response { get; set; }
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.global_dialogue_response;
            Id = GetPropValue<string>(data, "Id");
            Response = GetPropValue<string>(data, "Response");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.global_dialogue_response;
            Id = reader.GetString((int)Enums.LocGlobResp.id);
            Response = reader.GetString((int)Enums.LocGlobResp.response);
        }
        public LocGlobalResp(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocGlobalResp(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.Parameters.AddWithValue("Response", Response);
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocGlobResp.response.ToString()} = @Response
                WHERE {Enums.LocGlobResp.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocJrnlEntry : LocSQLItem
    {
        public int Id;
        public string Entry;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.journal_entries;
            Id = GetPropValue<int>(data, "Id");
            Entry = GetPropValue<string>(data, "Entry");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.journal_entries;
            Id = reader.GetInt32((int)Enums.LocJrnlEntries.id);
            Entry = reader.GetString((int)Enums.LocJrnlEntries.entry);
        }
        public LocJrnlEntry(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocJrnlEntry(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.Parameters.AddWithValue("Entry", Entry);
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET {Enums.LocJrnlEntries.entry.ToString()} = @Entry
                WHERE {Enums.LocJrnlEntries.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocFactRel : LocSQLItem
    {
        public int FactionId;
        public int TargetFactionId;
        public double Relationship;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.faction_relationships;
            FactionId = GetPropValue<int>(data, "FactionId");
            TargetFactionId = GetPropValue<int>(data, "TargetFactionId");
            Relationship = GetPropValue<double>(data, "Relationship");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.faction_relationships;
            FactionId = reader.GetInt32((int)Enums.LocFactionRel.faction_id);
            TargetFactionId = reader.GetInt32((int)Enums.LocFactionRel.target_faction_id);
            Relationship = reader.GetDouble((int)Enums.LocFactionRel.relationship);
        }
        public LocFactRel(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocFactRel(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("FactionId", FactionId);
            cmd.Parameters.AddWithValue("TargetId", TargetFactionId);
            cmd.Parameters.AddWithValue("Relationship", Relationship);
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET {Enums.LocFactionRel.relationship.ToString()} = @Relationship
                WHERE {Enums.LocFactionRel.faction_id.ToString()} = @FactionId 
                AND {Enums.LocFactionRel.target_faction_id.ToString()} = @TargetId;";
            return cmd;
        }
    }
    public class LocStoryConTrigger : LocSQLItem
    {
        public string Id;
        public int InitialValue;
        public bool IsActive;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.story_conditions_trigger;
            Id = GetPropValue<string>(data, "Id");
            InitialValue = GetPropValue<int>(data, "InitialValue");
            IsActive = GetPropValue<bool>(data, "IsActive");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.story_conditions_trigger;
            Id = reader.GetString((int)Enums.LocStoryConTriggers.id);
            InitialValue = reader.GetInt32((int)Enums.LocStoryConTriggers.initial_value);
            IsActive = reader.GetInt32((int)Enums.LocStoryConTriggers.is_active) > 0;
        }
        public LocStoryConTrigger(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocStoryConTrigger(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.Parameters.AddWithValue("InitialValue", InitialValue);
            cmd.Parameters.AddWithValue("IsActive", IsActive ? 1 : 0);
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocStoryConTriggers.initial_value.ToString()} = @InitialValue,
                    {Enums.LocStoryConTriggers.is_active.ToString()} = @IsActive
                WHERE {Enums.LocStoryConTriggers.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocNotePaper : LocSQLItem
    {
        public string Id;
        public string Text;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.note_papers;
            Id = GetPropValue<string>(data, "Id");
            Text = GetPropValue<string>(data, "Text");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.note_papers;
            Id = reader.GetString((int)Enums.LocNotePapers.id);
            Text = reader.GetString((int)Enums.LocNotePapers.text);
        }
        public LocNotePaper(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocNotePaper(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.Parameters.AddWithValue("Text", Text);
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocNotePapers.text.ToString()} = @Text
                WHERE {Enums.LocNotePapers.id.ToString()} = @Id ;";
            return cmd;
        }
    }
    public class LocEnvSounds : LocSQLItem
    {
        public string Location;
        public string Time;
        public string Weather;
        public string PrimarySet;
        public string SecondarySet;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.environment_sounds;
            Location = GetPropValue<string>(data, "Location");
            Time = GetPropValue<string>(data, "Time");
            Weather = GetPropValue<string>(data, "Weather");
            PrimarySet = GetPropValue<string>(data, "PrimarySet");
            SecondarySet = GetPropValue<string>(data, "SecondarySet");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.environment_sounds;
            Location = reader.GetString((int)Enums.LocEnvSounds.location);
            Time = reader.GetString((int)Enums.LocEnvSounds.time);
            Weather = reader.GetString((int)Enums.LocEnvSounds.weather);
            PrimarySet = reader.GetString((int)Enums.LocEnvSounds.primary_set);
            SecondarySet = reader.GetString((int)Enums.LocEnvSounds.secondary_set);
        }
        public LocEnvSounds(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocEnvSounds(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("Location", Location);
            cmd.Parameters.AddWithValue("Time", Time);
            cmd.Parameters.AddWithValue("Weather", Weather);
            cmd.Parameters.AddWithValue("PrimarySet", PrimarySet);
            cmd.Parameters.AddWithValue("SecondarySet", SecondarySet);
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocEnvSounds.primary_set.ToString()} = @PrimarySet,
                    {Enums.LocEnvSounds.secondary_set.ToString()} = @SecondarySet
                WHERE
                    {Enums.LocEnvSounds.location.ToString()} = @Location
                    AND {Enums.LocEnvSounds.time.ToString()} = @Time
                    AND {Enums.LocEnvSounds.weather.ToString()} = @Weather;";
            return cmd;
        }
    }
    public class LocTaskData : LocSQLItem
    {
        public int Id;
        public string Text;
        public int JournalId;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.task_data;
            Id = GetPropValue<int>(data, "Id");
            Text = GetPropValue<string>(data, "Text");
            JournalId = GetPropValue<int>(data, "JournalId");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.task_data;
            Id = reader.GetInt32((int)Enums.LocTaskData.id);
            Text = reader.GetString((int)Enums.LocTaskData.text);
            JournalId = reader.GetInt32((int)Enums.LocTaskData.journal_id);
        }
        public LocTaskData(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocTaskData(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.Parameters.AddWithValue("Text", Text);
            cmd.Parameters.AddWithValue("JournalId", JournalId);
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocTaskData.text.ToString()} = @Text,
                    {Enums.LocTaskData.journal_id.ToString()} = @JournalId
                WHERE {Enums.LocTaskData.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocInitSquads : LocSQLItem
    {
        public string Id;
        public int Tier;
        public int Faction;
        public string Household;
        public string Level;
        public bool Respawn;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.initial_squads;
            Id = GetPropValue<string>(data, "Id");
            Tier = GetPropValue<int>(data, "Tier");
            Faction = GetPropValue<int>(data, "Faction");
            Household = GetPropValue<string>(data, "Household");
            Level = GetPropValue<string>(data, "Level");
            Respawn = GetPropValue<bool>(data, "Respawn");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.initial_squads;
            Id = reader.GetString((int)Enums.LocInitSquads.id);
            Tier = reader.GetInt32((int)Enums.LocInitSquads.tier);
            Faction = reader.GetInt32((int)Enums.LocInitSquads.faction);
            Household = reader.GetString((int)Enums.LocInitSquads.household);
            Level = reader.GetString((int)Enums.LocInitSquads.level);
            Respawn = reader.GetInt32((int)Enums.LocInitSquads.respawn) > 0;
        }
        public LocInitSquads(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocInitSquads(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("Id",Id),
                new SqliteParameter("Tier", Tier),
                new SqliteParameter("Faction", Faction),
                new SqliteParameter("Household", Household),
                new SqliteParameter("Level", Level),
                new SqliteParameter("Respawn", Respawn ? 1 : 0),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocInitSquads.tier.ToString()} = @Tier,
                    {Enums.LocInitSquads.faction.ToString()} = @Faction,
                    {Enums.LocInitSquads.household.ToString()} = @Household,
                    {Enums.LocInitSquads.level.ToString()} = @Level,
                    {Enums.LocInitSquads.respawn.ToString()} = @Respawn
                WHERE {Enums.LocInitSquads.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocPresetChar : LocSQLItem
    {
        public string Id;
        public string Level;
        public double PosX;
        public double PosY;
        public double PosZ;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.preset_characters;
            Id = GetPropValue<string>(data, "Id");
            Level = GetPropValue<string>(data, "Level");
            PosX = GetPropValue<double>(data, "PosX");
            PosY = GetPropValue<double>(data, "PosY");
            PosZ = GetPropValue<double>(data, "PosZ");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.preset_characters;
            Id = reader.GetString((int)Enums.LocPresetChar.id);
            Level = reader.GetString((int)Enums.LocPresetChar.level);
            PosX = reader.GetDouble((int)Enums.LocPresetChar.position_x);
            PosY = reader.GetDouble((int)Enums.LocPresetChar.position_y);
            PosZ = reader.GetDouble((int)Enums.LocPresetChar.position_z);
        }
        public LocPresetChar(object data) =>
            CreateInstance(() => SetUpObject(data));
        public LocPresetChar(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("Id", Id),
                new SqliteParameter("Level", Level),
                new SqliteParameter("PosX", PosX),
                new SqliteParameter("PosY", PosY),
                new SqliteParameter("PosZ", PosZ),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocPresetChar.level.ToString()} = @Level,
                    {Enums.LocPresetChar.position_x.ToString()} = @PosX,
                    {Enums.LocPresetChar.position_y.ToString()} = @PosY,
                    {Enums.LocPresetChar.position_z.ToString()} = @PosZ,
                WHERE {Enums.LocPresetChar.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocConversation : LocSQLItem
    {
        public int Id;
        public int FactionId;
        public string Line;
        public int Type;
        public int Emotion;
        public string Responses;
        public bool IsForProcedural;
        public string TriggerScript;
        public bool IsEnabled;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.conversation;
            Id = GetPropValue<int>(data, "Id");
            FactionId = GetPropValue<int>(data, "FactionId");
            Type = GetPropValue<int>(data, "Type");
            Emotion = GetPropValue<int>(data, "Emotion");
            Line = GetPropValue<string>(data, "Line");
            Responses = GetPropValue<string>(data, "Response");
            TriggerScript = GetPropValue<string>(data, "TriggerScript");
            IsForProcedural = GetPropValue<bool>(data, "IsForProcedural");
            IsEnabled = GetPropValue<bool>(data, "IsEnabled");

        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.conversation;
            Id = reader.GetInt32((int)Enums.LocConvos.id);
            FactionId = reader.GetInt32((int)Enums.LocConvos.faction_id);
            Type = reader.GetInt32((int)Enums.LocConvos.type);
            Emotion = reader.GetInt32((int)Enums.LocConvos.emotion);
            Line = reader.GetString((int)Enums.LocConvos.line);
            Responses = reader.GetString((int)Enums.LocConvos.responses);
            TriggerScript = reader.GetString((int)Enums.LocConvos.trigger_script);
            IsForProcedural = reader.GetInt32((int)Enums.LocConvos.is_for_procedural) > 0;
            IsEnabled = reader.GetInt32((int)Enums.LocConvos.is_enabled) > 0;
        }
        public LocConversation(object data) => CreateInstance(() => SetUpObject(data));
        public LocConversation(IDataReader reader, Enums.Databases db) => 
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{ 
                new SqliteParameter("Id",Id),
                new SqliteParameter("FactionId",FactionId),
                new SqliteParameter("Type",Type),
                new SqliteParameter("Emotion",Emotion),
                new SqliteParameter("Line",Line),
                new SqliteParameter("Responses",Responses),
                new SqliteParameter("TriggerScript",TriggerScript),
                new SqliteParameter("IsForProcedural",IsForProcedural ? 1 : 0),
                new SqliteParameter("IsEnabled",IsEnabled ? 1 : 0)
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocConvos.faction_id.ToString()} = @FactionId,
                    {Enums.LocConvos.type.ToString()} = @Type,
                    {Enums.LocConvos.emotion.ToString()} = @Emotion,
                    {Enums.LocConvos.line.ToString()} = @Line,
                    {Enums.LocConvos.responses.ToString()} = @Responses,
                    {Enums.LocConvos.trigger_script.ToString()} = @TriggerScript,
                    {Enums.LocConvos.is_for_procedural.ToString()} = @IsForProcedural,
                    {Enums.LocConvos.is_enabled.ToString()} = @IsEnabled
                WHERE {Enums.LocConvos.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocNewsEntry : LocSQLItem
    {
        public int Id;
        public string Content;
        public int Pay;
        public string TopicId;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.news_entries;
            Id = GetPropValue<int>(data, "Id");
            Pay = GetPropValue<int>(data, "Pay");
            Content = GetPropValue<string>(data, "Content");
            TopicId = GetPropValue<string>(data, "TopicId");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.news_entries;
            Id = reader.GetInt32((int)Enums.LocNewsEntries.id);
            Pay = reader.GetInt32((int)Enums.LocNewsEntries.pay);
            Content = reader.GetString((int)Enums.LocNewsEntries.content);
            TopicId = reader.GetString((int)Enums.LocNewsEntries.topic_id);
        }
        public LocNewsEntry(object data) => CreateInstance(() => SetUpObject(data));
        public LocNewsEntry(IDataReader reader, Enums.Databases db) =>
            CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("Id",Id),
                new SqliteParameter("Content",Content),
                new SqliteParameter("Pay",Pay),
                new SqliteParameter("TopicId",TopicId),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocNewsEntries.content.ToString()} = @Content,
                    {Enums.LocNewsEntries.pay.ToString()} = @Pay,
                    {Enums.LocNewsEntries.topic_id.ToString()} = @TopicId
                WHERE {Enums.LocConvos.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocStoryConItem : LocSQLItem
    {
        public string Id;
        public string ItemId;
        public bool IsActive;
        public bool IsDurability;
        public bool IsType;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.story_conditions_item;
            Id = GetPropValue<string>(data, "Id");
            ItemId = GetPropValue<string>(data, "ItemId");
            IsActive = GetPropValue<bool>(data, "IsActive");
            IsDurability = GetPropValue<bool>(data, "IsDurability");
            IsType = GetPropValue<bool>(data, "IsType");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.story_conditions_item;
            Id = reader.GetString((int)Enums.LocStoryConItems.id);
            ItemId = reader.GetString((int)Enums.LocStoryConItems.item_id);
            IsActive = reader.GetInt32((int)Enums.LocStoryConItems.is_active) > 0;
            IsDurability = reader.GetInt32((int)Enums.LocStoryConItems.is_durability) > 0;
            IsType = reader.GetInt32((int)Enums.LocStoryConItems.is_type) > 0;
        }
        public LocStoryConItem(object data) => CreateInstance(() => SetUpObject(data));
        public LocStoryConItem(IDataReader reader, Enums.Databases db) => CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("Id", Id),
                new SqliteParameter("ItemId", ItemId),
                new SqliteParameter("IsActive",IsActive ? 1 : 0),
                new SqliteParameter("IsDurability",IsDurability ? 1 : 0),
                new SqliteParameter("IsType",IsType ? 1 : 0),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocStoryConItems.item_id.ToString()} = @ItemId,
                    {Enums.LocStoryConItems.is_active.ToString()} = @IsActive,
                    {Enums.LocStoryConItems.is_durability.ToString()} = @IsDurability,
                    {Enums.LocStoryConItems.is_type.ToString()} = @IsType
                WHERE {Enums.LocStoryConItems.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocStash : LocSQLItem
    {
        public string StashName;
        public string StashNotes;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.stashes;
            StashName = GetPropValue<string>(data, "StashName");
            StashNotes = GetPropValue<string>(data, "StashNote");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.stashes;
            StashName = reader.GetString((int)Enums.LocStashes.stash_name);
            StashNotes = reader.GetString((int)Enums.LocStashes.stash_notes);
        }
        public LocStash(object data) => CreateInstance(() => SetUpObject(data));
        public LocStash(IDataReader reader, Enums.Databases db) => CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("StashName", StashName),
                new SqliteParameter("StashNotes", StashNotes),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET {Enums.LocStashes.stash_notes.ToString()} = @StashNotes
                WHERE {Enums.LocStashes.stash_name.ToString()} = @StashName;";
            return cmd;
        }
    }
    public class LocRecipe : LocSQLItem
    {
        public string SerumId;
        public List<string> Ingredients;
        public List<int> IngredientsCounts;
        public double Temperature;
        public Enums.RecipePlatforms Platform;
        public int Quantity;
        public double Hours;
        internal override void SetUpObject(object data)
        {
            
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.recipes;
            SerumId = GetPropValue<string>(data, "SerumId");
            Ingredients = GetPropValue<List<string>>(data, "Ingredients");
            IngredientsCounts = GetPropValue<List<int>>(data, "IngredientsCounts");
            Temperature = GetPropValue<double>(data, "Temperature");
            Platform = GetPropValue<Enums.RecipePlatforms>(data, "Platform");
            Quantity = GetPropValue<int>(data, "Quantity");
            Hours = GetPropValue<double>(data, "Hours");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            List<string> ingreds = new List<string>();
            List<int> counts = new List<int>();
            _table = Enums.LocalizationTables.recipes;
            SerumId = reader.GetString((int)Enums.LocRecipes.serum_id);
            Temperature = reader.GetDouble((int)Enums.LocRecipes.temperature);
            Enum.TryParse(reader.GetString((int)Enums.LocRecipes.platform), out Platform);
            Quantity = reader.GetInt32((int)Enums.LocRecipes.quantity);
            Hours = reader.GetDouble((int)Enums.LocRecipes.hours);
            ingreds.Add(reader.GetString((int)Enums.LocRecipes.ingredients));
            counts.Add(reader.GetInt32((int)Enums.LocRecipes.ingredients_count));
            while(reader.Read())
            {
                if (reader.GetString((int)Enums.LocRecipes.serum_id) != SerumId)
                    break;
                ingreds.Add(reader.GetString((int)Enums.LocRecipes.ingredients));
                counts.Add(reader.GetInt32((int)Enums.LocRecipes.ingredients_count));
            }
            Ingredients = ingreds;
            IngredientsCounts = counts;
        }
        public LocRecipe(object data) => CreateInstance(() => SetUpObject(data));
        public LocRecipe(IDataReader reader, Enums.Databases db) => CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("SerumId",SerumId),
                new SqliteParameter("Temperature",Temperature),
                new SqliteParameter("Quantity",Quantity),
                new SqliteParameter("Hours",Hours),
                new SqliteParameter("Platform",Platform.ToString()),
            });
            cmd.CommandText = "";
            if (Ingredients == null || Ingredients.Count == 0)
            {
                cmd.CommandText = $@"
                    UPDATE {DbTable} SET 
                        {Enums.LocRecipes.temperature} = @Temperature,
                        {Enums.LocRecipes.quantity} = @Quantity,
                        {Enums.LocRecipes.hours} = @Hours,
                        {Enums.LocRecipes.platform} = @Platform
                    WHERE {Enums.LocRecipes.serum_id} = @SerumId;";
                return cmd;
            }
            try
            {
                for (int i = 0; i < Ingredients.Count; i++)
                {
                    cmd.Parameters.AddRange(new IDbDataParameter[]{
                        new SqliteParameter($"Ingredients{i}", Ingredients[i]),
                        new SqliteParameter($"IngCounts{i}", IngredientsCounts[i]),
                    });
                    cmd.CommandText += $@"
                    UPDATE {DbTable}
                    SET 
                        {Enums.LocRecipes.temperature.ToString()} = @Temperature,
                        {Enums.LocRecipes.quantity.ToString()} = @Quantity,
                        {Enums.LocRecipes.hours.ToString()} = @Hours,
                        {Enums.LocRecipes.platform.ToString()} = @Platform,
                        {Enums.LocRecipes.ingredients_count.ToString()} = @IngCounts{i}
                    WHERE
                        {Enums.LocRecipes.serum_id.ToString()} = @SerumId AND
                        {Enums.LocRecipes.ingredients.ToString()} = @Ingredients{i};";
                }
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(LogLevel.Error,
                    $"Failed to generate Sqlite Command for Recipe.\n{ex}");
                FrameworkUtils.InternalLog(LogLevel.Warning,
                    $"Applying Scalar update in place of multi-row update");
                cmd.CommandText = $@"
                    UPDATE {DbTable} SET 
                        {Enums.LocRecipes.temperature} = @Temperature,
                        {Enums.LocRecipes.quantity} = @Quantity,
                        {Enums.LocRecipes.hours} = @Hours,
                        {Enums.LocRecipes.platform} = @Platform
                    WHERE {Enums.LocRecipes.serum_id} = @SerumId;";
            }
            return cmd;
        }
        public void ScalarSync()
        {
            LocRecipe scalarItem = new LocRecipe(this)
            {
                Ingredients = null,
                IngredientsCounts = null
            };
            Framework.QueueSQL(scalarItem);
            scalarItem = null;
        }
    }
    public class LocSkillPerks : LocSQLItem
    {
        public string Id;
        public Skills SkillId;
        public int Sequence;
        public string Title;
        public string Description;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.skill_perks;
            Id = GetPropValue<string>(data, "Id");
            Enum.TryParse(GetPropValue<string>(data, "SkillId"), out SkillId);
            Sequence = GetPropValue<int>(data, "Sequence");
            Title = GetPropValue<string>(data, "Title");
            Description = GetPropValue<string>(data,"Description");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.skill_perks;
            Id = reader.GetString((int)Enums.LocSkillPerks.id);
            Enum.TryParse(reader.GetString((int)Enums.LocSkillPerks.id), out SkillId);
            Sequence = reader.GetInt32((int)Enums.LocSkillPerks.sequence);
            Title = reader.GetString((int)Enums.LocSkillPerks.title);
            Description = reader.GetString((int)Enums.LocSkillPerks.description);
        }
        public LocSkillPerks(object data) => CreateInstance(() => SetUpObject(data));
        public LocSkillPerks(IDataReader reader, Enums.Databases db) => CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("Id",Id),
                new SqliteParameter("SkillId",SkillId.ToString()),
                new SqliteParameter("Sequence",Sequence),
                new SqliteParameter("Title",Title),
                new SqliteParameter("Description",Description)
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocSkillPerks.skill_id.ToString()} = @SkillId,
                    {Enums.LocSkillPerks.sequence.ToString()} = @Sequence,
                    {Enums.LocSkillPerks.title.ToString()} = @Title,
                    {Enums.LocSkillPerks.description.ToString()} = @Description
                WHERE {Enums.LocSkillPerks.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocFaction : LocSQLItem
    {
        public int Id;
        public string Name;
        public string Models;
        public string ArmoredModels;
        public int CharacterType;
        public string DisplayName;
        public string EliteModel;
        public string DeadzoneModel;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.factions;
            Id = GetPropValue<int>(data, "Id");
            Name = GetPropValue<string>(data,"Name");
            Models = GetPropValue<string>(data, "Models");
            ArmoredModels = GetPropValue<string>(data, "ArmoredModels");
            CharacterType = GetPropValue<int>(data, "CharacterType");
            DisplayName = GetPropValue<string>(data, "DisplayName");
            EliteModel = GetPropValue<string>(data, "EliteModel");
            DeadzoneModel = GetPropValue<string>(data, "DeadzoneModel");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.factions;
            Id = reader.GetInt32((int)Enums.LocFactions.id);
            Name = reader.GetString((int)Enums.LocFactions.name);
            Models = reader.GetString((int)Enums.LocFactions.models);
            ArmoredModels = reader.GetString((int)Enums.LocFactions.armored_models);
            CharacterType = reader.GetInt32((int)Enums.LocFactions.character_type);
            DisplayName = reader.GetString((int)Enums.LocFactions.display_name);
            EliteModel = reader.GetString((int)Enums.LocFactions.elite_model);
            DeadzoneModel = reader.GetString((int)Enums.LocFactions.deadzone_model);
        }
        public LocFaction(object data) => CreateInstance(() => SetUpObject(data));
        public LocFaction(IDataReader reader, Enums.Databases db) => CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("Id",Id),
                new SqliteParameter("Name",Name),
                new SqliteParameter("Models",Models),
                new SqliteParameter("ArmoredModels",ArmoredModels),
                new SqliteParameter("CharacterType",CharacterType),
                new SqliteParameter("DisplayName",DisplayName),
                new SqliteParameter("EliteModel",EliteModel),
                new SqliteParameter("DeadzoneModel",DeadzoneModel)
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocFactions.name.ToString()} = @Name,
                    {Enums.LocFactions.models.ToString()} = @Models,
                    {Enums.LocFactions.armored_models.ToString()} = @ArmoredModels,
                    {Enums.LocFactions.character_type.ToString()} = @CharacterType,
                    {Enums.LocFactions.display_name.ToString()} = @DisplayName,
                    {Enums.LocFactions.elite_model.ToString()} = @EliteModel,
                    {Enums.LocFactions.deadzone_model.ToString()} = @DeadzoneModel
                WHERE {Enums.LocFactions.id.ToString()} = @Id;";
            return cmd;
        }
    }
    public class LocLvlData : LocSQLItem
    {
        public string Id;
        public string DisplayName;
        public string Description;
        public string MainLevelName;
        public double FogHeight;
        public bool AllowAirdrop;
        public double[] RainFogHeights;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.level_data;
            Id = GetPropValue<string>(data, "Id");
            DisplayName = GetPropValue<string>(data, "DisplayName");
            Description = GetPropValue<string>(data, "Description");
            MainLevelName = GetPropValue<string>(data, "MainLevelName");
            FogHeight = GetPropValue<double>(data, "FogHeight");
            AllowAirdrop = GetPropValue<bool>(data, "AllowAirdrop");
            RainFogHeights = GetPropValue<double[]>(data, "RainFogHeight");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.level_data;
            Id = reader.GetString((int)Enums.LocLvlData.id);
            DisplayName = reader.GetString((int)Enums.LocLvlData.display_name);
            Description = reader.GetString((int)Enums.LocLvlData.description);
            MainLevelName = reader.GetString((int)Enums.LocLvlData.main_level_name);
            FogHeight = reader.GetDouble((int)Enums.LocLvlData.fog_height);
            AllowAirdrop = reader.GetInt32((int)Enums.LocLvlData.allow_airdrop) > 0;
            RainFogHeights = reader.GetString((int)Enums.LocLvlData.rain_fog_heights)
                .Trim().Split('/').Select(double.Parse).ToArray();
        }
        public LocLvlData(object data) => CreateInstance(() => SetUpObject(data));
        public LocLvlData(IDataReader reader, Enums.Databases db) => CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("Id",Id),
                new SqliteParameter("DisplayName",DisplayName),
                new SqliteParameter("Description",Description),
                new SqliteParameter("MainLevelName",MainLevelName),
                new SqliteParameter("FogHeight",FogHeight),
                new SqliteParameter("AllowAirdrop",AllowAirdrop ? 1 : 0),
                new SqliteParameter("RainFogHeights",$"{RainFogHeights[0]}/{RainFogHeights[1]}"),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET
                    {Enums.LocLvlData.display_name.ToString()} = @DisplayName,
                    {Enums.LocLvlData.description.ToString()} = @Description,
                    {Enums.LocLvlData.main_level_name.ToString()} = @MainLevelName,
                    {Enums.LocLvlData.fog_height.ToString()} = @FogHeight,
                    {Enums.LocLvlData.allow_airdrop.ToString()} = @AllowAirdrop,
                    {Enums.LocLvlData.rain_fog_heights.ToString()} = @RainFogHeights
                WHERE {Enums.LocLvlData.id.ToString()} = @Id ;";
            return cmd;
        }
    }
    public class LocTutorials : LocSQLItem
    {
        public int Id;
        public string Title;
        public string Message;
        public string MessageController;
        public bool Disruptive;
        internal override void SetUpObject(object data)
        {
            base.SetUpObject(data);
            _table = Enums.LocalizationTables.tutorials;
            Id = GetPropValue<int>(data, "Id");
            Title = GetPropValue<string>(data,"Title");
            Message = GetPropValue<string>(data,"Message");
            MessageController = GetPropValue<string>(data, "MessageController");
            Disruptive = GetPropValue<bool>(data, "Disruptive");
        }
        internal override void SetUpObject(IDataReader reader, Enums.Databases db)
        {
            base.SetUpObject(reader, db);
            _table = Enums.LocalizationTables.tutorials;
            Id = reader.GetInt32((int)Enums.LocTutorials.id);
            Title = reader.GetString((int)Enums.LocTutorials.title);
            Message = reader.GetString((int)Enums.LocTutorials.message);
            MessageController = reader.GetString((int)Enums.LocTutorials.message_controller);
            Disruptive = reader.GetInt32((int)Enums.LocTutorials.disruptive) > 0;
        }
        public LocTutorials(object data) => CreateInstance(() => SetUpObject(data));
        public LocTutorials(IDataReader reader, Enums.Databases db) => CreateInstance(() => SetUpObject(reader, db));
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("Id",Id),
                new SqliteParameter("Title",Title),
                new SqliteParameter("Message",Message),
                new SqliteParameter("MessageController",MessageController),
                new SqliteParameter("Disruptive",Disruptive ? 1 : 0),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable}
                SET 
                    {Enums.LocTutorials.title.ToString()} = @Title,
                    {Enums.LocTutorials.message.ToString()} = @Message,
                    {Enums.LocTutorials.message_controller.ToString()} = @MessageController,
                    {Enums.LocTutorials.disruptive.ToString()} = @Disruptive
                WHERE {Enums.LocTutorials.id.ToString()} = @Id;";
            return cmd;
        }
    }
    // Template for creating Localization Class.
    //public class x : LocSQLItem
    //{
    //    internal override void SetUpObject(object data)
    //    {
    //        base.SetUpObject(data);
    //        _table = Enums.LocalizationTables.faction_relationships;
    //    }
    //    internal override void SetUpObject(IDataReader reader, Enums.Databases db)
    //    {
    //        base.SetUpObject(reader, db);
    //        _table = Enums.LocalizationTables.faction_relationships;
    //    }
    //    public x(object data) => CreateInstance(() => SetUpObject(data));
    //    public x(IDataReader reader, Enums.Databases db) => CreateInstance(() => SetUpObject(reader, db));
    //    public override IDbCommand GetSqlCommand()
    //    {
    //        SqliteCommand cmd = new SqliteCommand();
    //        cmd.Parameters.AddRange(new IDbDataParameter[]{
    //            new SqliteParameter("",),
    //            new SqliteParameter("",),
    //        });
    //        cmd.CommandText = $@"
    //            UPDATE {DbTable}
    //            SET
    //            WHERE ;";
    //        return cmd;
    //    }
    //}
    public sealed class LocBaseItem : BaseItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float MaxDurability { get; set; }
        public LocBaseItem(object data)
        {
            try
            {
                _database = GetPropValue<Enums.Databases>(data, "_database");
                _table = Enums.LocalizationTables.base_items;
                Id = GetPropValue<string>(data, "Id");
                Name = GetPropValue<string>(data, "Name");
                Description = GetPropValue<string>(data, "Description");
                PrefabName = GetPropValue<string>(data, "PrefabName");
                SpriteName = GetPropValue<string>(data, "SpriteName");
                Weight = GetPropValue<float>(data, "Weight");
                Type = GetPropValue<ItemType>(data, "Type");
                GridCols = GetPropValue<short>(data, "GridCols");
                GridRows = GetPropValue<short>(data, "GridRows");
                MaxStackSize = GetPropValue<int>(data, "MaxStackSize");
                Tier = GetPropValue<short>(data, "Tier");
                BasePrice = GetPropValue<float>(data, "BasePrice");
                MaxDurability = GetPropValue<int>(data, "MaxDurability");
                Durability = GetPropValue<float>(data, "Durability");
                UseLimit = GetPropValue<short>(data, "UseLimit");
                IsUsable = GetPropValue<bool>(data, "IsUsable");
                GlobalAttributes = GetPropValue<ItemAttributes>(data, "GlobalAttributes");
                NotForSale = GetPropValue<bool>(data, "NotForSale");
                ChallengeLevel = GetPropValue<short>(data, "ChallengeLevel");
            }
            catch (Exception ex)
            {
                FrameworkUtils.InternalLog(
                    LogLevel.Error,
                    $"Failed creating a Non Regional Item object. {ex}"
                );
            }
        }
        public LocBaseItem(IDataReader reader, Enums.Databases database)
        {
            try
            {
                _database = database;
                _table = Enums.LocalizationTables.base_items;
                Id = reader.GetString((int)Enums.LocBaseItems.id);
                Name = reader.GetString((int)Enums.LocBaseItems.name);
                Description = reader.GetString((int)Enums.LocBaseItems.description);
                PrefabName = reader.GetString((int)Enums.LocBaseItems.prefab_name);
                SpriteName = reader.GetString((int)Enums.LocBaseItems.sprite_name);
                Weight = reader.GetFloat((int)Enums.LocBaseItems.weight);
                ItemType itemType;
                Enum.TryParse(reader.GetString((int)Enums.LocBaseItems.type), false, out itemType);
                Type = itemType;
                short gridcols;
                short.TryParse(reader.GetString((int)Enums.LocBaseItems.grid_cols), out gridcols);
                GridCols = gridcols;
                GridRows = reader.GetInt16((int)Enums.LocBaseItems.grid_rows);
                MaxStackSize = reader.GetInt32((int)Enums.LocBaseItems.max_stack_size);
                Tier = reader.GetInt16((int)Enums.LocBaseItems.tier);
                BasePrice = reader.GetFloat((int)Enums.LocBaseItems.base_price);
                MaxDurability = reader.GetFloat((int)Enums.LocBaseItems.max_durability);
                Durability = reader.GetFloat((int)Enums.LocBaseItems.durability);
                UseLimit = reader.GetInt16((int)Enums.LocBaseItems.use_limit);
                IsUsable = reader.GetBoolean((int)Enums.LocBaseItems.is_usable);
                GlobalAttributes = new ItemAttributes(reader.GetString((int)Enums.LocBaseItems.attributes));
                NotForSale = reader.GetBoolean((int)Enums.LocBaseItems.not_for_sale);
                ChallengeLevel = reader.GetInt32((int)Enums.LocBaseItems.challenge_level);
            } catch (Exception ex)
            {
                FrameworkUtils.InternalLog(LogLevel.Error, $"Failed creating LocBaseItem\n{ex}");
            }
        }
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("id", Id),
                new SqliteParameter("name", Name),
                new SqliteParameter("description", Description),
                new SqliteParameter("prefab_name", PrefabName),
                new SqliteParameter("sprite_name", SpriteName),
                new SqliteParameter("weight", Weight),
                new SqliteParameter("type", Type.ToString()),
                new SqliteParameter("grid_cols", GridCols.ToString()),
                new SqliteParameter("grid_rows", GridRows),
                new SqliteParameter("max_stack_size", MaxStackSize),
                new SqliteParameter("tier", Tier),
                new SqliteParameter("base_price", BasePrice),
                new SqliteParameter("max_durability", MaxDurability),
                new SqliteParameter("durability", Durability),
                new SqliteParameter("use_limit", UseLimit),
                new SqliteParameter("is_usable", IsUsable ? 1 : 0),
                new SqliteParameter("global_attributes", GlobalAttributes.GetSql()),
                new SqliteParameter("not_for_sale", NotForSale ? 1 : 0),
                new SqliteParameter("challenge_level", ChallengeLevel),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable} 
                SET 
                    name = @name,
                    description = @description,
                    prefab_name = @prefab_name,
                    sprite_name = @sprite_name,
                    weight = @weight,
                    type = @type,
                    grid_cols = @grid_cols,
                    grid_rows = @grid_rows,
                    max_stack_size = @max_stack_size,
                    tier = @tier,
                    base_price = @base_price,
                    max_durability = @max_durability,
                    durability = @durability,
                    use_limit = @use_limit,
                    is_usable = @is_usable,
                    attributes = @global_attributes,
                    not_for_sale = @not_for_sale,
                    challenge_level = @challenge_level
                WHERE id = @id;";
            return cmd;
        }
    }
}
