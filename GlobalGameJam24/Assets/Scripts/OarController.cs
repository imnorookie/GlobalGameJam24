using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class OarController : MonoBehaviour
{

    [SerializeField]
    KeyCode m_pullOarKey;

    [SerializeField]
    KeyCode m_pushOarKey;

    [SerializeField]
    KeyCode m_rowOarForwardKey;

    [SerializeField]
    KeyCode m_rowOarBackwardKey;

    [SerializeField]
    Rigidbody2D m_oarRotRb;

    [SerializeField]
    Rigidbody2D m_oarRb;
    
    [SerializeField]
    float m_pushPullForce; // how much force one key-press pushes/pulls the oar down/up

    [SerializeField]
    float m_rowOarForce; // how much force one key-press rows


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // snappy (like below) or weight-y (remove below line)?
        m_oarRb.velocity = UnityEngine.Vector3.zero; 
        // m_oarRotRb.angularVelocity = 0.0f;


        if (Input.GetKeyDown(m_pushOarKey))
            m_oarRb.AddRelativeForce(UnityEngine.Vector3.up * -m_pushPullForce);

        if (Input.GetKeyDown(m_pullOarKey))
            m_oarRb.AddRelativeForce(UnityEngine.Vector3.up * m_pushPullForce);
    

        if (Input.GetKeyDown(m_rowOarBackwardKey))
            m_oarRotRb.AddTorque(-m_rowOarForce, ForceMode2D.Impulse);
        
        if (Input.GetKeyDown(m_rowOarForwardKey))
            m_oarRotRb.AddTorque(m_rowOarForce, ForceMode2D.Impulse);


    }
}
