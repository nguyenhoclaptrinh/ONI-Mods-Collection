using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace RonivansLegacy_ChemicalProcessing.Patches
{
	[HarmonyPatch(typeof(KBatchedAnimController), "LoadAnims")]
	public static class KBatchedAnimController_LoadAnims_Patch
	{
		public static readonly List<KBatchedAnimController> deferredControllers = new List<KBatchedAnimController>();

		public static bool Prefix(KBatchedAnimController __instance)
		{
			if (__instance.animFiles == null || __instance.animFiles.Length == 0)
			{
				return false;
			}

			KAnimFile firstFile = __instance.animFiles[0];
			if (firstFile == null)
			{
				return false;
			}

			// Kiểm tra xem KAnimBatchManager đã sẵn sàng và dữ liệu anim đã được nạp chưa
			if (!KAnimBatchManager.Instance().isReady || firstFile.GetData() == null)
			{
				// Thêm vào danh sách chờ
				if (!deferredControllers.Contains(__instance))
				{
					deferredControllers.Add(__instance);
				}
				return false; // Bỏ qua chạy hàm gốc để tránh crash
			}

			return true; // Chạy hàm gốc bình thường
		}
	}

	[HarmonyPatch(typeof(KAnimBatchManager), "CompleteInit")]
	public static class KAnimBatchManager_CompleteInit_Patch
	{
		public static void Postfix()
		{
			List<KBatchedAnimController> controllers = KBatchedAnimController_LoadAnims_Patch.deferredControllers;
			if (controllers.Count > 0)
			{
				Debug.Log($"[RonivanPatches] KAnimBatchManager ready. Loading anims for {controllers.Count} deferred controllers.");
				foreach (KBatchedAnimController controller in controllers)
				{
					if (controller != null)
					{
						controller.LoadAnims();
					}
				}
				controllers.Clear();
			}
		}
	}
}
