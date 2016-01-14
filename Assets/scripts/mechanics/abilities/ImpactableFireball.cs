using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class ImpactableFireball : MonoBehaviour
{
	public float Damage = 10.0f;
	public GameObject SourceObject;

	public void OnTriggerEnter(Collider offender_collider)
	{
		GameObject offender_object = offender_collider.gameObject;
		Living target_entity = offender_collider.GetComponentInParent<Living>();

		// XXX: DIRTY CHECK TO MAKE SURE FIREBALL DOES NOT HIT SOURCE
		while(offender_object != SourceObject && offender_object.transform.parent) {
			offender_object = offender_object.transform.parent.gameObject;
		}

		if(offender_object == SourceObject) {
			return;
		}

		if(target_entity) {
			target_entity.ApplyDamage(Damage);
		}

		Destroy(this.gameObject);
	}
}
