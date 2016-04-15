using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionTrigger2DGirl : MonoBehaviour
{
	public Sprite sprite1;
	public Sprite sprite2;
	
	private SpriteRenderer spriteRenderer;

	PlatformBehaviour p;
	public Event MostRecentEvent
	{
		get;
		private set;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		MostRecentEvent = Event.Enter;

		if (other.tag == "Platform")
		{
			//other.renderer.material.color = Color.magenta;
			spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
			ChangeSprite();

			p = other.GetComponent<PlatformBehaviour>();
			p.girlTrue();
		}

		/*
		 * Met opzet disabled, omdat dit een issue met timing gaf nu dat de snelheid 
		 * van de girl kan varieren door de afstand tussen haar en de boy.
		 */
		//if (other.tag == "EndBridge")
		//{
		//	EndElementBridge e = other.GetComponent<EndElementBridge>();
		//	e.girl = true;
		//}
	}

	void ChangeSprite ()
	{
		if (spriteRenderer.sprite == sprite1)
		{
			spriteRenderer.sprite = sprite2;
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
