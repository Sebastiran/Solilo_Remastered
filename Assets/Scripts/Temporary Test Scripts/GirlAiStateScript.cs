using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class GirlAiStateScript : MonoBehaviour
{
	public Text output;
	public float detectionDistance = 1f;
	public Vector2 groundDetectionRayOrigin;
	public Vector2 edgeDetectionRayOrigin;
	public UnityEvent onPlatformEvent;
	public UnityEvent nearingEdgeEvent;
	public UnityEvent midAirEvent;

	State state;

	void FixedUpdate()
	{
		bool validGroundHit = ContainsValidHit(PerformGroundDetection());
		bool validEdgeHit = ContainsValidHit(PerformEdgeDetection());

		if (!validGroundHit)
			state = State.MidAir;
		else if (validEdgeHit)
			state = State.OnPlatform;
		else
			state = State.NearingEdge;

		if (output != null)
			output.text = state.ToString();

		switch (state)
		{
			case State.OnPlatform:
				onPlatformEvent.Invoke();
				break;

			case State.NearingEdge:
				nearingEdgeEvent.Invoke();
				break;

			case State.MidAir:
				midAirEvent.Invoke();
				break;
		}
	}

	void OnDrawGizmosSelected()
	{
		Ray2D groundDetectionRay = CreateGroundDetectionRay();
		Ray2D edgeDetectionRay = CreateEdgeDetectionRay();

		if (ContainsValidHit(Physics2D.RaycastAll(groundDetectionRay.origin, groundDetectionRay.direction, detectionDistance)))
			Gizmos.color = Color.green;
		else
			Gizmos.color = Color.red;

		Gizmos.DrawLine(groundDetectionRay.origin, groundDetectionRay.origin + groundDetectionRay.direction * detectionDistance);

		if (ContainsValidHit(Physics2D.RaycastAll(edgeDetectionRay.origin, edgeDetectionRay.direction, detectionDistance)))
			Gizmos.color = Color.green;
		else
			Gizmos.color = Color.red;

		Gizmos.DrawLine(edgeDetectionRay.origin, edgeDetectionRay.origin + edgeDetectionRay.direction * detectionDistance);
	}

	Ray2D CreateGroundDetectionRay()
	{
		Ray2D ray = new Ray2D();
		ray.origin = transform.position + (Vector3)groundDetectionRayOrigin;
		ray.direction = Vector3.down;
		return ray;
	}

	Ray2D CreateEdgeDetectionRay()
	{
		Ray2D ray = new Ray2D();
		ray.origin = transform.position + (Vector3)edgeDetectionRayOrigin;
		ray.direction = Vector3.down;
		return ray;
	}

	RaycastHit2D[] PerformGroundDetection()
	{
		Ray2D ray = CreateGroundDetectionRay();
		return Physics2D.RaycastAll(ray.origin, ray.direction, detectionDistance);
	}

	RaycastHit2D[] PerformEdgeDetection()
	{
		Ray2D ray = CreateEdgeDetectionRay();
		return Physics2D.RaycastAll(ray.origin, ray.direction, detectionDistance);
	}

	bool ContainsValidHit(RaycastHit2D[] hitArray)
	{
		foreach (RaycastHit2D hit in hitArray)
		{
			// Next hit if other is a trigger.
			if (hit.collider.isTrigger)
				continue;

			// Next hit if has hit this gameobject's collider.
			if (hit.collider == GetComponent<Collider2D>())
				continue;

			// At this point, the hit has passed all above tests.
			return true;
		}

		// At this point, none of the hits have passed all the tests.
		return false;
	}

	[Serializable]
	public enum State
	{
		OnPlatform,
		NearingEdge,
		MidAir
	}
}
