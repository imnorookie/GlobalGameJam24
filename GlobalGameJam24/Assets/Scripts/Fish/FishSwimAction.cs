using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimAction : MonoBehaviour
{
	const float DIRECTION_LERP_SPEED = 2f;
	const float SPEED_LERP_SPEED = 3f; // eg. 5 = 5m/s per second

	public float Speed = 1f;
	//public float VerticalSpeed = 1f; // when rising or falling (maybe later)
	[Tooltip("Global Y position of the water surface. Fish will not swim above this height.")]
	public float WaterHeight = .3f;
	public float WaterWidth = 7f;

	protected FishController _fishController;
	protected Rigidbody2D _rigidbody2D;
	protected Vector3 _directionNormalized = Vector3.up;
	protected float _lookAngle;
	protected float _lerpFactor;
	protected bool _isOutsideWater;

	private void Awake()
	{
		_fishController = GetComponent<FishController>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		_isOutsideWater =
			transform.position.y > WaterHeight ||
			Mathf.Abs(transform.position.x) > WaterWidth;

		SetGravity();
		Rotate();
	}

	private void SetGravity()
	{
		if (_isOutsideWater)
			_rigidbody2D.gravityScale = 1;
		else
		{
			_rigidbody2D.gravityScale = 0;
		}
	}

	private void Rotate()
	{
		// do not rotate if not swimming
		if (!_fishController.CanSwiw)
			return;

		// rotate fish to face direction of movement
		var vel = _rigidbody2D.velocity.normalized;
		_lookAngle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
		_rigidbody2D.MoveRotation(_lookAngle);
	}

	public void Swim(Vector3 targetPosition)
	{
		// don't swim above water
		if (_isOutsideWater)
			return;

		// move towards target
		var direction = (targetPosition - transform.position).normalized;
		_directionNormalized = Vector3.MoveTowards(_rigidbody2D.velocity.normalized, direction, DIRECTION_LERP_SPEED * Time.fixedDeltaTime); 
		var speed = Mathf.MoveTowards(_rigidbody2D.velocity.magnitude, Speed, SPEED_LERP_SPEED * Time.fixedDeltaTime);
		_rigidbody2D.velocity = _directionNormalized * speed;
	}	
}
