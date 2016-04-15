using UnityEngine;
using System.Collections;

public class SoloCameraMovement : MonoBehaviour {

	bool start = false;
	Vector3 velocity = new Vector3(3,0,0);
	public GameObject groundDetector;
	float minimalSpeed = 0.8f;

	void Start () {
	}
	
	void Update () {
		if(start)
		{
			float distance = (this.transform.position.x) - groundDetector.transform.position.x;
			float speed = velocity.x / (distance + 5);
			if (speed < minimalSpeed)
				speed = minimalSpeed;
			this.transform.position += velocity * speed * Time.deltaTime;
		}
		else
		{
			if(groundDetector.GetComponent<CameraStart>().firstJump == false)
				start = true;
		}
	}

}
