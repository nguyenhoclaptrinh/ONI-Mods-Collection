---
id: saveloader-reflection
type: research-output
status: done
created: 2026-06-24
source: Reflector tool
---
# Phản chiếu kiểu SaveLoader

## Mục đích
Quét cấu trúc kiểu SaveLoader từ Assembly-CSharp.dll để hỗ trợ phát triển mod.

## Kết quả
```csharp
namespace Game
{
    public class SaveLoader
    {
        // ==========================================
        // --- FIELDS ---
        // ==========================================
        private GridSettings gridSettings;
        private bool <loadedFromSave>k__BackingField;
        private bool saveFileCorrupt;
        private bool compressSaveData;
        private int lastUncompressedSize;
        public bool saveAsText;
        public SaveManager saveManager;
        private Action<Cluster> <OnWorldGenComplete>k__BackingField;
        private Cluster m_cluster;
        private ClusterLayout m_clusterLayout;
        private GameInfo <GameInfo>k__BackingField;
        private bool mustRestartOnFail;
        private GameSpawnData <cachedGSD>k__BackingField;
        private WorldDetailSave <clusterDetailSave>k__BackingField;
        private static SaveLoader <Instance>k__BackingField;
        private static bool force_infinity;
        public static string MAINMENU_LEVELNAME;
        public static string FRONTEND_LEVELNAME;
        public static string BACKEND_LEVELNAME;
        public static string SAVE_EXTENSION;
        public static string AUTOSAVE_FOLDER;
        public static string CLOUDSAVE_FOLDER;
        public static string SAVE_FOLDER;
        public static int MAX_AUTOSAVE_FILES;
        private static string CorruptFileSuffix;
        private static float SAVE_BUFFER_HEAD_ROOM;
        public static string METRIC_SAVED_PREFAB_KEY;
        public static string METRIC_IS_AUTO_SAVE_KEY;
        public static string METRIC_WAS_DEBUG_EVER_USED;
        public static string METRIC_IS_SANDBOX_ENABLED;
        public static string METRIC_RESOURCES_ACCESSIBLE_KEY;
        public static string METRIC_DAILY_REPORT_KEY;
        public static string METRIC_WORLD_METRICS_KEY;
        public static string METRIC_MINION_METRICS_KEY;
        public static string METRIC_CUSTOM_GAME_SETTINGS;
        public static string METRIC_CUSTOM_MIXING_SETTINGS;
        public static string METRIC_PERFORMANCE_MEASUREMENTS;
        public static string METRIC_FRAME_TIME;

        // ==========================================
        // --- PROPERTIES ---
        // ==========================================
        public bool loadedFromSave { get; set; }
        public static SaveLoader Instance { get; set; }
        public Action<Cluster> OnWorldGenComplete { get; set; }
        public Cluster Cluster { get; }
        public ClusterLayout ClusterLayout { get; }
        public GameInfo GameInfo { get; set; }
        public GameSpawnData cachedGSD { get; set; }
        public WorldDetailSave clusterDetailSave { get; set; }

        // ==========================================
        // --- METHODS ---
        // ==========================================
        public static void DestroyInstance();
        protected virtual void OnPrefabInit();
        private void MoveCorruptFile(string filename);
        protected virtual void OnSpawn();
        private static void CompressContents(BinaryWriter fileWriter, Byte[] uncompressed, int length);
        private Byte[] FloatToBytes(Single[] floats);
        private static Byte[] DecompressContents(Byte[] compressed);
        private Single[] BytesToFloat(Byte[] bytes);
        private SaveFileRoot PrepSaveFile();
        private void Save(BinaryWriter writer);
        private bool Load(IReader reader);
        private void LogActiveMods();
        public static string GetSavePrefix();
        public static string GetCloudSavePrefix();
        public static string GetSavePrefixAndCreateFolder();
        public static string GetUserID();
        public static string GetNextUsableSavePath(string filename);
        public static string GetOriginalSaveFileName(string filename);
        public static bool IsSaveAuto(string filename);
        public static bool IsSaveLocal(string filename);
        public static bool IsSaveCloud(string filename);
        public static string GetAutoSavePrefix();
        public static void SetActiveSaveFilePath(string path);
        public static string GetActiveSaveFilePath();
        public static string GetActiveAutoSavePath();
        public static string GetAutosaveFilePath();
        public static string GetActiveSaveColonyFolder();
        public static string GetActiveSaveFolder();
        public static List<SaveFileEntry> GetSaveFiles(string save_dir, bool sort, SearchOption search);
        public static List<SaveFileEntry> GetAllFiles(bool sort, SaveType type);
        public static List<SaveFileEntry> GetAllColonyFiles(bool sort, SearchOption search);
        public static bool GetCloudSavesDefault();
        public static string GetCloudSavesDefaultPref();
        public static void SetCloudSavesDefault(bool value);
        public static void SetCloudSavesDefaultPref(string pref);
        public static bool GetCloudSavesAvailable();
        public static string GetLatestSaveForCurrentDLC();
        public void InitialSave();
        public string Save(string filename, bool isAutoSave, bool updateSavePointer);
        public static GameInfo LoadHeader(string filename, Header& header);
        public bool Load(string filename);
        public bool LoadFromWorldGen();
        public void SetWorldDetail(WorldDetailSave worldDetail);
        private void ReportSaveMetrics(bool is_auto_save);
        private List<MinionMetricsData> GetMinionMetrics();
        private List<SavedPrefabMetricsData> GetSavedPrefabMetrics();
        private List<WorldInventoryMetricsData> GetWorldInventoryMetrics();
        private List<DailyReportMetricsData> GetDailyReportMetrics();
        private List<PerformanceMeasurement> GetPerformanceMeasurements();
        public float GetFrameTime();
        private List<WorldMetricsData> GetWorldMetrics();
        public bool IsDLCActiveForCurrentSave(string dlcid);
        public bool IsDlcListActiveForCurrentSave(String[] dlcIds);
        public bool IsAllDlcActiveForCurrentSave(String[] dlcIds);
        public bool IsAnyDlcActiveForCurrentSave(String[] dlcIds);
        public bool IsCorrectDlcActiveForCurrentSave(String[] required, String[] forbidden);
        public string GetSaveLoadContentLetters();
        public void UpgradeActiveSaveDLCInfo(string dlcId, bool trigger_load);
        public static void LoadScene();
    }
}
```

## Kết luận & Áp dụng
<Dùng kiến thức này như thế nào trong implementation>
