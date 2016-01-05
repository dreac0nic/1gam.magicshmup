using System.Collections;
﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RightClickPuntable : MonoBehaviour
{
	public Camera PickingCamera;
	public float PuntStrength = 50.0f;
	public bool RayDirection = false;

	protected Rigidbody m_Body;

	public void Awake()
	{
		if(!PickingCamera) {
			if(Debug.isDebugBuild) {
				Debug.Log("RightClickPuntable: PickingCamera not sent, using default main camera.");
			}

			PickingCamera = Camera.main;
		}

		m_Body = this.GetComponent<Rigidbody>();
	}

	public void Update()
	{
		if(!m_Body) {
			m_Body = this.GetComponent<Rigidbody>();
		}

		if(Input.GetMouseButton(1)) {
			RaycastHit hit_info;
			Ray ray = PickingCamera.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit_info) && m_Body) {
				RightClickPuntable target = hit_info.collider.GetComponentInParent<RightClickPuntable>();

				if(target && target == this) {
					m_Body.AddForceAtPosition(PuntStrength*(RayDirection ? ray.direction : -1*hit_info.normal), hit_info.point);
				}
			}
		}
	}
}
