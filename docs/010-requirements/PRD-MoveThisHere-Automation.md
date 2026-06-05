# PRD - Tích hợp Tự động hóa cho công trình Hauling Point (Mod MoveThisHere)

- **Mã tài liệu**: `docs/010-requirements/PRD-MoveThisHere-Automation.md`
- **Trạng thái**: Bản thảo (Draft)
- **Tác giả**: Antigravity (AI Coding Assistant)
- **Ngày tạo**: 2026-06-05

---

## 1. Bối cảnh & Mục tiêu (Context & Objectives)
Mod **MoveThisHere** giới thiệu công trình **Hauling Point** giúp gom tài nguyên (rắn, lỏng, khí) với khối lượng cực kỳ chính xác. Hiện tại, Hauling Point chỉ hỗ trợ:
- Gửi tín hiệu tự động hóa **RA** (Logic Output) khi đầy.
- Tự động xả đồ (Auto-Eject) hoặc tự huỷ (Auto-Drop/Self-destruct) khi đầy hoàn toàn.

**Hạn chế**: Người chơi không thể điều khiển việc xả đồ từ xa bằng mạng lưới tự động hóa (Automation Grid). Ví dụ, họ muốn xả đồ khi một cảm biến nhiệt độ kích hoạt, hoặc khi một chu kỳ thời gian nhất định trôi qua, hoặc dừng thu gom đồ khi hệ thống quá tải.

**Mục tiêu**:
- Thêm **Cổng vào Tự động hóa** (Logic Input Port) cho Hauling Point.
- Cho phép người chơi kích hoạt việc xả đồ (Eject) hoặc dừng nhận đồ (Disable fetching) từ xa thông qua tín hiệu Logic.

---

## 2. Yêu cầu tính năng (Feature Requirements)

### Yêu cầu 1: Cổng vào Tự động hóa (Logic Input Port)
- Thêm một cổng kết nối Logic Input vào thiết kế của `HaulingPoint`.
- Người chơi có thể nối dây tín hiệu tự động hóa vào cổng này.

### Yêu cầu 2: Hành vi điều khiển bằng tín hiệu Logic
Thêm một chế độ cấu hình (Toggle Button) trên giao diện người dùng của Hauling Point để chọn cách xử lý tín hiệu Input:
1. **Chế độ 1: Điều khiển Xả đồ (Logic Eject Mode)**:
   - Nhận tín hiệu **Green**: Tự động kích hoạt việc thả toàn bộ đồ trong kho ra sàn (Drop/Eject) mà không cần Hauling Point phải đầy 100%.
   - Nhận tín hiệu **Red**: Không làm gì cả.
2. **Chế độ 2: Điều khiển Thu gom (Logic Disable Fetching Mode)**:
   - Nhận tín hiệu **Green**: Bật chế độ thu gom bình thường.
   - Nhận tín hiệu **Red**: Tạm thời đình chỉ (Suspend) toàn bộ công việc mang đồ tới Hauling Point (hủy Fetch List hiện tại và không tạo Fetch List mới).

---

## 3. Tiêu chí nghiệm thu (Acceptance Criteria)
- Cổng tự động hóa mới hiển thị chính xác trong lớp phủ Tự động hóa (Automation Overlay).
- Khi kết nối dây tín hiệu và gửi tín hiệu thích hợp:
  - Ở chế độ *Logic Eject*, tín hiệu Green lập tức làm rơi toàn bộ tài nguyên chứa bên trong.
  - Ở chế độ *Logic Disable*, tín hiệu Red làm biến mất các công việc vận chuyển (Fetch chores) của Duplicant tới Hauling Point.
- Không gây crash game khi lưu/tải game với cổng logic mới đang hoạt động.
- Hỗ trợ đầy đủ Copy Settings và Mod Blueprints.
