#if UNITY_EDITOR
using System.IO;
using IdleGuildDemo.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleGuild.Editor
{
  public static class IdleGuildUIBuilder
  {
    private const string PrefabFolder = "Assets/Prefabs/UI";
    [MenuItem("Idle Guild/Add Gameplay UI To Active Scene")]
    public static void AddGameplayUiToScene()
    {
      var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabFolder + "/UI_GameplayRoot.prefab");
      if (prefab == null)
      {
        BuildGameplayUiPrefab();
        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabFolder + "/UI_GameplayRoot.prefab");
      }

      if (prefab == null)
      {
        Debug.LogError("Gameplay UI prefab could not be created.");
        return;
      }

      PrefabUtility.InstantiatePrefab(prefab);
      Debug.Log("Gameplay UI added to active scene.");
    }

    [MenuItem("Idle Guild/Add Main Menu UI To Active Scene")]
    public static void AddMainMenuUiToScene()
    {
      var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabFolder + "/UI_MainMenuRoot.prefab");
      if (prefab == null)
      {
        BuildMainMenuUiPrefab();
        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabFolder + "/UI_MainMenuRoot.prefab");
      }

      if (prefab == null)
      {
        Debug.LogError("Main menu UI prefab could not be created.");
        return;
      }

      PrefabUtility.InstantiatePrefab(prefab);
      Debug.Log("Main menu UI added to active scene.");
    }

    [MenuItem("Idle Guild/Build UI Structure (All)")]
    public static void BuildAll()
    {
      EnsureFolder(PrefabFolder);
      EnsureEventSystem();
      BuildSlotPrefabs();
      BuildGameplayUiPrefab();
      BuildMainMenuUiPrefab();
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
      Debug.Log("Idle Guild UI structure built. Prefabs saved to " + PrefabFolder);
    }

    [MenuItem("Idle Guild/Build Gameplay UI Prefab")]
    public static void BuildGameplayUiPrefab()
    {
      EnsureFolder(PrefabFolder);
      EnsureEventSystem();
      if (AssetDatabase.LoadAssetAtPath<GameObject>(PrefabFolder + "/UI_InventorySlot.prefab") == null)
      {
        BuildSlotPrefabs();
      }

      var root = BuildGameplayHierarchy();
      SavePrefab(root, PrefabFolder + "/UI_GameplayRoot.prefab");
      Object.DestroyImmediate(root);
    }

    [MenuItem("Idle Guild/Build Main Menu UI Prefab")]
    public static void BuildMainMenuUiPrefab()
    {
      EnsureFolder(PrefabFolder);
      EnsureEventSystem();
      var root = BuildMainMenuHierarchy();
      SavePrefab(root, PrefabFolder + "/UI_MainMenuRoot.prefab");
      Object.DestroyImmediate(root);
    }

    [MenuItem("Idle Guild/Build UI Slot Prefabs")]
    public static void BuildSlotPrefabs()
    {
      EnsureFolder(PrefabFolder);

      var slot = BuildInventorySlotHierarchy();
      SavePrefab(slot, PrefabFolder + "/UI_InventorySlot.prefab");
      Object.DestroyImmediate(slot);

      var card = BuildCharacterCardHierarchy();
      SavePrefab(card, PrefabFolder + "/UI_CharacterCard.prefab");
      Object.DestroyImmediate(card);

      var recipe = BuildRecipeRowHierarchy();
      SavePrefab(recipe, PrefabFolder + "/UI_CraftingRecipeRow.prefab");
      Object.DestroyImmediate(recipe);

      var talent = BuildTalentNodeHierarchy();
      SavePrefab(talent, PrefabFolder + "/UI_TalentNode.prefab");
      Object.DestroyImmediate(talent);
    }

    private static GameObject BuildGameplayHierarchy()
    {
      var sprites = LoadSprites();
      var canvasGo = CreateCanvas("UI_GameplayRoot");
      var safeArea = CreateRect("SafeArea", canvasGo.transform);
      Stretch(safeArea);

      var hud = CreateRect("HUD", safeArea.transform);
      Stretch(hud);

      var topBar = CreateAnchoredPanel(hud.transform, "TopBar", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -72f), Vector2.zero);
      AddImage(topBar, sprites.Banner, sliced: true);
      var coinsText = CreateText(topBar.transform, "CoinsText", "Coins: 0", 22, TextAnchor.MiddleLeft, new Vector2(24f, 0f), new Vector2(300f, 40f), TextAnchor.MiddleLeft);
      var zoneText = CreateText(topBar.transform, "ZoneNameText", "Town", 22, TextAnchor.MiddleRight, new Vector2(-24f, 0f), new Vector2(300f, 40f), TextAnchor.MiddleRight);

      var hudView = hud.gameObject.AddComponent<HUDView>();
      SetField(hudView, "coinsText", coinsText);
      SetField(hudView, "zoneNameText", zoneText);

      var questTrackerGo = CreateAnchoredPanel(hud.transform, "QuestTracker", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -96f), new Vector2(420f, 140f));
      AddImage(questTrackerGo, sprites.Carved, sliced: true);
      var questTitle = CreateText(questTrackerGo.transform, "QuestTitle", "Quest", 20, TextAnchor.UpperLeft, new Vector2(16f, -12f), new Vector2(380f, 28f), TextAnchor.UpperLeft);
      var questObjective = CreateText(questTrackerGo.transform, "QuestObjective", "Kill 5 Slimes", 18, TextAnchor.UpperLeft, new Vector2(16f, -44f), new Vector2(380f, 48f), TextAnchor.UpperLeft);
      var (_, questProgressFill) = CreateProgressBar(questTrackerGo.transform, "QuestProgress", sprites.Bar, new Vector2(16f, -100f), new Vector2(300f, 24f));
      var progressText = CreateText(questTrackerGo.transform, "ProgressText", "0/5", 18, TextAnchor.MiddleRight, new Vector2(-16f, -100f), new Vector2(80f, 24f), TextAnchor.MiddleRight);

      var questTracker = questTrackerGo.gameObject.AddComponent<QuestTrackerView>();
      SetField(questTracker, "questTitleText", questTitle);
      SetField(questTracker, "questObjectiveText", questObjective);
      SetField(questTracker, "progressFill", questProgressFill);
      SetField(questTracker, "progressText", progressText);

      var lootLogGo = CreateAnchoredPanel(hud.transform, "LootLog", new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-24f, -96f), new Vector2(360f, 180f));
      AddImage(lootLogGo, sprites.Carved, sliced: true);
      CreateText(lootLogGo.transform, "Header", "Loot Log", 20, TextAnchor.UpperLeft, new Vector2(16f, -12f), new Vector2(320f, 28f), TextAnchor.UpperLeft);
      var lootText = CreateText(lootLogGo.transform, "LogText", string.Empty, 16, TextAnchor.UpperLeft, new Vector2(16f, -44f), new Vector2(320f, 120f), TextAnchor.UpperLeft);
      var lootLog = lootLogGo.gameObject.AddComponent<LootLogView>();
      SetField(lootLog, "logText", lootText);

      var activityHudGo = CreateRect("ActivityHUD", hud.transform);
      Stretch(activityHudGo);
      var activityHud = activityHudGo.gameObject.AddComponent<ActivityHUDView>();

      var miningGroup = CreateAnchoredPanel(activityHudGo.transform, "MiningGroup", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 120f), new Vector2(520f, 72f));
      var (_, miningProgressFill) = CreateProgressBar(miningGroup.transform, "MiningProgress", sprites.Bar, new Vector2(0f, -8f), new Vector2(480f, 28f));
      var miningLabel = CreateText(miningGroup.transform, "MiningLabel", "Mining Copper...", 18, TextAnchor.MiddleCenter, new Vector2(0f, 24f), new Vector2(480f, 28f), TextAnchor.MiddleCenter);

      var combatGroup = CreateAnchoredPanel(activityHudGo.transform, "CombatGroup", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 120f), new Vector2(520f, 72f));
      var (_, enemyHpFill) = CreateProgressBar(combatGroup.transform, "EnemyHp", sprites.Bar, new Vector2(0f, -8f), new Vector2(480f, 28f));
      var enemyName = CreateText(combatGroup.transform, "EnemyName", "Slime", 18, TextAnchor.MiddleCenter, new Vector2(0f, 24f), new Vector2(480f, 28f), TextAnchor.MiddleCenter);
      combatGroup.gameObject.SetActive(false);

      SetField(activityHud, "miningGroup", miningGroup.gameObject);
      SetField(activityHud, "miningProgressFill", miningProgressFill);
      SetField(activityHud, "miningLabelText", miningLabel);
      SetField(activityHud, "combatGroup", combatGroup.gameObject);
      SetField(activityHud, "enemyHpFill", enemyHpFill);
      SetField(activityHud, "enemyNameText", enemyName);

      var navBarGo = CreateAnchoredPanel(safeArea.transform, "NavigationBar", new Vector2(0f, 0f), new Vector2(1f, 0f), Vector2.zero, new Vector2(0f, 88f));
      AddImage(navBarGo, sprites.Banner, sliced: true);
      var inventoryBtn = CreateButton(navBarGo.transform, "BtnInventory", "Inventory", sprites.Button, new Vector2(-360f, 0f), new Vector2(160f, 56f));
      var craftingBtn = CreateButton(navBarGo.transform, "BtnCrafting", "Crafting", sprites.Button, new Vector2(-180f, 0f), new Vector2(160f, 56f));
      var charactersBtn = CreateButton(navBarGo.transform, "BtnCharacters", "Characters", sprites.Button, new Vector2(0f, 0f), new Vector2(160f, 56f));
      var talentsBtn = CreateButton(navBarGo.transform, "BtnTalents", "Talents", sprites.Button, new Vector2(180f, 0f), new Vector2(160f, 56f));
      var afkBtn = CreateButton(navBarGo.transform, "BtnSimulateAfk", "Simulate 2h AFK", sprites.Button, new Vector2(360f, 0f), new Vector2(200f, 56f));

      var navigationBar = navBarGo.gameObject.AddComponent<NavigationBarView>();
      SetField(navigationBar, "inventoryButton", inventoryBtn);
      SetField(navigationBar, "craftingButton", craftingBtn);
      SetField(navigationBar, "charactersButton", charactersBtn);
      SetField(navigationBar, "talentsButton", talentsBtn);
      SetField(navigationBar, "simulateAfkButton", afkBtn);

      var panelsRoot = CreateRect("Panels", safeArea.transform);
      Stretch(panelsRoot);

      var inventoryPanelGo = BuildInventoryPanel(panelsRoot.transform, sprites);
      var craftingPanelGo = BuildCraftingPanel(panelsRoot.transform, sprites);
      var characterPanelGo = BuildCharacterPanel(panelsRoot.transform, sprites);
      var talentPanelGo = BuildTalentPanel(panelsRoot.transform, sprites);
      var classChoicePanelGo = BuildClassChoicePanel(panelsRoot.transform, sprites);

      var modalsRoot = CreateRect("Modals", safeArea.transform);
      Stretch(modalsRoot);
      var afkModalGo = BuildAfkModal(modalsRoot.transform, sprites);

      var toastGo = CreateAnchoredPanel(safeArea.transform, "Toast", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 112f), new Vector2(640f, 48f));
      AddImage(toastGo, sprites.Carved, sliced: true);
      var toastText = CreateText(toastGo.transform, "ToastText", string.Empty, 20, TextAnchor.MiddleCenter, Vector2.zero, new Vector2(600f, 40f), TextAnchor.MiddleCenter);
      var toastCanvasGroup = toastGo.gameObject.AddComponent<CanvasGroup>();
      toastCanvasGroup.alpha = 0f;
      var toastView = toastGo.gameObject.AddComponent<ToastView>();
      SetField(toastView, "toastText", toastText);
      SetField(toastView, "canvasGroup", toastCanvasGroup);

      var uiRoot = canvasGo.gameObject.AddComponent<UIRootController>();
      SetField(uiRoot, "inventoryPanel", inventoryPanelGo.GetComponent<InventoryPanel>());
      SetField(uiRoot, "craftingPanel", craftingPanelGo.GetComponent<CraftingPanel>());
      SetField(uiRoot, "characterPanel", characterPanelGo.GetComponent<CharacterPanel>());
      SetField(uiRoot, "talentPanel", talentPanelGo.GetComponent<TalentPanel>());
      SetField(uiRoot, "classChoicePanel", classChoicePanelGo.GetComponent<ClassChoicePanel>());
      SetField(uiRoot, "afkResultsModal", afkModalGo.GetComponent<AfkResultsModal>());

      var wiring = canvasGo.gameObject.AddComponent<UIWiring>();
      SetField(wiring, "root", uiRoot);
      SetField(wiring, "navigationBar", navigationBar);
      SetField(wiring, "afkResultsModal", afkModalGo.GetComponent<AfkResultsModal>());

      return canvasGo.gameObject;
    }

    private static GameObject BuildMainMenuHierarchy()
    {
      var sprites = LoadSprites();
      var canvasGo = CreateCanvas("UI_MainMenuRoot");
      var panel = CreateAnchoredPanel(canvasGo.transform, "MainMenuPanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(720f, 520f));
      AddImage(panel, sprites.Carved, sliced: true);

      CreateText(panel.transform, "Title", "Idle Guild Demo", 42, TextAnchor.UpperCenter, new Vector2(0f, -32f), new Vector2(640f, 56f), TextAnchor.UpperCenter);
      var newGameBtn = CreateButton(panel.transform, "NewGameButton", "New Game", sprites.Button, new Vector2(0f, 40f), new Vector2(320f, 64f));
      var continueBtn = CreateButton(panel.transform, "ContinueButton", "Continue", sprites.Button, new Vector2(0f, -40f), new Vector2(320f, 64f));
      var note = CreateText(panel.transform, "EvaluationNote", "Designed to show all major systems in under 30 minutes", 16, TextAnchor.LowerCenter, new Vector2(0f, 24f), new Vector2(640f, 64f), TextAnchor.LowerCenter);

      var view = panel.gameObject.AddComponent<MainMenuView>();
      SetField(view, "newGameButton", newGameBtn);
      SetField(view, "continueButton", continueBtn);
      SetField(view, "evaluationNoteText", note);

      return canvasGo.gameObject;
    }

    private static GameObject BuildInventoryPanel(Transform parent, UiSprites sprites)
    {
      var panelGo = BuildModalPanel(parent, "InventoryPanel", "Inventory", sprites, 900f, 560f);
      var content = panelGo.transform.Find("Content");
      var slotContainer = CreateRect("SlotContainer", content);
      Stretch(slotContainer, new Vector2(16f, 16f), new Vector2(-16f, -80f));
      var grid = slotContainer.gameObject.AddComponent<GridLayoutGroup>();
      grid.cellSize = new Vector2(72f, 72f);
      grid.spacing = new Vector2(8f, 8f);
      grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
      grid.constraintCount = 8;

      var detail = CreateText(content, "DetailText", "Select an item", 18, TextAnchor.LowerLeft, new Vector2(16f, 16f), new Vector2(840f, 48f), TextAnchor.LowerLeft);
      var slotPrefab = AssetDatabase.LoadAssetAtPath<InventorySlotView>(PrefabFolder + "/UI_InventorySlot.prefab");

      var panel = panelGo.AddComponent<InventoryPanel>();
      var canvasGroup = panelGo.GetComponent<CanvasGroup>();
      SetField(panel, "canvasGroup", canvasGroup);
      SetField(panel, "slotContainer", slotContainer);
      SetField(panel, "slotPrefab", slotPrefab);
      SetField(panel, "detailText", detail);
      panel.Hide();
      return panelGo;
    }

    private static GameObject BuildCraftingPanel(Transform parent, UiSprites sprites)
    {
      var panelGo = BuildModalPanel(parent, "CraftingPanel", "Crafting", sprites, 900f, 560f);
      var content = panelGo.transform.Find("Content");
      var stationLabel = CreateText(content, "StationLabel", "Crafting Station", 20, TextAnchor.UpperLeft, new Vector2(16f, -8f), new Vector2(840f, 32f), TextAnchor.UpperLeft);
      var recipeContainer = CreateRect("RecipeContainer", content);
      Stretch(recipeContainer, new Vector2(16f, 16f), new Vector2(-16f, -56f));
      var layout = recipeContainer.gameObject.AddComponent<VerticalLayoutGroup>();
      layout.spacing = 8f;
      layout.childControlHeight = true;
      layout.childForceExpandHeight = false;
      layout.childControlWidth = true;
      layout.childForceExpandWidth = true;

      var recipePrefab = AssetDatabase.LoadAssetAtPath<CraftingRecipeRowView>(PrefabFolder + "/UI_CraftingRecipeRow.prefab");
      var panel = panelGo.AddComponent<CraftingPanel>();
      SetField(panel, "canvasGroup", panelGo.GetComponent<CanvasGroup>());
      SetField(panel, "recipeContainer", recipeContainer);
      SetField(panel, "recipeRowPrefab", recipePrefab);
      SetField(panel, "stationLabelText", stationLabel);
      panel.Hide();
      return panelGo;
    }

    private static GameObject BuildCharacterPanel(Transform parent, UiSprites sprites)
    {
      var panelGo = BuildModalPanel(parent, "CharacterPanel", "Characters", sprites, 900f, 560f);
      var content = panelGo.transform.Find("Content");
      var summary = CreateText(content, "RosterSummary", "Roster", 20, TextAnchor.UpperLeft, new Vector2(16f, -8f), new Vector2(840f, 32f), TextAnchor.UpperLeft);
      var cardContainer = CreateRect("CardContainer", content);
      Stretch(cardContainer, new Vector2(16f, 16f), new Vector2(-16f, -56f));
      var layout = cardContainer.gameObject.AddComponent<HorizontalLayoutGroup>();
      layout.spacing = 16f;
      layout.childControlWidth = true;
      layout.childForceExpandWidth = true;
      layout.childControlHeight = true;
      layout.childForceExpandHeight = true;

      var cardPrefab = AssetDatabase.LoadAssetAtPath<CharacterCardView>(PrefabFolder + "/UI_CharacterCard.prefab");
      var panel = panelGo.AddComponent<CharacterPanel>();
      SetField(panel, "canvasGroup", panelGo.GetComponent<CanvasGroup>());
      SetField(panel, "cardContainer", cardContainer);
      SetField(panel, "cardPrefab", cardPrefab);
      SetField(panel, "rosterSummaryText", summary);
      panel.Hide();
      return panelGo;
    }

    private static GameObject BuildTalentPanel(Transform parent, UiSprites sprites)
    {
      var panelGo = BuildModalPanel(parent, "TalentPanel", "Talents", sprites, 900f, 560f);
      var content = panelGo.transform.Find("Content");
      var points = CreateText(content, "AvailablePoints", "Talent Points: 0", 20, TextAnchor.UpperLeft, new Vector2(16f, -8f), new Vector2(840f, 32f), TextAnchor.UpperLeft);
      var talentContainer = CreateRect("TalentContainer", content);
      Stretch(talentContainer, new Vector2(16f, 16f), new Vector2(-16f, -56f));
      var layout = talentContainer.gameObject.AddComponent<VerticalLayoutGroup>();
      layout.spacing = 8f;
      layout.childControlHeight = true;
      layout.childForceExpandHeight = false;
      layout.childControlWidth = true;
      layout.childForceExpandWidth = true;

      var nodePrefab = AssetDatabase.LoadAssetAtPath<TalentNodeView>(PrefabFolder + "/UI_TalentNode.prefab");
      var panel = panelGo.AddComponent<TalentPanel>();
      SetField(panel, "canvasGroup", panelGo.GetComponent<CanvasGroup>());
      SetField(panel, "talentContainer", talentContainer);
      SetField(panel, "talentNodePrefab", nodePrefab);
      SetField(panel, "availablePointsText", points);
      panel.Hide();
      return panelGo;
    }

    private static GameObject BuildClassChoicePanel(Transform parent, UiSprites sprites)
    {
      var panelGo = BuildModalPanel(parent, "ClassChoicePanel", "Choose Your Class", sprites, 760f, 480f);
      var content = panelGo.transform.Find("Content");
      var prompt = CreateText(content, "Prompt", "Reach level 5 to specialize.", 18, TextAnchor.UpperCenter, new Vector2(0f, -56f), new Vector2(680f, 48f), TextAnchor.UpperCenter);
      var warrior = CreateButton(content, "WarriorButton", "Warrior (+Damage)", sprites.Button, new Vector2(0f, 40f), new Vector2(420f, 56f));
      var archer = CreateButton(content, "ArcherButton", "Archer (+Drop Rate)", sprites.Button, new Vector2(0f, -24f), new Vector2(420f, 56f));
      var mage = CreateButton(content, "MageButton", "Mage (+AFK Gain)", sprites.Button, new Vector2(0f, -92f), new Vector2(420f, 56f));

      var panel = panelGo.AddComponent<ClassChoicePanel>();
      SetField(panel, "canvasGroup", panelGo.GetComponent<CanvasGroup>());
      SetField(panel, "warriorButton", warrior);
      SetField(panel, "archerButton", archer);
      SetField(panel, "mageButton", mage);
      SetField(panel, "promptText", prompt);
      panel.Hide();
      return panelGo;
    }

    private static GameObject BuildAfkModal(Transform parent, UiSprites sprites)
    {
      var root = CreateRect("AfkResultsModal", parent);
      Stretch(root);
      var dim = CreateRect("Dim", root);
      Stretch(dim);
      var dimImage = dim.gameObject.AddComponent<Image>();
      dimImage.color = new Color(0f, 0f, 0f, 0.55f);

      var panel = CreateAnchoredPanel(root, "Panel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(760f, 480f));
      AddImage(panel, sprites.Carved, sliced: true);
      CreateText(panel.transform, "Title", "AFK Results", 28, TextAnchor.UpperCenter, new Vector2(0f, -24f), new Vector2(680f, 40f), TextAnchor.UpperCenter);
      var results = CreateText(panel.transform, "ResultsText", "No offline gains yet.", 18, TextAnchor.UpperLeft, new Vector2(0f, -24f), new Vector2(680f, 300f), TextAnchor.UpperLeft);
      var close = CreateButton(panel.transform, "CloseButton", "Close", sprites.Button, new Vector2(0f, 24f), new Vector2(200f, 56f));
      var simulate = CreateButton(panel.transform, "SimulateButton", "Simulate 2 Hours AFK", sprites.Button, new Vector2(0f, -320f), new Vector2(280f, 56f));

      var modal = root.gameObject.AddComponent<AfkResultsModal>();
      SetField(modal, "root", panel.gameObject);
      SetField(modal, "resultsText", results);
      SetField(modal, "closeButton", close);
      SetField(modal, "simulateTwoHoursButton", simulate);
      panel.gameObject.SetActive(false);
      return root.gameObject;
    }

    private static GameObject BuildInventorySlotHierarchy()
    {
      var sprites = LoadSprites();
      var root = CreateRect("UI_InventorySlot");
      Stretch(root, Vector2.zero, Vector2.zero);
      root.sizeDelta = new Vector2(72f, 72f);
      var bg = AddImage(root.gameObject, sprites.Carved, sliced: true);
      var iconGo = CreateRect("Icon", root);
      Stretch(iconGo, new Vector2(8f, 8f), new Vector2(-8f, -8f));
      var icon = iconGo.gameObject.AddComponent<Image>();
      icon.enabled = false;
      var qty = CreateText(root, "Quantity", string.Empty, 16, TextAnchor.LowerRight, new Vector2(-4f, 4f), new Vector2(40f, 20f), TextAnchor.LowerRight);

      var view = root.gameObject.AddComponent<InventorySlotView>();
      SetField(view, "iconImage", icon);
      SetField(view, "quantityText", qty);
      SetField(view, "backgroundImage", bg);
      return root.gameObject;
    }

    private static GameObject BuildCharacterCardHierarchy()
    {
      var sprites = LoadSprites();
      var root = CreateRect("UI_CharacterCard");
      Stretch(root, Vector2.zero, Vector2.zero);
      root.sizeDelta = new Vector2(360f, 420f);
      AddImage(root.gameObject, sprites.Carved, sliced: true);

      var portraitGo = CreateAnchoredPanel(root, "Portrait", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -72f), new Vector2(96f, 96f));
      var portrait = portraitGo.gameObject.AddComponent<Image>();
      portrait.color = new Color(0.8f, 0.8f, 0.8f, 1f);

      var name = CreateText(root, "NameText", "Hero", 22, TextAnchor.UpperCenter, new Vector2(0f, -132f), new Vector2(320f, 32f), TextAnchor.UpperCenter);
      var level = CreateText(root, "LevelText", "Lv 1", 18, TextAnchor.UpperLeft, new Vector2(16f, -168f), new Vector2(120f, 28f), TextAnchor.UpperLeft);
      var xp = CreateText(root, "XpText", "0/100 XP", 16, TextAnchor.UpperRight, new Vector2(-16f, -168f), new Vector2(180f, 28f), TextAnchor.UpperRight);
      var (_, xpFill) = CreateProgressBar(root, "XpBar", sprites.Bar, new Vector2(0f, -204f), new Vector2(320f, 20f));
      var task = CreateText(root, "TaskText", "Task: Idle", 16, TextAnchor.UpperLeft, new Vector2(16f, -236f), new Vector2(320f, 48f), TextAnchor.UpperLeft);
      var classText = CreateText(root, "ClassText", "Class: None", 16, TextAnchor.UpperLeft, new Vector2(16f, -292f), new Vector2(320f, 28f), TextAnchor.UpperLeft);
      var talents = CreateText(root, "TalentPointsText", "Talents: 0", 16, TextAnchor.UpperLeft, new Vector2(16f, -332f), new Vector2(320f, 28f), TextAnchor.UpperLeft);

      var view = root.gameObject.AddComponent<CharacterCardView>();
      SetField(view, "portraitImage", portrait);
      SetField(view, "nameText", name);
      SetField(view, "levelText", level);
      SetField(view, "xpText", xp);
      SetField(view, "xpFill", xpFill);
      SetField(view, "taskText", task);
      SetField(view, "classText", classText);
      SetField(view, "talentPointsText", talents);
      return root.gameObject;
    }

    private static GameObject BuildRecipeRowHierarchy()
    {
      var sprites = LoadSprites();
      var root = CreateRect("UI_CraftingRecipeRow");
      Stretch(root, Vector2.zero, Vector2.zero);
      root.sizeDelta = new Vector2(0f, 88f);
      AddImage(root.gameObject, sprites.Banner, sliced: true);

      var iconGo = CreateAnchoredPanel(root, "ResultIcon", new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(48f, 0f), new Vector2(56f, 56f));
      var icon = iconGo.gameObject.AddComponent<Image>();
      var name = CreateText(root, "RecipeName", "Recipe", 18, TextAnchor.UpperLeft, new Vector2(96f, -12f), new Vector2(520f, 28f), TextAnchor.UpperLeft);
      var mats = CreateText(root, "MaterialsText", "Materials", 16, TextAnchor.UpperLeft, new Vector2(96f, -44f), new Vector2(520f, 28f), TextAnchor.UpperLeft);
      var craft = CreateButton(root, "CraftButton", "Craft", sprites.Button, new Vector2(-72f, 0f), new Vector2(120f, 48f));

      var view = root.gameObject.AddComponent<CraftingRecipeRowView>();
      SetField(view, "recipeNameText", name);
      SetField(view, "materialsText", mats);
      SetField(view, "craftButton", craft);
      SetField(view, "resultIcon", icon);
      return root.gameObject;
    }

    private static GameObject BuildTalentNodeHierarchy()
    {
      var sprites = LoadSprites();
      var root = CreateRect("UI_TalentNode");
      Stretch(root, Vector2.zero, Vector2.zero);
      root.sizeDelta = new Vector2(0f, 72f);
      AddImage(root.gameObject, sprites.Banner, sliced: true);

      var name = CreateText(root, "TalentName", "Talent", 18, TextAnchor.UpperLeft, new Vector2(16f, -8f), new Vector2(420f, 28f), TextAnchor.UpperLeft);
      var desc = CreateText(root, "Description", "Effect description", 16, TextAnchor.UpperLeft, new Vector2(16f, -36f), new Vector2(420f, 28f), TextAnchor.UpperLeft);
      var rank = CreateText(root, "RankText", "0/5", 16, TextAnchor.MiddleRight, new Vector2(-140f, 0f), new Vector2(80f, 28f), TextAnchor.MiddleRight);
      var upgrade = CreateButton(root, "UpgradeButton", "Upgrade", sprites.Button, new Vector2(-56f, 0f), new Vector2(120f, 48f));

      var view = root.gameObject.AddComponent<TalentNodeView>();
      SetField(view, "talentNameText", name);
      SetField(view, "descriptionText", desc);
      SetField(view, "rankText", rank);
      SetField(view, "upgradeButton", upgrade);
      return root.gameObject;
    }

    private static GameObject BuildModalPanel(Transform parent, string name, string title, UiSprites sprites, float width, float height)
    {
      var panelGo = CreateAnchoredPanel(parent, name, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(width, height));
      AddImage(panelGo, sprites.Carved, sliced: true);
      panelGo.gameObject.AddComponent<CanvasGroup>();
      CreateText(panelGo.transform, "Title", title, 28, TextAnchor.UpperCenter, new Vector2(0f, -16f), new Vector2(width - 40f, 40f), TextAnchor.UpperCenter);
      var content = CreateRect("Content", panelGo.transform);
      Stretch(content, new Vector2(0f, 0f), Vector2.zero);
      content.offsetMax = new Vector2(0f, -56f);
      return panelGo.gameObject;
    }

    private static Canvas CreateCanvas(string name)
    {
      var go = new GameObject(name);
      var canvas = go.AddComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      var scaler = go.AddComponent<CanvasScaler>();
      scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
      scaler.referenceResolution = new Vector2(1920f, 1080f);
      scaler.matchWidthOrHeight = 0.5f;
      go.AddComponent<GraphicRaycaster>();
      return canvas;
    }

    private static void EnsureEventSystem()
    {
      if (Object.FindFirstObjectByType<EventSystem>() != null)
      {
        return;
      }

      var eventSystem = new GameObject("EventSystem");
      eventSystem.AddComponent<EventSystem>();
      eventSystem.AddComponent<StandaloneInputModule>();
    }

    private static RectTransform CreateRect(string name, Transform parent = null)
    {
      var go = new GameObject(name, typeof(RectTransform));
      if (parent != null)
      {
        go.transform.SetParent(parent, false);
      }

      return go.GetComponent<RectTransform>();
    }

    private static RectTransform CreateAnchoredPanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
      var rect = CreateRect(name, parent);
      rect.anchorMin = anchorMin;
      rect.anchorMax = anchorMax;
      rect.pivot = new Vector2(0.5f, 0.5f);
      rect.anchoredPosition = anchoredPosition;
      rect.sizeDelta = sizeDelta;
      return rect;
    }

    private static void Stretch(RectTransform rect)
    {
      Stretch(rect, Vector2.zero, Vector2.zero);
    }

    private static void Stretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
    {
      rect.anchorMin = Vector2.zero;
      rect.anchorMax = Vector2.one;
      rect.offsetMin = offsetMin;
      rect.offsetMax = offsetMax;
      rect.pivot = new Vector2(0.5f, 0.5f);
    }

    private static Image AddImage(GameObject go, Sprite sprite, bool sliced)
    {
      var image = go.GetComponent<Image>();
      if (image == null)
      {
        image = go.AddComponent<Image>();
      }

      image.sprite = sprite;
      image.type = sliced ? Image.Type.Sliced : Image.Type.Simple;
      image.color = Color.white;
      return image;
    }

    private static Image AddImage(Component component, Sprite sprite, bool sliced)
    {
      return AddImage(component.gameObject, sprite, sliced);
    }

    private static Text CreateText(Transform parent, string name, string content, int fontSize, TextAnchor alignment, Vector2 anchoredPosition, Vector2 sizeDelta, TextAnchor pivotAlignment)
    {
      var rect = CreateRect(name, parent);
      rect.anchorMin = new Vector2(0.5f, 0.5f);
      rect.anchorMax = new Vector2(0.5f, 0.5f);
      rect.pivot = PivotFromTextAnchor(pivotAlignment);
      rect.anchoredPosition = anchoredPosition;
      rect.sizeDelta = sizeDelta;

      var text = rect.gameObject.AddComponent<Text>();
      text.text = content;
      text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
      text.fontSize = fontSize;
      text.alignment = alignment;
      text.color = Color.white;
      text.supportRichText = false;
      return text;
    }

    private static Button CreateButton(Transform parent, string name, string label, Sprite sprite, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
      var rect = CreateRect(name, parent);
      rect.anchorMin = new Vector2(0.5f, 0.5f);
      rect.anchorMax = new Vector2(0.5f, 0.5f);
      rect.pivot = new Vector2(0.5f, 0.5f);
      rect.anchoredPosition = anchoredPosition;
      rect.sizeDelta = sizeDelta;

      var image = rect.gameObject.AddComponent<Image>();
      image.sprite = sprite;
      image.type = Image.Type.Sliced;
      var button = rect.gameObject.AddComponent<Button>();
      button.targetGraphic = image;

      CreateText(rect.transform, "Label", label, 18, TextAnchor.MiddleCenter, Vector2.zero, sizeDelta, TextAnchor.MiddleCenter);
      return button;
    }

    private static (Image background, Image fill) CreateProgressBar(Transform parent, string name, Sprite barSprite, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
      var rect = CreateRect(name, parent);
      rect.anchorMin = new Vector2(0.5f, 0.5f);
      rect.anchorMax = new Vector2(0.5f, 0.5f);
      rect.pivot = new Vector2(0.5f, 0.5f);
      rect.anchoredPosition = anchoredPosition;
      rect.sizeDelta = sizeDelta;

      var background = rect.gameObject.AddComponent<Image>();
      background.sprite = barSprite;
      background.type = Image.Type.Sliced;
      background.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);

      var fillRect = CreateRect("Fill", rect);
      Stretch(fillRect);
      var fill = fillRect.gameObject.AddComponent<Image>();
      fill.sprite = barSprite;
      fill.type = Image.Type.Sliced;
      fill.color = new Color(0.35f, 0.75f, 0.35f, 1f);
      fill.fillMethod = Image.FillMethod.Horizontal;
      fill.fillOrigin = (int)Image.OriginHorizontal.Left;
      fill.fillAmount = 0.5f;
      return (background, fill);
    }

    private static Vector2 PivotFromTextAnchor(TextAnchor anchor)
    {
      switch (anchor)
      {
        case TextAnchor.UpperLeft: return new Vector2(0f, 1f);
        case TextAnchor.UpperCenter: return new Vector2(0.5f, 1f);
        case TextAnchor.UpperRight: return new Vector2(1f, 1f);
        case TextAnchor.MiddleLeft: return new Vector2(0f, 0.5f);
        case TextAnchor.MiddleCenter: return new Vector2(0.5f, 0.5f);
        case TextAnchor.MiddleRight: return new Vector2(1f, 0.5f);
        case TextAnchor.LowerLeft: return new Vector2(0f, 0f);
        case TextAnchor.LowerCenter: return new Vector2(0.5f, 0f);
        case TextAnchor.LowerRight: return new Vector2(1f, 0f);
        default: return new Vector2(0.5f, 0.5f);
      }
    }

    private static UiSprites LoadSprites()
    {
      return new UiSprites
      {
        Button = LoadSprite("Assets/Art/UI/UI/Buttons/Button_Blue.png"),
        Banner = LoadSprite("Assets/Art/UI/UI/Banners/Banner_Horizontal.png"),
        Carved = LoadSprite("Assets/Art/UI/UI/Banners/Carved_Regular.png"),
        Bar = LoadSprite("Assets/Art/UI/BigBar_Base.png"),
        Icon = LoadSprite("Assets/Art/Items/Icons/Icon_01.png")
      };
    }

    private static Sprite LoadSprite(string path)
    {
      return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static void SavePrefab(GameObject root, string path)
    {
      var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
      if (existing != null)
      {
        PrefabUtility.SaveAsPrefabAssetAndConnect(root, path, InteractionMode.AutomatedAction);
      }
      else
      {
        PrefabUtility.SaveAsPrefabAsset(root, path);
      }
    }

    private static void EnsureFolder(string path)
    {
      if (!AssetDatabase.IsValidFolder(path))
      {
        Directory.CreateDirectory(path);
        AssetDatabase.Refresh();
      }
    }

    private static void SetField(Object target, string fieldName, Object value)
    {
      var serializedObject = new SerializedObject(target);
      var property = serializedObject.FindProperty(fieldName);
      if (property == null)
      {
        Debug.LogWarning($"Field '{fieldName}' not found on {target.name}");
        return;
      }

      property.objectReferenceValue = value;
      serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    private struct UiSprites
    {
      public Sprite Button;
      public Sprite Banner;
      public Sprite Carved;
      public Sprite Bar;
      public Sprite Icon;
    }
  }
}
#endif
