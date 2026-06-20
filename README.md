# Idle Guild Demo

A compact single-player Unity 2D idle RPG vertical slice inspired by early-game IdleOn.

This project is built as a 25-hour take-home assignment. The focus is a small, polished, reviewable slice of the early idle-RPG loop: roster management, auto-combat, mining, crafting, quests, class choice, talents, save/load, and offline/AFK rewards.

---

## Playable Build

> Add final build link here.

```text
Build target: WebGL or Windows
Status: Not uploaded yet
```

---

## Video Demo

> Add short demo video link here.

```text
Target length: 3–5 minutes
Status: Not recorded yet
```

---

## Scope

The locked build scope is documented in [`scope.md`](scope.md).

Summary:

```text
One town hub
One slime combat activity
One copper mining activity
Stackable inventory
Three crafting recipes
Class choice at level 5
Four talents
Seven-step quest chain
Two characters
JSON save/load
Offline/AFK rewards
Basic UI polish
```

---

## Reviewer Path

The demo is tuned so the reviewer can see all major systems in under 30 minutes:

```text
Start game → Town → Quest → Combat → Loot/XP → Mining → Crafting → Level 5 → Class choice → Talents → Character 2 → AFK simulation
```

---

## Features

### Must Ship

- [ ] Main menu with New Game / Continue
- [ ] Town hub
- [ ] Character roster with two characters
- [ ] Auto-combat against Slime
- [ ] Copper mining
- [ ] Stackable inventory
- [ ] Crafting recipes
- [ ] Equipment effects
- [ ] Linear quest/tutorial chain
- [ ] Class choice at level 5
- [ ] Talent panel
- [ ] Offline/AFK reward simulation
- [ ] JSON save/load
- [ ] Basic UI polish

### Explicitly Out of Scope

- Multiplayer
- Audio
- Bosses
- Multiple worlds
- Multiple combat zones
- Multiple gathering resources
- Shops
- Cards
- Stamps
- Alchemy
- Pets
- Cloud save

---

## Architecture Plan

The project should stay data-driven and easy to review.

```text
ScriptableObjects:
- ItemDefinition
- EnemyDefinition
- RecipeDefinition
- QuestDefinition
- ClassDefinition
- TalentDefinition
- ZoneDefinition

Runtime State:
- SaveData
- PlayerProfile
- CharacterState
- InventoryState
- QuestProgressState
- TalentState

Systems:
- InventorySystem
- ProgressionSystem
- CombatSystem
- GatheringSystem
- CraftingSystem
- QuestSystem
- OfflineProgressionSystem
- SaveSystem

UI:
- Thin UI controllers only
- Gameplay logic stays in Systems
```

---

## Project Structure

See [`docs/folder-conventions.md`](docs/folder-conventions.md).

Expected Unity folders:

```text
Assets/
  Art/
  Data/
  Prefabs/
  Scenes/
  Scripts/
  ThirdParty/
  Resources/
```

---

## Asset Rules

All third-party 2D assets must come from the assignment-approved sources only:

```text
https://itch.io/game-assets/free/tag-2d
https://assetstore.unity.com/?category=2d&free=true&orderBy=1
https://www.gamedevmarket.net/category/2d?orderby=most-popular&pricing=free
```

All assets must be listed in [`CREDITS.md`](CREDITS.md).

No audio is included for this assignment.

---

## Development Workflow

```text
1. Work in small 30-minute tasks.
2. Use meaningful commits.
3. Keep gameplay logic out of UI scripts.
4. Test in Unity after each system-level change.
5. Do not expand scope without updating scope.md.
```

---

## Build Instructions

> Fill this in once the Unity project is created.

```text
Unity version:
Build target:
Scene order:
Build output path:
```

---

## Submission Checklist

- [ ] GitHub repo is public or Tier9GameStudios is added as collaborator
- [ ] Runnable build link is available
- [ ] Short video demo link is available
- [ ] README is complete
- [ ] CREDITS is complete
- [ ] Game is playable in under 30 minutes
- [ ] No unapproved assets are used
- [ ] No audio is included
- [ ] Meaningful commit history exists
