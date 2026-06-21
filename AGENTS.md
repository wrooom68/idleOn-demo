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
- When reaching a review gate, stop.
- Create `reports/review_gate_<gate_id>.md`.
- List exactly what the user must check in Unity or by source review.
- Mark the gate `WAITING_FOR_USER_REVIEW`.
- Do not continue until the user marks the gate `APPROVED` or updates `TASKS.md` with the approval phrase.
- Approval phrase format: `REVIEW DONE <gate_id>`.

## Unity Safety

- Never run Unity commands.
- Never run Unity Hub.
- Never run Unity batchmode.
- Never run Unity builds.
- Never run Unity licensing commands.
- Do not install packages.
- The user manually opens Unity for validation.
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
