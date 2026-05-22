---
id: ONI-Mods-Local-Indexing
type: Implementation-Document
status: Active
created: 2026-05-21
---

# Lập Chỉ Mục Mã Nguồn Bộ Sưu Tập Mod Oxygen Not Included (Local)

Tài liệu này tổng hợp toàn bộ danh sách các mod và dự án thành phần có trong mã nguồn cục bộ (Local workspace) của bạn. Hệ thống mã nguồn được tổ chức theo cấu trúc Monorepo/Multi-repo với các thư mục đại diện cho từng tác giả lớn (`*_source`).

Tổng số thư mục tác giả nguồn: **10 tác giả**
Tổng số lượng dự án/mod được phân loại: **310+ dự án & thư viện hỗ trợ**

> [!NOTE]
> Bản chỉ mục này hỗ trợ đắc lực cho việc định vị nhanh vị trí file mã nguồn, phục vụ cho việc gỡ lỗi (Debugging), nâng cấp API game (ví dụ từ v67 Frosty Planet lên v70 Scramble) và phát triển các mod mới dựa trên nền tảng có sẵn.

---

## 1. Bảng Tổng Hợp Tác Giả & Quy Mô Dự Án

| STT | Thư mục Nguồn | Tác giả | Số lượng Dự án | Vị trí Nguồn | Thư viện đi kèm |
| :--- | :--- | :--- | :--- | :--- | :--- |
| 1 | [beatlepie_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/beatlepie_source) | beatlepie | 6 | `/_source/beatlepie_source/` | *Không* |
| 2 | [cairath_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/cairath_source) | Cairath | 38 | `/_source/cairath_source/src/` | CaiLib |
| 3 | [doctorfeelgood_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/doctorfeelgood_source) | DoctorFeelGoodMD | 9 | `/_source/doctorfeelgood_source/source/` | *Không* |
| 4 | [glampi_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/glampi_source) | Glampi | 2 | `/_source/glampi_source/` | *Không* |
| 5 | [hilliurn_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/hilliurn_source) | Hilliurn | 1 | `/_source/hilliurn_source/` | *Không* |
| 6 | [peterhan_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/peterhan_source) | Peter Han | 56 | `/_source/peterhan_source/` | PLib (9 modules) |
| 7 | [sanchozz_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/sanchozz_source) | Sanchozz | 57 | `/_source/sanchozz_source/src/` | *Không* |
| 8 | [sgt_imalas_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/sgt_imalas_source) | Sgt_Imalas | 124 | `/_source/sgt_imalas_source/` | UtilLibs, TwitchLib |
| 9 | [AzeTheGreat_source](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/AzeTheGreat_source) | AzeTheGreat | 23 | `/_source/AzeTheGreat_source/src/` | AzeLib |
| 10 | [ony_mods](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/ony_mods) | Ony | 1 | `/_source/ony_mods/` | *Bug Tracker* |

---

## 2. Chi Tiết Danh Sách Mod Theo Tác Giả

### 2.1. Nhóm Mod của Beatlepie (`beatlepie_source`)
*Đường dẫn cục bộ:* [beatlepie_source/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/beatlepie_source)

| STT | Tên Thư mục / Project | Chức năng Dự kiến | Ghi chú |
| :--- | :--- | :--- | :--- |
| 1 | `ChooseNeuralVacillator` | Cho phép tự chọn thuộc tính đột biến nhận được khi Duplicant sử dụng máy Neural Vacillator. | Tiện ích (QoL) |
| 2 | `Condenser` | Thêm máy ngưng tụ giúp hóa lỏng hơi nước hoặc các chất khí một cách hiệu quả. | Công trình |
| 3 | `Custom Building Catagories` | Tự động phân loại và tùy chỉnh danh mục xây dựng công trình trong giao diện game. | Giao diện |
| 4 | `JumpSweepy` | Cải tiến robot Sweepy giúp nó có khả năng nhảy qua chướng ngại vật hoặc ô gạch đứng. | Robot AI |
| 5 | `Reapy` | Thêm robot tự động thu hoạch thực vật hoặc dọn dẹp hoa quả trên mặt đất. | Robot AI |
| 6 | `Vaporizer` | Máy hóa hơi chất lỏng thành thể khí trực tiếp bằng điện năng. | Công trình |

---

### 2.2. Nhóm Mod của Cairath (`cairath_source`)
*Đường dẫn cục bộ:* [cairath_source/src/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/cairath_source/src)

