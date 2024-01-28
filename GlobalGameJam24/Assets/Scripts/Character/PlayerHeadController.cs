using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHeadController : MonoBehaviour
{
	public OarController OarController;
	[Tooltip("Minimum oar moving speed to stun the player.")]
	public float MinimumOarSpeed = 12f;
	public bool DoPrintOarSpeed = false;
	// TODO: stun animation and such


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Fish"))
		{
			CollisionWithFish(collision);
		}
		else if (collision.gameObject.CompareTag("Oar"))
		{
			CollisionWithOar(collision);
		}
		
		if (collision.gameObject.CompareTag("LosingZone")) {
			CollisionWithLosingZone();
		}

	}

	private void CollisionWithFish(Collider2D collision)
	{
		var fishController = collision.gameObject.GetComponent<FishController>();

		fishController?.Bite(transform, OarController.StunDuration);

		SoundManager._instance.PlayFishOnPlayerCollisionSFX();
		OarController.Stun(transform);
	}

	private void CollisionWithOar(Collider2D collision)
	{
		var oar = collision.gameObject.GetComponent<Oar>();
		if (oar == null)
			return;

		var oarSpeed = oar.OarSpeed;
		bool isStunned = oarSpeed > MinimumOarSpeed;

		if (DoPrintOarSpeed)
			Debug.Log($"{(isStunned ? "" : "NOT ")} BONKED WITH SPEED: {oarSpeed.ToString("F2")} / {MinimumOarSpeed}");
        
		if (isStunned) {
			OarController.Stun(transform);
			SoundManager._instance.PlayOarOnPlayerCollisionSFX();
		}
    }

	private void CollisionWithLosingZone() {
		VFXManager._instance.PlayWaterSplashVFXAtPos(transform);
		if (this.CompareTag("P1Head"))
			FindObjectOfType<GameManager>().RoundEnd(0);
		else
			FindObjectOfType<GameManager>().RoundEnd(1);
	}
}
