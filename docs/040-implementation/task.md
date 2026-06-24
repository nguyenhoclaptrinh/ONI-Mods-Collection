# Danh sách tác vụ - Sửa lỗi MoveGeyserInstant

- [x] **[Research - S]** Xác định nguyên nhân và chọn phương án dùng ReplaceAndDisplaceElement của game để sửa lỗi biến mất chất lỏng/khí. (Đã hoàn thành phân tích)
- [x] **[Execute - S]** Chỉnh sửa `MoveGeyserTool.cs` -> verify: Thay thế `ReplaceElement` bằng `ReplaceAndDisplaceElement` ở dòng 277.
- [x] **[Verify - S]** Chạy lệnh biên dịch `dotnet build` ở chế độ Release. -> verify: Dự án biên dịch thành công 0 lỗi.
- [x] **[Verify - S]** Xác nhận file DLL `MoveGeyserInstant.dll` được tạo thành công và xuất sang thư mục mod local.
- [/] **[Commit - XS]** Thực hiện commit các thay đổi theo chuẩn Conventional Commits Tiếng Việt. -> verify: Commit thành công.
