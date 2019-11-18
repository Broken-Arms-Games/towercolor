using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Extensions
{
	static class DictionaryExtensions
	{
		public static string Log<T, U>(this Dictionary<T, U> d, string keyValueSeparator = " : ")
		{
			if(d == null)
				return "NULL";
			else if(d.Count == 0)
				return "EMPTY";
			else
			{
				string r = "";
				foreach(KeyValuePair<T, U> kv in d)
					r += kv.Key.ToString() + keyValueSeparator + (kv.Value != null ? kv.Value.ToString() : "NULL") + "\n";
				return r.Remove(r.Length - 1, 1);
			}
		}
	}
}
