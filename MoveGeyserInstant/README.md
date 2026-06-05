MoveGeyserInstant

This mod allows instant moving of geysers across asteroids.

Usage:
- Select a geyser in-game and choose "Di chuyển mạch" from the user menu.
- Click to place. Press `Ctrl+Z` to undo the last move (best-effort).

Config (mods/Local/MoveGeyserInstant/config.json):
{
  "allowStacking": true,
  "verboseNotifications": true
}

Development:
- Working branch: `dev/move-geyser-instant`
- Build: `dotnet build "_source/MoveGeyserInstant/MoveGeyserInstant.csproj" -c Release`

Safety notes:
- The mod avoids copying UnityEngine.Object references and only copies value/string/enum fields.
- Preview is instantiated from the geyser prefab deactivated to avoid animation controller asserts.
