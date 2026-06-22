# Project Overview
- Game Title: idleOn-demo
- High-Level Concept: An Idle MMO/RPG inspired incremental game where players assign characters to activities like combat and mining, view progression, and interact with the town hub.
- Players: Single player, incremental AFK progression.
- Inspiration / Reference Games: IdleOn, Melvor Idle, Runescape.
- Tone / Art Direction: Retro 2D pixel art style.
- Target Platform: WebGL, PC.
- Screen Orientation / Resolution: Landscape 1920x1080.
- Render Pipeline: Built-in Render Pipeline.

# Game Mechanics
## Core Gameplay Loop
The player selects activities (e.g. Fight Slimes in CombatZone, Mine Copper in MineZone), which progress automatically via timer ticks or background simulation (AFK). They collect resources (Copper Ore, Slime Goo, Coins), craft gear (Copper Bar, Copper Sword, Copper Pickaxe), spend talents to gain passive boosts, and fulfill quests.

## Controls and Input Methods
- Legacy input system with standard uGUI buttons for menu and panel navigation.
- **New Feature**: 2D Platformer-style world exploration. Players can move the character left/right using A/D or Arrow keys, and jump using Space.
- Fixed/clamped camera system with simple horizontal boundaries to prevent falling off screen.

# UI
The existing Canvas HUD overlays on top of the screen in Screen Space - Overlay.
- The UI panels (Inventory, Crafting, Talents, AFK Reward summaries, Quest tracker) will remain perfectly intact and clickable.
- Interactive buttons previously nested under the semi-transparent Figma boxes will remain active and fully functional, but the underlying solid visual placeholders will be disabled so the 2D world-space platformer environment is visible behind them.

# Key Asset & Context
### 1. PlayerMovement2D.cs
A new script to be added to `Assets/Scripts/Runtime/PlayerMovement2D.cs` that handles:
- Rigidbody2D movement using `Input.GetAxisRaw("Horizontal")`.
- Sprite flipping based on direction.
- Simple contact-normal-based ground check (checking contact normals with slope > 0.7) to allow jumping with Space.
- Animator state calls (`animator.Play("Idle")` or `animator.Play("Run")`) depending on velocity.

### 2. Scene Visual Assets
- **Town**: Ground platforms with a deep brown dirt look and green grass top layer, `NPC1.png` for Quest NPC, `Pixel Art Furnace and Sawmill.png` for Crafting Station, `Chest.png` for Storage, `portal/On (38x38).png` with PortalAnimator for the Combat Portal, and `Ores_0` (copper ore) for the Mining Portal.
- **CombatZone**: Ground platforms with grass top, `Slime_Medium_Blue.png` with `SlimeBlueAnimator` for the enemy slime.
- **MineZone**: Ground platforms with stone/cave textures (`Color(0.18, 0.18, 0.2)`), `Ores_0` (copper ore node) for mining.

# Implementation Steps
### Step 1: Create PlayerMovement2D script
- **Description**: Add the movement C# script to `Assets/Scripts/Runtime/PlayerMovement2D.cs` supporting basic physics-based A/D horizontal walking, Space jumping, and sprite flipping.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

### Step 2: Implement Environment Setup Script & Configure Scenes
- **Description**: Implement an editor script `ApplyPlatformerEnvironments.cs` and run it to construct the platformer environments for `Town.unity`, `CombatZone.unity`, and `MineZone.unity`.
  - Disable visual mockup placeholders in Canvas (hide standard image components on `WorldArea_Town`, `Image_QuestNPC`, etc.).
  - Set up orthographic camera viewing the world and UI.
  - Create a world container named `WorldRoot`.
  - Create `Ground` and platforms with `BoxCollider2D` and boundary walls.
  - Settle character world objects with `BoxCollider2D`, `Rigidbody2D`, `Animator`, and `PlayerMovement2D`.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No

### Step 3: Run Validation & Final Scene Layout Cleanup
- **Description**: Open each scene in Editor, execute final adjustments, and verify player-world interactions, collision bounds, and clickability of overlay buttons.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No

# Verification & Testing
1. **Movement Test**: Play each scene. Verify player walks left/right with A/D or arrows, and jumps with Space.
2. **Boundary Test**: Walk to extreme left/right and ensure colliders prevent the player from falling into the void.
3. **UI Overlay Test**: Open Inventory, Crafting, Talents panels and confirm buttons are fully responsive.
4. **Activity Test**: Confirm clicking "Attack Tick" and "Mine Tick" continues to advance combat/mining progress.
5. **Navigational Test**: Verify that MainMenu → Town → Combat/Mine → Town navigation functions smoothly without script failures.
6. **Console Check**: No red compilation or runtime errors.
