﻿using UnityEngine;
using UnityEditor;

namespace Bag.Scripts.Generic
{
	[CustomPropertyDrawer(typeof(DisplayWithoutEdit))]
	public class DisplayWithoutEditDrawer : PropertyDrawer
	{
		/// <summary>
		/// Display attribute and his value in inspector depending on the type
		/// Fill attribute needed
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			switch(property.propertyType)
			{
				case SerializedPropertyType.AnimationCurve:
					break;
				case SerializedPropertyType.ArraySize:
					break;
				case SerializedPropertyType.Boolean:
					EditorGUI.LabelField(position, label, new GUIContent(property.boolValue.ToString()));
					break;
				case SerializedPropertyType.Bounds:
					break;
				case SerializedPropertyType.Character:
					break;
				case SerializedPropertyType.Color:
					break;
				case SerializedPropertyType.Enum:
					EditorGUI.LabelField(position, label, new GUIContent(property.enumDisplayNames[property.enumValueIndex]));
					break;
				case SerializedPropertyType.Float:
					EditorGUI.LabelField(position, label, new GUIContent(property.floatValue.ToString()));
					break;
				case SerializedPropertyType.Generic:
					break;
				case SerializedPropertyType.Gradient:
					break;
				case SerializedPropertyType.Integer:
					EditorGUI.LabelField(position, label, new GUIContent(property.intValue.ToString()));
					break;
				case SerializedPropertyType.LayerMask:
					break;
				case SerializedPropertyType.ObjectReference:
					break;
				case SerializedPropertyType.Quaternion:
					break;
				case SerializedPropertyType.Rect:
					break;
				case SerializedPropertyType.String:
					EditorGUI.LabelField(position, label, new GUIContent(property.stringValue));
					break;
				case SerializedPropertyType.Vector2:
					EditorGUI.LabelField(position, label, new GUIContent(property.vector2Value.ToString()));
					break;
				case SerializedPropertyType.Vector3:
					EditorGUI.LabelField(position, label, new GUIContent(property.vector3Value.ToString()));
					break;
				case SerializedPropertyType.Vector4:
					EditorGUI.LabelField(position, label, new GUIContent(property.vector4Value.ToString()));
					break;
			}
		}
	}
}
