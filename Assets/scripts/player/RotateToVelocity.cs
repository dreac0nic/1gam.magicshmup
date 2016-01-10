using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RotateToVelocity : MonoBehaviour
{
	protected Rigidbody m_Body;

	public void Awake()
	{
		m_Body = this.GetComponent<Rigidbody>();
	}

	public void Update()
	{
		if(m_Body) {
			Vector3 look_direction = Vector3.ProjectOnPlane(m_Body.velocity, Vector3.up).normalized;

			if(look_direction.sqrMagnitude > float.Epsilon) {
				transform.rotation = Quaternion.LookRotation(look_direction);
			}
		}
	}
}
