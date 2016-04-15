using UnityEngine;
using System.Collections;

public class SoloCameraMovement : MonoBehaviour {

	bool start = false;
	Vector3 velocity = new Vector3(1,0,0);
	public GameObject character;
	float minSpeed = 5f;
    float maxSpeed = 20f;

    void OnTriggerExit2D(Collider2D other)
    {
        if (!start)
            start = true;
    }

	void Update () {
		if(start)
		{
			float distance = (this.transform.position.x) - character.transform.position.x;
			float speed = velocity.x / (distance + 5);
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
			this.transform.position += velocity * speed * Time.deltaTime;
		}
	}

}
