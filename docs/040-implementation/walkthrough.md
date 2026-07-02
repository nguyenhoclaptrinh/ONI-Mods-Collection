# Nhật ký Hoàn thành Vá lỗi (Walkthrough)

## 1. Các thay đổi đã thực hiện

### Mod: AI Improvements
* **Tệp chỉnh sửa**: [AIImprovementsPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/peterhan_source/AIImprovements/AIImprovementsPatches.cs)
* **Nội dung thay đổi**:
  - Tại hàm khởi chạy mod `OnLoad`, sử dụng Reflection để lấy trường static private `ChoreConsumer.FindNextChoreEvaluateEntryHelper`.
  - Thay thế giá trị của trường đó bằng delegate an toàn `SafeEvaluateEntry` của mod `AI Improvements`.
  - Phương thức `SafeEvaluateEntry` sẽ thực hiện logic tương tự như game gốc, ngoại trừ việc loại bỏ lệnh `DebugUtil.Assert(test: false, "FindNextChore found an entry that wasn't a FetchChore")` gây crash game và thay thế bằng việc bỏ qua nhẹ nhàng (`Util.IterationInstruction.Continue`).
  - Sử dụng phương thức public `GetLastPreconditionSnapshot()` để truy cập snapshot thông tin context thay vì truy cập trường private `preconditionSnapshot` gây lỗi biên dịch.

---

## 2. Kết quả biên dịch và triển khai
* **Biên dịch**: Project build thành công, không gặp lỗi.
* **Triển khai**: File DLL mới nhất đã được copy đè lên file mod chạy thực tế của game tại:
  - [AIImprovements.dll](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/AIImprovements/AIImprovements.dll) (cập nhật lúc 19:21:19 cùng ngày).

---

## 3. Khắc phục sự cố tương lai
* Bất kể khi có mod nào lỡ tay đưa một Chore không hợp lệ hoặc null vào `fetchChoreLayer`, hàm tìm việc của cánh tay robot gắp đồ sẽ tự động bỏ qua thay vì làm sập game.
* Điều này giúp Đại ca có thể thoải mái chơi game với cả 3 mod (`AI Improvements`, `No Manual Delivery`, `Priority Zero`) được kích hoạt cùng một lúc.
