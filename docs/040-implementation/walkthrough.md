# Walkthrough: Máy Gắp gắp chai nước/khí trong mod AutoDropBottlers

Chúng ta đã hoàn thành việc nâng cấp mod `AutoDropBottlers` nhằm hỗ trợ Máy gắp (Auto-Sweeper / SolidTransferArm) gắp các chai chất lỏng/chất khí và tự động nạp vào các công trình nhận chai như Máy xả chai (Bottle Emptier) hoặc Siêu máy tính (Advanced Research Center).

## Thay đổi đã thực hiện

### 1. Bổ sung bộ lọc vật phẩm cho Máy Gắp
- **Vị trí**: [AutoDropPatch.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/glampi_source/AutoDropBottlers/AutoDropPatch.cs#L35)
- **Nội dung**: Tại phương thức `OnLoad` của mod, thực hiện nhân bản mảng tĩnh `TUNING.STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE` và đưa toàn bộ các tag trong `TUNING.STORAGEFILTERS.LIQUIDS` (chất lỏng đóng chai) cùng `TUNING.STORAGEFILTERS.GASES` (chất khí đóng chai) vào. Việc này giúp Máy gắp nhận diện được các chai nước và chai khí trên mặt đất là đối tượng có thể gắp.

### 2. Bổ sung component Automatable vào các công trình đích
- **Vị trí**: [AutoDropPatch.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/glampi_source/AutoDropBottlers/AutoDropPatch.cs#L323)
- **Nội dung**: Thêm các lớp Harmony Patch để tự động gắn thêm component `Automatable` mặc định của game vào các công trình:
  - `BottleEmptierConfig` (Máy xả chai)
  - `BottleEmptierGasConfig` (Máy xả khí đóng bình)
  - `BottleEmptierConduitLiquidConfig` (Máy xả chai dẫn vào ống chất lỏng)
  - `BottleEmptierConduitGasConfig` (Máy xả khí dẫn vào ống chất khí)
  - `AdvancedResearchCenterConfig` (Siêu máy tính)
- **Kết quả**: Khi các công trình này được xây dựng, game sẽ tự động nhận diện chúng là "Automatable" (có thể tự động hóa), từ đó:
  - Cho phép Máy gắp thực hiện công việc tiếp vận (FetchChore).
  - Tự động hiển thị thêm một checkbox **"Cho phép nạp thủ công / Tự động hóa" (Allow Manual Delivery)** giúp Đại ca dễ dàng quản lý việc cho phép robot gắp hay đệ nạp nước.

---

## Kết quả kiểm thử tự động (Build)
Dự án đã được biên dịch thành công 100%:
- **Errors**: 0
- **Warnings**: 0
- **Output**: File mod đã được cập nhật trực tiếp tại thư mục phát hành cục bộ [AutoDropBottlers.dll](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/AutoDropBottlers/AutoDropBottlers.dll).

---

## Hướng dẫn kiểm thử thủ công (Dành cho Đại ca)
1. Khởi chạy game *Oxygen Not Included*.
2. Xây dựng một **Máy xả chai** (Bottle Emptier) hoặc một **Siêu máy tính** (Advanced Research Center) ở gần nguồn nước.
3. Khi bấm vào công trình đó, xác nhận có xuất hiện checkbox phụ trợ **Allow Manual Delivery**.
4. Ném một chai nước trên đất trong tầm gắp của **Máy gắp** (Auto-Sweeper).
5. Xác nhận Máy gắp tự động gắp chai nước và nạp thẳng vào Máy xả chai/Siêu máy tính.
