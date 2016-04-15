using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class CollisionTrigger2D : MonoBehaviour
{
	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite neutralsprite;
	
	private SpriteRenderer spriteRenderer;

	PlatformBehaviour p;
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

			if(other.tag == "Platform")
			{
				spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
				ChangeSprite ();

				GetComponent<AudioSource>().Play();
				p = other.GetComponent<PlatformBehaviour>();
				p.boyTrue();
			}
	}

	void ChangeSprite ()
	{
		if (spriteRenderer.sprite == sprite1)
		{
			spriteRenderer.sprite = sprite2;
		}
	}

	public void ChangeSpriteNeutral ()
	{
		spriteRenderer.sprite = neutralsprite;
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
