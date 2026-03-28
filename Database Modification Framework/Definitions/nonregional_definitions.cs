using BepInEx.Logging;
using Mono.Data.Sqlite;
using System;
using System.Data;

namespace Database_Modification_Framework.Definitions
{
    public sealed class SlaughterhouseEnemy : SQLItem
    {
        public string character_id;
        public short tier;
        public short character_type;
        public string Comments;
        public SlaughterhouseEnemy(IDataReader reader)
        {
            _database = Enums.Databases.NonRegional;
            character_id = reader.GetString((int)Enums.NonRegEnemy.character_id);
            tier = reader.GetInt16((int)Enums.NonRegEnemy.tier);
            character_type = reader.GetInt16((int)Enums.NonRegEnemy.character_type);
            Comments = reader.GetString((int)Enums.NonRegEnemy.Comments);
        }
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("cid", character_id),
                new SqliteParameter("tr", tier),
                new SqliteParameter("ct", character_type),
                new SqliteParameter("com", Comments),
            });
            cmd.CommandText = $@"
                UPDATE {Enums.NonRegionalTables.slaughterhouse_enemies} 
                SET 
                    {Enums.NonRegEnemy.tier.ToString()} = @tr,
                    {Enums.NonRegEnemy.character_type.ToString()} = @ct,
                    {Enums.NonRegEnemy.Comments.ToString()} = @com
                WHERE {Enums.NonRegEnemy.character_id.ToString()} = @cid;";
            return cmd;
        }
    }
    public sealed class SlaughterhouseProp : SQLItem
    {
        public string id;
        public short tier;
        public int price;
        public ItemType type;
        public short challenge_level;
        public SlaughterhouseProp(IDataReader reader)
        {
            _database = Enums.Databases.NonRegional;
            id = reader.GetString((int)Enums.NonRegProp.id);
            tier = reader.GetInt16((int)Enums.NonRegProp.tier);
            price = reader.GetInt32((int)Enums.NonRegProp.price);
            ItemType temp;
            Enum.TryParse(reader.GetString((int)Enums.NonRegProp.type), out temp);
            type = temp;
            challenge_level = reader.GetInt16((int)Enums.NonRegProp.challenge_level);
        }
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("id", id),
                new SqliteParameter("tr", tier),
                new SqliteParameter("pc", price),
                new SqliteParameter("tp", type.ToString()),
                new SqliteParameter("cl", challenge_level),
            });
            cmd.CommandText = $@"
                UPDATE {Enums.NonRegionalTables.slaughterhouse_props} 
                SET 
                    {Enums.NonRegProp.tier.ToString()} = @tr,
                    {Enums.NonRegProp.price.ToString()} = @pc,
                    {Enums.NonRegProp.type.ToString()} = @tp,
                    {Enums.NonRegProp.challenge_level.ToString()} = @cl
                WHERE {Enums.NonRegProp.id.ToString()} = @id;";
            return cmd;
        }
    }
    // The Non Regional Item Table Datatype
    public sealed class NonRegionalItem : BaseItem
    {
        private Enums.NonRegionalTables _table;
        public override string DbTable { get => _table.ToString(); }
        public NonRegionalItem(object data)
        {
            try
            {
                _database = Enums.Databases.NonRegional;
                _table = Enums.NonRegionalTables.item_attributes;
                Id = (string)GetPropValue(data, "Id");
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
        public NonRegionalItem(IDataReader reader)
        {
            _database = Enums.Databases.NonRegional;
            _table = Enums.NonRegionalTables.item_attributes;
            Id = reader.GetString((int)Enums.NonRegItem.id);
            PrefabName = reader.GetString((int)Enums.NonRegItem.prefab_name);
            SpriteName = reader.GetString((int)Enums.NonRegItem.sprite_name);
            Weight = reader.GetFloat((int)Enums.NonRegItem.weight);
            ItemType itemType;
            Enum.TryParse(reader.GetString((int)Enums.NonRegItem.type), false, out itemType);
            Type = itemType;
            GridCols = reader.GetInt16((int)Enums.NonRegItem.grid_cols);
            GridRows = reader.GetInt16((int)Enums.NonRegItem.grid_rows);
            MaxStackSize = reader.GetInt32((int)Enums.NonRegItem.max_stack_size);
            Tier = reader.GetInt16((int)Enums.NonRegItem.tier);
            BasePrice = reader.GetFloat((int)Enums.NonRegItem.base_price);
            MaxDurability = reader.GetInt32((int)Enums.NonRegItem.max_durability);
            Durability = reader.GetFloat((int)Enums.NonRegItem.durability);
            UseLimit = reader.GetInt16((int)Enums.NonRegItem.use_limit);
            IsUsable = reader.GetBoolean((int)Enums.NonRegItem.is_usable);
            GlobalAttributes = new ItemAttributes(reader.GetString((int)Enums.NonRegItem.global_attributes));
            NotForSale = reader.GetBoolean((int)Enums.NonRegItem.not_for_sale);
            ChallengeLevel = reader.GetInt32((int)Enums.NonRegItem.challenge_level);
        }

        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("id", Id),
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
                    global_attributes = @global_attributes,
                    not_for_sale = @not_for_sale,
                    challenge_level = @challenge_level
                WHERE id = @id";
            return cmd;
        }
    }
}
