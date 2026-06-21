# Autonomous Batch Runner Instructions

This repository uses `TASKS.md` as the source of truth for autonomous work. `scope.md` is the scope boundary.

## Task Statuses

- `DONE` = completed and should never be repeated.
- `READY` = executable now.
- `REVIEW` = task completed but needs manual/user/Unity review.
- `TODO` = future task.
- `BLOCKED` = cannot continue without user input.
- `CUT` = removed from scope.
- `WAITING_FOR_USER_REVIEW` = review gate is waiting for user confirmation.
- `APPROVED` = review gate approved by user.

## Autonomous Batch Rules

1. Read `TASKS.md` first.
2. Read `scope.md` before changing implementation files.
3. Find the first `READY` task.
4. Execute `READY` tasks in order.
5. Work one task at a time.
6. After each task, create `reports/task_<id>_report.md`.
7. Update task status in `TASKS.md`.
8. Commit each task separately using its listed commit message.
9. Continue to the next `READY` task only if no stop condition is hit.
10. Never repeat items marked `DONE`, `REVIEW`, `BLOCKED`, `CUT`, `TODO`, `WAITING_FOR_USER_REVIEW`, or `APPROVED`.

## Review Gate Rules

- Batch visual/manual reviews together.
- Do not ask for Unity visual review after every task.
- When reaching a review gate, create `reports/review_gate_<gate_id>.md`.
- Update `TASKS.md` so the gate status is `WAITING_FOR_USER_REVIEW`.
- Commit the review gate report and `TASKS.md` update.
- Stop execution.
- Print a clear in-session review message to the user.
- Do not create external popup apps, standalone notification tools, Telegram messages, ntfy messages, OS notifications, or mobile webhooks.
- The in-session review message must include the review gate ID, why Codex stopped, exactly what the user must review, whether Unity must be opened, which scenes/panels/scripts/features to check, what reply resumes execution, and what reply blocks execution.
- Codex must not continue until the user replies with `REVIEW DONE <gate_id>` or `REVIEW BLOCKED <gate_id>: <short reason>`.

## Required Review Gate Message

At every review gate, output this exact format:

```text
=== REVIEW GATE <ID> ===

Codex stopped because this batch needs user review.

Review type:
<Code review / Unity visual review / Gameplay review / Build review>

Open Unity:
<Yes/No>

Checklist:
[ ] item 1
[ ] item 2
[ ] item 3
[ ] item 4

If everything is good, reply exactly:
REVIEW DONE <ID>

If there is a problem, reply:
REVIEW BLOCKED <ID>: <short reason>

Codex must not continue until one of those replies is received.
```

## Review Gate Resume Rules

- When the user replies `REVIEW DONE <gate_id>`, mark the review gate `APPROVED` in `TASKS.md`, create or update `reports/review_gate_<gate_id>_approval.md`, commit with `Approve review gate <gate_id>`, then continue autonomous batch-runner from the next `READY` task.
- When the user replies `REVIEW BLOCKED <gate_id>: <short reason>`, mark the review gate `BLOCKED` in `TASKS.md`, create or update `reports/review_gate_<gate_id>_blocked.md`, record the reason, commit with `Block review gate <gate_id>`, and stop.

## Unity Safety

- Codex may run Unity validation only when the current task or review gate explicitly allows it.
- If Unity licensing, Hub, batchmode, or validation fails, stop and mark the current task or gate `BLOCKED`.
- Do not run final builds unless the task is specifically a build task.
- Do not install packages.
- When a gate asks the user to open Unity, the user manually opens Unity for validation unless that gate explicitly allows Codex-run validation.
- If a task requires Unity scene, prefab, or visual validation, mark it `REVIEW` or stop at a review gate.

## Scope Safety

- Only edit files explicitly allowed by the active task.
- Do not modify assets unless the active task explicitly allows it.
- Do not modify scenes unless the active task explicitly allows it.
- Do not add features outside `scope.md`.
- Stop on unclear scope, git conflict, missing files, or required Unity validation.

## Notification

- Do not attempt direct mobile notifications.
- The ChatGPT hourly watcher reads GitHub commits, reports, and `TASKS.md` changes.
- Writing reports, commits, and review gate files is enough to trigger notification.
