using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Generic;

namespace Bag.Scripts.Extensions
{
	static class Vector2Extensions
	{
		public static Vector2 ToVector2(this Vector3 value)
		{
			return new Vector2(value.x, value.y);
		}

		public static Vector3 ToVector3(this Vector2 value)
		{
			return new Vector3(value.x, value.y, 0);
		}

		public static Vector3 ToVector3(this Vector2 value, float z)
		{
			return new Vector3(value.x, value.y, z);
		}

		public static Vector2 Rotate(this Vector2 v, float degrees)
		{
			float radians = degrees * Mathf.Deg2Rad;
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);
			return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
		}

		public static Vector2 ToVector2(this Int2 value)
		{
			return new Vector2(value.x, value.y);
		}

		public static Int2 ToInt2(this Vector2 value)
		{
			return new Int2((int)value.x, (int)value.y);
		}

		public static Vector2 Clamp(this Vector2 value, float min, float max)
		{
			return new Vector2(Mathf.Clamp(value.x, min, max), Mathf.Clamp(value.y, min, max));
		}
	}
}
