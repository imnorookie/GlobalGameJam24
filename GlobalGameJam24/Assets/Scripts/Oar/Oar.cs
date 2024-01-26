using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oar : MonoBehaviour
{
	public OarRowingController OarRowingController;

	public float OarSpeed => OarRowingController.VelocityMagnitude;
}
