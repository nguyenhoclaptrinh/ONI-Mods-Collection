# ONI Mods Collection (v706793+)

Bộ sưu tập các bản mod được duy trì, tối ưu hóa và hiện đại hóa cho Oxygen Not Included (bao gồm cả DLC Spaced Out & The Frosty Planet). Tập trung vào hiệu năng CPU cực hạn ở late-game, trải nghiệm người dùng (QoL) và tính ổn định của bản dịch Tiếng Việt chuẩn hóa.

---

## 📂 Cấu trúc Thư mục Monorepo (Folder Structure)

Kho lưu trữ được tổ chức theo mô hình Monorepo giúp tách biệt hoàn chỉnh giữa thư mục cài đặt hoạt động cục bộ của game và kho mã nguồn gốc để phát triển:

```
.
├── [ModName]/                     # Thư mục Mod chạy cục bộ của game (đã deploy sạch 100%)
│   ├── [ModName].dll              # File thư viện mod chính
│   ├── mod.yaml & mod_info.yaml   # Metadata cấu hình mod của game
│   └── locales/                   # Bản dịch ngôn ngữ (nếu có)
├── _source/                       # Kho lưu trữ toàn bộ Mã nguồn gốc (Source Code)
│   ├── [Tac_Gia_source]/          # Thư mục mã nguồn của các tác giả lớn trong cộng đồng
│   │   ├── [ModName]/             # Mã nguồn cụ thể của từng mod
│   │   │   └── [ModName].csproj   # File dự án C# / MSBuild
│   │   ├── Directory.Build.props  # Cấu hình MSBuild chung
│   │   └── Directory.Build.targets# Script đóng gói & tự động deploy sạch
│   └── CritterAIArchitect/        # Mod tối ưu hóa AI do bạn tự phát triển
└── README.md                      # Hướng dẫn sử dụng và phát triển này
```

---

## 📑 Danh sách Mod & Đường dẫn Mã nguồn (Mod List & Source Paths)

### 1. Mod Tự phát triển & Tối ưu hóa sâu rộng (Đóng góp của bạn)

