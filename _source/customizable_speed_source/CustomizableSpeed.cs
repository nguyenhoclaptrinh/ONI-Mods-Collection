using HarmonyLib;
using Newtonsoft.Json;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using UnityEngine;

namespace CustomizableSpeed
{
    // Khởi tạo mod và đăng ký cấu hình PLib
    public class CustomizableSpeedMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            try
            {
                base.OnLoad(harmony);
                PUtil.InitLibrary();
                new POptions().RegisterOptions(this, typeof(SpeedOptions));
                Debug.Log("[CustomizableSpeed] Loaded and options registered successfully.");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[CustomizableSpeed] Lỗi OnLoad mod: " + e.Message);
            }
        }
    }

    // Lớp chứa tùy chọn cấu hình tốc độ của mod
    [ModInfo("https://github.com/Pholith/ONI-Mods", "preview.png")]
    [JsonObject(MemberSerialization.OptIn)]
    [RestartRequired]
    public class SpeedOptions : SingletonOptions<SpeedOptions>
    {
        [Option("Slow Speed (1)", "Speed multiplier for the first speed level (Default: 1x)")]
        [Limit(0.1f, 10f)]
        [JsonProperty]
        public float slowSpeed { get; set; }

        [Option("Normal Speed (2)", "Speed multiplier for the second speed level (Default: 2x)")]
        [Limit(0.5f, 20f)]
        [JsonProperty]
        public float normalSpeed { get; set; }

        [Option("Super Speed (3)", "Speed multiplier for the third speed level (Default: 3x)")]
        [Limit(1f, 30f)]
        [JsonProperty]
        public float superSpeed { get; set; }

        [Option("Enable Hotkeys", "Enables Ctrl + Plus/Minus to dynamically adjust speed of current level in-game")]
        [JsonProperty]
        public bool enableHotkeys { get; set; }

        public SpeedOptions()
        {
            slowSpeed = 1f;
            normalSpeed = 2f;
            superSpeed = 3f;
            enableHotkeys = true;
        }
    }

    // Tối ưu hóa: Patch một lần duy nhất khi khởi tạo SpeedControlScreen
    [HarmonyPatch(typeof(SpeedControlScreen), "OnPrefabInit")]
    public static class SpeedControlScreen_OnPrefabInit_Patch
    {
        public static void Postfix(SpeedControlScreen __instance)
        {
            try
            {
                if (__instance != null)
                {
                    var options = SpeedOptions.Instance;
                    if (options != null)
                    {
                        // Sử dụng Harmony Traverse để truy cập trường private speed factors
                        var speedFactors = Traverse.Create(__instance).Field<float[]>("speedFactors").Value;
                        if (speedFactors != null && speedFactors.Length > 3)
                        {
                            speedFactors[1] = options.slowSpeed;
                            speedFactors[2] = options.normalSpeed;
                            speedFactors[3] = options.superSpeed;
                        }
                        
                        // Thêm component lắng nghe phím nóng tăng giảm tốc độ nếu được bật
                        if (options.enableHotkeys)
                        {
                            __instance.gameObject.AddComponent<SpeedHotkeyHandler>();
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[CustomizableSpeed] Lỗi Postfix OnPrefabInit: " + e.Message);
            }
        }
    }

    // Component xử lý phím nóng tăng/giảm tốc độ và cập nhật UI động trong game
    public class SpeedHotkeyHandler : MonoBehaviour
    {
        private SpeedControlScreen screen;

        private void Start()
        {
            screen = GetComponent<SpeedControlScreen>();
        }

        private void Update()
        {
            if (screen == null) return;

            // Kiểm tra phím nóng Ctrl/Shift + Plus/Equals hoặc Ctrl/Shift + Minus
            bool ctrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                               Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if (ctrlPressed)
            {
                if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
                {
                    AdjustSpeed(true); // Tăng tốc
                }
                else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    AdjustSpeed(false); // Giảm tốc
                }
            }
        }

        private void AdjustSpeed(bool increase)
        {
            try
            {
                // Sử dụng Harmony Traverse để truy cập các trường private speed và speedFactors
                var speedTraverse = Traverse.Create(screen).Field<int>("speed");
                var speedFactorsTraverse = Traverse.Create(screen).Field<float[]>("speedFactors");

                int currentLevel = speedTraverse.Value;
                float[] speedFactors = speedFactorsTraverse.Value;

                if (currentLevel <= 0 || speedFactors == null || currentLevel >= speedFactors.Length) 
                    return; // Đang pause hoặc mốc không hợp lệ thì không chỉnh

                float currentFactor = speedFactors[currentLevel];
                float delta = 1f;

                // Nếu tốc độ nhỏ hơn 2x, cho phép chỉnh tăng/giảm 0.5x để mượt mà hơn
                if (currentFactor < 2f)
                {
                    delta = 0.5f;
                }

                if (increase)
                {
                    currentFactor = Mathf.Min(currentFactor + delta, 30f); // Tối đa 30x để đảm bảo an toàn CPU
                }
                else
                {
                    currentFactor = Mathf.Max(currentFactor - delta, 0.1f); // Tối thiểu 0.1x
                }

                // Gán lại hệ số tốc độ
                speedFactors[currentLevel] = currentFactor;

                // Nếu game đang chạy (không bị pause), cập nhật ngay Time.timeScale lập tức!
                if (!screen.IsPaused)
                {
                    Time.timeScale = currentFactor;
                }

                // Đồng bộ hiển thị chữ bay (PopFX) ngay tại vị trí nút tốc độ để thông báo cho người chơi
                if (PopFXManager.Instance != null)
                {
                    PopFXManager.Instance.SpawnFX(
                        PopFXManager.Instance.sprite_Plus, 
                        $"Speed x{currentFactor:0.0}", 
                        screen.transform,
                        Vector3.zero,
                        1.5f
                    );
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[CustomizableSpeed] Lỗi AdjustSpeed phím nóng: " + e.Message);
            }
        }
    }
}
