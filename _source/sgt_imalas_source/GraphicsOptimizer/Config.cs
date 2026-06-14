using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System;

namespace GraphicsOptimizer
{
    [Serializable]
    [RestartRequired]
    [ConfigFile(SharedConfigLocation: true)]
    public class Config : SingletonOptions<Config>
    {
        public enum LightBugMode
        {
            [Option("Không tối ưu")]
            None,
            [Option("Chỉ cập nhật khi đổi ô gạch (Khuyên dùng)")]
            CellChange,
            [Option("Tắt Lux hoàn toàn (Tối ưu tối đa)")]
            DisableLux
        }

        [Option("Độ sáng hiển thị (%)", "Điều chỉnh độ sáng màn hình game (10% - 100%).")]
        [Limit(10, 100)]
        [JsonProperty]
        public int BrightnessFactor { get; set; } = 100;

        [Option("Chỉ làm tối bản đồ game", "Làm tối thế giới game và giữ nguyên độ sáng của giao diện UI giúp dễ đọc chữ.")]
        [JsonProperty]
        public bool DimOnlyWorld { get; set; } = true;

        [Option("Tắt Post-Processing toàn cục", "Tắt hiệu ứng hậu kỳ (Bloom, Vignette) để giải phóng GPU tối đa cho máy yếu.")]
        [JsonProperty]
        public bool DisablePostProcessing { get; set; } = false;

        [Option("Tối ưu sứa phát sáng (Light Bug)", "Lựa chọn chế độ tối ưu hóa cho sứa phát sáng nhằm giảm lag CPU late-game.")]
        [JsonProperty]
        public LightBugMode LightBugOptimMode { get; set; } = LightBugMode.CellChange;

        [Option("Giảm cập nhật ánh sáng thực thể", "Chỉ cập nhật ánh sáng của thực thể mỗi 200ms thay vì mỗi khung hình.")]
        [JsonProperty]
        public bool OptimizeLightSymbolTracker { get; set; } = true;

        [Option("Tắt Placer Easing", "Tắt hiệu ứng trượt mượt của Placer khi kéo xây dựng nhiều block để tránh giật lag.")]
        [JsonProperty]
        public bool DisablePlacerEasing { get; set; } = true;

        [Option("Tối ưu hóa hiệu ứng bọt khí", "Bỏ qua tạo hiệu ứng bọt khí/khói của thực thể ngoài vùng hiển thị của camera.")]
        [JsonProperty]
        public bool OptimizeSublimates { get; set; } = true;
    }
}
