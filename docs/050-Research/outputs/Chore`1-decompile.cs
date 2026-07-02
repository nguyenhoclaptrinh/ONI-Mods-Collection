using System;
using UnityEngine;

public class Chore<StateMachineInstanceType> : StandardChoreBase, IStateMachineTarget where StateMachineInstanceType : StateMachine.Instance
{
	private int onTargetDestroyedHandlerID;

	public StateMachineInstanceType smi { get; protected set; }

	public override GameObject gameObject => target.gameObject;

	public Transform transform => target.gameObject.transform;

	public string name => gameObject.name;

	public override bool isNull => target.isNull;

	protected override StateMachine.Instance GetSMI ()
	{
		return smi;
	}

	public int Subscribe (int hash, Action<object> handler)
	{
		return GetComponent<KPrefabID> ().Subscribe (hash, handler);
	}

	public int Subscribe (int hash, Action<object, object> handler, object context)
	{
		return GetComponent<KPrefabID> ().Subscribe (hash, handler, context);
	}

	public void Unsubscribe (int hash, Action<object> handler)
	{
		GetComponent<KPrefabID> ().Unsubscribe (hash, handler);
	}

	public void Unsubscribe (int id)
	{
		GetComponent<KPrefabID> ().Unsubscribe (id);
	}

	public void Unsubscribe (ref int id)
	{
		Unsubscribe (id);
		id = -1;
	}

	public void Trigger (int hash, object data = null)
	{
		GetComponent<KPrefabID> ().Trigger (hash, data);
	}

	public ComponentType GetComponent<ComponentType> ()
	{
		return target.GetComponent<ComponentType> ();
	}

	public Chore (ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, PriorityScreen.PriorityClass master_priority_class = PriorityScreen.PriorityClass.basic, int master_priority_value = 5, bool is_preemptable = false, bool allow_in_context_menu = true, int priority_mod = 0, bool add_to_daily_report = false, ReportManager.ReportType report_type = ReportManager.ReportType.WorkTime)
		: base (chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, master_priority_class, master_priority_value, is_preemptable, allow_in_context_menu, priority_mod, add_to_daily_report, report_type)
	{
		onTargetDestroyedHandlerID = target.Subscribe (1969584890, OnTargetDestroyed);
		reportType = report_type;
		addToDailyReport = add_to_daily_report;
		if (addToDailyReport) {
			ReportManager.Instance.ReportValue (ReportManager.ReportType.ChoreStatus, 1f, chore_type.Name, GameUtil.GetChoreName (this, null));
		}
	}

	public override string ResolveString (string str)
	{
		if (!target.isNull) {
			str = str.Replace ("{Target}", target.gameObject.GetProperName ());
		}
		return base.ResolveString (str);
	}

	public override void Cleanup ()
	{
		base.Cleanup ();
		if (target != null) {
			target.Unsubscribe (ref onTargetDestroyedHandlerID);
		}
		if (onCleanup != null) {
			onCleanup (this);
		}
	}

	private void OnTargetDestroyed (object data)
	{
		Cancel ("Target Destroyed");
	}

	public override bool CanPreempt (Precondition.Context context)
	{
		return base.CanPreempt (context);
	}
}
