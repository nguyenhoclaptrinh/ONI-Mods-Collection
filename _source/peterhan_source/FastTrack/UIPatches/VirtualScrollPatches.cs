/*
 * Copyright 2026 Peter Han
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using HarmonyLib;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using ChangedSectionPool = DictionaryPool<UnityEngine.Transform, PeterHan.FastTrack.UIPatches.
	VirtualScroll, ReceptacleSideScreen>;
using ITState = ImageToggleState.State;

namespace PeterHan.FastTrack.UIPatches {
	/// <summary>
	/// Applied to DragMe to set it as always visible when dragged off screen.
	/// </summary>
	[HarmonyPatch(typeof(DragMe), nameof(DragMe.OnBeginDrag))]
	public static class DragMe_OnBeginDrag_Patch {
		internal static bool Prepare() => FastTrackOptions.Instance.VirtualScroll;

		/// <summary>
		/// Applied after OnBeginDrag runs.
		/// </summary>
		internal static void Postfix(DragMe __instance) {
			GameObject go;
			if (__instance != null && (go = __instance.gameObject) != null) {
				var vs = go.GetComponentInParent<VirtualScroll>();
				if (vs != null)
					vs.SetForceShow(go);
			}
		}
	}

	/// <summary>
	/// Applied to DragMe to clear it from always visible after dragging is complete.
	/// </summary>
	[HarmonyPatch(typeof(DragMe), nameof(DragMe.OnEndDrag))]
	public static class DragMe_OnEndDrag_Patch {
		internal static bool Prepare() => FastTrackOptions.Instance.VirtualScroll;

		/// <summary>
		/// Applied after OnEndDrag runs.
		/// </summary>
		internal static void Postfix(DragMe __instance) {
			GameObject go;
			if (__instance != null && (go = __instance.gameObject) != null) {
				var vs = go.GetComponentInParent<VirtualScroll>();
				if (vs != null)
					vs.ClearForceShow(go);
			}
		}
	}

	/// <summary>
	/// Applied to ModsScreen to update the scroll pane whenever the list changes.
	/// </summary>
	[HarmonyPatch(typeof(ModsScreen), nameof(ModsScreen.BuildDisplay))]
	public static class ModsScreen_BuildDisplay_Patch {
		internal static bool Prepare() => FastTrackOptions.Instance.VirtualScroll;

		/// <summary>
		/// Applied before BuildDisplay runs.
		/// </summary>
		[HarmonyPriority(Priority.High)]
		internal static void Prefix(ModsScreen __instance, ref VirtualScroll __state) {
			var entryList = __instance.entryParent;
			if (entryList != null && entryList.TryGetComponent(out VirtualScroll vs)) {
				vs.OnBuild();
				__state = vs;
			} else
				__state = null;
		}

		/// <summary>
		/// Applied after BuildDisplay runs.
		/// </summary>
		[HarmonyPriority(Priority.VeryLow)]
		internal static void Postfix(VirtualScroll __state) {
			if (__state != null)
				__state.Rebuild();
		}
	}

	/// <summary>
	/// Applied to ModsScreen to set up listeners and state for virtual scroll.
	/// </summary>
	[HarmonyPatch(typeof(ModsScreen), nameof(ModsScreen.OnActivate))]
	public static class ModsScreen_OnActivate_Patch {
		internal static bool Prepare() => FastTrackOptions.Instance.VirtualScroll;

		/// <summary>
		/// Applied after OnActivate runs.
		/// </summary>
		internal static void Postfix(ModsScreen __instance) {
			var entryList = __instance.entryParent;
			GameObject go;
			if (entryList != null && (go = entryList.gameObject) != null) {
				var vs = go.AddOrGet<VirtualScroll>();
				vs.freezeLayout = true;
				vs.Initialize();
			}
		}
	}
	
	/// <summary>
	/// Groups patches used for the Receptacle side screen (incubator, farm tile, pedestal...)
	/// </summary>
	public static class ReceptacleSideScreenPatches {
		/// <summary>
		/// Since ReceptacleSideScreen is a pseudo singleton (only one will be active at a
		/// time), static is safe here.
		/// </summary>
		private static bool initializing;

		/// <summary>
		/// Caches the number of active items in each category to avoid calling transform.GetChild() repeatedly.
		/// </summary>
		private static readonly Dictionary<Transform, int> ACTIVE_COUNTS = new Dictionary<Transform, int>(32);

		/// <summary>
		/// Caches the Image component for each MultiToggle to avoid GetComponentInChildrenOnly call.
		/// </summary>
		private static readonly Dictionary<MultiToggle, Image> TOGGLE_IMAGES = new Dictionary<MultiToggle, Image>(32);

		/// <summary>
		/// Caches the GameObject for each toggle key to avoid calling key.gameObject.
		/// </summary>
		private static readonly Dictionary<object, GameObject> TOGGLE_GO = new Dictionary<object, GameObject>(64);

		/// <summary>
		/// Caches the parent Transform of each toggle to avoid calling go.transform.parent.
		/// </summary>
		private static readonly Dictionary<object, Transform> TOGGLE_PARENTS = new Dictionary<object, Transform>(64);

		/// <summary>
		/// Caches the ImageToggleState for each MultiToggle to avoid calling TryGetComponent.
		/// </summary>
		private static readonly Dictionary<MultiToggle, ImageToggleState> TOGGLE_STATES = new Dictionary<MultiToggle, ImageToggleState>(64);

		/// <summary>
		/// Applied to ReceptacleSideScreen to prepare existing virtual scroll panels for a
		/// rebuild.
		/// </summary>
		[HarmonyPatch(typeof(ReceptacleSideScreen), nameof(ReceptacleSideScreen.Initialize))]
		internal static class Initialize_Patch {
			internal static bool Prepare() => FastTrackOptions.Instance.VirtualScroll;

			/// <summary>
			/// Applied before Initialize runs.
			/// </summary>
			internal static void Prefix(ReceptacleSideScreen __instance) {
				// Clear caches to prevent memory leak
				ACTIVE_COUNTS.Clear();
				TOGGLE_IMAGES.Clear();
				TOGGLE_GO.Clear();
				TOGGLE_PARENTS.Clear();
				TOGGLE_STATES.Clear();

				// Content containers are not disposed
				foreach (var pair in __instance.contentContainers)
					if (pair.Value.TryGetComponent(out HierarchyReferences hr)) {
						var grid = hr.GetReference<GridLayoutGroup>("GridLayout");
						if (grid != null && grid.TryGetComponent(out VirtualScroll vs))
							vs.OnBuild();
					}
				initializing = true;
			}
		}

		/// <summary>
		/// Applied to ReceptacleSideScreen to properly rebuild the layout if the available
		/// item amounts change.
		/// </summary>
		[HarmonyPatch(typeof(ReceptacleSideScreen), nameof(ReceptacleSideScreen.
			UpdateAvailableAmounts))]
		internal static class UpdateAvailableAmounts_Patch {
			/// <summary>
			/// Avoid reallocating a new StringBuilder every frame.
			/// </summary>
			private static readonly StringBuilder CACHED_BUILDER = new StringBuilder(16);

			internal static bool Prepare() => FastTrackOptions.Instance.VirtualScroll;

			/// <summary>
			/// Applied before UpdateAvailableAmounts runs.
			/// </summary>
			[HarmonyPriority(Priority.Low)]
			internal static bool Prefix(ReceptacleSideScreen __instance, ref bool __result) {
				ACTIVE_COUNTS.Clear();
				bool result = false, hide = !DebugHandler.InstantBuildMode &&
					__instance.hideUndiscoveredEntities;
				var inst = DiscoveredResources.Instance;
				var selected = __instance.selectedEntityToggle;
				var text = CACHED_BUILDER;
				var changed = new VirtualScrollTracker(__instance);
				foreach (var pair in __instance.depositObjectMap) {
					var key = pair.Key;
					var display = pair.Value;
					if (!TOGGLE_GO.TryGetValue(key, out GameObject go) || go == null) {
						go = key.gameObject;
						TOGGLE_GO[key] = go;
					}
					// Finds the GridLayout
					if (!TOGGLE_PARENTS.TryGetValue(key, out Transform parent) || parent == null) {
						parent = go.transform.parent;
						TOGGLE_PARENTS[key] = parent;
					}
					bool active = go.activeSelf;
					var tag = display.tag;
					// Hide undiscovered entities in some screens (like pedestal)
					if (hide && !inst.IsDiscovered(tag)) {
						if (active) {
							changed.Mark(parent);
							go.SetActive(active = false);
						}
					} else if (!active) {
						changed.Mark(parent);
						go.SetActive(active = true);
					}
					if (active) {
						if (parent != null) {
							if (ACTIVE_COUNTS.TryGetValue(parent, out int count))
								ACTIVE_COUNTS[parent] = count + 1;
							else
								ACTIVE_COUNTS[parent] = 1;
						}
						var toggle = key.toggle;
						// Do not update amounts of inactive items
						float availableAmount = __instance.GetAvailableAmount(tag);
						if (!Mathf.Approximately(display.lastAmount, availableAmount)) {
							result = true;
							// Update display only if it actually changed
							display.lastAmount = availableAmount;
							text.Clear();
							availableAmount.ToRyuSoftString(text, 2);
							key.amount.SetText(text);
						}
						if (!__instance.ValidRotationForDeposit(display.direction) ||
								availableAmount <= 0.0f)
							// Disable items which cannot fit in this orientation or are
							// unavailable
							SetImageToggleState(__instance, toggle, selected != key ? ITState.
								Disabled : ITState.DisabledActive);
						else if (selected != key)
							SetImageToggleState(__instance, toggle, ITState.Inactive);
						else
							SetImageToggleState(__instance, toggle, ITState.Active);
					}
				}
				changed.Sync();
				changed.Dispose();
				__result = result;
				initializing = false;
				return false;
			}

			/// <summary>
			/// Sets the toggle state of a button only if it actually changed.
			/// </summary>
			/// <param name="instance">The side screen being updated.</param>
			/// <param name="toggle">The toggle to modify.</param>
			/// <param name="state">The state to apply.</param>
			private static void SetImageToggleState(ReceptacleSideScreen instance,
					MultiToggle toggle, ITState state) {
				if (!TOGGLE_STATES.TryGetValue(toggle, out ImageToggleState its) || its == null) {
					toggle.TryGetComponent(out its);
					TOGGLE_STATES[toggle] = its;
				}
				if (its != null && (initializing || state != its.currentState)) {
					// SetState provides no feedback on whether the state actually changed
					if (!TOGGLE_IMAGES.TryGetValue(toggle, out Image targetImage) || targetImage == null) {
						targetImage = toggle.gameObject.GetComponentInChildrenOnly<Image>();
						TOGGLE_IMAGES[toggle] = targetImage;
					}
					if (targetImage != null) {
						switch (state) {
						case ITState.Disabled:
							its.SetDisabled();
							targetImage.material = instance.desaturatedMaterial;
							break;
						case ITState.Inactive:
							its.SetInactive();
							targetImage.material = instance.defaultMaterial;
							break;
						case ITState.Active:
							its.SetActive();
							targetImage.material = instance.defaultMaterial;
							break;
						case ITState.DisabledActive:
							its.SetDisabledActive();
							targetImage.material = instance.desaturatedMaterial;
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Tracks the dirty virtual scroll panes and updates the ones that need to be done.
		/// </summary>
		private readonly struct VirtualScrollTracker {
			private readonly ChangedSectionPool.PooledDictionary changed;

			private readonly ReceptacleSideScreen screen;

			public VirtualScrollTracker(ReceptacleSideScreen instance) {
				changed = ChangedSectionPool.Allocate();
				screen = instance;
			}

			public void Dispose() {
				changed.Recycle();
			}

			/// <summary>
			/// Marks a transform as dirty.
			/// </summary>
			/// <param name="parent">The transform of the group that is dirty.</param>
			public void Mark(Transform parent) {
				if (!changed.TryGetValue(parent, out _)) {
					if (parent.TryGetComponent(out VirtualScroll vs)) {
						vs.OnBuild();
						changed[parent] = vs;
					}
				}
			}

			/// <summary>
			/// Synchronizes the visibility of each group in the screen with the visibility of
			/// their contents.
			/// </summary>
			public void Sync() {
				foreach (var pair in screen.contentContainers) {
					var cc = pair.Value;
					GameObject grid;
					if (cc != null && cc.TryGetComponent(out HierarchyReferences refs)) {
						var gl = refs.GetReference<GridLayoutGroup>("GridLayout");
						var transform = gl.transform;
						bool anyActive = ACTIVE_COUNTS.TryGetValue(transform, out int count) && count > 0;
						if (cc.activeSelf != anyActive)
							cc.SetActive(anyActive);
						// If hidden, no need to rebuild it
						if (!anyActive)
							changed.Remove(transform);
						else if (!gl.TryGetComponent(out VirtualScroll _) &&
								(grid = gl.gameObject).activeSelf) {
							var target = grid.GetComponentInParent<KScrollRect>();
							if (target != null) {
								// Create here when the layout is guaranteed to be active
								var vs = grid.AddComponent<VirtualScroll>();
								vs.freezeLayout = true;
								vs.ForceAwake(target);
								vs.Initialize();
							}
						}
					}
				}
				foreach (var pair in changed)
					pair.Value.Rebuild();
			}
		}
	}
}
