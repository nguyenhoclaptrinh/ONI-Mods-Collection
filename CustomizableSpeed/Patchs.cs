using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using UnityEngine;

namespace CustomizableSpeed
{
    public class CustomizableSpeed : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new POptions().RegisterOptions(this, typeof(SpeedOptions));
            Debug.Log("[CustomizableSpeed] Mod loaded and options registered.");
        }
    }

    [HarmonyPatch(typeof(Game), "Load")]
    public static class GameOnLoadPatch
    {
        public static SpeedOptions Settings { get; private set; }

        public static void Prefix()
        {
            ReadSettings();
        }
        public static void ReadSettings()
        {
            Settings = POptions.ReadSettings<SpeedOptions>();
            if (Settings == null)
            {
                Settings = new SpeedOptions();
            }
            Debug.Log($"[CustomizableSpeed] Settings loaded: Slow={Settings.slowSpeed}, Normal={Settings.normalSpeed}, Super={Settings.superSpeed}");
        }
    }


    [HarmonyPatch(typeof(SpeedControlScreen), "OnChanged")]
    public static class SpeedControlPatchOnChanged
    {
        public static void Postfix(SpeedControlScreen __instance)
        {
            if (GameOnLoadPatch.Settings == null)
            {
                GameOnLoadPatch.ReadSettings();
            }

            if (__instance.IsPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                float newScale = 1.0f;
                int currentSpeed = __instance.GetSpeed();
                
                switch (currentSpeed)
                {
                    case 0:
                        newScale = GameOnLoadPatch.Settings.slowSpeed;
                        break;
                    case 1:
                        newScale = GameOnLoadPatch.Settings.normalSpeed;
                        break;
                    case 2:
                        newScale = GameOnLoadPatch.Settings.superSpeed;
                        break;
                    default:
                        return;
                }
                
                Time.timeScale = newScale;
                // Debug.Log($"[CustomizableSpeed] Speed changed to {currentSpeed}, TimeScale set to {newScale}");
            }
        }
    }
}
