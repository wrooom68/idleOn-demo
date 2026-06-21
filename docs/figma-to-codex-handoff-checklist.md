# Figma-to-Codex Handoff Checklist

Use this checklist before asking Codex to inspect Figma frames with Figma MCP and implement Unity UI for **Idle Guild Demo**.

This handoff is for Unity UI only. It must stay inside the locked scope in `scope.md` and must not add gameplay systems, Unity scenes, C# scripts, imported assets, or Figma file edits during this documentation task.

---

## Required Figma Frame Links

Paste exact frame links here before implementation work begins.

```text
Frame_01_MainMenu:
Frame_02_TownHUD:
Frame_03_CombatHUD:
Frame_04_MiningHUD:
Frame_05_InventoryCraftingPanel:
Frame_06_CharacterClassTalentPanel:
Frame_07_AfkResultsModal:
Frame_08_QuestCompleteToast:
```

Reference-only frame:

```text
Frame_09_ReviewerFlowMap: Reference only. Do not implement this as a Unity UI target unless explicitly requested later.
```

---

## Layer Naming Rules

Figma frames and layers must use clear, implementation-friendly names.

Required prefixes:

- Frames start with `Frame_`
- Panels start with `Panel_`
- Buttons start with `Button_`
- Text layers start with `Text_`
- Icons/images start with `Icon_` or `Image_`
- Progress bars start with `ProgressBar_`
- Inventory slots start with `Slot_`
- World areas start with `WorldArea_`
- Reusable components start with `Component_`

Use names that describe purpose and state:

```text
Button_Craft_Enabled
Button_Craft_Disabled
Text_QuestObjective
Panel_AfkResults
ProgressBar_EnemyHP_Background
ProgressBar_EnemyHP_Fill
ProgressBar_EnemyHP_Label
Slot_CopperOre_Filled
```

Avoid unclear or temporary names:

```text
Rectangle 42
Group 7
copy
final
test
temp
```

---

## Required Layer Checks Before Handoff

Before sharing frame links with Codex, confirm:

- [ ] Every button has a clear name.
- [ ] Every text layer has realistic placeholder text.
- [ ] Every panel has a clear purpose.
- [ ] Every progress bar has background, fill, and text/label naming.
- [ ] Inventory slots include an icon layer and quantity text where filled.
- [ ] Craft buttons include enabled and disabled examples.
- [ ] Class buttons show locked, available, and selected states where needed.
- [ ] AFK modal includes reward cards for two characters.
- [ ] No hidden experimental layers remain.
- [ ] No unapproved third-party art is used.

---

## Component States

Reusable Figma components should expose the states Codex needs to map into Unity prefabs/views.

```text
Button:
- default
- disabled
- selected/active, optional

Craft button:
- enabled
- disabled

Inventory slot:
- empty
- filled

Progress bar:
- background
- fill
- label

Talent node:
- locked
- available
- purchased

Class choice:
- locked
- available
- selected

Toast:
- quest complete
- level up
- item crafted

AFK reward card:
- at least two character examples
```

---

## Unity UI Implementation Notes

Implementation should follow the existing Unity project conventions:

- Use Unity uGUI Canvas.
- Use TextMeshPro for text.
- Use Unity UI `Image` and `Button` components.
- Use prefabs for reusable UI pieces.
- Prefer `VerticalLayoutGroup` and `HorizontalLayoutGroup` where useful.
- Keep gameplay logic out of UI scripts.
- UI scripts should only render state and call public system methods.
- Do not add gameplay systems while implementing Figma UI.
- Do not change `scope.md`.
- Use placeholder sprites if final sprites are not ready.
- Keep panels readable at 1920x1080 and 1280x720.

Implementation boundaries:

- Do not modify Unity scenes unless a later implementation task explicitly asks for scene wiring.
- Do not add gameplay features.
- Do not import unapproved assets.
- Do not use Figma-only art in Unity unless the source is approved and credited.

---

## Recommended Unity Prefab Mapping

Use these mappings when converting Figma components into Unity prefabs:

