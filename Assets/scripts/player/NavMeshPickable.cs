using System.Collections;
using UnityEngine;

public class NavMeshPickable : MonoBehaviour
{
	public Camera PickingCamera;
	public LayerMask PickableLayers;

	protected NavMeshAgent m_Agent;

	public void Awake()
	{
		if(!PickingCamera) {
			if(Debug.isDebugBuild) {
				Debug.Log("NavMeshPickable: PickingCamera not sent, using default main camera.");
			}

			PickingCamera = Camera.main;
		}

		m_Agent = this.GetComponent<NavMeshAgent>();
	}

	public void Update()
	{
		if(!m_Agent) {
			m_Agent = this.GetComponent<NavMeshAgent>();
		}

		if(Input.GetMouseButton(0)) {
			RaycastHit hit_info;
			Ray ray = PickingCamera.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit_info, Mathf.Infinity, PickableLayers) && m_Agent) {
				m_Agent.destination = hit_info.point;
			}
		}
	}
}
