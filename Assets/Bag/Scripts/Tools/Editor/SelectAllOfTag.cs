using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Bag.Scripts.Tools
{
	public class SelectAllOfTag : ScriptableWizard
	{
		public string searchTag = "Your tag here";

		[MenuItem("Tools/Bag/Select all of tag...")]
		static void SelectAllOfTagWizard()
		{
			ScriptableWizard.DisplayWizard<SelectAllOfTag>("Select all of tag...", "Make Selection");
		}

		void OnWizardCreate()
		{
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(searchTag);
			Selection.objects = gameObjects;
		}
	}
}