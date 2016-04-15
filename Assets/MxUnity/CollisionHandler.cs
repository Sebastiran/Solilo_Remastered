using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace Assets.MxUnity
{
	public class CollisionHandler : MonoBehaviour
	{
		public EventMode colliderTypeSelf = EventMode.TriggerOnly;
		public EventMode colliderTypeOther = EventMode.Any;
		public AdditionalOptions additionalOptions;
		public UnityEvent onEnter;
		public UnityEvent onExit;
		public UnityEvent onStay;

		HashSet<GameObject> currentlyStaying = new HashSet<GameObject>();

		static Collider2D currentOtherCollider;
		static EventType currentEventType;
		static ContactType currentContactType;
		public Collision2D currentCollision;

		public static Collider2D CurrentOther
		{
			get
			{
				if (currentOtherCollider == null)
					throw new InvalidOperationException();

				return currentOtherCollider;
			}
			private set
			{
				//// If both are either null or not null..
				//if ((value == null) == (currentOtherCollider == null))
				//	throw new InvalidOperationException("Both are " + (value != null) + ".");

				if (value == null && currentOtherCollider == null)
					throw new InvalidOperationException();

				currentOtherCollider = value;
			}
		}

		public static EventType CurrentEventType
		{
			get
			{
				if (currentOtherCollider == null)
					throw new InvalidOperationException();

				return currentEventType;
			}
		}

		public static ContactType CurrentContactType
		{
			get
			{
				if (currentOtherCollider == null)
					throw new InvalidOperationException();

				return currentContactType;
			}
		}

		public static Collision2D CurrentCollision
		{
			get
			{
				if (CurrentContactType == ContactType.Trigger)
					throw new InvalidOperationException();

				return CurrentCollision;
			}
		}

		public void EnableCollisionWithOther(bool value)
		{
			if (currentOtherCollider == null)
				throw new InvalidOperationException("Needs to be invoked during a collision event.");


			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), currentOtherCollider);
		}

		bool ShouldHandleOnCollision
		{
			//get { return eventMode == (EventMode.Any | EventMode.CollisionOnly) ? true : false; }
			get { return colliderTypeSelf == EventMode.Any || colliderTypeSelf == EventMode.CollisionOnly; }
		}

		bool ShouldHandleOnTrigger
		{
			//get { return eventMode == (EventMode.Any | EventMode.TriggerOnly) ? true : false; }
			get { return colliderTypeSelf == EventMode.Any || colliderTypeSelf == EventMode.TriggerOnly; }
		}

		bool IsValidCollider(Collider2D other)
		{
			if (additionalOptions.targetedTagsOnly)
				if (!additionalOptions.targetedTags.Contains(other.tag))
					return false;

			switch (colliderTypeOther)
			{
				case EventMode.Any:
					return true;

				case EventMode.CollisionOnly:
					return !other.isTrigger;

				case EventMode.TriggerOnly:
					return other.isTrigger;

				default:
					throw new InvalidOperationException();
			}
		}

		void HandleEnter(GameObject obj)
		{
			currentlyStaying.Add(obj);
			onEnter.Invoke();
		}

		void HandleExit(GameObject obj)
		{
			currentlyStaying.Remove(obj);
			onExit.Invoke();
		}

		//void FixedUpdate()
		//{
		//	foreach (GameObject e in currentlyStaying.ToArray())
		//		if (e.Equals(null))
		//		{
		//			currentlyStaying.Remove(e);
		//			HandleExit(e);
		//		}
		//}

		//#region OnCollision
		//void OnCollisionEnter(Collision c)
		//{
		//	if (ShouldHandleOnCollision)
		//		HandleEnter(c.gameObject);
		//}

		//void OnCollisionExit(Collision c)
		//{
		//	if (ShouldHandleOnCollision)
		//		HandleExit(c.gameObject);
		//}

		//void OnCollisionStay(Collision c)
		//{
		//	if (ShouldHandleOnCollision)
		//		onStay.Invoke();
		//}
		//#endregion

		//#region OnTrigger
		//void OnTriggerEnter(Collider other)
		//{
		//	if (ShouldHandleOnTrigger)
		//		HandleEnter(other.gameObject);
		//}

		//void OnTriggerExit(Collider other)
		//{
		//	if (ShouldHandleOnTrigger)
		//		HandleExit(other.gameObject);
		//}

		//void OnTriggerStay(Collider other)
		//{
		//	if (ShouldHandleOnTrigger)
		//		onStay.Invoke();
		//}
		//#endregion

		#region OnCollision2D
		void OnCollisionEnter2D(Collision2D c)
		{
			if (ShouldHandleOnCollision && IsValidCollider(c.collider))
				Handle(EventType.Enter, c.collider, c);
		}

		void OnCollisionExit2D(Collision2D c)
		{
			if (ShouldHandleOnCollision && IsValidCollider(c.collider))
				Handle(EventType.Exit, c.collider, c);
		}

		void OnCollisionStay2D(Collision2D c)
		{
			if (ShouldHandleOnCollision && IsValidCollider(c.collider))
				Handle(EventType.Stay, c.collider, c);
		}
		#endregion

		#region OnTrigger2D
		void OnTriggerEnter2D(Collider2D other)
		{
			if (ShouldHandleOnTrigger && IsValidCollider(other))
				Handle(EventType.Enter, other);
		}

		void OnTriggerExit2D(Collider2D other)
		{
			if (ShouldHandleOnTrigger && IsValidCollider(other))
				Handle(EventType.Exit, other);
		}

		void OnTriggerStay2D(Collider2D other)
		{
			if (ShouldHandleOnTrigger && IsValidCollider(other))
				Handle(EventType.Stay, other);
		}
		#endregion

		void OnDisable()
		{
			if (additionalOptions.checkOnExitWhenDisabled && currentlyStaying.Count > 0)
				onExit.Invoke();

			if (additionalOptions.forgetStayingWhenDisabled)
				currentlyStaying.Clear();
		}

		void OnDestroy()
		{
			if (additionalOptions.checkOnExitWhenDestroyed && currentlyStaying.Count > 0)
				onExit.Invoke();
		}

		void Handle(EventType eventType, Collider2D other, Collision2D collision = null)
		{
			CurrentOther = other;
			currentEventType = eventType;

			if (collision != null)
			{
				currentContactType = ContactType.Collision;
				currentCollision = collision;
			}
			else
				currentContactType = ContactType.Trigger;

			switch (eventType)
			{
				case EventType.Enter:
					HandleEnter(other.gameObject);
					break;

				case EventType.Exit:
					HandleExit(other.gameObject);
					break;

				case EventType.Stay:
					onStay.Invoke();
					break;
			}

			CurrentOther = null;
		}

		[Serializable]
		public class AdditionalOptions
		{
			public bool checkOnExitWhenDestroyed = true;
			public bool checkOnExitWhenDisabled = true;
			public bool forgetStayingWhenDisabled = true;
			public bool targetedTagsOnly = false;
			public string[] targetedTags = new string[0];
		}

		[Serializable]
		public enum EventMode
		{
			Any,
			CollisionOnly,
			TriggerOnly
		}

		public enum EventType
		{
			Enter,
			Exit,
			Stay
		}

		public enum ContactType
		{
			Collision,
			Trigger
		}
	}
}
