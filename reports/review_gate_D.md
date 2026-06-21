# Review Gate D

Status: WAITING_FOR_USER_REVIEW

Approval phrase: `REVIEW DONE D`

Blocked phrase: `REVIEW BLOCKED D: <short reason>`

## In-Session Message

```text
=== REVIEW GATE D ===

Codex stopped because Tasks 35-40 changed HUD, quest tracker, inventory, crafting, character, town flow, navigation, and readability UI scripts.

Review type:
Unity visual review

Open Unity:
Yes

Checklist:
[ ] Open Unity project manually.
[ ] Wait for Unity compile to finish.
[ ] Check Console for red compile errors.
[ ] Confirm there are no missing script errors.
[ ] Confirm HUD and quest tracker UI compile and display readable labels where wired.
[ ] Confirm inventory UI binding compiles and displays stack rows where wired.
[ ] Confirm crafting UI binding compiles, shows missing materials, and disables unavailable crafts where wired.
[ ] Confirm character UI binding compiles and shows unlocked character cards where wired.
[ ] Confirm town flow buttons/panels are understandable where wired.
[ ] Confirm scene navigation/readability labels are clear.
[ ] Confirm no scenes/assets were unexpectedly modified by Codex.
[ ] Confirm Codex can continue to QA/build/docs batch.

If everything is good, reply exactly:
REVIEW DONE D

If there is a problem, reply:
REVIEW BLOCKED D: <short reason>

Codex must not continue until one of those replies is received.
```

## Scope

Review Gate D covers Batch D:

- Task 35: HUD and quest tracker binding
- Task 36: Inventory UI binding
- Task 37: Crafting UI binding
- Task 38: Character UI binding
- Task 39: Town scene flow binding
- Task 40: Scene navigation and readability pass

## Reports To Check

- `reports/task_35_report.md`
- `reports/task_36_report.md`
- `reports/task_37_report.md`
- `reports/task_38_report.md`
- `reports/task_39_report.md`
- `reports/task_40_report.md`

## Required Review

This gate is a Unity visual/manual validation gate. Open Unity manually, let scripts compile, and inspect HUD, inventory, crafting, character, town flow, navigation, missing references, and readability in the available wired surfaces.

## Resume Behavior

If the user replies `REVIEW DONE D`, Codex must:

- Mark Review Gate D `APPROVED` in `TASKS.md`.
- Create or update `reports/review_gate_D_approval.md`.
- Promote Tasks 41-50 to `READY`.
- Commit with `Approve review gate D`.
- Continue autonomous batch-runner from Task 41.

If the user replies `REVIEW BLOCKED D: <short reason>`, Codex must:

- Mark Review Gate D `BLOCKED` in `TASKS.md`.
- Create or update `reports/review_gate_D_blocked.md`.
- Record the reason.
- Commit with `Block review gate D`.
- Stop.
