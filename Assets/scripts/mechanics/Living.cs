using System.Collections;
using UnityEngine;

public class Living : MonoBehaviour
{

	[SerializeField] protected float m_Health = 100.0f;
	public float MaximumHealth = 100.0f;
	public float HealthCooldownSpeed = 1.0f;

	public bool IsAlive { get { return m_Health > 0.0f; } }
	public float Health { get { return m_Health; } }

	public void Update()
	{
		if(m_Health > MaximumHealth) {
			m_Health -= Mathf.Clamp(m_Health - MaximumHealth, 0.0f, HealthCooldownSpeed*Time.deltaTime);
		}
	}

	public void ApplyDamage(float damage)
	{
		m_Health -= damage;
	}
}
