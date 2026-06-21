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

        private static readonly Color BackgroundColor = new Color(0.035f, 0.045f, 0.065f, 1f);
        private static readonly Color PanelColor = new Color(0.11f, 0.13f, 0.18f, 0.96f);
        private static readonly Color SubPanelColor = new Color(0.16f, 0.19f, 0.25f, 0.96f);
        private static readonly Color ButtonColor = new Color(0.23f, 0.32f, 0.48f, 1f);
        private static readonly Color ButtonHighlightColor = new Color(0.31f, 0.42f, 0.62f, 1f);
        private static readonly Color TextColor = new Color(0.94f, 0.96f, 1f, 1f);
        private static readonly Color MutedTextColor = new Color(0.72f, 0.78f, 0.86f, 1f);

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
            GameObject titlePanel = CreatePanel(canvas.transform, "TitlePanel", PanelColor, Center(0f, 90f, 760f, 520f));
            CreateText(titlePanel.transform, "TitleText", "Idle Guild", 56, TextAnchor.MiddleCenter, Rect(0f, 160f, 680f, 92f));
            CreateText(titlePanel.transform, "SubtitleText", "A small backend-first idle RPG demo", 28, TextAnchor.MiddleCenter, Rect(0f, 90f, 680f, 54f), MutedTextColor);

            Button newGameButton = CreateButton(titlePanel.transform, "NewGameButton", "New Game", Rect(0f, 10f, 420f, 72f));
            Button continueButton = CreateButton(titlePanel.transform, "ContinueButton", "Continue", Rect(0f, -78f, 420f, 72f));
            Button resetButton = CreateButton(titlePanel.transform, "DeleteSaveButton", "Delete Save", Rect(0f, -166f, 420f, 64f));
            Text evaluationNoteText = CreateText(titlePanel.transform, "EvaluationNoteText", "Evaluation note: systems-first demo flow for quick testing.", 24, TextAnchor.MiddleCenter, Rect(0f, -248f, 680f, 54f), MutedTextColor);
            Text statusText = CreateText(titlePanel.transform, "StatusText", "Ready", 22, TextAnchor.MiddleCenter, Rect(0f, -304f, 680f, 42f), MutedTextColor);
            CreateText(canvas.transform, "BuildVersionText", "Unity demo build", 22, TextAnchor.LowerRight, Anchored(1f, 0f, 1f, 0f, -260f, 20f, 240f, 40f), MutedTextColor);

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
            CreatePanel(canvas.transform, "TopBar", PanelColor, Top(0f, -42f, 1920f, 84f));
            Text coinsText = CreateText(canvas.transform, "CoinsInventoryText", "Coins 0 | Slime Goo 0 | Copper Ore 0", 26, TextAnchor.MiddleLeft, Anchored(0f, 1f, 0f, 1f, 36f, -42f, 820f, 56f));
            CreateText(canvas.transform, "SaveText", "Autosave ready", 22, TextAnchor.MiddleRight, Anchored(1f, 1f, 1f, 1f, -360f, -42f, 320f, 56f), MutedTextColor);

            GameObject characterPanel = CreatePanel(canvas.transform, "CharacterCard", PanelColor, Anchored(0f, 1f, 0f, 1f, 36f, -200f, 360f, 220f));
            Text activeCharacterText = CreateText(characterPanel.transform, "CharacterNameText", "Character 1", 30, TextAnchor.MiddleLeft, Rect(0f, 66f, 300f, 46f));
            Text levelText = CreateText(characterPanel.transform, "LevelText", "Level 1 | XP 0", 24, TextAnchor.MiddleLeft, Rect(0f, 14f, 300f, 42f), MutedTextColor);
            Text xpText = CreateText(characterPanel.transform, "XpText", "XP 0", 24, TextAnchor.MiddleLeft, Rect(0f, -36f, 300f, 42f), MutedTextColor);
            Text taskText = CreateText(characterPanel.transform, "TaskText", "Task: idle", 22, TextAnchor.MiddleLeft, Rect(0f, -86f, 300f, 42f), MutedTextColor);

            GameObject questPanel = CreatePanel(canvas.transform, "QuestTracker", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 36f, 40f, 420f, 260f));
            CreateText(questPanel.transform, "QuestTitleText", "Quest Tracker", 30, TextAnchor.UpperLeft, Rect(0f, 82f, 360f, 60f));
            CreateText(questPanel.transform, "QuestBodyText", "Current quest progress appears here once the quest UI is wired.", 24, TextAnchor.UpperLeft, Rect(0f, -24f, 360f, 138f), MutedTextColor);

            GameObject worldPanel = CreatePanel(canvas.transform, "TownWorldArea", SubPanelColor, Center(120f, -10f, 820f, 500f));
            CreateText(worldPanel.transform, "WorldTitleText", "Town", 40, TextAnchor.MiddleCenter, Rect(0f, 70f, 620f, 80f));
            CreateText(worldPanel.transform, "WorldBodyText", "Guild hall, crafting bench, mine road, and slime trail placeholders", 26, TextAnchor.MiddleCenter, Rect(0f, -10f, 680f, 110f), MutedTextColor);

            GameObject logPanel = CreatePanel(canvas.transform, "RecentRewardLog", PanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -300f, -20f, 420f, 440f));
            Text inventoryText = CreateText(logPanel.transform, "InventoryText", "Inventory: Slime Goo 0, Copper Ore 0", 24, TextAnchor.UpperLeft, Rect(0f, 112f, 350f, 120f), MutedTextColor);
            Text statusText = CreateText(logPanel.transform, "StatusText", "Ready", 24, TextAnchor.UpperLeft, Rect(0f, -40f, 350f, 170f), MutedTextColor);

            GameObject buttonPanel = CreatePanel(canvas.transform, "ActivityButtons", PanelColor, Anchored(0.5f, 0f, 0.5f, 0f, 0f, 110f, 1180f, 120f));
            Button combatButton = CreateButton(buttonPanel.transform, "FightSlimesButton", "Fight Slimes", Rect(-470f, 0f, 200f, 70f));
            Button miningButton = CreateButton(buttonPanel.transform, "MineCopperButton", "Mine Copper", Rect(-235f, 0f, 200f, 70f));
            Button inventoryButton = CreateButton(buttonPanel.transform, "InventoryButton", "Inventory", Rect(0f, 0f, 200f, 70f));
            Button characterPanelButton = CreateButton(buttonPanel.transform, "CharacterTalentsButton", "Character / Talents", Rect(235f, 0f, 250f, 70f));
            Button simulateAfkButton = CreateButton(buttonPanel.transform, "SimulateAfkButton", "Simulate 2 Hours AFK", Rect(520f, 0f, 300f, 70f));

            AssignObject(view, "inventoryButton", inventoryButton);
            AssignObject(view, "characterPanelButton", characterPanelButton);
            AssignObject(view, "combatButton", combatButton);
            AssignObject(view, "miningButton", miningButton);
            AssignObject(view, "simulateAfkButton", simulateAfkButton);
            AssignObject(view, "activeCharacterText", activeCharacterText);
            AssignObject(view, "levelText", levelText);
            AssignObject(view, "xpText", xpText);
            AssignObject(view, "taskText", taskText);
            AssignObject(view, "coinsText", coinsText);
            AssignObject(view, "inventoryText", inventoryText);
            AssignObject(view, "statusText", statusText);

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
            CreatePanel(canvas.transform, "TopBar", PanelColor, Top(0f, -42f, 1920f, 84f));
            Button backButton = CreateButton(canvas.transform, "BackToTownButton", "Back to Town", Anchored(0f, 1f, 0f, 1f, 36f, -42f, 230f, 58f));
            CreateText(canvas.transform, "TopStatsText", "Coins and inventory update after rewards save", 24, TextAnchor.MiddleRight, Anchored(1f, 1f, 1f, 1f, -520f, -42f, 480f, 56f), MutedTextColor);

            GameObject characterPanel = CreatePanel(canvas.transform, "CharacterCard", PanelColor, Anchored(0f, 1f, 0f, 1f, 36f, -205f, 380f, 230f));
            Text characterText = CreateText(characterPanel.transform, "CharacterText", "Character 1 L1 XP 0", 26, TextAnchor.UpperLeft, Rect(0f, 70f, 320f, 120f));

            GameObject questPanel = CreatePanel(canvas.transform, "QuestTracker", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 36f, 20f, 420f, 260f));
            CreateText(questPanel.transform, "QuestText", "Quest Tracker\nDefeat Slimes", 26, TextAnchor.UpperLeft, Rect(0f, 20f, 350f, 170f), MutedTextColor);

            GameObject combatPanel = CreatePanel(canvas.transform, "CombatArena", SubPanelColor, Center(120f, -20f, 860f, 520f));
            CreateText(combatPanel.transform, "PlayerPlaceholder", "Player", 34, TextAnchor.MiddleCenter, Rect(-250f, 40f, 220f, 100f));
            CreateText(combatPanel.transform, "SlimePlaceholder", "Slime", 34, TextAnchor.MiddleCenter, Rect(250f, 40f, 220f, 100f));
            Text enemyText = CreateText(combatPanel.transform, "EnemyHpText", "Slime HP --", 28, TextAnchor.MiddleCenter, Rect(0f, -108f, 500f, 60f));
            Image enemyHpFill = CreateProgressBar(combatPanel.transform, "EnemyHpBar", Rect(0f, -164f, 520f, 34f), new Color(0.78f, 0.2f, 0.25f, 1f));
            Button startButton = CreateButton(combatPanel.transform, "StartSlimeCombatButton", "Start Slime Combat", Rect(-145f, -230f, 270f, 66f));
            Button attackButton = CreateButton(combatPanel.transform, "AttackTickButton", "Attack Tick", Rect(155f, -230f, 230f, 66f));

            GameObject logPanel = CreatePanel(canvas.transform, "LootResultLog", PanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -300f, -20f, 420f, 440f));
            Text rewardText = CreateText(logPanel.transform, "RewardText", "Loot/result log", 24, TextAnchor.UpperLeft, Rect(0f, 92f, 350f, 150f), MutedTextColor);
            Text statusText = CreateText(logPanel.transform, "StatusText", "Ready", 24, TextAnchor.UpperLeft, Rect(0f, -80f, 350f, 170f), MutedTextColor);

            AssignObject(view, "slimeDefinition", AssetDatabase.LoadAssetAtPath<EnemyDefinition>(SlimeDefinitionPath));
            AssignObject(view, "backToTownButton", backButton);
            AssignObject(view, "startCombatButton", startButton);
            AssignObject(view, "attackButton", attackButton);
            AssignObject(view, "enemyHpFill", enemyHpFill);
            AssignObject(view, "characterText", characterText);
            AssignObject(view, "enemyText", enemyText);
            AssignObject(view, "rewardText", rewardText);
            AssignObject(view, "statusText", statusText);

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
            CreatePanel(canvas.transform, "TopBar", PanelColor, Top(0f, -42f, 1920f, 84f));
            Button backButton = CreateButton(canvas.transform, "BackToTownButton", "Back to Town", Anchored(0f, 1f, 0f, 1f, 36f, -42f, 230f, 58f));
            CreateText(canvas.transform, "TopStatsText", "Copper Ore appears in inventory after completed ticks", 24, TextAnchor.MiddleRight, Anchored(1f, 1f, 1f, 1f, -580f, -42f, 540f, 56f), MutedTextColor);

            GameObject characterPanel = CreatePanel(canvas.transform, "CharacterCard", PanelColor, Anchored(0f, 1f, 0f, 1f, 36f, -205f, 380f, 230f));
            Text characterText = CreateText(characterPanel.transform, "CharacterText", "Character 1 L1 XP 0", 26, TextAnchor.UpperLeft, Rect(0f, 70f, 320f, 120f));

            GameObject questPanel = CreatePanel(canvas.transform, "QuestTracker", PanelColor, Anchored(0f, 0.5f, 0f, 0.5f, 36f, 20f, 420f, 260f));
            CreateText(questPanel.transform, "QuestText", "Quest Tracker\nCollect Copper Ore", 26, TextAnchor.UpperLeft, Rect(0f, 20f, 350f, 170f), MutedTextColor);

            GameObject minePanel = CreatePanel(canvas.transform, "MiningArea", SubPanelColor, Center(120f, -20f, 860f, 520f));
            CreateText(minePanel.transform, "PlayerPlaceholder", "Player", 34, TextAnchor.MiddleCenter, Rect(-250f, 40f, 220f, 100f));
            CreateText(minePanel.transform, "CopperNodePlaceholder", "Copper Node", 34, TextAnchor.MiddleCenter, Rect(250f, 40f, 260f, 100f));
            Text progressText = CreateText(minePanel.transform, "MiningProgressText", "Mining progress --", 28, TextAnchor.MiddleCenter, Rect(0f, -108f, 560f, 60f));
            Image miningProgressFill = CreateProgressBar(minePanel.transform, "MiningProgressBar", Rect(0f, -164f, 520f, 34f), new Color(0.72f, 0.48f, 0.22f, 1f));
            Button startButton = CreateButton(minePanel.transform, "StartCopperMiningButton", "Start Copper Mining", Rect(-145f, -230f, 290f, 66f));
            Button tickButton = CreateButton(minePanel.transform, "MineTickButton", "Mine Tick", Rect(165f, -230f, 220f, 66f));

            GameObject logPanel = CreatePanel(canvas.transform, "RecentRewards", PanelColor, Anchored(1f, 0.5f, 1f, 0.5f, -300f, -20f, 420f, 440f));
            Text rewardText = CreateText(logPanel.transform, "RewardText", "Recent mining rewards", 24, TextAnchor.UpperLeft, Rect(0f, 92f, 350f, 150f), MutedTextColor);
            Text statusText = CreateText(logPanel.transform, "StatusText", "Ready", 24, TextAnchor.UpperLeft, Rect(0f, -80f, 350f, 170f), MutedTextColor);

            AssignObject(view, "backToTownButton", backButton);
            AssignObject(view, "startMiningButton", startButton);
            AssignObject(view, "tickMiningButton", tickButton);
            AssignObject(view, "miningProgressFill", miningProgressFill);
            AssignObject(view, "characterText", characterText);
            AssignObject(view, "progressText", progressText);
            AssignObject(view, "rewardText", rewardText);
            AssignObject(view, "statusText", statusText);

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
            fill.fillAmount = 0f;
            return fill;
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
                cachedFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
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