| STT | Tên Thư mục / Project | Chức năng Dự kiến | Phân loại |
| :--- | :--- | :--- | :--- |
| 1 | `AchievementProgress` | Hiển thị chi tiết tiến trình đạt các thành tựu (Achievements) ẩn ngay trong game. | QoL / Giao diện |
| 2 | `AlgaeGrower` | Tinh chỉnh/nâng cấp máy trồng tảo, cải tiến hiệu suất tạo oxy và thụ động tiêu thụ CO2. | Công trình |
| 3 | `BiggerBuildingMenu` | Mở rộng khung lưới hiển thị menu xây dựng giúp nhìn thấy nhiều công trình hơn. | Giao diện |
| 4 | `BiggerCameraZoomOut` | Cho phép camera kéo xa hơn rất nhiều so với mặc định để bao quát toàn bộ tiểu hành tinh. | QoL / Camera |
| 5 | `BuildablePOIProps` | Cho phép người chơi tự xây dựng các đạo cụ trang trí độc lạ chỉ có ở các điểm POI hoang dã. | Trang trí |
| 6 | `CaiLib` | Thư viện lõi hỗ trợ đăng ký cấu trúc, menu và nạp tài nguyên dùng chung cho mọi mod của Cairath. | Library |
| 7 | `ColorfulShinebugs` | Đom đóm (Shinebugs) phát ánh sáng nhiều màu sắc rực rỡ và đẹp mắt hơn. | Đồ họa |
| 8 | `ConfigurableMotionSensorRange` | Cho phép tùy chỉnh phạm vi quét cảm biến chuyển động thông qua file cấu hình. | Tự động hóa |
| 9 | `ConveyorRailUtilities` | Thêm các tiện ích phân nhánh hoặc cầu nối thông minh cho đường băng chuyền. | Vận chuyển |
| 10 | `DebugDoesNotDisableAchievements` | Chế độ Sandbox hoặc Debug của game sẽ không làm vô hiệu hóa các thành tựu Steam. | QoL |
| 11 | `DebugDoesNotDiscoverMap` | Bật chế độ Debug nhưng không tự động mở sáng toàn bộ bản đồ, giữ nguyên sương mù chiến tranh. | QoL |
| 12 | `DecorLights` | Bổ sung các loại đèn trang trí đa dạng về màu sắc và kiểu dáng, tăng decor đáng kể. | Trang trí |
| 13 | `DoubleSweeperRange` | Nhân đôi bán kính hoạt động của cánh tay gắp tự động (Auto-Sweeper). | Vận chuyển |
| 14 | `EerieColors` | Làm lại bảng màu sắc môi trường và ánh sáng của các quần xã, tạo cảm giác kỳ ảo. | Đồ họa |
| 15 | `FasterJetpacks` | Tăng tốc độ bay của Duplicant khi mặc bộ đồ phản lực (Jetpack). | QoL |
| 16 | `Fervine` | Thêm loài cây Fervine hoang dã tỏa nhiệt lượng lớn hoặc hấp thụ khí độc. | Sinh vật |
| 17 | `FlowSplitters` | Thêm bộ chia dòng chảy lỏng/khí tỉ lệ chính xác mà không cần dùng cổng logic phức tạp. | Tiện ích ống dẫn |
| 18 | `GeyserCalculatedAvgOutputTooltip` | Tính toán sản lượng trung bình thực tế (bao gồm cả chu kỳ ngủ/hoạt động) của mạch/núi lửa. | QoL |
| 19 | `LessWasteFromJetpacks` | Bộ đồ phản lực thải ra ít CO2 hơn, tránh làm ngạt các khu vực hoạt động trên cao. | QoL |
| 20 | `LightsOut` | Chế độ bóng tối hoàn toàn (Fog of War cực hạn) - không có nguồn sáng thì màn hình tối đen. | Thử thách |
| 21 | `MarbleTile` | Bổ sung ô gạch đá cẩm thạch cực kỳ sang trọng với điểm trang trí (decor) siêu cao. | Xây dựng |
| 22 | `MosaicTile` | Ô gạch khảm mosaic nghệ thuật với màu sắc thay đổi sinh động. | Xây dựng |
| 23 | `NoLeakyWalls` | Ngăn chặn việc rò rỉ nhiệt độ và khí gas ra không gian chân không qua các ô gạch biên hành tinh. | Xây dựng |
| 24 | `NoLongCommutes` | Vô hiệu hóa hoặc điều chỉnh thuật toán cảnh báo "Long Commutes" gây spam thông báo. | QoL |
| 25 | `NotificationTrigger` | Cho phép thiết lập tín hiệu tự động hóa khi có một thông báo cụ thể xuất hiện trên màn hình. | Tự động hóa |
| 26 | `OilWellAnyWater` | Cho phép sử dụng nước ô nhiễm hoặc nước muối để bơm vào giếng dầu thay vì bắt buộc dùng nước sạch. | Tiện ích |
| 27 | `PaintWalls` | Công cụ cho phép sơn màu tùy chọn lên các ô gạch sàn và tường nền Drywall. | Trang trí |
| 28 | `PalmeraTree` | Thêm loài cây Palmera có quả ăn được và sinh trưởng tốt trong môi trường khí Clo. | Sinh vật |
| 29 | `PipedAlgaeTerrarium` | Bổ sung cổng kết nối ống nước vào bồn tảo, loại bỏ việc Duplicant phải xách nước thủ công. | Nông nghiệp |
| 30 | `PlanBuildingsWithoutMaterials` | Cho phép đặt lệnh lập kế hoạch xây dựng công trình ngay cả khi trong kho chưa có tài nguyên đó. | QoL |
| 31 | `RefinedMetalsUsableAsRawMetals` | Cho phép dùng trực tiếp kim loại tinh chế để xây dựng các công trình chỉ yêu cầu kim loại thô. | Xây dựng |
| 32 | `ShowIndustrialMachineryTag` | Hiển thị rõ ràng tag "Industrial Machinery" trên các công trình để người chơi thiết kế phòng máy. | Giao diện |
| 33 | `SteelLadder` | Thang làm bằng thép giúp Duplicant leo trèo nhanh hơn 50% so với thang thường. | Xây dựng |
| 34 | `SuitDockStores75Kg` | Trạm sạc lưu trữ lượng oxy nhiều hơn gấp đôi (75kg) giúp sạc suit nhanh và liên tục. | QoL |
| 35 | `Wallpaper` | Giấy dán tường nhiều màu sắc để trang trí nền gạch, che đi các khoảng trống xấu xí. | Trang trí |
| 36 | `WirelessAutomation` | Hệ thống truyền dẫn tín hiệu tự động hóa không dây thông qua các máy phát/nhận tần số. | Tự động hóa |
| 37 | `WoundedGoToMedBed` | AI thông minh tự động điều hướng Duplicant bị thương nhẹ đi nằm giường y tế thay vì cố làm việc. | QoL / AI |

---

### 2.3. Nhóm Mod của DoctorFeelGoodMD (`doctorfeelgood_source`)
*Đường dẫn cục bộ:* [doctorfeelgood_source/source/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/doctorfeelgood_source/source)

| STT | Tên Thư mục / Project | Chức năng Dự kiến | Phân loại |
| :--- | :--- | :--- | :--- |
| 1 | `ConveyorLoadersAreNotMachinery` | Đầu nạp băng chuyền không bị tính là máy móc công nghiệp (giảm decor phạt trong phòng ngủ/ăn). | QoL |
| 2 | `DebrisMeltsToDebris` | Mảnh vụn chất rắn khi đạt nhiệt độ nóng chảy sẽ chuyển thành mảnh vụn nguyên tố mới thay vì hóa lỏng. | Vật lý |
| 3 | `HatchesDon'tEatMeat` | Con Hatch hoang dã/nuôi sẽ không bao giờ ăn thịt rơi vãi trên sàn nhà, bảo vệ kho thực phẩm. | Thú nuôi |
| 4 | `MoveThisHere` | Thêm công trình Gom đồ (Hauling Point) giúp vận chuyển tài nguyên nhanh gọn không cần xây rương chứa. | QoL |
| 5 | `NoEmptyConveyors` | Băng chuyền thông minh tự ngắt hoặc dừng hoạt động khi không có vật phẩm, giảm lag CPU. | Tối ưu |
| 6 | `NoWaterTofu` | Thay đổi công thức chế biến đậu phụ (Tofu) tại máy nấu ăn, giảm thiểu hoặc loại bỏ yêu cầu nước. | Thực phẩm |
| 7 | `PetroleumDoesntBreakPipes` | Đường ống dẫn dầu mỏ/xăng sẽ không bị nứt vỡ do sốc nhiệt độ cực hạn. | Tiện ích ống dẫn |
| 8 | `RefuelTurnedoffGenerators` | Duplicant vẫn có thể mang than/nhiên liệu nạp sẵn vào máy phát điện ngay cả khi máy đang tắt bằng tín hiệu logic. | QoL |
| 9 | `UntameCritters` | Cho phép dùng một công cụ chuyên dụng để thả thú nuôi đã thuần hóa trở lại hoang dã. | Thú nuôi |

