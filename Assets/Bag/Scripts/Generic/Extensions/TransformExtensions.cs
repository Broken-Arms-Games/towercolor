using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bag.Scripts.Extensions
{
	static class TransformExtensions
	{
		#region GetComponent

		/// <summary>
		/// Gets one T component in the transform, if it returns true after being passed though the filter function.
		/// </summary>
		public static T GetComponent<T>(this Transform parent, Func<T, bool> filter) where T : UnityEngine.Object
		{
			if(filter == null)
				return parent.GetComponent<T>();
			else
			{
				T[] components = parent.GetComponents<T>();
				for(int i = 0; i < components.Length; i++)
					if(filter(components[i]))
						return components[i];
				return null;
			}
		}

		/// <summary>
		/// Gets one T component in each of the children and nested children of a transform and the transform itself.
		/// </summary>
		public static T[] GetComponentInALL<T>(this Transform parent) where T : UnityEngine.Object
		{
			return parent.GetComponentInALL<T>(null);
		}

		/// <summary>
		/// Gets one T component in each of the children and nested children of a transform and the transform itself, if it returns true after being passed though the filter function.
		/// </summary>
		public static T[] GetComponentInALL<T>(this Transform parent, Func<T, bool> filter) where T : UnityEngine.Object
		{
			List<T> list = new List<T>();
			T component = parent.GetComponent<T>();
			if(component)
				list.Add(component);
			GetComponentInALLChildrenRecursive<T>(parent, ref list, ref component, filter);
			return list.ToArray();
		}

		/// <summary>
		/// Gets one T component for each of the children and nested children of a transform.
		/// </summary>
		public static T[] GetComponentInALLChildren<T>(this Transform parent) where T : UnityEngine.Object
		{
			return parent.GetComponentInALLChildren<T>(null);
		}

		/// <summary>
		/// Gets one T component, that returns true after being passed though the filter function, for each of the children and nested children of a transform.
		/// </summary>
		public static T[] GetComponentInALLChildren<T>(this Transform parent, Func<T, bool> filter) where T : UnityEngine.Object
		{
			List<T> list = new List<T>();
			T component = null;
			GetComponentInALLChildrenRecursive<T>(parent, ref list, ref component, filter);
			return list.ToArray();
		}

		static void GetComponentInALLChildrenRecursive<T>(Transform parent, ref List<T> list, ref T component, Func<T, bool> filter) where T : UnityEngine.Object
		{
			foreach(Transform child in parent)
			{
				component = child.GetComponent<T>(filter);
				if(component)
					list.Add(component);
				GetComponentInALLChildrenRecursive<T>(child, ref list, ref component, filter);
			}
		}

		#endregion


		#region GetComponents

		/// <summary>
		/// Gets all T components in the transform, if it returns true after being passed though the filter function.
		/// </summary>
		public static T[] GetComponents<T>(this Transform parent, Func<T, bool> filter) where T : UnityEngine.Object
		{
			if(filter == null)
				return parent.GetComponents<T>();
			else
				return parent.GetComponents<T>().ToList().FindAll(c => { return filter(c); }).ToArray();
		}

		/// <summary>
		/// Gets all T component in each of the children and nested children of a transform and the transform itself.
		/// </summary>
		public static T[] GetComponentsInALL<T>(this Transform parent) where T : UnityEngine.Object
		{
			return parent.GetComponentsInALL<T>(null);
		}

		/// <summary>
		/// Gets all T component in each of the children and nested children of a transform and the transform itself, if it returns true after being passed though the filter function.
		/// </summary>
		public static T[] GetComponentsInALL<T>(this Transform parent, Func<T, bool> filter) where T : UnityEngine.Object
		{
			List<T> list = new List<T>();
			T[] components = parent.GetComponents<T>();
			for(int i = 0; i < components.Length; i++)
				list.Add(components[i]);
			GetComponentsInALLChildrenRecursive<T>(parent, ref list, ref components, filter);
			return list.ToArray();
		}

		/// <summary>
		/// Gets one T component for each of the children and nested children of a transform.
		/// </summary>
		public static T[] GetComponentsInALLChildren<T>(this Transform parent) where T : UnityEngine.Object
		{
			return parent.GetComponentsInALLChildren<T>(null);
		}

		/// <summary>
		/// Gets one T component, that returns true after being passed though the filter function, for each of the children and nested children of a transform.
		/// </summary>
		public static T[] GetComponentsInALLChildren<T>(this Transform parent, Func<T, bool> filter) where T : UnityEngine.Object
		{
			List<T> list = new List<T>();
			T[] components = new T[0];
			GetComponentsInALLChildrenRecursive<T>(parent, ref list, ref components, filter);
			return list.ToArray();
		}

		static void GetComponentsInALLChildrenRecursive<T>(Transform parent, ref List<T> list, ref T[] components, Func<T, bool> filter) where T : UnityEngine.Object
		{
			foreach(Transform child in parent)
			{
				components = child.GetComponents<T>(filter);
				for(int i = 0; i < components.Length; i++)
					list.Add(components[i]);
				GetComponentsInALLChildrenRecursive<T>(child, ref list, ref components, filter);
			}
		}

		#endregion


		public static void DestroyChilds(this Transform parent)
		{
			parent.DestroyChilds(0, parent.childCount);
		}

		public static void DestroyChilds(this Transform parent, int start)
		{
			parent.DestroyChilds(start, parent.childCount);
		}

		public static void DestroyChilds(this Transform parent, int start, int count)
		{
			for(int i = count - 1; i >= start; i--)
				GameObject.Destroy(parent.GetChild(i).gameObject);
		}

		public static void DestroyImmediateChilds(this Transform parent)
		{
			parent.DestroyImmediateChilds(0, parent.childCount);
		}

		public static void DestroyImmediateChilds(this Transform parent, int start)
		{
			parent.DestroyImmediateChilds(start, parent.childCount);
		}

		public static void DestroyImmediateChilds(this Transform parent, int start, int count)
		{
			for(int i = count - 1; i >= start; i--)
				GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
		}

		public static GameObject InstantiateAsChild(this Transform parent, GameObject prefab, Vector3 localPosition = default(Vector3), Quaternion localRotation = default(Quaternion), bool oneScale = false)
		{
			GameObject go = GameObject.Instantiate(prefab);
			go.transform.SetParent(parent);
			go.transform.localPosition = localPosition;
			go.transform.localRotation = localRotation;
			if(oneScale)
				go.transform.localScale = Vector3.one;
			return go;
		}

		public static void AddPoolList(this Transform parent, GameObject prefab, int add, ref List<GameObject> pool, Action<GameObject, int> initPoolObj, bool inversePoolInit = false, bool oneScale = true)
		{
			if(pool == null)
				pool = new List<GameObject>();

			prefab.gameObject.SetActive(false);
			int count = 0;
			for(int i = 0; i < pool.Count; i++)
				if(pool[i].gameObject.activeSelf)
					count++;
			pool.Sort((a, b) => { return b.gameObject.activeSelf.CompareTo(a.gameObject.activeSelf); });
			for(int i = count; i < pool.Count; i++)
				pool[i].gameObject.SetActive(i < count + add);
			for(int i = pool.Count; i < count + add; i++)
			{
				pool.Add(parent.InstantiateAsChild(prefab.gameObject, oneScale: oneScale));
				pool[i].gameObject.SetActive(true);
			}
			if(inversePoolInit)
				for(int i = count + add - 1; i >= count; i--)
					initPoolObj(pool[i], i - count);
			else
				for(int i = count; i < count + add; i++)
					initPoolObj(pool[i], i - count);
		}

		public static void AddPoolList<T>(this Transform parent, T prefab, int add, ref List<T> pool, Action<T, int> initPoolObj, bool inversePoolInit = false, bool oneScale = true) where T : Component
		{
			if(pool == null)
				pool = new List<T>();

			prefab.gameObject.SetActive(false);
			int count = 0;
			for(int i = 0; i < pool.Count; i++)
				if(pool[i].gameObject.activeSelf)
					count++;
			pool.Sort((a, b) => { return b.gameObject.activeSelf.CompareTo(a.gameObject.activeSelf); });
			for(int i = count; i < pool.Count; i++)
				pool[i].gameObject.SetActive(i < count + add);
			for(int i = pool.Count; i < count + add; i++)
			{
				pool.Add(parent.InstantiateAsChild(prefab.gameObject, oneScale: oneScale).GetComponent<T>());
				pool[i].gameObject.SetActive(true);
			}
			if(inversePoolInit)
				for(int i = count + add - 1; i >= count; i--)
					initPoolObj(pool[i], i - count);
			else
				for(int i = count; i < count + add; i++)
					initPoolObj(pool[i], i - count);
		}

		public static void SetPoolList<T>(this Transform parent, T prefab, int count, ref List<T> pool, Action<T, int> initPoolObj, bool inversePoolInit = false, bool oneScale = true) where T : MonoBehaviour
		{
			if(pool == null)
				pool = new List<T>();

			prefab.gameObject.SetActive(false);
			for(int i = 0; i < pool.Count; i++)
				pool[i].gameObject.SetActive(i < count);
			for(int i = pool.Count; i < count; i++)
			{
				pool.Add(parent.InstantiateAsChild(prefab.gameObject, oneScale: oneScale).GetComponent<T>());
				pool[i].gameObject.SetActive(true);
			}
			if(inversePoolInit)
				for(int i = count - 1; i >= 0; i--)
					initPoolObj(pool[i], i);
			else
				for(int i = 0; i < count; i++)
					initPoolObj(pool[i], i);
		}

		public static void SetPoolList(this Transform parent, GameObject prefab, int count, ref List<GameObject> pool, Action<GameObject, int> initPoolObj, bool inversePoolInit = false, bool oneScale = true)
		{
			if(pool == null)
				pool = new List<GameObject>();

			prefab.gameObject.SetActive(false);
			for(int i = 0; i < pool.Count; i++)
				pool[i].gameObject.SetActive(i < count);
			for(int i = pool.Count; i < count; i++)
			{
				pool.Add(parent.InstantiateAsChild(prefab.gameObject, oneScale: oneScale));
				pool[i].gameObject.SetActive(true);
			}
			if(inversePoolInit)
				for(int i = count - 1; i >= 0; i--)
					initPoolObj(pool[i], i);
			else
				for(int i = 0; i < count; i++)
					initPoolObj(pool[i], i);
		}

		public static void SetPoolListLayoutHorizontal<T>(this Transform parent, T prefab, RectTransform holder, UnityEngine.UI.HorizontalLayoutGroup layout, int count, ref List<T> pool,
			Func<T, RectTransform> prefabRectGetter, Action<T, int> initPoolObj, bool inversePoolInit = false) where T : MonoBehaviour
		{
			float width = layout.padding.left;
			parent.SetPoolList(prefab, count, ref pool, (p, i) =>
			{
				initPoolObj(p, i);
				width += prefabRectGetter(p).sizeDelta.x;
				if(i < count - 1)
					width += layout.spacing;
			}, inversePoolInit);
			width += layout.padding.right;
			holder.sizeDelta = new Vector2(width, holder.sizeDelta.y);
		}

		public static void GetChilds(this Transform parent, ref List<GameObject> list)
		{
			foreach(Transform child in parent)
			{
				list.Add(child.gameObject);
			}
		}

		public static void GetALLChilds(this Transform parent, Func<Transform, bool> filter, ref List<GameObject> list)
		{
			foreach(Transform child in parent)
			{
				if(filter(child))
					list.Add(child.gameObject);
				GetALLChilds(child, filter, ref list);
			}
		}

		public static void GetALLChilds(this Transform parent, ref List<GameObject> list)
		{
			GetALLChilds(parent, c => { return true; }, ref list);
		}

		public static void FindALLChilds(this Transform parent, string name, ref List<GameObject> list)
		{
			GetALLChilds(parent, c => { return c.gameObject.name == name; }, ref list);
		}
	}
}
