using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity
{
	[Serializable]
	public class CollisionEvent : UnityEngine.Object
	{
		public readonly GameObject objA;
		public readonly GameObject objB;
		public readonly EventType eventType;

		public CollisionEvent(GameObject objA, GameObject objB, EventType eventType)
		{
			this.objA = objA;
			this.objB = objB;
			this.eventType = eventType;
		}

		public enum EventType
		{
			Enter,
			Exit
		}
	}
}
