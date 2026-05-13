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
            // Harmony will automatically find and apply all [HarmonyPatch] classes in this assembly
        }
    }

    // A component that handles the UI Checkbox for Auto Drop and saves the state
    [SerializationConfig(MemberSerialization.OptIn)]
    public class AutoDropControl : KMonoBehaviour, ICheckboxControl
    {
        [Serialize]
        public bool autoDropEnabled = true; // Set to true by default as per common request

        public string CheckboxTitleKey => "AUTODROP_TITLE";
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

    // Add the control to Gas Bottler
    [HarmonyPatch(typeof(GasBottlerConfig), "DoPostConfigureComplete")]
    public class GasBottlerConfig_DoPostConfigureComplete_Patch
    {
        public static void Postfix(GameObject go)
        {
            go.AddOrGet<AutoDropControl>();
        }
    }

    // Add the control to Liquid Bottler
    [HarmonyPatch(typeof(LiquidBottlerConfig), "DoPostConfigureComplete")]
    public class LiquidBottlerConfig_DoPostConfigureComplete_Patch
    {
        public static void Postfix(GameObject go)
        {
            go.AddOrGet<AutoDropControl>();
        }
    }

    // Modify the Bottler so it checks the autoDropEnabled state
    [HarmonyPatch(typeof(Bottler), "CreateBottleProxyObject")]
    public class Bottler_CreateBottleProxyObject_Patch
    {
        public static void Postfix(Bottler __instance)
        {
            // Get our custom control from the building
            AutoDropControl control = __instance.GetComponent<AutoDropControl>();

            // If auto drop is enabled and we have items
            if (control != null && control.autoDropEnabled && __instance.storage != null && __instance.storage.items.Count > 0)
            {
                __instance.storage.DropAll(false, false, default, true);
                
                // Call the private method CleanupBottleProxyObject using Traverse
                Traverse.Create(__instance).Method("CleanupBottleProxyObject").GetValue();
            }
        }
    }
}
