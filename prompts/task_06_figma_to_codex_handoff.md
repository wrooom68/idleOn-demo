# Task 06 Prompt: Figma-to-Codex Handoff Checklist

Create a documentation-only Figma-to-Codex handoff checklist for the Unity **Idle Guild Demo**.

Read first:

```text
scope.md
docs/figma-frame-plan.md
docs/folder-conventions.md
docs/unity-2d-asset-import-checklist.md
```

Create:

```text
docs/figma-to-codex-handoff-checklist.md
prompts/task_06_figma_to_codex_handoff.md
```

Goal:

```text
Define exactly how Figma frames should be prepared before Codex uses Figma MCP to implement Unity UI.
```

The checklist must include:

```text
Required frame link placeholders:
- Frame_01_MainMenu:
- Frame_02_TownHUD:
- Frame_03_CombatHUD:
- Frame_04_MiningHUD:
- Frame_05_InventoryCraftingPanel:
- Frame_06_CharacterClassTalentPanel:
- Frame_07_AfkResultsModal:
- Frame_08_QuestCompleteToast:

Frame_09_ReviewerFlowMap must be marked reference-only, not a Unity implementation target.
```

Include layer naming rules:

```text
Frames start with Frame_
Panels start with Panel_
Buttons start with Button_
Text layers start with Text_
Icons/images start with Icon_ or Image_
Progress bars start with ProgressBar_
Inventory slots start with Slot_
World areas start with WorldArea_
Reusable components start with Component_
Avoid names like Rectangle 42, Group 7, copy, final, test, or temp
```

Include required layer checks, component states, Unity UI implementation notes, recommended Unity prefab mappings, per-frame Codex handoff prompts, frame implementation order, and acceptance tests.

Reusable handoff prompt template:

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

Frame implementation order:

```text
1. Frame_01_MainMenu
2. Frame_02_TownHUD
3. Frame_03_CombatHUD
4. Frame_04_MiningHUD
5. Frame_05_InventoryCraftingPanel
6. Frame_06_CharacterClassTalentPanel
7. Frame_07_AfkResultsModal
8. Frame_08_QuestCompleteToast
```

Constraints:

```text
Do not modify Unity scenes.
Do not create C# scripts.
Do not import assets.
Do not create or edit Figma files.
Do not change scope.md.
This task is documentation only.
```

After completing:

```text
Summarize files created.
Show the git commands to commit and push.
```
