using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.MxUnity;

public class TrajectoryVisualizer : MonoBehaviour
{
	[SerializeField]
	float maxExtrapolationDeltaT = 1;

	[SerializeField]
	int trajectorySegments = 10;

	[SerializeField]
	Vector2 offset = Vector2.zero;

	public Vector2 Origin
	{
		get { return Position + offset; }
	}

	protected Vector2 Position
	{
		get;
		private set;
	}

	protected Vector2 Velocity
	{
		get;
		private set;
	}

	protected Vector2 Acceleration
	{
		get;
		private set;
	}

	protected float MaxExtrapolationDeltaT
	{
		get { return maxExtrapolationDeltaT; }
	}

	protected int TrajectorySegments
	{
		get { return trajectorySegments; }
	}

	protected Vector2[] TrajectoryCurvePoints()
	{
		return MxArithmetic.CurvePoints(Physics2D.gravity, Velocity, Position + offset, maxExtrapolationDeltaT, trajectorySegments);
	}

	protected virtual void FixedUpdateB()
	{ }

	void OnValidate()
	{
		if (maxExtrapolationDeltaT < 0)
			maxExtrapolationDeltaT = -maxExtrapolationDeltaT;

		if (trajectorySegments == 0)
			trajectorySegments = 1;
		else if (trajectorySegments < 0)
			trajectorySegments = -trajectorySegments;
	}

	void FixedUpdate()
	{
		Velocity = ((Vector2)GetComponent<Rigidbody2D>().position - Position) / Time.fixedDeltaTime;
		Position = GetComponent<Rigidbody2D>().position;

		FixedUpdateB();
	}

	void Update()
	{
		Vector2[] curvePoints = TrajectoryCurvePoints();

		for (int i = 0; i < trajectorySegments - 1; i++)
			if (i % 2 == 0)
				Debug.DrawLine(curvePoints[i], curvePoints[i + 1]);
	}
}
