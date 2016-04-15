using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity.Geometry
{
	[Serializable]
	public struct Line
	{
		public Vector3 start;
		public Vector3 end;

		public Line(Vector3 start, Vector3 end)
		{
			this.start = start;
			this.end = end;
		}

		public Vector3 Delta
		{
			get { return end - start; }
		}

		public Vector3 Direction
		{
			get { return Delta.normalized; }
		}

		public float Angle
		{
			get { return Mathf.Rad2Deg * Mathf.Atan2(Delta.y, Delta.y); }
		}

		public float Length
		{
			get { return Delta.magnitude; }
		}

		public Plane ToPlane2D()
		{
			return new Plane(start, end, Vector3.back);
		}

		public static Plane[] ToPlanes2D(params Line[] lines)
		{
			LinkedList<Plane> output = new LinkedList<Plane>();

			foreach (Line e in lines)
				output.AddLast(e.ToPlane2D());

			return output.ToArray();
		}
	}
}
