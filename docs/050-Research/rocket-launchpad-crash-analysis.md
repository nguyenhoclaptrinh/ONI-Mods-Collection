---
id: rocket-launchpad-crash-analysis
type: research-output
source: Player.log, Assembly-CSharp decompile (LaunchPad, SelectModuleSideScreen)
status: done
created: 2026-06-24
---

# Phân tích nguyên nhân Crash bệ phóng Rocket (LaunchPad)

Tài liệu phân tích chi tiết về lỗi crash `NullReferenceException` xảy ra tại bệ phóng Rocket (`LaunchPad`) khi người chơi thực hiện xây dựng hoặc thay đổi các mô-đun Rocket.

---

## 1. StackTrace Lỗi từ Player.log
Lỗi được ghi nhận trong file log như sau:
```
NullReferenceException: Object reference not set to an instance of an object
  at LaunchPad.AddBaseModule (BuildingDef moduleDefID, System.Collections.Generic.IList`1[T] elements) [0x00088] in <4bcb387f967442adaa0564cdaae39f3f>:0 
  at SelectModuleSideScreen.OrderBuildSelectedModule () [0x0008a] in <4bcb387f967442adaa0564cdaae39f3f>:0 
  at SelectModuleSideScreen.OnClickBuildSelectedModule () [0x0000e] in <4bcb387f967442adaa0564cdaae39f3f>:0 
```

---

## 2. Phân tích Luồng Code Game Gốc (Decompiled)

### A. Tại SelectModuleSideScreen.OrderBuildSelectedModule()
Khi người chơi chọn một mô-đun rocket và nhấn nút "Build" (xây dựng), game gọi:
```csharp
gameObject = launchPad.AddBaseModule(selectedModuleDef, materialSelectionPanel.GetSelectedElementAsList);
```
Phương thức này chuyển định nghĩa mô-đun (`selectedModuleDef`) và danh sách vật liệu đã chọn sang bệ phóng để tiến hành đặt mô-đun.

### B. Tại LaunchPad.AddBaseModule()
Tại đây, game thực hiện đặt mô-đun vào ô lưới (`cell`) tương ứng:
```csharp
public GameObject AddBaseModule(BuildingDef moduleDefID, IList<Tag> elements)
{
    int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), baseModulePosition);
    GameObject gameObject = null;
    
    // Thực hiện đặt thử hoặc xây dựng mô-đun trực tiếp
    gameObject = ((!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive) 
        ? moduleDefID.TryPlace(null, Grid.CellToPosCBC(cell, moduleDefID.SceneLayer), Orientation.Neutral, elements) 
        : moduleDefID.Build(cell, Orientation.Neutral, null, elements, 293.15f, playsound: true, GameClock.Instance.GetTime()));
    
    GameObject obj = Util.KInstantiate(Assets.GetPrefab("Clustercraft"));
    obj.SetActive(value: true);
    Clustercraft component = obj.GetComponent<Clustercraft>();
    
    // DÒNG BỊ CRASH (0x00088)
    component.GetComponent<CraftModuleInterface>().AddModule(gameObject.GetComponent<RocketModuleCluster>());
    ...
}
```

### C. Cơ chế xảy ra NullReferenceException
1. Hàm `moduleDefID.TryPlace(...)` hoặc `moduleDefID.Build(...)` trả về kết quả là **`null`** do vị trí đặt mô-đun không hợp lệ (ví dụ: bị cản trở vật lý bởi các công trình xung quanh, vượt quá giới hạn chiều cao tối đa của tháp rocket, hoặc thiếu điều kiện kết nối).
2. Khi `gameObject` bị `null`, game vẫn tiếp tục thực hiện lệnh:
   `gameObject.GetComponent<RocketModuleCluster>()` (tương đương với `null.GetComponent<...>`).
3. Điều này ngay lập tức kích hoạt lỗi **`NullReferenceException`** và làm crash game về màn hình desktop.

---

## 3. Tại sao người chơi bấm được nút Build khi vị trí không hợp lệ?

Thông thường, game gốc sẽ vô hiệu hóa nút Build (`buildSelectedModuleButton.isInteractable = false`) nếu các điều kiện xây dựng không được thỏa mãn. Tuy nhiên, nút Build vẫn bấm được do:
1. **Chế độ Sandbox / Instant Build Mode (Debug)**: Game gốc tự động bỏ qua các điều kiện kiểm tra va chạm vật lý và chiều cao khi Sandbox hoặc Debug Mode hoạt động để cho phép người chơi xây tự do. Nhưng khi gọi `Build` hoặc `TryPlace` thực tế bên trong engine Sim, lệnh vẫn bị từ chối và trả về `null`.
2. **Xung đột từ các mod thay đổi Rocket**:
   Đặc biệt là các mod lớn can thiệp sâu vào bệ phóng và mô-đun như:
   - **`Rockets-TinyYetBig` (RTB)**: Mod này thay đổi chiều cao tháp, thêm các mô-đun đặc biệt và sửa đổi logic kết nối mô-đun.
   - **`Robo Rockets`**: Thay đổi cách điều khiển và gắn kết các mô-đun tự động.
   - **`Blueprints Expanded`**: Cho phép dán các bản vẽ rocket đè lên bệ phóng đã có mô-đun.

Khi các mod này ghi đè điều kiện kiểm tra (`TestBuildable`) để hiển thị nút Build hợp lệ, nhưng khi gọi đến hàm thực thi gốc của game thì game gốc lại trả về `null`, dẫn đến crash.

---

## 4. Giải pháp khắc phục đề xuất cho người chơi

1. **Kiểm tra và gỡ bỏ/cập nhật các mod liên quan đến Rocket**:
   Ưu tiên kiểm tra các mod:
   - **`Rockets-TinyYetBig`** (Sgt_Imalas)
   - **`Robo Rockets`**
   - **`Blueprints Expanded`** (nếu dán bản vẽ đè lên bệ phóng)
2. **Kiểm tra không gian xây dựng bệ phóng**:
   Tránh xây dựng mô-đun khi tháp rocket quá cao hoặc xung quanh bệ phóng có các công trình khác cản trở đường đi của mô-đun.
3. **Tránh ép buộc xây dựng bằng Sandbox / Debug**:
   Hạn chế click xây dựng mô-đun quá nhanh hoặc ép xây ở những vị trí mà game bình thường hiển thị màu đỏ (không cho phép).