---

### 2.4. Nhóm Mod của Glampi (`glampi_source`)
*Đường dẫn cục bộ:* [glampi_source/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/glampi_source)

| STT | Tên Thư mục / Project | Chức năng Dự kiến | Phân loại |
| :--- | :--- | :--- | :--- |
| 1 | `AutoDropBottlers` | Tự động xả chai lỏng/khí từ trạm nạp ngay khi đầy để tránh tắc nghẽn trạm. | Tự động hóa |
| 2 | `ChainErrand` | Cho phép người chơi liên kết các hành động (đào, xây, dọn dẹp) thành một chuỗi ưu tiên thực hiện liền mạch. | QoL |

---

### 2.5. Nhóm Mod của Hilliurn (`hilliurn_source`)
*Đường dẫn cục bộ:* [hilliurn_source/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/hilliurn_source)

| STT | Tên Thư mục / Project | Chức năng Dự kiến | Phân loại |
| :--- | :--- | :--- | :--- |
| 1 | `Auto Liquid Bottler` | Phiên bản tự động hóa của trạm đóng chai chất lỏng, tự động hút nước từ ống và đóng chai thả ra sàn. | Công trình |

---

### 2.6. Nhóm Mod & Thư Viện của Peter Han (`peterhan_source`)
*Đường dẫn cục bộ:* [peterhan_source/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/peterhan_source)

Đây là kho lưu trữ của **Peter Han**, tác giả nổi tiếng bậc nhất trong cộng đồng ONI modding với bộ thư viện **PLib** huyền thoại.

#### Các Module Thư viện Cốt lõi (PLib):
- `PLib`: Bộ thư viện tổng hợp cốt lõi.
- `PLibCore`: Lõi điều hành nạp và đăng ký Harmony Patch.
- `PLibAVC`: Tự động kiểm tra phiên bản mod trực tuyến (Auto Version Checker).
- `PLibActions`: Đăng ký các phím tắt (keybinds) tùy biến trong game.
- `PLibBuildings`: Hỗ trợ đăng ký và cấu hình các công trình mới cực kỳ dễ dàng.
- `PLibDatabase`: Quản lý lưu trữ/đọc dữ liệu cấu hình tùy chọn của mod.
- `PLibLighting`: Đăng ký các loại ánh sáng, góc chiếu và độ sáng mở rộng.
- `PLibOptions`: Tạo giao diện Menu cấu hình chuyên nghiệp cho mod ngay trong game.
- `PLibUI`: Cung cấp các widget, cửa sổ giao diện tùy biến chất lượng cao.

