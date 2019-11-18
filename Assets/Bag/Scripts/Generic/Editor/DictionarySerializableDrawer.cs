using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bag.Scripts.Extensions;

namespace Bag.Scripts.Generic
{
	public static class TypeSerialized
	{
		public enum Type
		{
			NULL,
			String,
			Int,
			Float,
			Bool
		}

		public static GUIContent[] typeLabels = new GUIContent[] { new GUIContent("NULL"), new GUIContent("string"), new GUIContent("int"), new GUIContent("float"), new GUIContent("bool") };

		public static Type GetSerializedType(string s)
		{
			return GetType(s.JsonDeserialize());
		}

		public static Type GetType(object o)
		{
			if(o is string)
				return Type.String;
			else if(o is int)
				return Type.Int;
			else if(o is float)
				return Type.Float;
			else if(o is bool)
				return Type.Bool;
			else
				return Type.NULL;
		}

		public static object GetObj(Type t)
		{
			switch(t)
			{
				case Type.String:
					return "";
				case Type.Int:
					return 0;
				case Type.Float:
					return 0f;
				case Type.Bool:
					return false;
				default:
					return null;
			}
		}
	}

	[CustomPropertyDrawer(typeof(DictionarySerializable))]
	public class DictionarySerializableDrawer : PropertyDrawer
	{
		public class EditorItem
		{
			public string key;
			public TypeSerialized.Type type;
			public string s;
			public int i;
			public float f;
			public bool b;

			public EditorItem(string keyvalue)
			{
				if(keyvalue.Contains("#!"))
				{
					int ind = keyvalue.IndexOf("#!");
					key = keyvalue.Substring(0, ind);
					object o = keyvalue.Substring(ind + 2).JsonDeserialize();
					if(o is string)
					{
						type = TypeSerialized.Type.String;
						s = o.ToString();
					}
					else if(o is int)
					{
						type = TypeSerialized.Type.Int;
						i = o.ToInt();
					}
					else if(o is float)
					{
						type = TypeSerialized.Type.Float;
						f = float.Parse(o.ToString());
					}
					else if(o is bool)
					{
						type = TypeSerialized.Type.Bool;
						b = bool.Parse(o.ToString());
					}
				}
				else
				{
					key = keyvalue;
					type = TypeSerialized.Type.NULL;
				}
			}

			public string Serialized()
			{
				string r = key + "#!";
				switch(type)
				{
					default:
						r += "n";
						break;
					case TypeSerialized.Type.String:
						r += s.JsonSerialize();
						break;
					case TypeSerialized.Type.Int:
						r += i.JsonSerialize();
						break;
					case TypeSerialized.Type.Float:
						r += f.JsonSerialize();
						break;
					case TypeSerialized.Type.Bool:
						r += b.JsonSerialize();
						break;
				}
				return r;
			}

			public static bool DictContainsKey(List<EditorItem> dict, string key)
			{
				for(int i = 0; i < dict.Count; i++)
					if(dict[i].key == key)
						return true;
				return false;
			}
		}


		Vector2 scroll;
		List<EditorItem> list;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			// codice originale che ho trovato online
			//SerializedObject childObj = new SerializedObject(property.serializedObject.targetObject);
			//SerializedProperty ite = childObj.GetIterator();
			//float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;
			//while(ite.NextVisible(true))
			//{
			//	totalHeight += EditorGUI.GetPropertyHeight(ite, label, true) + EditorGUIUtility.standardVerticalSpacing;
			//}
			//return totalHeight;

			float totalHeight = EditorGUI.GetPropertyHeight(property, label, false) + EditorGUIUtility.standardVerticalSpacing;
			if(property.isExpanded)
			{
				SerializedProperty array = property.FindPropertyRelative("dict");
				if(array.arraySize > 0)
				{
					for(int i = 0; i < array.arraySize; i++)
					{
						SerializedProperty iProp = array.GetArrayElementAtIndex(i);
						GUIContent iPropLabel = new GUIContent(iProp.displayName);
						totalHeight += EditorGUI.GetPropertyHeight(iProp, iPropLabel, false) + EditorGUIUtility.standardVerticalSpacing;
					}
					totalHeight += EditorGUI.GetPropertyHeight(property, label, false) + EditorGUIUtility.standardVerticalSpacing;
				}
				else
					totalHeight += EditorGUI.GetPropertyHeight(property, label, false) + EditorGUIUtility.standardVerticalSpacing;
			}

			return totalHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty array = property.FindPropertyRelative("dict");
			List<EditorItem> list = new List<EditorItem>();
			for(int i = 0; i < array.arraySize; i++)
				list.Add(new EditorItem(array.GetArrayElementAtIndex(i).stringValue));
			GUIContent lbl = new GUIContent("a");

