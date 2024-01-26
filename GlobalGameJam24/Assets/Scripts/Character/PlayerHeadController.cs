using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHeadController : MonoBehaviour
{
	[Header("Stun")]
	public OarController OarController;

	// TODO: stun animation and such


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Fish"))
		{
			CollisionWithFish(collision);
		}
	}

	private void CollisionWithFish(Collider2D collision)
	{
		var fishController = collision.gameObject.GetComponent<FishController>();

		fishController?.Bite(transform, OarController.StunDuration);

		OarController.Stun();
	}
}
