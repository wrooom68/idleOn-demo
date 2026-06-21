# Figma Frame Plan for Idle Guild Demo UI

Use this document to create the Figma file for the **Idle Guild Demo** UI and prepare clean frame links for Codex handoff.

The goal is not to create perfect final art. The goal is to give Codex and Unity a clear layout, hierarchy, component naming scheme, and screen flow.

---

## Do UI Assets Need to Be in Figma?

No. Figma does not need every UI asset.

Use Figma for:

```text
Screen layout
Panel hierarchy
Text placement
Button placement
HUD structure
Component naming
Frame-to-frame flow
Codex handoff reference
```

Only import a few selected assets if they help visual clarity:

```text
Panel background
Button sprite
Inventory slot sprite
HP / XP bar sprite
A few item icons
Character portrait
Slime portrait
```

Do not import:

```text
Entire asset packs
Raw ZIP files
Full tilesets unless needed for a mockup
Audio
Random Figma Community assets
Unapproved third-party assets
```

Important assignment rule:

```text
Any actual UI/game art used in the final Unity project must come from the approved assignment sources and be recorded in CREDITS.md.
```

Figma-only rectangles, text, and layout shapes are fine as mockup elements.

---

## Figma File Name

```text
Idle Guild Demo UI
```

Recommended page structure:

```text
00_StyleGuide
01_MainFlow
02_Panels
03_Components
04_Reference
```

---

## Canvas Size

Design all gameplay screens at:

```text
1920 x 1080
```

Unity target reference:

```text
16:9 landscape
Main testing sizes: 1920x1080 and 1280x720
```

Use simple responsive thinking:

```text
HUD corners stay anchored
Center gameplay area remains readable
Large modals are centered
Panels avoid tiny text
```

---

## Naming Rules for Codex Handoff

Name frames and layers clearly. Codex should not need to guess intent.

Use names like:

```text
Frame_01_MainMenu
Frame_02_TownHUD
Frame_03_CombatHUD
Panel_Inventory
Button_StartNewGame
Text_QuestObjective
Image_PlayerPortrait
ProgressBar_XP
Slot_Item_01
```

Avoid names like:

```text
Rectangle 42
Group 18
Frame copy copy
Button final v2
```

Layer naming convention:

```text
Type_Purpose_State
```

Examples:

```text
Button_Craft_Enabled
Button_Craft_Disabled
Text_MissingMaterials
Panel_AfkResults
ProgressBar_EnemyHP
Icon_CopperOre
```

---

## Required Frames

Create these frames in Figma.

```text
Frame_00_StyleGuide
Frame_01_MainMenu
Frame_02_TownHUD
Frame_03_CombatHUD
Frame_04_MiningHUD
Frame_05_InventoryCraftingPanel
Frame_06_CharacterClassTalentPanel
Frame_07_AfkResultsModal
Frame_08_QuestCompleteToast
Frame_09_ReviewerFlowMap
```

Do not add more frames unless needed to clarify an existing UI state.

---

## Frame_00_StyleGuide

Purpose:

```text
Reusable visual rules and component references for Codex/Unity UI implementation.
```

Required elements:

```text
Text styles:
- Title
- Header
- Body
- Small label
- Number / reward text

Color swatches:
- Panel background
- Button normal
- Button hover/pressed, optional
- Button disabled
- Positive reward text
- Missing material text
- XP bar fill
- HP bar fill
- Progress bar fill

Components:
- Button_Default
- Button_Disabled
- Panel_Default
- Slot_Inventory
- ProgressBar_XP
- ProgressBar_HP
- ProgressBar_Task
- Toast_Default
- Modal_Default
```

Hierarchy notes:

```text
Use this frame as the source of consistent spacing, fonts, and component naming.
This does not need final art; simple rectangles are acceptable.
```

Codex handoff note:

```text
Implement Unity UI with reusable prefabs/components matching these component names where practical.
```

---

## Frame_01_MainMenu

Purpose:

```text
Start or continue the demo and communicate that it is tuned for evaluation.
```

Required elements:

```text
Text_GameTitle: Idle Guild Demo
Text_Subtitle: Early-game idle RPG vertical slice
Button_NewGame
Button_Continue
Text_EvaluationNote: Designed to show all major systems in under 30 minutes.
Text_BuildVersionPlaceholder
```

