using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using UtilLibs;

namespace GraphicsOptimizer
{
    public class GraphicsOptimizerMod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            // Khởi tạo thư viện PLib và đăng ký Options menu
            PUtil.InitLibrary(false);
            new POptions().RegisterOptions(this, typeof(Config));
            
            base.OnLoad(harmony);
            SgtLogger.LogVersion(this, harmony);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            CompatibilityNotifications.FixBrokenTimeout(harmony);
        }
    }
}
