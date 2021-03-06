using System.Collections;
﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ThirdPersonMovementController))] // XXX: REALLY BAD PRACTICE A GROUND CHECK COMPONENT MAYBE?
public class ImpulseJumpable : MonoBehaviour
{
	public float JumpForce = 7.5f;
	public float JumpCooldown = 0.25f;
	public float LandingCooldown = 0.3f;
	public string JumpInput = "Jump";

	protected ThirdPersonMovementController m_Player;
	protected Rigidbody m_Rigidbody;

	protected bool m_JumpInput;
	protected float m_JumpTimeout;

	public void Awake()
	{
		m_Player = this.GetComponent<ThirdPersonMovementController>();
		m_Rigidbody = this.GetComponent<Rigidbody>();
	}

	public void Update()
	{
		m_JumpInput = Input.GetButton(JumpInput);
	}

	public void FixedUpdate()
	{
		if(m_JumpInput && m_Player.IsGrounded && Time.time >= m_JumpTimeout && Time.time >= m_Player.LandingTime + LandingCooldown) {
			Vector3 jump_direction = this.transform.TransformDirection(Vector3.up);

			// XXX: Cancel out gravity if we're jumping without ground below us!
			if(!m_Player.HasPassedGroundCheck) {
				Vector3 corrected_velocity = m_Rigidbody.velocity; // TODO: Correct for odd-angle velocities?
				corrected_velocity.y = 0.0f;

				m_Rigidbody.velocity = corrected_velocity;
				jump_direction = m_Player.GroundNormal;
			}

			m_JumpTimeout = Time.time + JumpCooldown;
			m_Rigidbody.AddForce(JumpForce*jump_direction, ForceMode.Impulse); // NOTE: Impulse? Why not just a normal force? Seems to make sense. Insert argument for mass-based movement principles...?
		}
	}
}
