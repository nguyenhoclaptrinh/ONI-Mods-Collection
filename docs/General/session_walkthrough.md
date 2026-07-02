# Nhật ký Hoàn thành Tối ưu hóa & Vá lỗi (Walkthrough)

Đã hoàn thành vá lỗi crash và tối ưu hóa hiệu năng cho các mod đang active của Đại ca. Toàn bộ mã nguồn đã được biên dịch thành công, triển khai DLL thực tế vào thư mục mod game local, và commit sạch sẽ lên Git theo từng phần đúng chuẩn quy ước.

---

## 1. Các thay đổi và tối ưu hóa chi tiết

### Mod: AI Improvements (Sửa lỗi crash sập game)
* **Mã nguồn**: [AIImprovementsPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/peterhan_source/AIImprovements/AIImprovementsPatches.cs)
* **Nội dung**: Sử dụng Reflection để thay thế delegate an toàn `SafeEvaluateEntry` tại `OnLoad`. Triệt tiêu lệnh Assert sập game của game gốc khi gặp Chore không hợp lệ, chuyển sang bỏ qua nhẹ nhàng.
* **Commit**: `fix(ai-improvements): sửa lỗi crash assert trong FindNextChore`

### Mod: Move Geyser Instant (Tối ưu hóa CPU overhead)
* **Mã nguồn**: [MoveGeyserPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/MoveGeyserInstant/MoveGeyserPatches.cs), [MoveGeyserTool.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/MoveGeyserInstant/MoveGeyserTool.cs)
* **Nội dung**: 
  - Chuyển Harmony Patch từ `KPrefabID.OnSpawn` (chạy hàng triệu lần) sang `Assets.CreatePrefab` (chỉ chạy duy nhất 1 lần khi khởi động game).
  - Tối ưu hóa tránh cấp phát bộ nhớ (GC allocation) trong `UpdateOverlay` của `MoveGeyserTool` bằng cách tái sử dụng các List static/readonly qua `.Clear()`.
  - Cache kết quả truy vấn kiểu dữ liệu qua `AccessTools.TypeByName`.
* **Commit**:
  - `perf(move-geyser-instant): tối ưu hóa chuyển sang patch Assets.CreatePrefab`
  - `perf(move-geyser-instant): tối ưu hóa MoveGeyserTool tránh allocate List trong Update`

### Mod: NoManualDelivery (Tối ưu hóa thuật toán và GC)
* **Mã nguồn**: [Patches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/sanchozz_source/src/NoManualDelivery/Patches.cs)
* **Nội dung**:
  - Tối ưu hóa vòng lặp quét vật phẩm của tay gắp tự động (`SolidTransferArm_Sim.Postfix`) bằng cách sử dụng `HashSetPool` của game để lọc trùng danh sách.
  - Sắp xếp các câu lệnh kiểm tra nhẹ lên trước, đạt hiệu năng **Zero Garbage Collection Allocation** (không sinh rác cho GC).
  - Khôi phục đầy đủ 4 file thư viện dùng chung bị thiếu của tác giả Sanchozz (`BaseOptions.cs`, `StateMachinesExtensions.cs`, `TranspilerUtils.cs`, `Utils.cs`) trong thư mục `src/lib/` để dự án build độc lập hoàn toàn.
* **Commit**: `perf(no-manual-delivery): tối ưu hóa vòng lặp SolidTransferArm.Sim và GC`

### Mod: Unlock All Blueprints (Cài đặt mod mở khóa skin)
* **Mã nguồn**: [UnlockAllBlueprintsPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/UnlockAllBlueprints/UnlockAllBlueprintsPatches.cs)
* **Nội dung**: Ép giá trị độ hiếm (`rarity`) của tất cả các diện mạo công trình, tác phẩm nghệ thuật, quần áo, và bóng bay về `PermitRarity.Universal` để mở khóa offline toàn bộ skin mà không cần kết nối server Klei.
* **Commit**: `feat(unlock-all-blueprints): cài đặt mod mở khóa toàn bộ blueprint`