Suggested layout:

```text
RootCanvas
  Background_MainMenu
  Panel_TitleBlock
    Text_GameTitle
    Text_Subtitle
  Panel_MenuButtons
    Button_NewGame
    Button_Continue
  Text_EvaluationNote
  Text_BuildVersionPlaceholder
```

Hierarchy notes:

```text
Buttons should be large and centered.
Continue can be disabled if no save exists.
Evaluation note should be visible without opening another panel.
```

Codex handoff note:

```text
Create MainMenuView with New Game and Continue button hooks. No gameplay logic inside the view.
```

---

## Frame_02_TownHUD

Purpose:

```text
Main hub screen where the reviewer understands the available activities and current quest.
```

Required elements:

```text
Panel_TopBar
  Text_Coins
  Button_SaveOrSaveIndicator
  Button_Inventory

Panel_CharacterCard
  Image_PlayerPortrait
  Text_CharacterName
  Text_Level
  ProgressBar_XP
  Text_CurrentTask
  Button_SwitchCharacter

Panel_QuestTracker
  Text_QuestTitle
  Text_QuestObjective
  ProgressBar_QuestProgress optional
  Button_ClaimReward if complete

WorldArea_Town
  Image_QuestNPC
  Image_CraftingStation
  Image_StorageChest
  Button_GoCombat
  Button_GoMining
  Button_ClassTalent
  Button_SimulateAfk

Panel_LootLog optional
```

Suggested layout:

```text
RootCanvas
  Panel_TopBar anchored top stretch
  Panel_CharacterCard anchored top-left
  Panel_QuestTracker anchored left-center
  WorldArea_Town centered
  Panel_ActivityButtons anchored bottom-center
  Panel_LootLog anchored right
```

Hierarchy notes:

```text
The reviewer should instantly see the active quest and the Combat/Mining actions.
Simulate AFK should be visible but clearly labeled as an evaluation helper.
Inventory and crafting should be reachable in one click.
```

Codex handoff note:

```text
Implement TownHUDView as a thin UI controller that reads character, quest, inventory, and task state from systems.
```

---

## Frame_03_CombatHUD

Purpose:

```text
Show the automatic combat loop clearly: enemy HP, damage, loot, XP, and quest progress.
```

Required elements:

```text
Panel_TopBar
  Button_BackToTown
  Text_Coins
  Button_Inventory

Panel_CharacterCard
  Image_PlayerPortrait
  Text_CharacterName
  Text_Level
  ProgressBar_XP
  Text_DamageStat

WorldArea_Combat
  Image_Player
  Image_Slime
  ProgressBar_EnemyHP
  Text_EnemyName: Slime
  FloatingText_DamageExample

Panel_QuestTracker
  Text_QuestTitle
  Text_QuestObjective

Panel_LootLog
  Row_Loot_XP
  Row_Loot_Coins
  Row_Loot_SlimeGoo
```

Suggested layout:

```text
RootCanvas
  Panel_TopBar top
  Panel_CharacterCard top-left
  Panel_QuestTracker left
  WorldArea_Combat center
  Panel_LootLog right
  Panel_BottomActions bottom-center
```

Hierarchy notes:

```text
The enemy HP bar must be readable.
Floating damage and loot log are high-impact polish.
The user should not need an Attack button; combat is automatic.
```

Codex handoff note:

```text
CombatHUDView should subscribe to combat events for damage, enemy killed, XP gained, and loot gained.
```

---

## Frame_04_MiningHUD

Purpose:

```text
Show copper mining as an automatic gathering activity with repeated progress.
```

Required elements:

```text
Panel_TopBar
  Button_BackToTown
  Text_Coins
  Button_Inventory

Panel_CharacterCard
  Image_PlayerPortrait
  Text_CharacterName
  Text_Level
  ProgressBar_XP
  Text_MiningStat

WorldArea_Mining
  Image_Player
  Image_CopperNode
  ProgressBar_MiningProgress
  Text_CurrentAction: Mining Copper Ore

Panel_QuestTracker
  Text_QuestTitle
  Text_QuestObjective

Panel_RecentRewards
  Row_CopperOre
  Row_MiningXP optional
```

