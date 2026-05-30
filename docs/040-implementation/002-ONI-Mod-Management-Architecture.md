# Kiến Trúc Quản Lý Mod Trong Oxygen Not Included (Bản Crack & DLC)

- **Mã tài liệu**: `docs/040-implementation/002-ONI-Mod-Management-Architecture.md`
- **Tác giả**: Antigravity (AI Coding Assistant)
- **Ngày tạo**: 2026-05-31
- **Trạng thái**: Đã phê duyệt & Lưu trữ Workspace

---

## 1. Vị Trí Cấu Hình Mods (Mod Configuration Path)
Game *Oxygen Not Included* (kể cả bản Steam chính thức hay bản bẻ khóa/crack sử dụng Steam Emulator như Goldberg, SmartSteamEmu...) đều sử dụng cơ chế gọi API hệ thống của Windows để truy xuất thư mục lưu trữ cá nhân.

Trên máy của người dùng hiện tại, thư mục `Documents` đã được hệ điều hành chuyển hướng sang ổ `D:\`. Do đó, đường dẫn cấu hình mod duy nhất hoạt động là:
👉 **`D:\Documents\Klei\OxygenNotIncluded\mods\mods.json`**

*(Không hề có file cấu hình trùng lặp trong thư mục ẩn `%USERPROFILE%\AppData\Local` hay `Roaming`)*.

---

## 2. Cơ Chế Lưu Trạng Thái Bật/Tắt Mod (Mod Activation Mechanism)
Đây là kiến thức kỹ thuật quan trọng nhất về cấu trúc dữ liệu JSON của game trong kỷ nguyên hỗ trợ DLC (*Spaced Out!* và *Frosty Planet Pack*):

### A. Thuộc tính `"enabled"` (Boolean: `true` / `false`)
- **Tác dụng**: Chỉ được game sử dụng khi chạy ở phiên bản **Vanilla gốc** (không kích hoạt bất kỳ DLC nào).
- **Trạng thái hiện tại**: Khi chạy ở chế độ DLC, game sẽ bỏ qua giá trị này và mặc định gán là `false` cho toàn bộ các mod trong tệp `mods.json`.

### B. Mảng `"enabledForDlc"` (Array of Strings)
- **Tác dụng**: Lưu giữ trạng thái kích hoạt của mod đối với từng DLC cụ thể.
- **Mã ID của DLC**:
  - **`EXPANSION1_ID`**: DLC *Spaced Out!* (Mảnh vỡ không gian).
  - **`DLC2_ID`**: DLC *The Frosty Planet Pack* (Hành tinh băng giá).
- **Quy tắc kích hoạt**:
  - **BẬT MOD**: Nếu mảng chứa phần tử ID của DLC đó. Ví dụ: `"enabledForDlc": ["EXPANSION1_ID"]`.
  - **TẮT MOD**: Nếu mảng trống rỗng `"enabledForDlc": []`.

---

## 3. Cấu Trúc File `mods.json` Thực Tế
Ví dụ về một phần tử mod đang được **Bật** cho DLC *Spaced Out!*:

```json
{
  "label": {
    "distribution_platform": 0,
    "id": "FastTrack",
    "title": "Fast Track",
    "version": 123456789
  },
  "status": 1,
  "enabled": false,               // Bị bỏ qua ở chế độ DLC
  "enabledForDlc": [
    "EXPANSION1_ID"               // MOD ĐANG ĐƯỢC BẬT CHO DLC SPACED OUT!
  ],
  "crash_count": 0,
  "reinstall_path": null,
  "staticID": "PeterHan.FastTrack"
}
```

---

## 4. Cách Bật/Tắt Mod Thủ Công Bằng Code (Bypass Game UI)
Nếu muốn kích hoạt hoặc vô hiệu hóa nhanh một bản mod (ví dụ: kích hoạt mod vừa cài đặt `ChooseNeuralVacillator` hoặc `BiggerCameraZoomOut`) mà không cần mở giao diện game, nhà phát triển hoặc AI Agent có thể chạy đoạn mã PowerShell sau để cập nhật trực tiếp tệp `mods.json`:

### Lệnh PowerShell bật mod `ChooseNeuralVacillator` cho DLC Spaced Out!:
```powershell
$path = "D:\Documents\Klei\OxygenNotIncluded\mods\mods.json"
$json = Get-Content $path -Raw | ConvertFrom-Json
$targetMod = $json.mods | Where-Object { $_.label.id -eq "ChooseNeuralVacillator" }
if ($targetMod) {
    $targetMod.enabledForDlc = @("EXPANSION1_ID")
    $json | ConvertTo-Json -Depth 100 | Set-Content $path
    Write-Host "Kích hoạt thành công ChooseNeuralVacillator!"
}
```
