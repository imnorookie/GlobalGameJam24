using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHeadController : MonoBehaviour
{
	[Header("Stun")]
	[Tooltip("How long the Oar Controller is disbaled after being hit by a fish")]
	public float StunDuration = .5f;
	public OarController OarController;

	public UnityEvent OnStun;

	protected Coroutine _stunCoroutine;

	// TODO: stun animation and such


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Fish"))
		{
			if (_stunCoroutine != null)
				StopCoroutine(_stunCoroutine);

			_stunCoroutine = StartCoroutine(StunCoroutine());
		}
	}

	protected IEnumerator StunCoroutine()
	{
		OarController.enabled = false;
		OnStun?.Invoke();

		yield return new WaitForSeconds(StunDuration);

		OarController.enabled = true;
	}
}
