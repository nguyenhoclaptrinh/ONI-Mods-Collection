# Hướng Dẫn Tự Tùy Biến (Custom) Bản Đồ Mod AllDLCWorld

Tài liệu này sẽ giải thích chi tiết, dễ hiểu nhất về cấu trúc và các thông số trong hai tệp cấu hình quan trọng của mod AllDLCWorld. Dựa vào hướng dẫn này, bạn có thể tự tay chỉnh sửa kích thước thế giới, tăng giảm số lượng biome, thêm bớt mạch địa chất hoặc núi lửa theo sở thích của mình.

---

## Phần 1: Tùy Biến Thế Giới (Worlds)
Đường dẫn file cấu hình thế giới: `worldgen/worlds/AllDLCWorld.yaml`

Tệp này quy định kích thước hành tinh, các loại biome (phân khu) sẽ xuất hiện, tỷ lệ phân bổ của chúng, và các loại mạch địa chất sinh ra trên đất liền.

### 1. Kích thước bản đồ (`worldsize`)
Quyết định chiều rộng (X) và chiều cao (Y) của hành tinh khởi đầu.
```yaml
worldsize:
  X: 512  # Chiều rộng (Số ô đất)
  Y: 760  # Chiều cao (Số ô đất)
```
* **Mẹo Custom:**
  * Bản đồ mặc định của game (Classic Start) thường là `256 x 380`.
  * Nếu máy bạn yếu hoặc card màn hình onboard (như Intel UHD Graphics), nên giảm xuống `384 x 570` hoặc `256 x 380` để chơi mượt hơn, tránh giật lag về sau.
  * Nếu máy bạn cực mạnh và muốn thử thách siêu lớn, bạn có thể tăng lên nhưng tránh vượt quá `512 x 760` vì game có thể bị tràn bộ nhớ (Out of Memory) hoặc crash khi tải màn hình.

---

### 2. Danh sách Biome (`subworldFiles`)
Khai báo toàn bộ các loại biome sẽ có mặt trên hành tinh và quy định ràng buộc tối thiểu của chúng.
```yaml
subworldFiles:
  - name: subworlds/jungle/Jungle  # Đường dẫn đến biome Rừng Rậm (Jungle) của game
    weight: 1.0                    # Trọng số xuất hiện (Càng cao càng dễ sinh ra nhiều)
    minCount: 1                    # Số lượng phân khu này bắt buộc phải có tối thiểu
```
* **Cách hoạt động của `weight` và `minCount`:**
  * **`minCount`**: Chỉ nên đặt là `1` (hoặc `0` nếu không quan trọng). Nếu bạn đặt `minCount: 5`, game bắt buộc phải nhét đủ 5 biome đó vào bản đồ, nếu thiếu dù chỉ 1 ô trống thích hợp, game sẽ báo lỗi đỏ (crash worldgen) ngay lập tức.
  * **`weight`**: Nếu muốn thế giới có **nhiều dầu thô** hơn, bạn hãy tăng `weight` của `subworlds/oil/OilPockets` từ `1.5` lên `3.0`. Nếu muốn **ít đầm lầy** lại, giảm `weight` của `subworlds/marsh/HotMarsh` xuống `0.3`.

* **Ví dụ cụ thể:** Bạn muốn biến thế giới thành một hành tinh băng giá phủ đầy tuyết của DLC2 Ceres:
  Hãy tăng trọng số của các biome băng tuyết lên cao:
  ```yaml
    - name: dlc2::subworlds/icecaves/IceCavesBasic
      weight: 4.0  # Tăng vọt trọng số để chiếm phần lớn bản đồ
      minCount: 1
  ```

---

### 3. Quy tắc phân tầng khoảng cách (`unknownCellsAllowedSubworlds`)
Game ONI chia hành tinh ra làm các tầng độ sâu dựa trên khoảng cách tính từ tâm điểm xuất phát (AtStart), tầng đáy (AtDepths) và tầng bề mặt vũ trụ (AtSurface).
```yaml
  # Ring-1: Các Biome nằm sát khu vực xuất phát (Khoảng cách = 2)
  - tagcommand: DistanceFromTag
    tag: AtStart
    minDistance: 2
    maxDistance: 2
    command: Replace
    subworldNames:
      - subworlds/forest/Forest
      - subworlds/jungle/Jungle
```
* **Mẹo Custom:**
  * Nếu bạn muốn biome Phóng xạ (`Radioactive`) xuất hiện ngay gần cổng dịch chuyển (để lấy Uranium sớm), hãy chuyển tên của nó từ `Ring-3` (minDistance: 4) lên nhóm `Ring-1` (minDistance: 2).

---

