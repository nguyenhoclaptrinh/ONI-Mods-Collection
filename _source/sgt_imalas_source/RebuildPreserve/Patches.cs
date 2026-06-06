using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UtilLibs;

namespace RebuildPreserve
{
    internal class Patches
    {
        public static string cacheName = "rebuild_preserve_cachedSource_";
        [HarmonyPatch(typeof(Reconstructable))]
        [HarmonyPatch(nameof(Reconstructable.TryCommenceReconstruct))]
        public static class Reconstructable_TryCommenceReconstruct_Patch
        {
            private static readonly FieldInfo ReconstructRequestedField = AccessTools.Field(typeof(Reconstructable), "reconstructRequested");

            public static void Prefix(Reconstructable __instance)
            {
                if (!IsReconstructRequested(__instance))
                    return;

                var go = __instance.gameObject;
                __instance.TryGetComponent<BuildingComplete>(out var building);
                GameObject cachedSource = null;

                if (building == null || building.Def == null)
                    return;

                foreach (var comp in go.GetComponents(typeof(KMonoBehaviour)))
                {
                    if (comp != null)
                    {
                        if (cachedSource == null)
                        {
                            cachedSource = new GameObject(cacheName + go.name);
                            cachedSource.SetActive(false);
                        }

                        var method = new Traverse(comp).Method("OnCopySettings", new[] { typeof (object)});
                        var method2 = new Traverse(comp).Method("OnCopySettingsDelegate", new[] { typeof(object) });
                        if (method.MethodExists() || method2.MethodExists())
                        {
                            var cache = cachedSource.AddComponent(comp.GetType());
                            //SgtLogger.l(message: "Caching " + comp.GetType().ToString());
                            CopyProperties(comp, cache);
                        }
                    }
                }

                if (go.TryGetComponent<IHaveUtilityNetworkMgr>(out var networkItem))
                {
                    if (cachedSource == null)
                    {
                        cachedSource = new GameObject(cacheName + go.name);
                        cachedSource.SetActive(false);
                    }
                    cachedSource.AddComponent<ConduitDirectionInfo>().StoreConduitConnections(building.NaturalBuildingCell(), networkItem.GetNetworkManager());
                }

                if (cachedSource != null)
                {
                    //SgtLogger.l(message: "adding to dic ");
                    var targetPos = new Tuple<int, ObjectLayer>(building.NaturalBuildingCell(), building.Def.ObjectLayer);

                    BuildSettingsPreservationData.Instance.ReplaceEntry(targetPos, cachedSource, building.Def.PrefabID);

                    //if (cachedSource.TryGetComponent<TreeFilterable>(out var filter))
                    //{

                    //    SgtLogger.l("getting filters, count: " + filter.GetTags().Count());
                    //}
                }
            }

            private static bool IsReconstructRequested(Reconstructable instance)
            {
                return instance != null
                    && ReconstructRequestedField != null
                    && (bool)ReconstructRequestedField.GetValue(instance);
            }

            private static readonly Dictionary<Type, FieldInfo[]> fieldCache = new Dictionary<Type, FieldInfo[]>();

