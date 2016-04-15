using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

public class PlatformerControls : MonoBehaviour
{
	public CollisionTrigger2D groundDetector;
	public Type movementType = Type.Displacement;
	public float moveMagnitude = 1;
	public float jumpMagnitude = 1;
	public float midairMoveMult = 0;
	public KeyCode moveLeftKey;
	public KeyCode moveRightKey;
	public KeyCode jumpKey;
    public GameObject player;
    int timer;
	int powerTime = 0;
	public Vector2 raycastOffset;
	public float rayLength;
	float timerPowerup = 4;

	void FixedUpdate()
	{
		HandleMovement();
	}

	void Update()
	{
		HandleJumping();
	}

	void HandleJumping()
	{
		if (!Input.GetKeyDown(jumpKey))
			return;

		RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)raycastOffset, -Vector2.up, rayLength);
		if (hit.collider == null)
			return;

		rigidbody2D.velocity += Vector2.up * jumpMagnitude;
	}

	void HandleMovement()
	{
		Vector3 movement = Vector3.zero;

		if (Input.GetKey(moveLeftKey))
			movement.x--;

		if (Input.GetKey(moveRightKey))
			movement.x++;

		if (groundDetector.MostRecentEvent != CollisionTrigger2D.Event.Enter)
			movement *= midairMoveMult;

		if (movement == Vector3.zero)
			return;

		movement *= moveMagnitude;

		switch (movementType)
		{
			case Type.Displacement:
				transform.position += movement * Time.fixedDeltaTime;
				break;

			case Type.Acceleration:
				GetComponent<Rigidbody2D>().velocity += new Vector2(movement.x, movement.y);
				break;
		}
	}

	public enum Type
	{
		Displacement,
		Acceleration
	}
}
