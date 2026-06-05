# Kết quả phân tích API Game v706793 (Mod Ronivan)

Tài liệu lưu trữ dữ liệu phân tích thô từ chương trình `Inspector` đối với thư viện game `Assembly-CSharp.dll` để phục vụ các lần nâng cấp sau.

## 1. Phân tích `STRINGS.UI.ExtractLinkID`
- **Tình trạng**: Còn tồn tại trong namespace `STRINGS.UI`.
- **Nguyên nhân lỗi trước đó**: Trong file `MultiIngredientCodexVisualizer.cs` có khai báo namespace/class trùng tên làm compiler hiểu lầm `UI.ExtractLinkID` thuộc namespace cục bộ của mod.
- **Giải pháp**: Gọi tuyệt đối bằng prefix `global::STRINGS.UI.ExtractLinkID(string)`.

## 2. Phân tích trạng thái nhiên liệu `ENOUGH_FUEL`
- **Tình trạng**: Thư viện game runtime thực tế không định nghĩa `STRINGS.BUILDINGS.STATUSITEMS.ENOUGH_FUEL` ổn định mà chỉ tồn tại ở các phiên bản publicized stub cũ.
- **Giải pháp**: Thay thế việc gọi hằng số của game bằng định nghĩa trực tiếp chuỗi chữ (String Literals) an toàn để tránh crash khi game cập nhật:
  - Tên hiển thị: `"Waiting for Fuel"`
  - Tooltip: `"Waiting for {0} ({1})"`

## 3. Phân tích Kiểu dữ liệu `MultiToggle.onClick`
- **Tình trạng**: `MultiToggle.onClick` là kiểu `System.Action`.
- **Nguyên nhân lỗi trước đó**: Tác giả mod viết các đoạn code delegate gán bằng cách ép kiểu `(Action)Delegate.Combine(...)`. Do namespace cục bộ nhận diện `Action` là game enum `KInputHandler.Action` dẫn đến lỗi cast không hợp lệ.
- **Giải pháp**: Ép kiểu tường minh bằng `(System.Action)` và thay đổi tham số nhận vào của các phương thức helper (như `AddButton`) thành `System.Action`.
