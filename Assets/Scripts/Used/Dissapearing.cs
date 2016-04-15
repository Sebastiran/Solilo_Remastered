using UnityEngine;
using System.Collections;

public class Dissapearing : MonoBehaviour {
    public int dissapearTime;
    private int timer;
    private int seconds;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        timer++;
        if (seconds >= dissapearTime)
        {
            gameObject.SetActive(false);
        }
        if (timer > 50)
        {
            seconds++;
        }

	}
}
