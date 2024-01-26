using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OarRowingController : MonoBehaviour
{

	public float LerpSpeed = 0.25f;
	public float WaterHeight = 0.5f;

	[SerializeField]
	protected float _underwaterVelocity;
	protected Vector3 _lastPosition;
	
	public bool IsUnderWater => transform.position.y < WaterHeight;

	private void FixedUpdate()
	{
		UpdateVelocity();

		_lastPosition = transform.position;
	}

	private void UpdateVelocity()
	{
		if (!IsUnderWater)
		{
			_underwaterVelocity = 0;
			return;
		}

		var immediateVelocity = transform.position.x - _lastPosition.x;
		_underwaterVelocity = Mathf.MoveTowards(_underwaterVelocity, immediateVelocity, LerpSpeed * Time.fixedDeltaTime);
	}


	private void OnDrawGizmosSelected()
	{
		Gizmos.color = IsUnderWater ? Color.blue : Color.green;
		Gizmos.DrawLine(new Vector3(-10, WaterHeight, 0), new Vector3(10, WaterHeight, 0));
	} 
}
