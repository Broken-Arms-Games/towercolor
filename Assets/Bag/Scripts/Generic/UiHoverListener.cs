using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Bag.Scripts.Generic
{
	public class UiHoverListener : MonoBehaviour
	{
		static UiHoverListener Singleton;

		public static bool IsUIOverride
		{
			get
			{
#if UNITY_IOS || UNITY_ANDROID
				PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
				eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				List<RaycastResult> results = new List<RaycastResult>();
				EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
				Singleton.debugIsUIOverride = isUIOverride = results.Count > 0;
				return isUIOverride;
#else
			return isUIOverride;
#endif
			}
			private set
			{
				isUIOverride = value;
				if(Singleton)
					Singleton.debugIsUIOverride = isUIOverride;
			}
		}
		public static bool IsPointerInScreen
		{
			get { return isPointerInScreen; }
			private set
			{
				isPointerInScreen = value;
				if(Singleton)
					Singleton.debugIsPointerInScreen = isPointerInScreen;
			}
		}
		static bool isUIOverride;
		static bool isPointerInScreen;

		[Header("DEBUG")]
		[SerializeField] [DisplayWithoutEdit] bool debugIsUIOverride;
		[SerializeField] [DisplayWithoutEdit] bool debugIsPointerInScreen;

		private void Awake()
		{
			Singleton = this;
		}

		void Update()
		{
			// It will turn true if hovering any UI Elements
#if !UNITY_IOS && !UNITY_ANDROID
		IsUIOverride = EventSystem.current.IsPointerOverGameObject() || !Application.isFocused;
#endif
			// It will turn true if inside game window
			IsPointerInScreen = InputHandler.MousePos.x < 1 && InputHandler.MousePos.x > 0 &&
				InputHandler.MousePos.y < 1 && InputHandler.MousePos.y > 0;
		}

		void OnApplicationFocus(bool focus)
		{
			if(!focus)
				IsUIOverride = true;
		}
	}
}
