# Manual QA Checklist

Use this checklist for the final manual Unity review. Codex must not mark these checks as passed unless a human reviewer or an allowed Unity validation task confirms them.

## Setup

- [ ] Open the Unity project manually.
- [ ] Wait for compilation to finish.
- [ ] Confirm the Console has no red compile errors.
- [ ] Confirm there are no missing script errors.
- [ ] Confirm no unexpected scene, prefab, package, or asset changes are present.

## Main Menu

- [ ] Main menu opens.
- [ ] New Game starts a fresh save.
- [ ] Continue is disabled or blocked when no valid save exists.
- [ ] Continue resumes an existing save.
- [ ] Evaluation note is visible: `Designed to show all major systems in under 30 minutes`.
- [ ] Reset Save requires confirmation before deleting progress.

## Town Hub

- [ ] Town scene opens after starting or continuing.
- [ ] Quest tracker shows the current tutorial objective.
- [ ] Inventory, crafting, character, talent/class, combat, mining, and AFK controls are understandable.
- [ ] Toast/status feedback appears for important actions.

## Combat

- [ ] Reviewer can enter combat.
- [ ] Slime HP decreases through automatic or button-driven combat flow.
- [ ] Slime defeat grants XP, coins, and Slime Goo.
- [ ] Quest progress updates for killing Slimes.
- [ ] Loot log or status feedback is readable.

## Mining

- [ ] Reviewer can enter mining.
- [ ] Copper mining progress repeats.
- [ ] Copper Ore is added to inventory.
- [ ] Mining XP is granted.
- [ ] Quest progress updates for Copper Ore collection.

## Crafting

- [ ] Copper Ore converts into Copper Bars.
- [ ] Copper Sword can be crafted from Copper Bars and Slime Goo.
- [ ] Copper Pickaxe can be crafted from Copper Bars.
- [ ] Craft buttons are disabled or clearly blocked when materials are missing.
- [ ] Crafting consumes required items and adds the crafted item.

## Progression

- [ ] Character reaches or approaches level 5 within the intended reviewer path.
- [ ] Class choice becomes available at level 5.
- [ ] Warrior, Archer, and Mage choices are visible and selectable.
- [ ] Talent points can be spent.
- [ ] Talent feedback and stat summaries are readable.

## Roster And AFK

- [ ] Character 2 unlocks through quest progression.
- [ ] Two character cards are visible after unlock.
- [ ] Each character can show a current task.
- [ ] Simulate 2 Hours AFK opens the AFK results modal.
- [ ] AFK rewards are applied to XP, coins, and inventory.
- [ ] Save/load preserves AFK-relevant state.

## Submission Readiness

- [ ] README exists and describes how to review the demo.
- [ ] CREDITS exists and matches used assets.
- [ ] Reports exist for completed tasks.
- [ ] Review Gate E report exists after Tasks 41-50.
