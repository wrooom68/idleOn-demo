# Review Gate B

Status: WAITING_FOR_USER_REVIEW

Approval phrase: `REVIEW DONE B`

Blocked phrase: `REVIEW BLOCKED B: <short reason>`

## In-Session Message

```text
=== REVIEW GATE B ===

Codex stopped because Tasks 29-30 changed class, talent, economy, and reward feedback UI scripts.

Review type:
Unity visual review

Open Unity:
Yes

Checklist:
[ ] Open Unity project manually.
[ ] Wait for Unity compile to finish.
[ ] Check Console for red compile errors.
[ ] Confirm there are no missing script errors.
[ ] Confirm class and talent UI scripts compile.
[ ] Confirm class and talent UI flows are understandable where wired.
[ ] Confirm economy, reward, loot log, and toast feedback scripts compile.
[ ] Confirm reward feedback text is readable where wired.
[ ] Confirm no scenes/assets were unexpectedly modified by Codex.
[ ] Confirm Codex can continue to AFK/task systems.

If everything is good, reply exactly:
REVIEW DONE B

If there is a problem, reply:
REVIEW BLOCKED B: <short reason>

Codex must not continue until one of those replies is received.
```

## Scope

Review Gate B covers Batch B:

- Task 29: Class and talent UI integration
- Task 30: Economy and reward feedback

## Reports To Check

- `reports/task_29_report.md`
- `reports/task_30_report.md`

## Required Review

This gate is a Unity visual/manual review. Open Unity manually, allow compile to finish, and inspect the Console plus any wired class, talent, economy, reward, loot log, and toast UI surfaces.

## Resume Behavior

If the user replies `REVIEW DONE B`, Codex must:

- Mark Review Gate B `APPROVED` in `TASKS.md`.
- Create or update `reports/review_gate_B_approval.md`.
- Promote Tasks 31-34 to `READY`.
- Commit with `Approve review gate B`.
- Continue autonomous batch-runner from Task 31.

If the user replies `REVIEW BLOCKED B: <short reason>`, Codex must:

- Mark Review Gate B `BLOCKED` in `TASKS.md`.
- Create or update `reports/review_gate_B_blocked.md`.
- Record the reason.
- Commit with `Block review gate B`.
- Stop.
