# Kế hoạch tích hợp tính năng Máy Gắp gắp chai nước/khí vào mod AutoDropBottlers

Bản kế hoạch chi tiết nhằm bổ sung tính năng cho phép Máy gắp (Auto-Sweeper / SolidTransferArm) gắp các chai chất lỏng/chất khí (liquid/gas bottles) và đưa vào các công trình nhận chai như Máy xả chai (Bottle Emptier), Máy xả khí (Canister Emptier), hoặc Siêu máy tính (Super Computer).

## User Review Required

> [!IMPORTANT]
> **Giải pháp triển khai**:
> - Không tạo công trình mới (sẽ patch trực tiếp vào game thông qua Harmony trong mod local `AutoDropBottlers`).
> - Tương thích hoàn toàn với game mới và game đang chơi.
> - Sau khi cài đặt, các công trình nhận chai sẽ có thêm nút Checkbox **"Cho phép nhận diện tự động" (Allow Manual Delivery)** từ thành phần `Automatable` mặc định của game. Đại ca có thể tích chọn để chỉ cho phép robot gắp hoặc cho phép cả đệ tử thủ công nạp nước.

## Proposed Changes

### AutoDropBottlers Mod

---

#### [MODIFY] [AutoDropPatch.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/glampi_source/AutoDropBottlers/AutoDropPatch.cs)
- Cập nhật phương thức `OnLoad` của lớp `AutoDropBottlersMod` để bổ sung các tag lọc chất lỏng (`LIQUIDS`) và chất khí (`GASES`) vào danh sách các vật phẩm Máy gắp được phép tương tác (`TUNING.STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE`).
- Thêm các lớp Harmony Patch để tự động gắn thêm component `Automatable` cho các công trình nhận chai khi game khởi tạo thiết lập hoàn chỉnh (`DoPostConfigureComplete`):
  - `BottleEmptierConfig` (Máy xả chai chất lỏng)
  - `BottleEmptierGasConfig` (Máy xả chai chất khí)
  - `BottleEmptierConduitLiquidConfig` (Máy xả chai chất lỏng vào ống)
  - `BottleEmptierConduitGasConfig` (Máy xả chai chất khí vào ống)
  - `AdvancedResearchCenterConfig` (Siêu máy tính - giúp tự động hóa nạp nước nghiên cứu)

Mã nguồn dự kiến bổ sung vào `AutoDropPatch.cs`:

```csharp
// Trong AutoDropBottlersMod.OnLoad:
try
{
    var conveyableList = new System.Collections.Generic.List<Tag>(TUNING.STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE);
    foreach (var tag in TUNING.STORAGEFILTERS.LIQUIDS)
    {
        if (!conveyableList.Contains(tag))
            conveyableList.Add(tag);
    }
    foreach (var tag in TUNING.STORAGEFILTERS.GASES)
    {
        if (!conveyableList.Contains(tag))
            conveyableList.Add(tag);
    }
    TUNING.STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE = conveyableList.ToArray();
    Debug.Log("[AutoDropBottlers] Successfully appended liquid and gas tags to SOLID_TRANSFER_ARM_CONVEYABLE.");
}
catch (System.Exception ex)
{
    Debug.LogWarning("[AutoDropBottlers] Failed to append liquid/gas tags: " + ex.Message);
}

// Các Harmony Patch bổ sung ở cuối file:
[HarmonyPatch(typeof(BottleEmptierConfig), "DoPostConfigureComplete")]
public static class BottleEmptierConfig_DoPostConfigureComplete_Patch
{
    public static void Postfix(GameObject go)
    {
        if (go != null)
        {
            go.AddOrGet<Automatable>();
        }
    }
}

[HarmonyPatch(typeof(BottleEmptierGasConfig), "DoPostConfigureComplete")]
public static class BottleEmptierGasConfig_DoPostConfigureComplete_Patch
{
    public static void Postfix(GameObject go)
    {
        if (go != null)
        {
            go.AddOrGet<Automatable>();
        }
    }
}

[HarmonyPatch(typeof(BottleEmptierConduitLiquidConfig), "DoPostConfigureComplete")]
public static class BottleEmptierConduitLiquidConfig_DoPostConfigureComplete_Patch
{
    public static void Postfix(GameObject go)
    {
        if (go != null)
        {
            go.AddOrGet<Automatable>();
        }
    }
}

[HarmonyPatch(typeof(BottleEmptierConduitGasConfig), "DoPostConfigureComplete")]
public static class BottleEmptierConduitGasConfig_DoPostConfigureComplete_Patch
{
    public static void Postfix(GameObject go)
    {
        if (go != null)
        {
            go.AddOrGet<Automatable>();
        }
    }
}

[HarmonyPatch(typeof(AdvancedResearchCenterConfig), "DoPostConfigureComplete")]
public static class AdvancedResearchCenterConfig_DoPostConfigureComplete_Patch
{
    public static void Postfix(GameObject go)
    {
        if (go != null)
        {
            go.AddOrGet<Automatable>();
        }
    }
}
```

---

## Verification Plan

### Automated Tests
- Thực hiện biên dịch dự án bằng lệnh `dotnet build` tại thư mục nguồn `d:\Documents\Klei\OxygenNotIncluded\mods\Local\_source\glampi_source\AutoDropBottlers` để kiểm tra lỗi cú pháp.
- Đảm bảo DLL được copy thành công vào thư mục mod game chạy cục bộ `d:\Documents\Klei\OxygenNotIncluded\mods\Local\AutoDropBottlers`.

### Manual Verification
1. Khởi chạy game *Oxygen Not Included*.
2. Xây dựng một Máy xả chai (Bottle Emptier) hoặc Siêu máy tính (Advanced Research Center).
3. Đảm bảo khi bấm vào các công trình này, bảng Side Screen xuất hiện lựa chọn "Allow Manual Delivery" (Cho phép nạp thủ công / Tự động hóa).
4. Đặt một chai nước trên sàn trong phạm vi của Máy gắp (Auto-Sweeper).
5. Xác nhận Máy gắp tự động gắp chai nước và đưa vào công trình đích thành công.
