using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(StatusItemGroup), nameof(StatusItemGroup.AddStatusItem))]
    internal class Patch_StatusItemGroup_AddStatusItem
    {
        // Transpiler to replace default ShouldShowIcon with a custom version
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            try
            {
                MethodInfo targetMethodIcon = AccessTools.Method(typeof(StatusItem), nameof(StatusItem.ShouldShowIcon));
                if (targetMethodIcon == null)
                {
                    Debug.LogWarning("[SuppressNotifications] Lỗi Transpiler: Không tìm thấy phương thức ShouldShowIcon gốc, bỏ qua patch.");
                    return instructions;
                }

                List<CodeInstruction> list = new List<CodeInstruction>();
                foreach (CodeInstruction i in instructions)
                {
                    if (i.Is(OpCodes.Callvirt, targetMethodIcon))
                    {
                        // Load gameObject onto stack
                        list.Add(new CodeInstruction(OpCodes.Ldarg_0));
                        list.Add(new CodeInstruction(OpCodes.Call,
                            AccessTools.Method(typeof(StatusItemGroup), "get_gameObject")));

                        // Call custom ShouldShowIcon
                        list.Add(new CodeInstruction(OpCodes.Call,
                            AccessTools.Method(typeof(Patch_StatusItemGroup_AddStatusItem), nameof(Patch_StatusItemGroup_AddStatusItem.ShouldShowIconSub))));
                        continue;
                    }

                    list.Add(i);
                }
                return list;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[SuppressNotifications] Lỗi nghiêm trọng trong Transpiler StatusItemGroup.AddStatusItem, fallback an toàn: " + e.Message);
                return instructions;
            }
        }

        private static bool ShouldShowIconSub(StatusItem statusItem, GameObject gameObject)
        {
            return gameObject.GetComponent<StatusItemsSuppressedComp>()?.ShouldShowIcon(statusItem) ?? statusItem.ShouldShowIcon();
        }
    }
}
