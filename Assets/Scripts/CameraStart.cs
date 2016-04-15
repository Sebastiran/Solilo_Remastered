using UnityEngine;
using System.Collections;

public class CameraStart : MonoBehaviour {

	public bool firstJump = true;
	public GameObject Girl;

	void OnTriggerExit2D(Collider2D other)
	{
		if(firstJump)
		{
			if (Girl != null)
			{
				GameObject.Find ("Girl").SetActive(false);
				Girl.SetActive(true);
			}
			firstJump = false;
		}
	}
}
