using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class ForcableHitbox : MonoBehaviour
{
	public float ForceStrength = 50.0f;
	public bool ApplyFromCenter = false;

	protected Collider m_WindBox;

	public void Awake()
	{
		m_WindBox = GetComponent<Collider>();
	}

	public void OnTriggerStay(Collider other)
	{
		Rigidbody body = other.GetComponentInParent<Rigidbody>();
		Vector3 direction = transform.forward;

		if(body) {
			if(ApplyFromCenter) {
				direction = (other.transform.position - transform.position).normalized;
			}

			body.AddForce(ForceStrength*direction);
		}
	}
}
