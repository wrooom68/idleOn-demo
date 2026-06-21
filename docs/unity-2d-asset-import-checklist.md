# Unity 2D Asset Import Checklist

Use this checklist when importing pixel art spritesheets, tilesets, icons, and UI sprites for **Idle Guild Demo**.

The goal is to keep all 2D assets crisp, organized, credited, and safe for the assignment requirements.

---

## 0. Rules Before Importing

Raw downloads stay local and ignored by Git.

```text
ExternalAssets/
```

Only Unity-ready selected files should be copied into the project.

```text
Assets/ThirdParty/<PackName_Creator>/
```

Do not import entire ZIP files into Unity.

Do not import:

```text
Audio files
Paid assets
Game templates
Unapproved sources
Random Figma Community assets
Unused asset-pack folders
```

Every imported asset pack must be recorded in:

```text
CREDITS.md
```

---

## 1. Folder Placement

### Raw ZIPs and extracted source files

Keep outside Unity project import paths:

```text
ExternalAssets/
  GandalfHardcore/
    original_zip_files/
    extracted/
  Stealthix/
    original_zip_files/
    extracted/
  PixelFrog/
    original_zip_files/
    extracted/
```

`ExternalAssets/` is ignored by Git.

### Approved imported packs

Copy only selected files into:

```text
Assets/ThirdParty/GandalfHardcore_FreeSidescroller32/
Assets/ThirdParty/Stealthix_AnimatedSlimes/
Assets/ThirdParty/PixelFrog_PixelAdventure1/        # optional fallback only
Assets/ThirdParty/PixelFrog_TinySwords_FreePack/   # optional fallback only
```

### Project-organized art references

Use these folders for cleaned/copied sprites that are actively used by scenes or prefabs:

```text
Assets/Art/Characters/
Assets/Art/Enemies/
Assets/Art/Environment/
Assets/Art/Items/
Assets/Art/UI/
```

### Tile assets and palettes

Use:

```text
Assets/Data/Zones/Tiles/
Assets/Data/Zones/Palettes/
```

Create these folders in Unity if needed.

---

## 2. Default Pixel Art Import Settings

For each imported PNG/spritesheet, select the texture in Unity and set:

```text
Texture Type: Sprite (2D and UI)
Sprite Mode: Single or Multiple, depending on asset type
Pixels Per Unit: see asset-specific table below
Mesh Type: Full Rect for tiles/UI; Tight is okay for standalone character/enemy sprites
Generate Physics Shape: Off unless specifically needed
sRGB (Color Texture): On
Alpha Source: Input Texture Alpha
Alpha Is Transparency: On for transparent sprites
Read/Write: Off unless a script needs pixel data
Generate Mip Maps: Off for pixel art
Filter Mode: Point (no filter)
Compression: None
```

Why:

```text
Point filtering keeps pixel art crisp.
Mip maps and compression can blur small pixel sprites.
Sprite Mode Multiple is required for spritesheets and tilesheets.
```

---

## 3. Pixels Per Unit Policy

Use PPU intentionally. Do not leave mixed values without a reason.

| Asset Type | Recommended PPU | Notes |
|---|---:|---|
| GandalfHardcore 32x32 world tiles | 32 | 32 px = 1 Unity unit / 1 Tilemap cell |
| GandalfHardcore 32x32 player/props | 32 | Keeps player and props aligned with tiles |
| Stealthix 16x16 slime | 16 | 16 px = 1 Unity unit; use 32 if you want the slime half-tile sized |
| Pixel Frog 16x16 fallback sprites | 16 | Use only if imported |
| Item icons | 32 or source-native size | Keep consistent per icon sheet |
| UI sprites/panels | 100 or source-native size | PPU matters less in Canvas UI, but keep settings consistent |

Project default recommendation:

```text
World grid: 32 PPU
Tilemap cell size: 1 x 1
Most environment art: 32 PPU
16x16 enemies: 16 PPU if they should appear 1 tile tall
```

