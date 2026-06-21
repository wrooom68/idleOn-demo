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
- When a review gate is reached, create reports/review_gate_<id>.md, mark the gate WAITING_FOR_USER_REVIEW, list the exact review checklist, commit, print the required in-session review message, and stop.
- Continue only when the user replies REVIEW DONE <id>.
- If the user replies REVIEW DONE <id>, mark the gate APPROVED in TASKS.md, create or update reports/review_gate_<id>_approval.md, commit with "Approve review gate <id>", and continue from the next READY task.
- If the user replies REVIEW BLOCKED <id>: <short reason>, mark the gate BLOCKED in TASKS.md, create or update reports/review_gate_<id>_blocked.md, record the reason, commit with "Block review gate <id>", and stop.

Required review gate message format:

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
