# First Commit Checklist

Use this checklist before starting the 25-hour implementation clock.

The goal is to make the repository safe, reviewable, and ready for Unity/Codex work.

---

## Repository Hygiene

- [ ] Repository exists on GitHub
- [ ] Default branch is `main`
- [ ] `scope.md` exists
- [ ] `README.md` exists
- [ ] `CREDITS.md` exists
- [ ] `.gitignore` exists
- [ ] `docs/folder-conventions.md` exists
- [ ] No generated Unity folders are committed
- [ ] Commit messages are meaningful

---

## Unity Project Settings

After the Unity project is created, confirm:

- [ ] Unity project opens successfully
- [ ] Project uses 2D template or 2D project setup
- [ ] `Assets/`, `Packages/`, and `ProjectSettings/` are committed
- [ ] `Library/`, `Temp/`, `Obj/`, `Build/`, `Builds/`, and `Logs/` are not committed
- [ ] Visible Meta Files are enabled
- [ ] Asset Serialization is set to Force Text
- [ ] Main scenes are created or planned:
  - [ ] `MainMenu.unity`
  - [ ] `Town.unity`
  - [ ] `CombatZone.unity`
  - [ ] `MineZone.unity`

---

## Folder Structure

Create this structure inside Unity:

```text
Assets/
  Art/
    Characters/
    Enemies/
    Environment/
    Items/
    UI/
  Data/
    Items/
    Enemies/
    Recipes/
    Quests/
    Classes/
    Talents/
    Zones/
  Prefabs/
    Characters/
    Enemies/
    Environment/
    Interactables/
    UI/
  Scenes/
  Scripts/
    Core/
    Data/
    Runtime/
    Systems/
    Save/
    UI/
  ThirdParty/
  Resources/
```

Confirm:

- [ ] Folder names match `docs/folder-conventions.md`
- [ ] No random scripts at `Assets/Scripts/` root unless temporary
- [ ] Third-party assets are separated from project-created assets

---

## Asset Compliance

Before importing third-party art:

- [ ] Asset source is one of the approved assignment sources
- [ ] Asset is free
- [ ] Asset is 2D
- [ ] License allows this use
- [ ] Asset is documented in `CREDITS.md`
- [ ] No audio files are imported
- [ ] No templates or prior work are imported

Approved sources:

```text
itch.io free 2D game assets
Unity Asset Store free 2D assets
GameDev Market free 2D assets
```

---

## Codex / Cursor Readiness

- [ ] Project opens in Cursor
- [ ] Codex can read the repo
- [ ] `prompts/` folder exists or is planned
- [ ] Each Codex task is scoped to one subsystem
- [ ] Codex is instructed not to add features outside `scope.md`

Recommended first prompt file:

```text
prompts/task_02_repo_hygiene.md
```

---

## First Real Implementation Commit

The first implementation commit should happen only after the hygiene files are present.

Suggested command sequence locally:

```bash
git status
git add README.md CREDITS.md .gitignore scope.md docs/folder-conventions.md docs/first-commit-checklist.md
git commit -m "Add repository hygiene documentation"
git push origin main
```

If these files were created through GitHub directly, pull the latest repo before creating the Unity project locally:

```bash
git pull origin main
```

---

## Definition of Done for Task 02

Task 02 is complete when:

```text
README skeleton exists
CREDITS skeleton exists
Unity .gitignore exists
Folder conventions doc exists
First commit checklist exists
All files are pushed to GitHub
The repo is ready for Unity project creation
```
