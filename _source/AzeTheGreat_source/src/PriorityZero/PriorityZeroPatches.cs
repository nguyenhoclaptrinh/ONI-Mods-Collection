using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PriorityZero
{
    public class PriorityZeroMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("Priority Zero Mod (v706793+) - Loaded successfully!");
        }
    }

    public static class PriorityZeroState
    {
        private static readonly Dictionary<PriorityScreen, PriorityButton> ZeroButtons = new Dictionary<PriorityScreen, PriorityButton>();
        private static readonly Dictionary<Prioritizable, TMPro.TextMeshPro> ZeroPriorityMarkers = new Dictionary<Prioritizable, TMPro.TextMeshPro>();
        private static readonly HashSet<Prioritizable> VisibleZeroPriorityMarkers = new HashSet<Prioritizable>();
        private static readonly List<Prioritizable> MarkersToRemove = new List<Prioritizable>();
        private static bool loggedPriorityReadFailure;
        private static UnityEngine.Texture2D zeroCursorTexture;

        public static bool IsPriorityZero(PrioritySetting priority)
        {
            return priority.priority_value == 0;
        }

        public static UnityEngine.Sprite GetPrioritySprite(List<UnityEngine.Sprite> sprites, int index)
        {
            return sprites != null && index >= 0 && index < sprites.Count ? sprites[index] : null;
        }

        public static bool HasZeroPriority(Chore chore)
        {
            if (chore == null)
            {
                return false;
            }

            try
            {
                return IsPriorityZero(chore.masterPriority) ||
                       (chore.prioritizable != null && IsPriorityZero(chore.prioritizable.GetMasterPriority()));
            }
            catch (System.Exception e)
            {
                if (!loggedPriorityReadFailure)
                {
                    loggedPriorityReadFailure = true;
                    Debug.LogWarning("Priority Zero: Failed to read chore priority, allowing chore to continue. " + e);
                }

                return false;
            }
        }

        public static bool HasZeroPriority(Prioritizable prioritizable)
        {
            if (prioritizable == null)
            {
                return false;
            }

            try
            {
                return IsPriorityZero(prioritizable.GetMasterPriority());
            }
            catch (System.Exception e)
            {
                if (!loggedPriorityReadFailure)
                {
                    loggedPriorityReadFailure = true;
                    Debug.LogWarning("Priority Zero: Failed to read prioritizable priority, allowing render to continue. " + e);
                }

                return false;
            }
        }

        public static bool HasZeroButton(PriorityScreen screen)
        {
            return TryGetZeroButton(screen, out _);
        }

        public static UnityEngine.Texture2D GetZeroCursorTexture(UnityEngine.Texture2D referenceTexture)
        {
            int width = referenceTexture != null ? referenceTexture.width : 64;
            int height = referenceTexture != null ? referenceTexture.height : 64;

            if (zeroCursorTexture == null || zeroCursorTexture.width != width || zeroCursorTexture.height != height)
            {
                zeroCursorTexture = CreateZeroTexture(width, height);
            }

            return zeroCursorTexture;
        }

        public static void HideZeroPriorityMarkers()
        {
            foreach (TMPro.TextMeshPro marker in ZeroPriorityMarkers.Values)
            {
                if (marker != null)
                {
                    marker.gameObject.SetActive(false);
                }
            }
        }

        public static void RenderZeroPriorityMarkers(IList<Prioritizable> prioritizables)
        {
            VisibleZeroPriorityMarkers.Clear();

            if (prioritizables != null)
            {
                for (int i = 0; i < prioritizables.Count; i++)
                {
                    Prioritizable prioritizable = prioritizables[i];
                    if (HasZeroPriority(prioritizable))
                    {
                        TMPro.TextMeshPro marker = GetOrCreateZeroPriorityMarker(prioritizable);
                        UpdateZeroPriorityMarker(marker, prioritizable);
                        marker.gameObject.SetActive(true);
                        VisibleZeroPriorityMarkers.Add(prioritizable);
                    }
                }
            }

            MarkersToRemove.Clear();
            foreach (KeyValuePair<Prioritizable, TMPro.TextMeshPro> entry in ZeroPriorityMarkers)
            {
                if (entry.Key == null || !VisibleZeroPriorityMarkers.Contains(entry.Key))
                {
                    if (entry.Value != null)
                    {
                        UnityEngine.Object.Destroy(entry.Value.gameObject);
                    }

                    MarkersToRemove.Add(entry.Key);
                }
            }

            for (int i = 0; i < MarkersToRemove.Count; i++)
            {
                ZeroPriorityMarkers.Remove(MarkersToRemove[i]);
            }
        }

        public static void DestroyZeroPriorityMarkers()
        {
            foreach (TMPro.TextMeshPro marker in ZeroPriorityMarkers.Values)
            {
                if (marker != null)
                {
                    UnityEngine.Object.Destroy(marker.gameObject);
                }
            }

            ZeroPriorityMarkers.Clear();
            VisibleZeroPriorityMarkers.Clear();
            MarkersToRemove.Clear();
        }

        public static void RegisterZeroButton(PriorityScreen screen, PriorityButton button)
        {
            ZeroButtons[screen] = button;
            RefreshZeroButton(screen, screen.lastSelectedPriority);
        }

        public static bool TryGetZeroButton(PriorityScreen screen, out PriorityButton button)
        {
            button = null;
            if (screen == null)
            {
                return false;
            }

            if (!ZeroButtons.TryGetValue(screen, out button) || button == null)
            {
                ZeroButtons.Remove(screen);
                return false;
            }

            return true;
        }

        public static void RefreshZeroButton(PriorityScreen screen, PrioritySetting selectedPriority)
        {
            if (!TryGetZeroButton(screen, out PriorityButton button) || button.toggle == null)
            {
                return;
            }

            bool isSelected = button.priority == selectedPriority;
            if (isSelected)
            {
                button.toggle.Select();
            }

            button.toggle.isOn = isSelected;
        }

        private static UnityEngine.Texture2D CreateZeroTexture(int width, int height)
        {
            UnityEngine.Texture2D texture = new UnityEngine.Texture2D(width, height, UnityEngine.TextureFormat.RGBA32, false);
            UnityEngine.Color transparent = new UnityEngine.Color(0f, 0f, 0f, 0f);
            UnityEngine.Color outline = new UnityEngine.Color(0f, 0f, 0f, 1f);
            UnityEngine.Color fill = new UnityEngine.Color(1f, 1f, 1f, 1f);
            float centerX = (width - 1) * 0.5f;
            float centerY = (height - 1) * 0.5f;
            // Tinh chỉnh tỷ lệ elip thon thả thanh mảnh như font gốc của game
            float radiusX = width * 0.22f;
            float radiusY = height * 0.38f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dx = (x - centerX) / radiusX;
                    float dy = (y - centerY) / radiusY;
                    float distance = dx * dx + dy * dy;
                    UnityEngine.Color color = transparent;

                    // Vẽ elip nét mảnh mai tinh tế
                    if (distance >= 0.75f && distance <= 1.25f)
                    {
                        color = outline;
                    }

                    if (distance >= 0.88f && distance <= 1.08f)
                    {
                        color = fill;
                    }

                    texture.SetPixel(x, y, color);
                }
            }

            texture.wrapMode = UnityEngine.TextureWrapMode.Clamp;
            texture.filterMode = UnityEngine.FilterMode.Bilinear;
            texture.Apply(false, true);
            return texture;
        }

        private static TMPro.TextMeshPro GetOrCreateZeroPriorityMarker(Prioritizable prioritizable)
        {
            if (!ZeroPriorityMarkers.TryGetValue(prioritizable, out TMPro.TextMeshPro marker) || marker == null)
            {
                UnityEngine.GameObject gameObject = new UnityEngine.GameObject("priority_zero_marker");
                if (Game.Instance != null)
                {
                    gameObject.transform.SetParent(Game.Instance.transform, false);
                }

                gameObject.layer = 0; // Layer Default giúp camera thế giới render đúng màu sắc và outline của TextMeshPro
                marker = gameObject.AddComponent<TMPro.TextMeshPro>();
                marker.text = "0";
                marker.font = Localization.GetFont(Localization.GetDefaultFontName());
                marker.fontSize = 4.5f; // TMP World Space Font Size
                marker.color = UnityEngine.Color.white;
                marker.alignment = TMPro.TextAlignmentOptions.Center;

                // Cấu hình outline đen cực đẹp và sắc nét giống font game
                marker.outlineWidth = 0.2f;
                marker.outlineColor = new UnityEngine.Color32(0, 0, 0, 255);

                UnityEngine.MeshRenderer renderer = marker.GetComponent<UnityEngine.MeshRenderer>();
                if (renderer != null)
                {
                    renderer.sortingOrder = 100;
                }

                ZeroPriorityMarkers[prioritizable] = marker;
            }

            return marker;
        }

        private static void UpdateZeroPriorityMarker(TMPro.TextMeshPro marker, Prioritizable prioritizable)
        {
            UnityEngine.Vector3 position;
            KAnimControllerBase kAnimController = prioritizable.GetComponent<KAnimControllerBase>();
            if (kAnimController != null)
            {
                position = kAnimController.GetWorldPivot();
            }
            else
            {
                position = prioritizable.transform.position;
            }

            position.x += prioritizable.iconOffset.x;
            position.y += prioritizable.iconOffset.y;
            position.z = -6f;

            marker.transform.SetPositionAndRotation(position, UnityEngine.Quaternion.identity);
            marker.transform.localScale = new UnityEngine.Vector3(prioritizable.iconScale, prioritizable.iconScale, 1f);
        }
    }

    [HarmonyPatch(typeof(StandardChoreBase), nameof(StandardChoreBase.IsValid))]
    public static class StandardChoreBase_IsValid_Patch
    {
        [HarmonyPriority(Priority.Last)]
        public static bool Prefix(Chore __instance, ref bool __result)
        {
            if (PriorityZeroState.HasZeroPriority(__instance))
            {
                __result = false;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(MinionTodoChoreEntry), nameof(MinionTodoChoreEntry.Apply))]
    public static class MinionTodoChoreEntry_Apply_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            System.Reflection.MethodInfo getItem = AccessTools.PropertyGetter(typeof(List<UnityEngine.Sprite>), "Item");
            System.Reflection.MethodInfo safeGetItem = AccessTools.Method(typeof(PriorityZeroState), nameof(PriorityZeroState.GetPrioritySprite));
            bool replaced = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Callvirt && Equals(instruction.operand, getItem))
                {
                    instruction.opcode = OpCodes.Call;
                    instruction.operand = safeGetItem;
                    replaced = true;
                }

                yield return instruction;
            }

            if (!replaced)
            {
                Debug.LogWarning("Priority Zero: Unable to patch MinionTodoChoreEntry.Apply priority icon lookup.");
            }
        }
    }

    [HarmonyPatch(typeof(ChoreConsumer), nameof(ChoreConsumer.FindNextChore))]
    public static class ChoreConsumer_FindNextChore_Patch
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(ref bool __result, ref Chore.Precondition.Context out_context)
        {
            if (__result && PriorityZeroState.HasZeroPriority(out_context.chore))
            {
                __result = false;
                out_context = new Chore.Precondition.Context();
            }
        }
    }

    [HarmonyPatch(typeof(PrioritizeTool), nameof(PrioritizeTool.Update))]
    public static class PrioritizeTool_Update_Patch
    {
        public static bool Prefix(PrioritizeTool __instance)
        {
            try
            {
                PriorityScreen priorityScreen = ToolMenu.Instance != null ? ToolMenu.Instance.PriorityScreen : null;
                if (priorityScreen == null || !PriorityZeroState.IsPriorityZero(priorityScreen.GetLastSelectedPriority()))
                {
                    return true;
                }

                UnityEngine.Texture2D[] cursors = __instance.cursors;
                if (cursors != null && cursors.Length > 0 && __instance.visualizer != null)
                {
                    UnityEngine.MeshRenderer renderer = __instance.visualizer.GetComponentInChildren<UnityEngine.MeshRenderer>();
                    if (renderer != null)
                    {
                        renderer.material.mainTexture = PriorityZeroState.GetZeroCursorTexture(cursors[0]);
                    }
                }

                return false;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[PriorityZero] Lỗi Prefix Update Tool: " + e.Message);
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(PrioritizableRenderer), nameof(PrioritizableRenderer.RenderEveryTick))]
    public static class PrioritizableRenderer_RenderEveryTick_ZeroMarkersPatch
    {
        public static void Postfix(List<Prioritizable> ___prioritizables)
        {
            try
            {
                if (GameScreenManager.Instance == null ||
                    SimDebugView.Instance == null ||
                    SimDebugView.Instance.GetMode() != OverlayModes.Priorities.ID)
                {
                    PriorityZeroState.HideZeroPriorityMarkers();
                    return;
                }

                PriorityZeroState.RenderZeroPriorityMarkers(___prioritizables);
            }
            catch (System.Exception e)
            {
                // Chỉ log cảnh báo, ẩn marker rác và cho phép game tiếp tục
                Debug.LogWarning("[PriorityZero] Lỗi vẽ nhãn ưu tiên số 0: " + e.Message);
                try
                {
                    PriorityZeroState.HideZeroPriorityMarkers();
                }
                catch {}
            }
        }
    }

    [HarmonyPatch(typeof(PrioritizableRenderer), nameof(PrioritizableRenderer.Cleanup))]
    public static class PrioritizableRenderer_Cleanup_ZeroMarkersPatch
    {
        public static void Prefix()
        {
            PriorityZeroState.DestroyZeroPriorityMarkers();
        }
    }

    [HarmonyPatch(typeof(PriorityScreen), "InstantiateButtons")]
    public static class PriorityScreen_InstantiateButtons_Patch
    {
        public static void Postfix(PriorityScreen __instance)
        {
            try
            {
                if (__instance.buttons_basic == null || __instance.buttons_basic.Count == 0) return;

                if (PriorityZeroState.HasZeroButton(__instance)) return;

                PriorityButton btn1 = __instance.buttons_basic[0];
                if (btn1 == null) return;

                PriorityButton btn0 = UnityEngine.Object.Instantiate(btn1, btn1.transform.parent);
                btn0.name = "button_prio_0";
                btn0.priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 0);
                btn0.onClick = priority =>
                {
                    __instance.SetScreenPriority(priority, false);
                    __instance.OnClick(priority);
                };

                if (btn0.text != null)
                {
                    btn0.text.text = "0";
                }

                if (btn0.tooltip != null)
                {
                    btn0.tooltip.SetSimpleTooltip("Vô hiệu hóa công việc (Priority 0) - Duplicants và Auto-Sweepers sẽ bỏ qua");
                }

                btn0.transform.SetAsFirstSibling();
                PriorityZeroState.RegisterZeroButton(__instance, btn0);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[PriorityZero] Không thể tạo nút bấm Priority 0 trên UI: " + e.Message);
            }
        }
    }

    [HarmonyPatch(typeof(PriorityScreen), nameof(PriorityScreen.PlayPriorityConfirmSound))]
    public static class PriorityScreen_PlayPriorityConfirmSound_Patch
    {
        public static bool Prefix(PrioritySetting priority)
        {
            return !PriorityZeroState.IsPriorityZero(priority);
        }
    }

    [HarmonyPatch(typeof(PriorityScreen), nameof(PriorityScreen.SetScreenPriority))]
    public static class PriorityScreen_SetScreenPriority_RefreshZeroPatch
    {
        public static void Postfix(PriorityScreen __instance, PrioritySetting priority)
        {
            PriorityZeroState.RefreshZeroButton(__instance, priority);
        }
    }
}
