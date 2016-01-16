using UnityEngine;
using System.Collections;

public class PulleySystem : MonoBehaviour
{
	public SpringJoint AlphaJoint;
	public SpringJoint BetaJoint;
	public float RopeLength = 5.0f;
	public float RopeSmoothingSpeed = 1.0f;
	public float RopeSmoothingCutoff = float.Epsilon;

	public float m_AlphaLast;
	public float m_BetaLast;

	public void Awake()
	{
		m_AlphaLast = AlphaJoint.transform.InverseTransformPoint(AlphaJoint.connectedBody.transform.position).magnitude - AlphaJoint.maxDistance;
		m_BetaLast = BetaJoint.transform.InverseTransformPoint(BetaJoint.connectedBody.transform.position).magnitude - BetaJoint.maxDistance;
	}

	public void FixedUpdate()
	{
		/*/
		float alpha_current = AlphaJoint.transform.InverseTransformPoint(AlphaJoint.connectedBody.transform.position).magnitude - AlphaJoint.maxDistance;
		float beta_current = BetaJoint.transform.InverseTransformPoint(BetaJoint.connectedBody.transform.position).magnitude - BetaJoint.maxDistance;
		AlphaJoint.maxDistance += m_BetaLast - beta_current;
		BetaJoint.maxDistance += m_AlphaLast - alpha_current;

		m_AlphaLast = alpha_current;
		m_BetaLast = beta_current;
		//*/

		float alpha_displacement = Mathf.Clamp((AlphaJoint.transform.InverseTransformPoint(AlphaJoint.connectedBody.transform.position) - AlphaJoint.anchor + AlphaJoint.connectedAnchor).magnitude - AlphaJoint.maxDistance, 0.0f, RopeLength);
		float beta_displacement = Mathf.Clamp((BetaJoint.transform.InverseTransformPoint(BetaJoint.connectedBody.transform.position) - BetaJoint.anchor + BetaJoint.connectedAnchor).magnitude - BetaJoint.maxDistance, 0.0f, RopeLength);

		m_AlphaLast = (AlphaJoint.transform.InverseTransformPoint(AlphaJoint.connectedBody.transform.position) + AlphaJoint.anchor + AlphaJoint.connectedAnchor).magnitude - AlphaJoint.maxDistance;
		m_BetaLast = (BetaJoint.transform.InverseTransformPoint(BetaJoint.connectedBody.transform.position) + BetaJoint.anchor + BetaJoint.connectedAnchor).magnitude - BetaJoint.maxDistance;

		float filtered_alpha_distance = Mathf.Lerp(AlphaJoint.maxDistance, RopeLength - beta_displacement, RopeSmoothingSpeed*Time.deltaTime);
		float filtered_beta_distance = Mathf.Lerp(BetaJoint.maxDistance, RopeLength - alpha_displacement, RopeSmoothingSpeed*Time.deltaTime);

		if(Mathf.Abs(BetaJoint.maxDistance - filtered_beta_distance) > RopeSmoothingCutoff) {
			BetaJoint.maxDistance = filtered_beta_distance;
			BetaJoint.connectedBody.WakeUp();
		}

		if(Mathf.Abs(AlphaJoint.maxDistance - filtered_alpha_distance) > RopeSmoothingCutoff) {
			AlphaJoint.maxDistance = filtered_alpha_distance;
			AlphaJoint.connectedBody.WakeUp();
		}
	}
}
