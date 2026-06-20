# Locked 25-Hour Scope

## Project

**Working title:** Idle Guild Demo

Build a small, polished Unity 2D idle RPG vertical slice inspired by early-game IdleOn.

The goal is to demonstrate:

1. Clean, data-driven Unity architecture
2. A recognizable early idle-RPG loop
3. Enough feature breadth to show judgment
4. A reviewer path that can be completed in under 30 minutes

This is not a full IdleOn clone. This scope is locked for a 25-hour build.

---

## Core Loop

```text
Town → Accept quest → Fight slime → Gain XP/loot → Mine copper → Craft gear → Level up → Choose class → Assign characters → Simulate AFK gains
```

The player manages a tiny roster of characters who can fight, gather resources, craft upgrades, level up, choose a class, and continue earning rewards while away.

---

## Must Ship

Only the features below are in scope.

### 1. Main Menu

Must include:

```text
New Game / Continue
Evaluation note: “Designed to show all major systems in under 30 minutes”
```

Expected result:

```text
Reviewer can start or continue a save.
```

### 2. Town Hub

One small town scene.

Must include:

```text
Quest NPC
Crafting station
Character panel
Inventory button
Portal/buttons to Combat and Mining
```

Expected result:

```text
Reviewer understands the town is the central hub.
```

### 3. Character Roster

Must include:

```text
Character 1 starts unlocked
Character 2 unlocks through quest progression
Each character has:
- Name
- Level
- XP
- Current task
- Class
- Talent points
```

Expected result:

```text
Reviewer can see that this is a roster-management idle game, not just one character.
```

### 4. Auto-Combat

Only one combat activity.

Must include:

```text
Enemy: Slime
Character auto-attacks
Enemy HP bar
Floating damage numbers
XP reward
Coin reward
Slime Goo drop
Respawn loop
```

Expected result:

```text
Reviewer can enter combat and see automatic progression without manual attacking.
```

### 5. Mining

Only one gathering activity.

Must include:

```text
Resource: Copper Ore
Mining progress bar
Automatic repeat gathering
Mining rate affected by stats/equipment/talents
```

Expected result:

```text
Reviewer can assign a character to mining and receive copper over time.
```

### 6. Inventory

Must include stackable items only.

Required items:

```text
Copper Ore
Copper Bar
Slime Goo
Copper Sword
Copper Pickaxe
Coins
```

Expected result:

```text
Reviewer can see earned items persist and get consumed by crafting.
```

### 7. Crafting

Must include exactly these recipes:

```text
3 Copper Ore → 1 Copper Bar
2 Copper Bar + 3 Slime Goo → 1 Copper Sword
2 Copper Bar → 1 Copper Pickaxe
```

Expected result:

```text
Reviewer can convert resources into useful upgrades.
```

### 8. Equipment Effects

Keep equipment simple.

Must include:

```text
Copper Sword increases combat damage
Copper Pickaxe increases mining speed or mining power
```

Expected result:

```text
Reviewer can feel that crafting improves progression.
```

### 9. Quest / Tutorial Chain

One linear quest chain.

Must include:

```text
Quest 1: Kill 5 Slimes
Quest 2: Collect 10 Copper Ore
Quest 3: Craft 3 Copper Bars
Quest 4: Craft Copper Sword
Quest 5: Reach Level 5
Quest 6: Choose a Class
Quest 7: Unlock Character 2
```

Expected result:

```text
Reviewer always knows what to do next.
```

### 10. Class Choice

Class unlocks at level 5.

Must include three choices:

```text
Warrior: +damage
Archer: +drop rate
Mage: +AFK reward rate
```

Expected result:

```text
Reviewer sees early IdleOn-style class specialization.
```

### 11. Talents

Small talent panel only.

Must include four talents:

```text
+Damage
+Mining Speed
+XP Gain
+AFK Gain
```

Expected result:

```text
Reviewer can spend talent points and see stat changes.
```

### 12. Offline / AFK Rewards

Must include:

```text
Each character has a current task
Save timestamp
On load, calculate offline progress
Cap offline progress at 8 hours
Show AFK results modal
Demo button: “Simulate 2 Hours AFK”
```

Expected result:

```text
Reviewer can immediately evaluate the idle/offline system without waiting.
```

### 13. Save / Load

Must include:

```text
JSON save file
Inventory persists
Character state persists
Quest progress persists
Class/talent choices persist
Last saved time persists
```

Expected result:

```text
Reviewer can quit/reopen and continue progress.
```

### 14. UI Polish

Must include:

