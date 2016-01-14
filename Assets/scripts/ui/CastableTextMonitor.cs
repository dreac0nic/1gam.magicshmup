using System.Collections;
﻿using UnityEngine;
﻿using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CastableTextMonitor : MonoBehaviour
{
	public Castable Target;

	protected Text m_Readout;

	public void Awake()
	{
		m_Readout = this.GetComponent<Text>();
	}

	public void Update()
	{
		m_Readout.text = Target.CooldownRemaining.ToString("F2");
	}
}