			// layout data
			Rect indented = EditorGUI.IndentedRect(position);
			Rect pos = indented;
			float propHeight = EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(property, label, false);
			float labelWidth = GUI.skin.label.CalcSize(label).x + 65;
			pos.size = new Vector2(labelWidth, propHeight);
			// draw property title
			if(GUI.Button(pos, label))
				property.isExpanded = !property.isExpanded;
			//property.isExpanded = EditorGUI.PropertyField(pos, property, label, false);
			pos.x += labelWidth;
			// draw property info
			pos.width = indented.width - pos.x;
			EditorGUIUtility.labelWidth = 0.1f;
			EditorGUI.LabelField(pos, lbl, new GUIContent("Serializable dictionary entries: " + list.Count), EditorStyles.miniLabel);
			EditorGUIUtility.labelWidth = 0;
			pos.y += propHeight;
			// draw property childs
			if(property.isExpanded)
			{
				pos = indented;
				pos.y += propHeight;
				pos.x += 20;
				pos.width -= 20;
				// dictionary display
				int removeIndex = int.MinValue;
				if(list.Count > 0)
				{
					for(int i = 0; i < list.Count; i++)
					{
						// layout data
						SerializedProperty iProp = array.GetArrayElementAtIndex(i);
						GUIContent iPropLabel = new GUIContent(iProp.displayName);
						float iPropHeight = EditorGUI.GetPropertyHeight(iProp, iPropLabel, false);
						pos.size = new Vector2(pos.size.x, iPropHeight);
						Rect iPos = pos;
						// inizio layout
						EditorGUIUtility.labelWidth = 0.1f;
						// entry key
						iPos.width = 70;
						list[i].key = EditorGUI.TextField(iPos, lbl, list[i].key);
						iPos.x += iPos.width + 1;
						// entry type
						iPos.width = 50;
						list[i].type = (TypeSerialized.Type)EditorGUI.Popup(iPos, new GUIContent("a"), (int)list[i].type, TypeSerialized.typeLabels);
						iPos.x += iPos.width + 1;
						// entry content
						iPos.width = pos.size.x - (iPos.x - pos.x) - pos.size.y;
						switch(list[i].type)
						{
							default:
								GUI.Label(iPos, "Type value not supported.", Editor.EditorBag.textError);
								break;
							case TypeSerialized.Type.NULL:
								GUI.Label(iPos, "ERROR: Type value should not be null.", Editor.EditorBag.textError);
								break;
							case TypeSerialized.Type.String:
								if(list[i].s == null)
									list[i].s = "";
								list[i].s = EditorGUI.TextField(iPos, lbl, list[i].s);
								break;
							case TypeSerialized.Type.Int:
								list[i].i = EditorGUI.IntField(iPos, lbl, list[i].i);
								break;
							case TypeSerialized.Type.Float:
								list[i].f = EditorGUI.FloatField(iPos, lbl, list[i].f);
								break;
							case TypeSerialized.Type.Bool:
								list[i].b = EditorGUI.Toggle(iPos, lbl, list[i].b);
								break;
						}
						iPos.x += iPos.width;
						// remove entry button
						iPos.size = new Vector2(pos.height, pos.height);
						if(GUI.Button(iPos, new GUIContent("-", "Remove dictionary entry.\nWARNING: value will be lost."), EditorStyles.miniButton))
							// get remove entry index
							removeIndex = i;
						iPos.x += 18;

						pos.y += iPropHeight + EditorGUIUtility.standardVerticalSpacing;
						EditorGUIUtility.labelWidth = 0;
					}
				}
				else
				{
					pos.height = EditorGUI.GetPropertyHeight(property, label, false);
				}
				// execute remove button
				if(removeIndex >= 0 && removeIndex < list.Count)
					list.RemoveAt(removeIndex);
				// add entry button
				if(GUI.Button(pos, new GUIContent("+", "Add dictionary entry.\nRemember to change the key."), EditorStyles.miniButton))
				{
					if(EditorItem.DictContainsKey(list, "key"))
					{
						int i = 1;
						while(EditorItem.DictContainsKey(list, "key" + i))
							i++;
						list.Add(new EditorItem("key" + i));
					}
					else
						list.Add(new EditorItem("key"));
				}

				// save changes in array parameter
				array.arraySize = list.Count;
				for(int i = 0; i < list.Count; i++)
					array.GetArrayElementAtIndex(i).stringValue = list[i].Serialized();
			}

			// default drawer
			//EditorGUI.PropertyField(pos, iProp, iPropLabel, true);
		}
	}
}
