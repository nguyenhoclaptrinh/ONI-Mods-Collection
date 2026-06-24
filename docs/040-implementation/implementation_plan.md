# Sửa lỗi MoveGeyserInstant - Kế hoạch thực hiện

Khắc phục lỗi khi Đại ca di chuyển mạch địa nhiệt (Geyser), gạch Neutronium được tạo ra tại vị trí mới sẽ đè lên và làm biến mất hoàn toàn chất lỏng hoặc chất khí đang chiếm chỗ ở các ô đó.

## Giải pháp đề xuất

Sử dụng API native của Oxygen Not Included để tự động dịch chuyển (displace) chất lỏng/khí sang các ô lân cận thay vì ghi đè trực tiếp.

### Thay đổi cụ thể
Trong file `MoveGeyserTool.cs`, tại hàm `PlaceDestinationNeutronium`:
Thay thế:
```csharp
SimMessages.ReplaceElement(cell, SimHashes.Unobtanium, CellEventLogger.Instance.DebugTool, NeutroniumMass, NeutroniumTemperature);
```
bằng:
```csharp
SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Unobtanium, CellEventLogger.Instance.DebugTool, NeutroniumMass, NeutroniumTemperature);
```

### Ưu điểm
- Giải pháp sử dụng API gốc của game (`ReplaceAndDisplaceElement`), giúp game tự động tính toán dồn nén chất lỏng/khí sang các ô trống lân cận và tăng áp suất một cách tự nhiên.
- Code cực kỳ an toàn, chỉ sửa đổi đúng 1 dòng logic spawn Neutronium mới, không ảnh hưởng đến các logic di chuyển mạch khác.

## Thay đổi đề xuất

### [MoveGeyserInstant]

#### [MODIFY] [MoveGeyserTool.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/MoveGeyserInstant/MoveGeyserTool.cs)
- Thay đổi dòng 277: thay `ReplaceElement` bằng `ReplaceAndDisplaceElement`.

## Kế hoạch xác minh

### Biên dịch tự động
Chạy lệnh biên dịch sau tại thư mục chứa mã nguồn:
```powershell
dotnet build d:\Documents\Klei\OxygenNotIncluded\mods\Local\_source\MoveGeyserInstant\MoveGeyserInstant.csproj -c Release
```

### Kiểm tra thủ công (Đại ca thực hiện trong game)
1. Chọn một mạch địa nhiệt bất kỳ.
2. Dùng công cụ Move Geyser để chọn di chuyển mạch tới một khu vực đang chứa nhiều khí (ví dụ Carbon Dioxide) hoặc chất lỏng (ví dụ Nước).
3. Xác nhận di chuyển.
4. **Xác minh**:
   - Gạch Neutronium xuất hiện tại chân mạch mới.
   - Lượng chất lỏng/khí ban đầu ở các ô đó được đẩy dồn sang các ô xung quanh (áp suất tăng lên hoặc dồn sang ô trống) thay vì bị biến mất hoàn toàn.
