using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimAction : MonoBehaviour
{
	const float DIRECTION_CHANGE_SPEED = 1.5f;

	public float Speed = 1f;
	//public float VerticalSpeed = 1f; // when rising or falling (maybe later)

	protected Rigidbody2D _rigidbody2D;
	protected Vector3 _directionNormalized = Vector3.up;
	protected float _lookAngle;
	protected bool _isAboveWater;

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		_isAboveWater = transform.position.y > 0;
		_lookAngle = Mathf.Atan2(_directionNormalized.y, _directionNormalized.x) * Mathf.Rad2Deg;
		_rigidbody2D.MoveRotation(_lookAngle);
	}

	public void Swim(Vector3 targetPosition)
	{
		var direction = (targetPosition - transform.position).normalized;
		_directionNormalized = Vector3.MoveTowards(_directionNormalized,direction, DIRECTION_CHANGE_SPEED * Time.fixedDeltaTime);

		_rigidbody2D.velocity = _directionNormalized * Speed;
	}
}
