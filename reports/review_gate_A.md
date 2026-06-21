# Review Gate A

Status: WAITING_FOR_USER_REVIEW

Approval phrase: `REVIEW DONE A`

Blocked phrase: `REVIEW BLOCKED A: <short reason>`

## In-Session Message

```text
=== REVIEW GATE A ===

Codex stopped because Tasks 25-28 changed quest, class, and talent systems.

Review type:
Code/system review

Open Unity:
Yes, recommended.

Checklist:
[ ] Open Unity project manually.
[ ] Wait for Unity compile to finish.
[ ] Check Console for red compile errors.
[ ] Confirm there are no missing script errors.
[ ] Confirm QuestSystem-related scripts compile.
[ ] Confirm QuestTracker UI shell scripts compile.
[ ] Confirm ClassSystem scripts compile.
[ ] Confirm TalentSystem scripts compile.
[ ] Confirm no duplicate class names or namespace conflicts.
[ ] Confirm no scenes/assets were unexpectedly modified.
[ ] Confirm Codex can continue to the next batch.

If everything is good, reply exactly:
REVIEW DONE A

If there is a problem, reply:
REVIEW BLOCKED A: <short reason>

Codex must not continue until one of those replies is received.
```

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

This gate is a code/system review. Unity is recommended so the user can manually confirm compile status and missing-script state.

## Resume Behavior

If the user replies `REVIEW DONE A`, Codex must:

- Mark Review Gate A `APPROVED` in `TASKS.md`.
- Create or update `reports/review_gate_A_approval.md`.
- Commit with `Approve review gate A`.
- Continue autonomous batch-runner from the next `READY` task.

If the user replies `REVIEW BLOCKED A: <short reason>`, Codex must:

- Mark Review Gate A `BLOCKED` in `TASKS.md`.
- Create or update `reports/review_gate_A_blocked.md`.
- Record the reason.
- Commit with `Block review gate A`.
- Stop.
