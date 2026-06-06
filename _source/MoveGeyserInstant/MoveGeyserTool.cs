using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MoveGeyserInstant {
    public sealed class MoveGeyserTool : InterfaceTool {
        private const int FootprintWidth = 4;
        // Geyser neutronium base in the game is typically 4x1 (width x height)
        private const int FootprintHeight = 1;
        private const float NeutroniumMass = 1840f;
        private const float NeutroniumTemperature = 293.15f;

        public static MoveGeyserTool Instance { get; private set; }

        private static readonly MethodInfo GetWorldIdMethod;
        private static readonly FieldInfo WorldIdxField;

        static MoveGeyserTool() {
            try {
                Type gridType = typeof(Grid);
                GetWorldIdMethod = gridType.GetMethod("GetWorldId", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? gridType.GetMethod("GetWorldIdx", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? gridType.GetMethod("get_worldIdx", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                if (GetWorldIdMethod == null) {
                    WorldIdxField = gridType.GetField("WorldIdx", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        ?? gridType.GetField("worldIdx", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                }
            }
            catch (Exception ex) {
                Debug.LogError("[MoveGeyserInstant] Failed to reflect Grid world ID methods: " + ex.Message);
            }
        }

        private GeyserSnapshot snapshot;
        private int lastCell = Grid.InvalidCell;
        private GameObject overlayObject;
        private Mesh overlayMesh;
        private Material overlayMaterial;
        private GameObject previewInstance;

        public override void OnPrefabInit() {
            var hover = gameObject.AddComponent<HoverTextConfiguration>();
            hover.ToolName = Strings.Get("STRINGS.MOVEGEYSERINSTANT.TOOL_NAME");
            hover.ActionName = Strings.Get("STRINGS.MOVEGEYSERINSTANT.TOOL_ACTION");

            base.OnPrefabInit();
            Instance = this;
        }

        public void BeginMove(GameObject source) {
            if (source == null)
                return;

            snapshot = GeyserSnapshot.Capture(source);
            if (snapshot == null) {
                Debug.LogWarning("[MoveGeyserInstant] Failed to capture geyser snapshot.");
                return;
            }

            lastCell = Grid.PosToCell(source);
            EnsureOverlay();
            UpdateOverlay(lastCell);
            EnsurePreviewForSnapshot();
            PlayerController.Instance.ActivateTool(this);
        }

        public override void OnMouseMove(Vector3 cursor_pos) {
            int cell = Grid.PosToCell(cursor_pos);
            lastCell = cell;
            UpdateOverlay(cell);
            UpdatePreviewPosition(cell);
        }

        public override void OnLeftClickDown(Vector3 cursor_pos) {
            int cell = Grid.PosToCell(cursor_pos);
            if (!Grid.IsValidCell(cell))
                cell = lastCell;
            // Check modifier keys: Shift = force exact, Ctrl = conservative (no stacking)
            bool shift = UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftShift) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightShift);
            bool ctrl = UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftControl) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightControl);
            PlaceAt(cell, shift, ctrl);
        }

        public override void OnDeactivateTool(InterfaceTool newTool) {
            HideOverlay();
            HidePreview();
            base.OnDeactivateTool(newTool);
            if (newTool != this)
                snapshot = null;
        }

        public override void OnKeyDown(KButtonEvent e) {
            if (e.TryConsume(global::Action.Escape)) {
                CancelMove();
                ActivateDefaultTool();
                return;
            }
        }

        public override void OnRightClickDown(Vector3 cursor_pos, KButtonEvent e) {
            if (snapshot == null) {
                base.OnRightClickDown(cursor_pos, e);
                return;
            }
        }

        private void PlaceAt(int targetCell, bool forceExact = false, bool conservative = false) {
            if (snapshot == null || !Grid.IsValidCell(targetCell))
                return;

            try {
                var prefab = Assets.GetPrefab(snapshot.PrefabTag);
                if (prefab == null) {
                    Debug.LogWarning("[MoveGeyserInstant] Cannot find geyser prefab: " + snapshot.PrefabTag);
                    return;
                }

                // apply modifiers
                bool originalAllowStacking = Config.AllowStacking;
                if (conservative)
                    Config.AllowStacking = false;

                if (!TryValidatePlacement(targetCell, out string validationError)) {
                    Debug.LogWarning("[MoveGeyserInstant] Cannot move geyser: " + validationError);
                    Config.AllowStacking = originalAllowStacking;
                    return;
                }

                Config.AllowStacking = originalAllowStacking;

                HideOverlay();

                // DELETE SOURCE FIRST so singleton buildings (Telepad etc.) unregister
                //    their grid cells before the new instance spawns, preventing
                //    PrimaryElement.OnSpawn NPE caused by concurrent occupancy.
                var sourceRef = snapshot.Source;
                if (sourceRef != null)
                    sourceRef.DeleteObject();

                // SPAWN NEW after source has been cleared
                Vector3 position = Grid.CellToPosCBC(targetCell, Grid.SceneLayer.Building);
                GameObject moved = Util.KInstantiate(prefab, position, Quaternion.identity);
                if (moved == null) {
                    Debug.LogWarning("[MoveGeyserInstant] Failed to instantiate geyser prefab.");
                    ClearAndDeactivate();
                    return;
                }

                // Copy configuration state before activating the prefab
                // so Geyser.OnSpawn() sees the captured configurator type
                // instead of generating/applying a fresh default first.
                snapshot.ApplyFieldsTo(moved);
                moved.SetActive(true);
                snapshot.TriggerCopySettings(moved);

                int targetWorldId = GetCellWorldId(targetCell);
                // If stacking (another structure exists at target cell), notify player
                bool stacking = false;
                foreach (var geyser in Components.Geysers.GetItems(targetWorldId)) {
                    if (geyser == null)
                        continue;
                    if (Grid.PosToCell(geyser) == targetCell) {
                        stacking = true;
                        break;
                    }
                }
                if (stacking)
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, "Stacked structure", moved.transform, Vector3.zero, 1.5f, false, false);

                if (snapshot.IsGeyser) {
                    int[] targetNeutronium = snapshot.GetTranslatedNeutroniumFootprint(targetCell);
                    PlaceDestinationNeutronium(targetNeutronium, targetWorldId);
                    ClearOldNeutroniumIfUnused();
                }

                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Strings.Get("STRINGS.MOVEGEYSERINSTANT.MOVE_BUTTON"), moved.transform, Vector3.zero, 1.5f, false, false);
                ClearAndDeactivate();
            }
            catch (Exception e) {
                Debug.LogWarning("[MoveGeyserInstant] Failed to move geyser: " + e);
                ClearAndDeactivate();
            }
        }

        private void EnsurePreviewForSnapshot() {
            if (snapshot == null)
                return;
            if (previewInstance != null)
                return;

            // Create a clean dummy GameObject for rendering only
            previewInstance = new GameObject("MoveGeyserInstant Preview");
            previewInstance.SetActive(false);
            if (Game.Instance != null)
                previewInstance.transform.SetParent(Game.Instance.transform, false);

            var kbac = previewInstance.AddComponent<KBatchedAnimController>();
            if (kbac != null && snapshot.PreviewAnimFiles != null) {
                kbac.AnimFiles = snapshot.PreviewAnimFiles;
                kbac.initialAnim = snapshot.GetPreviewAnim().ToString();
                kbac.defaultAnim = "idle";
                kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
                previewInstance.AddComponent<KSelectable>();
            }
        }

        private void UpdatePreviewPosition(int cell) {
            if (previewInstance == null || !Grid.IsValidCell(cell))
                return;

            Vector3 p = Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
            previewInstance.transform.position = p;

            // Dynamically tint the preview based on placement validity
            var kbac = previewInstance.GetComponent<KBatchedAnimController>();
            if (kbac != null) {
                bool valid = TryValidatePlacement(cell, out _);
                // Semi-transparent White (original color faded) if valid, Semi-transparent Red if invalid
                kbac.TintColour = valid ? new Color(1.0f, 1.0f, 1.0f, 0.45f) : new Color(1.0f, 0.1f, 0.1f, 0.45f);

                // Ensure anim plays in loop
                if (kbac.GetCurrentAnim() == null) {
                    kbac.Play(snapshot.GetPreviewAnim(), KAnim.PlayMode.Loop);
                }
            }

            if (!previewInstance.activeSelf)
                previewInstance.SetActive(true);
        }

        private void HidePreview() {
            if (previewInstance != null) {
                UnityEngine.Object.Destroy(previewInstance);
                previewInstance = null;
            }
        }

        private static void ActivateDefaultTool() {
            var controller = PlayerController.Instance;
            if (controller == null)
                return;

            InterfaceTool nextTool = SelectTool.Instance;
            if (nextTool == null && controller.tools != null) {
                for (int i = 0; i < controller.tools.Length; i++) {
                    var tool = controller.tools[i];
                    if (tool != null && !(tool is MoveGeyserTool)) {
                        nextTool = tool;
                        break;
                    }
                }
            }

            if (nextTool != null)
                controller.ActivateTool(nextTool);
        }

        private void PlaceDestinationNeutronium(int[] cells, int targetWorldId) {
            var skipped = new List<int>();
            foreach (int cell in cells) {
                if (!Grid.IsValidCell(cell)) {
                    skipped.Add(cell);
                    continue;
                }
                if (GetCellWorldId(cell) != targetWorldId) {
                    skipped.Add(cell);
                    continue;
                }
                if (IsCellBlockedForNeutronium(cell)) {
                    skipped.Add(cell);
                    continue;
                }

                SimMessages.ReplaceElement(cell, SimHashes.Unobtanium, CellEventLogger.Instance.DebugTool, NeutroniumMass, NeutroniumTemperature);
            }

            // Provide feedback for skipped cells so the player understands why some neutronium
            // were not placed. Show a PopFX at each skipped cell and log a short message.
            if (skipped.Count > 0) {
                foreach (int sc in skipped) {
                    try {
                        Vector3 pos = Grid.CellToPosCBC(sc, Grid.SceneLayer.Move);
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, "Neutronium skipped", null, pos);
                    }
                    catch {
                    }
                }
                Debug.LogFormat("[MoveGeyserInstant] Skipped placing neutronium on {0} cells.", skipped.Count);
            }
        }

        private void ClearOldNeutroniumIfUnused() {
            if (snapshot == null)
                return;

            foreach (int cell in snapshot.SourceFootprint) {
                if (!Grid.IsValidCell(cell))
                    continue;

                // Only operate on cells in the source world
                if (GetCellWorldId(cell) != snapshot.SourceWorldId)
                    continue;

                if (Grid.Element[cell].id != SimHashes.Unobtanium || IsUsedByAnotherGeyserFootprint(cell))
                    continue;

                // Re-check immediately before replacing to reduce race window
                if (Grid.Element[cell].id != SimHashes.Unobtanium)
                    continue;

                SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f);
            }
        }

        private bool IsUsedByAnotherGeyserFootprint(int neutroniumCell) {
            foreach (var geyser in Components.Geysers.GetItems(snapshot.SourceWorldId)) {
                if (geyser == null || geyser.gameObject == snapshot.Source)
                    continue;

                int geyserCell = Grid.PosToCell(geyser);
                int[] footprint = snapshot.GetTranslatedFootprint(geyserCell);
                for (int i = 0; i < footprint.Length; i++) {
                    if (footprint[i] == neutroniumCell)
                        return true;
                }
            }
            return false;
        }

        private static bool IsCellBlockedForNeutronium(int cell) {
            if (!Grid.IsValidCell(cell))
                return true;
            if (Grid.Element[cell].id == SimHashes.Unobtanium)
                return true;
            if (Grid.Solid[cell])
                return true;
            return Grid.Objects[cell, (int)ObjectLayer.Building] != null ||
                Grid.Objects[cell, (int)ObjectLayer.FoundationTile] != null ||
                Grid.Objects[cell, (int)ObjectLayer.Backwall] != null;
        }

        private void CancelMove() {
            snapshot = null;
            HideOverlay();
        }

        private void ClearAndDeactivate() {
            snapshot = null;
            HidePreview();
            HideOverlay();
            ActivateDefaultTool();
        }

        private void EnsureOverlay() {
            if (overlayObject != null)
                return;

            overlayMesh = new Mesh { name = "MoveGeyserInstant Overlay Mesh" };
            overlayMesh.MarkDynamic();
            overlayMaterial = new Material(Shader.Find("Klei/Prioritizable")) {
                name = "MoveGeyserInstant Overlay Material",
                color = Color.green
            };
            overlayObject = new GameObject("MoveGeyserInstant Overlay", typeof(MeshFilter), typeof(MeshRenderer));
            overlayObject.layer = LayerMask.NameToLayer("UI");
            overlayObject.GetComponent<MeshFilter>().sharedMesh = overlayMesh;
            overlayObject.GetComponent<MeshRenderer>().sharedMaterial = overlayMaterial;
            if (Game.Instance != null)
                overlayObject.transform.SetParent(Game.Instance.transform, false);
            overlayObject.SetActive(false);
        }

        private bool TryValidatePlacement(int targetCell, out string reason) {
            reason = null;
            if (snapshot == null) {
                reason = "missing snapshot";
                return false;
            }

            if (!Grid.IsValidCell(targetCell)) {
                reason = "target cell is invalid";
                return false;
            }

            int targetWorldId = GetCellWorldId(targetCell);
            if (targetWorldId < 0) {
                reason = "target world is unavailable";
                return false;
            }

            if (snapshot.SourceWorldId < 0) {
                reason = "source world is unavailable";
                return false;
            }

            int[] targetFootprint = snapshot.GetTranslatedFootprint(targetCell);
            if (targetFootprint == null || targetFootprint.Length == 0) {
                reason = "target footprint is incomplete";
                return false;
            }

            for (int i = 0; i < targetFootprint.Length; i++) {
                int footprintCell = targetFootprint[i];
                if (!Grid.IsValidCell(footprintCell)) {
                    reason = "target footprint leaves the world bounds";
                    return false;
                }

                int footprintWorldId = GetCellWorldId(footprintCell);
                if (footprintWorldId != targetWorldId) {
                    reason = "target footprint crosses asteroids";
                    return false;
                }
            }

            // Respect config: allow or disallow stacking
            if (!Config.AllowStacking && OverlapsAnotherGeyser(targetFootprint, targetWorldId)) {
                reason = "target footprint overlaps another geyser";
                return false;
            }

            return true;
        }

        private bool OverlapsAnotherGeyser(int[] footprint, int targetWorldId) {
            if (footprint == null || footprint.Length == 0)
                return false;

            foreach (var geyser in Components.Geysers.GetItems(targetWorldId)) {
                if (geyser == null || geyser.gameObject == snapshot.Source)
                    continue;

                int geyserCell = Grid.PosToCell(geyser);
                int[] otherFootprint = snapshot.GetTranslatedFootprint(geyserCell);
                for (int i = 0; i < footprint.Length; i++) {
                    int cell = footprint[i];
                    for (int j = 0; j < otherFootprint.Length; j++) {
                        if (cell == otherFootprint[j])
                            return true;
                    }
                }
            }

            return false;
        }

        private static int GetCellWorldId(int cell) {
            if (!Grid.IsValidCell(cell))
                return -1;

            try {
                if (GetWorldIdMethod != null) {
                    object value = GetWorldIdMethod.Invoke(null, new object[] { cell });
                    if (value != null)
                        return Convert.ToInt32(value);
                }
                else if (WorldIdxField != null) {
                    if (WorldIdxField.GetValue(null) is Array worldIdx && cell >= 0 && cell < worldIdx.Length) {
                        return Convert.ToInt32(worldIdx.GetValue(cell));
                    }
                }
            }
            catch {
            }

            return -1;
        }

        private void UpdateOverlay(int cell) {
            if (snapshot == null || !Grid.IsValidCell(cell)) {
                HideOverlay();
                return;
            }

            EnsureOverlay();
            bool valid = Grid.IsValidCell(cell);
            Color color = valid ? new Color(0.2f, 1f, 0.25f, 0.45f) : new Color(1f, 0.1f, 0.1f, 0.45f);
            var vertices = new List<Vector3>(16);
            var uvs = new List<Vector2>(16);
            var colors = new List<Color>(16);
            var triangles = new List<int>(24);

            AddCellQuad(cell, color, vertices, uvs, colors, triangles, 0f);
            int[] footprint = snapshot.GetTranslatedFootprint(cell);
            for (int i = 0; i < footprint.Length; i++)
                AddCellQuad(footprint[i], new Color(0.1f, 0.45f, 1f, 0.35f), vertices, uvs, colors, triangles, -0.01f);

            overlayMesh.Clear();
            overlayMesh.SetVertices(vertices);
            overlayMesh.SetUVs(0, uvs);
            overlayMesh.SetColors(colors);
            overlayMesh.SetTriangles(triangles, 0);
            overlayObject.SetActive(true);
        }

        private static void AddCellQuad(int cell, Color color, List<Vector3> vertices, List<Vector2> uvs, List<Color> colors, List<int> triangles, float zOffset) {
            if (!Grid.IsValidCell(cell))
                return;

            Vector3 center = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
            center.z = -5f + zOffset;
            float half = 0.5f;
            int start = vertices.Count;
            vertices.Add(new Vector3(center.x - half, center.y - half, center.z));
            vertices.Add(new Vector3(center.x - half, center.y + half, center.z));
            vertices.Add(new Vector3(center.x + half, center.y + half, center.z));
            vertices.Add(new Vector3(center.x + half, center.y - half, center.z));
            uvs.Add(new Vector2(0f, 0f));
            uvs.Add(new Vector2(0f, 1f));
            uvs.Add(new Vector2(1f, 1f));
            uvs.Add(new Vector2(1f, 0f));
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
            triangles.Add(start);
            triangles.Add(start + 1);
            triangles.Add(start + 2);
            triangles.Add(start);
            triangles.Add(start + 2);
            triangles.Add(start + 3);
        }

        private void HideOverlay() {
            if (overlayObject != null)
                overlayObject.SetActive(false);
        }

        private new void OnDestroy() {
            if (overlayObject != null)
                UnityEngine.Object.Destroy(overlayObject);
            if (overlayMesh != null)
                UnityEngine.Object.Destroy(overlayMesh);
            if (overlayMaterial != null)
                UnityEngine.Object.Destroy(overlayMaterial);

            overlayObject = null;
            overlayMesh = null;
            overlayMaterial = null;
        }

        private sealed class GeyserSnapshot {
            public GameObject Source { get; private set; }
            public int SourceCell { get; private set; }
            public int SourceWorldId { get; private set; }
            public Tag PrefabTag { get; private set; }
            public int[] SourceFootprint { get; private set; }
            public KAnimFile[] PreviewAnimFiles { get; private set; }
            public bool IsGeyser { get; private set; }
            public CellOffset[] OccupiedOffsets { get; private set; }
            public bool KeepAnalysis { get; private set; } = true;
            public bool KeepEruptionState { get; private set; } = true;
            public bool ResetVacillator { get; private set; } = false;
            public bool IsStudied { get; private set; }
            public bool IsGeneShufflerUsed { get; private set; }
            private HashedString previewAnim;
            private string previewInitialAnim;
            private string previewDefaultAnim;
            private Dictionary<string, object> geyserFields;
            private Dictionary<string, object> configuratorFields;

            public static GeyserSnapshot Capture(GameObject source) {
                var snapshot = new GeyserSnapshot {
                    Source = source,
                    SourceCell = Grid.PosToCell(source),
                    SourceWorldId = source.GetMyWorldId(),
                    PrefabTag = source.PrefabID(),
                    IsGeyser = source.GetComponent<Geyser>() != null
                };

                var movable = source.GetComponent<MovableGeyser>();
                if (movable != null) {
                    snapshot.KeepAnalysis = movable.keepAnalysis;
                    snapshot.KeepEruptionState = movable.keepEruptionState;
                    snapshot.ResetVacillator = movable.resetVacillator;
                }

                var studyable = source.GetComponent<Studyable>();
                if (studyable != null) {
                    snapshot.IsStudied = studyable.studied;
                }

                var shuffler = source.GetComponent<GeneShuffler>();
                if (shuffler != null) {
                    snapshot.IsGeneShufflerUsed = shuffler.IsConsumed;
                }

                var occupyArea = source.GetComponent<OccupyArea>();
                if (occupyArea != null) {
                    snapshot.OccupiedOffsets = occupyArea.OccupiedCellsOffsets;
                } else {
                    snapshot.OccupiedOffsets = new CellOffset[] { new CellOffset(0, 0) };
                }

                if (snapshot.IsGeyser) {
                    snapshot.SourceFootprint = FindFootprint(snapshot.SourceCell);
                    snapshot.geyserFields = CaptureFields(source.GetComponent<Geyser>());
                    snapshot.configuratorFields = CaptureFields(source.GetComponent<GeyserConfigurator>());
                } else {
                    snapshot.SourceFootprint = Array.Empty<int>();
                }

                snapshot.CapturePreviewAnimation(source);
                return snapshot;
            }

            public int[] GetTranslatedFootprint(int targetGeyserCell) {
                if (OccupiedOffsets == null || OccupiedOffsets.Length == 0)
                    return new int[] { targetGeyserCell };

                int[] result = new int[OccupiedOffsets.Length];
                for (int i = 0; i < OccupiedOffsets.Length; i++) {
                    result[i] = Grid.OffsetCell(targetGeyserCell, OccupiedOffsets[i]);
                }
                return result;
            }

            public int[] GetTranslatedNeutroniumFootprint(int targetGeyserCell) {
                if (SourceFootprint == null || SourceFootprint.Length == 0)
                    return Array.Empty<int>();

                int dx = Grid.CellToXY(targetGeyserCell).x - Grid.CellToXY(SourceCell).x;
                int dy = Grid.CellToXY(targetGeyserCell).y - Grid.CellToXY(SourceCell).y;
                int[] result = new int[SourceFootprint.Length];
                for (int i = 0; i < SourceFootprint.Length; i++) {
                    var xy = Grid.CellToXY(SourceFootprint[i]);
                    result[i] = Grid.XYToCell(xy.x + dx, xy.y + dy);
                }
                return result;
            }

            public void ApplyFieldsTo(GameObject target) {
                if (IsGeyser) {
                    ApplyFields(target.GetComponent<Geyser>(), geyserFields);
                    ApplyFields(target.GetComponent<GeyserConfigurator>(), configuratorFields);

                    var targetStudyable = target.GetComponent<Studyable>();
                    if (targetStudyable != null) {
                        targetStudyable.studied = KeepAnalysis ? IsStudied : false;
                    }

                    var targetGeyser = target.GetComponent<Geyser>();
                    if (targetGeyser != null) {
                        if (!KeepEruptionState) {
                            try {
                                var type = typeof(Geyser);
                                var fieldsToReset = new string[] { 
                                    "nextEruptTime", "nextActiveTime", "isErupting", "timeInCurrentState", 
                                    "keepStateElapsed", "idleDuration", "eruptDuration", "activeDuration", "inactiveDuration" 
                                };
                                foreach (var fieldName in fieldsToReset) {
                                    var f = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                                    if (f != null && !f.IsInitOnly) {
                                        if (f.FieldType == typeof(float)) f.SetValue(targetGeyser, 0f);
                                        else if (f.FieldType == typeof(bool)) f.SetValue(targetGeyser, false);
                                    }
                                }
                            }
                            catch (Exception ex) {
                                Debug.LogWarning("[MoveGeyserInstant] Failed to reset eruption cycle fields: " + ex.Message);
                            }
                        }
                    }
                }

                var targetShuffler = target.GetComponent<GeneShuffler>();
                if (targetShuffler != null) {
                    targetShuffler.IsConsumed = ResetVacillator ? false : IsGeneShufflerUsed;
                }
            }

            public void TriggerCopySettings(GameObject target) {
                target.Trigger((int)GameHashes.CopySettings, Source);
            }

            public HashedString GetPreviewAnim() {
                if (previewAnim.IsValid)
                    return previewAnim;
                if (!string.IsNullOrEmpty(previewInitialAnim))
                    return new HashedString(previewInitialAnim);
                if (!string.IsNullOrEmpty(previewDefaultAnim))
                    return new HashedString(previewDefaultAnim);
                return new HashedString("idle");
            }

            private void CapturePreviewAnimation(GameObject source) {
                var anim = source.GetComponent<KAnimControllerBase>();
                if (anim == null)
                    return;

                PreviewAnimFiles = anim.AnimFiles;
                previewAnim = anim.currentAnim;
                previewInitialAnim = anim.initialAnim;
                previewDefaultAnim = anim.defaultAnim;
            }

            private static int[] FindFootprint(int geyserCell) {
                var origin = Grid.CellToXY(geyserCell);
                var cells = new int[FootprintWidth * FootprintHeight];
                int index = 0;
                for (int x = origin.x - 1; x <= origin.x + 2; x++) {
                    cells[index++] = Grid.XYToCell(x, origin.y - 1);
                }
                return cells;
            }

            private static int[] PickBestFootprint(List<int> candidates, int geyserCell) {
                var geyserXY = Grid.CellToXY(geyserCell);
                int bestCount = int.MinValue;
                int bestScore = int.MaxValue;
                int bestX = geyserXY.x - 1;
                int bestY = geyserXY.y;
                for (int y = geyserXY.y - 3; y <= geyserXY.y; y++) {
                    for (int x = geyserXY.x - 4; x <= geyserXY.x + 2; x++) {
                        int count = 0;
                        for (int fy = 0; fy < FootprintHeight; fy++) {
                            for (int fx = 0; fx < FootprintWidth; fx++) {
                                if (candidates.Contains(Grid.XYToCell(x + fx, y + fy)))
                                    count++;
                            }
                        }
                        int score = Math.Abs((x + 1) - geyserXY.x) + Math.Abs((y + 0) - geyserXY.y);
                        if (count > bestCount || (count == bestCount && score < bestScore)) {
                            bestCount = count;
                            bestScore = score;
                            bestX = x;
                            bestY = y;
                        }
                    }
                }

                var result = new List<int>(FootprintWidth * FootprintHeight);
                for (int fy = 0; fy < FootprintHeight; fy++) {
                    for (int fx = 0; fx < FootprintWidth; fx++)
                        result.Add(Grid.XYToCell(bestX + fx, bestY + fy));
                }
                return result.ToArray();
            }

            private static Dictionary<string, object> CaptureFields(Component component) {
                var values = new Dictionary<string, object>();
                if (component == null)
                    return values;

                    foreach (FieldInfo field in component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                        if (field.IsInitOnly || field.IsLiteral)
                            continue;

                        Type fieldType = field.FieldType;
                        // Only capture primitive/value types and strings/enums. Do NOT capture
                        // UnityEngine.Object references or component references which can
                        // point to engine-managed objects (anim controllers, meters, etc.).
                        if (!(fieldType.IsValueType || fieldType == typeof(string) || fieldType.IsEnum))
                            continue;

                        try {
                            values[field.Name] = field.GetValue(component);
                        }
                        catch {
                        }
                }
                return values;
            }

            private static void ApplyFields(Component component, Dictionary<string, object> values) {
                if (component == null || values == null)
                    return;

                Type type = component.GetType();
                foreach (var pair in values) {
                    FieldInfo field = type.GetField(pair.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (field == null || field.IsInitOnly || field.IsLiteral)
                        continue;

                    Type fieldType = field.FieldType;
                    if (!(fieldType.IsValueType || fieldType == typeof(string) || fieldType.IsEnum))
                        continue;

                    try {
                        field.SetValue(component, pair.Value);
                    }
                    catch (Exception e) {
                        Debug.LogWarning("[MoveGeyserInstant] Failed to copy field " + type.Name + "." + pair.Key + ": " + e.Message);
                    }
                }
            }
        }
    }
}
