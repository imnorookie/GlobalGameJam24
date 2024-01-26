using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OarController : MonoBehaviour
{
    [Tooltip("set by player head controller when hit by fish")]
    public bool IsStunned = false;


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

    protected HingeJoint2D _oarHingeJoint;

	protected float _inputX; // A, D, Left, Right
    protected float _inputY; // W, S, Up, Down


    // Start is called before the first frame update
    void Start()
    {
        // TODO: set oarRB.gameObject.GetComponent<HingeJoint2D>().anchor = m_oarRotPt.position; local
        UnityEngine.Vector3 startPos = m_oarRb.transform.localPosition;
        clampValues = new UnityEngine.Vector2(startPos.y - m_maxYOffset, startPos.y + m_maxYOffset);

        _oarHingeJoint = m_oarRb.gameObject.GetComponent<HingeJoint2D>();

		m_oarRb.centerOfMass = Vector2.zero;
	}

    // Update is called once per frame
    void Update()
    {
		//// snappy (like below) or weight-y (remove below line)?
		//// m_oarRb.velocity = UnityEngine.Vector3.zero; 
		//_oarHingeJoint.enabled = true;
  //      m_oarRb.angularVelocity = 0.0f;
            
  //      UnityEngine.Vector3 pivotPt = m_oarRb.transform.InverseTransformPoint(m_oarRotPt.position);

		//// updating the anchor pos here to be congruent to the pivot.
		//_oarHingeJoint.anchor =
  //          new UnityEngine.Vector2(pivotPt.x, pivotPt.y);

  //      UnityEngine.Vector3 oarLocalPos = m_oarRb.transform.localPosition;
  //      float rotation = -m_oarRb.transform.localEulerAngles.z * Mathf.Deg2Rad;
  //      float undoneRotY = oarLocalPos.x * Mathf.Sin(rotation) + oarLocalPos.y * Mathf.Cos(rotation); 

        if (IsStunned)
        	return;

        _inputX = 0;
        _inputY = 0;

        // get input
        if (Input.GetKey(m_rowOarBackwardKey))
            _inputX = -1;
        else if (Input.GetKey(m_rowOarForwardKey))
            _inputX = 1;
        if (Input.GetKey(m_pullOarKey))
			_inputY = -1;
		else if (Input.GetKey(m_pushOarKey))
			_inputY = 1;


        // resolve movement
        if (_inputX != 0)
			MoveX();

        if (_inputY != 0)
            MoveY();


		//if (Input.GetKey(m_rowOarBackwardKey)) {
  //          return; // can't row and move at the same time.
  //      }
        
  //      if (Input.GetKey(m_rowOarForwardKey)) {
  //          m_oarRb.AddTorque(m_rowOarForce, ForceMode2D.Impulse);
  //          return; 
  //      }


  //      if (Input.GetKey(m_pushOarKey) && undoneRotY >= clampValues.x) {
  //          _oarHingeJoint.enabled = false;
  //          m_oarRb.AddRelativeForce(UnityEngine.Vector3.up * -m_pushPullForce);
  //      }

  //      if (Input.GetKey(m_pullOarKey) && undoneRotY <= clampValues.y) {
  //          _oarHingeJoint.enabled = false;
  //          m_oarRb.AddRelativeForce(UnityEngine.Vector3.up * m_pushPullForce);
  //      }
    }

    /// <summary>
    /// Rotates the oar with _inputX
    /// </summary>
    public void MoveX()
	{
		m_oarRb.AddTorque(m_rowOarForce * _inputX, ForceMode2D.Impulse);
	}

    /// <summary>
    /// Push and pulls the oar with _inputY
    /// </summary>
    public void MoveY()
    {

	}
}
