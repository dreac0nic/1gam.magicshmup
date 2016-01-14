using System.Collections;
using UnityEngine;

public class Castable : MonoBehaviour
{
	public float CooldownDuration = 5.0f;

	protected float m_CooldownTimeout;

	public bool IsCoolingDown { get { return Time.time < m_CooldownTimeout; } }
	public float CooldownRemaining { get { return Mathf.Clamp(m_CooldownTimeout - Time.time, 0.0f, m_CooldownTimeout); } }
	public float CooldownRemainingNormalized { get { return this.CooldownRemaining/m_CooldownTimeout; } }

	public virtual void Cast()
	{
		if(Time.time >= m_CooldownTimeout) {
			m_CooldownTimeout = Time.time + CooldownDuration;

			if(Debug.isDebugBuild) {
				Debug.Log(this.transform.name + ": CASTED A SPELL!");
			}
		}
	}
}
