using Assets.MxUnity.Geometry;
using Assets.MxUnity.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity.SerializableObjects
{
	[Serializable]
	public class DirectionalRaycaster
	{
		public Line originLine;
		public Transform relativeTo;

		[SerializeField] [Range(1, 1000)]
		int raycastBudget = 25;

		public bool showRays;
		public bool showNormals;
		//public bool showSlopes;

		public Vector2 RelativeLineP1
		{
			get
			{
				if (relativeTo == null)
					return originLine.start;
				else
					return relativeTo.position + relativeTo.rotation * originLine.start;
			}
		}

		public Vector2 RelativeLineP2
		{
			get
			{
				if (relativeTo == null)
					return originLine.end;
				else
					return relativeTo.position + relativeTo.rotation * originLine.end;
			}
		}

		public Vector2 RelativeLineDelta
		{
			get { return RelativeLineP2 - RelativeLineP1; }
		}
		
		public int RaycastBudget
		{
			get { return raycastBudget; }
		}

		bool ShouldShowLines
		{
			get { return showRays || showNormals; }
		}

		public RaycastHit2D[] PerformRaycasts()
		{
			if (ShouldShowLines)
			{
				Ray2D[] mockOutput;
				return PerformRaycasts(out mockOutput);
			}

			List<RaycastHit2D> hits = new List<RaycastHit2D>();

			for (int i = 0; i < raycastBudget; i++)
			{
				Vector2 rayOrigin = RelativeLineP1 + (float)i / raycastBudget * RelativeLineDelta;
				Vector2 rayDirection = Vector3.Cross(RelativeLineDelta, Vector3.forward);

				hits.AddRange(Physics2D.RaycastAll(rayOrigin, rayDirection));
			}

			return hits.ToArray();
		}

		public RaycastHit2D[] PerformRaycasts(out Ray2D[] usedRays)
		{
			Dictionary<RaycastHit2D, Ray2D> hits = new Dictionary<RaycastHit2D, Ray2D>();

			for (int i = 0; i < raycastBudget; i++)
			{
				Vector2 rayOrigin = RelativeLineP1 + (float)i / raycastBudget * RelativeLineDelta;
				Vector2 rayDirection = Vector3.Cross(RelativeLineDelta, Vector3.forward);
				Ray2D ray = new Ray2D(rayOrigin, rayDirection);

				if (showRays)
					Debug.DrawLine(ray.origin, ray.origin + float.MaxValue * ray.direction, new Color(1f, 0f, 0f, .25f));

				foreach (RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction))
				{
					hits.Add(hit, ray);

					if (showNormals && hit.collider != null)
					{
						float r = Mathf.Abs(hit.normal.y);
						float g = Mathf.Abs(hit.normal.x);
						float b = 0f;
						Debug.DrawLine(hit.point, hit.point + hit.normal, new Color(r, g, b));
					}
				}
			}

			usedRays = hits.Values.ToArray();
			return hits.Keys.ToArray();
		}

		public void PerformRaycasts(out Dictionary<Ray2D, RaycastHit2D[]> results)
		{
			results = new Dictionary<Ray2D, RaycastHit2D[]>();

			for (int i = 0; i < raycastBudget; i++)
			{
				Vector2 rayOrigin = RelativeLineP1 + (float)i / raycastBudget * RelativeLineDelta;
				Vector2 rayDirection = Vector3.Cross(RelativeLineDelta, Vector3.forward);
				Ray2D ray = new Ray2D(rayOrigin, rayDirection);
				RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

				results.Add(ray, hits);

				if (showRays)
					Debug.DrawLine(ray.origin, ray.origin + float.MaxValue * ray.direction, new Color(1f, 0f, 0f, .25f));

				if (showNormals)
					foreach (RaycastHit2D hit in hits)
					{
						if (hit.collider == null)
							continue;

						float r = Mathf.Abs(hit.normal.y);
						float g = Mathf.Abs(hit.normal.x);
						float b = 0f;

						Debug.DrawLine(hit.point, hit.point + hit.normal, new Color(r, g, b));
					}
			}
		}
	}
}
