using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformerAI : MonoBehaviour
{
	public float minJumpImpulseMag = 0.5f; 
	public float maxJumpImpulseMag = 1;
	public float heightTolerance = 0;
	public int numOfImpulseIncrements = 1;
	public TrajectoryIntersectionFinder trajectoryIntersectionFinder = null;

	Vector2 jumpImpulse = Vector2.zero;
	bool standingOnGround;

	public void StandingOnGround(bool value)
	{
		standingOnGround = value;
	}

	void OnValidate()
	{
		if (numOfImpulseIncrements == 0)
			numOfImpulseIncrements = 1;
		else if (numOfImpulseIncrements < 0)
			numOfImpulseIncrements = -numOfImpulseIncrements;
	}

	void FixedUpdate()
	{
		//Debug.Log(GetType().Name + " (" + Time.realtimeSinceStartup + ") -> void FixedUpdate()");
		if (standingOnGround && !trajectoryIntersectionFinder.IsIntersecting)
			PromptJump();

		if (jumpImpulse != Vector2.zero)
		{
			GetComponent<Rigidbody2D>().AddForce(jumpImpulse, ForceMode2D.Impulse);
			jumpImpulse = Vector2.zero;
			StandingOnGround(false);
		}
	}

	void PromptJump()
	{
		if (!standingOnGround)
			return;

		//LinkedList<Vector2> cPoints = new LinkedList<Vector2>();
		Dictionary<Transform, LinkedList<Vector2>> platformHits = new Dictionary<Transform, LinkedList<Vector2>>();

		Vector2 impulseIncrement = Vector2.up * (maxJumpImpulseMag - minJumpImpulseMag) / numOfImpulseIncrements;
		jumpImpulse = Vector2.up * minJumpImpulseMag;

		for (int i = 0; i < numOfImpulseIncrements; i++)
		{
			RaycastHit2D hit;

			if (HasValidTrajectoryIntersectionFollowingImpulse(jumpImpulse, out hit))
			{
				if (!platformHits.ContainsKey(hit.transform))
					platformHits.Add(hit.transform, new LinkedList<Vector2>());

				if (hit.normal.y >= .707f && platformHits[hit.transform].Count < 5)
				{
					Debug.DrawLine(hit.point, hit.point + 10f * hit.normal, Color.yellow, .2f);
					platformHits[hit.transform].AddLast(jumpImpulse);
				}

				//if (HasValidTrajectoryIntersectionFollowingImpulse(jumpImpulse - 1f * impulseIncrement))
				//{
				//	if (HasValidTrajectoryIntersectionFollowingImpulse(jumpImpulse + 1f * impulseIncrement))
				//		return;

				//	cPoints.AddLast(jumpImpulse);
				//}
			}

			jumpImpulse += impulseIncrement;
		}

		LinkedList<Vector2>[] hitArray = new LinkedList<Vector2>[platformHits.Count];
		platformHits.Values.CopyTo(hitArray, 0);

		if (platformHits.Count > 0)
		{
			Vector2 chosenImpulse = hitArray[Random.Range(0, hitArray.Length)].Last.Value;
			jumpImpulse = chosenImpulse;
		}
		else
			jumpImpulse = Vector2.up * minJumpImpulseMag;
	}

	bool HasValidTrajectoryIntersection()
	{
		//Debug.Log(GetType().Name + "bool HasValidTrajectoryIntersection()");
		Vector2 refPoint = trajectoryIntersectionFinder.Origin;
		float minX = refPoint.y - heightTolerance;

		if (trajectoryIntersectionFinder.LastKnownIntersection.point.y < minX)
			return true;
		// else..
		return false;
	}

	bool HasValidTrajectoryIntersectionFollowingImpulse(Vector2 impulse, out RaycastHit2D hit)
	{
		//Debug.Log(GetType().Name + " (" + Time.realtimeSinceStartup + ") -> bool HasValidTrajectoryIntersectionFollowingImpulse(Vector2 impulse, out RaycastHit2D hit)");
		hit = trajectoryIntersectionFinder.IntersectionFollowingImpulse(impulse, NormalPointsUp);

		if (!trajectoryIntersectionFinder.HasIntersectionFollowingImpulse(impulse, NormalPointsUp))
			return false;

		Vector2 refPoint = trajectoryIntersectionFinder.Origin;
		Vector2 intersection = hit.point;
		float minY = refPoint.y - heightTolerance;

		if (hit.normal.y < 0.8f)
			return false;

		if (intersection.y < minY)
			return false;
		
		return true;
	}

	bool NormalPointsUp(RaycastHit2D hit)
	{
		return hit.normal.y > 0f;
	}
}
