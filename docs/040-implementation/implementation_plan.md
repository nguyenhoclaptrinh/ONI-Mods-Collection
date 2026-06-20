# Sửa lỗi Suit Locker Copy Settings - Kế hoạch thực hiện

Khắc phục lỗi khi Đại ca cấu hình một Trạm đồ bảo hộ (Suit Locker) rồi ấn "Sao chép cách cài" (Copy Settings), các locker mục tiêu không nhận được cấu hình đúng.

## Nguyên nhân lỗi & Đề xuất cải tiến

### Nguyên nhân lỗi
Hiện tại, mod đang sử dụng Harmony để hook vào State Machine `SuitLocker.States.InitializeStates` và đăng ký EventHandler ở `root` state:
```csharp
__instance.root.EventHandler(GameHashes.CopySettings, OnCopySettings);
```
Tuy nhiên, trong Oxygen Not Included, việc đăng ký sự kiện ở `root` state đôi khi không được truyền xuống đúng hoặc bị ghi đè bởi các trạng thái con cụ thể của `SuitLocker` (như `empty`, `full`, `returning`). Hơn nữa, chữ ký của hàm tĩnh `OnCopySettings` có thể không khớp chính xác với delegate mong đợi từ game.

### Giải pháp
Thay vì can thiệp vào State Machine phức tạp và kém ổn định, chúng ta sẽ chuyển sang một giải pháp chuẩn và an toàn hơn:
1. **Tạo custom component**: `SuitLockerCopySettingsComponent` kế thừa từ `KMonoBehaviour`.
2. **Lắng nghe sự kiện trực tiếp**: Trong `OnPrefabInit`, component này sẽ đăng ký lắng nghe sự kiện `GameHashes.CopySettings` bằng cơ chế `Subscribe` chuẩn của game:
   ```csharp
   Subscribe((int)GameHashes.CopySettings, OnCopySettings);
   ```
3. **Copy logic**: Khi nhận được sự kiện, đọc trạng thái của locker nguồn (`source`) từ dữ liệu sự kiện (`data` chính là `GameObject` của locker nguồn), sau đó gọi trực tiếp `ConfigRequestSuit()` hoặc `ConfigNoSuit()` trên locker hiện tại.
4. **Gắn component**: Gắn component này vào các locker (`SuitLockerConfig`, `JetSuitLockerConfig`, `LeadSuitLockerConfig`) trong Harmony patch cấu hình.

Giải pháp này hoàn toàn độc lập với trạng thái hiện tại của locker và luôn nhận được sự kiện sao chép cài đặt dù locker đang trống, đang đầy hay đang đợi đồ bảo hộ.

## Thay đổi đề xuất

### [SuitLockerCopySettings]

#### [MODIFY] [Patches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/glampi_source/SuitLockerCopySettings/Patches.cs)
- Xóa bỏ class patch cũ `SuitLocker_States_InitializeStates_Patch`.
- Thêm class component mới:
  ```csharp
  public class SuitLockerCopySettingsComponent : KMonoBehaviour
  {
      protected override void OnPrefabInit()
      {
          base.OnPrefabInit();
          Subscribe((int)GameHashes.CopySettings, OnCopySettings);
      }

      private void OnCopySettings(object data)
      {
          var srcGo = data as GameObject;
          if (srcGo == null) return;

          var srcLocker = srcGo.GetComponent<SuitLocker>();
          var dstLocker = GetComponent<SuitLocker>();
          if (srcLocker == null || dstLocker == null) return;

          if (srcLocker.smi == null || !srcLocker.smi.sm.isConfigured.Get(srcLocker.smi)) return;

          if (srcLocker.smi.sm.isWaitingForSuit.Get(srcLocker.smi) || srcLocker.GetStoredOutfit() != null)
              dstLocker.ConfigRequestSuit();
          else
              dstLocker.ConfigNoSuit();
      }
  }
  ```
- Cập nhật `SuitLockerConfig_ConfigureBuildingTemplate_Patch` để gắn thêm component mới này:
  ```csharp
  static void Postfix(GameObject go, Tag prefab_tag)
  {
      go.AddOrGet<CopyBuildingSettings>().copyGroupTag = prefab_tag;
      go.AddOrGet<SuitLockerCopySettingsComponent>();
  }
  ```

## Kế hoạch xác minh

### Biên dịch tự động
Chạy lệnh biên dịch sau tại thư mục chứa mã nguồn:
```powershell
dotnet build d:\Documents\Klei\OxygenNotIncluded\mods\Local\_source\glampi_source\SuitLockerCopySettings\SuitLockerCopySettings.csproj -c Release
```

### Kiểm tra thủ công (Đại ca thực hiện trong game)
1. Xây dựng 3 trạm Atmo Suit Locker.
2. Thiết lập trạm 1: Yêu cầu treo đồ bảo hộ (Request Suit).
3. Ấn "Sao chép cách cài" từ trạm 1 và áp dụng cho trạm 2 và trạm 3.
4. Xác minh: Trạm 2 và trạm 3 tự động chuyển sang trạng thái "Đang chờ đồ bảo hộ".
5. Bấm "Hủy yêu cầu" trên trạm 1, sao chép cài đặt sang trạm 2. Xác minh trạm 2 hủy yêu cầu.
