using UnityEngine;
using System.Collections;

public class CourageColl : MonoBehaviour {
    public bool used = false;
	// Use this for initialization
	void Awake () {
	    
	}
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boy")
            DoIt();
    }

    void DoIt()
    {

        foreach (CourageMove cm in GetComponentsInChildren<CourageMove>())
        {
            cm.b = false;
        }

        Debug.Log("Doing It");
    }
}
