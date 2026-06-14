using System;
using UnityEngine;
using UnityEngine.UI;

namespace GraphicsOptimizer
{
    public class BrightnessOverlay : MonoBehaviour
    {
        private static BrightnessOverlay _instance;
        public static BrightnessOverlay Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("GraphicsOptimizerOverlay");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<BrightnessOverlay>();
                }
                return _instance;
            }
        }

        private Canvas canvas;
        private Image panelImage;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Khởi tạo Canvas
            canvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<CanvasScaler>();

            // Tạo Panel làm mờ màn hình
            GameObject panel = new GameObject("DimPanel");
            panel.transform.SetParent(transform, false);
            panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0f, 0f, 0f, 0f);
            panelImage.raycastTarget = false; // Click xuyên qua panel này

            // Phủ toàn màn hình
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            ApplySettings();
        }

        public void ApplySettings()
        {
            var config = Config.Instance;
            if (config == null) return;

            int brightness = config.BrightnessFactor;
            float opacity = 1f - (brightness / 100f);

            if (panelImage != null)
            {
                panelImage.color = new Color(0f, 0f, 0f, opacity);
            }

            if (canvas != null)
            {
                if (config.DimOnlyWorld)
                {
                    Camera worldCam = CameraController.Instance?.GetComponent<Camera>() ?? Camera.main;
                    if (worldCam != null)
                    {
                        canvas.renderMode = RenderMode.ScreenSpaceCamera;
                        canvas.worldCamera = worldCam;
                        canvas.planeDistance = worldCam.nearClipPlane + 0.1f; // Trước camera thế giới game
                        canvas.sortingOrder = 9999;
                    }
                    else
                    {
                        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.sortingOrder = 99999;
                    }
                }
                else
                {
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.sortingOrder = 99999; // Đè lên cả UI
                }
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                bool changed = false;
                int currentBrightness = Config.Instance.BrightnessFactor;

                if (Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals))
                {
                    currentBrightness = Math.Min(100, currentBrightness + 5);
                    changed = true;
                }
                else if (Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
                {
                    currentBrightness = Math.Max(10, currentBrightness - 5);
                    changed = true;
                }

                if (changed)
                {
                    Config.Instance.BrightnessFactor = currentBrightness;
                    ApplySettings();
                    PeterHan.PLib.Options.POptions.WriteSettings(Config.Instance);
                    
                    // Hiển thị thông báo góc màn hình nếu game đang chạy
                    if (PopFXManager.Instance != null && CameraController.Instance != null)
                    {
                        PopFXManager.Instance.SpawnFX(
                            PopFXManager.Instance.sprite_Plus, 
                            "Độ sáng: " + currentBrightness + "%", 
                            null, 
                            Vector3.zero, 
                            1.5f, 
                            false, 
                            true
                        );
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
