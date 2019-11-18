using UnityEditor;
using UnityEngine;
using System;


namespace Bag.Scripts.Generic.Editor
{
	public static class EditorList
	{
		[Flags]
		public enum Option
		{
			None = 0,
			ListSize = 1,
			ListLabel = 2,
			ElementLabels = 4,
			Buttons = 8,
			Default = ListSize | ListLabel | ElementLabels,
			NoElementLabels = ListSize | ListLabel,
			All = Default | Buttons
		}

		//static GUIContent moveButtonContentDown = new GUIContent("\u21b4", "move down");
		static GUIContent moveButtonContentDown = new GUIContent("\u25bc", "move down");
		static GUIContent moveButtonContentUp = new GUIContent("\u25b2", "move up");
		static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
		static GUIContent deleteButtonContent = new GUIContent("-", "delete");
		static GUIContent addButtonContent = new GUIContent("+", "add element");
		static float miniBtnSide = 18;
		static GUILayoutOption miniBtnW = GUILayout.Width(miniBtnSide);
		static GUILayoutOption miniBtnW2 = GUILayout.Width(miniBtnSide * 2f);
		static GUILayoutOption miniBtnH = GUILayout.Height(miniBtnSide);


		public static void Show(SerializedProperty list, Option options = Option.Default, Action<SerializedProperty> visualization = null)
		{
			if(!list.isArray)
			{
				EditorGUILayout.HelpBox(list.name + " is neither an array nor a list.", MessageType.Error);
				return;
			}

			bool showListLabel = (options & Option.ListLabel) != 0;
			bool showListSize = (options & Option.ListSize) != 0;

			if(showListLabel)
			{
				EditorGUILayout.PropertyField(list);
				++EditorGUI.indentLevel;
			}
			if(!showListLabel || list.isExpanded)
			{
				SerializedProperty size = list.FindPropertyRelative("Array.size");
				if(showListSize)
					EditorGUILayout.PropertyField(size);
				if(size.hasMultipleDifferentValues)
					EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
				else
					ShowElements(list, options, visualization);
			}
			if(showListLabel)
				--EditorGUI.indentLevel;
		}

		static void ShowElements(SerializedProperty list, Option options, Action<SerializedProperty> visualization)
		{
			bool showElementLabels = (options & Option.ElementLabels) != 0;
			bool showButtons = (options & Option.Buttons) != 0;

			EditorGUILayout.BeginVertical();
			for(int i = 0; i < list.arraySize; i++)
			{
				//int propertyHeightLvl = Mathf.FloorToInt(EditorGUI.GetPropertyHeight(list.GetArrayElementAtIndex(i), list.GetArrayElementAtIndex(i).isExpanded) / miniBtnSide);
				int propertyHeightLvl = EditorGUI.GetPropertyHeight(list.GetArrayElementAtIndex(i), list.GetArrayElementAtIndex(i).isExpanded) > miniBtnSide * 8 ? 2 : 0;
				if(showButtons)
					EditorGUILayout.BeginHorizontal();
				if(visualization != null)
				{
					EditorGUILayout.BeginVertical();
					++EditorGUI.indentLevel;
					visualization(list.GetArrayElementAtIndex(i));
					--EditorGUI.indentLevel;
					EditorGUILayout.EndVertical();
				}
				else
				{
					if(showElementLabels)
						EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), true);
					else
						EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none, true);
				}
				if(showButtons)
				{
					ShowButtons(list, i, propertyHeightLvl);
					EditorGUILayout.EndHorizontal();
				}
			}
			if(showButtons && list.arraySize == 0)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(EditorGUI.indentLevel * 20);
				if(GUILayout.Button(addButtonContent, EditorStyles.miniButton))
					list.arraySize++;
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
		}

		static void ShowButtons(SerializedProperty list, int index, int propertyHeightLvl = 0)
		{
			Func<GUIStyle, bool>[] btns = new Func<GUIStyle, bool>[] {
				s => { return GUILayout.Button(moveButtonContentUp, s, miniBtnW, miniBtnH); },
				s => { return GUILayout.Button(moveButtonContentDown, s, miniBtnW, miniBtnH); },
				s => { return GUILayout.Button(duplicateButtonContent, s, miniBtnW, miniBtnH); },
				s => { return GUILayout.Button(deleteButtonContent, s, miniBtnW, miniBtnH); }
			};
			Action<SerializedProperty, int>[] acts = new Action<SerializedProperty, int>[] {
				(l, i) => { l.MoveArrayElement(i, i - 1); },
				(l, i) => { l.MoveArrayElement(i, i + 1); },
				(l, i) => { l.InsertArrayElementAtIndex(i); },
				(l, i) => {
					int oldSize = l.arraySize;
					l.DeleteArrayElementAtIndex(i);
					if(l.arraySize == oldSize)
						l.DeleteArrayElementAtIndex(i);
				}
			};

			switch(propertyHeightLvl)
			{
				case 0:
					for(int i = 0; i < btns.Length; i++)
						if(btns[i](i == 0 ? EditorBag.miniButtonLeft : (i == btns.Length - 1) ? EditorBag.miniButtonRight : EditorBag.miniButtonMid))
							acts[i](list, index);
					break;
				case 1:
					EditorGUILayout.BeginVertical(miniBtnW2);
					for(int y = 0; y < 2; y++)
					{
						EditorGUILayout.BeginHorizontal();
						for(int x = 0; x < 2; x++)
							if(btns[x + y * 2](x == 0 ? EditorBag.miniButtonLeft : (x >= 1) ? EditorBag.miniButtonRight : EditorBag.miniButtonMid))
								acts[x + y * 2](list, index);
						EditorGUILayout.EndHorizontal();
					}
					EditorGUILayout.EndVertical();
					break;
				default:
					EditorGUILayout.BeginVertical(miniBtnW);
					for(int i = 0; i < btns.Length; i++)
						if(btns[i](EditorBag.miniButton))
							acts[i](list, index);
					EditorGUILayout.EndVertical();
					break;
			}
		}
	}
}