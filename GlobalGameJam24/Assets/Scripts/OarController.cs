using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class OarController : MonoBehaviour
{

    [SerializeField]
    private KeyCode m_pullOarKey;

    [SerializeField]
    private KeyCode m_pushOarKey;

    [SerializeField]
    private KeyCode m_rowOarForwardKey;

    [SerializeField]
    private KeyCode m_rowOarBackwardKey;

    [SerializeField]
    private Transform m_oarRotPt;

    [SerializeField]
    private Rigidbody2D m_oarRb;
    
    [SerializeField]
    private float m_pushPullForce; // how much force one key-press pushes/pulls the oar down/up

    [SerializeField]
    private float m_rowOarForce; // how much force one key-press rows

    [SerializeField]
    private float m_maxYOffset;

    private UnityEngine.Vector2 clampValues;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: set oarRB.gameObject.GetComponent<HingeJoint2D>().anchor = m_oarRotPt.position; local
        UnityEngine.Vector3 startPos = m_oarRb.transform.localPosition;
        clampValues = new UnityEngine.Vector2(startPos.y - m_maxYOffset, startPos.y + m_maxYOffset);
    }

    // Update is called once per frame
    void Update()
    {
        // snappy (like below) or weight-y (remove below line)?
        m_oarRb.velocity = UnityEngine.Vector3.zero; 
        m_oarRb.gameObject.GetComponent<HingeJoint2D>().enabled = true;
        m_oarRb.angularVelocity = 0.0f;
        
        UnityEngine.Vector3 pivotPt = m_oarRb.transform.InverseTransformPoint(m_oarRotPt.position);

        // updating the anchor pos here to be congruent to the pivot.
        m_oarRb.gameObject.GetComponent<HingeJoint2D>().anchor =
            new UnityEngine.Vector2(pivotPt.x, pivotPt.y);

        UnityEngine.Vector3 oarLocalPos = m_oarRb.transform.localPosition;
        float rotation = -m_oarRb.transform.localEulerAngles.z * Mathf.Deg2Rad;
        float undoneRotY = oarLocalPos.x * Mathf.Sin(rotation) + oarLocalPos.y * Mathf.Cos(rotation); 

        if (Input.GetKey(m_rowOarBackwardKey)) {
            m_oarRb.AddTorque(-m_rowOarForce, ForceMode2D.Impulse);
            return; // can't row and move at the same time.
        }
        
        if (Input.GetKey(m_rowOarForwardKey)) {
            m_oarRb.AddTorque(m_rowOarForce, ForceMode2D.Impulse);
            return; 
        }

        if (Input.GetKey(m_pushOarKey) && undoneRotY >= clampValues.x) {
            m_oarRb.gameObject.GetComponent<HingeJoint2D>().enabled = false;
            m_oarRb.AddRelativeForce(UnityEngine.Vector3.up * -m_pushPullForce);
        }

        if (Input.GetKey(m_pullOarKey) && undoneRotY <= clampValues.y) {
            m_oarRb.gameObject.GetComponent<HingeJoint2D>().enabled = false;
            m_oarRb.AddRelativeForce(UnityEngine.Vector3.up * m_pushPullForce);
        }
    }
}
