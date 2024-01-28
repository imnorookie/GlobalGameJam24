using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAISimple : MonoBehaviour
{
	static readonly List<RowAction> RowActions = new List<RowAction>()
	{
		RowAction.Backward,
		RowAction.Forward,
	};
	static readonly List<PushPullAction> PushPullActions = new List<PushPullAction>()
	{
		PushPullAction.Push,
		PushPullAction.Wait,
		PushPullAction.Pull,
		PushPullAction.Wait,
	};

	public bool MirrorXInput = false;
	public TimeRange RowForwardTime;
	public TimeRange RowBackwardTime;
	public TimeRange PushPullTime;
	public TimeRange PullPullWaitTime;

	[Header("Debug")]
	OarController _controller;
	OarController.ControlSetEnum _originalControlSet;
	[SerializeField] RowAction _rowAction = RowActions[0];
	[SerializeField] PushPullAction _pushPullAction = PushPullActions[0];
	int _rowActionIndex = 0;
	int _pushPullActionIndex = 0;
	float rowTimer = 0;
	float pushPullTimer = 0;

	public enum RowAction
	{
		Forward, Backward, Wait
	}
	public enum PushPullAction
	{
		Push, Pull, Wait
	}


	private void Awake()
	{
		_controller = GetComponentInParent<OarController>();
		_controller.ControlSet = OarController.ControlSetEnum.ScriptDriven;

		// init ai state timers
		rowTimer = RowBackwardTime.RandomeTime;
		pushPullTimer = PushPullTime.RandomeTime;
	}
	private void OnDestroy()
	{
		_controller.ControlSet = _originalControlSet;
	}


	private void Update()
	{
		// determine state

		AIActionSimple();
	}

	protected void AIActionSimple()
	{
		DecideRowAction();
		DecidePushPullAction();
		SimpleRow();
		SimplePushPull();
	}

	protected void DecideRowAction()
	{
		if (rowTimer <= 0)
		{
			// change to next action
			_rowActionIndex = (_rowActionIndex + 1) % RowActions.Count;
			_rowAction = RowActions[_rowActionIndex];
			// reset timer
			if (_rowAction == RowAction.Forward)
				rowTimer = RowForwardTime.RandomeTime;
			else
				rowTimer = RowBackwardTime.RandomeTime;
		}
		else
		{
			rowTimer -= Time.deltaTime;
		}
	}

	protected void DecidePushPullAction()
	{
		if (pushPullTimer <= 0)
		{
			// change action
			_pushPullActionIndex = (_pushPullActionIndex + 1) % PushPullActions.Count;
			_pushPullAction = PushPullActions[_pushPullActionIndex];
			// reset timer
			if (_pushPullAction == PushPullAction.Wait)
				pushPullTimer = PullPullWaitTime.RandomeTime;
			else
				pushPullTimer = PushPullTime.RandomeTime;
		}
		else
		{
			pushPullTimer -= Time.deltaTime;
		}
	}

	protected void SimpleRow()
	{
		float rowInput = 0;
		if (_rowAction == RowAction.Forward)
			rowInput = 1;
		else if (_rowAction == RowAction.Backward)
			rowInput = -1;

		if (MirrorXInput)
			rowInput *= -1;

		_controller.SetInputX(rowInput);
	}

	protected void SimplePushPull()
	{
		float pushPullInput = 0;
		if (_pushPullAction == PushPullAction.Push)
			pushPullInput = 1;
		else if (_pushPullAction == PushPullAction.Pull)
			pushPullInput = -1;

		_controller.SetInputY(pushPullInput);
	}

	[System.Serializable]
	public class TimeRange
	{
		public float MinTime;
		public float MaxTime;
		public float RandomeTime => Random.Range(MinTime, MaxTime);
	}
}
