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
            // Load config if present
            try {
                Config.Load();
            }
            catch (System.Exception e) {
                Debug.LogWarning("[MoveGeyserInstant] Failed to load config: " + e.Message);
            }
        }
    }

    public static class Config {
        public static bool AllowStacking = true;
        public static bool VerboseNotifications = true;

        private const string ConfigPath = "./mods/Local/MoveGeyserInstant/config.json";

        public static void Load() {
            if (!System.IO.File.Exists(ConfigPath))
                return;
            var text = System.IO.File.ReadAllText(ConfigPath);
            var data = SimpleJson.SimpleJson.DeserializeObject<System.Collections.Generic.Dictionary<string, object>>(text);
            if (data == null)
                return;
            if (data.TryGetValue("allowStacking", out var a))
                AllowStacking = ConvertToBool(a, AllowStacking);
            if (data.TryGetValue("verboseNotifications", out var v))
                VerboseNotifications = ConvertToBool(v, VerboseNotifications);
        }

        private static bool ConvertToBool(object o, bool def) {
            if (o is bool b) return b;
            if (o is string s && bool.TryParse(s, out var r)) return r;
            return def;
        }
    }
}
