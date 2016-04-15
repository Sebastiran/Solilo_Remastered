using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity
{
	public static class MxPhysics
	{
		public static Vector2[] DescribeTrajectory(Rigidbody2D rigidbody, Vector2 nettoAcceleration, float maxDeltaT, int nSegments, params Vector2[] velocityAdjustments)
		{
			Vector2 position = rigidbody.position;
			Vector2 velocity = rigidbody.velocity;
			Vector2 acceleration = nettoAcceleration;

			foreach (Vector2 deltaV in velocityAdjustments)
				velocity += deltaV;

			return MxArithmetic.CurvePoints(acceleration, velocity, position, maxDeltaT, nSegments);
		}

		public static Vector2[] DescribeBallisticTrajectory(Rigidbody2D rigidbody, float maxDeltaT, int nSegments, params Vector2[] velocityAdjustments)
		{
			return DescribeTrajectory(rigidbody, rigidbody.gravityScale * Physics2D.gravity, maxDeltaT, nSegments, velocityAdjustments);
		}
	}
}
