namespace IdleGuildDemo.Core
{
    /// <summary>
    /// Shared IDs and constants used across systems.
    /// </summary>
    public static class GameConstants
    {
        public const string CoinItemId = "item.coins";
        public const string CopperOreItemId = "item.copper_ore";
        public const string CopperBarItemId = "item.copper_bar";
        public const string SlimeGooItemId = "item.slime_goo";
        public const string CopperSwordItemId = "item.copper_sword";
        public const string CopperPickaxeItemId = "item.copper_pickaxe";
        public const string SlimeEnemyId = "enemy.slime";
        public const string ItemCopperOreId = CopperOreItemId;
        public const string ItemCopperBarId = CopperBarItemId;
        public const string ItemCopperSwordId = CopperSwordItemId;
        public const string ItemCopperPickaxeId = CopperPickaxeItemId;
        public const string ItemCoinsId = CoinItemId;
        public const string ItemSlimeGooId = SlimeGooItemId;
        public const string EnemySlimeId = SlimeEnemyId;
        public const string TownZoneId = "zone.town";
        public const string SlimeCombatZoneId = "zone.combat_slimes";
        public const string CopperMineZoneId = "zone.mine_copper";
        public const string ZoneMineCopperId = CopperMineZoneId;
        public const string CopperBarRecipeId = "recipe.copper_bar";
        public const string CopperSwordRecipeId = "recipe.copper_sword";
        public const string CopperPickaxeRecipeId = "recipe.copper_pickaxe";
        public const string RecipeCopperBarId = CopperBarRecipeId;
        public const string RecipeCopperSwordId = CopperSwordRecipeId;
        public const string RecipeCopperPickaxeId = CopperPickaxeRecipeId;
        public const string KillSlimesQuestId = "quest.kill_slimes";
        public const string CollectCopperOreQuestId = "quest.collect_copper_ore";
        public const string CraftCopperBarsQuestId = "quest.craft_copper_bars";
        public const string CraftCopperSwordQuestId = "quest.craft_copper_sword";
        public const string ReachLevel5QuestId = "quest.reach_level_5";
        public const string ChooseClassQuestId = "quest.choose_class";
        public const string UnlockCharacter2QuestId = "quest.unlock_character_2";
        public const string QuestKillSlimesId = KillSlimesQuestId;
        public const string QuestCollectCopperOreId = CollectCopperOreQuestId;
        public const string QuestCraftCopperBarsId = CraftCopperBarsQuestId;
        public const string QuestCraftCopperSwordId = CraftCopperSwordQuestId;
        public const string QuestReachLevel5Id = ReachLevel5QuestId;
        public const string QuestChooseClassId = ChooseClassQuestId;
        public const string QuestUnlockCharacter2Id = UnlockCharacter2QuestId;
        public const int MaxOfflineHours = 8;
        public const int OfflineMaxHours = MaxOfflineHours;
        public const int OfflineDemoSimulatedHours = 2;
        public const int OfflineCombatXpPerMinute = 4;
        public const int OfflineCombatCoinsPerMinute = 1;
        public const int OfflineSlimeGooPerMinute = 1;
        public const int OfflineMiningXpPerMinute = 2;
        public const int OfflineCopperOrePerMinute = 1;
        public const int ClassUnlockLevel = 5;
        public const string StartingCharacterId = "char_001";
        public const string SecondCharacterId = "char_002";
        public const string TaskIdle = "idle";
        public const string TaskCombat = "combat";
        public const string TaskMining = "mining";
        public const int CombatSlimeBaseCoins = 3;
        public const int CombatSlimeGooDropQuantity = 1;
        public const float MiningCopperBaseDurationSeconds = 2.0f;
        public const int MiningCopperOreRewardQuantity = 1;
        public const int MiningCopperXpReward = 2;
        public const string WarriorClassId = "warrior";
        public const string ArcherClassId = "archer";
        public const string MageClassId = "mage";
        public const string DamageTalentId = "damage";
        public const string MiningSpeedTalentId = "mining_speed";
        public const string XpGainTalentId = "xp_gain";
        public const string AfkGainTalentId = "afk_gain";
    }
}
