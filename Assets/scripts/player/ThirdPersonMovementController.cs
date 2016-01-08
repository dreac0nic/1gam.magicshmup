using System.Collections;
﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovementController : MonoBehaviour
{
	[Header("Movement")]
	public Transform CameraBias;
	public float MaximumVelocity = 15.0f;
	public float Acceleration = 20.0f;
	public bool AerialControl = false;
	public float AerialMovementModifier = 0.125f;

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

	// External player references
	private Rigidbody m_Rigidbody;
	private CapsuleCollider m_Collider;

	protected bool m_HasPassedGroundCheck = false;
	protected float m_GhostingTimeout;
	protected Vector2 m_MovementInput;
	protected Vector3 m_GroundNormal;

	public bool IsGrounded { get { return m_HasPassedGroundCheck || (EnableGhosting && Time.time >= m_GhostingTimeout); } }
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
		m_MovementInput = new Vector2(Input.GetAxis(SideAxis), Input.GetAxis(ForwardAxis));
	}

	public void FixedUpdate()
	{
		RaycastHit hit_info;

		// Perform ground check to test.
		if(Physics.SphereCast(GroundCheckStart.position, GroundCheckRadius, GroundCheckDirection, out hit_info, (0.5f*m_Collider.height - GroundCheckRadius) + GroundCheckDistance)) {
			if(!m_HasPassedGroundCheck) {
				m_HasPassedGroundCheck = true;
			}

			m_GroundNormal = hit_info.normal;
		} else if(!m_HasPassedGroundCheck) {
			m_HasPassedGroundCheck = false;
			m_GroundNormal = Vector3.up;
			m_GhostingTimeout = Time.time + GhostingDuration;
		}
	}
}
