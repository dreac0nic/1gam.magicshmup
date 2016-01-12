using System.Collections;
﻿using UnityEngine;
﻿using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovementController : MonoBehaviour
{
	[Header("Movement")]
	public Transform CameraBias;
	public float MaximumVelocity = 7.5f;
	public float Acceleration = 20.0f;
	public bool AerialControl = false;
	public float AerialMovementModifier = 0.125f;
	public int NumberOfJumps = 1;
	public float JumpForce = 20.0f;

	[Header("Ground Check")]
	public Transform GroundCheckStart;
	public float GroundCheckDistance = 0.01f;
	public float GroundCheckRadius = 0.5f;
	public Vector3 GroundCheckDirection = Vector3.down;

	[Header("Ground Check Ghosting")]
	public bool EnableGhosting = false;
	public float GhostingDuration = 0.5f;

	[Header("Input Names")]
	public string ForwardAxis = "Vertical";
	public string SideAxis = "Horizontal";
	public string JumpInput = "Jump";

	[Header("Debug Values")]
	public Text DebugText;

	// External player references
	private Rigidbody m_Rigidbody;
	private CapsuleCollider m_Collider;

	protected bool m_HasPassedGroundCheck;
	protected bool m_JumpInput;
	protected int m_JumpCount;
	protected float m_GhostingTimeout;
	protected Vector2 m_MovementInput;
	protected Vector3 m_GroundNormal;

	public bool IsGrounded { get { return m_HasPassedGroundCheck || (EnableGhosting && Time.time < m_GhostingTimeout); } }
	public bool HasPassedGroundCheck { get { return m_HasPassedGroundCheck; } }

	public void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Collider = GetComponent<CapsuleCollider>();

		if(!GroundCheckStart) {
			GroundCheckStart = transform;

			if(Debug.isDebugBuild) {
				Debug.LogWarning("ThirdPersonMovementController: Ground Check Start has not been assigned. Reassigning to object transform. (" + transform.name + ")");
			}
		}
	}

	public void Update()
	{
		// Poll for current input
		m_JumpInput = Input.GetButton(JumpInput);
		m_MovementInput = new Vector2(Input.GetAxis(SideAxis), Input.GetAxis(ForwardAxis));

		if(DebugText) {
			DebugText.text = "Player Grounded: " + IsGrounded + "; " + HasPassedGroundCheck + ", " + (EnableGhosting && Time.time < m_GhostingTimeout);
		}
	}

	public void FixedUpdate()
	{
		RaycastHit hit_info;
		Vector3 bias_forward = Vector3.forward;
		Vector3 bias_right = Vector3.right;

		if(CameraBias) {
			bias_forward = Vector3.ProjectOnPlane(CameraBias.forward, Vector3.up).normalized;
			bias_right = Vector3.ProjectOnPlane(CameraBias.right, Vector3.up).normalized;
		}

		// Perform ground check
		if(Physics.SphereCast(GroundCheckStart.position, GroundCheckRadius, GroundCheckDirection, out hit_info, (0.5f*m_Collider.height - GroundCheckRadius) + GroundCheckDistance)) {
			if(!m_HasPassedGroundCheck) {
				m_HasPassedGroundCheck = true;
				m_JumpCount = 0;
			}

			m_GroundNormal = hit_info.normal;
		} else if(m_HasPassedGroundCheck) {
			m_HasPassedGroundCheck = false;
			m_GroundNormal = Vector3.up;
			m_GhostingTimeout = Time.time + GhostingDuration;
		}

		// J - U - M - P
		// NOTE: MULTIJUMP HAPPENS INSTANTLY, CAN JUMP IN MID-AIR WITH ONLY ONE JUMP
		if(m_JumpInput && ((m_JumpCount == 0 && IsGrounded) || m_JumpCount < NumberOfJumps)) {
			Vector3 jump_direction = this.transform.TransformDirection(Vector3.up);
			m_JumpCount++;

			Debug.Log("Jump!");

			// XXX: Cancel out gravity if we're jumping off-ground.
			if(!HasPassedGroundCheck) {
				Vector3 rb_velocity = m_Rigidbody.velocity; // TODO: Make velocity for gravity in direction of gravity?
				rb_velocity.y = 0.0f;

				m_Rigidbody.velocity = rb_velocity;
				jump_direction = m_GroundNormal;
			}

			m_Rigidbody.AddForce(JumpForce*jump_direction, ForceMode.Impulse); // NOTE: Why not take mass into account? Seems reasonable.
		}

		// Calculate movement base on last polled inputs.
		if(m_MovementInput.sqrMagnitude > float.Epsilon && (HasPassedGroundCheck || AerialControl)) {
			float time_acceleration = Acceleration*Time.deltaTime;
			Vector3 movement = Vector3.ClampMagnitude(m_MovementInput.y*bias_forward + m_MovementInput.x*bias_right, 1.0f);
			Vector3 delta_velocity = MaximumVelocity*Vector3.ProjectOnPlane(movement, m_GroundNormal) - m_Rigidbody.velocity;

			delta_velocity = Mathf.Clamp(delta_velocity.magnitude, -time_acceleration, time_acceleration)*delta_velocity;

			if(!HasPassedGroundCheck) {
				delta_velocity *= AerialMovementModifier;
			}

			m_Rigidbody.AddForce(delta_velocity, ForceMode.VelocityChange);
		}
	}
}
