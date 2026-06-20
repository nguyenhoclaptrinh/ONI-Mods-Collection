---
id: copy-building-settings-usages-raw
type: research-output
status: done
created: 2026-06-20
source: grep_search("CopyBuildingSettings", "_source/", "*.cs")
---

# Raw Output: CopyBuildingSettings usages trong _source/

## Mục đích
Tìm tất cả nơi dùng `CopyBuildingSettings` trong các mod nguồn để hiểu pattern.

## Kết quả (raw grep output)

```
_source\AzeTheGreat_source\src\SuppressNotifications\UI\Buildings\BuildingSuppressionButton.cs:5
  [MyCmpAdd] private CopyBuildingSettings copyBuildingSettings;

_source\ONI-Mods\src\ParticleCollider\ParticleColliderConfig.cs:102
  go.AddOrGet<CopyBuildingSettings>();

_source\aki_art_source\Backwalls\Buildings\DecorativeBackwallConfig.cs:25
  go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;

_source\aki_art_source\Backwalls\Buildings\SealedBackwallConfig.cs:25
  go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;

_source\aki_art_source\DecorPackA\Buildings\MoodLamp\MoodLampConfig.cs:62
  go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;

_source\aki_art_source\GravitasBigStorage\Content\GravitasBigStorageConfig.cs:53
  go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;

_source\peterhan_source\AirlockDoor\AirlockDoorConfig.cs:122
  go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;

_source\peterhan_source\AirlockDoor\AirlockDoorInsulatedConfig.cs:119
  go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;

_source\peterhan_source\ToastControl\ToastControlPatches.cs:95
  "CopyBuildingSettings:ApplyCopy",

_source\peterhan_source\ToastControl\ToastControlPopups.cs:61
  { nameof(CopyBuildingSettings), (c, t) => Options.CopySettings },

_source\sanchozz_source\src\AnyIceKettle\Patches.cs:80
  go.AddOrGet<CopyBuildingSettings>();

_source\sanchozz_source\src\AnyIceMachine\Patches.cs:72
  go.AddOrGet<CopyBuildingSettings>();

_source\sanchozz_source\src\NoManualDelivery\Patches.cs:316
  [HarmonyPatch(typeof(CopyBuildingSettings), nameof(CopyBuildingSettings.OnRefreshUserMenu))]

_source\sanchozz_source\src\NoManualDelivery\Patches.cs:333
  if (__instance.gameObject.TryGetComponent<CopyBuildingSettings>(out var settings))

_source\sanchozz_source\src\NoManualDelivery\Patches.cs:343
  if (__instance.gameObject.TryGetComponent<CopyBuildingSettings>(out var settings))

_source\sanchozz_source\src\NoManualDelivery\Patches.cs:356
  go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;

_source\sanchozz_source\src\SuitRecharger\SuitRechargerConfig.cs:106       ← SuitRecharger (suit-related!)
  go.AddOrGet<CopyBuildingSettings>();

_source\sanchozz_source\src\WornSuitDischarge\Patches.cs:255               ← QUAN TRỌNG: patch vào SuitLocker
  go.AddOrGet<CopyBuildingSettings>();

_source\sgt_imalas_source\BlueprintsV2\...\UnderConstructionDataSettingHelper.cs:167
  [HarmonyPatch(typeof(CopyBuildingSettings), nameof(CopyBuildingSettings.OnRefreshUserMenu))]

_source\sgt_imalas_source\PaintYourPipes\Patches.cs:205
  [HarmonyPatch(typeof(CopyBuildingSettings), nameof(CopyBuildingSettings.ApplyCopy))]
```

## Kết luận
- Pattern phổ biến nhất: `go.AddOrGet<CopyBuildingSettings>().copyGroupTag = <ID hoặc GameTag>;`
- **Patch quan trọng cần đọc**: `WornSuitDischarge/Patches.cs:255` — đã add vào SuitLocker
- Nếu không set `copyGroupTag` → copy được với mọi building cùng loại (dùng khi chỉ có 1 building của loại đó)
- Nếu set `copyGroupTag` → chỉ copy trong nhóm (an toàn hơn khi có nhiều variant như Atmo/Jet/Lead)
