# Review Gate A

Status: WAITING_FOR_USER_REVIEW

Approval phrase: `REVIEW DONE A`

## Scope

Review Gate A covers Batch A:

- Task 25: QuestSystem event listeners and quest flow
- Task 26: Quest Tracker UI shell
- Task 27: Class selection system
- Task 28: Talent system

## Reports To Check

- `reports/task_25_report.md`
- `reports/task_26_report.md`
- `reports/task_27_report.md`
- `reports/task_28_report.md`

## Required Review

This gate is intended as a code/system review. No Unity visual review is required unless compile errors are suspected.

Check:

- Reports for Tasks 25-28 are present and match the intended work.
- Quest flow work stayed within the locked tutorial chain scope.
- Quest tracker work is a UI shell only and does not modify scenes or prefabs.
- Class selection supports Beginner to Warrior, Archer, and Mage only.
- Talent work supports only Damage, Mining Speed, XP Gain, and AFK Gain.
- No gameplay systems outside `scope.md` were added.
- No scenes, prefabs, assets, Unity packages, builds, or licensing files were intentionally changed by Batch A.
- Source-level changes look ready for the next UI/economy batch.

## Optional Manual Unity Check

Open Unity manually only if you suspect compile errors. Codex did not run Unity, Unity Hub, batchmode, builds, or licensing commands.

## Approval

To let Codex continue to the next batch, update `TASKS.md` by either:

- changing Review Gate A status to `APPROVED`, or
- adding the phrase `REVIEW DONE A`.