### Mod: Move This Here (Đồng bộ storage tags)
* **Mã nguồn**: [HaulingPoint.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/doctorfeelgood_source/source/MoveThisHere/HaulingPoint.cs)
* **Nội dung**: Thêm phương thức `UpdateStorageTags` để tự động kích hoạt sự kiện `OnStorageInteracted` cho các item khi thay đổi cấu hình lưu trữ, giữ logic đồng bộ tốt.
* **Commit**: `perf(move-this-here): tối ưu hóa HaulingPoint cập nhật storage tags`

### Mod: Reapy (Sửa đổi tương thích phiên bản game mới)
* **Mã nguồn**: [ReapStates.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/beatlepie_source/Reapy/Reapy/ReapStates.cs)
* **Nội dung**: Thay thế thuộc tính đã bị game loại bỏ `maxProbingRadius` thành phép tính toán động lấy giá trị lớn nhất giữa `maxProbeRadiusX` và `maxProbeRadiusY` của Navigator.
* **Commit**: `fix(reapy): cập nhật maxProbingRadius thành maxProbeRadius cho tương thích ONI mới`

### Mod: MassMoveTo (Chặn leak click khi kích hoạt công cụ)
* **Mã nguồn**: [TargetSelectTool.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/sgt_imalas_source/MassMoveTo/Tools/TargetSelectTool.cs), [MoveToLocationTool_Patches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/sgt_imalas_source/MassMoveTo/Patches/MoveToLocationTool_Patches.cs)
* **Nội dung**: Thêm cơ chế ghi nhận `activateFrame` để chặn leak click chuột trái khi người dùng vừa chọn công cụ di chuyển hàng loạt.
* **Commit**: `fix(mass-move-to): chặn leak click khi vừa kích hoạt công cụ và thêm ILRepack`

### Mod: SuitLockerCopySettings & CameraJumpDebugger (Triển khai công cụ debug)
* **Nội dung**: Build lại và triển khai các file DLL ổn định cho mod copy cấu hình tủ đồ bay và công cụ debug bước nhảy camera.
* **Commit**:
  - `chore(suit-locker-copy-settings): build và triển khai dll của SuitLockerCopySettings`
  - `debug(camera-jump): thêm mod CameraJumpDebugger để debug stacktrace của camera jump`

---

## 2. Chuẩn hóa cấu trúc thư mục Tài liệu (docs/)
* **Ý định**: Tổ chức lại thư mục `docs/` để phân nhóm tài liệu theo từng mod đang hoạt động, đồng thời gom các tài liệu phân tích và mã nguồn dịch ngược dùng chung vào một thư mục chung `General/`.
* **Kết quả di chuyển**:
  - Gom toàn bộ file decompile thô của game vào `docs/General/Decompiled/` (bao gồm cả file `Chore_1-decompile.cs` được đổi tên chuẩn hóa).
  - Di chuyển các tài liệu kế hoạch, phân tích crash log (`player-log-analysis.md`) vào `docs/General/`.
  - Phân nhóm tài liệu của từng mod về đúng thư mục của mod đó (ví dụ: `docs/NoManualDelivery/`, `docs/UnlockAllBlueprints/`, `docs/MoveGeyserInstant/`...).
  - Dọn dẹp sạch sẽ toàn bộ thư mục trống cũ (`040-implementation/`, `050-Research/`).

---

## 3. Kết quả Biên dịch và Triển khai thực tế

* **Trạng thái**: Biên dịch 100% thành công trên tất cả các dự án bằng .NET SDK, không còn lỗi cú pháp hay thiếu thư viện tham chiếu.
* **Triển khai**: Toàn bộ các file DLL phân phối đã được cập nhật trực tiếp tại thư mục game mod local:
  - `AIImprovements\AIImprovements.dll`
  - `MoveGeyserInstant\MoveGeyserInstant.dll`
  - `NoManualDelivery\NoManualDelivery.dll`
  - `UnlockAllBlueprints\UnlockAllBlueprints.dll`
  - `MoveThisHere\MoveThisHere.dll`
  - `Reapy\Reapy.dll`
  - `MassMoveTo\MassMoveTo.dll`
  - `SuitLockerCopySettings\SuitLockerCopySettings.dll`
  - `CameraJumpDebugger\CameraJumpDebugger.dll`

---

## 4. Trạng thái Git Repository
* **Cấu trúc mới**: Đã được Git MV theo dõi đổi vị trí chính xác của 48 file tài liệu.
* **Trạng thái**: Working tree chuẩn hóa sạch sẽ.
