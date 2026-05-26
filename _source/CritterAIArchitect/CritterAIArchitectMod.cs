using HarmonyLib;
using KMod;

namespace CritterAIArchitect {
    public sealed class CritterAIArchitectMod : UserMod2 {
        public override void OnLoad(Harmony harmony) {
            base.OnLoad(harmony);
            harmony.PatchAll();
            Debug.Log("CritterAIArchitect: Mod loaded and patches applied successfully!");
        }
    }
}
