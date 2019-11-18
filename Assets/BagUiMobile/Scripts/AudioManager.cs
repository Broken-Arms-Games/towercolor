using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour

{


	#region VARIABLES

	public static AudioManager instance;
	public AudioFile[] musicFiles;
	public AudioFile[] sfxFiles;
	private float timeToReset;
	private bool timerIsSet = false;
	private string tmpName;
	private float tmpVol;
	private bool isLowered = false;
	private bool fadeOut = false;
	private bool fadeIn = false;
	private string fadeInUsedString;
	private string fadeOutUsedString;
	#endregion

	// Use this for initialization
	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		foreach(var s in musicFiles)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.audioClip;
			s.source.volume = s.volume;
			s.source.loop = s.isLooping;

			if(s.playOnAwake)
			{
				s.source.Play();
			}
		}

		foreach(var s in sfxFiles)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.audioClip;
			s.source.volume = s.volume;
			s.source.loop = s.isLooping;
		}
	}



	#region METHODS
	public static void PlayMusic(string name, bool pitchVariation = false)
	{
		if(instance)
			instance.Play(instance.musicFiles, name, pitchVariation);
	}

	public static void PlaySfx(string name, bool pitchVariation = false)
	{
		if(instance)
			instance.Play(instance.sfxFiles, name, pitchVariation);
	}

	private void Play(AudioFile[] am, string name, bool pitchVariation = false)
	{
		AudioFile s = Array.Find(am, AudioFile => AudioFile.audioName == name);
		if(s == null)
		{
			Debug.LogError("Sound name" + name + "not found!");
			return;
		}
		else
		{
			if(pitchVariation)
				s.source.pitch = 1 + UnityEngine.Random.Range(-0.08f, 0.08f);
			else
				s.source.pitch = 1;
			s.source.Play();
		}
	}

	public static void SetMusicVolume(float v)
	{
		if(instance)
			instance.SetVolume(instance.musicFiles, v);
	}

	public static void SetSfxVolume(float v)
	{
		if(instance)
			instance.SetVolume(instance.sfxFiles, v);
	}

	public void SetVolume(AudioFile[] af, float v)
	{
		foreach(var s in af)
		{
			s.source.volume = v;
		}
	}

	public static void StopMusic(String name)
	{

		AudioFile s = Array.Find(instance.musicFiles, AudioFile => AudioFile.audioName == name);
		if(s == null)
		{

			Debug.LogError("Sound name" + name + "not found!");

			return;

		}

		else

		{

			s.source.Stop();

		}

	}


	public static void PauseMusic(String name)
	{
		AudioFile s = Array.Find(instance.musicFiles, AudioFile => AudioFile.audioName == name);

		if(s == null)
		{
			Debug.LogError("Sound name" + name + "not found!");
			return;
		}
		else
		{
			s.source.Pause();
		}
	}


	public static void UnPauseMusic(String name)
	{
		AudioFile s = Array.Find(instance.musicFiles, AudioFile => AudioFile.audioName == name);

		if(s == null)
		{
			Debug.LogError("Sound name" + name + "not found!");
			return;
		}
		else
		{
			s.source.UnPause();
		}

	}


	public static void LowerVolume(String name, float _duration)

	{

		if(instance.isLowered == false)

		{

			AudioFile s = Array.Find(instance.musicFiles, AudioFile => AudioFile.audioName == name);

			if(s == null)

			{

				Debug.LogError("Sound name" + name + "not found!");

				return;

			}

			else

			{

				instance.tmpName = name;

				instance.tmpVol = s.volume;

				instance.timeToReset = Time.time + _duration;

				instance.timerIsSet = true;

				s.source.volume = s.source.volume / 3;

			}


			instance.isLowered = true;

		}

	}


	public static void FadeOut(String name, float duration)

	{

		instance.StartCoroutine(instance.IFadeOut(name, duration));

	}


	public static void FadeIn(String name, float targetVolume, float duration)

	{

		instance.StartCoroutine(instance.IFadeIn(name, targetVolume, duration));

	}




	//not for use

	private IEnumerator IFadeOut(String name, float duration)

	{

		AudioFile s = Array.Find(instance.musicFiles, AudioFile => AudioFile.audioName == name);

		if(s == null)

		{

			Debug.LogError("Sound name" + name + "not found!");

			yield return null;

		}

		else

		{

			if(fadeOut == false)

			{

				fadeOut = true;

				float startVol = s.source.volume;

				fadeOutUsedString = name;

				while(s.source.volume > 0)

				{

					s.source.volume -= startVol * Time.deltaTime / duration;

					yield return null;

				}


				s.source.Stop();

				yield return new WaitForSeconds(duration);

				fadeOut = false;

			}


			else

			{

				Debug.Log("Could not handle two fade outs at once : " + name + " , " + fadeOutUsedString + "! Stopped the music " + name);

				StopMusic(name);

			}

		}

	}


	public IEnumerator IFadeIn(string name, float targetVolume, float duration)

	{

		AudioFile s = Array.Find(instance.musicFiles, AudioFile => AudioFile.audioName == name);

		if(s == null)

		{

			Debug.LogError("Sound name" + name + "not found!");

			yield return null;

		}

		else

		{

			if(fadeIn == false)

			{

				fadeIn = true;

				instance.fadeInUsedString = name;

				s.source.volume = 0f;

				s.source.Play();

				while(s.source.volume < targetVolume)

				{

					s.source.volume += Time.deltaTime / duration;

					yield return null;

				}



				yield return new WaitForSeconds(duration);

				fadeIn = false;

			}

			else

			{

				Debug.Log("Could not handle two fade ins at once: " + name + " , " + fadeInUsedString + "! Played the music " + name);

				StopMusic(fadeInUsedString);

				PlayMusic(name);

			}

		}

	}


	void ResetVol()

	{

		AudioFile s = Array.Find(instance.musicFiles, AudioFile => AudioFile.audioName == tmpName);

		s.source.volume = tmpVol;

		isLowered = false;

	}


	private void Update()

	{

		if(Time.time >= timeToReset && timerIsSet)

		{

			ResetVol();

			timerIsSet = false;

		}

	}


	#endregion

}