#define DEBUG
//#undef DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.MxUnity.Helpers
{
	public static class ArrayOps
	{
		public static T[] RemoveDuplicates<T>(T[] array)
		{
			T[] output;
			HashSet<T> set;

			set = new HashSet<T>(array);
			output = new T[set.Count];
			set.CopyTo(output);

			return output;
		}

		public static T[] RemoveMatches<T>(T[] array, params Predicate<T>[] conditions)
		{
			List<T> output = new List<T>(array);

			foreach (T element in array)
			{
				bool metAllConditions = true;

				foreach (Predicate<T> meetsCondition in conditions)
					if (!meetsCondition(element))
					{
						metAllConditions = false;
						break;
					}

				if (metAllConditions)
					output.Remove(element);
			}

			return output.ToArray();
		}

		public static T[] KeepMatches<T>(T[] array, params Predicate<T>[] conditions)
		{
			List<T> output = new List<T>(array);

			foreach (T element in array)
			{
				foreach (Predicate<T> meetsCondition in conditions)
					if (!meetsCondition(element))
					{
						output.Remove(element);
						break;
					}
			}

			return output.ToArray();
		}

		public static bool Contains<T>(T[] array, params T[] instances)
		{
			return new HashSet<T>(array).IsSupersetOf(instances);
		}

		public static T RandomElement<T>(T[] array)
		{
#if DEBUG
			if (array.Length == 0)
			{
				Debug.Log("Array length is zero.");
				Debug.Break();
			}
#endif

			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		public static T[] RemoveElement<T>(T[] array, int index)
		{
			List<T> list = new List<T>(array);
			list.RemoveAt(index);
			return list.ToArray();
		}

		public delegate T[] Filter<T>(T[] array);
	}
}
