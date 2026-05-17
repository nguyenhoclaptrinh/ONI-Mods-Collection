using System.Collections.Generic;
using KMod;
using static Localization;
using HarmonyLib;
using UnityEngine;

namespace Auto_Liquid_Bottler
{
    public class ALB_Mod : UserMod2
    {
        public static UserMod2 _mod;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            _mod = this;
        }

        // Patch 1: Thêm đầu vào đường ống lỏng vào BuildingDef của vanilla LiquidBottler
        [HarmonyPatch(typeof(LiquidBottlerConfig), "CreateBuildingDef")]
        public class LiquidBottlerDefPatch
        {
            static void Postfix(ref BuildingDef __result)
            {
                __result.InputConduitType = ConduitType.Liquid;
                __result.UtilityInputOffset = new CellOffset(0, 0);
                __result.ViewMode = OverlayModes.LiquidConduits.ID;
                GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, LiquidBottlerConfig.ID);
            }
        }

        // Patch 2: Inject component auto-drop và ConduitConsumer vào vanilla LiquidBottler
        [HarmonyPatch(typeof(LiquidBottlerConfig), "ConfigureBuildingTemplate")]
        public class LiquidBottlerTemplatePatch
        {
            static void Postfix(GameObject go)
            {
                Storage storage = go.GetComponent<Storage>();

                LiquidBottler lb = go.AddOrGet<LiquidBottler>();
                lb.storage = storage;

                ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
                conduitConsumer.storage = storage;
                conduitConsumer.conduitType = ConduitType.Liquid;
                conduitConsumer.ignoreMinMassCheck = true;
                conduitConsumer.capacityKG = storage.capacityKg;
                conduitConsumer.keepZeroMassObject = false;
            }
        }

        // Patch 3: Localization
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class LiquidBottlerLocalizationPatch
        {
            public static void Postfix()
            {
                Dictionary<string, string> translations;
                string file_po = System.IO.Path.Combine(_mod.path, "translations", GetLocale()?.Code + ".po");
                if (System.IO.File.Exists(file_po))
                {
                    translations = LoadStringsFile(file_po, false);
                }
                else
                {
                    file_po = System.IO.Path.Combine(_mod.path, "translations", "zh" + ".po");
                    if (!System.IO.File.Exists(file_po)) return;
                    translations = LoadStringsFile(file_po, true);
                }
                foreach (KeyValuePair<string, string> translation in translations)
                {
                    Strings.Add(translation.Key, translation.Value);
                }
            }
        }
    }
}
