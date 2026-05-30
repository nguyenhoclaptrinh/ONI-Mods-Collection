# ONI Mods Collection (v706793+)

[![Game Version](https://img.shields.io/badge/Game_Version-U56--706793-blue.svg)](https://forums.kleientertainment.com/forums/forum/119-oxygen-not-included-mods-and-tools/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Mod Count](https://img.shields.io/badge/Mods-66_Forks-orange.svg)](#)

A curated monorepo containing maintained, optimized, and localized mods for **Oxygen Not Included** (supporting base game, *Spaced Out!* DLC, and *The Frosty Planet Pack* DLC).

Dự án này là bộ sưu tập monorepo lưu trữ, duy trì và tối ưu hóa sâu các bản mod trong game **Oxygen Not Included**. Trọng tâm của bộ sưu tập là cải thiện hiệu năng CPU cực hạn ở giai đoạn late-game, nâng cấp trải nghiệm người dùng (QoL), và cung cấp bản dịch tiếng Việt chuẩn hóa, chính xác nhất.

---

## 🎮 Hướng dẫn Cài đặt dành cho Người chơi (Installation)

Để cài đặt và sử dụng bộ mod này một cách nhanh chóng và an toàn:

1. **Tải xuống**: Tải tệp nén của bộ mod về máy tính và giải nén.
2. **Mở thư mục Mod**:
   - Khởi động game **Oxygen Not Included**.
   - Truy cập vào mục **Mods** trong Menu chính.
   - Nhấp chuột vào nút **"Show Local Mods Folder"** (Hiển thị thư mục mod cục bộ). Hệ điều hành sẽ tự động mở thư mục mod chính xác nhất của bạn.
3. **Sao chép**: Copy (sao chép) toàn bộ các thư mục mod đã giải nén (ví dụ: `AutoDropBottlers`, `FastTrack`, `TiengViet`...) thả trực tiếp vào thư mục mod cục bộ vừa được mở.
4. **Kích hoạt**: Bật các mod mong muốn trong menu Mod của game.
5. **Khởi động lại**: Khởi động lại game để áp dụng các thay đổi và thưởng thức!

---

## 📋 Danh sách Mod chính & Đường dẫn Mã nguồn (Featured Mods)

Đây là các bản mod được phát triển mới, Việt hóa chất lượng cao hoặc tối ưu hóa sâu bởi tác giả của bộ sưu tập này.

| Tên Mod | Tính năng & Tác dụng chính | Đường dẫn Mã nguồn (Source Path) |
| :--- | :--- | :--- |
| **Critter AI Architect** | Mod độc lập tối ưu hiệu năng AI sinh vật (LOD & Time-Slicing), giảm lag CPU cực hạn cho trang trại late-game. | [`_source/CritterAIArchitect`](./_source/CritterAIArchitect) |
| **Tiếng Việt chuẩn hóa** | Bản dịch Việt hóa Oxygen Not Included chất lượng cao, chuẩn văn phong game của Chuot Chanel. | [`TiengViet`](./TiengViet) |
| **Auto Drop Bottlers** | Tự động thả chai chất lỏng/chất khí ra đất. Hỗ trợ sao chép thiết lập nhanh, menu cấu hình PLib và Việt hóa. | [`_source/glampi_source/AutoDropBottlers`](./_source/glampi_source/AutoDropBottlers) |
| **Move This Here** | Điểm gom đồ và trung chuyển tạm thời. Bản dịch Tiếng Việt chuẩn hóa (sửa dịch máy thô sơ của bản gốc). | [`_source/doctorfeelgood_source/source/MoveThisHere`](./_source/doctorfeelgood_source/source/MoveThisHere) |
| **Auto Liquid Bottler** | Máy đóng chai chất lỏng tự động. Cấu hình build deploy sạch sẽ, tối ưu hóa dung lượng (chỉ 16 KB). | [`_source/hilliurn_source/Auto Liquid Bottler`](./_source/hilliurn_source/Auto%20Liquid%20Bottler) |
| **Customizable Speed** | Tùy chỉnh tốc độ chạy game linh hoạt. Sửa lỗi cơ chế PostBuild an toàn, dọn dẹp file DLL thừa. | [`_source/customizable_speed_source`](./_source/customizable_speed_source) |
| **Priority Zero** | Cho phép đặt mức ưu tiên 0 để vô hiệu hóa công việc (Duplicants và Auto-Sweepers bỏ qua hoàn toàn). | [`_source/AzeTheGreat_source/src/PriorityZero`](./_source/AzeTheGreat_source/src/PriorityZero) |
| **Plan Buildings Without Materials** | Quy hoạch công trình không cần tài nguyên trong kho, hỗ trợ thiết kế base linh hoạt. | [`_source/cairath_source/src/PlanBuildingsWithoutMaterials`](./_source/cairath_source/src/PlanBuildingsWithoutMaterials) |
| **Suppress Notifications** | Ẩn/tắt các thông báo lỗi và biểu tượng trạng thái phiền phức (cây chết, thiếu điện) trực tiếp trên giao diện. | [`_source/AzeTheGreat_source/src/SuppressNotifications`](./_source/AzeTheGreat_source/src/SuppressNotifications) |
| **Bigger Building Menu** | Mở rộng lưới hiển thị của thực đơn xây dựng, giúp xem nhiều công trình cùng lúc mà không cần cuộn nhiều. | [`_source/cairath_source/src/BiggerBuildingMenu`](./_source/cairath_source/src/BiggerBuildingMenu) |

*(Đối với hơn 60+ mod fork và duy trì khác, vui lòng xem mã nguồn chi tiết tại các thư mục tương ứng trong `_source/`)*

---

## 📂 Cấu trúc Thư mục Monorepo (Project Structure)

Kho lưu trữ Monorepo được thiết kế theo cấu trúc tách biệt rõ ràng giữa các mod chạy thực tế và mã nguồn phát triển:

```
.
├── [ModName]/                     # Thư mục mod chạy cục bộ của game (deploy sạch 100%)
│   ├── [ModName].dll              # File thư viện mod chính
│   ├── mod.yaml & mod_info.yaml   # Metadata cấu hình mod của game
│   └── locales/                   # Bản dịch ngôn ngữ (nếu có)
├── _source/                       # Kho lưu trữ toàn bộ Mã nguồn gốc (Source Code)
│   ├── [Tác_giả_source]/          # Thư mục mã nguồn của từng tác giả lớn trong cộng đồng
│   │   ├── [ModName]/             # Mã nguồn dự án cụ thể của mod
│   │   │   └── [ModName].csproj   # Tệp cấu hình dự án C# / MSBuild
│   │   ├── Directory.Build.props  # Cấu hình MSBuild chung
│   │   └── Directory.Build.targets# Script tự động đóng gói & deploy sạch
│   └── CritterAIArchitect/        # Mod tối ưu hóa AI gốc tự phát triển
├── LICENSE                        # Giấy phép sử dụng mã nguồn
└── README.md                      # Tài liệu hướng dẫn này
```

---

## ⚙️ Thiết lập Môi trường Phát triển (Developer Setup)

Để biên dịch thành công mã nguồn trên máy tính của bạn mà không bị lỗi thiếu tham chiếu DLL game gốc:

### 1. Cấu hình Đường dẫn Game cục bộ
1. Truy cập vào thư mục mã nguồn của tác giả bạn muốn build (ví dụ: `_source/peterhan_source/` hoặc `_source/sgt_imalas_source/`).
2. Sao chép tệp cấu hình mẫu `Directory.Build.props.default` và đặt tên là **`Directory.Build.props.user`**.
3. Mở `Directory.Build.props.user` và cập nhật chính xác đường dẫn đến thư mục cài đặt game của bạn:
   ```xml
   <Project>
     <PropertyGroup>
       <GameFolder>C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed</GameFolder>
       <ModFolder>[Đường_dẫn_thư_mục_mod_local_của_bạn]</ModFolder>
     </PropertyGroup>
   </Project>
   ```
   *(Lưu ý: Tệp `.user` này đã được cấu hình trong `.gitignore`, đảm bảo không bao giờ bị commit lên Git làm lộ thông tin cá nhân hoặc đường dẫn tuyệt đối của bạn)*.

### 2. Biên dịch & Đóng gói bằng CLI
* **Khôi phục Dependencies (NuGet Restore)**:
  ```powershell
  dotnet restore "_source/sgt_imalas_source/SgtImalasOniMods.sln"
  ```
* **Biên dịch & Tự động Deploy**:
  * **Cấu hình Debug** (Tự động biên dịch và copy thẳng vào thư mục mod hoạt động của game để test):
    ```powershell
    dotnet build "_source/glampi_source/AutoDropBottlers/AutoDropBottlers.csproj" -c Debug
    ```
  * **Cấu hình Release** (Biên dịch tối ưu hóa hiệu năng, loại bỏ 100% DLL game thừa thãi để đóng gói sạch sẽ):
    ```powershell
    dotnet build "_source/glampi_source/AutoDropBottlers/AutoDropBottlers.csproj" -c Release
    ```

---

## 📜 Credits & Bản quyền (Credits & Attribution)

Dự án monorepo này hoạt động dựa trên việc kế thừa và duy trì di sản tuyệt vời từ các modder tài năng nhất trong cộng đồng Oxygen Not Included. Tôi xin gửi lời cảm ơn sâu sắc nhất và ghi nhận toàn bộ credit tới các tác giả gốc:

| Tác giả gốc | Kho mã nguồn gốc (Original GitHub) | Kênh Steam Workshop | Giấy phép (Original License) |
| :--- | :--- | :--- | :--- |
| **Peter Han** | [peterhaneve/ONIMods](https://github.com/peterhaneve/ONIMods) | [Workshop](https://steamcommunity.com/profiles/76561198025154321/myworkshopfiles/?appid=457140) | MIT / CC BY-NC-SA 4.0 |
| **Sgt_Imalas** | [Sgt-Imalas/Sgt_Imalas-Oni-Mods](https://github.com/Sgt-Imalas/Sgt_Imalas-Oni-Mods) | [Workshop](https://steamcommunity.com/id/Sgt_Imalas/myworkshopfiles/) | MIT |
| **Sanchozz** | [SanchozzDeponianin/ONIMods](https://github.com/SanchozzDeponianin/ONIMods) | [Workshop](https://steamcommunity.com/profiles/76561198341359629/myworkshopfiles/?appid=457140) | MIT |
| **Cairath** | [Cairath/ONI-Mods](https://github.com/Cairath/ONI-Mods) | [Workshop](https://steamcommunity.com/profiles/76561198076768290/myworkshopfiles/?appid=457140) | N/A |
| **Aze** | [AzeTheGreat/ONI-Mods](https://github.com/AzeTheGreat/ONI-Mods) | [Workshop](https://steamcommunity.com/profiles/76561198044590606/myworkshopfiles/?appid=457140) | MIT |
| **aki-art** | [aki-art/ONI-Mods](https://github.com/aki-art/ONI-Mods) | [Workshop](https://steamcommunity.com/id/oniki/myworkshopfiles/?appid=457140) | N/A |
| **Pholith** | [Pholith/ONI-Mods](https://github.com/Pholith/ONI-Mods) | [Workshop](https://steamcommunity.com/profiles/76561198263471888/myworkshopfiles/?appid=457140) | N/A |
| **DoctorFeelGoodMD** | — | — | MIT |
| **Glampi** | — | — | N/A |
| **beatlepie** | — | — | N/A |

---

## ⚙️ Cơ chế Quản lý Mod (Mod Management)

Đối với phiên bản game hỗ trợ các DLC (như *Spaced Out!* và *Frosty Planet Pack*), trạng thái Bật/Tắt của mod không được lưu trữ trong trường `"enabled"` mà được lưu trữ trong mảng `"enabledForDlc"` của tệp cấu hình **`mods.json`**. 

Chi tiết cấu trúc kỹ thuật và hướng dẫn bật/tắt mod tự động bằng PowerShell được tài liệu hóa chi tiết tại:
👉 **[Kiến Trúc Quản Lý Mod ONI](./docs/040-implementation/002-ONI-Mod-Management-Architecture.md)**

---

## ⚠️ Tuyên bố Miễn trừ Trách nhiệm (Disclaimer)

- **Mục đích**: Repository này là bộ sưu tập cá nhân, phi thương mại, được phát triển và fork với mục đích duy nhất là sửa lỗi tương thích (compatibility patch) cho game phiên bản mới và tối ưu hóa trải nghiệm người dùng Việt Nam.
- **Bản quyền**: Mọi quyền sở hữu trí tuệ của các mã nguồn nguyên bản hoàn toàn thuộc về các tác giả gốc được liệt kê trong phần Credits. Mã nguồn tự phát triển mới của tôi được phân phối theo giấy phép MIT.

