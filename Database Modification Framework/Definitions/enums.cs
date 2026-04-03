
namespace Database_Modification_Framework.Definitions
{
    public static class Enums
    {
        public enum Databases
        {
            AI = 0,
            Main = 1,
            MainCH = 2,
            MainFR = 3,
            MainGER = 4,
            MainITA = 5,
            MainRUS = 6,
            MainSPA = 7,
            MainUA = 8,
            NonRegional = 9,
            Translation = 10,
            Translation_old = 11,
        }
        public enum NonRegionalTables //Shorthand NonReg
        {
            slaughterhouse_enemies,
            slaughterhouse_props,
            item_attributes,
        }
        public enum LocalizationTables //Shorthand Loc
        {
            global_dialogue_response,
            journal_entries,
            //sqlite_sequence,
            faction_relationships,
            story_conditions_trigger,
            note_papers,
            environment_sounds,
            task_data,
            initial_squads,
            preset_characters,
            conversation,
            news_entries,
            story_conditions_item,
            stashes,
            recipes,
            skill_perks,
            //level_distance_matrix,
            base_items,
            factions,
            level_data,
            tutorials,
        }
        public enum NonRegItem
        {
            id = 0,
            prefab_name = 1,
            sprite_name = 2,
            weight = 3,
            type = 4,
            grid_cols = 5,
            grid_rows = 6,
            max_stack_size = 7,
            tier = 8,
            base_price = 9,
            max_durability = 10,
            durability = 11,
            use_limit = 12,
            is_usable = 13,
            global_attributes = 14,
            not_for_sale = 15,
            challenge_level = 16,
        }
        public enum NonRegEnemy
        {
            character_id = 0,
            tier = 1,
            character_type = 2,
            Comments = 3,
        }
        public enum NonRegProp
        {
            id = 0,
            tier = 1,
            price = 2,
            type = 3,
            challenge_level = 4,
        }
        public enum LocBaseItems
        {
            id = 0,
            name = 1,
            description = 2,
            prefab_name = 3,
            sprite_name = 4,
            weight = 5,
            type = 6,
            grid_cols = 7,
            grid_rows = 8,
            max_stack_size = 9,
            tier = 10,
            base_price = 11,
            max_durability = 12,
            durability = 13,
            use_limit = 14,
            is_usable = 15,
            attributes = 16,
            not_for_sale = 17,
            challenge_level = 18,
        }
        public enum LocConvos
        {
            id = 0,
            faction_id = 1,
            line = 2,
            type = 3,
            emotion = 4,
            responses = 5,
            is_for_procedural = 6,
            trigger_script = 7,
            is_enabled = 8,
        }
        public enum LocEnvSounds
        {
            location = 0,
            time = 1,
            weather = 2,
            primary_set = 3,
            secondary_set = 4,
        }
        public enum LocFactionRel
        {
            faction_id = 0,
            target_faction_id = 1,
            relationship = 2,
        }
        public enum LocFactions
        {
            id = 0,
            name = 1,
            models = 2,
            armored_models = 3,
            character_type = 4,
            display_name = 5,
            elite_model = 6,
            deadzone_model = 7,
        }
        public enum LocGlobResp
        {
            id = 0,
            response = 1,
        }
        public enum LocInitSquads
        {
            id = 0,
            tier = 1,
            faction = 2,
            household = 3,
            level = 4,
            respawn = 5,
        }
        public enum LocJrnlEntries
        {
            id = 0,
            entry = 1,
        }
        public enum LocLvlData
        {
            id = 0,
            display_name = 1,
            description = 2,
            main_level_name = 3,
            fog_height = 4,
            allow_airdrop = 5,
            rain_fog_heights = 6,
        }
        public enum LocLvlDistMatrix
        {
            SublevelName = 0,
            Village = 1,
            Mill = 2,
            BarnChurch = 3,
            RoadBlock = 4,
            TrainStation = 5,
            RailroadCamp = 6,
            Station11Proper = 7,
            LuxuryHouse = 8,
            LakeCheko = 9,
            WaterTreatment = 10,
            SwampCottage = 11,
            Sanatorium = 12,
            Oblenska = 13,
            Swamp = 14,
            Cottages = 15,
            RavenwoodCrypt = 16,
            LakeChurch = 17,
            Valleys = 18,
            CottagesUpsidedown = 19,
            Border = 20,
            SwampVillage = 21,
        }
        public enum LocNewsEntries
        {
            id = 0,
            content = 1,
            pay = 2,
            topic_id = 3,
        }
        public enum LocNotePapers
        {
            id = 0,
            text = 1,
        }
        public enum LocPresetChar
        {
            id = 0,
            level = 1,
            position_x = 2,
            position_y = 3,
            position_z = 4,
        }
        public enum LocRecipes
        {
            serum_id = 0,
            ingredients = 1,
            ingredients_count = 2,
            temperature = 3,
            platform = 4,
            quantity = 5,
            hours = 6,
        }
        public enum LocSkillPerks
        {
            id = 0,
            skill_id = 1,
            sequence = 2,
            title = 3,
            description = 4,
        }
        internal enum LocSqlLiteSeq
        {
            name = 0,
            seq = 1,
        }
        public enum LocStashes
        {
            stash_name = 0,
            stash_notes = 1,
        }
        public enum LocStoryConItems
        {
            id = 0,
            item_id = 1,
            is_active = 2,
            is_durability = 3,
            is_type = 4,
        }
        public enum LocStoryConTriggers
        {
            id = 0,
            initial_value = 1,
            is_active = 2,
        }
        public enum LocTaskData
        {
            id = 0,
            text = 1,
            journal_id = 2,
        }
        public enum  LocTutorials
        {
            id = 0,
            title = 1,
            message = 2,
            message_controller = 3,
            disruptive = 4,
        }
        // Non Regional Item's Types are defined as ItemType in the base game.
        public enum RecipePlatforms
        {
            Serum = 0,
            Cooking = 1,
            Grill = 2,
            Distiller = 3,
            Cauldron = 4,
            SoupCauldron = 5,
        }
    }
}
