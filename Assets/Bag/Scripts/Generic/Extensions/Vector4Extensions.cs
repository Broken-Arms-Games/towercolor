using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Extensions
{
	static class Vector4Extensions
	{
		public static Vector3 ToVector3(this Vector4 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}
	}
}
