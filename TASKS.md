# Autonomous Task Queue

Status system:

- `DONE` = completed and must not be repeated.
- `READY` = executable now.
- `REVIEW` = completed but needs user/manual Unity review.
- `TODO` = future task.
- `BLOCKED` = needs user input.
- `CUT` = removed from scope.

Rules:

- Execute only the first task marked `READY`.
- Never repeat tasks marked `DONE`, `REVIEW`, `BLOCKED`, `CUT`, or `TODO`.
- Work one task at a time.
- Create `reports/task_<id>_report.md` after each task.
- Update task status after each task.
- Commit after each task using the listed commit message.
- Stop if Unity validation, unclear scope, git conflict, or missing files are encountered.
- Do not run Unity, Unity Hub, Unity batchmode, build commands, or licensing commands.

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

Status: READY
Title: Town activity loop
Commit: Add town activity loop

## Task 26

Status: TODO
Title: Mining activity loop
Commit: Add mining activity loop

## Task 27

Status: TODO
Title: Combat activity loop
Commit: Add combat activity loop

## Task 28

Status: TODO
Title: Crafting activity loop
Commit: Add crafting activity loop

## Task 29

Status: TODO
Title: Quest tracking integration
Commit: Add quest tracking integration

## Task 30

Status: TODO
Title: Talent unlock integration
Commit: Add talent unlock integration

## Task 31

Status: TODO
Title: Class choice integration
Commit: Add class choice integration

## Task 32

Status: TODO
Title: AFK rewards integration
Commit: Add AFK rewards integration

## Task 33

Status: TODO
Title: UI prefab structure
Commit: Add UI prefab structure

## Task 34

Status: TODO
Title: HUD UI binding
Commit: Add HUD UI binding

## Task 35

Status: TODO
Title: Inventory UI binding
Commit: Add inventory UI binding

## Task 36

Status: TODO
Title: Crafting UI binding
Commit: Add crafting UI binding

## Task 37

Status: TODO
Title: Character UI binding
Commit: Add character UI binding

## Task 38

Status: TODO
Title: Talent UI binding
Commit: Add talent UI binding

## Task 39

Status: TODO
Title: Quest UI binding
Commit: Add quest UI binding

## Task 40

Status: TODO
Title: AFK modal UI binding
Commit: Add AFK modal UI binding

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
