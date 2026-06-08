using HarmonyLib;
using KMod;

namespace AllDLCWorld
{
    public class AllDLCWorld : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    public class AllDLCWorldPatches
    {
        public static LocString WORLD_NAME = "Pangaea (All-DLC World)";
        public static LocString WORLD_DESC = "Một thế giới khổng lồ (kích thước gấp đôi cả về chiều rộng và chiều cao) hợp nhất tất cả các quần xã sinh vật (biomes), tài nguyên, sinh vật và thực vật từ tất cả các DLC (Spaced Out, Frosty Planet, và Prehistoric) vào một hành tinh duy nhất. Bạn sẽ không cần phải du hành vũ trụ để tìm kiếm các tài nguyên đặc thù của DLC nữa.";

        public static LocString CLUSTER_NAME = "Pangaea Cluster";
        public static LocString CLUSTER_DESC = "Hệ thống hành tinh Pangaea, nơi tất cả vật chất và sự sống của vũ trụ tập trung tại một hành tinh siêu lớn duy nhất. Thử thách tối thượng cho khả năng sinh tồn và tối ưu hóa tài nguyên.";

        public static void Prefix()
        {
            // Register world strings
            Strings.Add("STRINGS.WORLDS.ALLDLCWORLD.NAME", WORLD_NAME);
            Strings.Add("STRINGS.WORLDS.ALLDLCWORLD.DESCRIPTION", WORLD_DESC);

            // Register cluster strings
            Strings.Add("STRINGS.CLUSTER_NAMES.ALLDLCWORLD_CLUSTER.NAME", CLUSTER_NAME);
            Strings.Add("STRINGS.CLUSTER_NAMES.ALLDLCWORLD_CLUSTER.DESCRIPTION", CLUSTER_DESC);
        }
    }
}
