using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float Speed = 360f;

    // Update is called once per frame
    void Update()
    {
        //rotate the object z rotation by speed
        transform.Rotate(0, 0, Speed * Time.deltaTime);
        
    }
}
