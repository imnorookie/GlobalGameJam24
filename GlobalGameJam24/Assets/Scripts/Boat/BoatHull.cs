using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatHull : MonoBehaviour
{
	public Rigidbody2D BoatRigidbody;
	public int FishLimit = 3;
	public float WeightPerFish = 5f;
	public float WeightWhenOverloaded = 20f;

	public UnityEvent OnFishEnter;
	public UnityEvent OnFishExit;
	public UnityEvent OnBoatOverloaded;

	[SerializeField]
	protected int _fishCount;
	protected float _originalMass;

	private void Awake()
	{
		_originalMass = BoatRigidbody.mass;
	}

	protected void UpdateBoatWeight()
	{
		if (_fishCount == 0)
		{
			BoatRigidbody.useAutoMass = true;
		}
		else if (_fishCount <= FishLimit)
		{
			BoatRigidbody.useAutoMass = false;
			BoatRigidbody.mass = _originalMass + (_fishCount * WeightPerFish);
			OnBoatOverloaded?.Invoke();
		}
		else
		{
			BoatRigidbody.useAutoMass = false;
			BoatRigidbody.mass = _originalMass + WeightWhenOverloaded;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Fish"))
		{
			_fishCount++;
			UpdateBoatWeight();
			OnFishEnter?.Invoke();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Fish"))
		{
			_fishCount--;
			UpdateBoatWeight();
			OnFishExit?.Invoke();
		}
	}
}
