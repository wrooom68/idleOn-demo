using System.Collections.Generic;
using IdleGuildDemo.Core;
using IdleGuildDemo.Data;
using IdleGuildDemo.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IdleGuildDemo.Editor
{
    public static class IdleGuildSceneBuilder
    {
        private const string MainMenuPath = "Assets/Scenes/MainMenu.unity";
        private const string TownPath = "Assets/Scenes/Town.unity";
        private const string CombatPath = "Assets/Scenes/CombatZone.unity";
        private const string MinePath = "Assets/Scenes/MineZone.unity";
        private const string SlimeDefinitionPath = "Assets/Data/Enemies/Enemy_Slime.asset";
        private const string CopperBarRecipePath = "Assets/Data/Recipes/Recipe_CopperBar.asset";
        private const string CopperSwordRecipePath = "Assets/Data/Recipes/Recipe_CopperSword.asset";
        private const string CopperPickaxeRecipePath = "Assets/Data/Recipes/Recipe_CopperPickaxe.asset";

        private static readonly Color BackgroundColor = new Color(0.043f, 0.063f, 0.125f, 1f); // #0B1020
        private static readonly Color PanelColor = new Color(0.122f, 0.161f, 0.216f, 1f); // #1F2937
        private static readonly Color SubPanelColor = new Color(0.153f, 0.208f, 0.286f, 1f); // #273549
        private static readonly Color ButtonColor = new Color(0.420f, 0.306f, 0.180f, 1f); // #6B4E2E
        private static readonly Color ButtonHighlightColor = new Color(0.55f, 0.42f, 0.28f, 1f);
        private static readonly Color TextColor = new Color(0.976f, 0.980f, 0.984f, 1f); // #F9FAFB
        private static readonly Color MutedTextColor = new Color(0.718f, 0.753f, 0.800f, 1f); // #B7C0CC
        private static readonly Color GoldColor = new Color(0.953f, 0.788f, 0.365f, 1f); // #F3C95D
        private static readonly Color LightGreenColor = new Color(0.718f, 0.969f, 0.718f, 1f); // #B7F7B7
        private static readonly Color BlueColor = new Color(0.302f, 0.639f, 1f, 1f); // #4DA3FF
        private static readonly Color RedColor = new Color(0.851f, 0.290f, 0.290f, 1f); // #D94A4A

        private static Font cachedFont;

        [MenuItem("Tools/Idle Guild/Rebuild Demo Scenes")]
        public static void RebuildDemoScenes()
        {
            BuildMainMenuScene();
            BuildTownScene();
            BuildCombatScene();
            BuildMineScene();
            EnsureBuildSettings();
            AssetDatabase.SaveAssets();
            Debug.Log("Idle Guild demo scenes rebuilt with Unity editor APIs.");
        }

        private static void BuildMainMenuScene()
        {
            Scene scene = CreateScene("MainMenu");
            AddMainCamera();
            CreateEventSystem();
            Canvas canvas = CreateCanvas();
            AddGameBootstrap();

            GameObject viewObject = new GameObject("MainMenuView");
            MainMenuView view = viewObject.AddComponent<MainMenuView>();

            CreatePanel(canvas.transform, "Background", BackgroundColor, Stretch());
            
            // Panel_TitleBlock (matching Frame_01_MainMenu)
            GameObject titleBlock = CreatePanel(canvas.transform, "Panel_TitleBlock", PanelColor, Center(0f, 215f, 800f, 190f));
            CreateText(titleBlock.transform, "Text_GameTitle", "Idle Guild Demo", 64, TextAnchor.MiddleCenter, Rect(0f, 30f, 760f, 86f));
            CreateText(titleBlock.transform, "Text_Subtitle", "Early-game idle RPG vertical slice", 28, TextAnchor.MiddleCenter, Rect(0f, -40f, 760f, 38f), MutedTextColor);

            // Panel_MenuButtons (matching Frame_01_MainMenu)
            GameObject menuPanel = CreatePanel(canvas.transform, "Panel_MenuButtons", PanelColor, Center(0f, -60f, 500f, 240f));
            Button newGameButton = CreateButton(menuPanel.transform, "Button_NewGame", "New Game", Rect(0f, 75f, 320f, 58f));
            Button continueButton = CreateButton(menuPanel.transform, "Button_Continue", "Continue", Rect(0f, 15f, 320f, 58f));
            Button resetButton = CreateButton(menuPanel.transform, "Button_DeleteSave", "Delete Save", Rect(0f, -45f, 320f, 48f));

            // Status and Notes
            Text evaluationNoteText = CreateText(canvas.transform, "Text_EvaluationNote", "Designed to show all major systems in under 30 minutes.", 28, TextAnchor.MiddleCenter, Center(0f, -220f, 800f, 50f), GoldColor);
            Text statusText = CreateText(canvas.transform, "Text_Status", "Ready", 22, TextAnchor.MiddleCenter, Center(0f, -270f, 800f, 42f), MutedTextColor);
            
            // Build Version (matching Frame_01_MainMenu)
            CreateText(canvas.transform, "Text_BuildVersionPlaceholder", "Build v0.1", 20, TextAnchor.LowerRight, Anchored(1f, 0f, 1f, 0f, -150f, 40f, 240f, 40f), MutedTextColor);

            AssignObject(view, "newGameButton", newGameButton);
            AssignObject(view, "continueButton", continueButton);
            AssignObject(view, "resetButton", resetButton);
            AssignObject(view, "evaluationNoteText", evaluationNoteText);
            AssignObject(view, "statusText", statusText);

            SaveScene(scene, MainMenuPath);
        }

        private static void BuildTownScene()
        {
            Scene scene = CreateScene("Town");
            AddMainCamera();
            CreateEventSystem();
            Canvas canvas = CreateCanvas();
            AddGameBootstrap();

            GameObject viewObject = new GameObject("TownHUDView");
            TownHUDView view = viewObject.AddComponent<TownHUDView>();

            CreatePanel(canvas.transform, "Background", BackgroundColor, Stretch());
            
            // Panel_TopBar (matching Frame_02_TownHUD)
            GameObject topBar = CreatePanel(canvas.transform, "Panel_TopBar", PanelColor, Top(0f, -42f, 1872f, 78f));
            Text coinsText = CreateText(topBar.transform, "Text_Coins", "Coins: 150", 26, TextAnchor.MiddleLeft, Anchored(0f, 0.5f, 0f, 0.5f, 140f, 0f, 220f, 35f), GoldColor);
            Button inventoryButton = CreateButton(topBar.transform, "Button_Inventory", "Inventory", Anchored(1f, 0.5f, 1f, 0.5f, -100f, 0f, 160f, 50f));
            CreateText(topBar.transform, "Text_SaveNote", "Autosave ready", 22, TextAnchor.MiddleRight, Anchored(1f, 0.5f, 1f, 0.5f, -320f, 0f, 240f, 35f), MutedTextColor);

            // Panel_CharacterCard (matching Frame_02_TownHUD)
            GameObject charCard = CreatePanel(canvas.transform, "Panel_CharacterCard", PanelColor, Anchored(0f, 1f, 0f, 1f, 204f, -241f, 360f, 235f));
            GameObject portraitGo = CreatePanel(charCard.transform, "Image_PlayerPortrait", SubPanelColor, Anchored(0f, 1f, 0f, 1f, 67f, -67f, 86f, 86f));
            CreateText(portraitGo.transform, "Label", "Portrait", 18, TextAnchor.MiddleCenter, Stretch(), MutedTextColor);
            
            Text activeCharacterText = CreateText(charCard.transform, "Text_CharacterName", "Ruchir", 30, TextAnchor.MiddleLeft, Anchored(0.5f, 1f, 0.5f, 1f, 40f, -40f, 210f, 40f));
            Text levelText = CreateText(charCard.transform, "Text_Level", "Level 4 Beginner", 22, TextAnchor.MiddleLeft, Anchored(0.5f, 1f, 0.5f, 1f, 40f, -75f, 210f, 30f), MutedTextColor);
            Button characterPanelButton = CreateButton(charCard.transform, "Button_SwitchCharacter", "Switch", Anchored(1f, 1f, 1f, 1f, -72f, -40f, 112f, 44f));
            
            // ProgressBar_XP (nested text inside)
            Image xpFill = CreateProgressBar(charCard.transform, "ProgressBar_XP", Anchored(0.5f, 0.5f, 0.5f, 0.5f, 0f, -20f, 315f, 34f), BlueColor);
            Text xpText = CreateText(xpFill.transform.parent, "Text_ProgressBar_XP_Label", "60 / 105 XP", 18, TextAnchor.MiddleCenter, Stretch());
            
            Text taskText = CreateText(charCard.transform, "Text_CurrentTask", "Current Task: Fighting Slimes", 18, TextAnchor.MiddleLeft, Anchored(0.5f, 0f, 0.5f, 0f, 0f, 30f, 315f, 24f), MutedTextColor);

            // Panel_QuestTracker (matching Frame_02_TownHUD)
            GameObject questPanel = CreatePanel(canvas.transform, "Panel_QuestTracker", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 204f, -40f, 360f, 220f));
            Text questTitleText = CreateText(questPanel.transform, "Text_QuestTitle", "Quest: First Hunt", 26, TextAnchor.UpperLeft, Anchored(0.5f, 1f, 0.5f, 1f, 0f, -30f, 310f, 35f), GoldColor);
            Text questObjectiveText = CreateText(questPanel.transform, "Text_QuestObjective", "Kill 5 Slimes", 22, TextAnchor.UpperLeft, Anchored(0.5f, 1f, 0.5f, 1f, 0f, -65f, 310f, 30f));
            
            Image questFill = CreateProgressBar(questPanel.transform, "ProgressBar_QuestProgress", Anchored(0.5f, 0.5f, 0.5f, 0.5f, 0f, -20f, 315f, 30f), GoldColor);
            Text questProgressText = CreateText(questFill.transform.parent, "Text_ProgressBar_QuestProgress_Label", "2 / 5", 18, TextAnchor.MiddleCenter, Stretch());
            Button claimQuestButton = CreateButton(questPanel.transform, "Button_ClaimReward", "Claim", Anchored(0f, 0f, 0f, 0f, 94f, 35f, 140f, 38f));

            // WorldArea_Town (matching Frame_02_TownHUD)
            GameObject worldPanel = CreatePanel(canvas.transform, "WorldArea_Town", SubPanelColor, Center(125f, -50f, 1010f, 690f));
            CreateText(worldPanel.transform, "Text_WorldAreaTitle", "Town Hub", 34, TextAnchor.MiddleCenter, Rect(0f, 290f, 400f, 46f));
            
            GameObject npcGo = CreatePanel(worldPanel.transform, "Image_QuestNPC", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 160f, 60f, 140f, 180f));
            CreateText(npcGo.transform, "Label", "Quest NPC", 18, TextAnchor.MiddleCenter, Stretch(), MutedTextColor);
            
            GameObject craftGo = CreatePanel(worldPanel.transform, "Image_CraftingStation", PanelColor, Rect(35f, 50f, 180f, 150f));
            CreateText(craftGo.transform, "Label", "Crafting", 18, TextAnchor.MiddleCenter, Stretch(), MutedTextColor);
            
            GameObject storageGo = CreatePanel(worldPanel.transform, "Image_StorageChest", PanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -160f, 60f, 150f, 130f));
            CreateText(storageGo.transform, "Label", "Storage", 18, TextAnchor.MiddleCenter, Stretch(), MutedTextColor);

            // Activity buttons at bottom of Town Hub (matching Frame_02_TownHUD)
            Button combatButton = CreateButton(worldPanel.transform, "Button_GoCombat", "Fight Slimes", Rect(-270f, -120f, 220f, 58f));
            Button miningButton = CreateButton(worldPanel.transform, "Button_GoMining", "Mine Copper", Rect(-30f, -120f, 220f, 58f));
            Button characterPanelButton2 = CreateButton(worldPanel.transform, "Button_ClassTalent", "Class / Talents", Rect(220f, -120f, 250f, 58f));
            Button simulateAfkButton = CreateButton(worldPanel.transform, "Button_SimulateAfk", "Simulate 2 Hours AFK", Rect(-100f, -210f, 350f, 58f));
            CreateText(worldPanel.transform, "Text_AfkHelperNote", "Evaluation helper", 18, TextAnchor.MiddleLeft, Rect(200f, -210f, 210f, 24f), GoldColor);

            // Panel_LootLog (matching Frame_02_TownHUD)
            GameObject logPanel = CreatePanel(canvas.transform, "Panel_LootLog", PanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -204f, -40f, 360f, 310f));
            CreateText(logPanel.transform, "Text_LootLogTitle", "Recent", 28, TextAnchor.UpperLeft, Rect(0f, 110f, 300f, 38f));
            CreateText(logPanel.transform, "Row_Loot_XP", "+5 XP", 22, TextAnchor.UpperLeft, Rect(0f, 60f, 300f, 30f), LightGreenColor);
            CreateText(logPanel.transform, "Row_Loot_Coins", "+3 Coins", 22, TextAnchor.UpperLeft, Rect(0f, 20f, 300f, 30f), GoldColor);
            Text inventoryText = CreateText(logPanel.transform, "Row_Loot_Item", "+1 Slime Goo", 22, TextAnchor.UpperLeft, Rect(0f, -20f, 300f, 30f), LightGreenColor);
            Text statusText = CreateText(logPanel.transform, "Row_Loot_Info", "Recent rewards appear here", 18, TextAnchor.UpperLeft, Rect(0f, -80f, 300f, 50f), MutedTextColor);

            ToastView toastView = CreateToast(canvas.transform);
            InventoryCraftingPanel inventoryCraftingPanel = CreateInventoryCraftingPanel(canvas.transform, view, toastView);
            CharacterProgressionPanel characterProgressionPanel = CreateCharacterProgressionPanel(canvas.transform, view, toastView);
            AfkResultsModal afkResultsModal = CreateAfkResultsModal(canvas.transform);
            QuestDefinition[] questDefinitions = LoadQuestDefinitions();

            AssignObject(view, "inventoryButton", inventoryButton);
            AssignObject(view, "characterPanelButton", characterPanelButton2);
            AssignObject(view, "combatButton", combatButton);
            AssignObject(view, "miningButton", miningButton);
            AssignObject(view, "simulateAfkButton", simulateAfkButton);
            AssignObject(view, "claimQuestButton", claimQuestButton);
            AssignObject(view, "activeCharacterText", activeCharacterText);
            AssignObject(view, "levelText", levelText);
            AssignObject(view, "xpText", xpText);
            AssignObject(view, "taskText", taskText);
            AssignObject(view, "coinsText", coinsText);
            AssignObject(view, "inventoryText", inventoryText);
            AssignObject(view, "questTitleText", questTitleText);
            AssignObject(view, "questObjectiveText", questObjectiveText);
            AssignObject(view, "questProgressText", questProgressText);
            AssignObject(view, "questProgressFill", questFill);
            AssignObject(view, "statusText", statusText);
            AssignObject(view, "inventoryCraftingPanel", inventoryCraftingPanel);
            AssignObject(view, "characterProgressionPanel", characterProgressionPanel);
            AssignObject(view, "afkResultsModal", afkResultsModal);
            AssignObject(view, "toastView", toastView);
            AssignObjectArray(view, "questDefinitions", questDefinitions);

            SaveScene(scene, TownPath);
        }

        private static void BuildCombatScene()
        {
            Scene scene = CreateScene("CombatZone");
            AddMainCamera();
            CreateEventSystem();
            Canvas canvas = CreateCanvas();
            AddGameBootstrap();

            GameObject viewObject = new GameObject("CombatHUDView");
            CombatHUDView view = viewObject.AddComponent<CombatHUDView>();

            CreatePanel(canvas.transform, "Background", BackgroundColor, Stretch());
            
            // Panel_TopBar (matching Frame_03_CombatHUD)
            GameObject topBar = CreatePanel(canvas.transform, "Panel_TopBar", PanelColor, Top(0f, -42f, 1872f, 78f));
            Button backButton = CreateButton(topBar.transform, "Button_BackToTown", "Town", Anchored(0f, 0.5f, 0f, 0.5f, 100f, 0f, 140f, 50f));
            Text coinsText = CreateText(topBar.transform, "Text_Coins", "Coins: 150", 26, TextAnchor.MiddleLeft, Anchored(0f, 0.5f, 0f, 0.5f, 260f, 0f, 220f, 35f), GoldColor);
            Button inventoryButton = CreateButton(topBar.transform, "Button_Inventory", "Inventory", Anchored(1f, 0.5f, 1f, 0.5f, -100f, 0f, 160f, 50f));

            // Panel_CharacterCard (matching Frame_03_CombatHUD)
            GameObject charCard = CreatePanel(canvas.transform, "Panel_CharacterCard", PanelColor, Anchored(0f, 1f, 0f, 1f, 204f, -241f, 360f, 235f));
            GameObject portraitGo = CreatePanel(charCard.transform, "Image_PlayerPortrait", SubPanelColor, Anchored(0f, 1f, 0f, 1f, 67f, -67f, 86f, 86f));
            CreateText(portraitGo.transform, "Label", "Portrait", 18, TextAnchor.MiddleCenter, Stretch(), MutedTextColor);
            
            Text characterNameText = CreateText(charCard.transform, "Text_CharacterName", "Ruchir", 30, TextAnchor.MiddleLeft, Anchored(0.5f, 1f, 0.5f, 1f, 40f, -40f, 210f, 40f));
            Text levelText = CreateText(charCard.transform, "Text_Level", "Level 4 Beginner", 22, TextAnchor.MiddleLeft, Anchored(0.5f, 1f, 0.5f, 1f, 40f, -75f, 210f, 30f), MutedTextColor);
            
            Image xpFill = CreateProgressBar(charCard.transform, "ProgressBar_XP", Anchored(0.5f, 0.5f, 0.5f, 0.5f, 0f, -20f, 315f, 34f), BlueColor);
            CreateText(xpFill.transform.parent, "Text_ProgressBar_XP_Label", "60 / 105 XP", 18, TextAnchor.MiddleCenter, Stretch());
            CreateText(charCard.transform, "Text_CurrentTask", "Current Task: Fighting Slimes", 18, TextAnchor.MiddleLeft, Anchored(0.5f, 0f, 0.5f, 0f, 0f, 30f, 315f, 24f), MutedTextColor);
            CreateText(charCard.transform, "Text_DamageStat", "Damage: 4", 18, TextAnchor.MiddleLeft, Anchored(0.5f, 0f, 0.5f, 0f, 0f, 4f, 300f, 24f), LightGreenColor);

            // Consolidated Text field for the card reference
            Text characterText = CreateText(canvas.transform, "Text_Character_Consolidated", "Character 1 L1 XP 0", 1, TextAnchor.MiddleLeft, Rect(0f, 0f, 0f, 0f));
            characterText.gameObject.SetActive(false); // Used only to satisfy the view serialization hook

            // Panel_QuestTracker (matching Frame_03_CombatHUD)
            GameObject questPanel = CreatePanel(canvas.transform, "Panel_QuestTracker", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 204f, -40f, 360f, 220f));
            CreateText(questPanel.transform, "Text_QuestTitle", "Quest: First Hunt", 26, TextAnchor.UpperLeft, Anchored(0.5f, 1f, 0.5f, 1f, 0f, -30f, 310f, 35f), GoldColor);
            CreateText(questPanel.transform, "Text_QuestObjective", "Kill 5 Slimes", 22, TextAnchor.UpperLeft, Anchored(0.5f, 1f, 0.5f, 1f, 0f, -65f, 310f, 30f));
            
            Image questFill = CreateProgressBar(questPanel.transform, "ProgressBar_QuestProgress", Anchored(0.5f, 0.5f, 0.5f, 0.5f, 0f, -20f, 315f, 30f), GoldColor);
            CreateText(questFill.transform.parent, "Text_ProgressBar_QuestProgress_Label", "4 / 5", 18, TextAnchor.MiddleCenter, Stretch());
            CreateButton(questPanel.transform, "Button_ClaimReward_Disabled", "Claim", Anchored(0f, 0f, 0f, 0f, 94f, 35f, 140f, 38f));

            // WorldArea_Combat (matching Frame_03_CombatHUD)
            GameObject combatPanel = CreatePanel(canvas.transform, "WorldArea_Combat", SubPanelColor, Center(125f, -50f, 1010f, 690f));
            CreateText(combatPanel.transform, "Text_WorldAreaTitle", "Combat Zone", 34, TextAnchor.MiddleCenter, Rect(0f, 290f, 400f, 46f));
            
            GameObject playerGo = CreatePanel(combatPanel.transform, "Image_Player", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 160f, 60f, 150f, 210f));
            CreateText(playerGo.transform, "Label", "Player", 18, TextAnchor.MiddleCenter, Stretch(), MutedTextColor);
            
            GameObject slimeGo = CreatePanel(combatPanel.transform, "Image_Slime", new Color(0.310f, 0.706f, 0.467f, 1f), Anchored(1f, 0.5f, 1f, 0.5f, -160f, 60f, 130f, 120f));
            CreateText(slimeGo.transform, "Label", "Slime", 18, TextAnchor.MiddleCenter, Stretch(), TextColor);

            Text enemyText = CreateText(combatPanel.transform, "Text_EnemyName", "Slime", 28, TextAnchor.MiddleCenter, Rect(160f, 160f, 180f, 38f));
            Image enemyHpFill = CreateProgressBar(combatPanel.transform, "ProgressBar_EnemyHP", Rect(160f, 210f, 250f, 36f), RedColor);
            CreateText(enemyHpFill.transform.parent, "Text_ProgressBar_EnemyHP_Label", "6 / 10 HP", 18, TextAnchor.MiddleCenter, Stretch());

            CreateText(combatPanel.transform, "FloatingText_DamageExample", "-4", 42, TextAnchor.MiddleCenter, Rect(160f, 110f, 120f, 57f), RedColor);
            CreateText(combatPanel.transform, "Text_AutoCombatNote", "Auto-attacking — no attack button", 24, TextAnchor.MiddleCenter, Rect(0f, -80f, 460f, 32f), MutedTextColor);

            // Manual Interactive Buttons (Start Combat, Attack Tick) placed nicely
            Button startButton = CreateButton(combatPanel.transform, "Button_StartSlimeCombat", "Start Slime Combat", Rect(-145f, -210f, 270f, 58f));
            Button attackButton = CreateButton(combatPanel.transform, "Button_AttackTick", "Attack Tick", Rect(155f, -210f, 230f, 58f));

            // Panel_LootLog (matching Frame_03_CombatHUD)
            GameObject logPanel = CreatePanel(canvas.transform, "Panel_LootLog", PanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -204f, -40f, 360f, 310f));
            CreateText(logPanel.transform, "Text_LootLogTitle", "Loot Log", 28, TextAnchor.UpperLeft, Rect(0f, 110f, 300f, 38f));
            CreateText(logPanel.transform, "Row_Loot_XP", "+5 XP", 22, TextAnchor.UpperLeft, Rect(0f, 60f, 300f, 30f), LightGreenColor);
            CreateText(logPanel.transform, "Row_Loot_Coins", "+3 Coins", 22, TextAnchor.UpperLeft, Rect(0f, 20f, 300f, 30f), GoldColor);
            Text rewardText = CreateText(logPanel.transform, "Row_Loot_Item", "+1 Slime Goo", 22, TextAnchor.UpperLeft, Rect(0f, -20f, 300f, 30f), LightGreenColor);
            Text statusText = CreateText(logPanel.transform, "Row_Loot_Info", "Recent rewards appear here", 18, TextAnchor.UpperLeft, Rect(0f, -80f, 300f, 50f), MutedTextColor);
            ToastView toastView = CreateToast(canvas.transform);

            AssignObject(view, "slimeDefinition", AssetDatabase.LoadAssetAtPath<EnemyDefinition>(SlimeDefinitionPath));
            AssignObject(view, "backToTownButton", backButton);
            AssignObject(view, "startCombatButton", startButton);
            AssignObject(view, "attackButton", attackButton);
            AssignObject(view, "enemyHpFill", enemyHpFill);
            AssignObject(view, "characterText", characterText); //satisfy serialization
            AssignObject(view, "enemyText", enemyText);
            AssignObject(view, "rewardText", rewardText);
            AssignObject(view, "statusText", statusText);
            AssignObject(view, "toastView", toastView);
            AssignObjectArray(view, "questDefinitions", LoadQuestDefinitions());

            SaveScene(scene, CombatPath);
        }

        private static void BuildMineScene()
        {
            Scene scene = CreateScene("MineZone");
            AddMainCamera();
            CreateEventSystem();
            Canvas canvas = CreateCanvas();
            AddGameBootstrap();

            GameObject viewObject = new GameObject("MiningHUDView");
            MiningHUDView view = viewObject.AddComponent<MiningHUDView>();

            CreatePanel(canvas.transform, "Background", BackgroundColor, Stretch());
            
            // Panel_TopBar (matching Frame_04_MiningHUD)
            GameObject topBar = CreatePanel(canvas.transform, "Panel_TopBar", PanelColor, Top(0f, -42f, 1872f, 78f));
            Button backButton = CreateButton(topBar.transform, "Button_BackToTown", "Town", Anchored(0f, 0.5f, 0f, 0.5f, 100f, 0f, 140f, 50f));
            Text coinsText = CreateText(topBar.transform, "Text_Coins", "Coins: 150", 26, TextAnchor.MiddleLeft, Anchored(0f, 0.5f, 0f, 0.5f, 260f, 0f, 220f, 35f), GoldColor);
            Button inventoryButton = CreateButton(topBar.transform, "Button_Inventory", "Inventory", Anchored(1f, 0.5f, 1f, 0.5f, -100f, 0f, 160f, 50f));

            // Panel_CharacterCard (matching Frame_04_MiningHUD)
            GameObject charCard = CreatePanel(canvas.transform, "Panel_CharacterCard", PanelColor, Anchored(0f, 1f, 0f, 1f, 204f, -241f, 360f, 235f));
            GameObject portraitGo = CreatePanel(charCard.transform, "Image_PlayerPortrait", SubPanelColor, Anchored(0f, 1f, 0f, 1f, 67f, -67f, 86f, 86f));
            CreateText(portraitGo.transform, "Label", "Portrait", 18, TextAnchor.MiddleCenter, Stretch(), MutedTextColor);
            
            Text characterNameText = CreateText(charCard.transform, "Text_CharacterName", "Ruchir", 30, TextAnchor.MiddleLeft, Anchored(0.5f, 1f, 0.5f, 1f, 40f, -40f, 210f, 40f));
            Text levelText = CreateText(charCard.transform, "Text_Level", "Level 4 Beginner", 22, TextAnchor.MiddleLeft, Anchored(0.5f, 1f, 0.5f, 1f, 40f, -75f, 210f, 30f), MutedTextColor);
            
            Image xpFill = CreateProgressBar(charCard.transform, "ProgressBar_XP", Anchored(0.5f, 0.5f, 0.5f, 0.5f, 0f, -20f, 315f, 34f), BlueColor);
            CreateText(xpFill.transform.parent, "Text_ProgressBar_XP_Label", "60 / 105 XP", 18, TextAnchor.MiddleCenter, Stretch());
            CreateText(charCard.transform, "Text_CurrentTask", "Current Task: Fighting Slimes", 18, TextAnchor.MiddleLeft, Anchored(0.5f, 0f, 0.5f, 0f, 0f, 30f, 315f, 24f), MutedTextColor);
            CreateText(charCard.transform, "Text_MiningStat", "Mining Speed: 1.2x", 18, TextAnchor.MiddleLeft, Anchored(0.5f, 0f, 0.5f, 0f, 0f, 4f, 300f, 24f), LightGreenColor);

            // Consolidated Text field for card serialization
            Text characterText = CreateText(canvas.transform, "Text_Character_Consolidated", "Character 1 L1 XP 0", 1, TextAnchor.MiddleLeft, Rect(0f, 0f, 0f, 0f));
            characterText.gameObject.SetActive(false);

            // Panel_QuestTracker (matching Frame_04_MiningHUD)
            GameObject questPanel = CreatePanel(canvas.transform, "Panel_QuestTracker", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 204f, -40f, 360f, 220f));
            CreateText(questPanel.transform, "Text_QuestTitle", "Quest: Miner Time", 26, TextAnchor.UpperLeft, Anchored(0.5f, 1f, 0.5f, 1f, 0f, -30f, 310f, 35f), GoldColor);
            CreateText(questPanel.transform, "Text_QuestObjective", "Collect 10 Copper Ore", 22, TextAnchor.UpperLeft, Anchored(0.5f, 1f, 0.5f, 1f, 0f, -65f, 310f, 30f));
            
            Image questFill = CreateProgressBar(questPanel.transform, "ProgressBar_QuestProgress", Anchored(0.5f, 0.5f, 0.5f, 0.5f, 0f, -20f, 315f, 30f), GoldColor);
            CreateText(questFill.transform.parent, "Text_ProgressBar_QuestProgress_Label", "7 / 10", 18, TextAnchor.MiddleCenter, Stretch());
            CreateButton(questPanel.transform, "Button_ClaimReward_Disabled", "Claim", Anchored(0f, 0f, 0f, 0f, 94f, 35f, 140f, 38f));

            // WorldArea_Mining (matching Frame_04_MiningHUD)
            GameObject minePanel = CreatePanel(canvas.transform, "WorldArea_Mining", SubPanelColor, Center(125f, -50f, 1010f, 690f));
            CreateText(minePanel.transform, "Text_WorldAreaTitle", "Copper Mine", 34, TextAnchor.MiddleCenter, Rect(0f, 290f, 400f, 46f));
            
            GameObject playerGo = CreatePanel(minePanel.transform, "Image_Player", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 160f, 60f, 150f, 210f));
            CreateText(playerGo.transform, "Label", "Player", 18, TextAnchor.MiddleCenter, Stretch(), MutedTextColor);
            
            GameObject oreGo = CreatePanel(minePanel.transform, "Image_CopperNode", new Color(0.722f, 0.451f, 0.200f, 1f), Anchored(1f, 0.5f, 1f, 0.5f, -160f, 60f, 220f, 200f));
            CreateText(oreGo.transform, "Label", "Copper Node", 18, TextAnchor.MiddleCenter, Stretch(), TextColor);

            Image miningProgressFill = CreateProgressBar(minePanel.transform, "ProgressBar_MiningProgress", Rect(0f, -100f, 420f, 42f), GoldColor);
            Text progressText = CreateText(miningProgressFill.transform.parent, "Text_ProgressBar_MiningProgress_Label", "Mining Copper... 68%", 18, TextAnchor.MiddleCenter, Stretch());

            // Manual buttons (Start Mining, Mine Tick)
            Button startButton = CreateButton(minePanel.transform, "Button_StartCopperMining", "Start Copper Mining", Rect(-145f, -210f, 290f, 58f));
            Button tickButton = CreateButton(minePanel.transform, "Button_MineTick", "Mine Tick", Rect(165f, -210f, 220f, 58f));

            // Panel_RecentRewards (matching Frame_04_MiningHUD)
            GameObject logPanel = CreatePanel(canvas.transform, "Panel_RecentRewards", PanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -204f, -40f, 360f, 260f));
            CreateText(logPanel.transform, "Text_RecentRewardsTitle", "Recent Rewards", 28, TextAnchor.UpperLeft, Rect(0f, 85f, 300f, 38f));
            CreateText(logPanel.transform, "Row_CopperOre", "+1 Copper Ore", 22, TextAnchor.UpperLeft, Rect(0f, 35f, 300f, 30f), LightGreenColor);
            Text rewardText = CreateText(logPanel.transform, "Row_MiningXP", "+2 XP", 22, TextAnchor.UpperLeft, Rect(0f, -5f, 300f, 30f), BlueColor);
            Text statusText = CreateText(logPanel.transform, "Text_Status", "Ready", 18, TextAnchor.UpperLeft, Rect(0f, -55f, 300f, 40f), MutedTextColor);
            ToastView toastView = CreateToast(canvas.transform);

            AssignObject(view, "backToTownButton", backButton);
            AssignObject(view, "startMiningButton", startButton);
            AssignObject(view, "tickMiningButton", tickButton);
            AssignObject(view, "miningProgressFill", miningProgressFill);
            AssignObject(view, "characterText", characterText); //satisfy serialization
            AssignObject(view, "progressText", progressText);
            AssignObject(view, "rewardText", rewardText);
            AssignObject(view, "statusText", statusText);
            AssignObject(view, "toastView", toastView);
            AssignObjectArray(view, "questDefinitions", LoadQuestDefinitions());

            SaveScene(scene, MinePath);
        }

        private static Scene CreateScene(string sceneName)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = sceneName;
            return scene;
        }

        private static Camera AddMainCamera()
        {
            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = BackgroundColor;
            camera.orthographic = true;
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);
            return camera;
        }

        private static GameBootstrap AddGameBootstrap()
        {
            GameObject bootstrapObject = new GameObject("GameBootstrap");
            return bootstrapObject.AddComponent<GameBootstrap>();
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        private static Canvas CreateCanvas()
        {
            GameObject canvasObject = new GameObject("Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObject.AddComponent<GraphicRaycaster>();
            RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            return canvas;
        }

        private static GameObject CreatePanel(Transform parent, string name, Color color, RectPreset preset)
        {
            GameObject panel = CreateUiObject(name, parent, preset);
            Image image = panel.AddComponent<Image>();
            image.color = color;
            return panel;
        }

        private static Text CreateText(Transform parent, string name, string value, int fontSize, TextAnchor alignment, RectPreset preset)
        {
            return CreateText(parent, name, value, fontSize, alignment, preset, TextColor);
        }

        private static Text CreateText(Transform parent, string name, string value, int fontSize, TextAnchor alignment, RectPreset preset, Color color)
        {
            GameObject textObject = CreateUiObject(name, parent, preset);
            Text text = textObject.AddComponent<Text>();
            text.font = GetUiFont();
            text.text = string.IsNullOrEmpty(value) ? " " : value;
            text.color = color;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        private static Button CreateButton(Transform parent, string name, string label, RectPreset preset)
        {
            GameObject buttonObject = CreateUiObject(name, parent, preset);
            Image image = buttonObject.AddComponent<Image>();
            image.color = ButtonColor;

            Button button = buttonObject.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = ButtonColor;
            colors.highlightedColor = ButtonHighlightColor;
            colors.pressedColor = new Color(0.18f, 0.25f, 0.38f, 1f);
            colors.selectedColor = ButtonHighlightColor;
            colors.disabledColor = new Color(0.22f, 0.23f, 0.27f, 0.65f);
            button.colors = colors;

            Text labelText = CreateText(buttonObject.transform, "Text", label, 24, TextAnchor.MiddleCenter, Stretch(18f, 8f, 18f, 8f));
            labelText.raycastTarget = false;
            return button;
        }

        private static Image CreateProgressBar(Transform parent, string name, RectPreset preset, Color fillColor)
        {
            GameObject root = CreatePanel(parent, name, new Color(0.04f, 0.05f, 0.07f, 1f), preset);
            GameObject fillObject = CreateUiObject("Fill", root.transform, Stretch());
            Image fill = fillObject.AddComponent<Image>();
            fill.color = fillColor;
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = (int)Image.OriginHorizontal.Left;
            fill.fillAmount = 0.5f;
            return fill;
        }

        private static InventoryCraftingPanel CreateInventoryCraftingPanel(
            Transform parent,
            TownHUDView townHudView,
            ToastView toastView)
        {
            GameObject panel = CreatePanel(parent, "Panel_InventoryCrafting", PanelColor, Center(0f, 0f, 1180f, 740f));
            InventoryCraftingPanel view = panel.AddComponent<InventoryCraftingPanel>();
            CreateText(panel.transform, "Text_Title", "Inventory & Crafting", 34, TextAnchor.MiddleLeft, Anchored(0f, 1f, 0f, 1f, 260f, -45f, 420f, 48f), GoldColor);
            Button closeButton = CreateButton(panel.transform, "Button_Close", "Close", Anchored(1f, 1f, 1f, 1f, -95f, -45f, 140f, 48f));

            GameObject inventoryPanel = CreatePanel(panel.transform, "Panel_Inventory", SubPanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 275f, -10f, 470f, 520f));
            CreateText(inventoryPanel.transform, "Text_Header", "Inventory", 28, TextAnchor.UpperLeft, Rect(0f, 210f, 390f, 42f));
            Text inventoryText = CreateText(inventoryPanel.transform, "Text_ItemQuantities", "Copper Ore: 0\nCopper Bar: 0\nSlime Goo: 0\nCopper Sword: 0\nCopper Pickaxe: 0\nCoins: 0", 24, TextAnchor.UpperLeft, Rect(0f, -10f, 390f, 360f), TextColor);

            GameObject craftingPanel = CreatePanel(panel.transform, "Panel_Crafting", SubPanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -335f, -10f, 560f, 520f));
            CreateText(craftingPanel.transform, "Text_Header", "Crafting", 28, TextAnchor.UpperLeft, Rect(0f, 210f, 470f, 42f));

            Text copperBarRecipeText = CreateText(craftingPanel.transform, "Recipe_CopperBar_Text", "Copper Bar\nNeeds: 3 Copper Ore", 21, TextAnchor.UpperLeft, Rect(-90f, 130f, 330f, 86f), TextColor);
            Button craftCopperBarButton = CreateButton(craftingPanel.transform, "Button_CraftCopperBar", "Craft", Rect(190f, 120f, 130f, 50f));

            Text copperSwordRecipeText = CreateText(craftingPanel.transform, "Recipe_CopperSword_Text", "Copper Sword\nNeeds: 2 Copper Bar + 3 Slime Goo", 21, TextAnchor.UpperLeft, Rect(-90f, -10f, 330f, 96f), TextColor);
            Button craftCopperSwordButton = CreateButton(craftingPanel.transform, "Button_CraftCopperSword", "Craft", Rect(190f, -20f, 130f, 50f));

            Text copperPickaxeRecipeText = CreateText(craftingPanel.transform, "Recipe_CopperPickaxe_Text", "Copper Pickaxe\nNeeds: 2 Copper Bar", 21, TextAnchor.UpperLeft, Rect(-90f, -150f, 330f, 86f), TextColor);
            Button craftCopperPickaxeButton = CreateButton(craftingPanel.transform, "Button_CraftCopperPickaxe", "Craft", Rect(190f, -160f, 130f, 50f));

            Text statusText = CreateText(panel.transform, "Text_Status", "Crafting status appears here.", 23, TextAnchor.MiddleLeft, Anchored(0.5f, 0f, 0.5f, 0f, 0f, 46f, 980f, 42f), MutedTextColor);

            AssignObject(view, "root", panel);
            AssignObject(view, "closeButton", closeButton);
            AssignObject(view, "inventoryText", inventoryText);
            AssignObject(view, "copperBarRecipeText", copperBarRecipeText);
            AssignObject(view, "copperSwordRecipeText", copperSwordRecipeText);
            AssignObject(view, "copperPickaxeRecipeText", copperPickaxeRecipeText);
            AssignObject(view, "craftCopperBarButton", craftCopperBarButton);
            AssignObject(view, "craftCopperSwordButton", craftCopperSwordButton);
            AssignObject(view, "craftCopperPickaxeButton", craftCopperPickaxeButton);
            AssignObject(view, "statusText", statusText);
            AssignObject(view, "toastView", toastView);
            AssignObject(view, "townHudView", townHudView);
            AssignObject(view, "copperBarRecipe", AssetDatabase.LoadAssetAtPath<RecipeDefinition>(CopperBarRecipePath));
            AssignObject(view, "copperSwordRecipe", AssetDatabase.LoadAssetAtPath<RecipeDefinition>(CopperSwordRecipePath));
            AssignObject(view, "copperPickaxeRecipe", AssetDatabase.LoadAssetAtPath<RecipeDefinition>(CopperPickaxeRecipePath));
            AssignObjectArray(view, "questDefinitions", LoadQuestDefinitions());

            panel.SetActive(false);
            return view;
        }

        private static CharacterProgressionPanel CreateCharacterProgressionPanel(
            Transform parent,
            TownHUDView townHudView,
            ToastView toastView)
        {
            GameObject panel = CreatePanel(parent, "Panel_CharacterProgression", PanelColor, Center(0f, 0f, 1220f, 720f));
            CharacterProgressionPanel view = panel.AddComponent<CharacterProgressionPanel>();
            CreateText(panel.transform, "Text_Title", "Character / Class / Talents", 34, TextAnchor.MiddleLeft, Anchored(0f, 1f, 0f, 1f, 320f, -45f, 560f, 48f), GoldColor);
            Button closeButton = CreateButton(panel.transform, "Button_Close", "Close", Anchored(1f, 1f, 1f, 1f, -95f, -45f, 140f, 48f));

            GameObject summaryPanel = CreatePanel(panel.transform, "Panel_CharacterSummary", SubPanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 270f, -5f, 440f, 520f));
            Text summaryText = CreateText(summaryPanel.transform, "Text_Summary", "Character\nLevel 1\nClass: None\nTalent Points: 0", 24, TextAnchor.UpperLeft, Rect(0f, 100f, 360f, 230f));
            Text statsText = CreateText(summaryPanel.transform, "Text_Stats", "Damage: 3\nMining Speed: 1x\nXP Gain: 1x\nDrop Rate: 1x\nAFK Gain: 1x", 23, TextAnchor.UpperLeft, Rect(0f, -120f, 360f, 230f), MutedTextColor);

            GameObject classPanel = CreatePanel(panel.transform, "Panel_ClassChoice", SubPanelColor, Center(130f, 80f, 330f, 360f));
            CreateText(classPanel.transform, "Text_Header", "Class Choice", 27, TextAnchor.UpperCenter, Rect(0f, 135f, 280f, 38f), GoldColor);
            Button warriorButton = CreateButton(classPanel.transform, "Button_Warrior", "Warrior +Damage", Rect(0f, 70f, 250f, 52f));
            Button archerButton = CreateButton(classPanel.transform, "Button_Archer", "Archer +Drop", Rect(0f, 0f, 250f, 52f));
            Button mageButton = CreateButton(classPanel.transform, "Button_Mage", "Mage +AFK", Rect(0f, -70f, 250f, 52f));

            GameObject talentPanel = CreatePanel(panel.transform, "Panel_Talents", SubPanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -270f, -5f, 420f, 520f));
            CreateText(talentPanel.transform, "Text_Header", "Talents", 27, TextAnchor.UpperCenter, Rect(0f, 205f, 340f, 38f), GoldColor);
            Button damageTalentButton = CreateButton(talentPanel.transform, "TalentNode_Damage", "Damage + (0)", Rect(0f, 125f, 310f, 56f));
            Button miningSpeedTalentButton = CreateButton(talentPanel.transform, "TalentNode_MiningSpeed", "Mining Speed + (0)", Rect(0f, 50f, 310f, 56f));
            Button xpGainTalentButton = CreateButton(talentPanel.transform, "TalentNode_XPGain", "XP Gain + (0)", Rect(0f, -25f, 310f, 56f));
            Button afkGainTalentButton = CreateButton(talentPanel.transform, "TalentNode_AfkGain", "AFK Gain + (0)", Rect(0f, -100f, 310f, 56f));

            Text statusText = CreateText(panel.transform, "Text_Status", "Class unlocks at level 5.", 23, TextAnchor.MiddleLeft, Anchored(0.5f, 0f, 0.5f, 0f, 0f, 44f, 1000f, 42f), MutedTextColor);

            AssignObject(view, "root", panel);
            AssignObject(view, "closeButton", closeButton);
            AssignObject(view, "summaryText", summaryText);
            AssignObject(view, "statsText", statsText);
            AssignObject(view, "statusText", statusText);
            AssignObject(view, "warriorButton", warriorButton);
            AssignObject(view, "archerButton", archerButton);
            AssignObject(view, "mageButton", mageButton);
            AssignObject(view, "damageTalentButton", damageTalentButton);
            AssignObject(view, "miningSpeedTalentButton", miningSpeedTalentButton);
            AssignObject(view, "xpGainTalentButton", xpGainTalentButton);
            AssignObject(view, "afkGainTalentButton", afkGainTalentButton);
            AssignObject(view, "toastView", toastView);
            AssignObject(view, "townHudView", townHudView);
            AssignObjectArray(view, "questDefinitions", LoadQuestDefinitions());

            panel.SetActive(false);
            return view;
        }

        private static AfkResultsModal CreateAfkResultsModal(Transform parent)
        {
            GameObject panel = CreatePanel(parent, "Modal_AfkResults", PanelColor, Center(0f, 0f, 820f, 560f));
            AfkResultsModal modal = panel.AddComponent<AfkResultsModal>();
            Text titleText = CreateText(panel.transform, "Text_Title", "AFK Results", 36, TextAnchor.MiddleCenter, Anchored(0.5f, 1f, 0.5f, 1f, 0f, -50f, 700f, 56f), GoldColor);
            Text durationText = CreateText(panel.transform, "Text_AwayDuration", "120 minutes simulated", 24, TextAnchor.MiddleCenter, Anchored(0.5f, 1f, 0.5f, 1f, 0f, -100f, 700f, 38f), MutedTextColor);
            Text resultsText = CreateText(panel.transform, "List_CharacterRewardSummaries", "Character rewards appear here.", 24, TextAnchor.UpperLeft, Rect(0f, -20f, 680f, 300f));
            Button closeButton = CreateButton(panel.transform, "Button_ClaimContinue", "Continue", Anchored(0.5f, 0f, 0.5f, 0f, 0f, 54f, 240f, 58f));

            AssignObject(modal, "root", panel);
            AssignObject(modal, "titleText", titleText);
            AssignObject(modal, "durationText", durationText);
            AssignObject(modal, "resultsText", resultsText);
            AssignObject(modal, "closeButton", closeButton);

            panel.SetActive(false);
            return modal;
        }

        private static ToastView CreateToast(Transform parent)
        {
            GameObject toastObject = CreatePanel(parent, "Toast_Default", new Color(0.08f, 0.10f, 0.14f, 0.96f), Anchored(0.5f, 1f, 0.5f, 1f, 0f, -112f, 620f, 68f));
            CanvasGroup canvasGroup = toastObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            ToastView toastView = toastObject.AddComponent<ToastView>();
            Text toastText = CreateText(toastObject.transform, "Text_Toast", "Status", 24, TextAnchor.MiddleCenter, Stretch(18f, 8f, 18f, 8f), TextColor);

            AssignObject(toastView, "toastText", toastText);
            AssignObject(toastView, "canvasGroup", canvasGroup);
            return toastView;
        }

        private static GameObject CreateUiObject(string name, Transform parent, RectPreset preset)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.SetParent(parent, false);
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            ApplyRect(rectTransform, preset);
            gameObject.transform.localScale = Vector3.one;
            return gameObject;
        }

        private static void ApplyRect(RectTransform rectTransform, RectPreset preset)
        {
            rectTransform.anchorMin = preset.anchorMin;
            rectTransform.anchorMax = preset.anchorMax;
            rectTransform.pivot = preset.pivot;
            rectTransform.anchoredPosition = preset.anchoredPosition;
            rectTransform.sizeDelta = preset.sizeDelta;
            if (preset.useOffsets)
            {
                rectTransform.offsetMin = preset.offsetMin;
                rectTransform.offsetMax = preset.offsetMax;
            }

            rectTransform.localScale = Vector3.one;
        }

        private static Font GetUiFont()
        {
            if (cachedFont == null)
            {
                cachedFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }

            return cachedFont;
        }

        private static void AssignObject(Object target, string propertyName, Object value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                Debug.LogWarning($"{target.GetType().Name} is missing serialized field '{propertyName}'.");
                return;
            }

            property.objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void AssignObjectArray(Object target, string propertyName, Object[] values)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null || !property.isArray)
            {
                Debug.LogWarning($"{target.GetType().Name} is missing serialized array field '{propertyName}'.");
                return;
            }

            Object[] safeValues = values ?? new Object[0];
            property.arraySize = safeValues.Length;
            for (int i = 0; i < safeValues.Length; i++)
            {
                property.GetArrayElementAtIndex(i).objectReferenceValue = safeValues[i];
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static QuestDefinition[] LoadQuestDefinitions()
        {
            string[] guids = AssetDatabase.FindAssets("t:QuestDefinition", new[] { "Assets/Data/Quests" });
            List<QuestDefinition> quests = new List<QuestDefinition>();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                QuestDefinition quest = AssetDatabase.LoadAssetAtPath<QuestDefinition>(path);
                if (quest != null)
                {
                    quests.Add(quest);
                }
            }

            quests.Sort((left, right) => string.CompareOrdinal(left.Id, right.Id));
            return quests.ToArray();
        }

        private static void SaveScene(Scene scene, string path)
        {
            EditorSceneManager.SaveScene(scene, path);
        }

        private static void EnsureBuildSettings()
        {
            string[] requiredScenePaths =
            {
                MainMenuPath,
                TownPath,
                CombatPath,
                MinePath
            };

            Dictionary<string, EditorBuildSettingsScene> scenesByPath = new Dictionary<string, EditorBuildSettingsScene>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!string.IsNullOrEmpty(scene.path) && !scenesByPath.ContainsKey(scene.path))
                {
                    scenesByPath.Add(scene.path, scene);
                }
            }

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            foreach (string scenePath in requiredScenePaths)
            {
                scenes.Add(scenesByPath.TryGetValue(scenePath, out EditorBuildSettingsScene existing)
                    ? new EditorBuildSettingsScene(existing.path, true)
                    : new EditorBuildSettingsScene(scenePath, true));
            }

            HashSet<string> requiredPaths = new HashSet<string>(requiredScenePaths);
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!string.IsNullOrEmpty(scene.path) && !requiredPaths.Contains(scene.path))
                {
                    scenes.Add(scene);
                }
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static RectPreset Stretch()
        {
            return Stretch(0f, 0f, 0f, 0f);
        }

        private static RectPreset Stretch(float left, float bottom, float right, float top)
        {
            return new RectPreset
            {
                anchorMin = Vector2.zero,
                anchorMax = Vector2.one,
                pivot = new Vector2(0.5f, 0.5f),
                anchoredPosition = Vector2.zero,
                sizeDelta = Vector2.zero,
                offsetMin = new Vector2(left, bottom),
                offsetMax = new Vector2(-right, -top),
                useOffsets = true
            };
        }

        private static RectPreset Center(float x, float y, float width, float height)
        {
            return Anchored(0.5f, 0.5f, 0.5f, 0.5f, x, y, width, height);
        }

        private static RectPreset Top(float x, float y, float width, float height)
        {
            return Anchored(0.5f, 1f, 0.5f, 1f, x, y, width, height);
        }

        private static RectPreset Rect(float x, float y, float width, float height)
        {
            return Anchored(0.5f, 0.5f, 0.5f, 0.5f, x, y, width, height);
        }

        private static RectPreset Anchored(float minX, float minY, float maxX, float maxY, float x, float y, float width, float height)
        {
            return new RectPreset
            {
                anchorMin = new Vector2(minX, minY),
                anchorMax = new Vector2(maxX, maxY),
                pivot = new Vector2(0.5f, 0.5f),
                anchoredPosition = new Vector2(x, y),
                sizeDelta = new Vector2(width, height),
                offsetMin = Vector2.zero,
                offsetMax = Vector2.zero,
                useOffsets = false
            };
        }

        private struct RectPreset
        {
            public Vector2 anchorMin;
            public Vector2 anchorMax;
            public Vector2 pivot;
            public Vector2 anchoredPosition;
            public Vector2 sizeDelta;
            public Vector2 offsetMin;
            public Vector2 offsetMax;
            public bool useOffsets;
        }
    }
}
