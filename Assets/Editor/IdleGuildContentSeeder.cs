using System.Collections.Generic;
using System.IO;
using IdleGuildDemo.Core;
using IdleGuildDemo.Data;
using UnityEditor;
using UnityEngine;

namespace IdleGuildDemo.Editor
{
    public static class IdleGuildContentSeeder
    {
        private const string MenuPath = "Tools/Idle Guild/Seed Demo Content";

        [MenuItem(MenuPath)]
        public static void SeedDemoContent()
        {
            SeedItems();
            SeedEnemies();
            SeedClasses();
            SeedTalents();
            SeedZones();
            SeedRecipes();
            SeedQuests();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Idle Guild demo content seed complete.");
        }

        private static void SeedItems()
        {
            UpsertItem("Assets/Data/Items/Item_CopperOre.asset", GameConstants.CopperOreItemId, "Copper Ore", "Raw copper gathered from the mine.", true);
            UpsertItem("Assets/Data/Items/Item_CopperBar.asset", GameConstants.CopperBarItemId, "Copper Bar", "Smelted copper used for early crafting.", true);
            UpsertItem("Assets/Data/Items/Item_SlimeGoo.asset", GameConstants.SlimeGooItemId, "Slime Goo", "Sticky material dropped by slimes.", true);
            UpsertItem("Assets/Data/Items/Item_CopperSword.asset", GameConstants.CopperSwordItemId, "Copper Sword", "Equipment-like item that will improve combat damage.", true);
            UpsertItem("Assets/Data/Items/Item_CopperPickaxe.asset", GameConstants.CopperPickaxeItemId, "Copper Pickaxe", "Equipment-like item that will improve mining.", true);
            UpsertItem("Assets/Data/Items/Item_Coins.asset", GameConstants.CoinItemId, "Coins", "Basic currency.", true);
        }

        private static void SeedEnemies()
        {
            EnemyDefinition enemy = LoadOrCreate<EnemyDefinition>("Assets/Data/Enemies/Enemy_Slime.asset");
            SerializedObject serialized = new SerializedObject(enemy);
            SetString(serialized, "id", GameConstants.SlimeEnemyId);
            SetString(serialized, "displayName", "Slime");
            SetInt(serialized, "maxHp", 10);
            SetInt(serialized, "xpReward", 5);
            Save(serialized, enemy);
        }

        private static void SeedClasses()
        {
            UpsertTextAsset<ClassDefinition>("Assets/Data/Classes/Class_Warrior.asset", GameConstants.WarriorClassId, "Warrior", "+damage");
            UpsertTextAsset<ClassDefinition>("Assets/Data/Classes/Class_Archer.asset", GameConstants.ArcherClassId, "Archer", "+drop rate");
            UpsertTextAsset<ClassDefinition>("Assets/Data/Classes/Class_Mage.asset", GameConstants.MageClassId, "Mage", "+AFK gain");
        }

        private static void SeedTalents()
        {
            UpsertTextAsset<TalentDefinition>("Assets/Data/Talents/Talent_Damage.asset", GameConstants.DamageTalentId, "Damage", "Increases combat damage.");
            UpsertTextAsset<TalentDefinition>("Assets/Data/Talents/Talent_MiningSpeed.asset", GameConstants.MiningSpeedTalentId, "Mining Speed", "Increases copper mining speed.");
            UpsertTextAsset<TalentDefinition>("Assets/Data/Talents/Talent_XpGain.asset", GameConstants.XpGainTalentId, "XP Gain", "Increases XP gained.");
            UpsertTextAsset<TalentDefinition>("Assets/Data/Talents/Talent_AfkGain.asset", GameConstants.AfkGainTalentId, "AFK Gain", "Increases offline reward rate.");
        }

        private static void SeedZones()
        {
            UpsertTextAsset<ZoneDefinition>("Assets/Data/Zones/Zone_Town.asset", GameConstants.TownZoneId, "Town", "Central hub for the demo.");
            UpsertTextAsset<ZoneDefinition>("Assets/Data/Zones/Zone_SlimeField.asset", GameConstants.SlimeCombatZoneId, "Slime Field", "Single combat zone for fighting slimes.");
            UpsertTextAsset<ZoneDefinition>("Assets/Data/Zones/Zone_CopperMine.asset", GameConstants.CopperMineZoneId, "Copper Mine", "Single gathering zone for copper ore.");
        }

