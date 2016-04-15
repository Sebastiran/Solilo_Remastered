using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Assets.MxUnity;

public class CollisionTrigger : MonoBehaviour
{
	public Mode mode = Mode.OnTriggerEnter2D;
	public UnityEvent delegateMethods = null;
	public bool filterOnTag = false;
	public string targetTag = "";

	void OnTriggerEnter2D(Collider2D other)
	{
		if (mode != Mode.OnTriggerEnter2D)
			return;

		if (filterOnTag && other.gameObject.tag != targetTag)
			return;

		delegateMethods.Invoke();
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (mode != Mode.OnTriggerExit2D)
			return;

		if (filterOnTag && other.gameObject.tag != targetTag)
			return;

		delegateMethods.Invoke();
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (mode != Mode.OnCollisionEnter2D)
			return;

		if (filterOnTag && coll.gameObject.tag != targetTag)
			return;

		delegateMethods.Invoke();
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if (mode != Mode.OnCollisionExit2D)
			return;

		if (filterOnTag && coll.gameObject.tag != targetTag)
			return;

		delegateMethods.Invoke();
	}

	[System.Serializable]
	public enum Mode
	{
		OnTriggerEnter2D,
		OnTriggerExit2D,
		OnCollisionEnter2D,
		OnCollisionExit2D
	}
}