If a sprite looks too large or too small, fix the PPU before scaling lots of instances manually.

---

## 4. Spritesheet Import Checklist

Use for:

```text
Character animation sheets
Enemy animation sheets
Multi-icon sheets
UI sprite sheets
```

Checklist:

- [ ] File is from an approved asset pack
- [ ] File is copied under `Assets/ThirdParty/<PackName_Creator>/`
- [ ] Texture Type = `Sprite (2D and UI)`
- [ ] Sprite Mode = `Multiple`
- [ ] PPU follows the table above
- [ ] Filter Mode = `Point (no filter)`
- [ ] Compression = `None`
- [ ] Generate Mip Maps = `Off`
- [ ] Alpha Is Transparency = `On`
- [ ] Sprite Editor opens without package errors
- [ ] Sheet is sliced correctly
- [ ] Sliced sprites have clear names
- [ ] Animation frames are not accidentally skipped
- [ ] `.meta` file is generated and committed

Recommended slicing:

```text
Sprite Editor → Slice → Grid By Cell Size
```

Use:

```text
Cell Size: 16x16 for 16px sheets
Cell Size: 32x32 for 32px sheets
Offset: 0,0 unless the sheet has a margin
Padding: 0,0 unless the sheet has spacing between frames
Pivot: Bottom Center for characters/enemies, Center for icons/UI
Method: Delete Existing for first import; Smart/Safe when updating an already-used sheet
```

For irregular sheets:

```text
Use Automatic slicing only if sprites are separated by transparency.
Manually verify every SpriteRect.
```

---

## 5. Tileset Import Checklist

Use for:

```text
Ground tiles
Town tiles
Mine tiles
Forest/rock tiles
Background tile pieces
```

Checklist:

- [ ] File is from an approved asset pack
- [ ] File is copied under `Assets/ThirdParty/<PackName_Creator>/`
- [ ] Texture Type = `Sprite (2D and UI)`
- [ ] Sprite Mode = `Multiple`
- [ ] PPU = `32` for 32x32 tiles, or native tile size if different
- [ ] Filter Mode = `Point (no filter)`
- [ ] Compression = `None`
- [ ] Generate Mip Maps = `Off`
- [ ] Alpha Is Transparency = `On`, if transparent
- [ ] Sprite Editor slicing uses exact tile cell size
- [ ] Tiles align cleanly on a 1x1 Tilemap grid
- [ ] Tile Palette is created only after slicing is correct
- [ ] Tile assets are saved under `Assets/Data/Zones/Tiles/`
- [ ] Tile palettes are saved under `Assets/Data/Zones/Palettes/`

Recommended tile slicing:

```text
Sprite Editor → Slice → Grid By Cell Size → 32x32
Pivot: Center
Method: Delete Existing for first import
```

Create palette:

```text
Assets → Create → 2D → Tile Palette
```

Then:

```text
Window → 2D → Tile Palette
Drag sliced sprites or texture into the Tile Palette window
Save generated Tile assets under Assets/Data/Zones/Tiles/
```

Tilemap scene layering should use:

```text
TM_Background
TM_DecorBehind
TM_Ground
TM_Collision
TM_DecorFront
```

---

## 6. Item Icon Import Checklist

Use for:

```text
Copper Ore
Copper Bar
Slime Goo
Copper Sword
Copper Pickaxe
Coin icon
```

Checklist:

- [ ] Icon source is approved and recorded in `CREDITS.md`
- [ ] Texture Type = `Sprite (2D and UI)`
- [ ] Sprite Mode = `Single` for individual icon files
- [ ] Sprite Mode = `Multiple` for icon sheets
- [ ] PPU = `32` or source-native icon size
- [ ] Filter Mode = `Point (no filter)`
- [ ] Compression = `None`
- [ ] Generate Mip Maps = `Off`
- [ ] Alpha Is Transparency = `On`
- [ ] Pivot = `Center`
- [ ] Icon looks crisp in Inventory UI at intended scale
- [ ] Icon is assigned to an `ItemDefinition` later

