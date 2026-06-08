# Pangaea (All-DLC World) Mod

Mod này tích hợp toàn bộ thế giới, quần xã sinh vật (biomes), sinh vật, thực vật và tài nguyên từ tất cả các bản DLC của *Oxygen Not Included* (bao gồm **Spaced Out**, **Frosty Planet Pack**, và **Prehistoric/Raptor**) vào một hành tinh siêu lớn (Pangaea) duy nhất. Bạn sẽ không cần phải du hành sang các hành tinh khác để lấy tài nguyên đặc thù của DLC nữa.

## Tính năng chính
1. **Một thế giới duy nhất (Pangaea)**: Toàn bộ nội dung của game được nén vào hành tinh khởi đầu. Không cần quản lý nhiều tiểu hành tinh (planetoids).
2. **Kích thước siêu lớn (2x Width & 2x Height)**: Bản đồ được mở rộng lên **512 x 760** (gấp 4 lần diện tích của bản đồ Classic tiêu chuẩn) để chứa tất cả các biome một cách mượt mà và tự nhiên.
3. **Phân bố Concentric (Vòng tròn đồng tâm)**:
   - **Vùng trung tâm (Start)**: Sandstone ấm áp và các tài nguyên khởi đầu cơ bản (Forest mini, Water, Oxygen, Metal).
   - **Vòng trong (Close Biomes)**: Forest, Swamp, Marsh, Jungle, Ocean, và các biome DLC mới (Wetlands, Garden, Carrot Quarry).
   - **Vòng giữa (Medium Biomes)**: Frozen, Rust, Wasteland, Sugar Woods, Raptor, Ice Caves.
   - **Vòng ngoài (Exotic Biomes)**: Radioactive, Moo, Niobium, Graphite, Tundra, v.v.
   - **Đáy bản đồ (Depths & Core)**: Các mỏ Dầu (bao gồm Fossil Oil) và Magma Core khổng lồ.
   - **Đỉnh bản đồ (Surface & Space)**: Lớp không gian vũ trụ với các mảnh vỡ thiên thạch đặc trưng của các DLC.
4. **Hệ thống mỏ & Vents phong phú**: Đảm bảo xuất hiện đầy đủ các loại giếng phun,间歇泉 từ kim loại lỏng đến nước, khí ga của tất cả DLC.

## Yêu cầu hệ thống & Game
- Phiên bản Oxygen Not Included: **v706793** (bản crack/non-Steam hoặc bản quyền đều hoạt động).
- Bắt buộc phải kích hoạt đồng thời 3 DLC: **Spaced Out**, **Frosty Planet Pack (DLC2)**, và **DLC4**.

## Hướng dẫn cài đặt (Không cần Steam)
1. Thư mục mod này đã được build và triển khai sẵn tại:
   `D:\Documents\Klei\OxygenNotIncluded\mods\Local\AllDLCWorld`
2. Khởi động trò chơi *Oxygen Not Included*.
3. Vào menu **Mods** trong game.
4. Tìm mod **All-DLC Pangaea World** và nhấn tick để bật nó.
5. Trò chơi sẽ yêu cầu restart để tải mod. Sau khi restart, hãy chọn **New Game** -> chọn chế độ chơi của **Spaced Out** -> chọn Cluster **Pangaea Cluster** trong danh sách thế giới đặc biệt (Special).

## Cấu trúc mã nguồn & Dự án
- `AllDLCWorld.csproj`: File dự án .NET SDK sử dụng ngôn ngữ C#.
- `AllDLCWorld.cs`: Harmony Patch dùng để đăng ký tên và mô tả cho thế giới/cluster trong game database.
- `worldgen/worlds/AllDLCWorld.yaml`: File định nghĩa địa hình thế giới và quy tắc phân bố biome.
- `worldgen/clusters/AllDLCWorld.yaml`: File định nghĩa cluster, quy tắc tạo các điểm POI ngoài không gian và Temporal Tear.
