# Kế hoạch tối ưu hóa công cụ Reflector (Không Hardcode)

Tối ưu hóa công cụ Reflector hiện có (`tools/Reflector/Program.cs`) thành một tiện ích độc lập, không hardcode đường dẫn game, có khả năng tái sử dụng cao và tự động xuất kết quả phản chiếu ra file Markdown theo chuẩn Rule 00.

## Yêu cầu và Thiết kế mới
- **Đường dẫn game (Không Hardcode)**: Đọc từ file cấu hình `tools/Reflector/config.json`.
  - Nếu file cấu hình không tồn tại hoặc rỗng, chương trình sẽ báo lỗi hướng dẫn cấu hình.
  - Đường dẫn mặc định của Đại ca sẽ được tạo sẵn trong file `config.json` này để tiện dùng ngay.
- **Tham số đầu vào**: Nhận danh sách tên Class cần phản chiếu từ tham số dòng lệnh `args` (ví dụ: `dotnet run --project tools/Reflector KCrashReporter SaveLoader`).
- **Dữ liệu đầu ra**:
  - Tự động ghi kết quả vào file markdown `docs/050-Research/outputs/<TypeName>-reflection.md`.
  - Định dạng file md tuân thủ cấu trúc YAML Frontmatter quy định trong Rule 00.
  - Phản chiếu đầy đủ: Fields, Properties và Methods định dạng theo syntax C# chuẩn để dễ đọc nhất.
  - Đồng thời in ra Console để tiện theo dõi trực tiếp.

## Các tệp tin thay đổi và tạo mới

### [tools]

#### [NEW] [config.json](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/tools/Reflector/config.json)
- Lưu cấu hình đường dẫn thư mục `Managed` của game.

#### [MODIFY] [Program.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/tools/Reflector/Program.cs)
- Cập nhật cơ chế đọc đường dẫn game từ file `config.json`.
- Duyệt qua `args` để lấy danh sách tên Class.
- Thêm logic phản chiếu chi tiết: Properties, Fields, Methods (kèm kiểu trả về, kiểu tham số, modifiers).
- Tự động ghi file markdown kết quả vào `docs/050-Research/outputs/` và in ra Console.

## Kế hoạch kiểm thử & xác minh
1. Tạo file `config.json` với đường dẫn thực tế của Đại ca.
2. Chạy lệnh biên dịch và thực thi Reflector với tham số `KCrashReporter` và `SaveLoader`:
   ```powershell
   dotnet run --project d:\Documents\Klei\OxygenNotIncluded\mods\Local\tools\Reflector KCrashReporter SaveLoader
   ```
3. Xác minh xem các file kết quả có được tạo ra tại thư mục `docs/050-Research/outputs/` dưới tên:
   - `KCrashReporter-reflection.md`
   - `SaveLoader-reflection.md`
4. Kiểm tra cấu trúc nội dung file và YAML Frontmatter xem đã đúng chuẩn chưa.
