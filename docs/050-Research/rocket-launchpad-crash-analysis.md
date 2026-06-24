---
id: rocket-launchpad-crash-analysis
type: research-output
source: Player.log, Assembly-CSharp decompile (LaunchPad, SelectModuleSideScreen), mods.json
status: done
created: 2026-06-24
---

# Phân tích nguyên nhân Crash bệ phóng Rocket (LaunchPad) - Đã Cập Nhật

Tài liệu phân tích chi tiết về lỗi crash `NullReferenceException` xảy ra tại bệ phóng Rocket (`LaunchPad`) khi người chơi thực hiện xây dựng mô-đun Rocket.

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

## 3. Xác minh dựa trên danh sách Mod kích hoạt của Đại ca

Đối chiếu với danh sách các mod đang thực sự kích hoạt (`enabled: true` hoặc active cho DLC Spaced Out) của Đại ca:
- **Không có các mod can thiệp sâu vào Rocket**: Các mod lớn về rocketry như `Rockets-TinyYetBig` hay `Robo Rockets` hiện tại đang bị tắt.
- **Mod Blueprints Expanded không liên quan**: Thực tế mod Blueprints Expanded không thể lưu hay dán các mô-đun tên lửa (do game quản lý mô-đun theo cơ chế xếp chồng đặc biệt của `CraftModuleInterface` chứ không phải dạng Grid-based Building thông thường). Do đó, mod này không liên quan đến lỗi crash này.

### Kết luận nguyên nhân:
Vì không có mod nào can thiệp vào bệ phóng hay mô-đun được kích hoạt, lỗi crash này **xảy ra trực tiếp trong logic gốc của game**.
Lỗi xuất hiện khi người chơi cố tình hoặc vô tình kích hoạt lệnh xây dựng mô-đun ở vị trí không hợp lệ. Điều này thường xảy ra khi:
1. **Chế độ Sandbox / Instant Build Mode (Debug)** đang bật: Game gốc tự động bỏ qua các kiểm tra cản trở vật lý và chiều cao trên giao diện (`SelectModuleSideScreen.TestBuildable` trả về `true` cho phép click nút Build). Nhưng khi thực hiện xây dựng thực tế, Sim engine vẫn từ chối và trả về `null`, dẫn đến crash.
2. **Lỗi tính toán chiều cao tháp**: Khi tháp rocket đã chạm tới giới hạn chiều cao tối đa (hoặc đỉnh bản đồ), game đôi khi vẫn để nút Build sáng do lỗi cập nhật trạng thái, nhưng khi xây dựng thực tế thì không thể đặt được mô-đun.

---

## 4. Giải pháp khắc phục đề xuất cho Đại ca

1. **Tránh xây dựng mô-đun bằng Sandbox/Debug**: Hạn chế sử dụng chế độ Instant Build (Sandbox/Debug) để đặt mô-đun rocket trực tiếp trên bệ phóng nếu vị trí đó bị cản trở hoặc tháp đã quá cao.
2. **Kiểm tra không gian bệ phóng**: Đảm bảo xung quanh bệ phóng thông thoáng, không có các công trình hoặc gạch tự nhiên cản trở đường đi của mô-đun mới, và tháp tên lửa chưa vượt quá giới hạn chiều cao cho phép.
