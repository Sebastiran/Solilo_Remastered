using UnityEngine;
using System.Collections;

public static class GizmoOps
{
	public static void DrawDashedLine(Vector3 start, Vector3 end, float dashLength = .1f)
	{
		float lineLength = (start - end).magnitude;
		int nIterations = (int)(lineLength / dashLength);
		Vector3 lineDirection = (end - start).normalized;

		for (int i = 0; i < nIterations; i += 2)
		{
			Vector3 p1 = start + lineDirection * dashLength * i;
			Vector3 p2 = start + lineDirection * dashLength * (i + 1);
			Gizmos.DrawLine(p1, p2);
		}
	}

	public static void DrawDashedVector(Vector3 origin, Vector3 direction, float length = 1f, float dashLength = .1f)
	{
		DrawDashedLine(origin, origin + length * direction, dashLength);
	}
}