#### Chi tiết các Mod của Peter Han:
| STT | Tên Thư mục / Project | Chức năng Dự kiến | Phân loại |
| :--- | :--- | :--- | :--- |
| 1 | `AIImprovements` | Cải tiến toàn diện trí thông minh nhân tạo (AI) của Duplicant giúp tìm đường thông minh hơn. | Tối ưu / AI |
| 2 | `AirlockDoor` | Cửa gió chân không thực tế, ngăn chặn triệt để sự thất thoát nhiệt và khí gas khi đóng mở. | Xây dựng |
| 3 | `AutoEject` | Tự động xả các vật phẩm/chất thải tích tụ ra khỏi các công trình lưu trữ mà không cần Duplicant can thiệp. | Tự động hóa |
| 4 | `BuildStraightUp` | Cho phép Duplicant xây dựng các bức tường cao thẳng đứng mà không cần bắc giàn giáo rườm rà. | QoL |
| 5 | `BulkSettingsChange` | Công cụ quét diện rộng cho phép thay đổi cấu hình cài đặt hàng loạt công trình giống nhau cùng lúc. | QoL |
| 6 | `Challenge100K` | Kích hoạt chế độ thử thách nhiệt độ toàn tiểu hành tinh giảm sâu xuống 100 Kelvin (-173°C). | Thử thách |
| 7 | `Claustrophobia` | Thêm thuộc tính tiêu cực "Sợ không gian hẹp", làm giảm Morale dữ dội khi Duplicant ở phòng nhỏ. | Độ khó |
| 8 | `CleanDrop` | Đảm bảo các chất lỏng/khí khi xả ra từ chai sẽ rơi thẳng đứng và sạch sẽ, không tạo bọt khí vương vãi. | QoL |
| 9 | `CritterInventory` | Hiển thị trực quan số lượng tất cả các loại thú nuôi đang có trên hành tinh trong bảng tài nguyên. | QoL |
| 10 | `DebugNotIncluded` | Gói công cụ mở rộng các phím tắt, giao diện gỡ lỗi cao cấp dành riêng cho nhà phát triển mod. | Công cụ Dev |
| 11 | `DecorReimagined` | Thiết kế lại hoàn toàn cách tính điểm trang trí (Decor), giúp việc làm đẹp căn cứ có ý nghĩa thực tế hơn. | Hệ thống |
| 12 | `DeselectNewMaterials` | Tự động bỏ chọn vật liệu mới phát hiện khi xây dựng, tránh việc vô tình dùng kim loại hiếm làm gạch. | QoL |
| 13 | `EfficientFetch` | Tối ưu hóa cách thức gom đồ của Duplicant, họ sẽ gom đầy túi từ nhiều đống gần nhau trước khi đi cất. | Tối ưu / AI |
| 14 | `FallingSand` | Tự động phát hiện và phát lệnh đào khẩn cấp khi có cát/sỏi rơi tự do đè lên công trình hoặc Duplicant. | QoL |
| 15 | `FastSave` | Dọn dẹp dữ liệu rác, lược bỏ lịch sử di chuyển thừa của Duplicant để giảm dung lượng file save và tăng tốc độ lưu game. | Tối ưu cực mạnh |
| 16 | `FastTrack` | Mod tối ưu hóa hiệu năng đỉnh cao cho ONI, viết lại hàng loạt thuật toán kiểm tra nhiệt độ, đường ống và AI. | Tối ưu siêu cấp |
| 17 | `FinishTasks` | Buộc Duplicant hoàn thành dứt điểm công việc hiện tại (ví dụ lau nhà, xây nốt gạch) trước khi đi ăn hoặc ngủ. | QoL |
| 18 | `FoodTooltip` | Hiển thị bảng thông tin cực kỳ chi tiết về dinh dưỡng, hạn sử dụng, hiệu ứng của thực phẩm trong tooltip. | Giao diện |
| 19 | `ForbidItems` | Thêm nút cấm sử dụng/cấm ăn/cấm tương tác với các vật phẩm cụ thể ngay tại chỗ. | QoL |
| 20 | `MismatchedFinder` | Quét và làm nổi bật các đường ống dẫn hoặc dây cáp điện xây sai kích cỡ/sai loại gây tắc nghẽn. | Công cụ quét |
| 21 | `ModUpdateDate` | Hiển thị ngày cập nhật lần cuối của mod ngay trên danh sách quản lý mod trong game. | Giao diện |
| 22 | `MooReproduction` | Chỉnh sửa cơ chế sinh sản và ăn uống của loài Moo (Bò vũ trụ) để có thể chăn nuôi bền vững. | Thú nuôi |
| 23 | `MoreAchievements` | Thêm 20+ thành tựu mới đầy thử thách trong quá trình chơi game từ sơ khai đến vũ trụ. | Thử thách |
| 24 | `NoSensorLimits` | Loại bỏ hoàn toàn các giới hạn tối đa/tối thiểu của cảm biến nhiệt độ, áp suất, logic. | Tự động hóa |
| 25 | `NoSplashScreen` | Bỏ qua màn hình chào mừng (Splash Screen) logo Klei khi khởi chạy game để rút ngắn thời gian load. | Tối ưu |
| 26 | `NoWasteWant` | Tối ưu thuật toán ăn uống, Duplicant chỉ ăn vừa đủ lượng calo thiếu hụt, tránh lãng phí đồ ăn thừa. | QoL |
| 27 | `NotEnoughTags` | Bổ sung các tag phân loại chi tiết giúp người chơi lọc tài nguyên trong rương chứa dễ dàng hơn. | QoL |
| 28 | `OldPipeColor` | Khôi phục lại màu sắc đặc trưng của đường ống dẫn chất lỏng/khí thời kỳ đầu của game. | Đồ họa |
| 29 | `PipPlantOverlay` | Lớp phủ hình ảnh chỉ ra chính xác các ô đất Pip có thể trồng cây tự nhiên dựa trên khoảng cách. | Công cụ nông nghiệp |
| 30 | `PreserveSeed` | Thêm hộp bảo quản hạt giống chân không giúp hạt giống không bao giờ bị phân hủy. | Nông nghiệp |
| 31 | `QueueForSink` | Bắt buộc Duplicant phải xếp hàng rửa tay tại bồn nếu bồn đang bận, ngăn chặn đi qua mang vi khuẩn. | QoL / AI |
| 32 | `Resculpt` | Cho phép các nhà điêu khắc sửa lại các bức tượng xấu mà không cần đập bỏ xây lại từ đầu. | QoL |
| 33 | `ResearchQueue` | Cho phép lập hàng đợi nghiên cứu nhiều công nghệ cùng lúc, tự động chuyển tiếp khi xong. | QoL |
| 34 | `ResourcesInMotion` | Thêm hiệu ứng hoạt họa mượt mà biểu diễn tài nguyên đang trượt trên băng chuyền. | Đồ họa |
| 35 | `SandboxTools` | Bổ sung các cọ quét, công cụ chỉnh sửa địa hình cao cấp trong chế độ Sandbox. | Công cụ Sandbox |
| 36 | `ShowRange` | Hiển thị bán kính/phạm vi tác động của tất cả công trình (đèn, máy tỏa nhiệt, máy khử trùng). | Giao diện |
| 37 | `SmartPumps` | Máy bơm chất lỏng/khí thông minh tự dừng hoạt động nếu phát hiện tạp chất chạy vào. | Công trình |
| 38 | `StarmapQueue` | Lập lịch trình bay hàng loạt cho tên lửa trên bản đồ sao tự động. | Vũ trụ |
| 39 | `StockBugFix` | Vá hàng loạt lỗi vụn vặt và lỗi logic ẩn trong game gốc mà nhà phát hành Klei chưa kịp sửa. | Hệ thống |
| 40 | `SweepByType` | Thêm tùy chọn chỉ quét dọn các loại tài nguyên cụ thể được chọn trong menu quét. | QoL |
| 41 | `ThermalPlate` | Thêm các tấm truyền nhiệt/cách nhiệt chuyên dụng với độ tùy biến cao về hệ số dẫn nhiệt. | Xây dựng |
| 42 | `ThermalTooltips` | Bảng tooltip hiển thị chi tiết nhiệt năng tích tụ, nhiệt dung riêng của từng thực thể. | Giao diện |
| 43 | `TileTempSensor` | Ô gạch sàn tích hợp sẵn cảm biến nhiệt độ bên trong, tiết kiệm không gian xây dựng. | Tự động hóa |
| 44 | `ToastControl` | Cho phép bật/tắt hoặc ẩn các thông báo Toast bay nhảy gây mất tập trung. | Giao diện |
| 45 | `TraitRework` | Cân bằng lại toàn bộ các thuộc tính của Duplicant, giảm bớt tác hại của các trait quá phế. | Hệ thống |
| 46 | `TurnBackTheClock` | Cho phép lưu trữ tự động nhiều mốc thời gian cũ hơn để dễ dàng rollback khi thảm họa xảy ra. | QoL |
| 47 | `WorkshopProfiles` | Quản lý, sao lưu và khôi phục các hồ sơ cấu hình mod (Profiles) cực kỳ tiện lợi. | QoL |

---

### 2.7. Nhóm Mod của Sanchozz (`sanchozz_source`)
*Đường dẫn cục bộ:* [sanchozz_source/src/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/sanchozz_source/src)

