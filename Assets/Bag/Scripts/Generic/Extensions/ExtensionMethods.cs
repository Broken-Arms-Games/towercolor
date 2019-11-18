//using PlayFab.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Bag.Scripts.Extensions
{
	public static class ExtensionMethods
	{
		// questo file è obsoleto, conservato solo per le estensioni integrate con playfab che potrebbero tornare utili
		// non aggiungere estensioni qui, usare il file apposito per la classe o crearne uno
	}




	//public static class JsonObjectExtensions
	//{
	//	public static T ParseClass<T>(this JsonObject o) where T : new()
	//	{
	//		return o.ParseClass<T>(new List<string>());
	//	}

	//	public static T ParseClass<T>(this JsonObject o, string ignoreField) where T : new()
	//	{
	//		return o.ParseClass<T>(new List<string>() { ignoreField });
	//	}

	//	public static T ParseClass<T>(this JsonObject o, List<string> ignoreFIelds) where T : new()
	//	{
	//		T n = new T();

	//		FieldInfo[] fields = typeof(T).GetFields();
	//		for(int i = 0; i < fields.Length; i++)
	//		{
	//			if(!fields[i].IsStatic && !ignoreFIelds.Contains(fields[i].Name))
	//			{
	//				//if(fields[i].Name == "tracks") // DEBUG
	//				//{
	//				//	Debug.LogError((fields[i].FieldType == typeof(int[])).ToString());
	//				//	Debug.LogError(fields[i].FieldType.ToString() + " == " + typeof(int[]).ToString());
	//				//}

	//				if(fields[i].FieldType == typeof(int))
	//					fields[i].SetValue(n, o.KeyToInt(fields[i].Name));
	//				else if(fields[i].FieldType == (new int[0]).GetType())
	//					fields[i].SetValue(n, o.KeyToIntArray(fields[i].Name));
	//				else if(fields[i].FieldType == typeof(string))
	//					fields[i].SetValue(n, o.KeyToString(fields[i].Name));
	//				else if(fields[i].FieldType == (new string[0]).GetType())
	//					fields[i].SetValue(n, o.KeyToStringArray(fields[i].Name));
	//				else if(fields[i].FieldType == typeof(float))
	//					fields[i].SetValue(n, o.KeyToFloat(fields[i].Name));
	//				else if(fields[i].FieldType == (new float[0]).GetType())
	//					fields[i].SetValue(n, o.KeyToFloatArray(fields[i].Name));
	//				else if(fields[i].FieldType == typeof(DateTime))
	//					fields[i].SetValue(n, o.KeyToDateTime(fields[i].Name));
	//				//else if(fields[i].FieldType.IsSubclassOf(typeof(Enum)))
	//				//fields[i].SetValue(n, o.KeyToEnum < fields[i].FieldType > (fields[i].Name));
	//				else if(fields[i].FieldType == typeof(CurrencyType))
	//					fields[i].SetValue(n, o.KeyToEnum<CurrencyType>(fields[i].Name));
	//				else if(fields[i].FieldType == typeof(RaceType))
	//					fields[i].SetValue(n, o.KeyToEnum<RaceType>(fields[i].Name));
	//				else
	//					Debug.LogError("[JSON OBJ PARSER] Error parsing field '" + fields[i].Name + "' did not have a type case for " + fields[i].FieldType.ToString());
	//			}
	//		}

	//		return n;
	//	}

	//	public static string KeyToString(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		if(o.ContainsKey(key))
	//			return o[key] != null ? o[key].ToString() : "";
	//		else
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "' returning empty string.");
	//			return "";
	//		}
	//	}

	//	public static string[] KeyToStringArray(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		if(o.ContainsKey(key) && !string.IsNullOrEmpty(o[key].ToString()))
	//		{
	//			JsonArray ja = JsonWrapper.DeserializeObject<JsonArray>(o[key].ToString());
	//			if(ja == null)
	//				return new string[0];
	//			string[] a = new string[ja.Count];
	//			for(int i = 0; i < ja.Count; i++)
	//				a[i] = ja[i].ToString();
	//			return a;
	//		}
	//		else
	//		{
	//			if(!o.ContainsKey(key) || logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning empty array.");
	//			return new string[0];
	//		}
	//	}

	//	public static int KeyToInt(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		int r;
	//		if(o.ContainsKey(key) && int.TryParse(o[key].ToString(),System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out r))
	//			return r;
	//		else if(o.ContainsKey(key))
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object under key '" + key + "' with value '" + o[key].ToString() + "' could not be parsed, returning 0.");
	//			return 0;
	//		}
	//		else
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning 0.");
	//			return 0;
	//		}
	//	}

	//	public static int[] KeyToIntArray(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		if(o.ContainsKey(key) && !string.IsNullOrEmpty(o[key].ToString()))
	//		{
	//			//Debug.LogError(o[key].ToString());
	//			JsonArray ja = JsonWrapper.DeserializeObject<JsonArray>(o[key].ToString());
	//			if(ja == null)
	//				return new int[0];
	//			int[] a = new int[ja.Count];
	//			for(int i = 0; i < ja.Count; i++)
	//			{
	//				if(!int.TryParse(ja[i].ToString(),System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out a[i]))
	//				{
	//					if(logErrorFail)
	//						Debug.LogError("Json Object under key '" + key + "' with value '" + ja[i].ToString() + "' could not be parsed, returning empty array.");
	//					return new int[0];
	//				}
	//			}
	//			return a;
	//		}
	//		else
	//		{
	//			if(!o.ContainsKey(key) || logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning empty array.");
	//			return new int[0];
	//		}
	//	}

	//	public static float KeyToFloat(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		float r;
	//		if(o.ContainsKey(key) && float.TryParse(o[key].ToString(),System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out r))
	//			return r;
	//		else if(o.ContainsKey(key))
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object under key '" + key + "' with value '" + o[key].ToString() + "' could not be parsed, returning 0.");
	//			return 0;
	//		}
	//		else
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning 0.");
	//			return 0;
	//		}
	//	}

	//	public static float[] KeyToFloatArray(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		if(o.ContainsKey(key) && !string.IsNullOrEmpty(o[key].ToString()))
	//		{
	//			//Debug.LogError(o[key].ToString());
	//			JsonArray ja = JsonWrapper.DeserializeObject<JsonArray>(o[key].ToString());
	//			if(ja == null)
	//				return new float[0];
	//			float[] a = new float[ja.Count];
	//			for(int i = 0; i < ja.Count; i++)
	//			{
	//				if(!float.TryParse(ja[i].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture,out a[i]))
	//				{
	//					if(logErrorFail)
	//						Debug.LogError("Json Object under key '" + key + "' with value '" + ja[i].ToString() + "' could not be parsed, returning empty array.");
	//					return new float[0];
	//				}
	//			}
	//			return a;
	//		}
	//		else
	//		{
	//			if(!o.ContainsKey(key) || logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning empty array.");
	//			return new float[0];
	//		}
	//	}

	//	public static DateTime KeyToDateTime(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		DateTime r;
	//		if(o.ContainsKey(key) && DateTime.TryParse(key, out r))
	//			return r;
	//		else if(o.ContainsKey(key))
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object under key '" + key + "' with value '" + o[key].ToString() + "' could not be parsed, returning DateTime.MinValue.");
	//			return DateTime.MinValue;
	//		}
	//		else
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning DateTime.MinValue.");
	//			return DateTime.MinValue;
	//		}
	//	}

	//	public static T KeyToEnum<T>(this JsonObject o, string key, bool logErrorFail = false) where T : struct, IConvertible
	//	{
	//		if(o.ContainsKey(key))
	//			return (T)Enum.Parse(typeof(T), o[key].ToString(), true);
	//		else
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning default Enum.");
	//			return default(T);
	//		}
	//	}

	//	public static JsonObject KeyToJObject(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		if(o.ContainsKey(key))
	//			return JsonWrapper.DeserializeObject<JsonObject>(o.KeyToString(key));
	//		else
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning null JsonObject.");
	//			return null;
	//		}
	//	}

	//	public static JsonArray KeyToJArray(this JsonObject o, string key, bool logErrorFail = false)
	//	{
	//		if(o.ContainsKey(key))
	//			return JsonWrapper.DeserializeObject<JsonArray>(o.KeyToString(key));
	//		else
	//		{
	//			if(logErrorFail)
	//				Debug.LogError("Json Object did not contain key '" + key + "', returning null JsonObject.");
	//			return null;
	//		}
	//	}
	//}


	//public static class ExecuteCloudScriptResultExtensions
	//{
	//	public static void LogError(this PlayFab.ClientModels.ExecuteCloudScriptResult result, string prefix = "", string checkd = "", bool asError = true)
	//	{
	//		if(asError)
	//			Debug.LogError((string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + " Cloud script '" + result.FunctionName + "' " + (string.IsNullOrEmpty(checkd) ? "" : checkd + " ") + " with error:\n" + result.Error.Error + "\n" + result.Error.Message + "\nLogging:" + result.LogsToString());
	//		else
	//			Debug.Log((string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + " Cloud script '" + result.FunctionName + "' " + (string.IsNullOrEmpty(checkd) ? "" : checkd + " ") + " with error:\n" + result.Error.Error + "\n" + result.Error.Message + "\nLogging:" + result.LogsToString());
	//	}

	//	public static void LogLogs(this PlayFab.ClientModels.ExecuteCloudScriptResult result, string prefix = "", bool asError = false)
	//	{
	//		if(!asError)
	//			Debug.Log((string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + " Cloud script '" + result.FunctionName + "' logged:\n" + result.LogsToString());
	//		else
	//			Debug.LogError((string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + " Cloud script '" + result.FunctionName + "' logged:\n" + result.LogsToString());
	//	}

	//	public static void LogResult(this PlayFab.ClientModels.ExecuteCloudScriptResult result, string prefix = "", bool asError = false)
	//	{
	//		if(!asError)
	//			Debug.Log((string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + " Cloud script '" + result.FunctionName + "' returned:\n" + result.FunctionResult.ToString());
	//		else
	//			Debug.LogError((string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + " Cloud script '" + result.FunctionName + "' returned:\n" + result.FunctionResult.ToString());
	//	}

	//	public static void LogResultLogs(this PlayFab.ClientModels.ExecuteCloudScriptResult result, string prefix = "", bool asError = false)
	//	{
	//		if(!asError)
	//			Debug.Log((string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + " Cloud script '" + result.FunctionName + "' returned:\n" + result.FunctionResult.ToString() + "\nLogging:\n" + result.LogsToString());
	//		else
	//			Debug.LogError((string.IsNullOrEmpty(prefix) ? "" : prefix + " ") + " Cloud script '" + result.FunctionName + "' returned:\n" + result.FunctionResult.ToString() + "\nLogging:\n" + result.LogsToString());
	//	}

	//	public static string LogsToString(this PlayFab.ClientModels.ExecuteCloudScriptResult result)
	//	{
	//		string logs = "";
	//		for(int i = 0; i < result.Logs.Count; i++)
	//			logs = "\n" + result.Logs[i].Message;
	//		return logs;
	//	}
	//}
}
