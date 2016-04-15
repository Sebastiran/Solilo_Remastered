using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJumpController : MonoBehaviour
{
	public GroundDetector groundDetector;
	//public TagFilter platformTagFilter;

	[Header("Normal Jump")]
	public KeyCode normalJumpKey;
	public float normalJumpSpeed;

	[Header("Double Jump")]
	public KeyCode doubleJumpKey;
	public float doubleJumpSpeed;
	public float maxSpeedAfterDoubleJump;
	public int doubleJumpCourageCost;
	public UnityEvent onDoubleJump;

	[Header("Debug Monitor (Read-Only)")]
	[SerializeField]
	State state;
	[SerializeField]
	bool groundDetected;
	[SerializeField]
	bool doubleJumpEnabled;

	void OnValidate()
	{
		if (Application.isEditor)
		{
			state = State.InEditor;
			groundDetected = false;
			doubleJumpEnabled = false;
		}

		if (maxSpeedAfterDoubleJump < doubleJumpSpeed)
			maxSpeedAfterDoubleJump = doubleJumpSpeed;

		groundDetector.ValidateFieldValues();
	}

	void FixedUpdate()
	{
		HandleGroundDetection();

		switch (state)
		{
			default:
				state = State.Airborne;
				goto case State.Airborne;

			case State.OnPlatform:
				if (!groundDetected)
				{
					state = State.Airborne;
					doubleJumpEnabled = true;
				}
				break;

			case State.LeavingPlatform:
				if (!groundDetected)
					state = State.Airborne;
				break;

			case State.Airborne:
				if (groundDetected)
				{
					state = State.OnPlatform;
					doubleJumpEnabled = false; // Not necessary, but provides clearer feedback as a read-only inspector field.
				}
				break;
		}

		HandleNormalJump();
		HandleDoubleJump();
	}

	void Update()
	{
		/* Additional calls to jump handle methods for increased responsiveness to user input. */
		HandleNormalJump();
		HandleDoubleJump();
	}

	void HandleNormalJump()
	{
		if (state != State.OnPlatform)
			return;

		if (!Input.GetKeyDown(normalJumpKey))
			return;

		Vector2 currentVelocity = GetComponent<Rigidbody2D>().velocity;

		if (normalJumpSpeed > currentVelocity.y)
			GetComponent<Rigidbody2D>().velocity = new Vector2(currentVelocity.x, normalJumpSpeed);

		state = State.LeavingPlatform;
		doubleJumpEnabled = true;
	}

	void HandleDoubleJump()
	{
		if (state != State.Airborne)
			return;

		if (!doubleJumpEnabled)
			return;

		if (!Input.GetKeyDown(doubleJumpKey))
			return;

		Vector2 currentVelocity = GetComponent<Rigidbody2D>().velocity;

		if (doubleJumpSpeed > currentVelocity.y)
		{
			float newVelocityY;

			newVelocityY = currentVelocity.y + doubleJumpSpeed;
			newVelocityY = Mathf.Clamp(newVelocityY, doubleJumpSpeed, maxSpeedAfterDoubleJump);
			GetComponent<Rigidbody2D>().velocity = new Vector2(currentVelocity.x, newVelocityY);
		}

		doubleJumpEnabled = false;
		onDoubleJump.Invoke();
	}

	void HandleGroundDetection()
	{

		foreach (RaycastHit2D hit in groundDetector.RaycastAll(transform.position))
        {
			groundDetected = true;
			return;
		}

		groundDetected = false;
	}

	[Serializable]
	public class GroundDetector
	{
		public int numberOfRays = 1;
		public Vector2 offset;
		public float spread;
		public float maxRange = 1f;

		public void ValidateFieldValues()
		{
			numberOfRays = Mathf.Clamp(numberOfRays, 1, int.MaxValue);
			spread = Mathf.Clamp(spread, 0f, float.MaxValue);
			maxRange = Mathf.Clamp(maxRange, 0f, float.MaxValue);
		}

		public IEnumerable<RaycastHit2D> RaycastAll(Vector2 origin)
		{
			foreach (Ray2D ray in CreateDetectionRays(origin))
				foreach (RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction, maxRange))
					yield return hit;
		}

		public IEnumerable<Ray2D> CreateDetectionRays(Vector2 origin)
		{
			origin += offset;
			Vector3 lineStart = origin - .5f * spread * Vector2.right;
			Vector3 lineEnd = origin + .5f * spread * Vector2.right;

			for (int i = 0; i < numberOfRays; i++)
			{
				Vector2 rayOrigin = Vector2.Lerp(lineStart, lineEnd, (float)i / numberOfRays);
				Vector2 rayDirection = -Vector2.up;

				yield return new Ray2D(rayOrigin, rayDirection);
			}
		}
	}

	[Serializable]
	public enum State
	{
		InEditor,
		OnPlatform,
		LeavingPlatform,
		Airborne
	}
}
