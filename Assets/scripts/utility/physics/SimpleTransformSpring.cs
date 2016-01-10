using System.Collections;
﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleTransformSpring : MonoBehaviour
{
	public Transform Target;
	public float SpringConstant = 5.0f;

	protected Rigidbody m_Body;

	public void Awake()
	{
		m_Body = this.GetComponent<Rigidbody>();
	}

	public void FixedUpdate()
	{
		if(m_Body) {
			m_Body.AddForce(-1*SpringConstant*(this.transform.position - Target.position));
		}
	}
}
