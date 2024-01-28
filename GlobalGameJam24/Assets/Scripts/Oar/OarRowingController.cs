using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OarRowingController : MonoBehaviour
{
	public float LerpSpeed = 0.25f;
	public float WaterHeight = 0.5f;
	public float ForceMultiplier = 100f;

	// how fast the oar ought to be 4 playing SFX/VFX entering/exiting water
	[SerializeField]
	private float m_minimumOarSplashSpeed = 15f; 

	[Header("Boat")]
	public Rigidbody2D BoatRigidbody;
	public Vector3 BoatLeftPointLocalOffset;
	public Vector3 BoatRightPointLocalOffset;
	public Vector3 PushForceLeft2Right;
	public Vector3 PushForceRight2Left;

	[Header("Events")]
	public UnityEvent OnRowEnterWater;
	public UnityEvent OnRowExitWater;

	[Header("Debug")]
	public bool DoDrawGizmos = false;


	protected float _underwaterVelocity;
	protected Vector3 _generalVelocity;
	protected Vector3 _lastPosition;
	protected Vector3 _boatLeftPoint;
	protected Vector3 _boatRightPoint;
	protected Vector3 _pushForce;
	protected bool _wasUnderWaterLastFrame;
	public bool IsUnderWater => transform.position.y < WaterHeight;
	public float VelocityMagnitude => _generalVelocity.magnitude;

	private void FixedUpdate()
	{
		CheckIfUnderWater();

		UpdateVelocity();

		PushBoat();

		_lastPosition = transform.position;
		_wasUnderWaterLastFrame = IsUnderWater;
	}

	private void CheckIfUnderWater()
	{
		
		bool shouldSplash = VelocityMagnitude > m_minimumOarSplashSpeed / 100f;
		
		if (IsUnderWater && !_wasUnderWaterLastFrame && shouldSplash) {
			SoundManager._instance.PlayOarEnterWaterSFX();
			OnRowEnterWater?.Invoke();
		}
		else if (!IsUnderWater && _wasUnderWaterLastFrame && shouldSplash) {
			SoundManager._instance.PlayOarExitWaterSFX();
			OnRowExitWater?.Invoke();
		}
	}

	private void UpdateVelocity()
	{

		var immediateVelocity = transform.position - _lastPosition;

		_generalVelocity = Vector3.MoveTowards(_generalVelocity, immediateVelocity, LerpSpeed * Time.fixedDeltaTime);

		if (IsUnderWater)
			_underwaterVelocity = Mathf.MoveTowards(_underwaterVelocity, immediateVelocity.x, LerpSpeed * Time.fixedDeltaTime);
		else
			_underwaterVelocity = 0;
	}

	private void PushBoat()
	{
		if (_underwaterVelocity == 0)
			return;

		// Push force is applied to the opposite side of the boat
		if (_underwaterVelocity > 0)
		{
			// Push force is multiplied by the velocity of the oar
			_pushForce = PushForceRight2Left;
			_pushForce *= Mathf.Abs(_underwaterVelocity) * ForceMultiplier;

			// Push force is applied
			_boatLeftPoint = BoatRigidbody.transform.TransformPoint(BoatLeftPointLocalOffset);
			BoatRigidbody.AddForceAtPosition(_pushForce, _boatLeftPoint);
		}
		else
		{
			_pushForce = PushForceLeft2Right;
			_pushForce *= Mathf.Abs(_underwaterVelocity) * ForceMultiplier;
			_boatRightPoint = BoatRigidbody.transform.TransformPoint(BoatRightPointLocalOffset);
			BoatRigidbody.AddForceAtPosition(_pushForce, _boatRightPoint);
		}
	}


	private void OnDrawGizmosSelected()
	{
		if (!DoDrawGizmos)
			return;

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

	public void ZPrintHi()
	{
		Debug.Log("Hi");
	}
}
