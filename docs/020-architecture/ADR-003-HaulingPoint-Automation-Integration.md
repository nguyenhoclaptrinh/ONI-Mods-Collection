# ADR-003: Tích hợp cổng tự động hóa Logic Input vào Hauling Point

- **Mã tài liệu**: `docs/020-architecture/ADR-003-HaulingPoint-Automation-Integration.md`
- **Trạng thái**: Đã phê duyệt (Approved)
- **Tác giả**: Antigravity (AI Coding Assistant)
- **Ngày tạo**: 2026-06-05

---

## 1. Bối cảnh (Context)
Hiện tại, Hauling Point chỉ có một cổng Logic Output để thông báo trạng thái đầy (`AmountStored >= capacity`). Để tăng tính linh hoạt và khả năng tự động hóa, người chơi cần điều khiển hành vi của Hauling Point từ xa bằng tín hiệu tự động hóa (Logic Input).

Chúng ta cần quyết định giải pháp kiến trúc tối ưu để:
- Thêm cổng vào tự động hóa mà không gây xung đột đồ họa hoặc lỗi crash do vị trí đặt cổng.
- Quản lý trạng thái logic và đồng bộ hóa với hệ thống Fetch của game ONI một cách an toàn.

---

## 2. Quyết định kỹ thuật (Decision)

### A. Thiết lập cổng Logic trên BuildingDef
- Thêm một cổng vào tự động hóa (Logic Input Port) vào danh sách `LogicInputPorts` của `BuildingDef` trong `HaulingPointConfig.cs`.
- Tên ID cổng: `HaulingPointLogicInputPort`.
- Vị trí (CellOffset): Đặt ở vị trí `(0, 0)` để tránh chồng lấn đồ họa với cổng ra hiện tại (cũng ở vị trí `(0, 0)`). Trong game ONI, một công trình 1x1 có thể chứa cả cổng Logic Input và Output tại cùng 1 cell hoặc cell lệch. Tuy nhiên, để tránh rối mắt, ta sẽ đặt Logic Input ở `(0, 0)` và chuyển Logic Output sang `(0, 0)` hoặc ngược lại (cần kiểm tra xem game hiển thị thế nào). 
  - *Giải pháp*: Giữ Logic Output tại `(0, 0)` và đặt Logic Input tại `(0, 0)`. Game ONI hỗ trợ việc này (ví dụ các công trình 1x1 thông minh như Logic Gates).

### B. Quản lý sự kiện tín hiệu (Logic Event Handling)
- Class `HaulingPoint` sẽ kế thừa hoặc subscribe sự kiện thay đổi tín hiệu của `LogicPorts` bằng cách đăng ký `OnLogicValueChanged`.
- Lắng nghe giá trị tín hiệu logic đầu vào:
  - Nếu `LogicEjectMode == true`: Khi nhận tín hiệu Green (`value > 0`), lập tức gọi `storage.DropAll()` tương tự khi đầy.
  - Nếu `LogicDisableFetchMode == true`: Khi nhận tín hiệu Red (`value == 0`), cập nhật bộ lọc hoặc trạng thái hoạt động của `FilteredStorageHaulingPoint` để tạm ngắt Fetch List.

### C. Cơ chế tạm ngắt Fetch List (Disable Fetching)
- Trong `FilteredStorageHaulingPoint`, phương thức `OnFilterChanged` chịu trách nhiệm tạo và đệ trình `FetchList2`.
- Chúng ta sẽ bổ sung thuộc tính `bool isFetchingEnabled` vào `FilteredStorage` này.
- Khi `isFetchingEnabled == false`, `OnFilterChanged` sẽ hủy `FetchList` hiện tại và không tạo Fetch List mới. Điều này đảm bảo Duplicant không mang thêm đồ tới công trình khi nhận tín hiệu Red.

---

## 3. Trade-offs & Consequences (Đánh đổi & Hệ quả)

### Ưu điểm:
- Cải thiện đáng kể QoL và khả năng mở rộng của mod, cho phép người chơi tạo các hệ thống dọn dẹp và vận chuyển tài nguyên cực kỳ thông minh.
- Sử dụng các API chuẩn của game ONI (`LogicPorts`, `FetchList2`), đảm bảo tính tương thích và hiệu năng cao.

### Nhược điểm & Rủi ro:
- Đòi hỏi cập nhật cơ chế lưu dữ liệu (Serialization) cho các thuộc tính cấu hình mới (`willLogicEject`, `willLogicDisableFetch`, v.v.). Điều này có thể ảnh hưởng đến các file save game cũ nếu không xử lý fallback an toàn.
  - *Biện pháp giảm thiểu*: Gán giá trị mặc định cho các biến mới trong `OnSpawn` hoặc sử dụng kiểu dữ liệu an toàn.

---

## 4. Cấu trúc dữ liệu lưu trữ (Serialization)
Các thuộc tính mới cần được lưu lại trong file save:
- `[Serialize] public bool willLogicEject = false;`
- `[Serialize] public bool willLogicDisableFetch = false;`
- Cập nhật cả phương thức sao chép cấu hình `OnCopySettings` và Mod Blueprints.
