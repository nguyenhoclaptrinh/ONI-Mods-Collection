using HarmonyLib;
using System;
using UnityEngine;
using UtilLibs;

namespace GraphicsOptimizer.Patches
{
    // ==========================================
    // 1. TỐI ƯU HÓA SỨA PHÁT SÁNG (LIGHT BUG)
    // ==========================================
    [HarmonyPatch(typeof(Light2D), "OnMoved")]
    public static class Light2D_OnMoved_Patch
    {
        public static bool Prefix(Light2D __instance)
        {
            if (Config.Instance == null) return true;

            // Kiểm tra xem đối tượng có phải là sứa phát sáng hay không
            if (__instance.gameObject != null && __instance.gameObject.name.Contains("LightBug"))
            {
                var mode = Config.Instance.LightBugOptimMode;
                if (mode == Config.LightBugMode.DisableLux)
                {
                    __instance.enabled = false;
                    return false;
                }
                else if (mode == Config.LightBugMode.CellChange)
                {
                    int currentCell = Grid.PosToCell(__instance.transform.position);
                    // Nếu sứa vẫn ở trong cùng 1 ô gạch thì bỏ qua việc cập nhật lưới Lux (Giải pháp A)
                    if (currentCell == __instance.cachedCell)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    // ==========================================
    // 2. TỐI ƯU HÓA LIGHT SYMBOL TRACKER
    // ==========================================
    [HarmonyPatch(typeof(Light2D), "OnSpawn")]
    public static class Light2D_OnSpawn_Patch
    {
        public static void Postfix(Light2D __instance)
        {
            if (Config.Instance != null && Config.Instance.OptimizeLightSymbolTracker)
            {
                var go = __instance.gameObject;
                if (go != null && go.GetComponent<LightSymbolTracker>() != null)
                {
                    go.AddOrGet<SlowLightSymbolTracker>();
                }
            }

            // Tối ưu sứa phát sáng - Chế độ tắt Lux hoàn toàn
            if (Config.Instance != null && Config.Instance.LightBugOptimMode == Config.LightBugMode.DisableLux)
            {
                if (__instance.gameObject != null && __instance.gameObject.name.Contains("LightBug"))
                {
                    __instance.enabled = false;
                    SgtLogger.l("Tối ưu LightBug: Đã tắt nguồn sáng của " + __instance.gameObject.name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(LightSymbolTracker), "RenderEveryTick")]
    public static class LightSymbolTracker_RenderEveryTick_Patch
    {
        public static bool Prefix()
        {
            // Trả về false để bỏ qua hàm chạy every frame mặc định của game
            return Config.Instance == null || !Config.Instance.OptimizeLightSymbolTracker;
        }
    }

    [SkipSaveFileSerialization]
    public class SlowLightSymbolTracker : KMonoBehaviour, IRender200ms
    {
        [MyCmpReq]
        private LightSymbolTracker tracker;

        private static System.Reflection.MethodInfo _renderMethod;

        private static void InitRenderMethod()
        {
            if (_renderMethod == null)
            {
                _renderMethod = typeof(LightSymbolTracker).GetMethod("RenderEveryTick", 
                    System.Reflection.BindingFlags.Public | 
                    System.Reflection.BindingFlags.NonPublic | 
                    System.Reflection.BindingFlags.Instance);
            }
        }

        public void Render200ms(float dt)
        {
            if (tracker != null)
            {
                InitRenderMethod();
                if (_renderMethod != null)
                {
                    try
                    {
                        _renderMethod.Invoke(tracker, new object[] { dt });
                    }
                    catch (Exception)
                     {
                         // Bỏ qua lỗi
                     }
                }
            }
        }
    }

    // ==========================================
    // 3. TỐI ƯU HÓA PLACER EASING (KHI KÉO XÂY DỰNG)
    // ==========================================
    [HarmonyPatch(typeof(CancellableDig), "OnCancel")]
    public static class CancellableDig_OnCancel_Patch
    {
        public static bool Prefix(CancellableDig __instance)
        {
            if (Config.Instance != null && Config.Instance.DisablePlacerEasing)
            {
                if (__instance != null)
                {
                    __instance.DeleteObject();
                }
                return false; // Xóa placer ngay lập tức và chặn easing animation
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(EasingAnimations), "Initialize")]
    public static class EasingAnimations_Initialize_Patch
    {
        public static bool Prefix()
        {
            return Config.Instance == null || !Config.Instance.DisablePlacerEasing;
        }
    }

    [HarmonyPatch(typeof(EasingAnimations), "PlayAnimation")]
    public static class EasingAnimations_PlayAnimation_Patch
    {
        public static bool Prefix()
        {
            return Config.Instance == null || !Config.Instance.DisablePlacerEasing;
        }
    }

    // ==========================================
    // 4. TỐI ƯU HÓA HIỆU ỨNG HẠT (SPAWN FX) NGOÀI VIEWPORT
    // ==========================================
    [HarmonyPatch(typeof(Game), "SpawnFX", new Type[] { typeof(SpawnFXHashes), typeof(Vector3), typeof(float) })]
    public static class Game_SpawnFX_Patch
    {
        public static bool Prefix(SpawnFXHashes fx, Vector3 position)
        {
            if (Config.Instance == null || !Config.Instance.OptimizeSublimates) return true;

            int cell = Grid.PosToCell(position);
            if (Grid.IsValidCell(cell))
            {
                // Chỉ vẽ FX ở hành tinh hiện tại người chơi đang xem
                byte worldIdx = Grid.WorldIdx[cell];
                if (worldIdx != ClusterManager.INVALID_WORLD_IDX && ClusterManager.Instance != null && ClusterManager.Instance.activeWorldId == worldIdx)
                {
                    // Lấy giới hạn viewport camera đang hiển thị trên màn hình
                    Grid.GetVisibleExtents(out int minX, out int minY, out int maxX, out int maxY);
                    Grid.CellToXY(cell, out int x, out int y);
                    
                    if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                    {
                        return true; // Trong tầm nhìn camera -> cho phép vẽ
                    }
                }
                return false; // Ngoài tầm nhìn -> bỏ qua để tăng FPS
            }
            return true;
        }
    }
}
