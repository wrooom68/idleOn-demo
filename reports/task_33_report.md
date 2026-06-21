# Task 33 Report

## Summary

Added second-character task behavior and unlocked-roster safeguards without changing scenes, prefabs, assets, or running Unity.

## Files Changed

- `Assets/Scripts/Systems/CharacterRosterSystem.cs`
- `Assets/Scripts/Systems/OfflineProgressionSystem.cs`
- `TASKS.md`
- `reports/task_33_report.md`

## Completed Work

- Added `GetUnlockedCharacters()` for UI and systems that need usable roster members only.
- Made active-character lookup fall back to an unlocked character if the saved active ID points at a locked character.
- Prevented `SetActiveCharacter(...)` from selecting locked characters.
- Added `GetSecondCharacter()` helper.
- Added second-character assignment helpers for:
  - Idle
  - Slime combat
  - Copper mining
  - Generic validated task assignment
- Ensured the second character receives a task timestamp when unlocked.
- Updated AFK reward calculation to skip locked characters.

## Validation

- Performed source-only review.
- Did not run Unity, Unity Hub, batchmode, builds, or licensing commands.
- Did not modify scenes, prefabs, or assets.

## Notes

- AFK rewards already iterate over the roster, so once Character 2 is unlocked and assigned a valid task, it participates in AFK reward calculation independently.
