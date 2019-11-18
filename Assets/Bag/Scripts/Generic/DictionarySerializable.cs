using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Extensions;

namespace Bag.Scripts.Generic
{
	[System.Serializable]
	public class DictionarySerializable : ISerializationCallbackReceiver
	{
		[SerializeField] string[] dict = new string[0]; // json serialization slot

		private Dictionary<string, object> d = new Dictionary<string, object>();

		public Dictionary<string, object> Dict
		{
			get { return d; }
			set { d = value; }
		}

		public int Count
		{
			get { return Dict.Count; }
		}

		public object this[string key]
		{
			get
			{
				return Dict[key];
			}
			set
			{
				Dict[key] = value;
			}
		}

		public string Log()
		{
			return Dict.Log();
		}

		public void Add(string key, object value)
		{
			Dict.Add(key, value);
		}

		public bool Remove(string key)
		{
			return Dict.Remove(key);
		}

		public bool ContainsKey(string key)
		{
			return Dict.ContainsKey(key);
		}

		public void Set(string key, object value)
		{
			if(Dict.ContainsKey(key))
				Dict[key] = value;
			else
				Dict.Add(key, value);
		}

		public void Set(DictionarySerializable other)
		{
			foreach(KeyValuePair<string, object> kv in other.d)
			{
				if(ContainsKey(kv.Key))
					this[kv.Key] = kv.Value;
				else
					Add(kv.Key, kv.Value);
			}
		}

		public object Get(string key, object defaultValue)
		{
			if(ContainsKey(key))
				return this[key];
			else
				return defaultValue;
		}

		public DictionarySerializable()
		{
			dict = new string[0];
			d = new Dictionary<string, object>();
		}

		public DictionarySerializable(DictionarySerializable source)
		{
			dict = new string[source.dict.Length];
			for(int i = 0; i < dict.Length; i++)
				dict[i] = source.dict[i];
			d = new Dictionary<string, object>(source.d);
		}


		public void OnBeforeSerialize()
		{
			//Debug.LogError("Before");
			if(d == null)
			{
				dict = null;
				return;
			}

			dict = new string[d.Count];
			int i = 0;
			foreach(KeyValuePair<string, object> kv in Dict)
			{
				dict[i] = kv.Key + "#!" + kv.Value.JsonSerialize();
				i++;
			}
		}

		public void OnAfterDeserialize()
		{
			//Debug.LogError("After");
			if(dict == null)
				d = null;

			d = new Dictionary<string, object>();
			for(int i = 0; i < dict.Length; i++)
			{
				if(dict[i].Contains("#!"))
				{
					int ind = dict[i].IndexOf("#!");
					d.Add(dict[i].Substring(0, ind), dict[i].Substring(ind + 2).JsonDeserialize());
				}
				else
				{
					Debug.LogError("error: error SerializableDictionary KeyValuePair '" + dict[i] + "' was not serialized correctly as it does not contain '#!'.");
					d.Add(dict[i], "");
				}
			}
		}
	}
}