| STT | Tên Thư mục / Project | Chức năng Dự kiến | Phân loại |
| :--- | :--- | :--- | :--- |
| 1 | `0Cleaner` | Tiện ích dọn dẹp các cache thừa hoặc log file của mod để giảm tải ổ đĩa. | Tiện ích |
| 2 | `AnyIceKettle` | Cho phép sử dụng mọi loại băng tuyết có trong game để đun sôi thành nước uống. | Tiện ích |
| 3 | `AnyIceMachine` | Máy làm đá đa năng, có thể làm đông đá từ nước muối hoặc nước ô nhiễm. | Công trình |
| 4 | `AquaticFarm` | Ô đất trồng trọt chuyên dụng dưới nước dành cho các loại cây ngập mặn. | Nông nghiệp |
| 5 | `Archaeologist` | Mở rộng thuộc tính khảo cổ học, tăng tỉ lệ khai quật được công nghệ cổ đại. | Tiện ích |
| 6 | `ArtifactCarePackages` | Bổ sung các cổ vật vũ trụ hiếm vào danh sách Care Package của cổng in. | QoL |
| 7 | `AthleticsGenerator` | Máy phát điện chạy bằng vòng quay thể dục, giúp tăng chỉ số Athletics cực nhanh. | Công trình |
| 8 | `AttributeRestrictions` | Cho phép thiết lập bảng phân quyền: chỉ Duplicant có chỉ số cụ thể mới được dùng máy. | QoL / AI |
| 9 | `AutoComposter` | Máy ủ phân (Compost) tự động xả đất ra đất và tiếp tục ủ không cần Duplicant lật thủ công. | Tự động hóa |
| 10 | `AutomaticDispenserOnlyTransferFromLowerPriority` | Ngăn chặn vòng lặp vô tận: máy phân phối tự động chỉ nhận đồ từ rương có mức ưu tiên thấp hơn. | Logic hệ thống |
| 11 | `BetterPlantTending` | Cải tiến hành vi chăm sóc cây trồng của Duplicant, giúp nông nghiệp đạt hiệu quả cao hơn. | Nông nghiệp |
| 12 | `BuildableGeneShuffler` | Cho phép chế tạo và xây dựng máy biến đổi gen (Neural Vacillator) bằng vật liệu siêu cấp. | Xây dựng |
| 13 | `ButcherStation` | Trạm giết mổ gia súc tự động giúp quản lý số lượng thú nuôi và tự động thu hoạch thịt. | Thú nuôi |
| 14 | `CarouselCentrifuge` | Máy ly tâm giải trí đu quay tăng mạnh chỉ số Morale cho Duplicant. | Giải trí |
| 15 | `ControlYourRobots` | Bảng điều khiển cho phép ra lệnh trực tiếp cho robot Sweepy và Rover làm việc cụ thể. | Robot AI |
| 16 | `CorpseOnPedestal` | Cho phép trưng bày các xác sinh vật hoặc bia mộ kỷ niệm của Duplicant lên bệ trưng bày. | Trang trí |
| 17 | `CrabsFlippCompost` | Cải tiến loài cua Pinchers tự động bò đến lật đống ủ phân hộ Duplicant. | Thú nuôi |
| 18 | `CrabsProfit` | Khai thác các sản phẩm phụ từ loài cua (vỏ cua dùng làm vôi, thịt cua, v.v.). | Thú nuôi |
| 19 | `DeadPlantsNotifier` | Gửi cảnh báo đỏ khẩn cấp ngay khi có cây trồng trong trang trại bị chết do thiếu điều kiện. | Giao diện |
| 20 | `DeathReimagined` | Làm lại cơ chế Duplicant qua đời, ảnh hưởng tâm lý dây chuyền đến các Duplicant khác thực tế hơn. | Hệ thống |
| 21 | `DualDiningTable` | Bàn ăn đôi cho phép 2 Duplicant ngồi ăn đối diện trò chuyện nâng cao Morale. | Trang trí |
| 22 | `DumpIncorrectFertilizers` | Tự động đẩy các loại phân bón không phù hợp ra khỏi máy trồng cây nếu điều kiện nhiệt độ thay đổi. | Nông nghiệp |
| 23 | `EatEveryDay` | Buộc Duplicant phải duy trì bữa ăn hàng ngày thay vì nhịn đói dồn calo nhiều chu kỳ. | Thử thách |
| 24 | `EndlessTelescope` | Kính viễn vọng ngoài không gian không giới hạn tầm nhìn phát hiện các hành tinh mới. | Vũ trụ |
| 25 | `ExoticSpices` | Thêm các loại gia vị mới chế biến từ hoa quả hiếm tại máy nghiền gia vị. | Thực phẩm |
| 26 | `ExplorerBooster` | Cung cấp thiết bị hỗ trợ tăng tốc độ thám hiểm và di chuyển ngoài không gian hành tinh. | Vũ trụ |
| 27 | `GraveyardKeeper` | Thêm công việc chuyên trách chăm nom nghĩa trang, giúp giảm stress cho căn cứ khi có người mất. | QoL |
| 28 | `HEPBridgeInsulationTile` | Ô gạch cách nhiệt đặc biệt giúp ống dẫn hạt Radbolt đi qua không bị rò rỉ nhiệt độ. | Xây dựng |
| 29 | `Hydrocactus` | Thêm giống xương rồng thủy sinh hấp thụ nước ô nhiễm và giải phóng oxy sạch thụ động. | Sinh vật |
| 30 | `Lagoo` | Bổ sung sinh vật đầm lầy mới Lagoo với chuỗi thức ăn độc đáo. | Sinh vật |
| 31 | `LargeTelescope` | Kính viễn vọng khổng lồ đặt trên bề mặt hành tinh với độ chính xác cao. | Vũ trụ |
| 32 | `MSBuildTasksHelper` | Bộ công cụ tự động hóa việc build/compile mã nguồn C# và copy sang thư mục game. | Công cụ Dev |
| 33 | `MechanicsStation` | Trạm bảo trì cơ khí giúp bôi trơn máy móc xung quanh tăng tốc độ hoạt động. | Công trình |
| 34 | `MooDiet` | Đơn giản hóa chế độ ăn của loài Moo, cho phép chúng ăn cỏ thường thay vì chỉ ăn cỏ khí Clo. | Thú nuôi |
| 35 | `MoreEmotions` | Thêm các biểu cảm khuôn mặt và trạng thái tâm lý phong phú khi Duplicant rảnh rỗi. | Đồ họa |
| 36 | `MorePlantMutations` | Bổ sung 15 loại đột biến gen cây trồng mới cực kỳ độc đáo và hữu ích. | Nông nghiệp |
| 37 | `NoManualDelivery` | Ngăn chặn triệt để hành vi Duplicant chạy đến cấp liệu thủ công cho các máy đã có băng chuyền. | Tự động hóa |
| 38 | `OldLiquidReservoir` | Đổi lại skin bồn chứa chất lỏng giống phiên bản cổ điển được yêu thích. | Đồ họa |
| 39 | `PickupFloppingPacu` | Duplicant tự động đến nhặt cá Pacu đang giãy đành đạch trên cạn bỏ vào bể nước gần nhất. | Thú nuôi |
| 40 | `PotatoElectrobanks` | Pin củ khoai tây vui nhộn sử dụng dòng điện sinh học công suất nhỏ. | Công trình |
| 41 | `ReBuildableAETN` | Cho phép phá dỡ cỗ máy Entropy khổng lồ (AETN) để lấy nguyên liệu hoặc xây lại ở vị trí khác. | Xây dựng |
| 42 | `RoverRefueling` | Thêm trạm sạc điện năng cho robot Rover ngoài không gian, kéo dài tuổi thọ robot. | Vũ trụ |
| 43 | `SandboxMutantPlant` | Cho phép người chơi chọn nhanh các đột biến cây trồng trong menu Sandbox. | Công cụ Sandbox |
| 44 | `SmartLogicDoors` | Cửa tích hợp sẵn các cổng logic And/Or giúp đóng mở tự động thông minh. | Tự động hóa |
| 45 | `Smelter` | Lò luyện kim thô kích thước 1x2 nhỏ gọn cho giai đoạn đầu game. | Công trình |
| 46 | `SquirrelGenerator` | Máy phát điện chạy bằng con sóc Pip chạy trên bánh xe gỗ. | Công trình |
| 47 | `SuitRecharger` | Trạm sạc oxy khẩn cấp cho suit đặt trên hành lang di chuyển xa. | QoL |
| 48 | `SupplyToClosest` | AI ưu tiên mang tài nguyên cấp cho các công trình ở khoảng cách gần nhất trước. | Tối ưu / AI |
| 49 | `TravelTubesExpanded` | Thêm các khớp nối chữ T, ngã tư và hệ thống chia làn cho ống trung chuyển nhanh. | Vận chuyển |
| 50 | `TravelTubesFlydo` | Robot bay vận chuyển tự động chạy trong mạng lưới ống trung chuyển chất khí. | Vận chuyển |
| 51 | `TweakedBiologicalCargoBay` | Tinh chỉnh khoang sinh học chứa thú nuôi của tàu vũ trụ, tăng sức chứa tối đa. | Vũ trụ |
| 52 | `VaricolouredBalloons` | Thêm bóng bay nghệ thuật với nhiều màu sắc rực rỡ và chuyển động ngộ nghĩnh. | Trang trí |
| 53 | `VitalsDisplayRads` | Hiển thị mức nhiễm phóng xạ của từng Duplicant trực quan ngay trên bảng điều khiển chính. | Giao diện |
| 54 | `WhereMyLoot` | Công cụ tìm kiếm nhanh vật phẩm bị rơi vãi dưới đống đổ nát hoặc bị chôn sâu. | QoL |
| 55 | `WornSuitDischarge` | Tự động thu hồi oxy và nước ô nhiễm còn sót lại trong suit rách khi cởi ra. | QoL |
| 56 | `WrangleCarry` | Duplicant có thể trói sinh vật và mang đi ngay lập tức thay vì trói xong bỏ đó đi làm việc khác. | Thú nuôi |

