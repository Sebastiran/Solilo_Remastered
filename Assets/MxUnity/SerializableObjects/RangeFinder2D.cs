using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity.SerializableObjects
{
	[Serializable]
	public class RangeFinder2D
	{
		public Vector2 origin;
		public Vector2 direction = Vector2.right;
		public Transform relativeTo;
		[Range(0f, float.MaxValue)]
		public float maxDistance = 10f;
		public bool useTagFilters;
		[SerializeField]
		string[] tagFilters = new string[0];
		public bool showRaycasts;

		public RangeFinder2D(Vector2 origin, Vector2 direction, Transform relativeTo, float maxDistance, params string[] tagFilters)
		{
			this.origin = origin;
			this.direction = direction;
			this.relativeTo = relativeTo;
			this.maxDistance = maxDistance;

			if (tagFilters.Length > 0)
			{
				useTagFilters = true;
				this.tagFilters = tagFilters;
			}
		}

		Vector2 WorldOrigin
		{
			get
			{
				if (relativeTo == null)
					return origin;
				else
					return (Vector2)relativeTo.position + origin;
			}
		}

		Vector2 WorldDirection
		{
			get
			{
				if (direction == Vector2.zero)
					throw new InvalidOperationException("Direction vector is zero.");

				if (relativeTo == null)
					return direction;
				else
					return relativeTo.rotation * direction;
			}
		}

		public bool PerformRaycast(out float range)
		{
			RaycastHit2D[] hits = Physics2D.RaycastAll(WorldOrigin, WorldDirection, maxDistance);
			float? distToNearest = null;

			if (showRaycasts)
				Debug.DrawLine(WorldOrigin, WorldOrigin + maxDistance * WorldDirection.normalized, Color.red);

			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider == null)
					continue;

				if (useTagFilters && !IsFilteredTag(hit.collider.tag))
					continue;

				if (hit.collider.bounds.Contains(WorldOrigin))
					continue;

				distToNearest = Vector3.Distance(WorldOrigin, hit.point);
			}

			if (distToNearest != null)
			{
				range = distToNearest.Value;
				return true;
			}
			else
			{
				range = float.NaN;
				return false;
			}
		}

		public void RefreshTagFilters() // (Makes them unique.)
		{
			tagFilters = new HashSet<string>(tagFilters).ToArray();
		}

		bool IsFilteredTag(string tagValue)
		{
			return tagFilters.Contains(tagValue);
		}
	}
}
