# Danh sách tác vụ - Tích hợp Máy Gắp gắp chai nước/khí vào mod AutoDropBottlers

- [x] **[Execute - M]** Chỉnh sửa `AutoDropPatch.cs` -> verify: Tích hợp logic append tag vào `OnLoad` và các Harmony Patch gắn component `Automatable` cho các công trình nhận chai.
- [x] **[Clean - XS]** Dọn dẹp mã nguồn và kiểm tra các thư viện tham chiếu. -> verify: Loại bỏ code thừa.
- [x] **[Verify - S]** Chạy lệnh biên dịch `dotnet build` tại thư mục nguồn `AutoDropBottlers`. -> verify: Dự án biên dịch thành công 0 lỗi.
- [x] **[Verify - S]** Xác nhận file DLL và config được sinh ra đầy đủ ở thư mục mod local. -> verify: AutoDropBottlers.dll nằm trong `mods/Local/AutoDropBottlers`.
- [x] **[Commit - XS]** Thực hiện commit các thay đổi theo chuẩn Conventional Commits Tiếng Việt. -> verify: Commit thành công.
