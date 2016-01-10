using UnityEngine;
using System.Collections;

public class LookTowardsTarget : MonoBehaviour
{
	public Transform Target;
	public float Speed = 5.0f;

	public void Update()
	{
		if(Target) {
			Quaternion new_rotation = Quaternion.LookRotation(Target.position - transform.position);

			transform.rotation = Quaternion.Slerp(transform.rotation, new_rotation, Speed*Time.deltaTime);
		}
	}
}
