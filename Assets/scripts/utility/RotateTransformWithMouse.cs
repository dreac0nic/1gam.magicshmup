using UnityEngine;
using System.Collections;

public class RotateTransformWithMouse : MonoBehaviour
{
	public float TurnSpeed = 100.0f;
	public string YawInput = "Mouse X";

	public void Update()
	{
		float yaw_influence = Input.GetAxis("Mouse X");

		if(Input.GetMouseButton(2) && Mathf.Abs(yaw_influence) > float.Epsilon) {
			Vector3 rotation = new Vector3(0.0f, yaw_influence, 0.0f);
			this.transform.Rotate(TurnSpeed*rotation*Time.deltaTime);
		}
	}
}
