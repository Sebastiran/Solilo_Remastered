using UnityEngine;
using System.Collections;
using System;
using Assets.MxUnity.SerializableObjects;
using System.Collections.Generic;
using Assets.MxUnity.Geometry;
using System.Linq;
using Assets.MxUnity;
using Assets.MxUnity.Helpers;
using UnityEngine.Events;

public class PlatformerAI3 : MonoBehaviour
{
	public RangeFinder2D groundDetector = new RangeFinder2D(Vector2.zero, -Vector2.up, null, .5f, "Platform"); // Used to check if the character is standing on solid ground (and thus for checking if able to walk or jump).
	public DirectionalRaycaster flatSurfaceFinder;
	public Vector2 minVelocity;
	public Vector2 maxVelocity = new Vector2(5f, 20f);
	public bool affectVelocityX = true;
	public bool affectVelocityY = true;
	public ImpulseMode impulseMode = ImpulseMode.Incremental;
	public Vector2 feetPosition;
	public bool showFeetPosition;
	public UnityEvent onJump;

	[SerializeField]
	[Range(1, 1000)]
	int maxTrajectoryEvaluations = 100;

	[SerializeField]
	[Range(0f, 90f)]
	float maxSurfaceSlope = 15f;

	public bool showIdentifiedSurfaces;
	public bool showChosenLandingSites;

	[HideInInspector]
	public float jumpMultiplier = 1f;

	LinkedList<LandingSiteData> storedLandingSiteData = new LinkedList<LandingSiteData>();

	Vector2 RandomValidDeltaV
	{
		get
		{
			Vector2 deltaVelocityBudget;

			deltaVelocityBudget.x = UnityEngine.Random.Range(minVelocity.x, maxVelocity.x);
			deltaVelocityBudget.y = UnityEngine.Random.Range(minVelocity.y, maxVelocity.y);
			deltaVelocityBudget -= GetComponent<Rigidbody2D>().velocity;

			// EXPERIMENTAL
			if (!affectVelocityX)
				deltaVelocityBudget.x = 0f;

			if (!affectVelocityY)
				deltaVelocityBudget.y = 0f;
			// EXPERIMENTAL

			return deltaVelocityBudget;
		}
	}

	void OnValidate()
	{
		groundDetector.RefreshTagFilters();
	}

	void FixedUpdate()
	{
		//float mockOutput;

		//if (rigidbody2D.velocity.y > 0f || !groundDetector.PerformRaycast(out mockOutput))
		//	return;

		Line[] foundSurfaces = IdentifySurfaces();
		Plane[] surfacePlanes = Line.ToPlanes2D(foundSurfaces);
		Vector2? targetDeltaV = null;

		/* Continuously raycast along ballistic trajectories (starting from their peaks) 
		 * derived from randomized velocity changes (within the specified delta-V budget). */
		for (int i = 0; i < maxTrajectoryEvaluations; i++)
		{
			Vector2 deltaV;

			switch (impulseMode)
			{
				case ImpulseMode.Incremental:
					deltaV = minVelocity + (maxVelocity - minVelocity) * i / maxTrajectoryEvaluations;
					break;

				case ImpulseMode.Random:
					deltaV = RandomValidDeltaV;
					break;

				default:
					throw new InvalidOperationException();
			}

			Vector2? prevPoint = null;

			foreach (Vector2 point in MxArithmetic.CurvePoints(GetComponent<Rigidbody2D>().gravityScale * Physics2D.gravity, GetComponent<Rigidbody2D>().velocity + deltaV, GetComponent<Rigidbody2D>().position + feetPosition, 5f, 100))
			{
				if (prevPoint != null && point.y < prevPoint.Value.y)
				{
					Debug.DrawLine(prevPoint.Value, point, Color.red);

					for (int j = 0; j < foundSurfaces.Length; j++)
					{
						Plane plane = surfacePlanes[j];
						Vector2 delta = point - prevPoint.Value;
						Ray ray = new Ray(prevPoint.Value, delta);
						float intersectionDist;

						if (plane.Raycast(ray, out intersectionDist))
						{
							Vector2 landingPoint = prevPoint.Value + intersectionDist * delta.normalized;

							if (MxArithmetic.IsValueInRange(landingPoint.x, foundSurfaces[j].start.x, foundSurfaces[j].end.x) &&
								MxArithmetic.IsValueInRange(intersectionDist, 0f, delta.magnitude))
							{
								targetDeltaV = deltaV;

								//if (showChosenLandingSites)
								//	Debug.DrawLine(landingPoint, (Vector3)landingPoint + Vector3.up, Color.yellow, 1f);

								storedLandingSiteData.AddLast(new LandingSiteData(landingPoint, Vector2.up));

								goto ApplyDeltaV;
							}
						}
					}
				}
				else if (prevPoint != null)
					Debug.DrawLine(prevPoint.Value, point, Color.green);

				prevPoint = point;
			}

			prevPoint = null;
		}

		return;

	ApplyDeltaV:
		//rigidbody2D.velocity += targetDeltaV.Value;
		GetComponent<Rigidbody2D>().velocity += jumpMultiplier * targetDeltaV.Value;
		//jumpMultiplier = 1f;

		onJump.Invoke();
	}

