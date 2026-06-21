# Autonomous Task Queue

Status system:

- `DONE` = completed and must not be repeated.
- `READY` = executable now.
- `REVIEW` = completed but needs user/manual Unity review.
- `TODO` = future task.
- `BLOCKED` = needs user input.
- `CUT` = removed from scope.
- `WAITING_FOR_USER_REVIEW` = review gate is waiting for user confirmation.
- `APPROVED` = review gate approved by user.

Rules:

- Execute only the first task marked `READY`.
- Never repeat tasks marked `DONE`, `REVIEW`, `BLOCKED`, `CUT`, `TODO`, `WAITING_FOR_USER_REVIEW`, or `APPROVED`.
- Work one task at a time.
- Create `reports/task_<id>_report.md` after each task.
- Update task status after each task.
- Commit after each task using the listed commit message.
- Stop if Unity validation, unclear scope, git conflict, or missing files are encountered.
- Do not run Unity, Unity Hub, Unity batchmode, build commands, or licensing commands.
- When a review gate is reached, create `reports/review_gate_<id>.md`, mark the gate `WAITING_FOR_USER_REVIEW`, commit, and stop.
- Continue after a review gate only when the gate is marked `APPROVED` or `TASKS.md` contains `REVIEW DONE <gate_id>`.

## Task 01

Status: DONE
Title: Locked 25-hour scope
Commit: Locked 25-hour scope
Notes: Completed and must not be repeated.

## Task 02

Status: DONE
Title: Repo hygiene files
Commit: Add repo hygiene files
Notes: Completed and must not be repeated.

## Task 03

Status: DONE
Title: Approved asset shortlist
Commit: Add approved asset shortlist
Notes: Completed and must not be repeated.

## Task 04

Status: DONE
Title: Unity 2D asset import checklist
Commit: Add Unity 2D asset import checklist
Notes: Completed and must not be repeated.

## Task 05

Status: DONE
Title: Figma frame plan and Figma frames
Commit: Add Figma frame plan and frames
Notes: Completed and must not be repeated.

## Task 06

Status: DONE
Title: Real Unity 2D project initialized
Commit: Initialize Unity 2D project
Notes: Completed and must not be repeated.

## Task 07

Status: REVIEW
Title: Approved assets imported and cleaned
Commit: Import and clean approved assets
Notes: Completed but needs user/manual Unity review. Do not repeat unless the user explicitly asks.

## Task 08

Status: DONE
Title: Core data definitions and runtime state

Allowed files:

- `Assets/Scripts/Data/`
- `Assets/Scripts/Runtime/`
- `Assets/Scripts/Core/`
- `reports/`
- `TASKS.md`

Do not touch:

- `Assets/Scenes/`
- `Assets/ThirdParty/`
- `Assets/Art/`
- `CREDITS.md`

Goal:

Create ScriptableObject data definitions and plain serializable runtime state classes for the locked idle RPG scope.

Commit:

Add core data definitions and runtime state

Acceptance:

- Data definition scripts exist for Item, Enemy, Recipe, Quest, Class, Talent, Zone, and Gatherable.
- Runtime state classes exist for SaveData, PlayerProfile, CharacterState, InventoryState, QuestProgressState, SkillState, TalentState, and TaskState.
- No MonoBehaviour references inside save/runtime state.
- No gameplay systems implemented yet.
- No UI implemented yet.
- Unity should compile when opened manually.

## Task 09

Status: DONE
Title: Seed initial data assets
Commit: Seed initial data assets

## Task 10

Status: DONE
Title: Save serialization service
Commit: Add save serialization service

## Task 11

Status: DONE
Title: Inventory domain logic
Commit: Add inventory domain logic

## Task 12

Status: DONE
Title: Character progression domain logic
Commit: Add character progression domain logic

## Task 13

Status: DONE
Title: Gathering domain logic
Commit: Add gathering domain logic

## Task 14

Status: DONE
Title: Combat domain logic
Commit: Add combat domain logic

## Task 15

Status: DONE
Title: Crafting domain logic
Commit: Add crafting domain logic

## Task 16

Status: DONE
Title: Quest domain logic
Commit: Add quest domain logic

## Task 17

Status: DONE
Title: Talent domain logic
Commit: Add talent domain logic

## Task 18

Status: DONE
Title: Offline progress calculation
Commit: Add offline progress calculation

## Task 19

Status: DONE
Title: Core service registry
Commit: Add core service registry

## Task 20

Status: DONE
Title: Bootstrap flow
Commit: Add bootstrap flow

## Task 21

Status: REVIEW
Title: Main scene wiring plan
Commit: Add main scene wiring plan

## Task 22

Status: REVIEW
Title: Player profile initialization
Commit: Add player profile initialization

## Task 23

Status: REVIEW
Title: Runtime event definitions
Commit: Add runtime event definitions

## Task 24

Status: REVIEW
Title: Gameplay controller shell
Commit: Add gameplay controller shell

## Task 25

Status: DONE
Title: QuestSystem event listeners and quest flow

Allowed files:

- `Assets/Scripts/Systems/`
- `Assets/Scripts/Runtime/`
- `Assets/Scripts/Data/`
- `reports/`
- `TASKS.md`

Do not touch:

- `Assets/Scenes/`
- `Assets/ThirdParty/`
- `Assets/Art/`
- `CREDITS.md`

Goal:

Implement QuestSystem event listeners, progress updates, claim rewards, and next quest flow.

Commit: Implement quest progression system

## Task 26