```text
Component_Button_Default -> UI Button prefab
Component_Panel_Default -> Panel prefab
Component_ProgressBar -> ProgressBarView prefab
Component_InventorySlot -> InventorySlotView prefab
Component_RecipeRow -> RecipeRowView prefab
Component_TalentNode -> TalentNodeView prefab
Component_Toast -> ToastView prefab
Component_AfkRewardCard -> AfkRewardCardView prefab
```

Recommended prefab destination:

```text
Assets/Prefabs/UI/
```

Recommended UI script destination for later implementation tasks:

```text
Assets/Scripts/UI/
```

---

## Per-Frame Codex Handoff Prompt Template

Use this prompt for each implementation pass:

```text
Use Figma MCP to inspect this exact frame:
<FRAME LINK>

Implement this as Unity uGUI prefabs and thin UI controller scripts.

Rules:
- Use TextMeshPro.
- Use Unity UI Image/Button components.
- Do not add gameplay systems.
- Do not change scope.md.
- Keep gameplay logic out of UI scripts.
- Match the Figma hierarchy and layer names where practical.
- Create reusable prefabs/views where useful.
- After changes, summarize files created, files modified, prefab references needed, and manual Unity setup required.
```

Frame-specific notes may be added below the template, but they must not expand the locked gameplay scope.

---

## Frame Implementation Order

Implement frames in this order:

1. `Frame_01_MainMenu`
2. `Frame_02_TownHUD`
3. `Frame_03_CombatHUD`
4. `Frame_04_MiningHUD`
5. `Frame_05_InventoryCraftingPanel`
6. `Frame_06_CharacterClassTalentPanel`
7. `Frame_07_AfkResultsModal`
8. `Frame_08_QuestCompleteToast`

Do not include `Frame_09_ReviewerFlowMap` as a Unity implementation target. It is reference-only.

---

## Per-Frame Handoff Notes

### Frame_01_MainMenu

Primary Unity output:

```text
Main menu Canvas layout
New Game button hook
Continue button hook
Evaluation note text
```

Keep save/start logic in existing systems or later wiring tasks.

### Frame_02_TownHUD

Primary Unity output:

```text
Top bar
Character card
Quest tracker
Town activity buttons
Inventory/crafting/class/talent/AFK entry points
```

The view should render state from character, quest, inventory, and task systems.

### Frame_03_CombatHUD

Primary Unity output:

```text
Enemy HP bar
Character summary
Quest tracker
Loot log
Floating damage placeholder
```

Combat rules stay in combat systems. The UI only renders combat events/state.

### Frame_04_MiningHUD

Primary Unity output:

```text
Mining progress bar
Character summary
Quest tracker
Recent rewards panel
```

Gathering rules stay in gathering systems. The UI only renders mining progress and rewards.

### Frame_05_InventoryCraftingPanel

Primary Unity output:

```text
Inventory grid
Recipe rows
Enabled/disabled craft button states
Missing materials text
```

Recipe validity should come from crafting/inventory systems, not duplicated UI rules.

### Frame_06_CharacterClassTalentPanel

Primary Unity output:

```text
Character summary
Stats panel
Class choice buttons
Talent nodes
Locked/available/selected states
```

Stat math, class choice, and talent spending rules stay in progression/class/talent systems.

### Frame_07_AfkResultsModal

Primary Unity output:

```text
AFK results modal
Away duration label
Two character reward cards
Claim/continue button
```

Offline reward calculation stays in the offline progression system.

### Frame_08_QuestCompleteToast

Primary Unity output:

```text
Toast view
Quest complete state
Level up state
Item crafted state
```

Toast UI should be non-blocking and driven by events/messages.

---

## Acceptance Test

This documentation task is done when:

- [ ] `docs/figma-to-codex-handoff-checklist.md` exists.
- [ ] `prompts/task_06_figma_to_codex_handoff.md` exists.
- [ ] Required frame link placeholders are present.
- [ ] Layer naming rules are clear.
- [ ] Component states are documented.
- [ ] Unity UI implementation notes are included.
- [ ] Codex prompt template is included.
- [ ] No gameplay features are added.

Documentation-only constraints:

- [ ] Do not modify Unity scenes.
- [ ] Do not create C# scripts.
- [ ] Do not import assets.
- [ ] Do not create or edit Figma files.
- [ ] Do not change `scope.md`.
