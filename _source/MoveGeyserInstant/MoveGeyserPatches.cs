using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace MoveGeyserInstant {
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.OnPrefabInit))]
    public static class RegisterMoveGeyserToolPatch {
        public static void Postfix(PlayerController __instance) {
            var tools = new List<InterfaceTool>(__instance.tools);
            var toolObject = new GameObject(nameof(MoveGeyserTool));
            toolObject.AddComponent<MoveGeyserTool>();
            toolObject.transform.SetParent(__instance.gameObject.transform);
            toolObject.SetActive(true);
            toolObject.SetActive(false);
            tools.Add(toolObject.GetComponent<InterfaceTool>());
            __instance.tools = tools.ToArray();
        }
    }

    [HarmonyPatch]
    public static class AddMovableGeyserPatch {
        public static MethodBase TargetMethod() {
            return AccessTools.Method(typeof(Geyser), "OnSpawn");
        }

        public static void Postfix(Geyser __instance) {
            if (__instance != null)
                __instance.gameObject.AddOrGet<MovableGeyser>();
        }
    }
}
