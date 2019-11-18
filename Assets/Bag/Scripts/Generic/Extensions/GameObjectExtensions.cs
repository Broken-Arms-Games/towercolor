using System;
using UnityEngine;

namespace Bag.Scripts.Extensions
{
	static class GameObjectExtensions
	{
		public static T[] GetComponentsInALL<T>(this GameObject parent) where T : UnityEngine.Object
		{
			return parent.transform.GetComponentsInALL<T>();
		}

		public static T[] GetComponentsInALL<T>(this GameObject parent, Func<T, bool> filter) where T : UnityEngine.Object
		{
			return parent.transform.GetComponentsInALL<T>(filter);
		}

		public static T[] GetComponentsInALLChildren<T>(this GameObject parent) where T : UnityEngine.Object
		{
			return parent.transform.GetComponentsInALLChildren<T>();
		}

		public static T[] GetComponentsInALLChildren<T>(this GameObject parent, Func<T, bool> filter) where T : UnityEngine.Object
		{
			return parent.transform.GetComponentsInALLChildren<T>(filter);
		}
	}
}
