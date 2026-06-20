---
id: copy-building-settings-mechanism
type: research-output
status: done
created: 2026-06-20
source: grep_search trên _source/, Reflect script, ILSpy
---

# Cơ Chế CopyBuildingSettings & SuitLocker

## Mục đích
Research cho feature "Sao chép cài đặt trạm đồ bảo hộ" (SuitLocker Copy Settings mod).
Người dùng muốn cấu hình 1 trạm rồi sao chép sang hàng loạt trạm khác giống như các công trình khác trong game.

---

## 1. CopyBuildingSettings — Cơ chế hoạt động

### Class gốc
- **Assembly**: `Assembly-CSharp.dll`
- **Namespace**: (root)
- **Base**: `KMonoBehaviour`

### Fields quan trọng
| Field | Type | Mô tả |
|-------|------|--------|
| `copyGroupTag` | `Tag` | Xác định nhóm building cùng loại (chỉ copy sang building có cùng tag). Ví dụ: `GameTags.StorageLocker`, `GameTags.Door`, hoặc ID của building config. |

### Methods quan trọng
| Method | Mô tả |
|--------|--------|
| `OnRefreshUserMenu()` | Thêm nút "Copy Settings" / "Apply Copy" vào menu chuột phải. Có thể patch bằng Harmony. |
| `ApplyCopy(GameObject src)` | Copy state từ `src` sang building hiện tại. Gọi `ICopyBuildingSettings.CopySettings()` trên mọi component implement interface đó. |

### Interface ICopyBuildingSettings
Mọi component muốn tham gia "copy" PHẢI implement interface:
```csharp
public interface ICopyBuildingSettings {
    bool CopySettings(GameObject sourceGameObject);
}
```
Khi `ApplyCopy` được gọi, nó duyệt **tất cả** component trên target implement `ICopyBuildingSettings` và gọi `CopySettings(source)`.

### Cách thêm CopyBuildingSettings vào building hiện có (qua Harmony patch):
```csharp
[HarmonyPatch(typeof(SuitLockerConfig), nameof(SuitLockerConfig.DoPostConfigureComplete))]
static class SuitLockerConfig_DoPostConfigureComplete_Patch {
    static void Postfix(GameObject go) {
        go.AddOrGet<CopyBuildingSettings>().copyGroupTag = SuitLockerConfig.ID;
    }
}
```

---

## 2. SuitLocker — Kết quả tìm kiếm

### Các kiểu liên quan (từ Find-TypesDetailed.ps1)
| Kiểu | Assembly |
|------|----------|
| `SuitLocker` | Assembly-CSharp |
| `SuitLocker+SuitMarkerState` | Assembly-CSharp |
| `SuitLocker+SuitLockerEntry` | Assembly-CSharp |
| `SuitLocker+SuitMarkerEntry` | Assembly-CSharp |
| `SuitLocker+ReturnSuitWorkable` | Assembly-CSharp |
| `SuitLocker+States` | Assembly-CSharp |
| `JetSuitLocker` (kế thừa SuitLocker) | Assembly-CSharp |
| `LeadSuitLocker` (kế thừa SuitLocker) | Assembly-CSharp |
| `STRINGS.BUILDINGS.PREFABS.SUITLOCKER` | Assembly-CSharp |
| `STRINGS.BUILDINGS.PREFABS.JETSUITLOCKER` | Assembly-CSharp |
| `STRINGS.BUILDINGS.PREFABS.LEADSUITLOCKER` | Assembly-CSharp |

### Config classes (từ grepping _source/)
- `SuitLockerConfig` — Atmo Suit Locker
- `JetSuitLockerConfig` — Jet Suit Locker  
- `LeadSuitLockerConfig` — Lead Suit Locker

### State cần copy
SuitLocker lưu trạng thái "suit được giao" trong `SuitLockerEntry`. Để "Copy Settings" hoạt động, cần xác định:
1. Suit type được assign cho locker (Tag của suit item)
2. Có `requestEquipment` hay không (cho SuitMarker)