Suggested layout:

```text
RootCanvas
  Panel_TopBar top
  Panel_CharacterCard top-left
  Panel_QuestTracker left
  WorldArea_Mining center
  Panel_RecentRewards right
```

Hierarchy notes:

```text
Mining must look automatic and repeatable.
The progress bar should be the main visual feedback.
```

Codex handoff note:

```text
MiningHUDView should render gathering progress and recent rewards. Gathering rules stay in GatheringSystem.
```

---

## Frame_05_InventoryCraftingPanel

Purpose:

```text
Show owned items, recipe requirements, craft availability, and missing materials.
```

Required elements:

```text
ModalOrPanel_InventoryCrafting
  Header
    Text_Title: Inventory & Crafting
    Button_Close

  Panel_Inventory
    Text_Header: Inventory
    Grid_InventorySlots
      Slot_CopperOre
      Slot_CopperBar
      Slot_SlimeGoo
      Slot_CopperSword
      Slot_CopperPickaxe
      Slot_Coins

  Panel_Crafting
    Recipe_CopperBar
      Text_RecipeName
      Text_Requirements: 3 Copper Ore
      Text_Output: 1 Copper Bar
      Button_Craft

    Recipe_CopperSword
      Text_RecipeName
      Text_Requirements: 2 Copper Bar + 3 Slime Goo
      Text_Output: 1 Copper Sword
      Button_Craft

    Recipe_CopperPickaxe
      Text_RecipeName
      Text_Requirements: 2 Copper Bar
      Text_Output: 1 Copper Pickaxe
      Button_Craft

  Text_MissingMaterialsExample
```

Suggested layout:

```text
RootPanel
  Header
  Left: Inventory grid
  Right: Recipe list
  Bottom: Item tooltip / missing material message
```

Hierarchy notes:

```text
Craft buttons should have enabled and disabled visual states.
Missing materials should be readable immediately.
Item quantities should be visible on slots.
```

Codex handoff note:

```text
InventoryCraftingPanel should render InventorySystem and CraftingSystem state. It should not calculate recipe validity itself except via system responses.
```

---

## Frame_06_CharacterClassTalentPanel

Purpose:

```text
Show character stats, class selection, and simple talent spending.
```

Required elements:

```text
ModalOrPanel_CharacterProgression
  Header
    Text_Title: Character
    Button_Close

  Panel_CharacterSummary
    Image_PlayerPortrait
    Text_Name
    Text_Level
    ProgressBar_XP
    Text_Class
    Text_UnspentTalentPoints

  Panel_Stats
    Text_Damage
    Text_MiningSpeed
    Text_XPGain
    Text_DropRate
    Text_AfkRate

  Panel_ClassChoice
    Button_Warrior: +Damage
    Button_Archer: +Drop Rate
    Button_Mage: +AFK Gain

  Panel_Talents
    TalentNode_Damage
    TalentNode_MiningSpeed
    TalentNode_XPGain
    TalentNode_AfkGain
```

Suggested layout:

```text
RootPanel
  Header
  Left: Character summary and stats
  Center: Class choice
  Right: Talent nodes
```

Hierarchy notes:

```text
Class buttons should appear locked/disabled before level 5.
After class choice, selected class should be clearly marked.
Talent nodes should show current rank and cost.
```

Codex handoff note:

```text
CharacterProgressionPanel should call ProgressionSystem/ClassSystem/TalentSystem methods. It should not own stat math.
```

---

## Frame_07_AfkResultsModal

Purpose:

```text
Make offline progress feel rewarding and easy to evaluate.
```

Required elements:

```text
Modal_AfkResults
  Text_Title: AFK Results
  Text_AwayDuration
  List_CharacterRewardSummaries
    CharacterReward_01
      Image_CharacterPortrait
      Text_CharacterName
      Text_Task: Fought Slimes / Mined Copper
      Text_XPGained
      Text_ItemsGained
      Text_CoinsGained
      Text_LevelsGained optional
    CharacterReward_02
      Same structure
  Button_ClaimContinue
```

Suggested layout:

```text
Centered modal over darkened background.
Each character gets one reward card.
Rewards use clear +number formatting.
```

