# Task 04 — Unity 2D Asset Import Checklist

Create a Unity 2D asset import checklist for pixel art spritesheets, tilesets, icons, and UI sprites.

## Goal

Make asset import repeatable and safe before importing approved third-party art into Unity.

## Required coverage

- Raw ZIP handling in `ExternalAssets/`
- Approved imported-pack placement under `Assets/ThirdParty/`
- Project art folders under `Assets/Art/`
- Pixel art import settings
- Pixels Per Unit policy
- Point filtering
- Compression and mip map rules
- Sprite Mode Single vs Multiple
- Sprite Editor slicing
- Grid By Cell Size for tiles/spritesheets
- Tile Palette creation
- Icon import checklist
- UI sprite import checklist
- 9-slice note for scalable UI panels/buttons
- Credit tracking in `CREDITS.md`
- Git commit checklist

## Constraints

- Do not import actual assets.
- Do not create Unity scripts.
- Do not expand gameplay scope.
- Keep the checklist aligned with `scope.md` and the approved asset shortlist.

## Expected result

A documentation file under `docs/` that can be followed during asset import without accidentally committing raw ZIPs, unapproved files, or badly configured pixel art.

## Acceptance test

- The checklist gives exact PPU guidance.
- The checklist gives exact filtering/compression/mip-map guidance.
- The checklist explains slicing for spritesheets and tilesets.
- The checklist explains folder placement.
- The checklist includes a verification step and commit command.
