# Review Gate E

Status: WAITING_FOR_USER_REVIEW

Approval phrase: `REVIEW DONE E`

Blocked phrase: `REVIEW BLOCKED E: <short reason>`

## In-Session Message

```text
=== REVIEW GATE E ===

Codex stopped because Tasks 41-50 completed the final menu, save/reset, tuning, QA, polish, release notes, and handoff batch.

Review type:
Build review

Open Unity:
Yes

Checklist:
[ ] Open Unity project manually.
[ ] Wait for Unity compile to finish.
[ ] Check Console for red compile errors.
[ ] Confirm there are no missing script or missing serialized reference errors.
[ ] Complete docs/manual-qa-checklist.md.
[ ] Confirm Main Menu New Game, Continue, and Reset Save flow works.
[ ] Confirm Town, Combat, Mining, Crafting, Quest, Class, Talent, Character 2, AFK, and Save/Load flows work.
[ ] Confirm the reviewer path is understandable and can be completed in under 30 minutes.
[ ] Confirm no unapproved assets or audio are included.
[ ] Create or verify the runnable build/build link.
[ ] Record or verify the short demo video.
[ ] Update README build/video placeholders if needed.
[ ] Recheck CREDITS against the final asset set.
[ ] Review unrelated local scene, asset, package, and CREDITS changes before final submission.
[ ] Confirm final submission materials are ready.

If everything is good, reply exactly:
REVIEW DONE E

If there is a problem, reply:
REVIEW BLOCKED E: <short reason>

Codex must not continue until one of those replies is received.
```

## Scope

Review Gate E covers final submission readiness after Tasks 41-50.

## Reports To Check

- `reports/task_41_report.md`
- `reports/task_42_report.md`
- `reports/task_43_report.md`
- `reports/task_44_report.md`
- `reports/task_45_report.md`
- `reports/task_46_report.md`
- `reports/task_47_report.md`
- `reports/task_48_report.md`
- `reports/task_49_report.md`
- `reports/task_50_report.md`
- `reports/final_handoff_report.md`

## Required Review

This gate is a final manual Unity, build, video, README, credits, and submission review. Codex did not run Unity or builds during the final batch.

## Resume Behavior

If the user replies `REVIEW DONE E`, Codex must:

- Mark Review Gate E `APPROVED` in `TASKS.md`.
- Create or update `reports/review_gate_E_approval.md`.
- Promote no additional tasks.
- Commit with `Approve review gate E`.
- Stop unless the user gives a new task.

If the user replies `REVIEW BLOCKED E: <short reason>`, Codex must:

- Mark Review Gate E `BLOCKED` in `TASKS.md`.
- Create or update `reports/review_gate_E_blocked.md`.
- Record the reason.
- Commit with `Block review gate E`.
- Stop.
