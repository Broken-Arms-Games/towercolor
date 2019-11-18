using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	[System.Serializable]
	public struct Int2
	{
		public static readonly Int2 zero = new Int2(0, 0);
		public static readonly Int2 one = new Int2(1, 1);
		public static readonly Int2 right = new Int2(1, 0);
		public static readonly Int2 left = new Int2(-1, 0);
		public static readonly Int2 up = new Int2(0, 1);
		public static readonly Int2 down = new Int2(0, -1);
		public static readonly Int2 min = new Int2(int.MinValue, int.MinValue);
		public static readonly Int2 max = new Int2(int.MaxValue, int.MaxValue);

		public int x;
		public int y;

		public Int2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public Int2(Int2 copy)
		{
			x = copy.x;
			y = copy.y;
		}

		public string Log()
		{
			return "(" + x + ", " + y + ")";
		}

		public bool Equals(Int2 a)
		{
			return x == a.x && y == a.y;
		}

		public bool Equals(int x, int y)
		{
			return this.x == x && this.y == y;
		}

		public void Add(Int2 a)
		{
			x += a.x;
			y += a.y;
		}

		public static Int2 operator -(Int2 a, Int2 b)
		{
			Int2 result;
			result.x = a.x - b.x;
			result.y = a.y - b.y;
			return result;
		}

		public static Int2 operator -(int a, Int2 b)
		{
			Int2 result;
			result.x = a - b.x;
			result.y = a - b.y;
			return result;
		}

		public static Int2 operator -(Int2 b)
		{
			Int2 result;
			result.x = -b.x;
			result.y = -b.y;
			return result;
		}

		public static Int2 operator +(Int2 a, Int2 b)
		{
			Int2 result;
			result.x = a.x + b.x;
			result.y = a.y + b.y;
			return result;
		}

		public static string operator +(Int2 a, string b)
		{
			return a.Log() + b;
		}

		public static string operator +(string a, Int2 b)
		{
			return a + b.Log();
		}

		public static Int2 operator *(Int2 a, Int2 b)
		{
			Int2 result;
			result.x = a.x * b.x;
			result.y = a.y * b.y;
			return result;
		}

		public static Int2 operator *(Int2 a, int b)
		{
			Int2 result;
			result.x = a.x * b;
			result.y = a.y * b;
			return result;
		}

		public static Int2 operator /(Int2 a, Int2 b)
		{
			Int2 result;
			result.x = a.x / b.x;
			result.y = a.y / b.y;
			return result;
		}

		public static Int2 operator /(Int2 a, int b)
		{
			Int2 result;
			result.x = a.x / b;
			result.y = a.y / b;
			return result;
		}

		public static bool operator ==(Int2 a, Int2 b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(Int2 a, Int2 b)
		{
			return a.x != b.x || a.y != b.y;
		}

		public override bool Equals(object o)
		{
			if(o == null)
				return false;
			var b = (Int2)o;
			return this == b;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() + y.GetHashCode();
		}

		public static Int2 Min(Int2 a, Int2 b)
		{
			Int2 r = a;
			if(b.x < r.x)
				r.x = b.x;
			if(b.y < r.y)
				r.y = b.y;
			return r;
		}

		public static Int2 Max(Int2 a, Int2 b)
		{
			Int2 r = a;
			if(b.x > r.x)
				r.x = b.x;
			if(b.y > r.y)
				r.y = b.y;
			return r;
		}

		public static Int2 Abs(Int2 a)
		{
			return new Int2(Mathf.Abs(a.x), Mathf.Abs(a.y));
		}

		public static int Distance(Int2 a, Int2 b)
		{
			Int2 delta = Max(a, b) - Min(a, b);
			return delta.x + delta.y;
		}

		public static Int2 PosToGrid(Vector3 pos, float slotSize, bool intYtoPosZ = true)
		{
			if(intYtoPosZ)
				return new Int2(Mathf.FloorToInt(pos.x / slotSize), Mathf.FloorToInt(pos.z / slotSize));
			else
				return new Int2(Mathf.FloorToInt(pos.x / slotSize), Mathf.FloorToInt(pos.y / slotSize));
		}

		public static Vector3 GridToPos(Int2 coords, float slotSize, bool intYtoPosZ = true)
		{
			if(intYtoPosZ)
				return new Vector3(
				coords.x * slotSize, 0,
				coords.y * slotSize);
			else
				return new Vector3(
				coords.x * slotSize,
				coords.y * slotSize, 0);
		}

		public static bool InRange(Int2 point, Int2 coord, int range)
		{
			return InRange(point, coord.x, coord.y, range);
		}

		public static bool InRange(Int2 point, int coordX, int coordY, int range)
		{
			return coordY > point.y - range && coordY < point.y + range &&
					coordX > point.x - range && coordX < point.x + range;
		}
	}


	public static class Int2Extensions
	{
		public static Int2 Min(this Int2[] data)
		{
			Int2 r = Int2.max;
			foreach(Int2 item in data)
				r = Int2.Min(r, item);
			return r;
		}

		public static Int2 Max(this Int2[] data)
		{
			Int2 r = Int2.min;
			foreach(Int2 item in data)
				r = Int2.Max(r, item);
			return r;
		}

		public static Int2 Min(this List<Int2> data)
		{
			Int2 r = Int2.max;
			foreach(Int2 item in data)
				r = Int2.Min(r, item);
			return r;
		}

		public static Int2 Max(this List<Int2> data)
		{
			Int2 r = Int2.min;
			foreach(Int2 item in data)
				r = Int2.Max(r, item);
			return r;
		}

		public static bool Has(this List<Int2> data, Int2 element)
		{
			foreach(Int2 item in data)
				if(item.Equals(element))
					return true;
			return false;
		}

		public static bool Has(this List<Int2> data, int x, int y)
		{
			foreach(Int2 item in data)
				if(item.Equals(x, y))
					return true;
			return false;
		}

		public static bool Fits(this List<Int2> data, int xPos, int yPos, int xSize, int ySize)
		{
			for(int y = yPos; y < yPos + ySize; y++)
				for(int x = xPos; x < xPos + xSize; x++)
					if(data.Has(x, y))
						return false;
			return true;
		}

		public static Int2 Average(this Int2[] array)
		{
			Int2 avrg = Int2.zero;
			for(int i = 0; i < array.Length; i++)
				avrg += array[i];
			return avrg / array.Length;
		}

		public static Int2 Middle(this Int2[] array)
		{
			return (array.Min() + array.Max()) / 2;
		}

		public static Int2[] RotateCW(this Int2[] array, int rotations)
		{
			if(rotations >= 4)
				rotations -= Mathf.FloorToInt(rotations / 4f) * 4;

			Int2[] rotated = new Int2[array.Length];
			for(int i = 0; i < array.Length; i++)
				rotated[i] = new Int2(array[i]);
			for(int i = 0; i < rotated.Length; i++)
				rotated[i] = Rotate(rotated[i], rotations);

			return rotated;
		}

		public static Int2 RotateCW(this Int2 array, int rotations)
		{
			if(rotations >= 4)
				rotations -= Mathf.FloorToInt(rotations / 4f) * 4;
			return Rotate(array, rotations);
		}

		static Int2 Rotate(Int2 coord, int rotations)
		{
			int yToX = 1;
			int xToY = -1;
			Int2 rotated;
			rotated = coord;
			for(int t = 0; t < rotations; t++)
				rotated = new Int2(rotated.y * yToX, rotated.x * xToY);

			return rotated;
		}
	}
}
