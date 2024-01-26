using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OarController : MonoBehaviour
{
	[Header("Oar Controls")]
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


	[Header("Stun")]
	public bool IsStunned = false;
	[Tooltip("How long the Oar Controller is disbaled from stun")]
	public float StunDuration = 1.5f;
	public UnityEvent OnStun;


	private UnityEngine.Vector2 clampValues;

    protected HingeJoint2D _oarHingeJoint;

	protected float _inputX; // A, D, Left, Right
    protected float _inputY; // W, S, Up, Down

    protected float _oarPosition; // height of the oar (sort of the y-axis)
    protected float _oarPositionLast; // from last frame

	protected Coroutine _stunCoroutine;


	// Start is called before the first frame update
	void Start()
	{
		_oarHingeJoint = m_oarRb.gameObject.GetComponent<HingeJoint2D>();

		UnityEngine.Vector3 startPos = m_oarRb.transform.localPosition;
        clampValues = new UnityEngine.Vector2(startPos.y - m_maxYOffset, startPos.y + m_maxYOffset);

		m_oarRb.centerOfMass = Vector2.zero;
	}

    // Update is called once per frame
    void Update()
    {
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
    }


	private void FixedUpdate()
	{
		// resolve movement
		if (_inputX != 0)
			MoveX();

		if (_inputY != 0)
			MoveY();
	}

	/// <summary>
	/// Stuns the character, disables input and movement for a short time
	/// </summary>
	/// <param name="stunDuration"></param>
	public void Stun(float stunDuration)
	{
		if (_stunCoroutine != null)
			StopCoroutine(_stunCoroutine);

		_stunCoroutine = StartCoroutine(StunCoroutine());

		OnStun?.Invoke();
	}


	/// <summary>
	/// Rotates the oar with _inputX
	/// </summary>
	protected void MoveX()
	{
		m_oarRb.AddTorque(m_rowOarForce * _inputX, ForceMode2D.Impulse);
	}

	/// <summary>
	/// Push and pulls the oar with _inputY
	/// </summary>
	protected void MoveY()
    {
        _oarPosition += _inputY * m_pushPullForce * Time.fixedDeltaTime; 
        _oarPosition = Mathf.Clamp(_oarPosition, -m_maxYOffset, m_maxYOffset);
        float oarPositionDelta = _oarPosition - _oarPositionLast;

        Vector3 relativeUpDirection = m_oarRb.transform.up;
        Vector3 offset = relativeUpDirection * oarPositionDelta;

		// move the oar up and down relative to its rotation so the anchor and center of mass stay in the same place
		m_oarRb.transform.Translate(Vector3.down * oarPositionDelta, Space.Self);

		// move the hinge anchor (local)
		_oarHingeJoint.anchor = new Vector2(_oarHingeJoint.anchor.x, _oarHingeJoint.anchor.y + oarPositionDelta);

		// set the center of mass (local)
        m_oarRb.centerOfMass = new Vector2(m_oarRb.centerOfMass.x, m_oarRb.centerOfMass.y + oarPositionDelta);

		_oarPositionLast = _oarPosition;
	}


	protected IEnumerator StunCoroutine()
	{
		IsStunned = true;
		OnStun?.Invoke();

		yield return new WaitForSeconds(StunDuration);

		IsStunned = false;
	}


	#region Unity Editor
	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, .3f);
        if (m_oarRb != null)
        {
			Gizmos.DrawSphere(m_oarRb.worldCenterOfMass, 0.1f);
		}
	}
    #endregion
}
