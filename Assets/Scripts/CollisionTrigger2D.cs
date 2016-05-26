using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class CollisionTrigger2D : MonoBehaviour
{
	public Event MostRecentEvent
	{
		get;
		set;
	}
	void Start ()
	{
		MostRecentEvent = Event.Exit;
	}
	

	void OnTriggerEnter2D(Collider2D other)
	{
		MostRecentEvent = Event.Enter;

        if (other.tag == "Platform")
        {
            GetComponent<AudioSource>().Play();
        }
    }

	void OnTriggerExit2D(Collider2D other)
	{
		MostRecentEvent = Event.Exit;
	}

	public enum Event
	{
		Enter,
		Exit
	}
}
