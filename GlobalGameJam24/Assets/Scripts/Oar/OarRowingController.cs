using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OarRowingController : MonoBehaviour
{
	public float LerpSpeed = 0.25f;
	public float WaterHeight = 0.5f;
	public float ForceMultiplier = 100f;
	

	[Header("Boat")]
	public Rigidbody2D BoatRigidbody;
	public Vector3 BoatLeftPointLocalOffset;
	public Vector3 BoatRightPointLocalOffset;
	public Vector3 PushForceLeft2Right;
	public Vector3 PushForceRight2Left;

	[SerializeField]
	protected float _underwaterVelocity;
	protected Vector3 _lastPosition;
	protected Vector3 _boatLeftPoint;
	protected Vector3 _boatRightPoint;
	protected Vector3 _pushForce;

	
	public bool IsUnderWater => transform.position.y < WaterHeight;


	private void Update()
	{
		
	}




	private void FixedUpdate()
	{
		UpdateVelocity();

		_boatLeftPoint = BoatRigidbody.transform.TransformPoint(BoatLeftPointLocalOffset);
		_boatRightPoint = BoatRigidbody.transform.TransformPoint(BoatRightPointLocalOffset);

		PushBoat();

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

	private void PushBoat()
	{
		if (_underwaterVelocity == 0)
			return;

		// Push force is applied to the opposite side of the boat
		if (_underwaterVelocity > 0)
			_pushForce = PushForceRight2Left;
		else
			_pushForce = PushForceLeft2Right;

		// Push force is multiplied by the velocity of the oar
		_pushForce *= Mathf.Abs(_underwaterVelocity) * ForceMultiplier;

		// Push force is applied
		BoatRigidbody.AddForceAtPosition(_pushForce, _boatLeftPoint);
	}


	private void OnDrawGizmosSelected()
	{
		Gizmos.color = IsUnderWater ? Color.blue : Color.green;
		Gizmos.DrawLine(new Vector3(-10, WaterHeight, 0), new Vector3(10, WaterHeight, 0));

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + _pushForce);


		if (!Application.isPlaying && BoatRigidbody != null)
		{
			_boatLeftPoint = BoatRigidbody.transform.TransformPoint(BoatLeftPointLocalOffset);
			_boatRightPoint = BoatRigidbody.transform.TransformPoint(BoatRightPointLocalOffset);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(_boatLeftPoint,  .05f);
			Gizmos.DrawSphere(_boatRightPoint, .05f);

			Gizmos.DrawLine(_boatLeftPoint, _boatLeftPoint + PushForceLeft2Right);
			Gizmos.DrawLine(_boatRightPoint, _boatRightPoint + PushForceRight2Left);
		}
	} 
}
