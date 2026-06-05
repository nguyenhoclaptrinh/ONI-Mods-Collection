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
    public static class AddMovableStructurePatch {
        public static IEnumerable<MethodBase> TargetMethods() {
            var types = new List<System.Type> {
                typeof(Geyser),
                typeof(GeneShuffler),
                typeof(WarpPortal),
                typeof(WarpReceiver),
                typeof(CryoTank),
                typeof(Telepad)
            };
            foreach (var type in types) {
                var method = AccessTools.Method(type, "OnSpawn");
                if (method != null)
                    yield return method;
            }
        }

        public static void Postfix(KMonoBehaviour __instance) {
            if (__instance != null)
                __instance.gameObject.AddOrGet<MovableGeyser>();
        }
    }
}
