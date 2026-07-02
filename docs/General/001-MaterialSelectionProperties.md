# Báo cáo Nghiên cứu: Mod Material Selection Properties

- **Mã tài liệu**: `docs/050-Research/001-MaterialSelectionProperties.md`
- **Tác giả**: Antigravity (AI Coding Assistant)
- **Ngày tạo**: 2026-05-30
- **Trạng thái**: Hoàn thành nghiên cứu lý thuyết & So sánh kiến trúc

---

## 1. Bản chất của mod Material Selection Properties (của mesi)
Mod **Material Selection Properties** (phát triển bởi tác giả **mesi** trên Steam Workshop) là một mod cải thiện trải nghiệm người dùng (Quality of Life - QoL) tập trung vào giao diện xây dựng.

### Chức năng chính:
- Hiển thị trực tiếp các thông số vật lý của vật liệu xây dựng ngay trên danh sách lựa chọn:
  - **Thermal Conductivity (TC)**: Độ dẫn nhiệt ($W/m\cdot K$).
  - **Specific Heat Capacity (SHC)**: Nhiệt dung riêng ($DTU/g\cdot^\circ C$).
  - **Melting Point**: Điểm nóng chảy (hoặc nhiệt độ phá huỷ đối với một số vật liệu đặc biệt).
- **Mục tiêu**: Giúp người chơi so sánh nhanh hiệu năng nhiệt học giữa các vật liệu (ví dụ: Sandstone vs Igneous Rock vs Granite) ngay khi click chọn vật liệu để đặt lệnh xây dựng, thay vì phải tra cứu bách khoa toàn thư trong game (Database) hoặc Wiki bên ngoài.

---

## 2. Đối chiếu với các Mod hiện có trong Source Monorepo
Trong monorepo hiện tại của bạn, chúng ta đã có sẵn mod **`ThermalTooltips`** của tác giả **PeterHan** (cả mã nguồn tại `_source/peterhan_source/ThermalTooltips` và bản DLL đã biên dịch chạy thực tế tại `ThermalTooltips/`).

Qua phân tích mã nguồn của `ThermalTooltips`, chúng ta thấy có sự trùng lặp và khác biệt rất rõ ràng:

### A. Sự giống nhau:
- Cả hai mod đều cung cấp cho người chơi các chỉ số vật lý quan trọng của vật liệu trước khi xây dựng (`Thermal Conductivity`, `Specific Heat Capacity` / `Thermal Mass`, `Melting Point`).

### B. Sự khác biệt về mặt UX/UI:

| Đặc điểm so sánh | Mod ThermalTooltips (PeterHan) | Mod Material Selection Properties (mesi) |
| :--- | :--- | :--- |
| **Vị trí hiển thị chính** | **Material Effects Panel** (Bảng mô tả tác động bên dưới bảng danh sách vật liệu). | **Danh sách các nút chọn (Toggle buttons)** của vật liệu. |
| **Cơ chế tương tác** | Người chơi **bắt buộc phải click chọn** vào một vật liệu cụ thể để panel bên dưới load và hiển thị các thông số nhiệt học của vật liệu đó. | Các thông số vật lý được viết tắt và hiển thị **trực tiếp bên cạnh tên** của tất cả các vật liệu trong danh sách. Người chơi chỉ cần nhìn lướt qua là so sánh được ngay. |
| **Các tính năng phụ** | Rất nhiều tính năng nâng cao: Tooltip chi tiết khi hover chuột trên bản đồ nhiệt, hiển thị tổng nhiệt năng (Heat Energy) của vật thể, khả năng hiển thị đồng thời 3 đơn vị nhiệt độ ($^\circ C$, $^\circ F$, $K$). | Chỉ tập trung hiển thị text thuộc tính trên danh sách lựa chọn vật liệu. |
| **Độ phức tạp mã nguồn** | Cao (Sử dụng transpiler để can thiệp sâu vào hiển thị hover text card của game). | Cực kỳ thấp (Chỉ cần patch hậu kỳ vào nhãn text của các toggle chọn vật liệu). |

