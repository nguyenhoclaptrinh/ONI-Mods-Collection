using HarmonyLib;
using System.Collections.Generic;

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
        private static readonly Dictionary<Prioritizable, UnityEngine.TextMesh> ZeroPriorityMarkers = new Dictionary<Prioritizable, UnityEngine.TextMesh>();
        private static readonly HashSet<Prioritizable> VisibleZeroPriorityMarkers = new HashSet<Prioritizable>();
        private static readonly List<Prioritizable> MarkersToRemove = new List<Prioritizable>();
        private static bool loggedPriorityReadFailure;
        private static UnityEngine.Texture2D zeroCursorTexture;

        public static bool IsPriorityZero(PrioritySetting priority)
        {
            return priority.priority_value == 0;
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
            foreach (UnityEngine.TextMesh marker in ZeroPriorityMarkers.Values)
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
                        UnityEngine.TextMesh marker = GetOrCreateZeroPriorityMarker(prioritizable);
                        UpdateZeroPriorityMarker(marker, prioritizable);
                        marker.gameObject.SetActive(true);
                        VisibleZeroPriorityMarkers.Add(prioritizable);
                    }
                }
            }

            MarkersToRemove.Clear();
            foreach (KeyValuePair<Prioritizable, UnityEngine.TextMesh> entry in ZeroPriorityMarkers)
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
            foreach (UnityEngine.TextMesh marker in ZeroPriorityMarkers.Values)
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
            float radiusX = width * 0.28f;
            float radiusY = height * 0.36f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dx = (x - centerX) / radiusX;
                    float dy = (y - centerY) / radiusY;
                    float distance = dx * dx + dy * dy;
                    UnityEngine.Color color = transparent;

                    if (distance >= 0.58f && distance <= 1.18f)
                    {
                        color = outline;
                    }

                    if (distance >= 0.72f && distance <= 0.98f)
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

        private static UnityEngine.TextMesh GetOrCreateZeroPriorityMarker(Prioritizable prioritizable)
        {
            if (!ZeroPriorityMarkers.TryGetValue(prioritizable, out UnityEngine.TextMesh marker) || marker == null)
            {
                UnityEngine.GameObject gameObject = new UnityEngine.GameObject("priority_zero_marker");
                if (Game.Instance != null)
                {
                    gameObject.transform.SetParent(Game.Instance.transform, false);
                }

                gameObject.layer = UnityEngine.LayerMask.NameToLayer("UI");
                marker = gameObject.AddComponent<UnityEngine.TextMesh>();
                marker.text = "0";
                marker.anchor = UnityEngine.TextAnchor.MiddleCenter;
                marker.alignment = UnityEngine.TextAlignment.Center;
                marker.fontSize = 64;
                marker.color = UnityEngine.Color.white;

                UnityEngine.MeshRenderer renderer = marker.GetComponent<UnityEngine.MeshRenderer>();
                if (renderer != null)
                {
                    renderer.sortingOrder = 100;
                }

                ZeroPriorityMarkers[prioritizable] = marker;
            }

            return marker;
        }

        private static void UpdateZeroPriorityMarker(UnityEngine.TextMesh marker, Prioritizable prioritizable)
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

            marker.characterSize = 0.12f * prioritizable.iconScale;
            marker.transform.SetPositionAndRotation(position, UnityEngine.Quaternion.identity);
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
    }

    [HarmonyPatch(typeof(PrioritizableRenderer), nameof(PrioritizableRenderer.RenderEveryTick))]
    public static class PrioritizableRenderer_RenderEveryTick_ZeroMarkersPatch
    {
        public static void Postfix(List<Prioritizable> ___prioritizables)
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
