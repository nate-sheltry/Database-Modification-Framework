using BepInEx.Logging;
using Mono.Data.Sqlite;
using System;
using System.Data;
using static Database_Modification_Framework.FrameworkUtils;


namespace Database_Modification_Framework.Definitions
{
    public sealed class SlaughterhouseEnemy : SQLItem
    {
        public string CharacterId { get; set; }
        public short Tier { get; set; }
        public short CharacterType { get; set; }
        public string Comments { get; set; }
        public SlaughterhouseEnemy(object data)
        {
            _database = Enums.Databases.NonRegional;
            _table = Enums.NonRegionalTables.slaughterhouse_enemies;
            CharacterId = GetPropValue<string>(data, "CharacterId");
            Tier = GetPropValue<short>(data, "Tier");
            CharacterType = GetPropValue<short>(data, "CharacterType");
            Comments = GetPropValue<string>(data, "Comments");
        }
        public SlaughterhouseEnemy(IDataReader reader)
        {
            _database = Enums.Databases.NonRegional;
            _table = Enums.NonRegionalTables.slaughterhouse_enemies;
            CharacterId = reader.GetString((int)Enums.NonRegEnemy.character_id);
            Tier = reader.GetInt16((int)Enums.NonRegEnemy.tier);
            CharacterType = reader.GetInt16((int)Enums.NonRegEnemy.character_type);
            Comments = reader.GetString((int)Enums.NonRegEnemy.Comments);
        }
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("id", CharacterId),
                new SqliteParameter("tr", Tier),
                new SqliteParameter("ct", CharacterType),
                new SqliteParameter("com", Comments),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable} 
                SET 
                    {Enums.NonRegEnemy.tier.ToString()} = @tr,
                    {Enums.NonRegEnemy.character_type.ToString()} = @ct,
                    {Enums.NonRegEnemy.Comments.ToString()} = @com
                WHERE {Enums.NonRegEnemy.character_id.ToString()} = @id;";
            return cmd;
        }
    }
    public sealed class SlaughterhouseProp : SQLItem
    {
        public string Id { get; set; }
        public short Tier { get; set; }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public short ChallengeLevel { get; set; }
        public SlaughterhouseProp(object data)
        {
            _database = Enums.Databases.NonRegional;
            _table = Enums.NonRegionalTables.slaughterhouse_props;
            Id = GetPropValue<string>(data, "Id");
            Tier = GetPropValue<short>(data, "Tier");
            Price = GetPropValue<int>(data, "Price");
            Type = GetPropValue<ItemType>(data, "Type");
            ChallengeLevel = GetPropValue<short>(data, "ChallengeLevel");
        }
        public SlaughterhouseProp(IDataReader reader)
        {
            _database = Enums.Databases.NonRegional;
            _table = Enums.NonRegionalTables.slaughterhouse_props;
            Id = reader.GetString((int)Enums.NonRegProp.id);
            Tier = reader.GetInt16((int)Enums.NonRegProp.tier);
            Price = reader.GetInt32((int)Enums.NonRegProp.price);
            ItemType temp;
            Enum.TryParse(reader.GetString((int)Enums.NonRegProp.type), out temp);
            Type = temp;
            ChallengeLevel = reader.GetInt16((int)Enums.NonRegProp.challenge_level);
        }
        public override IDbCommand GetSqlCommand()
        {
            SqliteCommand cmd = new SqliteCommand();
            cmd.Parameters.AddRange(new IDbDataParameter[]{
                new SqliteParameter("id", Id),
                new SqliteParameter("tr", Tier),
                new SqliteParameter("pc", Price),
                new SqliteParameter("tp", Type.ToString()),
                new SqliteParameter("cl", ChallengeLevel),
            });
            cmd.CommandText = $@"
                UPDATE {DbTable} 
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
        public int MaxDurability { get; set; }
        public NonRegionalItem(object data)
        {
            try
            {
                _database = Enums.Databases.NonRegional;
                _table = Enums.NonRegionalTables.item_attributes;
                Id = GetPropValue<string>(data, "Id");
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
                    $"Failed creating a Non Regional Item object.\n{ex}"
                );
            }
        }
        public NonRegionalItem(IDataReader reader)
        {
            try
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
            } catch (Exception ex)
            {
                FrameworkUtils.InternalLog(LogLevel.Error, $"Failed creating NonRegionalItem\n{ex}");
            }
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
                WHERE id = @id;";
            return cmd;
        }
    }
}
