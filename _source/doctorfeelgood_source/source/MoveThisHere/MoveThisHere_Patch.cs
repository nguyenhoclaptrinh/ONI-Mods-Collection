using System.Collections.Generic;
using HarmonyLib;
using KMod;
using UnityEngine;
using System.IO;
using static Localization;

namespace MoveThisHere
{
    public class MoveThisHere_Patch : UserMod2
    {

        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            private static readonly string ModPath = GetModPath();

            public static void Postfix()
            {
                RegisterForTranslation(typeof(STRINGS));
                GenerateStringsTemplate(typeof(STRINGS), Path.Combine(Manager.GetDirectory(), "strings_templates"));
                LoadStrings();
                LocString.CreateLocStringKeys(typeof(STRINGS), null);
            }

            private static void LoadStrings()
            {
                string localeCode = GetLocale()?.Code;
                if (string.IsNullOrEmpty(localeCode))
                    return;

                string path = Path.Combine(ModPath, "locales", localeCode + ".po");
                if (File.Exists(path))
                    OverloadStrings(LoadStringsFile(path, false));
            }

            private static string GetModPath()
            {
                var assembly = typeof(Localization_Initialize_Patch).Assembly;
                return Path.GetDirectoryName(assembly.Location);
            }
        }
        

        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Utils.AddPlan("Base", "storage", HaulingPointConfig.Id, "StorageLocker");
            }
        }

        [HarmonyPatch(typeof(ProductInfoScreen))]
        [HarmonyPatch(nameof(ProductInfoScreen.SetMaterials))]
        public static class ProductInfoScreen_SetMaterials_Patch
        {
            public static void Postfix(BuildingDef def, ref ProductInfoScreen __instance)
            {
                try
                {
                    if (def != null && def.name == "HaulingPoint" && __instance != null && __instance.materialSelectionPanel != null)
                    {
                        __instance.materialSelectionPanel.gameObject.SetActive(false); //remove material selector since no materials
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("[MoveThisHere] Lỗi Postfix SetMaterials: " + e.Message);
                }
            }
        }

        [HarmonyPatch(typeof(ResourceRemainingDisplayScreen))]
        [HarmonyPatch(nameof(ResourceRemainingDisplayScreen.GetString))]
        public static class ResourceRemainingDisplayScreen_Patch
        {
            public static string Postfix(string __result, Recipe ___currentRecipe)
            {
                try
                {
                    if (___currentRecipe != null && ___currentRecipe.Ingredients != null && ___currentRecipe.Ingredients.Count > 0 && ___currentRecipe.Ingredients[0].amount == 1f)
                    {
                        if (BuildTool.Instance != null)
                        {
                            var hoverCard = BuildTool.Instance.GetComponent<BuildToolHoverTextCard>();
                            if (hoverCard != null && hoverCard.currentDef != null && hoverCard.currentDef.name == "HaulingPoint")
                            {
                                __result = "No resources required";
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("[MoveThisHere] Lỗi Postfix ResourceRemainingDisplayScreen: " + e.Message);
                }
                return __result;
            }
        }

        [HarmonyPatch(typeof(BuildingDef))]
        [HarmonyPatch(nameof(BuildingDef.Instantiate))]
        public static class BuildingDef_Instantiate_Patch
        {
            public static bool Prefix(Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer, BuildingDef __instance, ref GameObject __result)
            {
                try
                {
                    if (__instance == null || __instance.PrefabID != HaulingPointConfig.Id)
                    {
                        return true;
                    }

                    if (selected_elements == null || selected_elements.Count == 0)
                    {
                        return true; // Fallback an toàn
                    }

                    selected_elements[0] = TagManager.Create("Vacuum");
                    __result = __instance.Build(Grid.PosToCell(pos), orientation, null, selected_elements, 293.15f, playsound: false, GameClock.Instance.GetTime());
                    return false;
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("[MoveThisHere] Lỗi Prefix Instantiate: " + e.Message);
                    return true; // Phục hồi bằng cách chạy game gốc
                }
            }
        }
    }

    public static class Utils
    {
        //public static void AddBuildingStrings(string buildingId, string name, string description, string effect)
        //{
        //    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, buildingId));
        //    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.DESC", description);
        //    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.EFFECT", effect);
        //}

        public static void AddPlan(HashedString category, string subcategory, string idBuilding, string addAfter = null)
        {
            Debug.Log("Adding " + idBuilding + " to category " + category);
            foreach (PlanScreen.PlanInfo menu in TUNING.BUILDINGS.PLANORDER)
            {
                if (menu.category == category)
                {
                    AddPlanToCategory(menu, subcategory, idBuilding, addAfter);
                    return;
                }
            }

            Debug.Log($"Unknown build menu category: ${category}");
        }

        private static void AddPlanToCategory(PlanScreen.PlanInfo menu, string subcategory, string idBuilding, string addAfter = null)
        {
            var data = menu.buildingAndSubcategoryData;
            if (data != null)
            {
                var item = new KeyValuePair<string, string>(idBuilding, subcategory);
                if (addAfter == null)
                {
                    data.Add(item);
                }
                else
                {
                    int index = data.FindIndex(x => x.Key == addAfter);
                    if (index == -1)
                    {
                        Debug.Log($"Could not find building {addAfter} in category {menu.category} to add {idBuilding} after. Adding at the end!");
                        data.Add(item);
                        return;
                    }
                    data.Insert(index + 1, item);
                }
            }
        }
    }
}

