using UnityEngine;
using System.Collections;

public static class KeyframeOps
{
	public static Vector2 ToVector2(this Keyframe obj)
	{
		return new Vector2(obj.time, obj.value);
	}
}