> [!NOTE]
> Bản thân tác giả **mesi** cũng đã ghi chú trên trang Steam Workshop của mình rằng: *"Chức năng này cũng đã được tích hợp trong mod Thermal Tooltips. Nếu bạn đã cài đặt Thermal Tooltips, bạn không thực sự cần cài đặt Material Selection Properties nữa."*
> Tuy nhiên, điều này chỉ đúng về mặt **dữ liệu cung cấp**, còn về mặt **trực quan tiện lợi (UX)** thì việc hiển thị trực tiếp trên danh sách toggle của `Material Selection Properties` vẫn mang lại trải nghiệm nhanh và tiện hơn rất nhiều (không cần click mỏi tay để so sánh).

---

## 3. Đánh giá kỹ thuật & Đề xuất Hướng đi

Nếu bạn muốn có trải nghiệm so sánh trực quan siêu tốc ngay trên danh sách vật liệu (giống mod của mesi):

### Phương án A: Tận dụng hoàn toàn ThermalTooltips (Khuyên dùng nếu muốn tối giản)
- **Ưu điểm**: Không cần thêm code mới, không sợ xung đột phiên bản game khi update. Game gốc chạy cực kỳ ổn định.
- **Cách dùng**: Khi xây dựng, bạn chỉ cần click vào từng vật liệu, bảng `Material Effects` bên dưới sẽ hiển thị đầy đủ thông số đã được Việt hoá (nếu có).

### Phương án B: Tự phát triển một bản mod Harmony Patch siêu nhẹ (Nếu bạn thích giao diện trực quan)
- **Cơ chế hoạt động**: Chúng ta sẽ viết một lớp Patch kế thừa Harmony can thiệp vào `MaterialSelector.ConfigureScreen` hoặc `MaterialSelector.RefreshToggleContents` (trong `Assembly-CSharp.dll` gốc).
- **Mã giả triển khai (C# Harmony)**:
  ```csharp
  [HarmonyPatch(typeof(MaterialSelector), "RefreshToggleContents")]
  public static class MaterialSelector_RefreshToggleContents_Patch
  {
      public static void Postfix(MaterialSelector __instance)
      {
          foreach (var kvp in __instance.ElementToggles)
          {
              Tag tag = kvp.Key;
              KToggle toggle = kvp.Value;
              Element element = ElementLoader.GetElement(tag);
              if (element != null)
              {
                  // Tìm nhãn văn bản hiển thị tên vật liệu (thường là LocText đầu tiên)
                  var textComponents = toggle.GetComponentsInChildren<LocText>();
                  if (textComponents != null && textComponents.Length > 0)
                  {
                      var nameLabel = textComponents[0];
                      // Nối thêm thông số vật lý viết tắt: TC (Thermal Conductivity) và SHC (Specific Heat Capacity)
                      string originalName = element.name;
                      string propertiesStr = $" [{element.thermalConductivity:0.#} | {element.specificHeatCapacity:0.##}]";
                      nameLabel.text = originalName + propertiesStr;
                  }
              }
          }
      }
  }
  ```
- **Đánh giá**: Rất dễ viết, hiệu năng cực cao, hoàn toàn tương thích và bổ trợ hoàn hảo cho `ThermalTooltips` mà không gây bất kỳ xung đột nào.

---

## 4. Kết luận
1. **Bản mod tương đương trong source**: Chính là `ThermalTooltips` của PeterHan, đã được cài đặt DLL và có sẵn mã nguồn.
2. **Khuyến nghị**: Bạn không cần cài thêm mod `Material Selection Properties` gốc của mesi vì nó trùng lặp dữ liệu với `ThermalTooltips`.
3. **Lựa chọn của bạn**: 
   - Nếu bạn thấy tính năng hiển thị của `ThermalTooltips` (click để xem thông số chi tiết ở panel bên dưới) đã là đủ dùng, chúng ta có thể dừng lại tại đây mà không cần viết thêm code gì để đảm bảo codebase monorepo sạch sẽ.
   - Nếu bạn vẫn muốn có trải nghiệm hiển thị trực quan thông số rút gọn ngay trên danh sách toggle chọn vật liệu, hãy cho tôi biết, tôi sẽ nhanh chóng tạo dự án và triển khai mã nguồn Patch C# siêu gọn này cho bạn!
