using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag
{
	public static class Unity
	{
		public static System.Globalization.CultureInfo ParseCulture
		{
			get
			{
				if(parseCulture == null)
					parseCulture = System.Globalization.CultureInfo.InvariantCulture;
				return parseCulture;
			}
		}

		static System.Globalization.CultureInfo parseCulture;
	}
}
