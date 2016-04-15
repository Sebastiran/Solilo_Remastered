using Assets.MxUnity.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity.Physics
{
	[Serializable]
	public struct Trajectory
	{
		public Vector3 position;
		public Vector3 velocity;
		public Vector3 acceleration;

		public LinkedList<Line> AsLines(int nLines, float tStart, float tEnd)
		{
			LinkedList<Line> outputList = new LinkedList<Line>();

			float timeSpan = tEnd - tStart;
			float tBetweenPoints = timeSpan / nLines;

			Vector3 pPrevious = PositionAtTime(tStart);

			for (int i = 1; i <= nLines; i++)
			{
				float tCurrent = tStart + tBetweenPoints * i;
				Vector3 pCurrent = PositionAtTime(tCurrent);

				outputList.AddLast(new Line(pPrevious, pCurrent));
				pPrevious = pCurrent;
			}

			return outputList;
		}

		public Vector3 PositionAtTime(float t)
		{
			Vector3 s = position;
			Vector3 v = velocity;
			Vector3 a = acceleration;

			return s + (v + .5f * a * t) * t;
		}

		public Vector3 VelocityAtTime(float t)
		{
			Vector3 v = velocity;
			Vector3 a = acceleration;

			return v + a * t;
		}
	}
}
