using HarmonyLib;
using KSerialization;
using UnityEngine;
using PeterHan.PLib.Options;
using Newtonsoft.Json;

namespace AutoDropBottlers
{
    // Khởi tạo mod, đăng ký cài đặt Mod Options bằng PLib và dịch thuật ngôn ngữ UI
    public class AutoDropBottlersMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            
            // Khởi tạo thư viện PLib và đăng ký Options
            PeterHan.PLib.Core.PUtil.InitLibrary();
            new POptions().RegisterOptions(this, typeof(AutoDropOptions));
            AutoDropOptions.Load();

            // Bản địa hóa tĩnh Tiếng Việt thông minh cho tiêu đề và giải thích Side Screen
            string title = "Auto Drop Bottles";
            string tooltip = "If enabled, bottles will automatically drop when the bottler reaches maximum capacity or is emptied manually.";
            try {
                var locale = Localization.GetLocale();
                if (locale != null && locale.Code != null && (locale.Code.StartsWith("vi") || locale.Code.IndexOf("vietnamese", System.StringComparison.OrdinalIgnoreCase) >= 0)) {
                    title = "Tự động thả chai";
                    tooltip = "Nếu bật, chai nước/khí sẽ tự động rơi ra đất ngay khi bơm xong hoặc khi máy đầy.";
                }
            } catch {}

            Strings.Add("STRINGS.UI.UISIDESCREENS.AUTODROP.TITLE", title);
            Strings.Add("STRINGS.UI.UISIDESCREENS.AUTODROP.TOOLTIP", tooltip);
            
