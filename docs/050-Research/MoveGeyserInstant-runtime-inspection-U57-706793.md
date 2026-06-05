# Runtime Inspection: MoveGeyserInstant on ONI U57-706793-SCRPN

Date captured: 2026-06-04

Game runtime DLLs inspected:

- `D:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll`
- `D:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll`

Mod DLL inspected:

- `D:\Documents\Klei\OxygenNotIncluded\mods\Local\MoveGeyserInstant\MoveGeyserInstant.dll`

Tool used:

- Mono.Cecil temp inspector in `%TEMP%\oni-runtime-inspect-cecil`
- Mono.Cecil focused decompile dump in `%TEMP%\oni-runtime-decompile-check`
- Important: this inspection used the real runtime DLLs, not publicized/reference DLLs from `_source\sgt_imalas_source\Lib`.

Raw focused dump saved for reuse:

- `docs\MoveGeyserInstant-runtime-decompile-U57-706793.txt`

## Access Check Results

No non-public field references were found in `MoveGeyserInstant.dll`.

All direct game field refs in the current DLL are public:

```text
public FIELD CellEventLogger::DebugTool
public FIELD CellEventLogger::Instance
public FIELD Components::Geysers
public FIELD Element::id
public FIELD Game::userMenu
public FIELD Grid::Element
public FIELD Grid::InvalidCell
public FIELD Grid::Objects
public FIELD Grid::Solid
public FIELD HoverTextConfiguration::ActionName
public FIELD HoverTextConfiguration::ToolName
public FIELD KAnimControllerBase::defaultAnim
public FIELD KAnimControllerBase::initialAnim
public FIELD KAnimControllerBase::initialMode
public FIELD KAnimControllerBase::visibilityType
public FIELD PlayerController::tools
public FIELD PopFXManager::Instance
public FIELD PopFXManager::sprite_Plus
public FIELD Vector2I::x
public FIELD Vector2I::y
```

Non-public method refs reported by Cecil are inherited/protected base overrides, not publicized-only field access:

```text
family METHOD InterfaceTool::OnDeactivateTool(InterfaceTool)
family METHOD KMonoBehaviour::OnPrefabInit()
family METHOD KMonoBehaviour::OnSpawn()
```

These are expected when overriding lifecycle methods from derived classes.

## Important Runtime Type Facts

`InterfaceTool` runtime fields:

```text
private HoverTextConfiguration hoverTextConfiguration
private KSelectable hoverOverride
public KSelectable hover
```

Do not access `InterfaceTool.hoverTextConfiguration` directly. It is private in the real runtime DLL and caused:

```text
System.FieldAccessException: Field `InterfaceTool:hoverTextConfiguration' is inaccessible
```

`HoverTextConfiguration` runtime API used by the mod:

```text
public FIELD/MEMBER ToolName
public FIELD/MEMBER ActionName
public METHOD UpdateHoverElements(List<KSelectable> hover_objects)
```

`KBatchedAnimController` runtime API relevant to preview:

```text
public METHOD LoadAnims()
public METHOD SwapAnims(KAnimFile[] anims)
public METHOD SetSceneLayer(Grid.SceneLayer layer)
public METHOD SetSymbolTint(KAnimHashedString symbol_name, Color color)
public FIELD setScaleFromAnim
```

`KAnimControllerBase` runtime API relevant to preview:

```text
public FIELD initialAnim
public FIELD defaultAnim
public FIELD visibilityType
public FIELD initialMode
public PROPERTY TintColour
public PROPERTY currentAnim
public PROPERTY AnimFiles
public METHOD Play(HashedString anim_name, KAnim.PlayMode mode, float speed, float time_offset)
public METHOD Play(HashedString[] anim_names, KAnim.PlayMode mode)
public METHOD HasAnimation(HashedString anim_name)
public METHOD GetCurrentAnim()
public METHOD GetAnim(HashedString anim_name)
```

`Util` runtime instantiate overload available:

```text
public GameObject KInstantiate(GameObject original, Vector3 position)
public GameObject KInstantiate(GameObject original, Vector3 position, Quaternion rotation, GameObject parent, string name, bool initialize_id, int gameLayer)
```

`PlayerController.ActivateTool(InterfaceTool tool)` is not null-safe in this runtime build:

```text
activeTool = tool
activeTool.enabled = true
activeTool.gameObject.SetActive(true)
activeTool.ActivateTool()
```

Do not call `PlayerController.Instance.ActivateTool(null)`. Use `SelectTool.Instance` or a non-null fallback from `PlayerController.tools`.

`HoverTextScreen.Update()` is also not safe if `PlayerController.ActiveTool` is null:

```text
PlayerController.Instance.ActiveTool.ShowHoverUI()
```

This means setting the active tool to null can surface as a later `HoverTextScreen.Update()` `NullReferenceException`, especially after placing/canceling a custom tool.

## Known Bad Assumptions Already Fixed

- `DragTool.boxCursor` is publicized-only for this mod context; direct access caused runtime `FieldAccessException`.
- `InterfaceTool.hoverTextConfiguration` is private in the real runtime DLL; direct access caused runtime `FieldAccessException`.
- Current source must not reference either `boxCursor` or `hoverTextConfiguration`.
- `PlayerController.ActivateTool(null)` is invalid on U57-706793; it can crash immediately or leave `ActiveTool` null for `HoverTextScreen.Update()`.

## Remaining Runtime-Risk Areas

These are public in runtime DLL, but still need in-game validation:

- Creating a render-only `KBatchedAnimController` from copied `AnimFiles`.
- Using source `KAnimControllerBase.AnimFiles/currentAnim/initialAnim/defaultAnim` for preview.
- Reflection-copying all fields from `Geyser` and `GeyserConfigurator`; access is via reflection and wrapped, but copied values may still be semantically unsafe.
- Cross-world placement and source deletion after target spawn.
- Sim element changes with `SimMessages.ReplaceElement` while placing on another asteroid.

## 2026-06-04 Follow-Up Fix

Confirmed by the real U57 runtime decompile:

- `PlayerController.ActivateTool(null)` is invalid.
- `HoverTextScreen.Update()` assumes `PlayerController.ActiveTool` is non-null.
- The mod was still calling `ActivateTool(null)` after Escape, right click cancel, and successful placement.

Source fix applied:

- Replaced all `ActivateTool(null)` calls with `ActivateDefaultTool()`.
- `ActivateDefaultTool()` uses `SelectTool.Instance` first, then falls back to the first non-null, non-`MoveGeyserTool` entry in `PlayerController.tools`.
- Moved geyser field restore before `moved.SetActive(true)` so `Geyser.OnSpawn()` sees the captured `configuration` and `serializedTimeShift` instead of generating/applying a fresh default first.
- Kept `GameHashes.CopySettings` trigger after activation.
- Rebuilt `MoveGeyserInstant.dll` successfully after the fix.

Verification after rebuild:

- `rg "ActivateTool\(null\)|hoverTextConfiguration|boxCursor|CopySettingsTool|DragTool" _source\MoveGeyserInstant MoveGeyserInstant -n` returned no matches.
- Focused runtime reference check still reports no non-public field refs in `MoveGeyserInstant.dll`.
