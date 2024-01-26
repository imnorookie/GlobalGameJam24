using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
	const float WATER_BOTTOM = -10f;

	public FishTypeEnum FishType;
	public float PathfindCornerDistance = .6f;
	public bool IsLeftToRight;

	[Header("Components")]
	public Transform Model;

	FishSwimAction _fishSwimAction;
	protected List<Vector3> _pathPoints = new List<Vector3>();
	protected int _currentPathIndex = 0;
	protected float _distanceToNextPoint = 0f;


	public enum FishTypeEnum
	{
		Regular,
		//Shark,
		//Whale,
	}

	public Vector3 NextPointPosition => _pathPoints[_currentPathIndex];

	private void Awake()
	{
		_fishSwimAction = GetComponent<FishSwimAction>();
	}
	private void Start()
	{
		if (_pathPoints.Count == 0)
			Initialize(IsLeftToRight);
	}
	private void FixedUpdate()
	{
		// TODO: check if fish is in water
		Pathfind();

		CheckOutOfRange();
	}


	public void Initialize(bool isLeftToRight)
	{
		if (isLeftToRight)
		{
			_pathPoints.Add(FishManager.Instance.SurfacePositionLeft);
			_pathPoints.Add(FishManager.Instance.SurfacePositionRight);
			_pathPoints.Add(FishManager.Instance.SpawnPositionRight);

			Model.localScale = new Vector3(1, 1, 1);
		}
		else
		{
			_pathPoints.Add(FishManager.Instance.SurfacePositionRight);
			_pathPoints.Add(FishManager.Instance.SurfacePositionLeft);
			_pathPoints.Add(FishManager.Instance.SpawnPositionLeft);

			Model.localScale = new Vector3(-1, 1, 1);
		}
	}

	/// <summary>
	/// Pathfind to the next point in the path. Tells the fish to swim.
	/// </summary>
	protected void Pathfind()
	{
		// see if is close enough to the current point
		_distanceToNextPoint = Vector3.Distance(transform.position, NextPointPosition);
		if (_distanceToNextPoint <= PathfindCornerDistance)
		{
			// if so, increment the current path index
			_currentPathIndex++;
			if (_currentPathIndex >= _pathPoints.Count)
			{
				HasReachedEndPoint();
				return;
			}
		}

		// swim to the next point
		_fishSwimAction.Swim(NextPointPosition);
	}

	/// <summary>
	/// Called when fish has reached the end of the path. Tell the fish manager to recycle this fish.
	/// </summary>
	protected void HasReachedEndPoint()
	{
		FishManager.Instance.RecycleFish(this);
	}

	/// <summary>
	/// if the fish falls below the water (out of view) without reaching end point, also recycle it.
	/// </summary>
	protected void CheckOutOfRange()
	{
		if (transform.position.y < WATER_BOTTOM)
		{
			FishManager.Instance.RecycleFish(this);
		}
	}
}