### 4. Custom Mạch địa chất & Núi lửa (`worldTemplateRules`)
Quy định số lượng và chủng loại núi lửa/mạch tài nguyên được tạo ra.
```yaml
  # Nhóm Núi lửa ở tầng magma đáy bản đồ
  - names:
      - geysers/volcanohole   # Tên núi lửa trong mã nguồn game
    listRule: GuaranteeOne    # Quy tắc đảm bảo sinh ra
    times: 4                  # Số lượng núi lửa sinh ra
    allowDuplicates: true     # Cho phép trùng lặp loại núi lửa
```
* **Mẹo Custom:**
  * Bạn muốn hành tinh có thật nhiều **Núi lửa Vàng** để làm giàu? Hãy tìm đến mục `Rare Metal Geysers` và tách riêng núi lửa vàng ra cấu hình riêng:
    ```yaml
      - names:
          - geysers/molten_gold
        listRule: GuaranteeOne
        times: 10 # Ép game sinh ra đúng 10 núi lửa vàng trên bản đồ!
        allowDuplicates: true
        priority: 100
        allowedCellsFilter:
          - command: Replace
            tagcommand: DistanceFromTag
            tag: AtStart
            minDistance: 2
            maxDistance: 99
    ```

---

## Phần 2: Tùy Biến Hệ Mặt Trời & Hành Tinh Phụ (Clusters)
Đường dẫn file cấu hình cụm sao: `worldgen/clusters/AllDLCWorld.yaml`

Tệp này quản lý bản đồ sao (Starmap), khoảng cách các hành tinh phụ khác xung quanh hành tinh chủ Pangaea.

### 1. Khoảng cách vũ trụ (`numRings`)
Quy định bán kính hệ mặt trời của bạn (số vòng quỹ đạo).
```yaml
numRings: 9  # Vũ trụ rộng 9 vòng quỹ đạo tròn
```
* Nếu bạn muốn hệ mặt trời rộng lớn hơn, nhiều chỗ để chứa các hành tinh phụ xa xôi hơn, bạn có thể tăng lên `12` hoặc `15`.

---

### 2. Định vị hành tinh phụ (`worldPlacements`)
Nơi đặt các hành tinh phụ trong bản đồ sao của cụm Pangaea.
```yaml
worldPlacements:
  - world: worlds/AllDLCWorld
    locationType: StartWorld # Hành tinh khởi đầu
    allowedRings:
      min: 0
      max: 0 # Luôn nằm ở tâm hệ mặt trời (Vòng 0)

  # Bạn có thể thêm các hành tinh phụ của game vào đây:
  - world: expansion1::worlds/TundraMoonlet # Hành tinh băng moonlet
    allowedRings:
      min: 3 # Xuất hiện ngẫu nhiên ở vòng 3
      max: 5 # Đến vòng 5 của hệ mặt trời
```
* **Mẹo Custom:** 
  * Nếu bạn muốn có một hành tinh phụ cụ thể của game Spaced Out xuất hiện gần bạn để dễ bay tên lửa sang, hãy đổi `allowedRings` của hành tinh đó về dải hẹp hơn như `min: 1, max: 2`.

---

### 3. Tên kỹ thuật của các Mạch / Núi lửa thông dụng để bạn copy-paste:
Khi sửa đổi mục `worldTemplateRules` trong tệp thế giới, bạn hãy sử dụng các tên sau đây để thay thế:

| Tên tiếng Việt | Mã cấu hình trong game |
| :--- | :--- |
| **Núi lửa Vàng** | `geysers/molten_gold` |
| **Núi lửa Sắt** | `geysers/molten_iron` |
| **Núi lửa Đồng** | `geysers/molten_copper` |
| **Núi lửa Niobium** | `expansion1::geysers/niobium` |
| **Mạch Khí Tự Nhiên** | `geysers/methane` |
| **Mạch Hơi Nước Nóng** | `geysers/hot_steam` |
| **Mạch Nước Bùn Lạnh** | `geysers/slush_water` |
| **Mạch Nước Muối Bùn Lạnh** | `expansion1::geysers/slush_salt_water` |
| **Núi lửa Magma thường** | `geysers/volcano` |
| **Núi lửa Magma lớn** | `geysers/volcanohole` |
| **Mạch Dầu Thô** | `poi/oil/small_oilpockets_geyser_a` |

---

## Quy trình áp dụng thay đổi sau khi tự sửa file YAML:
Sau khi bạn mở và chỉnh sửa các file cấu hình YAML theo ý muốn bằng Notepad hoặc VS Code, hãy làm theo các bước sau để game nhận cấu hình mới:
1. Lưu (Save) file lại.
2. Mở game **Oxygen Not Included** lên.
3. Bấm **New Game** $\rightarrow$ chọn **Pangaea Cluster** để game tạo lại thế giới mới theo các thông số bạn vừa chỉnh sửa!