---

### 2.8. Nhóm Mod & Dự án của Sgt_Imalas (`sgt_imalas_source`)
*Đường dẫn cục bộ:* [sgt_imalas_source/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/sgt_imalas_source)

Đây là kho mã nguồn khổng lồ của **Sgt_Imalas**, một modder cực kỳ năng suất. Dưới đây là các dự án mod nổi bật nhất trong tổng số 120+ thư mục:

| STT | Tên Thư mục / Project | Chức năng Dự kiến | Phân loại |
| :--- | :--- | :--- | :--- |
| 1 | `3GuBsVisualFixesNTweaks` | Tinh chỉnh hiển thị đồ họa và hiệu năng cho các hiệu ứng khói, nước chảy. | Đồ họa |
| 2 | `AkiTrueTiles_SkinSelectorAddon` | Addon cho phép chọn skin ô gạch sàn tương thích với mod TrueTiles của Aki. | Đồ họa |
| 3 | `AkisDecorPackB` | Gói đồ trang trí chất lượng cao cộng tác với Aki (tượng mới, đèn mới). | Trang trí |
| 4 | `AkisSnowThings` | Gói nội dung trang trí phong cách mùa đông băng giá (người tuyết, đèn tuyết). | Trang trí |
| 5 | `AllRefinedMetalsAsOres` | Cho phép sử dụng kim loại tinh chế thay thế cho quặng thô khi xây dựng các cấu trúc cơ bản. | Xây dựng |
| 6 | `AmogusMorb` | Thay đổi kết cấu đồ họa của sinh vật Morb thành các nhân vật Among Us. | Đồ họa |
| 7 | `BathTub` | Bồn tắm nằm thư giãn sang trọng giúp Duplicant xả Stress cực nhanh và tăng mạnh Morale. | Giải trí |
| 8 | `BawoonFwiend` | Bổ sung các quả bóng bay hình thú cưng di động bay theo chân Duplicant để tăng độ vui vẻ. | Trang trí |
| 9 | `BetterCritterMorpher` | Tối ưu hóa máy biến đổi gen thú nuôi, giúp quá trình biến đổi diễn ra chính xác hơn. | Thú nuôi |
| 10 | `BigSmallSculptures` | Cho phép xây dựng các tác phẩm điêu khắc nghệ thuật với kích thước khổng lồ hoặc siêu nhỏ. | Trang trí |
| 11 | `BioluminescentDupes` | Duplicant có thể mang đặc tính phát sáng sinh học, tự soi đường đi trong bóng đêm. | QoL |
| 12 | `BlueprintsV2` | Bản nâng cấp thế hệ 2 của mod Blueprints, cho phép vẽ, lưu và dán thiết kế cực mạnh. | QoL cực mạnh |
| 13 | `CannedFoodNGoods` | Hệ thống bảo quản thực phẩm bằng phương pháp đóng hộp kim loại, lưu trữ vô thời hạn. | Thực phẩm |
| 14 | `Cheese` | Bổ sung chuỗi sản xuất phô mai khổng lồ (vắt sữa bò Moo, ủ men phô mai, chế biến món ăn ngon). | Nội dung mở rộng |
| 15 | `ClaimNotification` | Nhắc nhở người chơi bằng thông báo nổi bật khi Cổng In có quà tiếp tế chưa nhận. | Giao diện |
| 16 | `ClusterTraitGenerationManager` [✅ Hoạt động] | Trình quản lý cho phép tùy biến chi tiết các đặc tính (Traits) của hành tinh khi tạo game mới. | Thế giới |
| 17 | `ComplexFabricatorRibbonController` | Hỗ trợ điều khiển tự động hóa các máy chế tạo phức tạp bằng cáp tín hiệu dẹt Ribbon. | Tự động hóa |
| 18 | `CompressedCritters` | Cho phép đóng gói thú nuôi thành dạng hộp nhỏ gọn để vận chuyển hàng loạt bằng băng chuyền. | Thú nuôi |
| 19 | `ConveyorTiles` | Ô gạch tích hợp sẵn đường băng chuyền ngầm chạy xuyên qua, tiết kiệm không gian xây dựng. | Xận chuyển |
| 20 | `CritterTraitsReborn` | Hồi sinh hệ thống đặc tính (Traits) ngẫu nhiên cho thú nuôi khi sinh ra (ví dụ: ăn nhiều, chạy nhanh). | Thú nuôi |
| 21 | `CrittersShedFurOnBrush` | Thú nuôi có lông (như Drecko) sẽ rụng thêm lông khi được chải chuốt tại trạm để lấy nguyên liệu dệt. | Thú nuôi |
| 22 | `Cryopod` | Công trình buồng đông lạnh đưa Duplicant vào trạng thái ngủ đông, ngưng tiêu thụ oxy/thức ăn. | Xây dựng |
| 23 | `CustomGameSettingsModifier` | Cho phép can thiệp sâu để chỉnh sửa các thiết lập độ khó ngay cả khi game đang diễn ra. | QoL |
| 24 | `DailyRoutine` | Thiết lập thời gian biểu hoạt động chi tiết từng giờ cho Duplicant (giờ tắm, giờ chơi, giờ làm). | QoL |
| 25 | `DemoliorStoryTrait` | Đặc tính cốt truyện mới khám phá sinh vật khổng lồ cổ đại Demolior hoang dã. | Nội dung mở rộng |
| 26 | `DigUpFossils` | Cho phép đào và khai quật các mỏ hóa thạch lớn để chế biến thành vôi (Lime) chất lượng cao. | Xây dựng |
| 27 | `DontCrashMyMods` | Bộ lọc bắt lỗi Exception thông minh giúp game không bị văng (Crash to Desktop) khi mod lỗi. | Hệ thống |
| 28 | `DupePodRailgun` | Súng điện từ bắn kén vận chuyển Duplicant qua lại giữa các hành tinh mà không cần tên lửa. | Vũ trụ |
| 29 | `DupePrioPresetManager` [✅ Hoạt động] | Trình quản lý lưu/tải các thiết lập ưu tiên công việc (Priorities) cho Duplicant theo nhóm. | Giao diện |
| 30 | `DupeTransportViaNetwork` | Dịch chuyển tức thời Duplicant xuyên qua mạng lưới đường ống năng lượng cao. | Vận chuyển |
| 31 | `FlybotRoboPort` | Trạm sạc cho Flybot - robot bay tự động sửa chữa các công trình bị hỏng trên bề mặt. | Robot AI |
| 32 | `ForceFieldWallTile` | Ô gạch tường lực trường chặn đứng khí và nước nhưng cho phép Duplicant đi xuyên qua tự do. | Xây dựng |
| 33 | `GeothermalStoryTrait` | Cốt truyện khai thác năng lượng địa nhiệt khổng lồ từ lõi hành tinh bằng hệ thống đường ống lớn. | Nội dung mở rộng |
| 34 | `LaserMeteorBlasterCannon` | Pháo phòng không tự động bắn tia laser tiêu diệt thiên thạch bảo vệ an toàn bề mặt hành tinh. | Phòng thủ bề mặt |
| 35 | `LightBridge` | Cầu ánh sáng cho phép Duplicant đi qua khi bật điện, tự động tắt đi bằng tín hiệu logic. | Xây dựng |
| 36 | `LocalModLoader` | Trình nạp mod cục bộ trực tiếp từ thư mục game, bỏ qua hoàn toàn cơ chế đồng bộ của Steam. | Tiện ích Dev |
| 37 | `MassMoveTo` | Công cụ cho phép chọn diện rộng ra lệnh di chuyển hàng loạt vật thể rơi vãi về một vị trí. | QoL |
| 38 | `MeteorShield` | Khiên năng lượng khổng lồ bảo vệ toàn bộ bề mặt khỏi các trận mưa thiên thạch tàn phá. | Phòng thủ bề mặt |
| 39 | `NeutroniumTrashCan` | Thùng rác làm từ Neutronium, hủy diệt vĩnh viễn mọi chất rắn, chất lỏng, chất khí ném vào đó. | Tiện ích |
| 40 | `NukeRocket` | Động cơ tên lửa hạt nhân cực mạnh sử dụng Uranium tinh chế với tốc độ bay cực cao. | Vũ trụ |
| 41 | `PaintYourPipes` | Sơn màu sắc tùy ý cho các đường ống dẫn nước, dẫn khí và đường dây điện. | Trang trí |
| 42 | `Robo Rockets` | Hệ thống phi hành gia robot AI tự động điều khiển tàu vũ trụ thám hiểm thay cho Duplicant. | Vũ trụ mở rộng |
| 43 | `Rockets-TinyYetBig` | Gói thiết kế lại các module tên lửa nhỏ gọn hơn nhưng mang lại dung tích lưu trữ cực lớn. | Vũ trụ |
| 44 | `RotatableRadboltStorage` | Thùng chứa Radbolt có khớp xoay 360 độ giúp bắn luồng hạt năng lượng theo mọi hướng logic. | Tự động hóa |
| 45 | `StoragePiles` | Đống chứa vật liệu ngoài trời với sức chứa cực lớn, không làm giảm decor như rương sắt. | Xây dựng |
| 46 | `TrainMod` | Hệ thống đường ray xe lửa vận chuyển khối lượng lớn tài nguyên và Duplicant chạy dọc căn cứ. | Vận chuyển lớn |
| 47 | `UndigYourself` | AI Duplicant tự động đào bới giải cứu bản thân khi vô tình bị cát/sỏi sạt lở chôn vùi. | QoL / AI |
| 48 | `UtilLibs` | Bộ thư viện tiện ích cốt lõi hỗ trợ vẽ lưới xây dựng, tính toán quỹ đạo và kết nối logic. | Library |
| 49 | `ItemDropPrevention` [✅ Hoạt động] | Ngăn chặn Duplicant đánh rơi vật phẩm trên tay khi bị gián đoạn công việc (No drop). | QoL |
| 50 | `SGTIM_NotificationManager` [✅ Hoạt động] | Quản lý và chặn các thông báo rác, toast thông báo phiền phức xuất hiện trên màn hình (Suppress noti). | Giao diện |

