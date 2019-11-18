using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Bag.Scripts.Generic.Editor
{
	public static partial class EditorBag
	{
		static bool inited;

		public static GUIStyle baseStyle
		{
			get
			{
				GUIStyle s = new GUIStyle();
				return s;
			}
		}

		public static GUIStyle tileTitleStyle
		{
			get
			{
				GUIStyle s = new GUIStyle();
				s.normal.textColor = Color.white;
				s.fontStyle = FontStyle.Bold;
				s.fontSize = 13;
				s.alignment = TextAnchor.MiddleCenter;
				return s;
			}
		}

		public static GUIStyle centered
		{
			get
			{
				GUIStyle s = new GUIStyle();
				s.alignment = TextAnchor.MiddleCenter;
				return s;
			}
		}

		public static GUIStyle left
		{
			get
			{
				GUIStyle s = new GUIStyle();
				s.alignment = TextAnchor.MiddleLeft;
				return s;
			}
		}

		public static GUIStyle[] menuArrow
		{
			get
			{
				GUIStyle[] s = new GUIStyle[] { new GUIStyle(), new GUIStyle() };
				s[0].normal.background = Resources.Load<Texture2D>("Menu/menuClsd");
				s[1].normal.background = Resources.Load<Texture2D>("Menu/menuOpen");
				return s;
			}
		}

		public static GUIStyle textError
		{
			get
			{
				GUIStyle s = new GUIStyle();
				s.normal.textColor = Color.white;
				s.fontStyle = FontStyle.Bold;
				Texture2D t = new Texture2D(1, 1);
				t.SetPixels(new Color[] { Color.red });
				t.Apply();
				s.normal.background = t;
				return s;
			}
		}

		public static GUIStyle textWarning
		{
			get
			{
				GUIStyle s = new GUIStyle();
				s.normal.textColor = Color.black;
				s.fontStyle = FontStyle.Bold;
				Texture2D t = new Texture2D(1, 1);
				t.SetPixels(new Color[] { Color.yellow });
				t.Apply();
				s.normal.background = t;
				return s;
			}
		}

		public static GUIStyle tileBkgStyle
		{
			get
			{
				GUIStyle s = new GUIStyle();
				s.normal.textColor = Color.white;
				s.normal.background = Resources.Load<Texture2D>("Menu/tile-bkg");
				return s;
			}
		}

		public static GUIStyle[] tileStyles
		{
			get
			{
				GUIStyle[] s = new GUIStyle[10];
				for(int i = 0; i < s.Length; i++)
				{
					s[i] = new GUIStyle();
					s[i].normal.background = Resources.Load<Texture2D>("Menu/tile-" + (i + 1).ToString("00"));
					s[i].fontSize = 8;
					s[i].alignment = TextAnchor.MiddleCenter;
				}
				return s;
			}
		}

		public static GUIStyle miniButton
		{
			get
			{
				GUIStyle s = EditorStyles.miniButton;
				s.alignment = TextAnchor.MiddleCenter;
				s.clipping = TextClipping.Overflow;
				s.padding = new RectOffset(0, 0, 0, 0);
				return s;
			}
		}

		public static GUIStyle miniButtonLeft
		{
			get
			{
				GUIStyle s = EditorStyles.miniButtonLeft;
				s.alignment = TextAnchor.MiddleCenter;
				s.clipping = TextClipping.Overflow;
				s.padding = new RectOffset(0, 0, 0, 0);
				return s;
			}
		}

		public static GUIStyle miniButtonMid
		{
			get
			{
				GUIStyle s = EditorStyles.miniButtonMid;
				s.alignment = TextAnchor.MiddleCenter;
				s.clipping = TextClipping.Overflow;
				s.padding = new RectOffset(0, 0, 0, 0);
				return s;
			}
		}

		public static GUIStyle miniButtonRight
		{
			get
			{
				GUIStyle s = EditorStyles.miniButtonRight;
				s.alignment = TextAnchor.MiddleCenter;
				s.clipping = TextClipping.Overflow;
				s.padding = new RectOffset(0, 0, 0, 0);
				return s;
			}
		}

		/// <summary>
		/// Create a EditorGUILayout.TextField with no space between label and text field
		/// </summary>
		public static string TextField(string label, string text, params GUILayoutOption[] options)
		{
			var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
			EditorGUIUtility.labelWidth = textDimensions.x;
			return EditorGUILayout.TextField(label, text, options);
		}

		/// <summary>
		/// Use to easily calculate the rect while using 'EditorGUI.PropertyFields'.
		/// </summary>
		public static void PropertyField(ref Rect pos, ref GUIContent lbl, SerializedProperty property, System.Action<Rect, GUIContent, SerializedProperty> assignment)
		{
			lbl = new GUIContent(property.displayName);
			pos.height = EditorGUI.GetPropertyHeight(property, lbl);
			assignment(pos, lbl, property);
			pos.y += pos.height;
		}

		/// <summary>
		/// Creates a 'EditorGUI.TextField' using 'EditorUtility.PropertyField'.
		/// </summary>
		public static void PropertyFieldString(ref Rect pos, ref GUIContent lbl, SerializedProperty property)
		{
			PropertyField(ref pos, ref lbl, property, (p, l, s) =>
			{
				s.stringValue = EditorGUI.TextField(p, l, s.stringValue);
			});
		}
	}
}