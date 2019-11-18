using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.PostProcessing;

namespace Bag.Mobile.UiLite
{
	public class OptionsManager : MonoBehaviour
	{
		public static bool Vibration { get { return GetOption(OptionType.Vibrate); } }
		public static OptionsManager Singleton { get; private set; }
		static OptionsRefereces OptionsRefs
		{
			get
			{
				return new OptionsRefereces() {/* postProcessing = Singleton.postProcessing*/ };
			}
		}
		public static float FPS { get { return Singleton ? Singleton.fps : -1; } }


		class OptionsRefereces
		{
			//public PostProcessingBehaviour postProcessing;
		}

		public enum OptionSet
		{
			SET,
			Switch
		}

		public enum OptionType
		{
			NONE,
			Battery,
			Music,
			Sfx,
			Vibrate
		}

		[SerializeField] Image musicImg;
		[SerializeField] Image vibrateImg;
		[SerializeField] Sprite musicOFF;
		[SerializeField] Sprite musicON;
		[SerializeField] Sprite vibrateOFF;
		[SerializeField] Sprite vibrateON;

		//[SerializeField] PostProcessingBehaviour postProcessing;
		//[SerializeField] PostProcessingProfile postProcPanel;

		//PostProcessingProfile postProcProfile;
		float fps;
		float fpsDeltaTime = 0.0f;
		float fpsLow = 0;
		int prefsVersion = 5;


		void Awake()
		{
			Singleton = this;
		}


		public void Init()
		{
			Singleton = this;

			//if(postProcessing == null)
			//	postProcessing = CanvasManager.Camera.GetComponent<PostProcessingBehaviour>();
			//if(postProcessing != null)
			//	postProcProfile = postProcessing.profile;

			if(PlayerPrefs.GetInt("prefs_version", 0) < prefsVersion)
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.SetInt("prefs_version", prefsVersion);
			}

			//SetBtnImage(OptionType.Battery, SetOption(OptionType.Battery, OptionSet.SET));
			SetOption(OptionType.Battery, OptionSet.SET);
			SetBtnImage(OptionType.Music, SetOption(OptionType.Music, OptionSet.SET));
			//SetBtnImage(OptionType.Sfx, SetOption(OptionType.Sfx, OptionSet.SET));
			SetOption(OptionType.Sfx, OptionSet.SET, GetOption(OptionType.Music) ? 1 : 0); // set like music
			SetBtnImage(OptionType.Vibrate, SetOption(OptionType.Vibrate, OptionSet.SET));
		}

		public void OnPanelsOpen(GameObject p)
		{
			//if(postProcessing != null)
			//{
			//	if(p != null)
			//	{
			//		postProcessing.profile = postProcPanel;
			//		postProcessing.enabled = true;
			//	}
			//	else
			//	{
			//		postProcessing.profile = postProcProfile;
			//		postProcessing.enabled = GetOption(OptionType.Battery);
			//	}
			//}
		}

		public void BatteryAutoSet()
		{
			fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;
			fps = 1.0f / fpsDeltaTime;

			if(PlayerPrefs.GetInt("battery", 2) == 2)
			{
				if(fps < 30)
					fpsLow += Time.deltaTime;
				else
					fpsLow = 0;
				if(fpsLow > 5f)
					SetOption(OptionType.Battery, OptionSet.SET, 0);
			}
		}


		#region UI

		void SetBtnImage(OptionType type, bool on)
		{
			switch(type)
			{
				case OptionType.Music:
					musicImg.sprite = on ? musicON : musicOFF;
					break;
				case OptionType.Vibrate:
					vibrateImg.sprite = on ? vibrateON : vibrateOFF;
					break;
			}
		}


		public void BatteryClick()
		{
			CanvasManager.SoundClick();
			SetBtnImage(OptionType.Battery, SetOption(OptionType.Battery, OptionSet.Switch));
		}

		public void MusicClick()
		{
			CanvasManager.SoundClick();
			SetBtnImage(OptionType.Music, SetOption(OptionType.Music, OptionSet.Switch));
			SetOption(OptionType.Sfx, OptionSet.SET, GetOption(OptionType.Music) ? 1 : 0); // set like music
		}

		public void VibrateClick()
		{
			CanvasManager.SoundClick();
			SetBtnImage(OptionType.Vibrate, SetOption(OptionType.Vibrate, OptionSet.Switch));
		}

		#endregion


		#region SET

		public static bool SetOption(OptionType type, OptionSet set, int value = -1)
		{
			Action<int> action = v => { };

			switch(type)
			{
				case OptionType.Battery:
					action = v =>
					{
						//if(OptionsRefs.postProcessing != null)
						//	OptionsRefs.postProcessing.enabled = v > 0 || CanvasManager.PanelOpen;
						QualitySettings.vSyncCount = 0;
						Application.targetFrameRate = v > 0 ? 60 : 60;
					};
					break;
				case OptionType.Music:
					action = v => { AudioManager.SetMusicVolume(Mathf.Clamp01(v)); };
					break;
				case OptionType.Sfx:
					action = v => { AudioManager.SetSfxVolume(Mathf.Clamp01(v)); };
					break;
				case OptionType.Vibrate:
					break;
				default:
					return false;
			}

			switch(set)
			{
				case OptionSet.SET:
					return SetPlayerPrefsOption(type, value >= 0 ? value : PlayerPrefs.GetInt(type.ToString().ToLower(), 2), action);
				case OptionSet.Switch:
					return SwitchPlayerPrefsOption(type, action);
				default:
					return false;
			}
		}

		public static bool GetOption(OptionType type)
		{
			return PlayerPrefs.GetInt(type.ToString().ToLower(), 1) > 0;
		}

		static bool SwitchPlayerPrefsOption(OptionType type, Action<int> action)
		{
			return SetPlayerPrefsOption(type, GetOption(type) ? 0 : 1, action);
		}

		static bool SetPlayerPrefsOption(OptionType type, int set, Action<int> action)
		{
			PlayerPrefs.SetInt(type.ToString().ToLower(), set);
			action(set);
			return set > 0;
		}

		#endregion
	}
}