            public static void CopyProperties(object source, object destination)
            {
                if (source == null || destination == null)
                    throw new Exception("Source or/and Destination Objects are null");

                Type type = source.GetType();
                if (!fieldCache.TryGetValue(type, out var fields))
                {
                    var fieldList = new List<FieldInfo>();
                    Type currentType = type;
                    while (currentType != null && currentType != typeof(KMonoBehaviour) && currentType != typeof(StateMachineComponent) && currentType != typeof(MonoBehaviour))
                    {
                        var currentFields = currentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        foreach (var f in currentFields)
                        {
                            if (!fieldList.Exists(existing => existing.Name == f.Name))
                            {
                                fieldList.Add(f);
                            }
                        }
                        currentType = currentType.BaseType;
                    }
                    fields = fieldList.ToArray();
                    fieldCache[type] = fields;
                }

                if (fields == null || fields.Length == 0)
                {
                    SgtLogger.l("no props found: " + source.ToString());
                    return;
                }

                foreach (var field in fields)
                {
                    try
                    {
                        var value = field.GetValue(source);
                        field.SetValue(destination, value);
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogWarning($"[BetterRebuilding] Failed to copy field {field.Name} on {type.FullName}: {ex.Message}");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(BuildingDef), nameof(BuildingDef.TryPlace), new Type[] { typeof(GameObject), typeof(Vector3), typeof(Orientation), typeof(IList<Tag>), typeof(string), typeof(bool), typeof(int) })]
        public class BuildingDef_TryPlace
        {
            public static void Postfix(BuildingDef __instance, GameObject __result, Vector3 pos)
            {
                if (__result == null)
                    return;
                int cell = Grid.PosToCell(pos);

                if (BuildSettingsPreservationData.Instance.TryGetEntry(new(cell, __instance.ObjectLayer), out var cachedData, out var cachedPrefabId)
                    )
                {
                    if (cachedData.TryGetComponent<ConduitDirectionInfo>(out var targetConduitDirectionInfo) && targetConduitDirectionInfo.initialized)
                    {

                        GameScheduler.Instance.ScheduleNextFrame("delayed conduit applying", (_) =>
                        {
                            targetConduitDirectionInfo.ApplyConduitConnections(false);
                            if (__result.TryGetComponent<KAnimGraphTileVisualizer>(out var targetAnimGraphTileVisualizer))
                            {
                                targetAnimGraphTileVisualizer.UpdateConnections(targetConduitDirectionInfo.connectedDirections);
                            }
                        });
                    }

                    if (cachedData.TryGetComponent<Prioritizable>(out _) && __result.TryGetComponent<Prioritizable>(out var targetPrio))
                    {
                        var copyPrioritySettings = Traverse.Create(targetPrio).Method("OnCopySettings", new[] { typeof(object) }, cachedData);
                        if (copyPrioritySettings.MethodExists())
                        {
                            copyPrioritySettings.GetValue();
                        }
                    }

                }
            }
        }


        [HarmonyPatch(typeof(Constructable), nameof(Constructable.OnCancel))]
        public class Constructable_OnCancel
        {
            public static void Prefix(Constructable __instance)
            {
                if (!__instance.TryGetComponent<Building>(out var building) || building.Def == null)
                    return;

                var cell = building.NaturalBuildingCell();
                var layer = building.Def.ObjectLayer;
                BuildSettingsPreservationData.Instance.RemoveEntry(new(cell, layer));
            }
        }

        [HarmonyPatch(typeof(SaveGame), nameof(SaveGame.OnPrefabInit))]
        public class SaveGame_OnPrefabInit_Patch
        {
            public static void Postfix(SaveGame __instance)
            {
                BuildSettingsPreservationData.Instance = __instance.gameObject.AddOrGet<BuildSettingsPreservationData>();
            }
        }

        //[HarmonyPatch(typeof(DetailsScreenMaterialPanel), nameof(DetailsScreenMaterialPanel.SetTarget))]
        //public class DetailsScreenMaterialPanel_SetTarget
        //{
        //    public static void Postfix(DetailsScreenMaterialPanel __instance, GameObject target)
        //    {
        //        if (target == null)
        //            return;

        //        GameScheduler.Instance.Schedule("instantly open material selection panel",0.2f, (_) =>
        //        {
        //            __instance.OpenMaterialSelectionPanel();
        //            __instance.RefreshMaterialSelectionPanel();
        //            __instance.RefreshOrderChangeMaterialButton();
        //        });
        //    }
        //}
        [HarmonyPatch(typeof(BuildingConfigManager), nameof(BuildingConfigManager.OnPrefabInit))]
        public class BuildingConfigManager_OnPrefabInit
        {
            private static readonly FieldInfo BaseTemplateField = AccessTools.Field(typeof(BuildingConfigManager), "baseTemplate");

            public static void Postfix(BuildingConfigManager __instance)
            {
                var baseTemplate = BaseTemplateField?.GetValue(__instance) as GameObject;
                if (baseTemplate != null)
                {
                    baseTemplate.AddComponent<AutomatedBrokenRebuild>();
                }
            }
        }

        public static class ApplySettingsToNewBuilding
        {
            private static readonly FieldInfo GameplayEventBuildingField = AccessTools.Field(typeof(BonusEvent.GameplayEventData), "building");

            [HarmonyPatch(typeof(GameplayEventManager), "OnSpawn")]
            public static class GameplayEventManager_OnSpawn
            {
                public static void Postfix(GameplayEventManager __instance)
                {
                    __instance.Subscribe(-1661515756, OnBuildingConstructed);
                }
            }
            [HarmonyPatch(typeof(GameplayEventManager), "OnCleanUp")]
            public static class GameplayEventManager_OnCleanup
            {
                public static void Postfix(GameplayEventManager __instance)
                {
                    __instance.Unsubscribe(-1661515756, OnBuildingConstructed);
                }
            }
            static void OnBuildingConstructed(object data)
            {
                if (data == null)
                    return;
                //SgtLogger.l("onbuildingconstructed");
                if (data is BonusEvent.GameplayEventData bonusData)
                {
                    var building = GameplayEventBuildingField?.GetValue(bonusData) as BuildingComplete;
                    if (building == null || building.Def == null)
                        return;

                    var pos = building.NaturalBuildingCell();
                    var layer = building.Def.ObjectLayer;
                    var targetPos = new Tuple<int, ObjectLayer>(pos, layer);

                    var targetBuilding = building.gameObject;
                    if (BuildSettingsPreservationData.Instance.TryGetEntry(targetPos, out var cachedGameObject, out var previousPrefabId)
                        && targetBuilding != null
                        && cachedGameObject != null
                        && previousPrefabId == building.Def.PrefabID)
                    {
                        cachedGameObject.AddOrGet<KPrefabID>().PrefabTag = previousPrefabId;


                        GameScheduler.Instance.ScheduleNextFrame("delayed settings application", (_) =>
                        {
                            targetBuilding.Trigger((int)GameHashes.CopySettings, cachedGameObject);
                            if (targetBuilding.TryGetComponent<KAnimGraphTileVisualizer>(out var targetAnimGraphTileVisualizer))
                            {
                                targetAnimGraphTileVisualizer.Refresh();
                            }
                            if(cachedGameObject.TryGetComponent<Bottler>(out var bottler) && targetBuilding.TryGetComponent<Bottler>(out var targetBottler))
							{
                                var maxCapacity = Traverse.Create(bottler).Field("userMaxCapacity").GetValue<float>();
                                SgtLogger.debuglog("bottler hack; setting capacity to " + maxCapacity + " for " + targetBuilding.GetProperName());
								Traverse.Create(targetBottler).Property("UserMaxCapacity").SetValue(maxCapacity); //because the public property getter defaults to 0 for reasons when certain params arent set, gotta set it manually here...
							}

							BuildSettingsPreservationData.Instance.RemoveEntry(targetPos);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Init. auto translation
        /// </summary>
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public static class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                LocalisationUtil.Translate(typeof(STRINGS), true);
            }
        }


        [HarmonyPatch(typeof(DetailsScreenMaterialPanel), "RefreshOrderChangeMaterialButton", new Type[] { typeof(object) })]
        public static class DetailsScreenMaterialPanel_RefreshOrderChangeMaterialButton
        {
            public static void Postfix(DetailsScreenMaterialPanel __instance)
            {
                __instance.orderChangeMaterialButton.isInteractable = __instance.materialSelectionPanel.CurrentSelectedElement != null;
            }
        }
    }
}
