using System;
using System.Collections.Generic;
using UnityEngine;


namespace Bag.Scripts.Extensions
{
	public static class objectExtensions
	{
		/// <summary>
		/// Parsa l'oggetto ad un int, funziona solo se o.ToString() è un numero. Sto dicendo a te Yves, lo so che hai provato ad usarlo su un DateTime, ho scritto questo commento apposta.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static int ToInt(this object o)
		{
			int r;
			if(int.TryParse(o.ToString(), System.Globalization.NumberStyles.Integer, Bag.Unity.ParseCulture, out r))
				return r;
			else
			{
				Debug.LogError("Object '" + o.ToString() + "' could not be parsed, returning 0.");
				return 0;
			}
		}

		public static double ToDouble(this object o)
		{
			return Convert.ToDouble(o, Bag.Unity.ParseCulture);
		}

		public static Dictionary<K, V> ToDictionary<K, V>(this Dictionary<string, object> o, Func<string, K> keyParser, Func<object, V> valueParser)
		{
			Dictionary<K, V> r = new Dictionary<K, V>();
			foreach(KeyValuePair<string, object> kv in o)
				r.Add(keyParser(kv.Key), valueParser(kv.Value));
			return r;
		}

		public static string JsonSerialize(this object o)
		{
			if(o != null)
			{
				var type = o.GetType();
				// do not use '#' or '!' they are used for dictionaries
				if(type == typeof(int))
					return "i" + ((int)o).ToString(Bag.Unity.ParseCulture);
				else if(type == typeof(float))
					return "f" + ((float)o).ToString(Bag.Unity.ParseCulture);
				else if(type == typeof(double))
					return "d" + ((double)o).ToString(Bag.Unity.ParseCulture);
				else if(type == typeof(string))
					return "s" + o.ToString();
				else if(type == typeof(bool))
					return "b" + o.ToString();
				else
				{
					Debug.LogError("error: object is not a serializable type, add it to the type check ifs.");
					return "e" + o.ToString();
				}
			}
			else
				return "n";
		}

		public static object JsonDeserialize(this string s)
		{
			if(s.Length == 0)
				return "";
			char type = s[0];
			// do not use '#' or '!' they are used for dictionaries
			if(type == 'n')
				return null;
			else if(type == 'i')
			{
				int v;
				if(int.TryParse(s.Substring(1), System.Globalization.NumberStyles.Integer, Bag.Unity.ParseCulture, out v))
					return v;
				else
				{
					Debug.LogError("Integer value '" + s.Substring(1) + "' was not recognized by the parser, returning 0.");
					return 0;
				}
			}
			else if(type == 'f')
			{
				float v;
				if(float.TryParse(s.Substring(1), System.Globalization.NumberStyles.Float, Bag.Unity.ParseCulture, out v))
					return v;
				else
				{
					Debug.LogError("Float value '" + s.Substring(1) + "' was not recognized by the parser, returning 0.");
					return 0;
				}
			}
			else if(type == 'd')
			{
				double v;
				if(double.TryParse(s.Substring(1), System.Globalization.NumberStyles.Integer, Bag.Unity.ParseCulture, out v))
					return v;
				else
				{
					Debug.LogError("Double value '" + s.Substring(1) + "' was not recognized by the parser, returning 0.");
					return 0;
				}
			}
			else if(type == 's')
				return s.Substring(1);
			else if(type == 'b')
				return bool.Parse(s.Substring(1));
			else if(type == 'e')
			{
				Debug.LogError("error: object was not a serializable type when it was serialized, check the other related error for the solution.");
				return s.Substring(1);
			}
			else
			{
				Debug.LogError("error: object serialized type '" + type + "' was not recongnized, did you forget to add it to the type check ifs?");
				return s;
			}
		}
	}
}
