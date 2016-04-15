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
	//public KeyCode jumpKey;
    public GameObject player;
    int timer;
	int curCourage;
	int powerTime = 0;
	public Vector2 raycastOffset;
	public float rayLength;
	float timerPowerup = 4;

	//[Header("Double Jump")]
	//public int dJumpCost;
	//public float doubleJumpMagnitude = 7f;
	//public KeyCode doubleJumpKey;
	//public UnityEvent onDoubleJump;
	//bool doubleJump = false;

	[Header("Platform Neutralizer")]
	public int neutralCost;
	public KeyCode neutralPlatform;
	public bool activateNeutral = false;
	public UnityEvent onNeutralEnabled;
	public UnityEvent onNeutralDisabled;

	/** This method allows a trigger on another gameObject to enable or disable Double Jump 
	 * if it detects that it is (no longer) in contact with a platform. */
	//public void EnableDoubleJump(bool value)
	//{
	//	doubleJump = value;
	//}

	void FixedUpdate()
	{
		HandleMovement();
	}

	void Update()
	{
		//HandleDoubleJump(); 
		//HandleJumping();
		NeutralPlatform();
	}

	//void HandleJumping()
	//{
	//	if (!Input.GetKeyDown(jumpKey))
	//		return;

	//	/*if (groundDetector.MostRecentEvent != CollisionTrigger2D.Event.Enter)
	//		return;*/

	//	RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)raycastOffset, -Vector2.up, rayLength);

	//	if (hit.collider == null)
	//		return;

	//	rigidbody2D.velocity += Vector2.up * jumpMagnitude;
	//	//doubleJump = true;
	//}

	//void HandleDoubleJump()
	//{ 
	//	if (!Input.GetKeyDown(doubleJumpKey))
	//		return;

	//	if (doubleJump != true)
	//		return;

	//	if (groundDetector.MostRecentEvent == CollisionTrigger2D.Event.Enter)
	//		return;

	//	int courage = player.GetComponent<Courage>().GetCourage();
	//	if (courage < (dJumpCost+1))
	//		return;

  
	//	//rigidbody2D.velocity += Vector2.up * doubleJumpMagnitude;

	//	if (rigidbody2D.velocity.y < doubleJumpMagnitude)
	//		rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, doubleJumpMagnitude);

	//	doubleJump = false;
	//	player.GetComponent<Courage>().SubtractCourage(dJumpCost);
	//	onDoubleJump.Invoke();
	//}

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

	void NeutralPlatform()
	{
		curCourage = player.GetComponent<Courage> ().GetCourage ();
		if (Input.GetKey (neutralPlatform))
		{
			if (curCourage >= neutralCost && activateNeutral == false)
			{
				Debug.Log("Active Neutralpowerup");
				activateNeutral = true;
				player.GetComponent<Courage>().SubtractCourage(neutralCost);
				onNeutralEnabled.Invoke();
			}
		}
		
		if (activateNeutral == true) 
		{
			powerTime++;
			timerPowerup -= Time.deltaTime;
			if (powerTime > 180)
			{
				activateNeutral = false;
				powerTime = 0;
				timerPowerup = 4;
				Debug.Log("Disable powerup");
				onNeutralDisabled.Invoke();
			}
		}
	}

	void OnDrawGizmos()
	{
		Vector2 rayOrigin = (Vector2)transform.position + raycastOffset;

		Gizmos.color = Color.white;
		Gizmos.DrawLine (rayOrigin, rayOrigin - rayLength * Vector2.up);
	}

	public enum Type
	{
		Displacement,
		Acceleration
	}
}
