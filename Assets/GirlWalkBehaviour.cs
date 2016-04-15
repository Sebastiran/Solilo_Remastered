using UnityEngine;
using System.Collections;

public class GirlWalkBehaviour : MonoBehaviour
{
	public AnimationCurve distanceSpeedGraph = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	public float maxAcceleration = 1f;
	public GameObject target;

	void FixedUpdate()
	{
		Vector2 separationFromBoy = target.transform.position - transform.position;
		float targetVX = distanceSpeedGraph.Evaluate(Mathf.Abs(separationFromBoy.x));
		float reqDeltaVX = targetVX - GetComponent<Rigidbody2D>().velocity.x;
		float possibleDeltaVX = Mathf.Clamp(Mathf.Abs(reqDeltaVX), 0f, maxAcceleration / Time.fixedDeltaTime) * Mathf.Sign(reqDeltaVX);

		//rigidbody2D.velocity += new Vector2(1f, .01f) * possibleDeltaVX * Time.fixedDeltaTime;
		GetComponent<Rigidbody2D>().velocity += Vector2.right * possibleDeltaVX * Time.fixedDeltaTime;

		Debug.DrawLine(GetComponent<Rigidbody2D>().position, GetComponent<Rigidbody2D>().position + 10f * Vector2.right * possibleDeltaVX, Color.green);
	}
}