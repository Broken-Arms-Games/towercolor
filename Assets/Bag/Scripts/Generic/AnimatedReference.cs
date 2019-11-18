using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	public class AnimatedReference : MonoBehaviour
	{
		public bool Paused { get; private set; }

		public Animation animation;

		AnimationClip animationClip;
		Action callback;
		float animSpeed;
		float animSpeedVel;

		IEnumerator invokeCallback;
		IEnumerator speedChange;


		public void Play(AnimationClip animationClip, Action callback)
		{
			if(animation != null && animation.isPlaying)
				animation.Stop();

			this.animationClip = animationClip;
			this.callback = callback;
			animation.clip = animationClip;

			animation.Play();
			invokeCallback = InvokeCallback();
			StartCoroutine(invokeCallback);
			Paused = false;

			foreach(AnimationState state in animation)
				state.speed = 1;
		}

		IEnumerator InvokeCallback()
		{
			yield return null;
			Action cb = callback;
			while(cb != null)
			{
				if(animation.isPlaying)
				{
					yield return null;
				}
				else
				{
					invokeCallback = null;
					callback = null;
					cb();
					cb = null;
				}
			}
		}

		public void Pause()
		{
			if(speedChange != null)
				StopCoroutine(speedChange);
			speedChange = SpeedChange(0);
			StartCoroutine(speedChange);
			Paused = true;
		}

		public void Resume()
		{
			if(speedChange != null)
				StopCoroutine(speedChange);
			speedChange = SpeedChange(1);
			StartCoroutine(speedChange);
			Paused = false;
		}

		IEnumerator SpeedChange(float target)
		{
			foreach(AnimationState state in animation)
				animSpeed = state.speed;

			while(!Mathf.Approximately(animSpeed, target))
			{
				foreach(AnimationState state in animation)
					state.speed = animSpeed = Mathf.SmoothDamp(state.speed, target, ref animSpeedVel, 0.1f);
				yield return null;
			}

			foreach(AnimationState state in animation)
				state.speed = animSpeed = target;

			speedChange = null;
		}

		void Stop()
		{
			animation.Stop();
			if(invokeCallback != null)
				StopCoroutine(invokeCallback);
			if(callback != null)
				callback();
			callback = null;
			Paused = false;
		}
	}
}
