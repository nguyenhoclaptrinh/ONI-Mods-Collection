# Tối ưu hóa hiệu năng MoveGeyserInstant - Kế hoạch thực hiện

Tối ưu hóa Harmony Patch `KPrefabIDOnSpawnPatch` để giảm thiểu áp lực lên bộ dọn rác (Garbage Collector) và cải thiện hiệu năng chung của game, loại bỏ hiện tượng giật lag nhẹ (stutter) khi spawn thực thể.

## Điểm nghẽn hiệu năng hiện tại
Trong file `MoveGeyserPatches.cs`, Harmony Patch `KPrefabIDOnSpawnPatch` hook vào phương thức `OnSpawn` của `KPrefabID`. Phương thức này được gọi hàng ngàn lần mỗi khi thế giới sinh ra bất kỳ vật thể nào (Debris, item, duplicant, critter...).

Hiện tại, patch đang lấy tên Tag dạng chuỗi (`PrefabTag.Name`) và liên tục thực hiện các so sánh chuỗi đắt đỏ (`StartsWith`, `Contains`, `==`):
```csharp
string tag = __instance.PrefabTag.Name;
if (tag.StartsWith("Prop") || tag.Contains("Satellite") || ...)
```
Phép so sánh chuỗi này chạy liên tục trên hot path tạo ra rất nhiều chuỗi rác tạm thời trên bộ nhớ Heap, làm kích hoạt bộ dọn rác (GC) của Unity thường xuyên hơn, dẫn đến tụt FPS hoặc giật hình.

## Giải pháp tối ưu
Áp dụng cơ chế **Memoization (Caching)** bằng cách sử dụng một `Dictionary<Tag, bool>` tĩnh:
1. Khi gặp một loại `Tag` của thực thể, kiểm tra xem nó đã được phân tích chưa trong cache `Dictionary`.
2. Nếu chưa có trong cache, thực hiện so sánh chuỗi 1 lần duy nhất để quyết định xem nó có thể di chuyển được hay không, sau đó lưu kết quả vào cache.
3. Nếu đã có trong cache, chỉ cần lấy kết quả bằng tra cứu O(1) qua mã băm của struct `Tag` cực kỳ nhanh và hoàn toàn **không cấp phát bộ nhớ trên Heap (zero GC allocations)**.

## Thay đổi đề xuất

### [MoveGeyserInstant]

#### [MODIFY] [MoveGeyserPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/MoveGeyserInstant/MoveGeyserPatches.cs)
- Thay đổi class `KPrefabIDOnSpawnPatch`:
  ```csharp
  [HarmonyPatch(typeof(KPrefabID), "OnSpawn")]
  public static class KPrefabIDOnSpawnPatch {
      private static readonly Dictionary<Tag, bool> isMovableCache = new Dictionary<Tag, bool>();

      public static void Postfix(KPrefabID __instance) {
          if (__instance == null) return;

          Tag prefabTag = __instance.PrefabTag;
          if (!isMovableCache.TryGetValue(prefabTag, out bool isMovable)) {
              string name = prefabTag.Name;
              isMovable = name.StartsWith("Prop") || 
                          name.Contains("Satellite") || 
                          name == "LonelyMinionHouse" || 
                          name == "TemporalTearOpener" || 
                          name == "MorbRoverSpawningLocker" || 
                          name.StartsWith("FossilDig") || 
                          name == "AncientMonument";
              isMovableCache[prefabTag] = isMovable;
          }

          if (isMovable) {
              MovableStructureSupport.AddMovable(__instance.gameObject);
          }
      }
  }
  ```

## Kế hoạch xác minh

### Biên dịch tự động
Chạy lệnh biên dịch sau tại thư mục chứa mã nguồn:
```powershell
dotnet build d:\Documents\Klei\OxygenNotIncluded\mods\Local\_source\MoveGeyserInstant\MoveGeyserInstant.csproj -c Release
```

### Kiểm tra thủ công (Đại ca thực hiện trong game)
1. Khởi động game và tải một thế giới có sẵn.
2. Kiểm tra xem game có hoạt động mượt mà khi di chuyển, đào bới, spawn vật thể hay không.
3. Xác minh tính năng di chuyển vẫn hoạt động bình thường trên các Prop (gạch tàn tích) hoặc các công trình đặc biệt như Ancient Monument, Lonely Minion House...
