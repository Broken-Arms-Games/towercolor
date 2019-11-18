using UnityEngine;

namespace Bag.Scripts.Extensions
{
	static class ColorExtensions
	{
		public static Color ToAlpha(this Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}

		public static Vector3 ToHSV(this Color color)
		{
			float H;
			float S;
			float V;
			Color.RGBToHSV(color, out H, out S, out V);
			return new Vector3(H, S, V);
		}
	}
}
