using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Bag.Mobile.UiLite
{
	public class CanvasManager : MonoBehaviour
	{
		public static CanvasManager Singleton { get; set; }
		public static Camera Camera
		{
			get
			{
				if(Singleton.cam != null)
					return Singleton.cam;
				else
					Debug.LogError("[CANVAS] Reference to the camera is null, please set it.");
				return null;
			}
		}
		public static OptionsManager OptionsManager { get { return Singleton.optionsManager; } }
		public static bool PanelOpen
		{
			get
			{
				for(int i = 0; i < Singleton.panels.Length; i++)
					if(Singleton.panels[i].activeSelf)
						return true;
				return false;
			}
		}
		public static CanvasScaler CanvasScaler
		{
			get
			{
				if(Singleton.canvasScaler != null)
					return Singleton.canvasScaler;
				else
					Debug.LogError("[CANVAS] Reference to the CanvasScaler is null, please set it.");
				return null;
			}
		}
		public static RectTransform CanvasRect
		{
			get
			{
				if(Singleton.canvasRect != null)
					return Singleton.canvasRect;
				else
					Debug.LogError("[CANVAS] Reference to the RectTransform is null, please set it.");
				return null;
			}
		}

		[Header("Canvas References")]
		[SerializeField] Camera cam;
		[SerializeField] RectTransform canvasRect;
		[SerializeField] CanvasScaler canvasScaler;
		[SerializeField] GameObject[] panels;
		[SerializeField] protected OptionsManager optionsManager;
		[SerializeField] bool autoLoadingClose = true;


		public event Action<GameObject> onPanelOpen;
		public event Action<GameObject[]> onPanelChanged;


		protected virtual void Awake()
		{
			Init();
		}

		protected virtual void Update()
		{
			optionsManager.BatteryAutoSet();
		}


		protected virtual void Init()
		{
			Singleton = this;
			onPanelChanged = p => { };
			onPanelOpen = p => { Debug.Log("[" + name.ToUpper() + "] opens panel '" + p.name + "'."); };
			onPanelOpen = p =>
			{
				if((IsPanel(p, "pause") || IsPanel(p, "options")))
					optionsManager.Init();
			};
			onPanelOpen = optionsManager.OnPanelsOpen;

			optionsManager.Init();
			if(autoLoadingClose)
				LoadingdAnimationManager.CloseLoading();
		}

		public void OpenPanel(string name)
		{
			SoundClick();
			bool open = false;
			for(int i = 0; i < panels.Length; i++)
			{
				panels[i].SetActive(IsPanel(panels[i], name));
				if(panels[i].activeSelf)
				{
					onPanelOpen(panels[i]);
					open = true;
				}
			}
			if(!open)
				onPanelOpen(null);
			onPanelChanged(panels);
		}

		protected bool IsPanel(GameObject panel, string name)
		{
			return panel != null &&
				(panel.name.ToLower().Trim() == name.ToLower().Trim() ||
				panel.name.ToLower().Trim() == "pnl_" + name.ToLower().Trim());
		}


		public static void SoundClick()
		{
			AudioManager.PlaySfx("ClickMenu");
		}


		#region VIBRATION

		public static void Vibrate()
		{
			Vibrate(1, 0.3f);
		}

		public static void Vibrate(int times, float interval)
		{
			Singleton.StartCoroutine(_Vibrate(times, interval));
		}

		static IEnumerator _Vibrate(int times, float interval = 0.5f)
		{
			if(OptionsManager.Vibration)
			{
				WaitForSeconds wait = new WaitForSeconds(interval);
				float t;

				for(t = 0; t < interval * times; t += interval)
				{
					Handheld.Vibrate();
					yield return wait;
				}
			}
		}

		#endregion
	}
}
