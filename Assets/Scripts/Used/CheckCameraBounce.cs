using UnityEngine;
using System.Collections;

public class CheckCameraBounce : MonoBehaviour {

	new public Transform camera;
	public string level;
	public bool niet;
	int reset = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(niet == false){
			if(reset > 80){
				Dead ();
			}
			else{
				reset++;
			}
		}
	}

	void OnBecameInvisible() {
		niet = false;
	}

	void OnBecameVisible()
	{
		niet = true;
		reset = 0;
	}

	void Dead(){
		Application.LoadLevel (level);
	}
}