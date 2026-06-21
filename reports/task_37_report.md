# Task 37 Report

## Summary

Added script-only crafting UI binding for recipe rows, missing-material display, and craft callbacks without changing scenes, prefabs, assets, or running Unity.

## Files Changed

- `Assets/Scripts/UI/CraftingPanel.cs`
- `Assets/Scripts/UI/CraftingRecipeRowView.cs`
- `TASKS.md`
- `reports/task_37_report.md`

## Completed Work

- Added `CraftingPanel.Bind(...)` for `CraftingSystem`, recipes, and optional item definitions.
- Added dynamic recipe row creation under the configured recipe container.
- Added material, output, missing-item, and ready-to-craft summaries.
- Added craft-by-recipe-ID callback handling.
- Added `CraftCompleted` event for higher-level UI refresh hooks.
- Added recipe ID and craft callback support to `CraftingRecipeRowView`.
- Preserved existing station label behavior.

## Validation

- Performed source-only review.
- Did not run Unity, Unity Hub, batchmode, builds, or licensing commands.
- Did not modify scenes, prefabs, or assets.

## Notes

- Scene-authorized work can wire recipe definitions, row prefab, and optional item icons later.
