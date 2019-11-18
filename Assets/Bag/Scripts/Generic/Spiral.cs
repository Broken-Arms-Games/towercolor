using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	public class Spiral
	{
		public int X { get { return c.x; } }
		public int Y { get { return c.y; } }
		public Int2 Coord { get { return c; } }

		Int2 c = Int2.zero;

		public void Next()
		{
			if(c == Int2.zero)
			{
				c = Int2.right;
				return;
			}
			if(Mathf.Abs(c.x) > Mathf.Abs(c.y) + 0.5f * Mathf.Sign(c.x) && Mathf.Abs(c.x) > (-c.y + 0.5f))
				c.y += (int)Mathf.Sign(c.x);
			else
				c.x -= (int)Mathf.Sign(c.y);
		}

		public Int2 NextCoord()
		{
			Next();
			return c;
		}

		public void Reset()
		{
			c = Int2.zero;
		}

		public void SetCoord(int x, int y)
		{
			c = new Int2(x, y);
		}
	}
}
