---
id: navigator-reflection
type: research-output
status: done
created: 2026-06-29
source: inspect_navigator.ps1 via net10.0 console tool
---
# Cấu trúc phản chiếu (Reflection) của class Navigator (Oxygen Not Included build 706793)

## Mục đích
Phân tích sự thay đổi trong class `Navigator` của game Oxygen Not Included phiên bản mới nhằm giải quyết lỗi biên dịch thiếu trường `maxProbingRadius` khi nâng cấp mod Reapy.

## Kết quả phản chiếu (Raw Output)

### Fields của Navigator:
- `System.Boolean DebugDrawPath`
- `KMonoBehaviour <target>k__BackingField`
- `CellOffset[] <targetOffsets>k__BackingField`
- `NavGrid <NavGrid>k__BackingField`
- `Facing facing`
- `System.Single defaultSpeed`
- `TransitionDriver transitionDriver`
- `System.String NavGridName`
- `System.Boolean updateProber`
- `System.Int32 maxProbeRadiusX` *(MỚI)*
- `System.Int32 maxProbeRadiusY` *(MỚI)*
- `PathFinder+PotentialPath+Flags flags`
- `LoggerFSS log`
- `System.Collections.Generic.Dictionary`2[[NavType, Assembly-CSharp],[System.Int32]] distanceTravelledByNavType`
- `PathGrid <PathGrid>k__BackingField`
- `Grid+SceneLayer sceneLayer`
- `PathFinderAbilities abilities`
- `KBatchedAnimController animController`
- `PathFinder+Path path`
- `NavType CurrentNavType`
- `System.Int32 AnchorCell`
- `KPrefabID targetLocator`
- `System.Int32 cachedCell`
- `System.Int32 reservedCell`
- `NavTactic tactic`
- `System.Boolean reportOccupation`
- `System.Collections.Generic.List`1[[System.Int32]] occupiedCells`
- `System.Action`2[[System.Int32],[System.Object]] OnBuildingTileChangedAction`
- `System.Boolean executePathProbeTaskAsync`
- `EventSystem+IntraObjectHandler`1[[Navigator]] OnDefeatedDelegate`
- `EventSystem+IntraObjectHandler`1[[Navigator]] OnRefreshUserMenuDelegate`
- `EventSystem+IntraObjectHandler`1[[Navigator]] OnSelectObjectDelegate`
- `EventSystem+IntraObjectHandler`1[[Navigator]] OnStoreDelegate`
- `EventSystem+IntraObjectHandler`1[[Navigator]] OnQueueDestroyDelegate`
- `StateMachineController stateMachineController`
- `System.Boolean autoRegisterSimRender`
- `System.Boolean simRenderLoadBalance`

### Properties của Navigator:
- `KMonoBehaviour target`
- `CellOffset[] targetOffsets`
- `NavGrid NavGrid`
- `PathGrid PathGrid`
- `Navigator+StatesInstance smi`
- `System.Boolean isSpawned`
- `UnityEngine.Transform transform`
- `System.Boolean isNull`
- `System.Threading.CancellationToken destroyCancellationToken`
- `System.Boolean useGUILayout`
- `System.Boolean enabled`
- `System.Boolean isActiveAndEnabled`
- `UnityEngine.GameObject gameObject`
- `System.String tag`
- `System.String name`
- `UnityEngine.HideFlags hideFlags`

## Kết luận & Áp dụng
Trường `maxProbingRadius` (kiểu float/int cũ) của `Navigator` đã bị Klei thay thế bằng hai trường kiểu `int` mới là:
1. `maxProbeRadiusX`: Tầm hoạt động tối đa theo trục ngang (X).
2. `maxProbeRadiusY`: Tầm hoạt động tối đa theo trục dọc (Y).

**Hướng sửa đổi mod:**
- Trong `ReapBotConfig.cs` (khởi tạo robot): Gán cả `maxProbeRadiusX` và `maxProbeRadiusY` bằng giá trị tầm hoạt động cấu hình.
- Trong `ReapStates.cs` (kiểm tra khoảng cách quay đầu): Lấy giá trị lớn nhất của 2 trường này (`Math.Max(navigator.maxProbeRadiusX, navigator.maxProbeRadiusY)`) để so sánh với chi phí di chuyển về trạm sạc.
