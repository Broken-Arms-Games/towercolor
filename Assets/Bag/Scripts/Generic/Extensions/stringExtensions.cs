using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Extensions
{
	static class stringExtensions
	{
		public static string ToUppercaseFirst(this string s)
		{
			if(string.IsNullOrEmpty(s))
				return string.Empty;
			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			for(int i = 1; i < a.Length; i++)
				a[i] = char.ToLower(a[i]);
			return new string(a);
		}
	}
}