            Debug.Log("[AutoDropBottlers] Loaded, PLib Options registered, and strings localized.");
        }
    }

    // Lớp cấu hình tùy chọn mặc định của mod lưu trữ trong config.json
    [ConfigFile("config.json", true, true)]
    public class AutoDropOptions
    {
        [Option("Auto Drop Enabled by Default", "If enabled, newly constructed bottlers will have 'Auto Drop' checked by default.")]
        [JsonProperty]
        public bool AutoDropEnabledByDefault { get; set; } = false;

        public static AutoDropOptions Instance { get; private set; }

        public static void Load()
        {
            Instance = POptions.ReadSettings<AutoDropOptions>() ?? new AutoDropOptions();
        }
    }

    // Component điều khiển Checkbox trên giao diện UI
    [SerializationConfig(KSerialization.MemberSerialization.OptIn)]
    public class AutoDropControl : KMonoBehaviour, ICheckboxControl
    {
        private const float DropDelaySeconds = 0.5f;

        [Serialize]
        public bool autoDropEnabled = false; // Trạng thái bật/tắt tự động thả chai

        public string CheckboxTitleKey => "STRINGS.UI.UISIDESCREENS.AUTODROP.TITLE";
        public string CheckboxToolTipKey => "STRINGS.UI.UISIDESCREENS.AUTODROP.TOOLTIP";

        // Bản địa hóa động Nhãn hiển thị của nút bấm Checkbox
        public string CheckboxLabel {
            get {
                try {
                    var locale = Localization.GetLocale();
                    if (locale != null && locale.Code != null && (locale.Code.StartsWith("vi") || locale.Code.IndexOf("vietnamese", System.StringComparison.OrdinalIgnoreCase) >= 0)) {
                        return "Tự động thả chai";
                    }
                } catch {}
                return "Auto Drop Bottles";
            }
        }

        // Bản địa hóa động Mô tả giải thích Checkbox
        public string CheckboxTooltip {
            get {
                try {
                    var locale = Localization.GetLocale();
                    if (locale != null && locale.Code != null && (locale.Code.StartsWith("vi") || locale.Code.IndexOf("vietnamese", System.StringComparison.OrdinalIgnoreCase) >= 0)) {
                        return "Nếu bật, chai nước/khí sẽ tự động rơi ra đất ngay khi bơm xong hoặc khi máy đầy.";
                    }
                } catch {}
                return "If enabled, bottles will automatically drop when the bottler reaches maximum capacity or is emptied manually.";
            }
        }

        private Bottler bottler;
        private bool dropQueued;

        public bool GetCheckboxValue()
        {
            return autoDropEnabled;
        }

        public void SetCheckboxValue(bool value)
        {
            autoDropEnabled = value;
            if (value)
            {
                // Nếu bật, kiểm tra trễ để không đụng vào proxy bottle trong lúc Bottler vừa kết thúc việc.
                QueueDrop();
            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            bottler = GetComponent<Bottler>();

            // Gán giá trị mặc định từ cấu hình Mod Options
            if (AutoDropOptions.Instance != null)
            {
                autoDropEnabled = AutoDropOptions.Instance.AutoDropEnabledByDefault;
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            // Đăng ký sự kiện thay đổi dung lượng kho chứa
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
            // Đăng ký sự kiện sao chép cài đặt (Copy Settings)
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        // Thực hiện sao chép cài đặt khi người chơi nhấn nút "Copy Settings"
        private void OnCopySettings(object data)
        {
            if (data != null && data is GameObject go)
            {
                var sourceControl = go.GetComponent<AutoDropControl>();
                if (sourceControl != null)
                {
                    SetCheckboxValue(sourceControl.autoDropEnabled);
                }
            }
        }

        // Tách biệt logic thả toàn bộ chai trong kho chứa ra đất
        public void TriggerDrop()
        {
            if (!CanDrop())
                return;

            bottler.storage.DropAll(false, false, default, true);
        }

        public void QueueDrop()
        {
            if (dropQueued || !autoDropEnabled || bottler == null || bottler.storage == null || bottler.storage.IsEmpty())
                return;

            dropQueued = true;
            GameScheduler.Instance.Schedule("AutoDropBottlers.DropWhenIdle", DropDelaySeconds, obj => {
                dropQueued = false;

                if (!autoDropEnabled || bottler == null || bottler.storage == null || bottler.storage.IsEmpty())
                    return;

                // Bottler creates a temporary proxy bottle while a Duplicant works. Dropping storage
                // before vanilla cleanup finishes leaves that proxy in a broken state.
                if (bottler.worker != null)
                {
                    QueueDrop();
                    return;
                }

                TriggerDrop();
            });
        }

        private bool CanDrop()
        {
            return autoDropEnabled &&
                bottler != null &&
                bottler.storage != null &&
                !bottler.storage.IsEmpty() &&
                bottler.worker == null;
        }

        private void OnStorageChange(object data)
        {
            if (!autoDropEnabled || bottler == null || bottler.storage == null) return;

            // Nếu kho chứa đầy, trì hoãn 1 frame để đảm bảo đồng bộ sự kiện của game và thả chai
            if (bottler.storage.IsFull())
            {
                QueueDrop();
            }
        }
    }

    // Tự động thêm component điều khiển vào tất cả các máy bơm chai (chất lỏng và chất khí)
    [HarmonyPatch(typeof(Bottler), "OnPrefabInit")]
    public class Bottler_OnPrefabInit_Patch
    {
        public static void Postfix(Bottler __instance)
        {
            if (__instance != null)
            {
                __instance.gameObject.AddOrGet<AutoDropControl>();
            }
        }
    }

    // Harmony Patch: Thả chai nước/khí ngay lập tức khi Duplicant dừng làm việc (bơm xong hoặc dừng dở dang)
    [HarmonyPatch(typeof(Bottler), "OnStopWork")]
    public class Bottler_OnStopWork_Patch
    {
        public static void Postfix(Bottler __instance)
        {
            if (__instance != null)
            {
                var control = __instance.GetComponent<AutoDropControl>();
                if (control != null && control.autoDropEnabled)
                {
                    control.QueueDrop();
                }
            }
        }
    }
}
