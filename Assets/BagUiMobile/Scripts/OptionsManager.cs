using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.PostProcessing;

public class OptionsManager : MonoBehaviour
{
	public static bool Vibration { get { return PlayerPrefs.GetInt("vibrate", 1) == 1; } }
	public static OptionsManager Singleton { get; private set; }
	static OptionsRefereces OptionsRefs
	{
		get
		{
			return new OptionsRefereces() {/* postProcessing = Singleton.postProcessing*/ };
		}
	}


	class OptionsRefereces
	{
		//public PostProcessingBehaviour[] postProcessing;
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


	[SerializeField] Image batteryImg;
	[SerializeField] Image musicImg;
	[SerializeField] Image sfxImg;
	[SerializeField] Image vibrateImg;

	[SerializeField] Sprite batteryOFF;
	[SerializeField] Sprite batteryON;
	[SerializeField] Sprite musicOFF;
	[SerializeField] Sprite musicON;
	[SerializeField] Sprite sfxOFF;
	[SerializeField] Sprite sfxON;
	[SerializeField] Sprite vibrateOFF;
	[SerializeField] Sprite vibrateON;

	//[SerializeField] PostProcessingBehaviour[] postProcessing;

	float fpsLow = 0;


	void Awake()
	{
		Singleton = this;
	}


	public void Init()
	{
		Singleton = this;

		SetBtnImage(OptionType.Battery, SetOption(OptionType.Battery, OptionSet.SET));
		SetBtnImage(OptionType.Music, SetOption(OptionType.Music, OptionSet.SET));
		SetBtnImage(OptionType.Sfx, SetOption(OptionType.Sfx, OptionSet.SET));
		SetBtnImage(OptionType.Vibrate, SetOption(OptionType.Vibrate, OptionSet.SET));
	}

	public void BatteryAutoSet()
	{
		if(PlayerPrefs.GetInt("battery", 2) == 2)
		{
			if(FPSDisplay.FPS < 55)
				fpsLow += Time.deltaTime;
			else
				fpsLow = 0;
			if(fpsLow > 2f)
				SetBtnImage(OptionType.Battery, SetOption(OptionType.Battery, OptionSet.SET, 0));
		}
	}


	#region UI

	void SetBtnImage(OptionType type, bool on)
	{
		switch(type)
		{
			case OptionType.Battery:
				batteryImg.sprite = !on ? batteryON : batteryOFF;
				break;
			case OptionType.Music:
				musicImg.sprite = on ? musicON : musicOFF;
				break;
			case OptionType.Sfx:
				sfxImg.sprite = on ? sfxON : sfxOFF;
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
	}

	public void SfxClick()
	{
		CanvasManager.SoundClick();
		SetBtnImage(OptionType.Sfx, SetOption(OptionType.Sfx, OptionSet.Switch));
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
					//for(int i = 0; OptionsRefs.postProcessing != null && i < OptionsRefs.postProcessing.Length; i++)
					//	OptionsRefs.postProcessing[i].enabled = v > 0;
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = v > 0 ? 60 : 30;
				};
				break;
			case OptionType.Music:
				action = v => { AudioManager.SetMusicVolume(v); };
				break;
			case OptionType.Sfx:
				action = v => { AudioManager.SetSfxVolume(v); };
				break;
			case OptionType.Vibrate:
				break;
			default:
				return false;
		}

		switch(set)
		{
			case OptionSet.SET:
				return SetPlayerPrefsOption(type.ToString().ToLower(), value >= 0 ? value : PlayerPrefs.GetInt(type.ToString().ToLower(), 2), action);
			case OptionSet.Switch:
				return SwitchPlayerPrefsOption(type.ToString().ToLower(), action);
			default:
				return false;
		}
	}

	static bool SwitchPlayerPrefsOption(string name, Action<int> action)
	{
		return SetPlayerPrefsOption(name, PlayerPrefs.GetInt(name, 1) > 0 ? 0 : 1, action);
	}

	static bool SetPlayerPrefsOption(string name, int set, Action<int> action)
	{
		PlayerPrefs.SetInt(name, set);
		action(set);
		return set > 0;
	}

	#endregion


	#region VIBRATION

	public static void Vibrate()
	{
		Game.Hub.StartCoroutine(Vibrate(1, 0.3f));
	}

	static IEnumerator Vibrate(int times, float lenght = 0.5f)
	{
		if(Vibration)
		{
			float interval = lenght;
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