Hierarchy notes:

```text
This is a key reviewer moment.
Make it readable, celebratory, and unambiguous.
```

Codex handoff note:

```text
AfkResultsModal should receive an AfkRewardSummary object and render it. Calculation stays in OfflineProgressionSystem.
```

---

## Frame_08_QuestCompleteToast

Purpose:

```text
Small feedback state for quest completion, level up, crafting, and item gain.
```

Required elements:

```text
Toast_QuestComplete
  Text_Title: Quest Complete!
  Text_Body: Return to town to claim reward.

Toast_LevelUp
  Text_Title: Level Up!
  Text_Body: +1 Talent Point

Toast_ItemCrafted
  Text_Title: Crafted!
  Text_Body: Copper Sword
```

Suggested layout:

```text
Small panel anchored upper-center or lower-right.
```

Hierarchy notes:

```text
Toasts should not block gameplay.
They should stack or replace cleanly.
```

Codex handoff note:

```text
Implement ToastView with a simple Show(message, type) method and optional queue.
```

---

## Frame_09_ReviewerFlowMap

Purpose:

```text
A simple visual map of the 30-minute reviewer journey.
```

Required elements:

```text
Step 1: Main Menu
Step 2: Town Quest
Step 3: Combat Slime
Step 4: Mining Copper
Step 5: Craft Bars / Sword / Pickaxe
Step 6: Reach Level 5
Step 7: Choose Class
Step 8: Spend Talents
Step 9: Unlock Character 2
Step 10: Simulate AFK
```

Suggested layout:

```text
Horizontal or vertical flow diagram.
```

Hierarchy notes:

```text
This frame is mainly for planning and interview explanation.
It may not become a Unity screen.
```

Codex handoff note:

```text
Do not implement this as in-game UI unless explicitly requested later.
```

---

## Components to Create in Figma

Create these components in `03_Components`:

```text
Component_Button_Default
Component_Button_Disabled
Component_Panel_Default
Component_Modal_Default
Component_InventorySlot
Component_ProgressBar
Component_CharacterCard
Component_QuestTracker
Component_LootLogRow
Component_RecipeRow
Component_TalentNode
Component_Toast
Component_AfkRewardCard
```

For each component, define at least:

```text
Normal state
Disabled state, if applicable
Selected state, if applicable
```

---

## Unity UI Implementation Notes

Preferred Unity UI stack:

```text
Canvas
Unity UI / uGUI
TextMeshPro text
Image components
Button components
Horizontal/Vertical Layout Groups where useful
```

Avoid overengineering:

```text
No custom UI framework
No complex animations first
No UI Toolkit unless the project already uses it confidently
```

Recommended prefab mapping:

```text
Figma Component_Button_Default → Unity UI Button prefab
Figma Component_Panel_Default → Unity Panel prefab
Figma Component_InventorySlot → InventorySlotView prefab
Figma Component_ProgressBar → ProgressBarView prefab
Figma Component_RecipeRow → RecipeRowView prefab
Figma Component_TalentNode → TalentNodeView prefab
Figma Component_AfkRewardCard → AfkRewardCardView prefab
```

---

## Codex Handoff Checklist

Before giving a Figma link to Codex:

```text
Frame has final name
Layers are named clearly
Buttons have meaningful names
Placeholder text is realistic
Disabled/enabled states are visible where needed
The frame is not cluttered with unused experimental elements
The frame uses only approved/imported art or simple placeholders
```

When handing off to Codex, use exact frame links and this prompt style:

```text
Use the Figma MCP server to inspect the exact frame linked below.
Implement the matching Unity uGUI layout as prefabs and thin UI controller scripts.
Do not add gameplay systems.
Do not change scope.md.
Use TextMeshPro for text.
Keep gameplay logic out of UI scripts.
Summarize required prefab references and manual Unity setup after changes.

Frame: <paste Figma frame link>
```

---

## Task 05 Definition of Done

Task 05 is complete when:

```text
Figma frame plan exists in docs/
Required frame names are listed
Required UI elements are listed per frame
Layer hierarchy notes are defined
Codex handoff notes are included
The plan clarifies that full UI assets are not required in Figma
The plan stays inside the locked 25-hour scope
```
