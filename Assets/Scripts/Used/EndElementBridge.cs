using UnityEngine;
using System.Collections;

public class EndElementBridge : MonoBehaviour {
	public bool girl;
	int timer = 0;
	
	// Update is called once per frame
	void Update () {
		if (girl == true) 
		{
			timer++;
			if(timer >20){
				//gameObject.SetActive(false);
                Destroy(GetComponent<HingeJoint2D>());
			}
		}
	}

	void girlTrue()
	{
		girl = true;
	}
}
