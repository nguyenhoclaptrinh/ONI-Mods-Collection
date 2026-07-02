using HarmonyLib;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Reflection;

namespace CameraJumpDebugger
{
    public class CameraJumpDebuggerMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("[CameraJumpDebugger] Loaded!");
            
            // Try to patch all methods in CameraController that might change position
            PatchMethod(harmony, typeof(CameraController), "SetTargetPos");
            PatchMethod(harmony, typeof(CameraController), "SnapTo");
            PatchMethod(harmony, typeof(CameraController), "SetPosition");
        }

        private void PatchMethod(Harmony harmony, Type type, string methodName)
        {
            try
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    if (method.Name == methodName)
                    {
                        var prefix = typeof(CameraJumpDebuggerMod).GetMethod(nameof(Prefix), BindingFlags.Static | BindingFlags.Public);
                        harmony.Patch(method, new HarmonyMethod(prefix));
                        Debug.Log($"[CameraJumpDebugger] Patched {methodName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"[CameraJumpDebugger] Failed to patch {methodName}: {ex}");
            }
        }

        public static void Prefix(MethodBase __originalMethod)
        {
            Debug.Log($"[CameraJumpDebugger] {__originalMethod.Name} called!\n{new StackTrace(true)}");
        }
    }
}