### SuitMarker
- Dùng để đặt vùng yêu cầu mặc đồ bảo hộ
- State: danh sách locker được link, có bật/tắt yêu cầu hay không

---

## 3. Pattern "Copy Settings" từ các mod khác

### Pattern đơn giản (không có state phức tạp)
```csharp
// Trong Config patch — thêm component
go.AddOrGet<CopyBuildingSettings>().copyGroupTag = MyBuildingConfig.ID;

// Component tự implement ICopyBuildingSettings
public class MyBuilding : KMonoBehaviour, ICopyBuildingSettings {
    public SomeData setting1;
    public bool setting2;

    public bool CopySettings(GameObject sourceGameObject) {
        var src = sourceGameObject.GetComponent<MyBuilding>();
        if (src == null) return false;
        setting1 = src.setting1;
        setting2 = src.setting2;
        return true;
    }
}
```

### Pattern phức tạp (patch ICopyBuildingSettings vào kiểu không thuộc mình)
Dùng Harmony + `ConditionalWeakTable` hoặc custom component được add qua patch:
```csharp
// Add component wrapper implement ICopyBuildingSettings
[HarmonyPatch(typeof(SuitLockerConfig), "DoPostConfigureComplete")]
static class AddCopySettings {
    static void Postfix(GameObject go) {
        go.AddOrGet<CopyBuildingSettings>().copyGroupTag = "SuitLocker";
        go.AddOrGet<SuitLockerCopySettingsProxy>(); // custom component
    }
}

public class SuitLockerCopySettingsProxy : KMonoBehaviour, ICopyBuildingSettings {
    public bool CopySettings(GameObject src) {
        var srcLocker = src.GetComponent<SuitLocker>();
        var dstLocker = GetComponent<SuitLocker>();
        if (srcLocker == null || dstLocker == null) return false;
        // copy state từ srcLocker sang dstLocker
        return true;
    }
}
```

---

## 4. Mod tham khảo trong workspace

| File | Pattern dùng |
|------|-------------|
| `sanchozz_source/src/SuitRecharger/SuitRechargerConfig.cs:106` | `go.AddOrGet<CopyBuildingSettings>()` (không set copyGroupTag — copy với mọi SuitRecharger) |
| `sanchozz_source/src/WornSuitDischarge/Patches.cs:255` | Patch thêm `CopyBuildingSettings` vào SuitLocker qua Harmony |
| `sanchozz_source/src/NoManualDelivery/Patches.cs:316` | Patch `CopyBuildingSettings.OnRefreshUserMenu` để filter |
| `aki_art_source/GravitasBigStorage/...` | `copyGroupTag = GameTags.StorageLocker` — pattern copy trong nhóm |

> [!TIP]
> File `sanchozz_source/src/WornSuitDischarge/Patches.cs` là tham khảo quan trọng nhất — đã patch `CopyBuildingSettings` vào `SuitLocker`. Cần đọc để hiểu state nào được copy.

---

## 5. Kết luận & Áp dụng

### Approach được chọn
**Harmony patch** vào `SuitLockerConfig.DoPostConfigureComplete` (và tương tự cho `JetSuitLockerConfig`, `LeadSuitLockerConfig`) để:
1. Thêm `CopyBuildingSettings` component với `copyGroupTag` theo loại locker
2. Thêm custom `SuitLockerCopyProxy` component implement `ICopyBuildingSettings`
3. `CopyProxy.CopySettings()` đọc field state từ `SuitLocker` source và apply vào target

### Câu hỏi còn mở (cần ilspycmd để giải quyết)
- [ ] `SuitLocker` có field nào lưu "suit tag được assign"? (`requestedSuitTag`? `assignedSuit`?)
- [ ] `SuitMarker` lưu state yêu cầu như thế nào?
- [ ] `WornSuitDischarge` đã copy state gì từ SuitLocker?

### Script cần chạy tiếp
```powershell
# Decompile SuitLocker để xem full source
tools\ilspy\ilspycmd.exe `
  "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll" `
  -t SuitLocker --outputdir scratch\decompiled\
```
