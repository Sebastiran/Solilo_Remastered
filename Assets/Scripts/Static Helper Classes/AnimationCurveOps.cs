using UnityEngine;
using System.Collections;
using System;

public static class AnimationCurveOps
{
	public static Keyframe PeakKeyframe(this AnimationCurve obj)
	{
		if (obj.length == 0)
			throw new NoKeyframesException();
		else if (obj.length == 1)
			return obj[0];

		Keyframe? highest = null;

		foreach (Keyframe e in obj.keys)
			if (highest == null || e.value > highest.Value.value)
				highest = e;

		return highest.Value;
	}

	public static float PeakValue(this AnimationCurve obj)
	{
		return obj.PeakKeyframe().value;
	}

	public static Vector2 PeakCoordinates(this AnimationCurve obj)
	{
		return obj.PeakKeyframe().ToVector2();
	}

	public static float EvaluateRelativeToPeak(this AnimationCurve obj, float time, float valueAtPeak)
	{
		return valueAtPeak * obj.Evaluate(time) / obj.PeakValue();
	}

	public class NoKeyframesException : Exception { }
}
