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
