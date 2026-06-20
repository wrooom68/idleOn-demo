# Credits

This file tracks all third-party assets considered or used in **Idle Guild Demo**.

The assignment allows only free 2D art assets from the approved sources below:

```text
https://itch.io/game-assets/free/tag-2d
https://assetstore.unity.com/?category=2d&free=true&orderBy=1
https://www.gamedevmarket.net/category/2d?orderby=most-popular&pricing=free
```

No audio assets are used.

---

## Asset Approval Checklist

Before adding any asset to `Assets/ThirdParty/`, confirm:

- [ ] The asset is free or name-your-own-price with $0 allowed
- [ ] The asset is 2D
- [ ] The asset comes from one of the three approved sources
- [ ] The license allows use in this take-home / portfolio project
- [ ] The creator name is recorded
- [ ] The source URL is recorded
- [ ] The asset is not a game template or previous work
- [ ] The asset does not include audio
- [ ] Only required files are imported into Unity
- [ ] Imported files are placed under `Assets/ThirdParty/<PackName_Creator>/`

---

## Recommended Asset Plan

Use as few packs as possible to avoid style mismatch and license confusion.

```text
Primary environment / player / props:
- FREE - Pixel Art Sidescroller Asset Pack 32x32 Overworld by GandalfHardcore

Primary slime enemy:
- Animated Slimes 16x16 px by Stealthix

Optional fallback for platformer tiles/items/placeholders:
- Pixel Adventure by Pixel Frog

Optional fallback for UI/resource icons only:
- Tiny Swords by Pixel Frog, Free Pack only
```

Do not import optional packs unless the primary packs are missing required visuals.

---

## Approved Shortlist — Not Yet Imported

These assets are approved candidates. Move an entry into the relevant category below once files are imported into Unity.

### FREE - Pixel Art Sidescroller Asset Pack 32x32 Overworld

```text
Creator: GandalfHardcore
Approved source: itch.io
Source URL: https://gandalfhardcore.itch.io/free-pixel-art-sidescroller-asset-pack-32x32-overworld
License: Custom permissive license on the itch.io page.
License notes:
- Commercial and non-commercial video games/projects allowed.
- Modification allowed.
- Reselling, repackaging, or redistributing the assets is prohibited.
- AI training, NFT, crypto/blockchain/web3 use is prohibited.
- Use in game development tools or printed materials is prohibited.
Use candidate for:
- Player character placeholder
- Town/combat/mine side-scroller environment
- Forest/rock tiles
- Backgrounds
- Portal
- Furnace/sawmill-style crafting props
- Ores
- HP bar
Files to import:
- GandalfHardcore FREE Platformer Assets.zip
- GandalfHardcore FREE Character Asset Pack.zip
- GandalfHardcore FREE Hp Bar.zip, only if needed
Status: Shortlisted, not imported yet
Notes: Strongest primary pack because it includes characters, tiles, props, ores, portal, HP bar, and backgrounds in one side-scroller style.
```

### Animated Slimes 16x16 px

```text
Creator: Stealthix
Approved source: itch.io
Source URL: https://stealthix.itch.io/animated-slimes
License: Creative Commons Zero v1.0 Universal (CC0)
License notes:
- Free to use however desired under CC0.
- Attribution appreciated but not required.
- Asset license field on itch.io lists Creative Commons Zero v1.0 Universal.
Use candidate for:
- Slime enemy
- Slime variants, if recolor is needed
Files to import:
- Slimes.zip
- Slimes Orange.zip, only if needed
Status: Shortlisted, not imported yet
Notes: Best match for locked scope because the game needs exactly one Slime enemy.
```

### Pixel Adventure

```text
Creator: Pixel Frog
Approved source: itch.io
Source URL: https://pixelfrog-assets.itch.io/pixel-adventure-1
License: Creative Commons Zero v1.0 Universal (CC0)
License notes:
- Commercial use allowed.
- Distribution, remixing, adaptation, and building upon the material are allowed.
- Attribution is not required.
Use candidate for:
- Optional fallback character
- Optional fallback tiles/items/placeholders
- Optional simple platformer props
Files to import:
- Pixel Adventure 1.zip, only if needed
Status: Shortlisted fallback, not imported yet
Notes: Use only if GandalfHardcore pack does not cover a needed visual. Do not use paid Pixel Adventure 2 because the assignment requires free assets.
```

### Tiny Swords — Free Pack Only

```text
Creator: Pixel Frog
Approved source: itch.io
Source URL: https://pixelfrog-assets.itch.io/tiny-swords
License: Custom permissive license on the itch.io page.
License notes:
- Personal and commercial projects allowed.
- Modification allowed.
- Crediting is not required but welcome.
- Redistributing, reselling, or repackaging assets is prohibited, even if modified.
Use candidate for:
- Optional UI icons
- Optional resource/class/weapon icons
- Optional buttons/live bars if the main packs are insufficient
Files to import:
- Free Pack assets only
Status: Shortlisted fallback, not imported yet
Notes: Do not import the paid Enemy Pack. Use only if UI/item icons are missing from the primary packs.
```

---

## Character Art

### Imported Asset Pack Name

```text
Creator:
Approved source: itch.io / Unity Asset Store / GameDev Market
Source URL:
License:
Used for:
Files imported:
Notes:
```

---

## Enemy Art

### Imported Asset Pack Name

```text
Creator:
Approved source: itch.io / Unity Asset Store / GameDev Market
Source URL:
License:
Used for:
Files imported:
Notes:
```

---

## Environment / Tileset Art

### Imported Asset Pack Name

```text
Creator:
Approved source: itch.io / Unity Asset Store / GameDev Market
Source URL:
License:
Used for:
Files imported:
Notes:
```

---

## Item Icons

### Imported Asset Pack Name

```text
Creator:
Approved source: itch.io / Unity Asset Store / GameDev Market
Source URL:
License:
Used for:
Files imported:
Notes:
```

---

## UI Art

### Imported Asset Pack Name

```text
Creator:
Approved source: itch.io / Unity Asset Store / GameDev Market
Source URL:
License:
Used for:
Files imported:
Notes:
```

---

## Fonts

Use Unity/TextMesh Pro default fonts unless a font is explicitly sourced from an approved source and documented here.

### Font Name

```text
Creator:
Approved source:
Source URL:
License:
Used for:
Files imported:
Notes:
```

---

## Rejected / Avoided Assets

Track rejected assets here so they do not accidentally enter the project.

| Asset | Source | Reason Rejected / Avoided |
|---|---|---|
| Pixel Adventure 2 | itch.io | Paid $5 minimum, so not allowed under the free-asset constraint. |
| Basic Pixel Health bar and Scroll bar | itch.io | Useful UI pack, but license text says non-commercial games are free and commercial use asks for contribution. Avoid unless absolutely needed. |
| Random Figma Community assets | Figma | Not one of the assignment-approved asset sources unless independently traced back to itch.io, Unity Asset Store, or GameDev Market and licensed. |
| Audio from any pack | Any source | Assignment says not to include audio. |
