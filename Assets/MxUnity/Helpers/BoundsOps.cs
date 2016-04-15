using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity.Helpers
{
	public static class BoundsOps
	{
		public static Bounds Encapsulate2D(params Vector2[] points)
		{
			Vector2 initialExtents = points[1] - points[0];
			Bounds output = new Bounds(points[0], 2f * initialExtents);

			for (int i = 2; i < points.Length; i++)
				output.Encapsulate(points[i]);

			return output;
		}
	}
}
