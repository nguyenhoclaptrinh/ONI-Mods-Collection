---
id: analysis-priority-zero
type: research
status: completed
created: 2026-05-28
---

# Nghiên Cứu Giải Pháp Kỹ Thuật Mod "Priority Zero" Cho Oxygen Not Included (v706793)

> [!NOTE]
> Tài liệu này phân tích cấu trúc hệ thống ưu tiên (Priority System) của game **Oxygen Not Included (v706793)** và đề xuất giải pháp kỹ thuật để tự phát triển mod "Priority Zero" (mức ưu tiên bằng 0 giúp vô hiệu hóa công việc) bằng C# và thư viện Harmony.

---

## 1. Executive Summary (Tóm Tắt Báo Cáo)
Mục tiêu của mod **Priority Zero** là cho phép người chơi thiết lập mức độ ưu tiên của một công việc hoặc tòa nhà về mức `0`. Khi ở mức này, các Duplicants và Auto-Sweepers sẽ hoàn toàn bỏ qua công việc đó, giúp người chơi dễ dàng quy hoạch cơ sở (planning layout) hoặc tạm thời dừng hoạt động của các máy móc mà không cần phá dỡ hay cắt lưới điện.

Qua phân tích phản chiếu (Reflection) trực tiếp trên `Assembly-CSharp.dll` của phiên bản game hiện tại (`v706793`), chúng tôi xác định hệ thống ưu tiên của game hoạt động thông qua cấu trúc `PrioritySetting` và lớp `Prioritizable`. Việc vô hiệu hóa công việc có thể được thực hiện vô cùng an toàn và hiệu năng cao bằng cách patch phương thức kiểm tra hợp lệ của công việc (`Chore.IsValid`).

---

## 2. Phân Tích Cấu Trúc Hệ Thống Ưu Tiên (Game Mechanics Analysis)

### A. Cấu trúc `PrioritySetting`
`PrioritySetting` là một `struct` (ValueType) lưu giữ mức độ ưu tiên của một công việc hoặc đối tượng:
- **`priority_class`** (`PriorityClass`): Enum xác định phân nhóm ưu tiên (`basic = 0`, `high = 1`, `personalNeeds = 2`, `topPriority = 3`, `compulsory = 4`, `idle = -1`).
- **`priority_value`** (`int`): Giá trị ưu tiên dạng số (mặc định từ 1 đến 9).

### B. Cơ chế Errands & Chores
Mọi công việc trong game được biểu diễn bằng lớp `Chore`. Mỗi `Chore` đều liên kết với một component `Prioritizable` trỏ đến tòa nhà hoặc đối tượng tương ứng.
Khi một Duplicant hoặc Auto-Sweeper tìm kiếm công việc thông qua `ChoreConsumer.FindNextChore()`, game sẽ kiểm tra xem `Chore` đó có hợp lệ hay không bằng cách gọi:
```csharp
public bool IsValid()
```
If `Chore.IsValid()` trả về `false`, công việc đó sẽ bị loại bỏ khỏi danh sách quét và hoàn toàn bị bỏ qua.

---

## 3. Đề Xuất Giải Pháp Kỹ Thuật (Technical Recommendation)

Chúng tôi đề xuất triển khai giải pháp Harmony Patch gồm hai phần cốt lõi:

### Phần 1: Patch Logic Vô Hiệu Hóa Công Việc (Chore Disabling)
Sử dụng Harmony Patch dạng **Prefix** để chặn hàm `Chore.IsValid()`. Nếu mức độ ưu tiên của công việc hoặc của đối tượng liên quan có `priority_value == 0` (hoặc lớp ưu tiên basic và giá trị bằng 0), phương thức sẽ trả về `false` ngay lập tức mà không cần chạy logic gốc của game.

```csharp
[HarmonyPatch(typeof(Chore), nameof(Chore.IsValid))]
public static class Chore_IsValid_Patch {
    public static bool Prefix(Chore __instance, ref bool __result) {
        if (__instance.masterPriority.priority_value == 0) {
            __result = false;
            return false; // Chặn hoàn toàn logic gốc
        }
        
        // Kiểm tra thêm thông qua Prioritizable nếu masterPriority chưa cập nhật
        if (__instance.prioritizable != null && __instance.prioritizable.GetMasterPriority().priority_value == 0) {
            __result = false;
            return false;
        }
        
        return true; // Tiếp tục chạy logic gốc
    }
}
```

### Phần 2: Thêm Nút Ưu Tiên "0" Vào Giao Diện `PriorityScreen` (UI Integration)
Màn hình `PriorityScreen` chịu trách nhiệm vẽ các nút ưu tiên từ 1 đến 9. Lớp này chứa một trường:
- `protected List<PriorityButton> buttons_basic`

Chúng ta cần patch phương thức `PriorityScreen.InstantiateButtons` để nhân bản thêm một nút ưu tiên thứ 10 đại diện cho mức số `0`, đồng thời gán nhãn văn bản và biểu tượng số 0 tương ứng.

#### Kỹ thuật Patch UI:
Sử dụng Harmony **Postfix** trên `PriorityScreen.InstantiateButtons` để clone một nút từ `buttons_basic` hiện tại, đổi giá trị của nó thành `0` và cập nhật sự kiện click chuột để khi người chơi bấm vào, game sẽ kích hoạt `PrioritySetting(PriorityScreen.PriorityClass.basic, 0)`.

```csharp
[HarmonyPatch(typeof(PriorityScreen), "InstantiateButtons")]
public static class PriorityScreen_InstantiateButtons_Patch {
    public static void Postfix(PriorityScreen __instance) {
        // Sao chép nút số 1 thành nút số 0
        // Đặt vị trí, text hiển thị thành "0"
        // Gán sự kiện OnClick để truyền PrioritySetting với value = 0
    }
}
```

---

## 4. Đánh Giá Hiệu Năng & An Toàn (Performance & Safety Analysis)

*   **Hiệu năng ($O(1)$)**: Phép kiểm tra `priority_value == 0` trong `Chore.IsValid` tốn thời gian cực kỳ nhỏ ($O(1)$), không gây suy giảm FPS ngay cả khi có hàng ngàn Chores trong game.
*   **Tính Tương Thích Ngược**: Khi người chơi gỡ bỏ mod này, game sẽ tự động tải các tòa nhà với priority = 0. Do game mặc định không giới hạn nghiêm ngặt việc nạp struct `PrioritySetting(basic, 0)` từ file lưu, game sẽ tự động coi mức 0 là một mức cơ bản cực kỳ thấp hoặc fallback về mặc định mà không gây crash save file. Tuy nhiên, khuyến nghị người dùng chuyển priority về mức 1-9 trước khi gỡ mod vẫn là một Best Practice.

---

## 5. Kế Hoạch Tiếp Theo (Next Steps)
1. **Lập Kế Hoạch Thực Thi**: Tạo file `implementation_plan.md` để người dùng duyệt qua.
2. **Khởi Tạo Dự Án C# Mod**: Tạo cấu trúc thư mục mod `PriorityZero` trong `_source`.
3. **Viết Code Patch**: Phát triển mã nguồn C# và Harmony hoàn chỉnh.
4. **Biên Dịch & Cài Đặt**: Build ra file `PriorityZero.dll` và copy trực tiếp vào thư mục `mods/Local/PriorityZero`.
5. **Kiểm Thử & Xác Minh**: Khởi động game và cung cấp bằng chứng chạy thành công.
