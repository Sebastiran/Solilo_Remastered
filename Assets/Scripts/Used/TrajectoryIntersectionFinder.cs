using UnityEngine;
using System.Collections;
using Assets.MxUnity;
using Assets.MxUnity.Delegates;

public class TrajectoryIntersectionFinder : TrajectoryVisualizer
{
	[SerializeField]
	bool visualizeTrajectory = false;
	Condition<RaycastHit2D>[] validHitConditions = new Condition<RaycastHit2D>[0];

	public bool IsIntersecting
	{
		get;
		private set;
	}

	public RaycastHit2D LastKnownIntersection
	{
		get;
		private set;
	}

	public bool HasIntersectionFollowingImpulse(Vector2 impulse, params Condition<RaycastHit2D>[] conditions)
	{
		//Debug.Log(GetType().Name + " (" + Time.realtimeSinceStartup + ") -> public bool HasIntersectionFollowingImpulse(Vector2 impulse, params Condition<RaycastHit2D>[] conditions)");
		validHitConditions = conditions;

		Vector2[] curvePoints = MxArithmetic.CurvePoints(Physics2D.gravity, Velocity + impulse, Origin, MaxExtrapolationDeltaT, TrajectorySegments);

		for (int i = 0; i < TrajectorySegments - 1; i++)
		{
			RaycastHit2D hit = Physics2D.Linecast(curvePoints[i], curvePoints[i + 1]);

			if (hit.collider != null && hit.collider.gameObject != gameObject)
			{
				foreach (Condition<RaycastHit2D> meetsCondition in conditions)
					if (!meetsCondition(hit))
						continue;

				return true;
			}
		}

		return false;
	}

	public RaycastHit2D IntersectionFollowingImpulse(Vector2 impulse, params Condition<RaycastHit2D>[] conditions)
	{
		//Debug.Log(GetType().Name + " (" + Time.realtimeSinceStartup + ") -> public RaycastHit2D IntersectionFollowingImpulse(Vector2 impulse, params Condition<RaycastHit2D>[] conditions)");
		validHitConditions = conditions;

		Vector2[] curvePoints = MxArithmetic.CurvePoints(Physics2D.gravity, Velocity + impulse, Origin, MaxExtrapolationDeltaT, TrajectorySegments);

		for (int i = 0; i < TrajectorySegments - 1; i++)
		{
			RaycastHit2D hit = Physics2D.Linecast(curvePoints[i], curvePoints[i + 1]);

			if (visualizeTrajectory && i % 2 == 0)
				Debug.DrawLine(curvePoints[i], curvePoints[i + 1]);

			if (hit.collider != null && hit.collider.gameObject != gameObject)
			{
				foreach (Condition<RaycastHit2D> meetsCondition in conditions)
					if (!meetsCondition(hit))
						continue;

				return hit;
			}
		}

		return LastKnownIntersection;
	}

	void Update()
	{
		//Debug.Log(GetType().Name + " (" + Time.realtimeSinceStartup + ") -> void Update()");
		IsIntersecting = false;

		Vector2[] curvePoints = TrajectoryCurvePoints();

		for (int i = 0; i < TrajectorySegments - 1; i++)
		{
			RaycastHit2D hit = Physics2D.Linecast(curvePoints[i], curvePoints[i + 1]);

			if (hit.collider != null && hit.collider.gameObject != gameObject)
			{
				foreach (Condition<RaycastHit2D> meetsCondition in validHitConditions)
					if (!meetsCondition(hit))
						continue;

				Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
				IsIntersecting = true;
				LastKnownIntersection = hit;
				break;
			}

			if (visualizeTrajectory && i % 2 == 0)
				Debug.DrawLine(curvePoints[i], curvePoints[i + 1]);
		}
	}
}
