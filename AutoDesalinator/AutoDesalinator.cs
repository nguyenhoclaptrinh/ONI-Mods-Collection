using HarmonyLib;
using UnityEngine;
using KSerialization;
using System.Collections.Generic;

namespace AutoDesalinator
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class AutoDropSaltControl : KMonoBehaviour, ICheckboxControl
    {
        [Serialize]
        public bool autoDropEnabled = true;

        public string CheckboxTitleKey => "STRINGS.UI.UISIDESCREENS.AUTODROP.TITLE";
        public string CheckboxToolTipKey => "STRINGS.UI.UISIDESCREENS.AUTODROP.TOOLTIP";
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
            if (autoDropEnabled) CheckAndDrop();
        }

        private void CheckAndDrop()
        {
            // Duyệt qua tất cả các storage của Desalinator
            foreach (Storage storage in GetComponents<Storage>())
            {
                if (storage == null || storage.items == null) continue;
                for (int i = storage.items.Count - 1; i >= 0; i--)
                {
                    GameObject item = storage.items[i];
                    if (item == null) continue;

                    PrimaryElement pe = item.GetComponent<PrimaryElement>();
                    // Nếu là chất rắn (Muối) và khối lượng đạt ngưỡng rớt (để tránh rớt lắt nhắt gây lag)
                    // Mặc định Desalinator đầy ở 1000kg, ta cho rớt khi >= 900kg hoặc bất cứ khi nào tắt máy.
                    if (pe != null && pe.Element.IsSolid)
                    {
                        if (pe.Mass >= 900f)
                        {
                            storage.Drop(item, true);
                        }
                    }
                }
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
