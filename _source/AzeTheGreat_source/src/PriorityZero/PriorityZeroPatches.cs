using HarmonyLib;
using System.Collections.Generic;

namespace PriorityZero
{
    public class PriorityZeroMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("Priority Zero Mod (v706793+) - Loaded successfully!");
        }
    }

    public static class PriorityZeroState
    {
        private static readonly Dictionary<PriorityScreen, PriorityButton> ZeroButtons = new Dictionary<PriorityScreen, PriorityButton>();
        private static bool loggedPriorityReadFailure;

        public static bool IsPriorityZero(PrioritySetting priority)
        {
            return priority.priority_value == 0;
        }

        public static bool HasZeroPriority(Chore chore)
        {
            if (chore == null)
            {
                return false;
            }

            try
            {
                return IsPriorityZero(chore.masterPriority) ||
                       (chore.prioritizable != null && IsPriorityZero(chore.prioritizable.GetMasterPriority()));
            }
            catch (System.Exception e)
            {
                if (!loggedPriorityReadFailure)
                {
                    loggedPriorityReadFailure = true;
                    Debug.LogWarning("Priority Zero: Failed to read chore priority, allowing chore to continue. " + e);
                }

                return false;
            }
        }

        public static bool HasZeroButton(PriorityScreen screen)
        {
            return TryGetZeroButton(screen, out _);
        }

        public static void RegisterZeroButton(PriorityScreen screen, PriorityButton button)
        {
            ZeroButtons[screen] = button;
            RefreshZeroButton(screen, screen.lastSelectedPriority);
        }

        public static bool TryGetZeroButton(PriorityScreen screen, out PriorityButton button)
        {
            button = null;
            if (screen == null)
            {
                return false;
            }

            if (!ZeroButtons.TryGetValue(screen, out button) || button == null)
            {
                ZeroButtons.Remove(screen);
                return false;
            }

            return true;
        }

        public static void RefreshZeroButton(PriorityScreen screen, PrioritySetting selectedPriority)
        {
            if (!TryGetZeroButton(screen, out PriorityButton button) || button.toggle == null)
            {
                return;
            }

            bool isSelected = button.priority == selectedPriority;
            if (isSelected)
            {
                button.toggle.Select();
            }

            button.toggle.isOn = isSelected;
        }
    }

    [HarmonyPatch(typeof(StandardChoreBase), nameof(StandardChoreBase.IsValid))]
    public static class StandardChoreBase_IsValid_Patch
    {
        [HarmonyPriority(Priority.Last)]
        public static bool Prefix(Chore __instance, ref bool __result)
        {
            if (PriorityZeroState.HasZeroPriority(__instance))
            {
                __result = false;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(ChoreConsumer), nameof(ChoreConsumer.FindNextChore))]
    public static class ChoreConsumer_FindNextChore_Patch
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(ref bool __result, ref Chore.Precondition.Context out_context)
        {
            if (__result && PriorityZeroState.HasZeroPriority(out_context.chore))
            {
                __result = false;
                out_context = new Chore.Precondition.Context();
            }
        }
    }

    [HarmonyPatch(typeof(PrioritizeTool), nameof(PrioritizeTool.Update))]
    public static class PrioritizeTool_Update_Patch
    {
        public static bool Prefix(PrioritizeTool __instance)
        {
            PriorityScreen priorityScreen = ToolMenu.Instance != null ? ToolMenu.Instance.PriorityScreen : null;
            if (priorityScreen == null || !PriorityZeroState.IsPriorityZero(priorityScreen.GetLastSelectedPriority()))
            {
                return true;
            }

            UnityEngine.Texture2D[] cursors = __instance.cursors;
            if (cursors != null && cursors.Length > 0 && __instance.visualizer != null)
            {
                UnityEngine.MeshRenderer renderer = __instance.visualizer.GetComponentInChildren<UnityEngine.MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material.mainTexture = cursors[0];
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(PriorityScreen), "InstantiateButtons")]
    public static class PriorityScreen_InstantiateButtons_Patch
    {
        public static void Postfix(PriorityScreen __instance)
        {
            if (__instance.buttons_basic == null || __instance.buttons_basic.Count == 0) return;

            if (PriorityZeroState.HasZeroButton(__instance)) return;

            PriorityButton btn1 = __instance.buttons_basic[0];
            if (btn1 == null) return;

            PriorityButton btn0 = UnityEngine.Object.Instantiate(btn1, btn1.transform.parent);
            btn0.name = "button_prio_0";
            btn0.priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 0);
            btn0.onClick = priority =>
            {
                __instance.SetScreenPriority(priority, false);
                __instance.OnClick(priority);
            };

            if (btn0.text != null)
            {
                btn0.text.text = "0";
            }

            if (btn0.tooltip != null)
            {
                btn0.tooltip.SetSimpleTooltip("Vô hiệu hóa công việc (Priority 0) - Duplicants và Auto-Sweepers sẽ bỏ qua");
            }

            btn0.transform.SetAsFirstSibling();
            PriorityZeroState.RegisterZeroButton(__instance, btn0);
        }
    }

    [HarmonyPatch(typeof(PriorityScreen), nameof(PriorityScreen.PlayPriorityConfirmSound))]
    public static class PriorityScreen_PlayPriorityConfirmSound_Patch
    {
        public static bool Prefix(PrioritySetting priority)
        {
            return !PriorityZeroState.IsPriorityZero(priority);
        }
    }

    [HarmonyPatch(typeof(PriorityScreen), nameof(PriorityScreen.SetScreenPriority))]
    public static class PriorityScreen_SetScreenPriority_RefreshZeroPatch
    {
        public static void Postfix(PriorityScreen __instance, PrioritySetting priority)
        {
            PriorityZeroState.RefreshZeroButton(__instance, priority);
        }
    }
}