        private static void SeedRecipes()
        {
            UpsertRecipe(
                "Assets/Data/Recipes/Recipe_CopperBar.asset",
                GameConstants.CopperBarRecipeId,
                "Copper Bar",
                "Smelt copper ore into a copper bar.",
                new[] { new IngredientSeed(GameConstants.CopperOreItemId, 3) },
                GameConstants.CopperBarItemId,
                1);

            UpsertRecipe(
                "Assets/Data/Recipes/Recipe_CopperSword.asset",
                GameConstants.CopperSwordRecipeId,
                "Copper Sword",
                "Craft an early combat upgrade.",
                new[] { new IngredientSeed(GameConstants.CopperBarItemId, 2), new IngredientSeed(GameConstants.SlimeGooItemId, 3) },
                GameConstants.CopperSwordItemId,
                1);

            UpsertRecipe(
                "Assets/Data/Recipes/Recipe_CopperPickaxe.asset",
                GameConstants.CopperPickaxeRecipeId,
                "Copper Pickaxe",
                "Craft an early mining upgrade.",
                new[] { new IngredientSeed(GameConstants.CopperBarItemId, 2) },
                GameConstants.CopperPickaxeItemId,
                1);
        }

        private static void SeedQuests()
        {
            UpsertQuest("Assets/Data/Quests/Quest_01_KillSlimes.asset", GameConstants.KillSlimesQuestId, "First Hunt", "Kill 5 Slimes", QuestObjectiveType.KillEnemy, GameConstants.SlimeEnemyId, 5, GameConstants.CollectCopperOreQuestId, 20, 10, string.Empty, 0, false);
            UpsertQuest("Assets/Data/Quests/Quest_02_CollectCopperOre.asset", GameConstants.CollectCopperOreQuestId, "Miner Time", "Collect 10 Copper Ore", QuestObjectiveType.CollectItem, GameConstants.CopperOreItemId, 10, GameConstants.CraftCopperBarsQuestId, 15, 5, string.Empty, 0, false);
            UpsertQuest("Assets/Data/Quests/Quest_03_CraftCopperBars.asset", GameConstants.CraftCopperBarsQuestId, "First Smelt", "Craft 3 Copper Bars", QuestObjectiveType.CraftItem, GameConstants.CopperBarItemId, 3, GameConstants.CraftCopperSwordQuestId, 20, 5, string.Empty, 0, false);
            UpsertQuest("Assets/Data/Quests/Quest_04_CraftCopperSword.asset", GameConstants.CraftCopperSwordQuestId, "Gear Up", "Craft Copper Sword", QuestObjectiveType.CraftItem, GameConstants.CopperSwordItemId, 1, GameConstants.ReachLevel5QuestId, 30, 10, string.Empty, 0, false);
            UpsertQuest("Assets/Data/Quests/Quest_05_ReachLevel5.asset", GameConstants.ReachLevel5QuestId, "Ready to Grow", "Reach Level 5", QuestObjectiveType.ReachLevel, string.Empty, 5, GameConstants.ChooseClassQuestId, 0, 20, string.Empty, 0, false);
            UpsertQuest("Assets/Data/Quests/Quest_06_ChooseClass.asset", GameConstants.ChooseClassQuestId, "Choose Your Path", "Choose a Class", QuestObjectiveType.ChooseClass, string.Empty, 1, GameConstants.UnlockCharacter2QuestId, 0, 25, string.Empty, 0, false);
            UpsertQuest("Assets/Data/Quests/Quest_07_UnlockCharacter2.asset", GameConstants.UnlockCharacter2QuestId, "More Hands", "Unlock Character 2", QuestObjectiveType.UnlockCharacter, string.Empty, 1, string.Empty, 0, 0, string.Empty, 0, true);
        }

        private static void UpsertItem(string path, string id, string displayName, string description, bool isStackable)
        {
            ItemDefinition item = LoadOrCreate<ItemDefinition>(path);
            SerializedObject serialized = new SerializedObject(item);
            SetString(serialized, "id", id);
            SetString(serialized, "displayName", displayName);
            SetString(serialized, "description", description);
            SetBool(serialized, "isStackable", isStackable);
            Save(serialized, item);
        }

