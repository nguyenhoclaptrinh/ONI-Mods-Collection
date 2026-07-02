# Kế hoạch sửa lỗi Crash: Assert failed: FindNextChore found an entry that wasn't a FetchChore

## 1. Bối cảnh & Nguyên nhân
* **Hiện tượng**: Khi cánh tay robot gắp đồ (`SolidTransferArm`) tìm kiếm công việc mới (`ChoreConsumer.FindNextChore`), game duyệt qua danh sách các chore thuộc layer `fetchChoreLayer` bằng delegate `FindNextChoreEvaluateEntryHelper`. Do sự can thiệp của các mod lọc Chore (như `Priority Zero` hoặc `AI Improvements`), một thực thể không phải `FetchChore` (hoặc có thể là `null`/đối tượng đã bị giải phóng) đã lọt vào layer, kích hoạt kiểm tra lỗi `DebugUtil.Assert(test: false, "FindNextChore found an entry that wasn't a FetchChore")` của game gốc làm crash game lập tức.
* **Mục tiêu**: Thay thế delegate lỗi `FindNextChoreEvaluateEntryHelper` của lớp `ChoreConsumer` bằng một delegate an toàn hơn (`Safe Delegate`) do mod `AI Improvements` gán đè lúc khởi động mod. Delegate này sẽ nhẹ nhàng bỏ qua đối tượng lỗi (bằng cách trả về `Util.IterationInstruction.Continue`) thay vì gọi Assert gây crash game.

---

## 2. Các thay đổi đề xuất

### Component: AI Improvements

#### [MODIFY] [AIImprovementsPatches.cs](file:///d:/Documents/Klei/OxygenNotIncluded/mods/Local/_source/peterhan_source/AIImprovements/AIImprovementsPatches.cs)
* Sửa đổi phương thức `OnLoad(Harmony harmony)` của lớp `AIImprovementsPatches` để dùng Reflection tìm và gán delegate an toàn của chúng ta cho trường static private `ChoreConsumer.FindNextChoreEvaluateEntryHelper`.
* Tạo phương thức delegate `SafeEvaluateEntry` tương tự như game gốc nhưng loại bỏ lệnh `DebugUtil.Assert` gây crash.

---

## 3. Mã nguồn chi tiết sửa đổi

### Thêm delegate an toàn `SafeEvaluateEntry` và gán trong `OnLoad`:
```csharp
// Trong class AIImprovementsPatches:

public override void OnLoad(Harmony harmony) {
    base.OnLoad(harmony);
    PUtil.InitLibrary();
    
    // ... code phát hiện StopNavigator ...

    // Vá lỗi Assert FindNextChore
    try {
        var field = typeof(ChoreConsumer).GetField("FindNextChoreEvaluateEntryHelper",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        if (field != null) {
            field.SetValue(null, new System.Func<object, ChoreConsumer, Util.IterationInstruction>(SafeEvaluateEntry));
            PUtil.LogDebug("AI Improvements: Successfully patched ChoreConsumer.FindNextChoreEvaluateEntryHelper with a safe delegate");
        } else {
            PUtil.LogWarning("AI Improvements: Could not find ChoreConsumer.FindNextChoreEvaluateEntryHelper field!");
        }
    } catch (System.Exception ex) {
        PUtil.LogError("AI Improvements: Failed to patch FindNextChoreEvaluateEntryHelper: " + ex.Message);
    }

    Options = new AIImprovementsOptionsInstance();
    // ...
}

private static Util.IterationInstruction SafeEvaluateEntry(object obj, ChoreConsumer consumer) {
    if (!(obj is FetchChore fetchChore)) {
        // Thay vì gọi DebugUtil.Assert gây crash game, ta chỉ bỏ qua và tiếp tục duyệt
        return Util.IterationInstruction.Continue;
    }
    if (fetchChore.target == null) {
        return Util.IterationInstruction.Continue;
    }
    if (fetchChore.isNull) {
        return Util.IterationInstruction.Continue;
    }
    int cell = Grid.PosToCell(fetchChore.gameObject);
    if (consumer.consumerState.solidTransferArm.IsCellReachable(cell)) {
        fetchChore.CollectChoresFromGlobalChoreProvider(consumer.consumerState,
            consumer.preconditionSnapshot.succeededContexts,
            consumer.preconditionSnapshot.failedContexts, false);
    }
    return Util.IterationInstruction.Continue;
}
```

---

## 4. Kế hoạch xác minh (Verification Plan)
1. **Biên dịch**: Chạy `dotnet build` trên dự án mod `AIImprovements`.
2. **Triển khai**: Copy file `.dll` đã build sang thư mục game Mod local: `d:\Documents\Klei\OxygenNotIncluded\mods\Local\AIImprovements\AIImprovements.dll`.
3. **Chạy thử**: Xác nhận game khởi động thành công và nạp mod đúng cách.
