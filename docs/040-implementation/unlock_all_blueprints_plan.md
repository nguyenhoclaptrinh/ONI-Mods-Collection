# Kế hoạch triển khai: Cài đặt và phân tích mod Unlock All Blueprints

Kế hoạch này chi tiết việc cài đặt mod `Unlock All Blueprints` từ nguồn đã clone vào cấu trúc thư mục của dự án monorepo Oxygen Not Included hiện tại, cấu hình build bằng .NET SDK, và phân tích chi tiết mã nguồn của mod.

## User Review Required

> [!IMPORTANT]
> **Độ tương thích phiên bản game**: Bản mod này sử dụng Harmony patch để thay đổi giá trị trả về của `PermitRarity` trong game. Cần đảm bảo game đã được cập nhật phiên bản tương thích (Mod sử dụng `netstandard2.1` và reference đến các DLL trong thư mục game `d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793`).

## Proposed Changes

Chúng ta sẽ tích hợp mod này dưới tên `UnlockAllBlueprints`.

---

### Cấu trúc mã nguồn (`_source/UnlockAllBlueprints`)

Tạo mới thư mục chứa source code cho mod này để quản lý mã nguồn và biên dịch.

#### [NEW] [UnlockAllBlueprints.csproj](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/UnlockAllBlueprints/UnlockAllBlueprints.csproj)
Tạo file dự án C# `.csproj` kế thừa cấu hình từ các mod khác như `MoveGeyserInstant.csproj`:
- Target Framework: `netstandard2.1`
- Reference đến các thư viện game như `Assembly-CSharp_public.dll`, `Assembly-CSharp-firstpass_public.dll`, `0Harmony.dll`, `UnityEngine.dll`.
- Tự động sinh `mod.yaml` và `mod_info.yaml` khi build.
- Cấu hình deploy tự động copy file `.dll`, `mod.yaml`, `mod_info.yaml` ra thư mục phân phối `d:\Documents\Klei\OxygenNotIncluded\mods\Local\UnlockAllBlueprints`.

#### [NEW] [UnlockAllBlueprintsPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/UnlockAllBlueprints/UnlockAllBlueprintsPatches.cs)
Chứa mã nguồn Harmony Patches dùng để override thuộc tính độ hiếm (`rarity`) của các vật phẩm trang trí, trang phục, tác phẩm nghệ thuật, và skin bóng bay thành `PermitRarity.Universal`.

---

### Thư mục phân phối mod (`UnlockAllBlueprints`)

Thư mục này là nơi chứa các file mod đã build mà game Oxygen Not Included sẽ load trực tiếp.

#### [NEW] Thư mục [UnlockAllBlueprints](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/UnlockAllBlueprints)
Sẽ chứa:
- `UnlockAllBlueprints.dll` (File binary đã build)
- `mod.yaml` (Metadata của mod)
- `mod_info.yaml` (Thông tin phiên bản game hỗ trợ)

---

### Tài liệu phân tích và báo cáo (`docs/`)

#### [NEW] [unlock-all-blueprints-analysis.md](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/docs/050-Research/outputs/unlock-all-blueprints-analysis.md)
Tài liệu phân tích chuyên sâu cấu trúc, cơ chế hoạt động của mod và cách game xử lý blueprint/permit rarity.

---

## Verification Plan

### Automated Tests
- Thực hiện build thử dự án bằng lệnh:
  ```bash
  dotnet build d:\Documents\Klei\OxygenNotIncluded\mods\Local\_source\UnlockAllBlueprints\UnlockAllBlueprints.csproj -c Release
  ```
- Kiểm tra xem build có thành công (exit code 0) hay không.
- Xác nhận các file output (`UnlockAllBlueprints.dll`, `mod.yaml`, `mod_info.yaml`) được deploy đúng vào thư mục `d:\Documents\Klei\OxygenNotIncluded\mods\Local\UnlockAllBlueprints\`.

### Manual Verification
- Xác minh định dạng nội dung file `mod.yaml` và `mod_info.yaml` được tạo ra đúng cấu trúc.
