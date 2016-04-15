using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity.Helpers
{
	public static class VectorOps
	{
		public static Vector3 Nearest(Vector3 refPoint, params Vector3[] candidates)
		{
			if (candidates.Length == 0)
				throw new ArgumentException();

			Vector3? nearest = null;
			float? distToNearest = null;

			foreach (Vector3 element in candidates)
			{
				float distToElement = Vector3.Distance(refPoint, element);

				if (nearest == null || distToElement < distToNearest.Value)
				{
					nearest = element;
					distToNearest = distToElement;
				}
			}

			return nearest.Value;
		}
	}
}
