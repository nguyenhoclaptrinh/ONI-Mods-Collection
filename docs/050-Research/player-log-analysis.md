---
id: player-log-analysis
type: research-output
status: done
created: 2026-06-28
source: Player.log analysis and reflection tools
---
# Phân Tích Player Log và Các Lỗi Mod Oxygen Not Included

## Mục đích
Phân tích tệp tin `Player.log` của game Oxygen Not Included để xác định nguyên nhân gây lỗi, cảnh báo và lỗi crash từ các mod trong workspace.

## Kết quả Phân Tích Lỗi & Cảnh Báo

Dưới đây là các vấn đề phát hiện được từ file log:

### 1. Lỗi Nghiêm Trọng (Error / Exception)

#### **Mod: AchievementProgress**
* **Vị trí lỗi:** `AchievementProgress.AchievementProgressPatches+PauseScreenOnPrefabInit.Postfix (PauseScreen __instance)` (dòng 1946-1958 trong log)
* **Chi tiết log:**
  ```log
  [09:14:05.515] [1] [ERROR] PauseScreen(Clone) ~~~!System.InvalidCastException: Specified cast is not valid.
    at (wrapper castclass) System.Object.__castclass_with_cache(object,intptr,intptr)
    at HarmonyLib.Traverse.GetValue[T] () [0x00014] in <6dcb326e4f6442999f701f1e67d0b5a0>:0 
    at AchievementProgress.AchievementProgressPatches+PauseScreenOnPrefabInit.Postfix (PauseScreen __instance) [0x00011] in <f5070849916f4fb18a3dd485aef3a76f>:0 
    at (wrapper dynamic-method) PauseScreen.PauseScreen.OnPrefabInit_Patch1(PauseScreen)
    at KMonoBehaviour.InitializeComponent () [0x00068] in <eeaae6bd36c2418387bac55a246d67a2>:0 !~~~Error in PauseScreen(Clone).PauseScreen.OnPrefabInit at (960.00, 540.00, 0.00)
  ```
* **Nguyên nhân kỹ thuật:** 
  Trong file [AchievementProgressPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/cairath_source/src/AchievementProgress/AchievementProgressPatches.cs#L17), mod cố gắng lấy giá trị của trường `buttons` của class `KButtonMenu` (mà `PauseScreen` kế thừa) và ép kiểu sang một mảng:
  `var buttons = instance.Field("buttons").GetValue<KButtonMenu.ButtonInfo[]>();`
  Tuy nhiên, kết quả phản chiếu (reflection) trực tiếp từ DLL của phiên bản game hiện tại (`Assembly-CSharp.dll`) cho thấy trường `buttons` có kiểu dữ liệu là:
  `System.Collections.Generic.IList<KButtonMenu.ButtonInfo>` (chứ không phải `KButtonMenu.ButtonInfo[]`).
  Việc Harmony cố gắng cast `IList` này thành mảng đã ném ra ngoại lệ `System.InvalidCastException`.
* **Giải pháp khắc phục:** 
  Sửa lại kiểu dữ liệu nhận được trong code patch thành `System.Collections.Generic.IList<KButtonMenu.ButtonInfo>` thay vì `KButtonMenu.ButtonInfo[]`.

---

### 2. Cảnh báo lỗi tải tài nguyên (Warnings)

#### **Mod: MoveGeyserInstant**
* **Vị trí lỗi:** `AssetsOnPrefabInitPatch.Prefix` (dòng 1386-1387 trong log)
* **Chi tiết log:**
  ```log
  [09:12:45.656] [1] [WARNING] [MoveGeyserInstant] Failed to load custom sprite: Object reference not set to an instance of an object
  ```
* **Nguyên nhân kỹ thuật:**
  Trong file [MoveGeyserPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/MoveGeyserInstant/MoveGeyserPatches.cs#L72-L92), mod cố gắng thêm icon vào `Assets.Sprites` bằng Harmony patch `Prefix` trên phương thức `Assets.OnPrefabInit`:
  `Assets.Sprites.Add("MoveGeyserToolIcon", sprite);`
  Tại thời điểm chạy `Prefix` của `Assets.OnPrefabInit`, class `Assets` chưa được khởi tạo hoàn chỉnh và `Assets.Sprites` vẫn đang là `null`. Do đó, dòng lệnh trên ném ra lỗi `NullReferenceException`.
* **Giải pháp khắc phục:**
  Đổi từ patch `Prefix` sang `Postfix` trên `Assets.OnPrefabInit` để đảm bảo hệ thống tài nguyên `Assets` đã khởi tạo xong và `Assets.Sprites` không bị null.

#### **Mod: BiggerBuildingMenu**
* **Vị trí lỗi:** Lúc khởi động game (dòng 517 trong log)
* **Chi tiết log:**
  ```log
  [09:12:23.861] <<-- Bigger Building Menu -->> Failed to read config file Config.json with exception: 
  Could not find file "D:\Documents\Klei\OxygenNotIncluded\mods\Local\BiggerBuildingMenu\Config.json"
  ```
* **Nguyên nhân kỹ thuật:** Mod không tìm thấy file cấu hình `Config.json` trong thư mục cài đặt của nó. 
* **Giải pháp khắc phục:** Trong thư mục `BiggerBuildingMenu` hiện tại chỉ có tệp mẫu `Config.template.json`. Cần tạo một bản sao của nó đặt tên là `Config.json` để mod có thể đọc đúng các thiết lập tùy chỉnh (hoặc mod tự fallback về mặc định nhưng vẫn in ra cảnh báo lỗi).

---

### 3. Phản chiếu Assembly (Assembly Reflection Output)

Để xác nhận kiểu dữ liệu của `KButtonMenu`, một script phản chiếu đã được chạy trên môi trường PowerShell 7+ (do .NET Core hỗ trợ Default Interface Methods trên DLL mới của game):

* **Tên Class:** `KButtonMenu` (từ `Assembly-CSharp.dll`)
* **Kiểu của trường `buttons`:** `System.Collections.Generic.IList<KButtonMenu.ButtonInfo>`

## Kết luận
Game không bị crash ngay lập tức từ các lỗi trên mà thoát do yêu cầu tắt ứng dụng có chủ đích (`Game.OnApplicationQuit()`). Tuy nhiên, lỗi cast trong `AchievementProgress` khiến nút "Achievement Progress" không thể xuất hiện trong menu tạm dừng (Pause Menu), và lỗi trong `MoveGeyserInstant` làm cho icon của công cụ di chuyển geyser không hiển thị đúng.
