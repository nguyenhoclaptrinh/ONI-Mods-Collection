using HarmonyLib;
using KMod;

namespace MoveGeyserInstant {
    public sealed class MoveGeyserInstantMod : UserMod2 {
        public override void OnLoad(Harmony harmony) {
            base.OnLoad(harmony);
            Strings.Add("STRINGS.MOVEGEYSERINSTANT.MOVE_BUTTON", "Di chuyển mạch");
            Strings.Add("STRINGS.MOVEGEYSERINSTANT.MOVE_TOOLTIP", "Chọn vị trí mới cho mạch phun. Có thể chuyển sang hành tinh khác rồi click để đặt.");
            Strings.Add("STRINGS.MOVEGEYSERINSTANT.TOOL_NAME", "Di chuyển mạch");
            Strings.Add("STRINGS.MOVEGEYSERINSTANT.TOOL_ACTION", "Click để đặt, chuột phải hoặc Escape để hủy");
            harmony.PatchAll();
            Debug.Log("[MoveGeyserInstant] Loaded.");
        }
    }
}
