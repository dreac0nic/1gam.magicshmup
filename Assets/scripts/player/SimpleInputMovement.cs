using System.Collections;
﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleInputMovement : MonoBehaviour
{
	public float MaximumVelocity = 6.0f;
	public float Acceleration = 24.0f;
	public float Friction = 8.0f;

	public string MovementInput = "Vertical";
	public string StrafeInput = "Horizontal";

	protected Rigidbody m_Body;

	protected Vector3 m_Velocity;

	public void Awake()
	{
		m_Body = GetComponent<Rigidbody>();
	}

	public void FixedUpdate()
	{
		Vector3 movement = new Vector3(Input.GetAxis(StrafeInput), 0.0f, Input.GetAxis(MovementInput));

		if(movement.sqrMagnitude > float.Epsilon) {
			m_Velocity += Mathf.Clamp(MaximumVelocity - m_Velocity.magnitude, 0.0f, Acceleration)*movement.normalized*Time.deltaTime;
		}

		if(m_Velocity.sqrMagnitude > float.Epsilon) {
			m_Velocity += -1*Mathf.Clamp(m_Velocity.magnitude, 0, Acceleration)*m_Velocity.normalized*Time.deltaTime;

			m_Body.MovePosition(this.transform.position + m_Velocity*Time.deltaTime);
		}
	}
}
