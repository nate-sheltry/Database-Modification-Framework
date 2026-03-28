using BepInEx.Logging;
using Mono.Data.Sqlite;
using System;
using System.Data;

namespace Database_Modification_Framework.Definitions
{
    public class LocBaseItem : BaseItem
    {
        protected internal Enums.LocalizationTables _table;
        public override string DbTable { get => _table.ToString(); }
        public string Name { get; set; }
        public string Description { get; set; }
        public LocBaseItem(object data)
        {
            try
            {
                _database = (Enums.Databases)GetPropValue(data, "_database");
                _table = Enums.LocalizationTables.base_items;
                Id = (string)GetPropValue(data, "Id");
                Name = (string)GetPropValue(data, "Name");
                Description = (string)GetPropValue(data, "Description");
                PrefabName = (string)GetPropValue(data, "PrefabName");
                SpriteName = (string)GetPropValue(data, "SpriteName");
                Weight = (float)GetPropValue(data, "Weight");
                Type = (ItemType)GetPropValue(data, "Type");
                GridCols = (short)GetPropValue(data, "GridCols");
                GridRows = (short)GetPropValue(data, "GridRows");
                MaxStackSize = (int)GetPropValue(data, "MaxStackSize");
                Tier = (short)GetPropValue(data, "Tier");
                BasePrice = (float)GetPropValue(data, "BasePrice");
                MaxDurability = (int)GetPropValue(data, "MaxDurability");
                Durability = (float)GetPropValue(data, "Durability");
                UseLimit = (short)GetPropValue(data, "UseLimit");
                IsUsable = (bool)GetPropValue(data, "IsUsable");
                GlobalAttributes = (ItemAttributes)GetPropValue(data, "GlobalAttributes");
                NotForSale = (bool)GetPropValue(data, "NotForSale");
                ChallengeLevel = (short)GetPropValue(data, "ChallengeLevel");
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
            GridCols = reader.GetInt16((int)Enums.LocBaseItems.grid_cols);
            GridRows = reader.GetInt16((int)Enums.LocBaseItems.grid_rows);
            MaxStackSize = reader.GetInt32((int)Enums.LocBaseItems.max_stack_size);
            Tier = reader.GetInt16((int)Enums.LocBaseItems.tier);
            BasePrice = reader.GetFloat((int)Enums.LocBaseItems.base_price);
            MaxDurability = reader.GetInt32((int)Enums.LocBaseItems.max_durability);
            Durability = reader.GetFloat((int)Enums.LocBaseItems.durability);
            UseLimit = reader.GetInt16((int)Enums.LocBaseItems.use_limit);
            IsUsable = reader.GetBoolean((int)Enums.LocBaseItems.is_usable);
            GlobalAttributes = new ItemAttributes(reader.GetString((int)Enums.LocBaseItems.attributes));
            NotForSale = reader.GetBoolean((int)Enums.LocBaseItems.not_for_sale);
            ChallengeLevel = reader.GetInt32((int)Enums.LocBaseItems.challenge_level);
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
                new SqliteParameter("grid_cols", GridCols),
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
                WHERE id = @id";
            return cmd;
        }
    }
}