---

### 2.9. Nhóm Mod của AzeTheGreat (`AzeTheGreat_source`)
*Đường dẫn cục bộ:* [AzeTheGreat_source/src/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/AzeTheGreat_source/src)

| STT | Tên Thư mục / Project | Chức năng Dự kiến | Phân loại |
| :--- | :--- | :--- | :--- |
| 1 | `AzeLib` | Thư viện lõi chứa code tiện ích mở rộng (Extensions) cho UI, LogicPorts dùng chung. | Library |
| 2 | `BetterDeselect` | Cải tiến cơ chế bỏ chọn công cụ khi ấn chuột phải mà không ảnh hưởng thao tác khác. | QoL |
| 3 | `BetterInfoCards` [✅ Hoạt động] | Thiết kế lại hoàn toàn thẻ thông tin (Info Card) ở góc dưới phải, trực quan và gọn hơn. | Giao diện |
| 4 | `BetterLogicOverlay` [✅ Hoạt động] | Tối ưu hóa lớp phủ Mạng tự động hóa (Logic Overlay), hiển thị số tín hiệu và dải ruy băng. | Giao diện / Logic |
| 5 | `BuildMenuSearchHotkey` | Thêm phím tắt giúp mở nhanh thanh tìm kiếm công trình trong menu xây dựng. | Giao diện |
| 6 | `ClarifiedMaxDecor` | Làm rõ hiển thị ngưỡng Decor (Điểm trang trí) tối đa mà một Duplicant có thể nhận. | Giao diện |
| 7 | `CleanFloors` | Ẩn bớt các hiệu ứng mảnh vỡ, nứt nẻ thừa thãi trên mặt sàn/gạch giúp gọn mắt hơn. | Đồ họa |
| 8 | `CleanHUD` | Tùy biến và ẩn bớt các thành phần thừa trên giao diện người dùng (HUD). | Giao diện |
| 9 | `DefaultBuildingSettings` | Lưu và thiết lập sẵn cấu hình mặc định (ví dụ: nhiệt độ, tín hiệu) cho công trình mới xây. | QoL |
| 10 | `DefaultSaveSettings` | Ghi nhớ tùy chỉnh cấu hình lưu game. | QoL |
| 11 | `DeleteFullWord` | Cho phép sử dụng Ctrl+Backspace để xóa nguyên một từ trong các khung nhập văn bản. | QoL |
| 12 | `DrinkTeaNotCoffee` | Đổi tên/hình ảnh máy pha cà phê thành máy pha trà (Espresso Machine -> Tea Machine). | Giải trí |
| 13 | `FixedCameraPan` | Sửa lỗi trượt camera, giúp góc nhìn cố định vững chắc hơn khi di chuyển chuột. | QoL / Camera |
| 14 | `NoDoorIdle` | Ngăn chặn hành vi Duplicant đứng chờ lảng vảng (Idle) trước cửa ra vào. | Tối ưu / AI |
| 15 | `NoMeteors` | Loại bỏ hoàn toàn mưa thiên thạch trên bề mặt hành tinh. | Thử thách / Dễ |
| 16 | `NoNotificationSounds` | Tắt âm thanh phát ra khi có thông báo hệ thống mới hiện lên. | Âm thanh |
| 17 | `NoPointlessScrollbars` | Ẩn các thanh cuộn thừa trên giao diện khi nội dung không dài. | Giao diện |
| 18 | `NoResearchAlerts` | Tắt chuông báo động mỗi khi hoàn thành một nhánh nghiên cứu mới. | QoL |
| 19 | `NoStutter` | Cố gắng triệt tiêu các đợt giật lag nhỏ (Stutter) xảy ra theo chu kỳ trong game. | Tối ưu |
| 20 | `RebalancedTiles` | Điều chỉnh và cân bằng lại thông số của một số loại gạch sàn. | Hệ thống |
| 21 | `SuppressNotifications` | Ẩn hoặc chặn triệt để các thông báo rác không mong muốn. | Giao diện |

