using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity
{
	[Serializable]
	public struct BoolVector3
	{
		public bool x;
		public bool y;
		public bool z;

		public BoolVector3(bool x, bool y, bool z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3 MaskedCopy(Vector3 src, Vector3 dest)
		{
			if (x)
				dest.x = src.x;

			if (y)
				dest.y = src.y;

			if (z)
				dest.z = src.z;

			return dest;
		}
	}
}
