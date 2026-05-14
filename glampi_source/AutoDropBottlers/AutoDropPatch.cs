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

    // Modify the Bottler so it checks the autoDropEnabled state
    [HarmonyPatch(typeof(Bottler), "CreateBottleProxyObject")]
    public class Bottler_CreateBottleProxyObject_Patch
    {
        public static void Postfix(Bottler __instance)
        {
            if (__instance == null) return;

            AutoDropControl control = __instance.GetComponent<AutoDropControl>();
            bool enabled = control != null && control.autoDropEnabled;
            
            int itemCount = (__instance.storage != null) ? __instance.storage.items.Count : -1;
            float totalMass = (__instance.storage != null) ? __instance.storage.GetMassAvailable(GameTags.Any) : 0f;

            // Log details for debugging
            if (enabled && __instance.storage != null && (itemCount > 0 || totalMass > 0f))
            {
                Debug.Log($"[AutoDropBottlers] Auto-dropping: Items={itemCount}, Mass={totalMass}");
                __instance.storage.DropAll(false, false, default, true);
                
                // We DON'T call CleanupBottleProxyObject here anymore.
                // The game will call it naturally in OnStopWork, avoiding the "could not clean up" warning.
            }
        }
    }
}
