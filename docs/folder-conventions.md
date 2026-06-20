# Folder Conventions

This document defines the Unity project folder layout for **Idle Guild Demo**.

The goal is to keep the take-home easy to review, easy to navigate, and safe for Codex/Cursor-assisted changes.

---

## Root Repository Layout

```text
idleOn-demo/
  Assets/
  Packages/
  ProjectSettings/
  docs/
  prompts/
  scope.md
  README.md
  CREDITS.md
  .gitignore
```

Do not commit Unity-generated folders such as `Library/`, `Temp/`, `Obj/`, or local build outputs.

---

## Unity Assets Layout

```text
Assets/
  Art/
    Characters/
    Enemies/
    Environment/
    Items/
    UI/
  Data/
    Items/
    Enemies/
    Recipes/
    Quests/
    Classes/
    Talents/
    Zones/
  Prefabs/
    Characters/
    Enemies/
    Environment/
    Interactables/
    UI/
  Scenes/
    MainMenu.unity
    Town.unity
    CombatZone.unity
    MineZone.unity
  Scripts/
    Core/
    Data/
    Runtime/
    Systems/
    Save/
    UI/
  ThirdParty/
  Resources/
```

---

## Folder Responsibilities

### `Assets/Art/`

Project-ready art after import processing.

Use for sprites that are actively used by the Unity project.

```text
Characters: player / NPC visuals
Enemies: slime or enemy visuals
Environment: backgrounds, props, tilesets after import
Items: item icons
UI: panels, buttons, icons, cursors
```

### `Assets/ThirdParty/`

Raw or lightly organized approved third-party assets.

Rules:

```text
Only assignment-approved free 2D assets
One folder per asset pack
Every pack must be listed in CREDITS.md
No audio
No templates
No previous work
```

Suggested naming:

```text
Assets/ThirdParty/PackName_Creator/
```

### `Assets/Data/`

ScriptableObject content definitions.

Expected examples:

```text
ItemDefinition
EnemyDefinition
RecipeDefinition
QuestDefinition
ClassDefinition
TalentDefinition
ZoneDefinition
```

Do not put runtime save data here.

### `Assets/Prefabs/`

Reusable Unity prefab objects.

Expected examples:

```text
PlayerView
SlimeView
QuestNpc
CraftingStation
PortalButton
InventorySlotView
TalentNodeView
```

### `Assets/Scenes/`

Only committed playable/editor scenes.

Required scenes:

```text
MainMenu.unity
Town.unity
CombatZone.unity
MineZone.unity
```

Temporary test scenes should be named clearly and removed before submission.

### `Assets/Scripts/Core/`

Project bootstrap, service locator/composition root, game events, and shared constants.

Examples:

```text
GameBootstrap.cs
GameEvents.cs
ServiceRegistry.cs
GameConstants.cs
```

### `Assets/Scripts/Data/`

ScriptableObject definitions and static data models.

Examples:

```text
ItemDefinition.cs
EnemyDefinition.cs
RecipeDefinition.cs
QuestDefinition.cs
ClassDefinition.cs
TalentDefinition.cs
```

### `Assets/Scripts/Runtime/`

Serializable runtime state models.

Examples:

```text
SaveData.cs
PlayerProfile.cs
CharacterState.cs
InventoryState.cs
QuestProgressState.cs
TalentState.cs
```

Rules:

```text
No MonoBehaviour references
No scene object references
JSON-friendly data only
```

### `Assets/Scripts/Systems/`

Gameplay rules and domain logic.

Examples:

```text
InventorySystem.cs
ProgressionSystem.cs
CombatSystem.cs
GatheringSystem.cs
CraftingSystem.cs
QuestSystem.cs
OfflineProgressionSystem.cs
```

Rules:

```text
Systems own gameplay logic
Systems should be testable where possible
UI must call systems, not duplicate rules
```

### `Assets/Scripts/Save/`

Save/load implementation.

Examples:

```text
SaveSystem.cs
SaveFilePaths.cs
JsonSaveSerializer.cs
```

### `Assets/Scripts/UI/`

Thin UI controllers and views.

Examples:

```text
InventoryPanel.cs
CraftingPanel.cs
QuestTrackerView.cs
CharacterPanel.cs
TalentPanel.cs
AfkResultsModal.cs
ToastView.cs
```

Rules:

```text
UI scripts render state
UI scripts call public system methods
UI scripts do not own gameplay rules
```

### `docs/`

Project process and documentation files.

Examples:

```text
folder-conventions.md
first-commit-checklist.md
```

### `prompts/`

Saved Codex/Cursor task prompts.

Suggested naming:

```text
task_01_scope_lock.md
task_02_repo_hygiene.md
task_03_unity_project_setup.md
```

---

## Naming Conventions

### Scripts

```text
PascalCase.cs
```

Examples:

```text
InventorySystem.cs
QuestTrackerView.cs
OfflineProgressionSystem.cs
```

### ScriptableObjects

Use readable asset names with stable IDs inside the asset.

Examples:

```text
Item_CopperOre.asset
Enemy_Slime.asset
Recipe_CopperBar.asset
Quest_01_KillSlimes.asset
Class_Warrior.asset
Talent_Damage.asset
```

### Prefabs

```text
Category_Name.prefab
```

Examples:

```text
Enemy_Slime.prefab
UI_InventorySlot.prefab
Interactable_CraftingStation.prefab
```

### Scenes

```text
MainMenu.unity
Town.unity
CombatZone.unity
MineZone.unity
```

### Branches

For this take-home, prefer committing directly to `main` unless a branch is needed.

Optional branch naming:

```text
feature/inventory-system
feature/quest-flow
fix/save-load
```

---

## Commit Rules

Use small meaningful commits.

Good examples:

```text
Add core data definitions
Implement stackable inventory state
Add JSON save load system
Implement slime auto combat loop
Add mining and crafting recipes
Wire quest tracker UI
Tune demo progression for 30 minute evaluation
```

Avoid:

```text
update
fix
stuff
final
changes
```

---

## Scope Rule

If a feature is not listed in `scope.md`, do not add it.

Extra ideas go into a parking lot, not into the build.
