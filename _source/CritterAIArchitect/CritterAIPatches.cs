using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace CritterAIArchitect {
    [HarmonyPatch(typeof(Brain), "UpdateBrain")]
    public static class Brain_UpdateBrain_Patch {
        // Cache theo frame để tối ưu hóa hiệu năng cực hạn
        private static int lastCameraFrame = -1;
        private static Camera cachedCamera = null;

        private static int lastWorldFrame = -1;
        private static int cachedActiveWorldId = 0;

        /// <summary>
        /// Lấy Camera chính và cache lại theo từng frame để tránh interop calls lặp lại.
        /// </summary>
        private static Camera GetMainCamera() {
            int currentFrame = Time.frameCount;
            if (currentFrame != lastCameraFrame || cachedCamera == null) {
                cachedCamera = Camera.main;
                lastCameraFrame = currentFrame;
            }
            return cachedCamera;
        }

        /// <summary>
        /// Lấy Active World ID hiện tại mà người chơi đang xem và cache theo frame.
        /// </summary>
        private static int GetActiveWorldId() {
            int currentFrame = Time.frameCount;
            if (currentFrame != lastWorldFrame) {
                cachedActiveWorldId = ClusterManager.Instance != null ? ClusterManager.Instance.activeWorldId : 0;
                lastWorldFrame = currentFrame;
            }
            return cachedActiveWorldId;
        }

        /// <summary>
        /// Kiểm tra xem một vị trí trong không gian game có nằm trong tầm nhìn của Camera chính hay không.
        /// </summary>
        private static bool IsPositionVisible(Vector3 position) {
            Camera camera = GetMainCamera();
            if (camera == null) return false;

            // Chuyển đổi tọa độ thế giới sang tọa độ viewport của camera (0.0 đến 1.0)
            Vector3 viewportPos = camera.WorldToViewportPoint(position);
            return viewportPos.x >= 0f && viewportPos.x <= 1f &&
                   viewportPos.y >= 0f && viewportPos.y <= 1f &&
                   viewportPos.z > 0f;
        }

               [HarmonyPrefix]
        public static bool Prefix(Brain __instance) {
            if (__instance == null) return true;

            try {
                // Kiểm tra và bỏ qua Duplicants (MinionBrain) và các thực thể có tag DupeBrain (Rovers)
                var kprefabId = __instance.GetComponent<KPrefabID>();
                if (kprefabId == null || kprefabId.HasTag(GameTags.DupeBrain) || __instance is MinionBrain) {
                    return true;
                }

                int instanceId = __instance.gameObject.GetInstanceID();
                int currentFrame = Time.frameCount;

                // 1. Xác định phân vùng thế giới (Asteroid)
                int myWorldId = __instance.GetMyWorldId();
                int activeWorldId = GetActiveWorldId();

                int updateInterval = 30; // Mặc định LOD 1: Cùng thế giới nhưng ngoài tầm nhìn camera (cập nhật mỗi 500ms)

                if (myWorldId != activeWorldId) {
                    // LOD 2: Ở thế giới khác, cập nhật rất chậm (mỗi 2.5 giây)
                    updateInterval = 150;
                } else {
                    // LOD 0 hoặc LOD 1: Cùng thế giới hoạt động
                    Vector3 pos = __instance.transform.position;
                    if (IsPositionVisible(pos)) {
                        // LOD 0: Trong tầm nhìn camera, cập nhật nhanh (mỗi 80ms)
                        updateInterval = 5;
                    }
                }

                // 2. AI Time-Slicing: Lập lịch lệch pha dựa trên InstanceID để dàn trải CPU đều các frame
                int phase = Mathf.Abs(instanceId) % updateInterval;
                if (currentFrame % updateInterval == phase) {
                    return true; // Đến lượt chạy AI thực tế
                }

                return false; // Chặn việc thực thi AI ở frame này để tiết kiệm tài nguyên CPU!
            }
            catch (System.Exception e) {
                Debug.LogWarning("[CritterAIArchitect] Lỗi xử lý LOD AI, phục hồi an toàn: " + e.Message);
                return true; // Phục hồi bằng cách cho phép chạy logic gốc của game
            }
        }
    }
}
