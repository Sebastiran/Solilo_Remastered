using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class CameraBoundsHandler : MonoBehaviour
{
	public GameObject player;
	public UnityEvent onOutOfBounds;

	[Header("Debug Monitor (Read-Only)")]
	public Camera trackedCamera;

	void Awake()
	{
		trackedCamera = Camera.main;
		transform.SetParent(trackedCamera.transform);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	void FixedUpdate()
	{
		Bounds boundingBox;
		Vector2 playerPosition;

		boundingBox = new Bounds(transform.position, transform.lossyScale);
		boundingBox.size.Scale(Vector2.one); // 'Flatten' the bounding box.
		playerPosition = player.transform.position;

		/* Return early if player is still within the bounding box. */
		if (boundingBox.Contains(playerPosition))
			return;

		/* Check if player is located left from the bounding box. */
		if (playerPosition.x < boundingBox.min.x)
			goto PlayerIsOutOfBounds;

		/* Check if player is located below the bounding box. */
		if (playerPosition.y < boundingBox.min.y)
			goto PlayerIsOutOfBounds;

		/* Player was not out of bounds. */
		return;

	PlayerIsOutOfBounds:
		onOutOfBounds.Invoke();
	}
}
