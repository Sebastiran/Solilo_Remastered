using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Assets.MxUnity.Physics;

public class OneWayCollision : MonoBehaviour
{
	public UnityEvent onIgnore;
	public UnityEvent onRevert;

	int nTriggerEventsSinceUpdate = 0;
	Trajectory predictedTrajectory;
	bool switchedToTrigger;
	float tAtLastFixedUpdate;

	void FixedUpdate()
	{
		tAtLastFixedUpdate = Time.realtimeSinceStartup;

		if (GetComponent<Collider2D>().isTrigger && !switchedToTrigger)
		{
			if (nTriggerEventsSinceUpdate == 0)
			{
				GetComponent<Collider2D>().isTrigger = false;
				onRevert.Invoke();
			}
			else
				nTriggerEventsSinceUpdate = 0;
		}

		if (switchedToTrigger)
		{
			GetComponent<Rigidbody2D>().position = predictedTrajectory.PositionAtTime(Time.fixedDeltaTime);
			GetComponent<Rigidbody2D>().velocity = predictedTrajectory.VelocityAtTime(Time.fixedDeltaTime);
			switchedToTrigger = false;
			onIgnore.Invoke();
		}

		predictedTrajectory.position = GetComponent<Rigidbody2D>().position;
		predictedTrajectory.velocity = GetComponent<Rigidbody2D>().velocity;
		predictedTrajectory.acceleration = GetComponent<Rigidbody2D>().gravityScale * Physics2D.gravity;
	}

	void Update()
	{
		if (switchedToTrigger)
		{
			float dt = Time.realtimeSinceStartup - tAtLastFixedUpdate;
			GetComponent<Rigidbody2D>().position = predictedTrajectory.PositionAtTime(dt);
			GetComponent<Rigidbody2D>().velocity = predictedTrajectory.VelocityAtTime(dt);
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.isTrigger)
			return;

		nTriggerEventsSinceUpdate++;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (switchedToTrigger)
			return;

		foreach (ContactPoint2D cp in collision.contacts)
			if (cp.normal.y < .707f && GetComponent<Collider2D>().bounds.min.y < cp.point.y)
			{
				GetComponent<Collider2D>().isTrigger = true;
				switchedToTrigger = true;
				break;
			}
	}
}
