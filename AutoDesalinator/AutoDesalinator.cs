using HarmonyLib;
using UnityEngine;
using KSerialization;
using System.Collections.Generic;

namespace AutoDesalinator
{
    // Entry point for the mod to register strings
    public class AutoDesalinatorMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Strings.Add("STRINGS.UI.UISIDESCREENS.AUTODROPSALT.TITLE", "Auto Drop Salt");
            Strings.Add("STRINGS.UI.UISIDESCREENS.AUTODROPSALT.TOOLTIP", "Automatically drop salt when full instead of requiring a duplicant to empty it.");
            Debug.Log("[AutoDesalinator] Loaded and strings registered.");
        }
    }

    [SerializationConfig(MemberSerialization.OptIn)]
    public class AutoDropSaltControl : KMonoBehaviour, ICheckboxControl
    {
        [Serialize]
        public bool autoDropEnabled = true;

        public string CheckboxTitleKey => "STRINGS.UI.UISIDESCREENS.AUTODROPSALT.TITLE";
        public string CheckboxToolTipKey => "STRINGS.UI.UISIDESCREENS.AUTODROPSALT.TOOLTIP";
        public string CheckboxLabel => "Auto Drop Salt";
        public string CheckboxTooltip => "Automatically drop salt when full instead of requiring a duplicant to empty it.";

        public bool GetCheckboxValue() { return autoDropEnabled; }
        public void SetCheckboxValue(bool value)
        {
            autoDropEnabled = value;
            if (value) CheckAndDrop();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
        }

        private void OnStorageChange(object data)
        {
            try
            {
                if (autoDropEnabled) CheckAndDrop();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[AutoDesalinator] Lỗi xử lý OnStorageChange: " + e.Message);
            }
        }

        private void CheckAndDrop()
        {
            try
            {
                // Duyệt qua tất cả các storage của Desalinator
                foreach (Storage storage in GetComponents<Storage>())
                {
                    if (storage == null || storage.items == null) continue;
                    for (int i = storage.items.Count - 1; i >= 0; i--)
                    {
                        GameObject item = storage.items[i];
                        if (item == null) continue;

                        try
                        {
                            PrimaryElement pe = item.GetComponent<PrimaryElement>();
                            // Hạ ngưỡng xuống 500kg để an toàn tuyệt đối, tránh việc máy dừng hoạt động 
                            // trước khi đạt tới 900kg nếu Klei có thay đổi capacity.
                            if (pe != null && pe.Element != null && pe.Element.IsSolid)
                            {
                                if (pe.Mass >= 500f)
                                {
                                    storage.Drop(item, true);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogWarning("[AutoDesalinator] Lỗi khi kiểm tra/thả item trong storage: " + ex.Message);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[AutoDesalinator] Lỗi xử lý CheckAndDrop: " + e.Message);
            }
        }
    }

    // Gắn Component này vào máy Desalinator
    [HarmonyPatch(typeof(DesalinatorConfig), "ConfigureBuildingTemplate")]
    public class DesalinatorConfig_ConfigureBuildingTemplate_Patch
    {
        public static void Postfix(GameObject go)
        {
            go.AddOrGet<AutoDropSaltControl>();
        }
    }
}
