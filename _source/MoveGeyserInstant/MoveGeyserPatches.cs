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

    [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
    public static class AssetsOnPrefabInitPatch {
        public static void Prefix() {
            try {
                string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string assetsFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dllPath), "assets");
                string spritePath = System.IO.Path.Combine(assetsFolder, "MoveGeyserToolIcon.png");
                if (System.IO.File.Exists(spritePath)) {
                    byte[] data = System.IO.File.ReadAllBytes(spritePath);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(data);
                    Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    sprite.name = "MoveGeyserToolIcon";
                    Assets.Sprites.Add("MoveGeyserToolIcon", sprite);
                }
            }
            catch (System.Exception ex) {
                Debug.LogWarning("[MoveGeyserInstant] Failed to load custom sprite: " + ex.Message);
            }
        }
    }

    [HarmonyPatch(typeof(KPrefabID), "OnSpawn")]
    public static class KPrefabIDOnSpawnPatch {
        private static readonly Dictionary<Tag, bool> isMovableCache = new Dictionary<Tag, bool>();

        public static void Postfix(KPrefabID __instance) {
            if (__instance == null) return;

            Tag prefabTag = __instance.PrefabTag;
            if (!isMovableCache.TryGetValue(prefabTag, out bool isMovable)) {
                string name = prefabTag.Name;
                isMovable = name.StartsWith("Prop") || 
                            name.Contains("Satellite") || 
                            name == "LonelyMinionHouse" || 
                            name == "TemporalTearOpener" || 
                            name == "MorbRoverSpawningLocker" || 
                            name.StartsWith("FossilDig") || 
                            name == "AncientMonument";
                isMovableCache[prefabTag] = isMovable;
            }

            if (isMovable) {
                MovableStructureSupport.AddMovable(__instance.gameObject);
            }
        }
    }
}
