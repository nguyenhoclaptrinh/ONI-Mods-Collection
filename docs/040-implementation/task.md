# Danh sách tác vụ - Tối ưu hóa MoveGeyserInstant

- [x] **[Research - S]** Xác định điểm nghẽn hiệu năng ở KPrefabIDOnSpawnPatch và chọn phương án dùng Dictionary cache Tag. (Đã hoàn thành phân tích)
- [x] **[Execute - M]** Chỉnh sửa `MoveGeyserPatches.cs` -> verify: Thêm `isMovableCache` và áp dụng Memoization trong `KPrefabIDOnSpawnPatch`.
- [/] **[Verify - S]** Chạy lệnh biên dịch `dotnet build` ở chế độ Release. -> verify: Dự án biên dịch thành công 0 lỗi.
- [ ] **[Verify - S]** Xác nhận file DLL `MoveGeyserInstant.dll` được tạo thành công và xuất sang thư mục mod local.
- [ ] **[Commit - XS]** Thực hiện commit các thay đổi theo chuẩn Conventional Commits Tiếng Việt. -> verify: Commit thành công.
