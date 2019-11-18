using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Extensions
{
	static class floatExtensions
	{
		public static float AngleNormalize(this float angle)
		{
			while(angle > 360)
				angle -= 360f;
			while(angle < -360)
				angle += 360f;
			return angle;
		}
	}
}
