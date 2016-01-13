using UnityEngine;
using System.Collections;

public class MedkitTarget : FireTargetBase
{
	public float HealthStrength = 15.0f;

	public override void Fire(GameObject offender, TriggerType type = TriggerType.GENERAL)
	{
		Living living_entity = offender.GetComponentInChildren<Living>();

		if(living_entity.IsAlive) {
			living_entity.ApplyDamage(-1*HealthStrength);
			Destroy(this);
		}
	}
}
