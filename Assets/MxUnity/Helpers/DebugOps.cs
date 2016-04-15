using UnityEngine;
using System.Collections;
using System;

namespace Assets.MxUnity.Helpers
{
	public static class DebugOps
	{
		public static void DrawDashedLine(Vector3 start, Vector3 end, float dashLength = .1f, Color? color = null, float? duration = null)
		{
			float lineLength = (start - end).magnitude;
			int nIterations = (int)(lineLength / dashLength);
			Vector3 lineDirection = (end - start).normalized;

			for (int i = 0; i < nIterations; i += 2)
			{
				Vector3 p1 = start + lineDirection * dashLength * i;
				Vector3 p2 = start + lineDirection * dashLength * (i + 1);
				DrawLine(p1, p2, color, duration);
			}
		}

		public static void Visualize(RaycastHit2D hit, Vector2 rayOrigin, RaycastHitColorAssigner rayColorAssigner = null, RaycastHitColorAssigner normalColorAssigner = null, float? visibilityDuration = null, params LineOptions[] options)
		{
			Vector2 p1 = rayOrigin;
			Vector2 p2;
			Color rayColor;
			Color normalColor = normalColorAssigner != null ? normalColorAssigner(hit) : Color.yellow;
			float duration = visibilityDuration != null ? visibilityDuration.Value : 0f;

			if (hit.collider != null)
			{
				p2 = hit.point;
				rayColor = rayColorAssigner != null ? rayColorAssigner(hit) : Color.red;

				Debug.DrawLine(hit.point, hit.point + hit.normal, normalColor, duration);
			}
			else
			{
				p2 = rayOrigin + hit.distance * hit.point;
				rayColor = rayColorAssigner != null ? rayColorAssigner(hit) : Color.green;
			}

			if (ArrayOps.Contains(options, LineOptions.Dashed))
				DrawDashedLine(p1, p2, .1f, rayColor, duration);
			else
				DrawLine(p1, p2, rayColor, duration);
		}

		public static void DrawLine(LineSpecs specs)
		{
			HandleDrawLine(specs.start, specs.end, specs.color, specs.duration, specs.options);
		}

		static void HandleDrawLine(Vector3 start, Vector3 end, Color? color = null, float? duration = null, params LineOptions[] options)
		{
			if (ArrayOps.Contains(options, LineOptions.Dashed))
				DrawDashedLine(start, end, .1f, color, duration);
			else
				DrawLine(start, end, color, duration);
		}

		static void DrawLine(Vector3 start, Vector3 end, Color? color = null, float? duration = null)
		{
			Debug.DrawLine(
				start, 
				end, 
				color != null ? color.Value : Color.white, 
				duration != null ? duration.Value : 0f);
		}

		public delegate Color RaycastHitColorAssigner(RaycastHit2D hit);

		public enum LineOptions
		{
			Dashed
		}

		[Serializable]
		public struct LineSpecs
		{
			public Vector3 start;
			public Vector3 end;
			public Color? color;
			public float? duration;
			public LineOptions[] options;

			public LineSpecs(Vector3 start, Vector3 end, Color? color = null, float? duration = null, params LineOptions[] options)
			{
				this.start = start;
				this.end = end;
				this.color = color;
				this.duration = duration;
				this.options = options;
			}
		}
	}
}