Folder placement:

```text
Assets/Art/Items/
```

or, if keeping original pack structure:

```text
Assets/ThirdParty/<PackName_Creator>/Items/
```

---

## 7. UI Sprite Import Checklist

Use for:

```text
Panels
Buttons
Frames
Progress bars
HP bars
Inventory slots
Talent nodes
Quest tracker background
```

Checklist:

- [ ] UI source is approved and recorded in `CREDITS.md`
- [ ] Texture Type = `Sprite (2D and UI)`
- [ ] Sprite Mode = `Single` or `Multiple` as appropriate
- [ ] PPU = `100` or source-native size, consistently per UI pack
- [ ] Filter Mode = `Point (no filter)` for pixel UI
- [ ] Compression = `None`
- [ ] Generate Mip Maps = `Off`
- [ ] Alpha Is Transparency = `On`
- [ ] Pivot = `Center`
- [ ] For scalable panels/buttons, configure Sprite Editor borders for 9-slicing
- [ ] Test at 1280x720 and 1920x1080 Canvas resolutions

For 9-sliced UI:

```text
Sprite Editor → set Border values
UI Image Type → Sliced
```

Do not overbuild UI art. Text clarity and layout are more important than decorative frames.

---

## 8. Animation Clip Checklist

Use only for minimal feedback:

```text
Player idle
Player attack, optional
Slime idle
Slime hit/death, optional
```

Checklist:

- [ ] Spritesheet is sliced first
- [ ] Frames are in correct order
- [ ] Animation clip is saved under `Assets/Art/Characters/Animations/` or `Assets/Art/Enemies/Animations/`
- [ ] Animator Controller is saved with the prefab if needed
- [ ] Clip speed is simple and readable
- [ ] Missing animation does not block gameplay

Recommended:

```text
Idle animation: 4–8 FPS
Attack animation: 8–12 FPS
```

If time is tight, use static sprites plus floating damage numbers instead of building many animations.

---

## 9. Import Verification Checklist

After import, check in Unity:

- [ ] Sprites are crisp, not blurry
- [ ] No dark/white outlines from bad transparency settings
- [ ] Tiles align without gaps
- [ ] Tile palette paints correctly
- [ ] Character and slime are visually proportional
- [ ] Icons are readable in inventory slots
- [ ] UI sprites scale correctly
- [ ] No audio files were imported
- [ ] No paid/unused pack folders were imported
- [ ] `CREDITS.md` has entries for every used pack
- [ ] `.meta` files exist
- [ ] Git status does not include `ExternalAssets/`

Run:

```bash
git status
```

Expected:

```text
ExternalAssets/ is not listed
Assets/ThirdParty/ selected files are listed
Unity .meta files are listed
```

---

## 10. Commit Checklist

Before committing imported assets:

```bash
git status
```

Confirm only selected Unity-ready files are staged:

```bash
git add Assets/ThirdParty Assets/Art Assets/Data/Zones CREDITS.md
git status
git commit -m "Import approved 2D asset shortlist"
git push origin main
```

Do not commit:

```text
ExternalAssets/
Raw ZIP files
Unused extracted folders
Audio files
Paid fallback assets
Temporary screenshots
```

---

## 11. Fast Import Order for This Project

Import in this order:

```text
1. GandalfHardcore environment / tiles
2. GandalfHardcore character
3. Stealthix slime
4. Item icons needed for Copper Ore, Copper Bar, Slime Goo, Sword, Pickaxe, Coin
5. Minimal UI sprites / HP bar
```

Stop once the locked scope has enough visuals.

Do not keep searching for better art after the game is visually understandable.

---

## 12. Definition of Done

Task 04 is complete when:

```text
Import checklist exists in docs/
Raw ZIPs remain outside Git
Selected assets have clear destination folders
PPU/filtering/slicing rules are defined
Tileset/import workflow is defined
Icon/UI workflow is defined
Credits workflow is defined
Commit workflow is defined
```
