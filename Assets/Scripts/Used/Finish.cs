using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {
	public string level;
	
	void OnTriggerEnter2D(Collider2D col) {
		if(col.tag=="Boy")
		{
			Application.LoadLevel(level);
		}
	}
	
}
