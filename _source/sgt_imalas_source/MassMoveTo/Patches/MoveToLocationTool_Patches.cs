using HarmonyLib;
using UnityEngine;

namespace MassMoveTo.Patches
{
	internal static class MoveToLocationToolPatchHelper
	{
		public static int activateFrame = -1;
	}

	[HarmonyPatch(typeof(MoveToLocationTool), "OnActivateTool")]
	public static class MoveToLocationTool_OnActivateTool_Patch
	{
		public static void Postfix()
		{
			MoveToLocationToolPatchHelper.activateFrame = Time.frameCount;
		}
	}

	[HarmonyPatch(typeof(MoveToLocationTool), "OnLeftClickDown")]
	public static class MoveToLocationTool_OnLeftClickDown_Patch
	{
		public static bool Prefix()
		{
			if (MoveToLocationToolPatchHelper.activateFrame != -1 && 
				Time.frameCount - MoveToLocationToolPatchHelper.activateFrame <= 2)
			{
				return false; // Chặn leak click
			}
			return true;
		}
	}
}
