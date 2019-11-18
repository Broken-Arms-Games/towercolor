using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	public class AnimatorReference : MonoBehaviour
	{
		public bool Paused { get; private set; }

		public Animator animator;

		float animSpeedVel;
		IEnumerator speedChange;

		public void SetTrigger(string id)
		{
			animator.SetTrigger(id);
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
			while(!Mathf.Approximately(animator.speed, target))
			{
				animator.speed = Mathf.SmoothDamp(animator.speed, target, ref animSpeedVel, 0.1f);
				yield return null;
			}

			animator.speed = target;

			speedChange = null;
		}

		public AnimationClip GetAnimationClip(string name)
		{
			if(!animator)
				return null; // no animator

			foreach(AnimationClip clip in animator.runtimeAnimatorController.animationClips)
			{
				//Debug.LogError(animator.name + " has clip " + clip.name);
				if(clip.name == name)
					return clip;
			}
			return null; // no clip by that name
		}
	}
}
