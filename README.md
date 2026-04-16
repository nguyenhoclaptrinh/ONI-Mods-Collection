# ONI Mods Collection (Build 679336+)

A collection of maintained and modernized mods for Oxygen Not Included. Focused on quality-of-life and localization stability.

## Mods Included

### Customizable Speed
Modernized fork of the classic speed control mod.
* **Tech**: Updated to SDK-style project format (`.csproj`).
* **Dependency**: Linked against **PLib 4.19.0**.
* **Fixes**: Corrected `POptions` registration and modernized Harmony patching for the July 2024 game engine update.

### TiengViet (Vietnamese Translation)
Comprehensive technical repair of the 2024 translation series.
* **Syntax**: Fixed 1,000+ broken `<link>` and `<style>` tags causing game log bloating.
* **Logic**: Corrected semantic signal color mismatches (Red/Green logic state reversals).
* **Parameters**: Restored missing `{0}` and `{Hotkey}` placeholders for runtime data display.

## Installation
1. Download repository.
2. Place folders in: `%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\Local\`
3. Restart game and enable in Mod menu.

## Development / Building
To build `CustomizableSpeed.dll`:
1. Ensure you have the .NET SDK installed.
2. Run `dotnet build -c Release` in the mod directory.
3. Output will be generated in the root of the mod folder.

## Credits
* **Customizable Speed**: Original by [Pholith](https://github.com/Pholith/ONI-Mods).
* **TiengViet**: Original by [Chuot Chanel (Nguyễn Thái Bảo)](https://steamcommunity.com/workshop/browse/?appid=457140&searchtext=TiengViet).
* **Maintained by**: [nguyenhoclaptrinh](https://github.com/nguyenhoclaptrinh).

---
*Distributed for personal and community use. Open an issue for bug reports related to these specific modernized versions.*
