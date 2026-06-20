# Danh sách tác vụ - Sửa lỗi Suit Locker Copy Settings

- [x] **[Research - S]** Xác định nguyên nhân và đưa ra giải pháp thay thế EventHandler của StateMachine bằng Event Subscription. (Đã hoàn thành phân tích)
- [x] **[Execute - M]** Chỉnh sửa `Patches.cs` -> verify: Xóa patch State Machine cũ, định nghĩa `SuitLockerCopySettingsComponent` và đăng ký lắng nghe sự kiện `GameHashes.CopySettings` bằng `Subscribe`.
- [x] **[Verify - S]** Chạy lệnh biên dịch `dotnet build` ở chế độ Release. -> verify: Dự án biên dịch thành công 0 lỗi.
- [x] **[Verify - S]** Xác nhận file DLL `SuitLockerCopySettings.dll` được tạo thành công và xuất sang thư mục mod local.
- [/] **[Commit - XS]** Thực hiện commit các thay đổi theo chuẩn Conventional Commits Tiếng Việt. -> verify: Commit thành công.
