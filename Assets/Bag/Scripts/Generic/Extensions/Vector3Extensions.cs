using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Extensions
{
	static class Vector3Extensions
	{
		public static Vector3 ReplaceY(this Vector3 v, float y)
		{
			return new Vector3(v.x, y, v.z);
		}

		public static Vector3 Multiply(this Vector3 v, Vector3 multy)
		{
			return v.Multiply(multy.x, multy.y, multy.z);
		}

		public static Vector3 Multiply(this Vector3 v, float x, float y, float z)
		{
			return new Vector3(v.x * x, v.y * y, v.z * z);
		}

		public static Vector3 RandomOffset(this Vector3 v, float rangeX, float rangeY, float rangeZ)
		{
			return v.RandomOffset(new Vector3(rangeX, rangeY, rangeZ));
		}

		public static Vector3 RandomOffset(this Vector3 v, Vector3 range)
		{
			return v + new Vector3(UnityEngine.Random.Range(-range.x, range.x), UnityEngine.Random.Range(-range.y, range.y), UnityEngine.Random.Range(-range.z, range.z));
		}

		public static Vector4 ToVector4(this Vector3 v, float w = 0)
		{
			return new Vector4(v.x, v.y, v.z, w);
		}
	}
}
