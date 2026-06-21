# Review Gate C

Status: WAITING_FOR_USER_REVIEW

Approval phrase: `REVIEW DONE C`

Blocked phrase: `REVIEW BLOCKED C: <short reason>`

## In-Session Message

```text
=== REVIEW GATE C ===

Codex stopped because Tasks 31-34 changed AFK task assignment, AFK rewards, second character behavior, and AFK results modal scripts.

Review type:
Gameplay review

Open Unity:
Yes

Checklist:
[ ] Open Unity project manually.
[ ] Wait for Unity compile to finish.
[ ] Check Console for red compile errors.
[ ] Confirm there are no missing script errors.
[ ] Confirm each character can have a current task.
[ ] Confirm locked characters do not receive AFK rewards.
[ ] Confirm Character 2 behavior works after unlock.
[ ] Confirm AFK simulation rewards are shown and applied.
[ ] Confirm the AFK results modal shows per-character rewards.
[ ] Confirm AFK reward changes update inventory, XP, coins, and quest progress where applicable.
[ ] Confirm no scenes/assets were unexpectedly modified by Codex.
[ ] Confirm Codex can continue to HUD/inventory/town/scene flow.

If everything is good, reply exactly:
REVIEW DONE C

If there is a problem, reply:
REVIEW BLOCKED C: <short reason>

Codex must not continue until one of those replies is received.
```

## Scope

Review Gate C covers Batch C:

- Task 31: AFK task assignment system
- Task 32: AFK rewards integration
- Task 33: Second character task behavior
- Task 34: AFK results modal integration

## Reports To Check

- `reports/task_31_report.md`
- `reports/task_32_report.md`
- `reports/task_33_report.md`
- `reports/task_34_report.md`

## Required Review

This gate is a Unity gameplay/manual validation gate. Open Unity manually, let scripts compile, and verify AFK plus Character 2 behavior in the available wired surfaces.

## Resume Behavior

If the user replies `REVIEW DONE C`, Codex must:

- Mark Review Gate C `APPROVED` in `TASKS.md`.
- Create or update `reports/review_gate_C_approval.md`.
- Promote Tasks 35-40 to `READY`.
- Commit with `Approve review gate C`.
- Continue autonomous batch-runner from Task 35.

If the user replies `REVIEW BLOCKED C: <short reason>`, Codex must:

- Mark Review Gate C `BLOCKED` in `TASKS.md`.
- Create or update `reports/review_gate_C_blocked.md`.
- Record the reason.
- Commit with `Block review gate C`.
- Stop.
