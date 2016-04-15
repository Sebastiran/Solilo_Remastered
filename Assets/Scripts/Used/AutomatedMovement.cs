using UnityEngine;
using System.Collections;

public class AutomatedMovement : MonoBehaviour
{
	Transform boy;
	public float minimalSpeed = 3f;
	public Mode mode = Mode.Idle;
	public float moveSpeed = 0;
	public float jumpImpulse = 0;
	float distanceToSpeed = 1.5f;

	public bool JumpOnNextUpdate
	{
		get;
		private set;
	}

	public void PromptJump()
	{
		JumpOnNextUpdate = true;
	}

	void Start ()
	{
		boy = GameObject.FindGameObjectWithTag ("Boy").transform;
	}

	void FixedUpdate()
	{

		Vector2 nettoDisplacement = Vector2.zero;
		Vector2 nettoDeltaV = Vector2.zero;

		switch (mode)
		{
			default:
			case Mode.Idle:
				break;
				
			case Mode.Left:
				nettoDisplacement.x -= moveSpeed;
				break;

			case Mode.Right:
				nettoDisplacement.x += moveSpeed;
				break;
		}

		if (JumpOnNextUpdate)
		{
			JumpOnNextUpdate = false;
			nettoDeltaV.y += jumpImpulse;
		}

		//Zorgt ervoor dat het meisje sneller gaat lopen als de jongen dichter bij komt
		float distance = this.transform.position.x - boy.transform.position.x;
		nettoDisplacement.x = nettoDisplacement.x / (distance - distanceToSpeed);
		//Minimale snelheid voor meisje
		if (nettoDisplacement.x < minimalSpeed)
		{
			nettoDisplacement.x = minimalSpeed;
		}

		GetComponent<Rigidbody2D>().position += nettoDisplacement * Time.fixedDeltaTime;
		GetComponent<Rigidbody2D>().velocity += nettoDeltaV;
	}

	[System.Serializable]
	public enum Mode
	{
		Idle,
		Left,
		Right
	}
}
