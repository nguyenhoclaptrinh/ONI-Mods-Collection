---
id: kcrashreporter-reflection
type: research-output
status: done
created: 2026-06-24
source: Reflector tool
---
# Phản chiếu kiểu KCrashReporter

## Mục đích
Quét cấu trúc kiểu KCrashReporter từ Assembly-CSharp.dll để hỗ trợ phát triển mod.

## Kết quả
```csharp
namespace Game
{
    public class KCrashReporter
    {
        // ==========================================
        // --- FIELDS ---
        // ==========================================
        private LoadScreen loadScreenPrefab;
        private GameObject reportErrorPrefab;
        private ConfirmDialogScreen confirmDialogPrefab;
        private GameObject errorScreen;
        public static string MOST_RECENT_SAVEFILE;
        private static Action<bool> onCrashReported;
        private static Action<float> onCrashUploadProgress;
        public static bool ignoreAll;
        public static bool debugWasUsed;
        public static bool haveActiveMods;
        public static uint logCount;
        public static string error_canvas_name;
        public static bool disableDeduping;
        private static bool <hasReportedError>k__BackingField;
        public static bool hasCrash;
        private static readonly Regex failedToLoadModuleRegEx;
        public static bool terminateOnError;
        private static string dataRoot;
        private static readonly String[] IgnoreStrings;
        private static HashSet<int> previouslyReportedDevNotifications;
        private static PendingReport pendingReport;
        private static PendingCrash pendingCrash;
        public static string CRASH_REPORTER_SERVER;
        public static uint MAX_LOGS;

        // ==========================================
        // --- PROPERTIES ---
        // ==========================================
        public static bool hasReportedError { get; set; }

        // ==========================================
        // --- METHODS ---
        // ==========================================
        public static void add_onCrashReported(Action<bool> value);
        public static void remove_onCrashReported(Action<bool> value);
        public static void add_onCrashUploadProgress(Action<float> value);
        public static void remove_onCrashUploadProgress(Action<float> value);
        private void OnEnable();
        private void OnDisable();
        private void HandleLog(string msg, string stack_trace, LogType type);
        public bool ShowDialog(string error, string stack_trace, bool includeSaveFile, String[] extraCategories, String[] extraFiles);
        private void OnCloseErrorDialog();
        private void OnQuitToDesktopCrashed();
        private static string GetUserID();
        private static string GetLogContents();
        public static void ReportDevNotification(string notification_name, string stack_trace, string details, bool includeSaveFile, String[] extraCategories);
        public static void ReportError(string msg, string stack_trace, ConfirmDialogScreen confirm_prefab, GameObject confirm_parent, string userMessage, bool includeSaveFile, String[] extraCategories, String[] extraFiles);
        private static IEnumerator SubmitCrashAsync(string jsonString, Byte[] archiveData, Action successCallback, Action<long> failureCallback);
        public static void ReportBug(string msg, GameObject confirmParent);
        public static void Assert(bool condition, string message, String[] extraCategories);
        public static void ReportSimDLLCrash(string msg, string stack_trace, string dmp_filename);
        private static Byte[] CreateArchiveZip(string log, List<string> files);
        private void Update();
    }
}
```

## Kết luận & Áp dụng
<Dùng kiến thức này như thế nào trong implementation>
