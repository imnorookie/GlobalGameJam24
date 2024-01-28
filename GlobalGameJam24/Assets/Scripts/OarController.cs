using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OarController : MonoBehaviour
{
	[Header("Oar Controls")]
	[Tooltip("Which player controls this oar. Player 1 uses WASD and controller 2. Player 2 uses arrowKeys and controller 1")]
	public ControlSetEnum ControlSet = ControlSetEnum.Player1;

	//[SerializeField]
	//private KeyCode m_pullOarKey;
	//[SerializeField]
	//private KeyCode m_pushOarKey;
	//[SerializeField]
	//private KeyCode m_rowOarForwardKey;
	//[SerializeField]
	//private KeyCode m_rowOarBackwardKey;

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


	public enum ControlSetEnum
	{
		ScriptDriven,
		Player1,
		Player2,
	}


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
		// get input
		if (ControlSet == ControlSetEnum.Player1)
		{
			_inputX = Input.GetAxis("Player 1 Horizontal");
			_inputY = Input.GetAxis("Player 1 Vertical");
		}
		else if (ControlSet == ControlSetEnum.Player2)
		{
			_inputX = Input.GetAxis("Player 2 Horizontal");
			_inputY = Input.GetAxis("Player 2 Vertical");
		}
    }


	private void FixedUpdate()
	{

		if (IsStunned)
			return;

		// resolve movement
		if (_inputX != 0)
			MoveX();

		if (_inputY != 0)
			MoveY();
	}

	public void SetInputX(float input)
	{
		if (ControlSet == ControlSetEnum.ScriptDriven)
		{
			_inputX = input;
		}
	}
	public void SetInputY(float input)
	{
		if (ControlSet == ControlSetEnum.ScriptDriven)
		{
			_inputY = input;
		}
	}


	/// <summary>
	/// Stuns the character, disables input and movement for a short time
	/// </summary>
	/// <param name="stunDuration"></param>
	public void Stun(Transform head)
	{
		if (_stunCoroutine != null)
			StopCoroutine(_stunCoroutine);

		_stunCoroutine = StartCoroutine(StunCoroutine(head));

		OnStun?.Invoke(); // can't seem to append listeners to this.
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
		// changed how push pull work. 
		// move this _oarPosition 1D float up or down first
		// then calculate the 3D movement from this _oarPosition
		// TODO: when adding acceleration, change how fast _oarPosition changes. 
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


	protected IEnumerator StunCoroutine(Transform head)
	{
		IsStunned = true;

		GameObject stunVFX = VFXManager._instance.PlayStunVFXAtPos(head);

		yield return new WaitForSeconds(StunDuration);

		Destroy(stunVFX);

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
