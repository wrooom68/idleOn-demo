# Autonomous Batch Review Workflow

## Overview

The autonomous batch runner lets Codex execute multiple `READY` tasks in order while keeping review-heavy work grouped behind explicit gates. `TASKS.md` is the task ledger, `AGENTS.md` is the runner contract, and `scope.md` is the feature boundary.

## How Batches Work

Codex reads `TASKS.md`, finds the first `READY` task, completes it, creates `reports/task_<id>_report.md`, updates the task status, commits with the task's listed commit message, and then moves to the next `READY` task only if no stop condition is hit.

Codex must not repeat anything marked `DONE`, `REVIEW`, `BLOCKED`, `CUT`, `TODO`, `WAITING_FOR_USER_REVIEW`, or `APPROVED`.

## Review Gates

Review gates batch manual validation together. When Codex reaches a gate, it creates `reports/review_gate_<id>.md`, marks that gate `WAITING_FOR_USER_REVIEW` in `TASKS.md`, commits the gate report and status update, and stops.

Each gate lists the exact checks the user should perform. Some gates are source/code review only. Other gates require the user to manually open Unity and visually inspect scenes, UI, sprites, references, readability, AFK behavior, or submission readiness.

## Approval

The user approves a gate by either:

- Changing the gate status in `TASKS.md` to `APPROVED`.
- Adding the approval phrase to `TASKS.md`.

Approval phrase format:

```text
REVIEW DONE <gate_id>
```

For example:

```text
REVIEW DONE A
```

After approval, Codex may resume from the next `READY` task.

## Mobile Notification

Codex does not send direct mobile notifications. The ChatGPT hourly watcher reads GitHub commits, reports, and `TASKS.md` changes. Writing reports, committing changes, and updating gate statuses is enough for the watcher to detect progress.

## What Codex Must Never Run

Codex must never run:

- Unity
- Unity Hub
- Unity batchmode
- Unity builds
- Unity licensing commands
- Package installation commands

The user manually opens Unity for validation.

## How To Resume

To resume autonomous work after this setup, use:

```text
Autonomous batch-runner mode.

Read:
- AGENTS.md
- TASKS.md
- scope.md

Execute READY tasks in order until a stop condition or review gate is reached.

Rules:
- Do not run Unity, Unity Hub, batchmode, builds, or licensing commands.
- Do not modify files outside the current task scope.
- Do not add features outside scope.md.
- Do not repeat DONE, REVIEW, BLOCKED, CUT, TODO, WAITING_FOR_USER_REVIEW, or APPROVED items.
- For each task, create reports/task_<id>_report.md.
- Update TASKS.md status.
- Commit each task separately with the listed commit message.
- When a review gate is reached, create reports/review_gate_<id>.md, mark the gate WAITING_FOR_USER_REVIEW, list exact review checklist, commit, and stop.
- Continue only when the gate is marked APPROVED or the approval phrase is added to TASKS.md.
```