```text
Quest tracker
XP bar
Inventory panel
Crafting panel
Character panel
Talent/class panel
Loot log
AFK results popup
Disabled craft buttons when materials are missing
Short toast messages for level up, quest complete, item crafted
```

Expected result:

```text
Reviewer can understand the game without explanation.
```

### 15. Submission Materials

Must include:

```text
GitHub repository
Runnable build or build link
Short video demo
README.md
CREDITS.md
Clean commit history
```

Expected result:

```text
Submission is complete and professional.
```

---

## Cut List

Do not build these.

```text
Multiplayer
Account system
Mobile-specific UI
Audio
Music
Sound effects
Bosses
Multiple worlds
Multiple towns
More than one combat enemy
More than one combat zone
More than one gathering resource
Chopping
Fishing
Alchemy
Cards
Stamps
Pets
Shops
NPC dialogue trees
Platforming physics
Complex enemy AI
Procedural generation
Skill minigames
Achievements
Daily rewards
Monetization
Cosmetics
Particle-heavy VFX
Complex animations
Controller support
Cloud save
Localization
Settings menu
```

If time remains, use it only for:

```text
Bug fixing
Balance
UI clarity
README
Video script
Build stability
```

No new gameplay systems.

---

## Reviewer Path

The game must guide the reviewer through this path in under 30 minutes.

### 0–3 Minutes

Expected experience:

```text
Main menu opens
Reviewer starts new game
Town loads
Quest tracker says: Kill 5 Slimes
Reviewer sees Combat button/portal
```

### 3–7 Minutes

Expected experience:

```text
Character auto-attacks Slime
Damage numbers appear
Slime HP decreases
Slime dies
XP, coins, and Slime Goo are awarded
Quest progress updates
```

### 7–11 Minutes

Expected experience:

```text
Quest reward is claimed
Next quest asks for Copper Ore
Reviewer enters Mining activity
Mining progress bar fills repeatedly
Copper Ore enters inventory
```

### 11–15 Minutes

Expected experience:

```text
Reviewer opens crafting panel
Copper Ore is converted into Copper Bars
Missing materials are clearly shown
Craft buttons enable/disable correctly
```

### 15–20 Minutes

Expected experience:

```text
Reviewer crafts Copper Sword
Combat damage improves
Quest progresses
Reviewer reaches or approaches level 5
```

### 20–24 Minutes

Expected experience:

```text
Level 5 unlocks class choice
Reviewer picks Warrior, Archer, or Mage
Stats update
Talent panel becomes meaningful
Reviewer spends talent points
```

### 24–27 Minutes

Expected experience:

```text
Quest chain unlocks Character 2
Reviewer sees two character cards
Each character can have a different current task
```

### 27–30 Minutes

Expected experience:

```text
Reviewer clicks “Simulate 2 Hours AFK”
AFK results modal appears
Rewards are shown per character
Inventory/XP/coins update
Save/load still works
```

---

## Non-Goals

### Not a clone

The project should be inspired by early-game IdleOn, but it must not attempt to recreate the full game.

### Not content-heavy

The demo should not rely on many enemies, zones, items, or quests. It should rely on a clear loop and polished systems.

### Not art-driven

The demo should use simple approved free 2D assets. Art quality should be coherent, but the main evaluation target is gameplay architecture and polish.

### Not animation-heavy

Simple idle/attack sprites are enough. UI feedback is more important than animation complexity.

### Not mechanically deep

Combat, gathering, and crafting should be simple, readable, and reliable.

### Not open-ended

The reviewer path should be guided by quests. The player should always know the next goal.

### Not expandable during the 25-hour build

Once implementation starts, the feature list is locked. Any extra idea goes to a parking lot, not into the build.

---

## Locked Success Criteria

The task is successful only if all of this is true:

```text
The game starts from a clean build
The reviewer can finish the intended path in under 30 minutes
Auto-combat works
Mining works
Crafting works
Quests work
Class choice works
Talents work
Second character unlocks
AFK simulation works
Save/load works
README and credits exist
No unapproved assets are used
No audio is included
Git history has meaningful commits
```

---

## Scope Lock Statement

```text
This project is a 25-hour Unity vertical slice inspired by early-game IdleOn. The scope is intentionally locked to one town hub, one combat enemy, one gathering resource, a short quest chain, simple crafting, class choice, talents, two characters, save/load, and AFK rewards. The goal is not to recreate IdleOn broadly, but to demonstrate a polished and reviewable version of its early idle-RPG loop within the assignment constraints.
```