Status: DONE
Title: Quest Tracker UI shell

Allowed files:

- `Assets/Scripts/UI/`
- `Assets/Scripts/Systems/`
- `reports/`
- `TASKS.md`

Do not touch:

- `Assets/ThirdParty/`
- `CREDITS.md`

Goal:

Implement quest tracker UI controller shell and UI-facing methods without scene/prefab changes unless already safe.

Commit: Add quest tracker UI shell

## Task 27

Status: DONE
Title: Class selection system

Allowed files:

- `Assets/Scripts/Systems/`
- `Assets/Scripts/Data/`
- `Assets/Scripts/Runtime/`
- `reports/`
- `TASKS.md`

Goal:

Implement Beginner to Warrior, Archer, Mage class selection logic and class stat modifiers.

Commit: Add class selection system

## Task 28

Status: DONE
Title: Talent system

Allowed files:

- `Assets/Scripts/Systems/`
- `Assets/Scripts/Data/`
- `Assets/Scripts/Runtime/`
- `reports/`
- `TASKS.md`

Goal:

Implement four-talent spend/validation/stat modifier system.

Commit: Implement talent system

## Task 29

Status: TODO
Title: Class and talent UI integration
Commit: Add class and talent UI integration

## Task 30

Status: TODO
Title: Economy and reward feedback
Commit: Add economy and reward feedback

## Task 31

Status: TODO
Title: AFK task assignment system
Commit: Add AFK task assignment system

## Task 32

Status: TODO
Title: AFK rewards integration
Commit: Add AFK rewards integration

## Task 33

Status: TODO
Title: Second character task behavior
Commit: Add second character task behavior

## Task 34

Status: TODO
Title: AFK results modal integration
Commit: Add AFK results modal integration

## Task 35

Status: TODO
Title: HUD and quest tracker binding
Commit: Add HUD and quest tracker binding

## Task 36

Status: TODO
Title: Inventory UI binding
Commit: Add inventory UI binding

## Task 37

Status: TODO
Title: Crafting UI binding
Commit: Add crafting UI binding

## Task 38

Status: TODO
Title: Character UI binding
Commit: Add character UI binding

## Task 39

Status: TODO
Title: Town scene flow binding
Commit: Add town scene flow binding

## Task 40

Status: TODO
Title: Scene navigation and readability pass
Commit: Add scene navigation and readability pass

## Task 41

Status: TODO
Title: Navigation UI binding
Commit: Add navigation UI binding

## Task 42

Status: TODO
Title: Main menu flow
Commit: Add main menu flow

## Task 43

Status: TODO
Title: Save and continue flow
Commit: Add save and continue flow

## Task 44

Status: TODO
Title: New game reset flow
Commit: Add new game reset flow

## Task 45

Status: TODO
Title: Demo pacing tuning
Commit: Tune demo pacing

## Task 46

Status: TODO
Title: Manual QA checklist
Commit: Add manual QA checklist

## Task 47

Status: TODO
Title: Bug fix pass
Commit: Apply bug fix pass

## Task 48

Status: TODO
Title: Polish pass
Commit: Apply polish pass

## Task 49

Status: TODO
Title: Release readiness notes
Commit: Add release readiness notes

## Task 50

Status: TODO
Title: Final handoff report
Commit: Add final handoff report

## Review Gate A

Status: WAITING_FOR_USER_REVIEW
ID: A
After tasks: 25, 26, 27, 28
Approval phrase: REVIEW DONE A
Review type: Code/system review. No visual Unity review required unless compile errors are suspected.

Checklist:

- Check reports for Tasks 25-28.
- Open Unity manually only if needed.
- Confirm no obvious script compile issues.
- Confirm no gameplay scope creep.
- Confirm Codex can continue to UI/economy batch.

## Review Gate B

Status: TODO
ID: B
After tasks: 29, 30
Approval phrase: REVIEW DONE B
Review type: Unity visual/manual review required.

Checklist:

- Check reports for Tasks 29-30.
- Open Unity manually.
- Confirm class and talent UI flows are understandable.
- Confirm economy and reward feedback are readable.
- Confirm Codex can continue to AFK/task systems.

## Review Gate C

Status: TODO
ID: C
After tasks: 31, 32, 33, 34
Approval phrase: REVIEW DONE C
Review type: Unity manual validation required for AFK and second character behavior.

Checklist:

- Check reports for Tasks 31-34.
- Open Unity manually.
- Confirm each character can have a current task.
- Confirm AFK simulation rewards are shown and applied.
- Confirm Character 2 behavior works after unlock.
- Confirm Codex can continue to HUD/inventory/town/scene flow.

## Review Gate D

Status: TODO
ID: D
After tasks: 35, 36, 37, 38, 39, 40
Approval phrase: REVIEW DONE D
Review type: Unity visual review required.

Checklist:

- Check reports for Tasks 35-40.
- Open Unity manually.
- Confirm UI, sprites, scene navigation, missing references, and readability.
- Confirm reviewer path is understandable without explanation.
- Confirm Codex can continue to QA/build/docs.

## Review Gate E

Status: TODO
ID: E
After tasks: 41, 42, 43, 44, 45, 46, 47, 48, 49, 50
Approval phrase: REVIEW DONE E
Review type: Final QA, playtest, build, video, and submission review.

Checklist:

- Check reports for Tasks 41-50.
- Confirm final QA/playtest notes are complete.
- Confirm build/submission materials are ready.
- Confirm README, credits, video notes, and handoff materials match `scope.md`.
