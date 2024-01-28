using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oar : MonoBehaviour
{
	public OarRowingController OarRowingController;

	public float OarSpeed => OarRowingController.VelocityMagnitude;

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Oar"))
            SoundManager._instance.PlayOarCollisionSFX();
		
		if (col.gameObject.CompareTag("Fish"))
			SoundManager._instance.PlayFishOnOarCollisionSFX();
    }

}
