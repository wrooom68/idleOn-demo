# Autonomous Task Runner Instructions

This repository uses `TASKS.md` as the source of truth for autonomous work.

## Required Workflow

1. Read `TASKS.md` first.
2. Execute only the first task marked `READY`.
3. Never repeat tasks marked `DONE`, `REVIEW`, `BLOCKED`, `CUT`, or `TODO`.
4. Work one task at a time.
5. Create `reports/task_<id>_report.md` after each task.
6. Update the task status in `TASKS.md` after each task.
7. Commit after each task using the commit message listed on that task.
8. Stop if Unity validation, unclear scope, git conflict, or missing files are encountered.
9. Do not run Unity, Unity Hub, Unity batchmode, build commands, or licensing commands.
10. Leave mobile notification to the ChatGPT hourly watcher, which watches GitHub reports, commits, and status changes.

## Safety Rules

- Only edit files explicitly allowed by the active task.
- Do not modify gameplay code unless the active task allows it.
- Do not modify assets unless the active task allows it.
- Do not modify scenes unless the active task allows it.
- Do not run builds.
- Do not run Unity validation locally.
- If manual Unity validation is required, mark the task `REVIEW` or `BLOCKED` according to `TASKS.md` and stop.

## Status Meanings

- `DONE` = completed and must not be repeated.
- `READY` = executable now.
- `REVIEW` = completed but needs user/manual Unity review.
- `TODO` = future task.
- `BLOCKED` = needs user input.
- `CUT` = removed from scope.
