using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

	public Transform boy;
	
	void Update () {
        this.transform.position = new Vector3(boy.transform.position.x, this.transform.position.y, this.transform.position.z); ;
	}


}