| Tên Mod | Tính năng & Tác dụng chính | Đường dẫn Mã nguồn (Source Path) |
| :--- | :--- | :--- |
| **Critter AI Architect** | Tối ưu hóa hiệu năng AI sinh vật (LOD & Time-Slicing), giảm lag CPU cực hạn cho trang trại thú late-game. | [`_source/CritterAIArchitect`](file:///_source/CritterAIArchitect) |
| **Auto Drop Bottlers** | Tự động thả chai chất lỏng/chất khí ra đất. Hỗ trợ sao chép cài đặt, PLib Options và Tiếng Việt chuẩn. | [`_source/glampi_source/AutoDropBottlers`](file:///_source/glampi_source/AutoDropBottlers) |
| **Move This Here** | Điểm gom đồ và vận chuyển vật phẩm tạm thời. Bản dịch Tiếng Việt chuẩn hóa văn phong game ONI. | [`_source/doctorfeelgood_source/source/MoveThisHere`](file:///_source/doctorfeelgood_source/source/MoveThisHere) |
| **FastTrack** | Bản mod tối ưu hóa hệ thống sâu rộng nhất của Peter Han, được cài đặt cục bộ sạch sẽ. | [`_source/peterhan_source/FastTrack`](file:///_source/peterhan_source/FastTrack) |
| **Auto Liquid Bottler** | Máy đóng chai chất lỏng tự động. Cấu hình build deploy sạch sẽ, tối ưu hóa dung lượng. | [`_source/hilliurn_source/Auto Liquid Bottler`](file:///_source/hilliurn_source/Auto%20Liquid%20Bottler) |
| **Customizable Speed** | Tùy chỉnh tốc độ chạy game linh hoạt. Cấu hình build deploy sạch sẽ, tối ưu hóa dung lượng. | [`_source/customizable_speed_source`](file:///_source/customizable_speed_source) |

### 2. Các Mod Duy trì & Fork (Maintain & Forked Mods)
*(Xem cấu trúc mã nguồn chung của các tác giả lớn trong thư mục tương ứng)*
*   **Hệ sinh thái Peter Han**: Hơn 35+ mod tối ưu QoL nằm tại [`_source/peterhan_source/`](file:///_source/peterhan_source/).
*   **Hệ sinh thái Sgt_Imalas**: Các mod giao diện lớn (BlueprintsV2, SetStartDupes, v.v.) nằm tại [`_source/sgt_imalas_source/`](file:///_source/sgt_imalas_source/).
*   **Hệ sinh thái Cairath**: Các mod tiện ích nằm tại [`_source/cairath_source/src/`](file:///_source/cairath_source/src/).
*   **Hệ sinh thái Sanchozz**: Các mod cơ chế sinh vật & trồng trọt nằm tại [`_source/sanchozz_source/src/`](file:///_source/sanchozz_source/src/).

---

## ⚙️ Hướng dẫn Thiết lập Môi trường Phát triển (Developer Setup)

Để biên dịch thành công mã nguồn trên máy tính của bạn mà không bị lỗi thiếu DLL tham chiếu của game:

1.  Truy cập vào thư mục mã nguồn của tác giả bạn muốn build (ví dụ: `_source/peterhan_source/` hoặc `_source/sgt_imalas_source/`).
2.  Sao chép tệp cấu hình mẫu `Directory.Build.props.default` (hoặc tạo mới) và đặt tên là **`Directory.Build.props.user`**.
3.  Mở `Directory.Build.props.user` và cập nhật chính xác đường dẫn đến thư mục cài đặt game của bạn:
    ```xml
    <Project>
      <PropertyGroup>
        <GameFolder>C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed</GameFolder>
        <ModFolder>[Đường_dẫn_thư_mục_mod_local_của_bạn]</ModFolder>
      </PropertyGroup>
    </Project>
    ```
    *(Tệp tin `.user` này đã được `.gitignore` bảo vệ, đảm bảo không bao giờ bị commit lên Git làm lộ thông tin cá nhân của bạn)*.

---

## 🛠️ Hướng dẫn Biên dịch & Đóng gói (How to Build & Restore)

*   **Khôi phục Dependencies (NuGet Restore)**:
    ```powershell
    dotnet restore "_source/sgt_imalas_source/SgtImalasOniMods.sln"
    ```
*   **Biên dịch & Deploy tự động**:
    *   **Cấu hình Debug** (Tự động copy bản build vào thư mục mod local chạy cục bộ của game để test):
        ```powershell
        dotnet build "_source/glampi_source/AutoDropBottlers/AutoDropBottlers.csproj" -c Debug
        ```
    *   **Cấu hình Release** (Đóng gói sạch sẽ tối ưu, loại bỏ 100% DLL thừa của game gốc):
        ```powershell
        dotnet build "_source/glampi_source/AutoDropBottlers/AutoDropBottlers.csproj" -c Release
        ```

---

## 🧹 Cẩm nang Tối ưu hóa Monorepo (Clean & Build Best Practices)

*   **Dọn dẹp rác build**: Chạy lệnh xóa các thư mục `bin` và `obj` tạm thời để giải phóng dung lượng đĩa (~266 MB dữ liệu rác).
*   **Không commit `.git` con**: Tuyệt đối không để sót thư mục `.git` ẩn trong `_source/` khi clone để tránh lỗi embedded repository.
*   **Quy chuẩn PostBuild sạch**: Tất cả các tệp `.csproj` nhỏ lẻ đều đã được tương đối hóa đường dẫn PostBuild qua biến `$(ProjectDir)` giúp bảo mật tuyệt đối 100% thông tin cá nhân của bạn và hoạt động tương thích ngay lập tức trên mọi máy tính khác khi clone về.

---

## 🎮 Hướng dẫn Cài đặt dành cho Người chơi

1. Tải bộ mod về và giải nén.
2. Mở game Oxygen Not Included, truy cập vào menu **Mods**.
3. Nhấp chọn nút **"Show Local Mods Folder"** (Hiển thị thư mục mod cục bộ). Game sẽ tự động mở thư mục mod chính xác nhất trên hệ điều hành của bạn.
4. Copy toàn bộ các thư mục mod đã giải nén thả vào đó.
5. Kích hoạt mod trong game và khởi động lại game.
