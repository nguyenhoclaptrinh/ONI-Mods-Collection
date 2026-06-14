using HarmonyLib;
using System;
using UnityEngine;
using UtilLibs;

namespace GraphicsOptimizer.Patches
{
    [HarmonyPatch(typeof(CameraController), "OnSpawn")]
    public static class CameraController_OnSpawn_Patch
    {
        public static void Postfix(CameraController __instance)
        {
            try
            {
                // Khởi tạo và cập nhật độ sáng hiển thị
                BrightnessOverlay.Instance.ApplySettings();
                SgtLogger.l("Khởi tạo BrightnessOverlay thành công trên CameraController.");
            }
            catch (Exception e)
            {
                SgtLogger.l("Lỗi khi khởi tạo BrightnessOverlay: " + e.Message);
            }

            // Tắt Post-Processing nếu được cấu hình
            if (Config.Instance.DisablePostProcessing)
            {
                try
                {
                    var camera = __instance.GetComponent<Camera>();
                    if (camera != null)
                    {
                        // Dùng Reflection để tránh dependency cứng vào PostProcessing stack
                        var ppLayer = camera.GetComponent("PostProcessLayer");
                        if (ppLayer != null)
                        {
                            var enabledProp = ppLayer.GetType().GetProperty("enabled");
                            if (enabledProp != null)
                            {
                                enabledProp.SetValue(ppLayer, false);
                                SgtLogger.l("Đã tắt PostProcessLayer để tối ưu hóa GPU.");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    SgtLogger.l("Lỗi khi tắt PostProcessLayer: " + e.Message);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public static class Game_OnSpawn_Patch
    {
        public static void Postfix()
        {
            try
            {
                // Áp dụng lại cài đặt khi game spawn hoàn tất
                BrightnessOverlay.Instance.ApplySettings();
            }
            catch (Exception e)
            {
                SgtLogger.l("Lỗi khi áp dụng cài đặt độ sáng lúc Game OnSpawn: " + e.Message);
            }
        }
    }
}
