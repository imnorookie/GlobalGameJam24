using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Boat"))
            SoundManager._instance.PlayBoatCollisionSFX();
    }


}
