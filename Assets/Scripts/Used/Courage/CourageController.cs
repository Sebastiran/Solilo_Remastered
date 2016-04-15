using UnityEngine;
using System.Collections;
using MxUnity;

public class CourageController : MonoBehaviour
{
	[Header("On Awake")]
	public int maxValue;
	//public int startValue;

	[Header("On Fixed Update")]
	public int minValueBeforeDrain;
	public float drainRate; // Units per second.

	[Header("Debug Monitor (Read-Only)")]
	public int currentValue;

	/** Non-serialized fields. */
	float depletedCourageBuffer;

	void Reset()
	{
		maxValue = Courage.MaxValue;
	}

	void OnValidate()
	{
		//if (startValue > maxValue)
		//	startValue = maxValue;

		MathOps.Clamp(ref drainRate, 0f, float.MaxValue);
	}

	void Awake()
	{
		Courage.MaxValue = maxValue;
		//Courage.CurrentValue = startValue;
	}

	void FixedUpdate()
	{
		if (Courage.CurrentValue > minValueBeforeDrain)
		depletedCourageBuffer += drainRate * Time.fixedDeltaTime;

		while (depletedCourageBuffer > 1f)
		{
			depletedCourageBuffer--;
			Courage.CurrentValue--;
		}

		/** Refresh debug monitor values. */
		currentValue = Courage.CurrentValue;
	}
}
