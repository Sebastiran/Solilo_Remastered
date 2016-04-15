using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

	public Transform girl;
	Vector3 velocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		velocity = new Vector3 (girl.transform.position.x,this.transform.position.y,this.transform.position.z);
		this.transform.position = velocity;
	}


}
