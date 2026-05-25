using HarmonyLib;
using KSerialization;
using UnityEngine;

namespace AutoDropBottlers
{
    // Khởi tạo mod và đăng ký ngôn ngữ UI
    public class AutoDropBottlersMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            
            // Đăng ký chuỗi ngôn ngữ cho giao diện UI
            Strings.Add("STRINGS.UI.UISIDESCREENS.AUTODROP.TITLE", "Auto Drop Bottles");
            Strings.Add("STRINGS.UI.UISIDESCREENS.AUTODROP.TOOLTIP", "If enabled, bottles will automatically drop when the bottler reaches maximum capacity or is emptied manually.");
            
            Debug.Log("[AutoDropBottlers] Loaded and strings registered successfully.");
        }
    }

    // Component điều khiển Checkbox trên giao diện UI
    [SerializationConfig(MemberSerialization.OptIn)]
    public class AutoDropControl : KMonoBehaviour, ICheckboxControl
    {
        [Serialize]
        public bool autoDropEnabled = false; // Mặc định tắt

        public string CheckboxTitleKey => "STRINGS.UI.UISIDESCREENS.AUTODROP.TITLE";
        public string CheckboxToolTipKey => "STRINGS.UI.UISIDESCREENS.AUTODROP.TOOLTIP";
        public string CheckboxLabel => "Auto Drop Bottles";
        public string CheckboxTooltip => "If enabled, bottles will automatically drop when the bottler reaches maximum capacity or is emptied manually.";

        private Bottler bottler;

        public bool GetCheckboxValue()
        {
            return autoDropEnabled;
        }

        public void SetCheckboxValue(bool value)
        {
            autoDropEnabled = value;
            if (value)
            {
                // Nếu bật, kiểm tra ngay xem trong máy có chai nước không để thả luôn
                TriggerDrop();
            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            bottler = GetComponent<Bottler>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            // Đăng ký sự kiện thay đổi dung lượng kho chứa (tốt cho máy tự động bơm không cần đệ)
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
        }

        // Tách biệt logic thả toàn bộ chai trong kho chứa ra đất
        public void TriggerDrop()
        {
            if (!autoDropEnabled || bottler == null || bottler.storage == null || bottler.storage.IsEmpty())
                return;

            // Đảm bảo không có Duplicant nào đang trực tiếp đứng vận hành máy bơm để tránh làm ngắt quãng công việc
            if (bottler.worker == null)
            {
                bottler.storage.DropAll(false, false, default, true);
            }
        }

        private void OnStorageChange(object data)
        {
            if (!autoDropEnabled || bottler == null || bottler.storage == null) return;

            // Nếu kho chứa đầy, trì hoãn 1 frame để đảm bảo đồng bộ sự kiện của game và thả chai
            if (bottler.storage.IsFull())
            {
                GameScheduler.Instance.Schedule("AutoDropOnStorageFull", 0f, (obj) => {
                    // Kiểm tra an toàn trước khi thực hiện để tránh NullReferenceException nếu công trình bị phá hủy
                    if (this != null && bottler != null && bottler.storage != null && bottler.storage.IsFull())
                    {
                        TriggerDrop();
                    }
                });
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
                    // Trì hoãn 1 frame để Duplicant hoàn toàn giải phóng khỏi trạng thái làm việc (workable state) trước khi drop
                    GameScheduler.Instance.Schedule("AutoDropOnStopWork", 0f, (obj) => {
                        if (control != null)
                        {
                            control.TriggerDrop();
                        }
                    });
                }
            }
        }
    }
}
