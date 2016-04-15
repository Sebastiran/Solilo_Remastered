using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MxUnity
{
	public static class MxArithmetic
	{
		public static void Unsigned(ref float value)
		{
			if (value < 0)
				value = -value;
		}

		public static void Unsigned(ref int value)
		{
			if (value < 0)
				value = -value;
		}

		public static void Clamp(ref float value, float min)
		{
			if (value < min)
				value = min;
		}

		public static void Clamp(ref int value, int min)
		{
			if (value < min)
				value = min;
		}

		public static void Clamp(ref float value, float min, float max)
		{
			Clamp(ref value, min);

			if (value > max)
				value = max;
		}

		public static void Clamp(ref int value, int min, int max)
		{
			Clamp(ref value, min);

			if (value > max)
				value = max;
		}

		public static Vector2[] CurvePoints(Vector2 a, Vector2 b, Vector2 c, float d, int i)
		{
			Vector2[] output = new Vector2[i];
			float d1 = d / i;

			for (int x = 0; x < output.Length; x++)
			{
				float d2 = d1 * x;
				output[x] = c + (b + 0.5f * a * d2) * d2;
			}

			return output;
		}

		public static bool IsValueInRange(float value, float min, float max)
		{
			if (value < min)
				return false;

			if (value > max)
				return false;

			return true;
		}
	}
}
