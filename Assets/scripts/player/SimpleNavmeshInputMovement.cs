using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleNavmeshInputMovement : MonoBehaviour
{
	public float MaximumVelocity = 6.0f;

	public string HorizontalInput = "Horizontal";
	public string VerticalInput = "Vertical";

	protected NavMeshAgent m_Agent;

	public void Awake()
	{
		m_Agent = this.GetComponent<NavMeshAgent>();
	}

	public void Update()
	{
		if(!m_Agent) {
			m_Agent = this.GetComponent<NavMeshAgent>();
		}
	}

	public void FixedUpdate()
	{
		Vector3 movement = new Vector3(Input.GetAxis(HorizontalInput), 0.0f, Input.GetAxis(VerticalInput));

		if(m_Agent && movement.sqrMagnitude >= float.Epsilon) {
			movement = MaximumVelocity*movement.normalized;

			m_Agent.ResetPath();
			m_Agent.velocity = movement;
		}
	}
}
