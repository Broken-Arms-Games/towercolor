using UnityEngine;
using System.Collections;
using UnityEditor;
using Bag.Scripts.Generic;

namespace Bag.Scripts.Tools
{
	public class ReplaceChilds : ScriptableWizard
	{
		public GameObject parent;
		public GameObject replace;

		[MenuItem("Tools/Bag/Replace childs...")]
		static void ReplaceChildsWizard()
		{
			ScriptableWizard.DisplayWizard<ReplaceChilds>("Replace childs...", "Replace");
		}

		void OnWizardCreate()
		{
			if(parent == null || replace == null)
			{
				Debug.LogError("Replace childs failed because the parent or the replace object was null.");
				return;
			}

			Undo.RecordObject(parent, "Childs replaced");
			int childs = parent.transform.childCount;
			Vector3 pos;
			Quaternion rot;
			Vector3 siz;
			GameObject o;
			for(int i = 0; i < childs; i++)
			{
				if(parent.transform.GetChild(0).gameObject != replace)
				{
					pos = parent.transform.GetChild(0).localPosition;
					rot = parent.transform.GetChild(0).localRotation;
					siz = parent.transform.GetChild(0).localScale;
					DestroyImmediate(parent.transform.GetChild(0).gameObject);
					if(PrefabUtility.GetCorrespondingObjectFromSource(replace) != null)
						o = PrefabUtility.InstantiatePrefab(replace, parent.transform) as GameObject;
					else
						o = Instantiate(replace, parent.transform);
					o.transform.localPosition = pos;
					o.transform.localRotation = rot;
					o.transform.localScale = siz;
				}
				else
					o = replace;
				o.transform.SetAsLastSibling();
			}
			if(childs > 0)
				EditorUtility.SetDirty(parent);
		}
	}
}