        private static void UpsertTextAsset<T>(string path, string id, string displayName, string description)
            where T : ScriptableObject
        {
            T asset = LoadOrCreate<T>(path);
            SerializedObject serialized = new SerializedObject(asset);
            SetString(serialized, "id", id);
            SetString(serialized, "displayName", displayName);
            SetString(serialized, "description", description);
            Save(serialized, asset);
        }

        private static void UpsertRecipe(string path, string id, string displayName, string description, IReadOnlyList<IngredientSeed> ingredients, string outputItemId, int outputQuantity)
        {
            RecipeDefinition recipe = LoadOrCreate<RecipeDefinition>(path);
            SerializedObject serialized = new SerializedObject(recipe);
            SetString(serialized, "id", id);
            SetString(serialized, "displayName", displayName);
            SetString(serialized, "description", description);
            SetString(serialized, "outputItemId", outputItemId);
            SetInt(serialized, "outputQuantity", outputQuantity);

            SerializedProperty ingredientList = serialized.FindProperty("ingredients");
            ingredientList.arraySize = ingredients.Count;
            for (int i = 0; i < ingredients.Count; i++)
            {
                SerializedProperty ingredient = ingredientList.GetArrayElementAtIndex(i);
                ingredient.FindPropertyRelative("itemId").stringValue = ingredients[i].itemId;
                ingredient.FindPropertyRelative("quantity").intValue = ingredients[i].quantity;
            }

            Save(serialized, recipe);
        }

        private static void UpsertQuest(string path, string id, string displayName, string description, QuestObjectiveType objectiveType, string targetId, int requiredAmount, string nextQuestId, int rewardXp, int rewardCoins, string rewardItemId, int rewardItemQuantity, bool unlocksSecondCharacter)
        {
            QuestDefinition quest = LoadOrCreate<QuestDefinition>(path);
            SerializedObject serialized = new SerializedObject(quest);
            SetString(serialized, "id", id);
            SetString(serialized, "displayName", displayName);
            SetString(serialized, "description", description);
            SetEnum(serialized, "objectiveType", objectiveType);
            SetString(serialized, "targetId", targetId);
            SetInt(serialized, "requiredAmount", requiredAmount);
            SetString(serialized, "nextQuestId", nextQuestId);
            SetInt(serialized, "rewardXp", rewardXp);
            SetInt(serialized, "rewardCoins", rewardCoins);
            SetString(serialized, "rewardItemId", rewardItemId);
            SetInt(serialized, "rewardItemQuantity", rewardItemQuantity);
            SetBool(serialized, "unlocksSecondCharacter", unlocksSecondCharacter);
            Save(serialized, quest);
        }

        private static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            EnsureFolder(Path.GetDirectoryName(path)?.Replace("\\", "/"));
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            Debug.Log($"Created {path}");
            return asset;
        }

        private static void EnsureFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder) || AssetDatabase.IsValidFolder(folder))
            {
                return;
            }

            Directory.CreateDirectory(folder);
        }

        private static void Save(SerializedObject serialized, Object asset)
        {
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(asset);
            Debug.Log($"Updated {AssetDatabase.GetAssetPath(asset)}");
        }

        private static void SetString(SerializedObject serialized, string propertyName, string value)
        {
            serialized.FindProperty(propertyName).stringValue = value ?? string.Empty;
        }

        private static void SetInt(SerializedObject serialized, string propertyName, int value)
        {
            serialized.FindProperty(propertyName).intValue = value;
        }

        private static void SetBool(SerializedObject serialized, string propertyName, bool value)
        {
            serialized.FindProperty(propertyName).boolValue = value;
        }

        private static void SetEnum(SerializedObject serialized, string propertyName, QuestObjectiveType value)
        {
            serialized.FindProperty(propertyName).enumValueIndex = (int)value;
        }

        private readonly struct IngredientSeed
        {
            public readonly string itemId;
            public readonly int quantity;

            public IngredientSeed(string itemId, int quantity)
            {
                this.itemId = itemId;
                this.quantity = quantity;
            }
        }
    }
}
