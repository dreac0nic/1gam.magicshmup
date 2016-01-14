using UnityEngine;
using System.Collections;

public class MagicMissile : Castable
{
	public GameObject MissilePrefab;
	public float ProjectileForce = 20.0f;
	public float Lifetime = 10.0f;

	public override void Cast()
	{
		if(!IsCoolingDown) {
			m_CooldownTimeout = Time.time + CooldownDuration;

			GameObject fireball = (GameObject)Instantiate(MissilePrefab, this.transform.position + 0.5f*this.transform.TransformDirection(Vector3.forward), this.transform.rotation);
			Rigidbody fireball_body = fireball.GetComponent<Rigidbody>();
			fireball_body.AddForce(ProjectileForce*this.transform.forward, ForceMode.Impulse);

			ImpactableFireball fireball_impactor = fireball.GetComponent<ImpactableFireball>();
			fireball_impactor.SourceObject = this.gameObject;

			Destroy(fireball, Lifetime);
		}
	}
}
