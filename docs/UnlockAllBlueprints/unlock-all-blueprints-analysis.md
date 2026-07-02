---
id: unlock-all-blueprints-analysis
type: research-output
status: done
created: 2026-06-22
source: source-code-analysis
---

# Phân tích mã nguồn Mod Unlock All Blueprints

## 1. Mục đích
Tài liệu này phân tích cơ chế hoạt động của mod **Unlock All Blueprints**, giải thích cách game Oxygen Not Included quản lý độ hiếm (rarity) của các bản thiết kế (blueprints/permits) và tại sao việc ép giá trị độ hiếm về `PermitRarity.Universal` lại giúp mở khóa toàn bộ skin và vật phẩm trang trí mà không cần kết nối server Klei hoặc sở hữu trong Steam Inventory.

---

## 2. Cơ chế cốt lõi của game (Klei Inventory System)
Trong game Oxygen Not Included, Klei cung cấp hệ thống skin/trang trí được gọi a **Permit** (Giấy phép). Các Permit này bao gồm:
- **Building Facade**: Diện mạo mới cho các công trình (ví dụ: giường ngủ, tủ đồ, bồn nước, v.v.).
- **Artable**: Các tác phẩm hội họa, điêu khắc do Duplicant tạo ra.
- **Clothing**: Quần áo, mũ nón cho Duplicant.
- **Balloon Artist Facade**: Kiểu dáng bóng bay được tạo ra bởi Duplicant có kỹ năng Balloon Artist.

Mỗi Permit trong game được đại diện bởi một lớp dữ liệu chứa thông tin metadata, trong đó có một thuộc tính là **Rarity** (Độ hiếm).
Các loại độ hiếm phổ biến bao gồm:
- `Universal` (Phổ thông/Toàn cầu)
- `Common` (Thường)
- `Uncommon` (Không thông dụng)
- `Rare` (Hiếm)
- `Elegant` (Thanh lịch)

Trong mã nguồn game (class `Db` hoặc `Database`), khi game load danh sách Permit, nó sẽ kiểm tra quyền sở hữu của người dùng đối với các Permit đó:
1. Nếu Permit có độ hiếm khác `Universal`, game sẽ truy vấn tài khoản Klei (qua Steam Inventory hoặc Klei Server) để kiểm tra xem tài khoản của người chơi có sở hữu Permit này hay không. Nếu không sở hữu, Permit sẽ bị khóa (locked).
2. Nếu Permit có độ hiếm là `PermitRarity.Universal`, game mặc định coi đây là các skin cơ bản mà **bất kỳ người chơi nào cũng sở hữu**. Do đó, game sẽ mở khóa tự động và không cần kiểm tra quyền sở hữu từ server.

---

## 3. Phân tích mã nguồn Harmony Patches
Bản mod can thiệp vào quá trình lấy thông tin độ hiếm của 4 lớp dữ liệu Permit chính thông qua thư viện Harmony.

### 3.1. Patch 1: Lớp `BuildingFacadeInfo`
```csharp
[HarmonyPatch(typeof(BuildingFacadeInfo), "get_rarity")]
public static class BuildingFacadeInfo_Rarity_Patch
{
    public static bool Prefix(ref PermitRarity __result)
    {
        __result = PermitRarity.Universal;
        return false; // Bỏ qua code gốc của game
    }
}
```
- **Mục tiêu**: Override getter `get_rarity` (hoặc thuộc tính `rarity`) của class `BuildingFacadeInfo`.
- **Tác dụng**: Trả về `PermitRarity.Universal` cho tất cả các diện mạo công trình. Giúp mở khóa toàn bộ skin công trình trong game.

### 3.2. Patch 2: Lớp `ArtableInfo`
```csharp
[HarmonyPatch(typeof(ArtableInfo), "get_rarity")]
public static class ArtableInfo_Rarity_Patch
{
    public static bool Prefix(ref PermitRarity __result)
    {
        __result = PermitRarity.Universal;
        return false; // Bỏ qua code gốc của game
    }
}
```
- **Mục tiêu**: Override getter `get_rarity` của class `ArtableInfo`.
- **Tác dụng**: Trả về `PermitRarity.Universal` cho tất cả các tác phẩm nghệ thuật. Giúp các tác phẩm nghệ thuật đẹp mắt luôn sẵn sàng khi Duplicant sáng tác.

### 3.3. Patch 3: Lớp `ClothingItemInfo`
```csharp
[HarmonyPatch(typeof(ClothingItemInfo), "get_rarity")]
public static class ClothingItemInfo_Rarity_Patch
{
    public static bool Prefix(ref PermitRarity __result)
    {
        __result = PermitRarity.Universal;
        return false; // Bỏ qua code gốc của game
    }
}
```
- **Mục tiêu**: Override getter `get_rarity` của class `ClothingItemInfo`.
- **Tác dụng**: Trả về `PermitRarity.Universal` cho tất cả các loại trang phục. Giúp tủ quần áo (wardrobe) của Duplicant có đầy đủ mọi trang phục.

### 3.4. Patch 4: Lớp `BalloonArtistFacadeInfo`
```csharp
[HarmonyPatch(typeof(BalloonArtistFacadeInfo), "get_rarity")]
public static class BalloonArtistFacadeInfo_Rarity_Patch
{
    public static bool Prefix(ref PermitRarity __result)
    {
        __result = PermitRarity.Universal;
        return false; // Bỏ qua code gốc của game
    }
}
```
- **Mục tiêu**: Override getter `get_rarity` của class `BalloonArtistFacadeInfo`.
- **Tác dụng**: Trả về `PermitRarity.Universal` cho các kiểu bóng bay. Mở khóa toàn bộ các skin bong bóng nghệ thuật trong game.

---

## 4. Kết luận & Tác động của Mod
Bằng cách sử dụng Harmony để **tiền xử lý (Prefix)** và **ghi đè giá trị trả về (`__result`)** thành `PermitRarity.Universal`, mod đã lừa hệ thống inventory nội bộ của game rằng tất cả các skin đều là trang phục/thiết kế mặc định (Universal). 

Do đó:
- Game không thực hiện yêu cầu xác thực sở hữu với Steam/Klei server đối với các item này nữa.
- Toàn bộ tính năng offline của skin vẫn hoạt động hoàn hảo.
- Nếu gỡ mod, các công trình đã xây với skin mở khóa bằng mod vẫn giữ nguyên skin đó (được lưu trong file save game), nhưng nếu phá đi xây lại hoặc xây công trình mới thì sẽ không chọn được skin đó nữa (trừ khi tài khoản Klei thực sự sở hữu nó).
