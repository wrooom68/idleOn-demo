# Autonomous Batch Review Workflow

## Overview

The autonomous batch runner lets Codex execute multiple `READY` tasks in order while keeping review-heavy work grouped behind explicit gates. `TASKS.md` is the task ledger, `AGENTS.md` is the runner contract, and `scope.md` is the feature boundary.

## How Batches Work

Codex reads `TASKS.md`, finds the first `READY` task, completes it, creates `reports/task_<id>_report.md`, updates the task status, commits with the task's listed commit message, and then moves to the next `READY` task only if no stop condition is hit.

Codex must not repeat anything marked `DONE`, `REVIEW`, `BLOCKED`, `CUT`, `TODO`, `WAITING_FOR_USER_REVIEW`, or `APPROVED`.

## Review Gates

Review gates batch manual validation together. When Codex reaches a gate, it creates `reports/review_gate_<id>.md`, marks that gate `WAITING_FOR_USER_REVIEW` in `TASKS.md`, commits the gate report and status update, prints a clear in-session review message, and stops.

Each gate lists the exact checks the user should perform. Some gates are source/code review only. Other gates require the user to manually open Unity and visually inspect scenes, UI, sprites, references, readability, AFK behavior, or submission readiness.

Review gates pause inside the same Codex session. Codex must not create external popup apps, standalone notification tools, Telegram messages, ntfy messages, OS notifications, or mobile webhooks.

## In-Session Gate Message

At every review gate, Codex prints:

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

## Approval

The user approves a gate by replying in the same Codex session:

```text
REVIEW DONE <gate_id>
```

For example:

```text
REVIEW DONE A
```

When the user replies `REVIEW DONE <gate_id>`, Codex marks the gate `APPROVED` in `TASKS.md`, creates or updates `reports/review_gate_<gate_id>_approval.md`, promotes the linked next batch from `TODO` to `READY`, commits the approval and promotion with `Approve review gate <gate_id>`, and immediately resumes from the promoted `READY` tasks.

No separate manual batch preparation is needed after approval. The approval phrase is the user's permission for Codex to prepare and execute the next linked batch.

Gate-to-next-batch promotion:

- Gate A approval promotes Tasks 29-30 to `READY`.
- Gate B approval promotes Tasks 31-34 to `READY`.
- Gate C approval promotes Tasks 35-40 to `READY`.
- Gate D approval promotes Tasks 41-50 to `READY`.
- Gate E approval promotes no tasks because it is the final submission review.

If there is a problem, the user replies:

```text
REVIEW BLOCKED <gate_id>: <short reason>
```

When the user replies `REVIEW BLOCKED <gate_id>: <short reason>`, Codex marks the gate `BLOCKED` in `TASKS.md`, creates or updates `reports/review_gate_<gate_id>_blocked.md`, records the reason, commits with `Block review gate <gate_id>`, and stops.

## Mobile Notification

Codex does not send direct mobile notifications. The ChatGPT hourly watcher is only for passive mobile notification from GitHub updates. It reads GitHub commits, reports, and `TASKS.md` changes. Writing reports, committing changes, and updating gate statuses is enough for the watcher to detect progress.

## Unity Command Rules

Codex may run Unity validation only when the current task or review gate explicitly allows it.

If Unity licensing, Hub, batchmode, or validation fails, Codex stops and marks the current task or gate `BLOCKED`.

Codex must not run final builds unless the task is specifically a build task. Codex must not install packages unless a task explicitly allows it.

When a review gate asks the user to open Unity, the user manually opens Unity for validation unless the gate explicitly allows Codex-run validation.

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
- Codex may run Unity validation only when the current task or review gate explicitly allows it. If Unity licensing, Hub, batchmode, or validation fails, stop and mark the current task or gate BLOCKED. Do not run final builds unless the task is specifically a build task.
- Do not modify files outside the current task scope.
- Do not add features outside scope.md.
- Do not repeat DONE, REVIEW, BLOCKED, CUT, TODO, WAITING_FOR_USER_REVIEW, or APPROVED items.
- For each task, create reports/task_<id>_report.md.
- Update TASKS.md status.
- Commit each task separately with the listed commit message.
- When a review gate is reached, create reports/review_gate_<id>.md, mark the gate WAITING_FOR_USER_REVIEW, list exact review checklist, commit, print the required in-session review message, and stop.
- If the latest user message is REVIEW DONE <id>, approve the gate, promote the linked next batch, commit, then continue execution.
- Continue only when the user replies REVIEW DONE <id>.
```
