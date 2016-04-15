using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Assets.MxUnity.Helpers
{
	public static class PhysicsOps
	{
		public static RaycastHit2D[] SpokeCastAll2D(Vector2 origin, int nRays, float range = float.MaxValue, ArrayOps.Filter<RaycastHit2D> filter = null)
		{
			if (nRays == 0)
				throw new ArgumentException("Number of raycasts cannot be zero.");

			List<RaycastHit2D> hits = new List<RaycastHit2D>();

			for (float currentAngle = 0f; currentAngle < 360f; currentAngle += 360f / nRays)
				hits.AddRange(RaycastAll2D(origin, Quaternion.Euler(0f, 0f, currentAngle) * Vector2.right, range, filter));

			return hits.ToArray();
		}

		public static RaycastHit2D[] RaycastAll2D(Vector2 origin, Vector2 direction, float range = float.MaxValue, ArrayOps.Filter<RaycastHit2D> filter = null)
		{
			RaycastHit2D[] output = Physics2D.RaycastAll(origin, direction, range);

			if (filter != null)
				output = filter(output);

			return output;
		}

		public static RaycastHit2D? Nearest(Vector2 referencePoint, params RaycastHit2D[] hits)
		{
			RaycastHit2D? nearest = null;
			float? distToNearest = null;

			foreach (RaycastHit2D element in hits)
			{
				if (element.collider == null)
					continue;

				float distToElement = (referencePoint - element.point).magnitude;

				if (nearest  == null || distToElement < distToNearest.Value)
				{
					nearest = element;
					distToNearest = distToElement;
				}
			}

			return nearest;
		}

		public static RaycastHit2D? NearestRaycastHit2D(Vector2 referencePoint, Ray2D ray, params Predicate<RaycastHit2D>[] conditions)
		{
			RaycastHit2D[] hits;
			
			hits = Physics2D.RaycastAll(ray.origin, ray.direction);
			hits = ArrayOps.KeepMatches(hits, conditions);

			return Nearest(referencePoint, hits);
		}

		public static RaycastHit2D? LinkedNearestRaycast2D(Vector2[] points, params Predicate<RaycastHit2D>[] raycastHitConditions)
		{
			Vector2? prevPoint = null;

			foreach (Vector2 point in points)
			{
				if (prevPoint != null)
				{
					Ray2D ray = new Ray2D(prevPoint.Value, point - prevPoint.Value);
					RaycastHit2D? hit = NearestRaycastHit2D(ray.origin, ray, raycastHitConditions);

					if (hit != null)
						return hit;
				}

				prevPoint = point;
			}

			return null;
		}
	}
}
