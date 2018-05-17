using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPool : MonoBehaviour {

	public void TimeScaleChange()
	{
		EventManager.TriggerEvent ("timeScaleChange");
	}
		

	public void ValueChanged()
	{
		EventManager.TriggerEvent ("reset");
	}

	public void AmplitudeChange()
	{
		EventManager.TriggerEvent ("amplitudeChange");
	}

	public void VelocityChange()
	{
		EventManager.TriggerEvent ("velocityChange");
	}

	public void PeriodChange()
	{
		EventManager.TriggerEvent ("periodChange");
	}

	public void PulsationChange()
	{
		EventManager.TriggerEvent ("pulsationChange");
	}

	public void LambdaChange()
	{
		EventManager.TriggerEvent ("lambdaChange");
	}

	public void RadiusChange()
	{
		EventManager.TriggerEvent ("radiusChange");
	}

	public void InitPhaseChange()
	{
		EventManager.TriggerEvent ("initPhaseChange");
	}

	/*

	


	public void MKOmegaChange()
	{
		EventManager.TriggerEvent ("MKOmegaChange");
	}

	public void XAmplitudeChange()
	{
		EventManager.TriggerEvent ("xAmplitudeChange");
	}

	public void YAmplitudeChange()
	{
		EventManager.TriggerEvent ("yAmplitudeChange");
	}

	public void XPhaseChange()
	{
		EventManager.TriggerEvent ("xPhaseChange");
	}

	public void YPhaseChange()
	{
		EventManager.TriggerEvent ("yPhaseChange");
	}

	public void XPulsationChange()
	{
		EventManager.TriggerEvent ("xPulsationChange");
	}

	public void YPulsationChange()
	{
		EventManager.TriggerEvent ("yPulsationChange");
	}

	public void PhasorReset()
	{
		EventManager.TriggerEvent ("phasorReset");
	}
*/

}
