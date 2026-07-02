# Kế Hoạch Cài Đặt và Đồng Bộ Mod Oxygen Not Included từ GitHub

Tài liệu này mô tả chi tiết kế hoạch tìm kiếm, clone từ GitHub và cài đặt các mod được yêu cầu vào thư mục `mods/Local` trên máy của người dùng.

## User Review Required

> [!IMPORTANT]
> **YÊU CẦU ĐÁNH GIÁ TỪ NGƯỜI DÙNG**:
> Qua quá trình nghiên cứu chuyên sâu, một số mod được yêu cầu không có kho lưu trữ GitHub chính thức hoặc công khai (chỉ phát hành trên Steam Workshop). Dưới đây là phân loại chi tiết và đề xuất xử lý:
> 
> 1. **Mod có repo GitHub xác định**:
>    - **Geyser Calculated Average Output Tooltip** (Cairath): Repo `https://github.com/Cairath/ONI-Mods` (Thư mục con `GeyserAverageOutput`).
>    - **No Manual Delivery** (Sanchozz): Repo `https://github.com/SanchozzDeponianin/ONIMods` (Thư mục con `NoManualDelivery`).
>    - **Choose Neural Vacillator** (Bản gốc của beetlepie): Repo `https://github.com/beatlepie/ONI-Mods` (Thư mục con `ChooseNeuralVacillator`). *Lưu ý: Bản cập nhật U&F của 白马非马儿 không có repo GitHub công khai, chúng tôi sẽ clone bản gốc của beetlepie hoặc báo cáo lại nếu bạn muốn giữ nguyên bản U&F.*
> 
> 2. **Mod KHÔNG tìm thấy repo GitHub chính thức** (Chỉ có trên Steam Workshop):
>    - **Custom Game Settings (fixed)** (Stephen/Peter Han): Không tìm thấy repo GitHub riêng cho mod này. Tuy nhiên, trong mã nguồn cục bộ đã có sẵn mod **Modify Difficulty Settings** (`CustomGameSettingsModifier` của Sgt_Imalas) với tính năng tương tự (cho phép chỉnh độ khó giữa game).
>    - **Immigrants Reroll** (echo_ol): Không có repo GitHub chính thức.
>    - **Rational Priority** (mesi): Không có repo GitHub chính thức.
>    - **Replace Floors** (波尔布特-Official): Không có repo GitHub chính thức.
> 
> **Đề xuất**: 
> - Chúng tôi sẽ tiến hành clone và cài đặt 3 mod có repo GitHub trên.
> - Đối với 4 mod không có repo, chúng tôi sẽ báo cáo chi tiết trạng thái để bạn nắm rõ thông tin.

## Open Questions

> [!IMPORTANT]
> **CÂU HỎI CHO NGƯỜI DÙNG**:
> Bạn có muốn chúng tôi cài đặt bản gốc của mod **Choose Neural Vacillator** từ repo của `beetlepie` không, hay chỉ muốn cài các mod có repo GitHub chính xác hoàn toàn?

## Proposed Changes

Kế hoạch thực hiện cụ thể cho từng mod:

---

### [Component] Clone và Cài đặt Mod từ GitHub

Chúng tôi sẽ tạo một thư mục tạm `scratch/clones` để clone các repo GitHub, sau đó copy thư mục chứa mã nguồn mod tương ứng vào thư mục `mods/Local`.

#### [NEW] [GeyserAverageOutput](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/GeyserAverageOutput)
- **Nguồn:** `https://github.com/Cairath/ONI-Mods.git` (thư mục con `GeyserAverageOutput`)
- **Cách thực hiện:** Clone repo về `scratch/clones/Cairath-ONI-Mods`, sao chép thư mục `GeyserAverageOutput` vào thư mục Local mods.

#### [NEW] [NoManualDelivery](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/NoManualDelivery)
- **Nguồn:** `https://github.com/SanchozzDeponianin/ONIMods.git` (thư mục con `NoManualDelivery` hoặc tương đương)
- **Cách thực hiện:** Clone repo về `scratch/clones/Sanchozz-ONIMods`, sao chép thư mục mod tương ứng vào thư mục Local mods.

#### [NEW] [ChooseNeuralVacillator](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/ChooseNeuralVacillator)
- **Nguồn:** `https://github.com/beatlepie/ONI-Mods.git` (thư mục con `ChooseNeuralVacillator`)
- **Cách thực hiện:** Clone repo về `scratch/clones/beatlepie-ONI-Mods`, sao chép thư mục mod tương ứng vào thư mục Local mods.

---

## Verification Plan

### Automated Tests
- Kiểm tra sự tồn tại của các thư mục mod mới trong thư mục Local mods.
- Kiểm tra tính toàn vẹn của mã nguồn mod vừa cài đặt (sự hiện diện của file `.cs`, `.csproj` hoặc các file cấu hình).

### Manual Verification
- Người dùng có thể khởi động game để kiểm tra xem các mod mới cài đặt có hiển thị trong danh sách Mod cục bộ của Oxygen Not Included hay không.
