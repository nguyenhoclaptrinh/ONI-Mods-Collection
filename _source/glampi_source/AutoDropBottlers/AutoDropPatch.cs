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
            
            // Cho phép Auto-Sweeper gắp chai nước và chai khí bằng cách thêm các tag Liquid và Gas vào bộ lọc conveyable
            try
            {
                var conveyableList = new System.Collections.Generic.List<Tag>(TUNING.STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE);
                foreach (var tag in TUNING.STORAGEFILTERS.LIQUIDS)
                {
                    if (!conveyableList.Contains(tag))
                        conveyableList.Add(tag);
                }
                foreach (var tag in TUNING.STORAGEFILTERS.GASES)
                {
                    if (!conveyableList.Contains(tag))
                        conveyableList.Add(tag);
                }
                TUNING.STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE = conveyableList.ToArray();
                Debug.Log("[AutoDropBottlers] Successfully appended liquid and gas tags to SOLID_TRANSFER_ARM_CONVEYABLE.");
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("[AutoDropBottlers] Failed to append liquid/gas tags: " + ex.Message);
            }

            Debug.Log("[AutoDropBottlers] Loaded, PLib Options registered, and strings localized.");
        }
    }

    // Lớp cấu hình tùy chọn mặc định của mod lưu trữ trong config.json
    [ConfigFile("config.json", true, true)]
    public class AutoDropOptions
    {
        [Option("Auto Drop Enabled by Default", "If enabled, newly constructed bottlers will have 'Auto Drop' checked by default.")]
        [JsonProperty]
        public bool AutoDropEnabledByDefault { get; set; } = true;

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

        // Bản địa hóa động nhãn và mô tả thông qua Strings.Get chính thức
        public string CheckboxLabel => Strings.Get(CheckboxTitleKey);
        public string CheckboxTooltip => Strings.Get(CheckboxToolTipKey);

        private Bottler bottler;
        private bool dropQueued;

        public bool GetCheckboxValue()
        {
            return autoDropEnabled;
        }

        private void UpdateStorageAllowItemRemoval()
        {
            if (bottler != null && bottler.storage != null)
            {
                bottler.storage.allowItemRemoval = !autoDropEnabled;
            }
        }

        public void SetCheckboxValue(bool value)
        {
            autoDropEnabled = value;
            UpdateStorageAllowItemRemoval();
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
            UpdateStorageAllowItemRemoval();
            // Đăng ký sự kiện thay đổi dung lượng kho chứa
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
            // Đăng ký sự kiện sao chép cài đặt (Copy Settings)
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        // Thực hiện sao chép cài đặt khi người chơi nhấn nút "Copy Settings"
        private void OnCopySettings(object data)
        {
            try
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
            catch (System.Exception e)
            {
                Debug.LogWarning("[AutoDropBottlers] Lỗi OnCopySettings: " + e.Message);
            }
        }

        // Tách biệt logic thả toàn bộ chai trong kho chứa ra đất
        public void TriggerDrop()
        {
            try
            {
                if (!CanDrop())
                    return;

                if (bottler != null && bottler.storage != null)
                {
                    bottler.storage.DropAll(false, false, default, true);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[AutoDropBottlers] Lỗi TriggerDrop: " + e.Message);
            }
        }

        public void QueueDrop()
        {
            try
            {
                if (dropQueued || !autoDropEnabled || bottler == null || bottler.storage == null || bottler.storage.IsEmpty())
                    return;

                if (GameScheduler.Instance == null)
                    return;

                dropQueued = true;
                GameScheduler.Instance.Schedule("AutoDropBottlers.DropWhenIdle", DropDelaySeconds, obj => {
                    try
                    {
                        dropQueued = false;

                        if (!autoDropEnabled || bottler == null || bottler.storage == null || bottler.storage.IsEmpty())
                            return;

                        // Bottler creates a temporary proxy bottle while a Duplicant works. Dropping storage
                        // before vanilla cleanup finishes leaves that proxy in a broken state.
                        // Thêm kiểm tra FastTrack: Tuyệt đối không drop nếu đã có Đệ nhận lệnh lấy (Reserved)
                        if (bottler.worker != null || IsItemReserved())
                        {
                            QueueDrop();
                            return;
                        }

                        TriggerDrop();
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogWarning("[AutoDropBottlers] Lỗi trong Scheduled Drop task: " + ex.Message);
                    }
                });
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[AutoDropBottlers] Lỗi QueueDrop: " + e.Message);
            }
        }

        private bool CanDrop()
        {
            return autoDropEnabled &&
                bottler != null &&
                bottler.storage != null &&
                !bottler.storage.IsEmpty() &&
                bottler.worker == null;
        }

        private bool IsAtCapacityThreshold()
        {
            if (bottler == null || bottler.storage == null) return false;
            
            float maxCapacity = bottler.storage.capacityKg;
            if (bottler is IUserControlledCapacity capacityControl)
            {
                maxCapacity = capacityControl.UserMaxCapacity;
            }
            
            return bottler.storage.MassStored() >= maxCapacity;
        }

        private bool IsItemReserved()
        {
            if (bottler == null || bottler.storage == null) return false;
            
            foreach (var item in bottler.storage.items)
            {
                if (item != null)
                {
                    var pickupable = item.GetComponent<Pickupable>();
                    if (pickupable != null && pickupable.ReservedAmount > 0)
                    {
                        return true; 
                    }
                }
            }
            return false;
        }

        private void OnStorageChange(object data)
        {
            try
            {
                if (!autoDropEnabled || bottler == null || bottler.storage == null) return;

                // Nếu lượng chất lỏng/khí đạt ngưỡng Slider, trì hoãn thả chai để đảm bảo đồng bộ
                if (IsAtCapacityThreshold())
                {
                    QueueDrop();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[AutoDropBottlers] Lỗi OnStorageChange: " + e.Message);
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

    // Harmony Patch: Ngăn chặn NullReferenceException tại Workable.GetConversationTopic khi workable bị Unity destroy (fake null)
    [HarmonyPatch(typeof(Workable), nameof(Workable.GetConversationTopic))]
    public class Workable_GetConversationTopic_Patch
    {
        public static bool Prefix(Workable __instance, ref string __result)
        {
            try
            {
                if (__instance == null || __instance.gameObject == null)
                {
                    __result = null;
                    return false;
                }
                var kPrefabID = __instance.GetComponent<KPrefabID>();
                if (kPrefabID == null)
                {
                    __result = null;
                    return false;
                }
                if (kPrefabID.HasTag(GameTags.NotConversationTopic))
                {
                    __result = null;
                    return false;
                }
                __result = kPrefabID.PrefabTag.Name;
                return false;
            }
            catch (System.Exception)
            {
                __result = null;
                return false;
            }
        }
    }

    // Các Harmony Patch bổ sung component Automatable cho các công trình nhận chai để Auto-Sweeper có thể cung cấp
    [HarmonyPatch(typeof(BottleEmptierConfig), "DoPostConfigureComplete")]
    public static class BottleEmptierConfig_DoPostConfigureComplete_Patch
    {
        public static void Postfix(GameObject go)
        {
            if (go != null)
            {
                go.AddOrGet<Automatable>();
            }
        }
    }

    [HarmonyPatch(typeof(BottleEmptierGasConfig), "DoPostConfigureComplete")]
    public static class BottleEmptierGasConfig_DoPostConfigureComplete_Patch
    {
        public static void Postfix(GameObject go)
        {
            if (go != null)
            {
                go.AddOrGet<Automatable>();
            }
        }
    }

    [HarmonyPatch(typeof(BottleEmptierConduitLiquidConfig), "DoPostConfigureComplete")]
    public static class BottleEmptierConduitLiquidConfig_DoPostConfigureComplete_Patch
    {
        public static void Postfix(GameObject go)
        {
            if (go != null)
            {
                go.AddOrGet<Automatable>();
            }
        }
    }

    [HarmonyPatch(typeof(BottleEmptierConduitGasConfig), "DoPostConfigureComplete")]
    public static class BottleEmptierConduitGasConfig_DoPostConfigureComplete_Patch
    {
        public static void Postfix(GameObject go)
        {
            if (go != null)
            {
                go.AddOrGet<Automatable>();
            }
        }
    }

    [HarmonyPatch(typeof(AdvancedResearchCenterConfig), "DoPostConfigureComplete")]
    public static class AdvancedResearchCenterConfig_DoPostConfigureComplete_Patch
    {
        public static void Postfix(GameObject go)
        {
            if (go != null)
            {
                go.AddOrGet<Automatable>();
            }
        }
    }
}
