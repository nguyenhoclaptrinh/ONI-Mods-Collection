using HarmonyLib;
using UnityEngine;
using PeterHan.PLib.Core;
using System.Collections.Generic;
using System.Reflection;

namespace SuitLockerCopySettings
{
    // -------------------------------------------------------------------------
    // Entry point
    // -------------------------------------------------------------------------
    public class SuitLockerCopySettingsMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary(false);
            harmony.PatchAll();
        }
    }

    // -------------------------------------------------------------------------
    // Patch 1: Thêm CopyBuildingSettings vào 3 loại SuitLocker
    //   copyGroupTag = prefab_tag đảm bảo chỉ copy cùng loại
    //   (Atmo→Atmo, Jet→Jet, Lead→Lead)
    // -------------------------------------------------------------------------
    [HarmonyPatch]
    internal static class SuitLockerConfig_ConfigureBuildingTemplate_Patch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            const string methodName = nameof(IBuildingConfig.ConfigureBuildingTemplate);
            yield return typeof(SuitLockerConfig).GetMethod(methodName);
            yield return typeof(JetSuitLockerConfig).GetMethod(methodName);
            if (DlcManager.IsExpansion1Active())
                yield return typeof(LeadSuitLockerConfig).GetMethod(methodName);
        }

        static void Postfix(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = prefab_tag;
            go.AddOrGet<SuitLockerCopySettingsComponent>();
        }
    }

    // -------------------------------------------------------------------------
    // Component xử lý việc sao chép cài đặt tủ đồ bảo hộ
    // Lắng nghe sự kiện GameHashes.CopySettings trực tiếp trên GameObject
    // -------------------------------------------------------------------------
    public class SuitLockerCopySettingsComponent : KMonoBehaviour
    {
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        private void OnCopySettings(object data)
        {
            var srcGo = data as GameObject;
            if (srcGo == null) return;

            var srcLocker = srcGo.GetComponent<SuitLocker>();
            var dstLocker = GetComponent<SuitLocker>();
            if (srcLocker == null || dstLocker == null) return;

            // Chỉ copy nếu source đã được cấu hình
            if (srcLocker.smi == null || !srcLocker.smi.sm.isConfigured.Get(srcLocker.smi)) return;

            // Nếu source đang đợi suit hoặc đã có suit -> yêu cầu suit trên target
            if (srcLocker.smi.sm.isWaitingForSuit.Get(srcLocker.smi) || srcLocker.GetStoredOutfit() != null)
                dstLocker.ConfigRequestSuit();
            else
                dstLocker.ConfigNoSuit();
        }
    }
}

