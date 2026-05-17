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

    [HarmonyPatch(typeof(Bottler), "UpdateStoredItemState")]
    public class Bottler_UpdateStoredItemState_Patch
    {
        public static void Postfix(Bottler __instance)
        {
            if (__instance == null) return;

            AutoDropControl control = __instance.GetComponent<AutoDropControl>();
            bool enabled = control != null && control.autoDropEnabled;

            if (__instance.storage != null)
            {
                float currentMass = __instance.storage.GetMassAvailable(GameTags.Any);
                float maxCapacity = __instance.UserMaxCapacity;

                // Log every change where mass > 0 to help debug
                if (currentMass > 0)
                {
                    Debug.Log($"[AutoDropBottlers] {__instance.name} Updated - Mass: {currentMass:F3}kg, Max: {maxCapacity:F3}kg, Enabled: {enabled}");
                }

                // If storage is at or above user set capacity, drop it
                if (enabled && currentMass >= maxCapacity && maxCapacity > 0)
                {
                    Debug.Log($"[AutoDropBottlers] Auto-dropping (Full): {currentMass:F3}kg / {maxCapacity:F3}kg");
                    __instance.storage.DropAll(false, false, default, true);
                }
            }
        }
    }


    [HarmonyPatch(typeof(Bottler), "OnCompleteWork")]
    public class Bottler_OnCompleteWork_Patch
    {
        public static void Postfix(Bottler __instance)
        {
            if (__instance == null) return;
            AutoDropControl control = __instance.GetComponent<AutoDropControl>();
            if (control != null && control.autoDropEnabled && __instance.storage != null)
            {
                if (__instance.storage.items.Count > 0)
                {
                    Debug.Log($"[AutoDropBottlers] Auto-dropping (Work Complete): {__instance.name}");
                    __instance.storage.DropAll(false, false, default, true);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Bottler), "OnStopWork")]
    public class Bottler_OnStopWork_Patch
    {
        public static void Postfix(Bottler __instance)
        {
            if (__instance == null) return;
            AutoDropControl control = __instance.GetComponent<AutoDropControl>();
            if (control != null && control.autoDropEnabled && __instance.storage != null)
            {
                if (__instance.storage.items.Count > 0)
                {
                    Debug.Log($"[AutoDropBottlers] Auto-dropping (Work Stop): {__instance.name}");
                    __instance.storage.DropAll(false, false, default, true);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Bottler), "CreateBottleProxyObject")]
    public class Bottler_CreateBottleProxyObject_Patch
    {
        public static void Postfix(Bottler __instance)
        {
            if (__instance == null) return;
            AutoDropControl control = __instance.GetComponent<AutoDropControl>();
            if (control != null && control.autoDropEnabled && __instance.storage != null)
            {
                if (__instance.storage.items.Count > 0)
                {
                    Debug.Log($"[AutoDropBottlers] Auto-dropping (Proxy Created): {__instance.name}");
                    __instance.storage.DropAll(false, false, default, true);
                }
            }
        }
    }
}
