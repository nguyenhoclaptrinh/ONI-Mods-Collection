using HarmonyLib;
using KSerialization;
using UnityEngine;

namespace AutoDropBottlers
{
    // Entry point for the mod
    public class AutoDropBottlersMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            
            // Add strings for the UI
            Strings.Add("STRINGS.UI.UISIDESCREENS.AUTODROP.TITLE", "Auto Drop Bottles");
            Strings.Add("STRINGS.UI.UISIDESCREENS.AUTODROP.TOOLTIP", "If enabled, bottles will automatically drop when the bottler reaches maximum capacity or is emptied manually.");
            
            Debug.Log("[AutoDropBottlers] Loaded and strings registered.");
        }
    }

    // A component that handles the UI Checkbox for Auto Drop and saves the state
    [SerializationConfig(MemberSerialization.OptIn)]
    public class AutoDropControl : KMonoBehaviour, ICheckboxControl
    {
        [Serialize]
        public bool autoDropEnabled = true; // Set to true by default as per common request

        public string CheckboxTitleKey => "STRINGS.UI.UISIDESCREENS.AUTODROP.TITLE";
        public string CheckboxToolTipKey => "STRINGS.UI.UISIDESCREENS.AUTODROP.TOOLTIP";
        public string CheckboxLabel => "Auto Drop Bottles";
        public string CheckboxTooltip => "If enabled, bottles will automatically drop when the bottler reaches maximum capacity or is emptied manually.";

        public bool GetCheckboxValue()
        {
            return autoDropEnabled;
        }

        public void SetCheckboxValue(bool value)
        {
            autoDropEnabled = value;
            if (value)
            {
                // Nếu bật, thử kiểm tra xem đã đầy chưa để rớt luôn
                OnStorageChange(null);
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            // Đăng ký sự kiện thay đổi dung lượng của công trình thay vì patch Update loop
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
        }

        private void OnStorageChange(object data)
        {
            if (!autoDropEnabled) return;

            Bottler bottler = GetComponent<Bottler>();
            if (bottler != null && bottler.storage != null)
            {
                float currentMass = bottler.storage.GetMassAvailable(GameTags.Any);
                float maxCapacity = bottler.UserMaxCapacity;

                // Nếu lượng chất lỏng/khí đạt tối đa, nhả bình ngay lập tức
                if (currentMass >= maxCapacity && maxCapacity > 0)
                {
                    bottler.storage.DropAll(false, false, default, true);
                }
            }
        }
    }

    // Add the control to all Bottler instances (Gas and Liquid)
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
}
