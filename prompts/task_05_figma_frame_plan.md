# Task 05 — Figma Frame Plan

Create a Figma frame plan for the IdleOn-inspired demo UI.

## Goal

Prepare a clean Figma layout plan that can be handed to Codex through Figma MCP without ambiguity.

## Required coverage

- Figma file name
- Page structure
- Frame names
- Required elements per frame
- UI hierarchy notes
- Component naming rules
- Codex handoff notes
- Clarification that full UI assets are not required in Figma

## Required frames

- `Frame_00_StyleGuide`
- `Frame_01_MainMenu`
- `Frame_02_TownHUD`
- `Frame_03_CombatHUD`
- `Frame_04_MiningHUD`
- `Frame_05_InventoryCraftingPanel`
- `Frame_06_CharacterClassTalentPanel`
- `Frame_07_AfkResultsModal`
- `Frame_08_QuestCompleteToast`
- `Frame_09_ReviewerFlowMap`

## Constraints

- Do not add new gameplay features.
- Do not create Unity scripts.
- Do not import assets.
- Do not require every UI asset to be imported into Figma.
- Keep the plan aligned with `scope.md`.

## Expected result

A documentation file under `docs/` that tells the user exactly what Figma frames to create, what each frame must contain, how layers should be named, and how Codex should use the frame links.

## Acceptance test

- Every required frame has a purpose.
- Every required frame lists necessary UI elements.
- Each frame has hierarchy notes.
- Codex handoff guidance is explicit.
- The plan explains that Figma can use simple placeholder shapes and does not need full UI art assets.
