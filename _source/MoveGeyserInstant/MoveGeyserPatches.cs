using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace MoveGeyserInstant {
    internal static class MovableStructureSupport {
        internal static void AddMovable(GameObject go) {
            if (go != null)
                go.AddOrGet<MovableGeyser>();
        }
    }

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
            var typeNames = new List<string> {
                "Geyser",
                "GeneShuffler",
                "WarpPortal",
                "WarpReceiver",
                "CryoTank",
                "Telepad",
                "MegaBrainTank",
                "GravitasCreatureManipulator",
                "OilWell",
                "OilWellCap"
            };
            foreach (var name in typeNames) {
                var type = AccessTools.TypeByName(name);
                if (type == null) continue;
                var method = AccessTools.Method(type, "OnSpawn");
                if (method != null)
                    yield return method;
            }
        }

        public static void Postfix(KMonoBehaviour __instance) {
            if (__instance != null)
                MovableStructureSupport.AddMovable(__instance.gameObject);
        }
    }

    [HarmonyPatch(typeof(OilWellConfig), nameof(OilWellConfig.CreatePrefab))]
    public static class OilWellConfigCreatePrefabPatch {
        public static void Postfix(GameObject __result) {
            MovableStructureSupport.AddMovable(__result);
        }
    }

    [HarmonyPatch(typeof(OilWellCapConfig), nameof(OilWellCapConfig.ConfigureBuildingTemplate))]
    public static class OilWellCapConfigConfigureBuildingTemplatePatch {
        public static void Postfix(GameObject go) {
            MovableStructureSupport.AddMovable(go);
        }
    }
}
