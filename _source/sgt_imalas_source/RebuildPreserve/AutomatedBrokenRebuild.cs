using KSerialization;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace RebuildPreserve
{
	internal class AutomatedBrokenRebuild : KMonoBehaviour
	{
		private static readonly FieldInfo AllowDeconstructionField = AccessTools.Field(typeof(Deconstructable), "allowDeconstruction");
		private static readonly FieldInfo DestroyOnDamagedField = AccessTools.Field(typeof(BuildingHP), "destroyOnDamaged");
		private static readonly FieldInfo ReconstructRequestedField = AccessTools.Field(typeof(Reconstructable), "reconstructRequested");

		[MyCmpGet]
		Reconstructable reconstructable;
		[MyCmpGet]
		BuildingComplete building;
		[MyCmpGet]
		Deconstructable deconstructable;
		[MyCmpGet]
		PrimaryElement primaryElement;
		[MyCmpGet]
		BuildingHP hp;

		[Serialize]
		public bool RebuildOnBreaking = false;

		private static readonly EventSystem.IntraObjectHandler<AutomatedBrokenRebuild> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AutomatedBrokenRebuild>((component, data) => component.OnCopySettings(data));

		public bool IsValidForRebuilding => CanRebuild();

		bool CanRebuild()
		{
			if (reconstructable == null)
				return false;

			if (building == null || building.Def == null)
				return false;

			if (building.Def.Invincible)
				return false;

			if (deconstructable == null)
				return false;

			if (!AllowsDeconstruction(deconstructable))
				return false;

			if (hp == null || DestroysOnDamage(hp))
				return false;

			return true;
		}

		static bool AllowsDeconstruction(Deconstructable target)
		{
			return target != null
				&& AllowDeconstructionField != null
				&& (bool)AllowDeconstructionField.GetValue(target);
		}

		static bool DestroysOnDamage(BuildingHP target)
		{
			return target == null
				|| DestroyOnDamagedField == null
				|| (bool)DestroyOnDamagedField.GetValue(target);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (reconstructable == null)
			{
				this.enabled = false;
				return;
			}
			if (CanRebuild())
			{
				Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
				Subscribe((int)GameHashes.BuildingBroken, OnBuildingBroken);
				GameScheduler.Instance.ScheduleNextFrame("queue rebuild for already broken building", (_) => RequestRebuildIfBroken());
			}
		}
		public override void OnCleanUp()
		{
			base.OnCleanUp();
			if (!CanRebuild())
			{
				return;
			}
			Unsubscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
			Unsubscribe((int)GameHashes.BuildingBroken, OnBuildingBroken);
		}

		public void OnBuildingBroken(object data)
		{
			RequestRebuildIfBroken();
		}

		public void RequestRebuildIfBroken()
		{
			if (RebuildOnBreaking && CanRebuild() && hp.IsBroken && !IsReconstructRequested())
			{
				reconstructable.RequestReconstruct(primaryElement.Element.tag);
			}
		}

		bool IsReconstructRequested()
		{
			return reconstructable != null
				&& ReconstructRequestedField != null
				&& (bool)ReconstructRequestedField.GetValue(reconstructable);
		}

		public void OnCopySettings(object data)
		{
			if (data is GameObject sauceGameObject && sauceGameObject.TryGetComponent<AutomatedBrokenRebuild>(out var addon))
			{
				this.RebuildOnBreaking = addon.RebuildOnBreaking;
			}
		}
	}
}
