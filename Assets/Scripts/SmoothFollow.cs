using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

	public Transform boy;
	
    private void Awake()
    {
        this.transform.position = new Vector3(boy.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    void Update ()
    {
        this.transform.position = new Vector3(boy.transform.position.x, this.transform.position.y, this.transform.position.z);
	}
}