---

### 2.10. Nhóm Mod của Ony (`ony_mods`)
*Đường dẫn cục bộ:* [ony_mods/](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/ony_mods)

*Lưu ý: Kho mã nguồn này trên Github hiện tại chỉ đóng vai trò là Bug Tracker và chứa hình ảnh giới thiệu, không chứa mã nguồn C# thực tế.*

---

## 3. Hướng Dẫn Kỹ Thuật Cho Nhà Phát Triển (Mono-Workspace)

### 3.1. Cơ chế Compile tự động
Tất cả các thư mục nguồn của các tác giả lớn (`cairath_source`, `peterhan_source`, `sanchozz_source`, `sgt_imalas_source`) đều được cấu hình bằng file `Directory.Build.props` ở thư mục gốc của họ. 

Để biên dịch thành công và xuất trực tiếp file `.dll` vào thư mục game ONI:
1. Bạn cần sao chép file `Directory.Build.props.default` thành `Directory.Build.props.user` (nếu chưa có).
2. Chỉnh sửa đường dẫn game trong `Directory.Build.props.user` khớp với đường dẫn cài đặt game Oxygen Not Included thực tế trên máy tính của bạn:
   ```xml
   <PropertyGroup>
     <SteamFolder>C:\Program Files (x86)\Steam</SteamFolder>
     <InstallFolder>$(SteamFolder)\steamapps\common\OxygenNotIncluded</InstallFolder>
   </PropertyGroup>
   ```
3. Chạy lệnh build thông qua MSBuild hoặc Visual Studio bằng cách mở file giải pháp `.sln` tương ứng.

### 3.2. Cấu trúc tổ chức Project C# điển hình
Mỗi thư mục mod con thường chứa cấu trúc tối thiểu sau:
- `/Properties/AssemblyInfo.cs`: Định nghĩa phiên bản mod, thông tin tác giả.
- `*.cs` (C# source files): Các class chứa code logic, Harmony patches.
- `mod.yaml` hoặc `mod_info.yaml`: File metadata mô tả phiên bản API game tương thích (ví dụ: `APIVersion: 2`, `MinVersion: 643023`).
- `/translations/`: Thư mục chứa các file dịch ngôn ngữ (nếu có, dạng `.po`).

---
*Tài liệu được lập tự động bởi trợ lý Antigravity AI.*
