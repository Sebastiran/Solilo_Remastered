using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.MxUnity.Helpers
{
	public static class DictionaryOps
	{
		public static T RandomKey<T, U>(Dictionary<T, U> dictionary)
		{
			return ArrayOps.RandomElement(new List<T>(dictionary.Keys).ToArray());
		}
	}
}
