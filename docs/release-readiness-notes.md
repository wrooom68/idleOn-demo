# Release Readiness Notes

These notes summarize what still needs human validation before submission.

## Current Readiness

- Core idle RPG systems are implemented in source.
- UI controller scripts exist for menu, HUD, quest tracker, inventory, crafting, character, class, talent, combat, mining, AFK results, and navigation.
- Manual QA checklist exists at `docs/manual-qa-checklist.md`.
- Task reports exist under `reports/`.
- README and CREDITS files exist.

## Required Manual Validation

- Open Unity manually.
- Let scripts compile.
- Confirm the Console has no red errors.
- Confirm there are no missing scripts or missing serialized references.
- Confirm Main Menu can start New Game and Continue.
- Confirm Town, Combat, and Mine scenes are included and navigable.
- Confirm quest progress, crafting, class choice, talents, Character 2 unlock, AFK simulation, and save/load work in the intended reviewer path.
- Confirm no unapproved assets or audio are included.

## Build And Submission Items

- Create a runnable build or uploadable build link.
- Record a short 3-5 minute demo video.
- Update README build/link placeholders after a build exists.
- Recheck CREDITS against the final assets present in Unity.
- Confirm the final GitHub repository has a clean, meaningful commit history.

## Known Constraints

- Codex did not run Unity, Unity Hub, batchmode, builds, or licensing commands during the autonomous batch.
- Unity compile, visual scene wiring, and final build readiness remain manual review items.
- The local working tree may contain unrelated scene, asset, package, or credits changes made outside this task batch; review them before final submission commits.

## Recommended Final Review Order

1. Resolve or intentionally commit any unrelated local scene/asset/package changes.
2. Open Unity manually and complete `docs/manual-qa-checklist.md`.
3. Fix any compile or missing-reference issues found by Unity.
4. Make the final runnable build.
5. Record the demo video.
6. Update README with build and video links.
7. Complete Review Gate E.