	void OnDrawGizmosSelected()
	{
		float dashLength = flatSurfaceFinder.RelativeLineDelta.magnitude / flatSurfaceFinder.RaycastBudget;

		Gizmos.color = Color.yellow;
		GizmoOps.DrawDashedLine(flatSurfaceFinder.RelativeLineP1, flatSurfaceFinder.RelativeLineP2, dashLength);

		if (showFeetPosition)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position + (Vector3)feetPosition, .125f);
		}

		if (showChosenLandingSites)
		{
			Gizmos.color = Color.yellow;

			foreach (LandingSiteData e in storedLandingSiteData)
				Gizmos.DrawLine(e.position, e.position + e.normal);
		}
	}

	Line[] IdentifySurfaces()
	{
		Dictionary<Ray2D, RaycastHit2D[]> raycastResults;
		List<Line> identifiedSurfaces = new List<Line>();

		flatSurfaceFinder.PerformRaycasts(out raycastResults);

		Ray2D[] usedRays = raycastResults.Keys.ToArray();

		for (int i = 0; i < usedRays.Length - 1; i++)
		{
			foreach (RaycastHit2D hit1 in raycastResults[usedRays[i]])
				foreach (RaycastHit2D hit2 in raycastResults[usedRays[i + 1]])
					if (CalculateSlope(hit1, hit2) <= maxSurfaceSlope)
						identifiedSurfaces.Add(new Line(hit1.point, hit2.point));
		}

		if (showIdentifiedSurfaces)
			foreach (Line surface in identifiedSurfaces)
				Debug.DrawLine(surface.start, surface.end, Color.green);

		return identifiedSurfaces.ToArray();
	}

	//LineStrip[] MergeSurfaces(Line[] surfaces)
	//{


	//	for (int a = 0; a < surfaces.Length - 1; a++)
	//		for (int b = a + 1; b < surfaces.Length; b++)
	//		{
	//			Line lineA = surfaces[a];
	//			Line lineB = surfaces[b];

	//			if (lineA.Equals(lineB))
	//				continue;


	//		}
	//}

	float CalculateSlope(RaycastHit2D hit1, RaycastHit2D hit2)
	{
		float angle = Mathf.Abs(new Line(hit1.point, hit2.point).Angle);

		if (angle < 90f)
			return angle;
		else
			return 180f - angle;
	}

	struct LandingSiteData
	{
		public Vector2 position;
		public Vector2 normal;

		public LandingSiteData(Vector2 position, Vector2 normal)
		{
			this.position = position;
			this.normal = normal;
		}
	}

	[Serializable]
	public enum ImpulseMode
	{
		Incremental,
		Random
	}
}
