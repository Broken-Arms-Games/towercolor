using System;
using System.Collections;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	public class InstantiatedCoroutine
	{
		public class WaitForSecondsRT : CustomYieldInstruction
		{
			float m_Time;
			public override bool keepWaiting
			{
				get { return (m_Time -= Time.unscaledDeltaTime) > 0; }
			}
			public WaitForSecondsRT(float aWaitTime)
			{
				m_Time = aWaitTime;
			}
			public WaitForSecondsRT NewTime(float aTime)
			{
				m_Time = aTime;
				return this;
			}
		}

		public bool IsPlaying { get { return coroutine != null; } }

		MonoBehaviour script;
		IEnumerator coroutine;
		Action callback;

		public InstantiatedCoroutine(MonoBehaviour script)
		{
			this.script = script;
			coroutine = null;
			callback = null;
		}

		public void Stop(bool ignoreCallback = false)
		{
			if(coroutine != null)
			{
				script.StopCoroutine(coroutine);
				coroutine = null;
			}
			if(!ignoreCallback && this.callback != null)
				this.callback();
		}

		public void Clear()
		{
			if(coroutine != null)
				script.StopCoroutine(coroutine);
		}

		public void Start(float time, Action<float> execute, Action callback, bool ignoreOldCallback = false)
		{
			Start(time, execute, callback, ignoreOldCallback, false);
		}

		public void StartRealtime(float time, Action<float> execute, Action callback, bool ignoreOldCallback = false)
		{
			Start(time, execute, callback, ignoreOldCallback, true);
		}

		void Start(float time, Action<float> execute, Action callback, bool ignoreOldCallback, bool realtime)
		{
			Stop(ignoreOldCallback);

			if(time > 0)
			{
				coroutine = Co(time, execute, callback, realtime);
				this.callback = callback;
				script.StartCoroutine(coroutine);
			}
			else
			{
				coroutine = null;
				this.callback = null;
				execute(1);
				if(callback != null)
					callback();
			}
		}

		IEnumerator Co(float time, Action<float> execute, Action callback, bool realtime)
		{
			WaitForSecondsRT wait = new WaitForSecondsRT(time);
			execute(0);
			float t = 0;
			while(t <= 1 && time > 0)
			{
				if(realtime)
					t += Time.unscaledDeltaTime / time;
				else
					t += Time.deltaTime / time;
				execute(t);
				if(realtime)
					yield return wait.NewTime(Time.unscaledDeltaTime);
				else
					yield return null;
			}
			execute(1);
			coroutine = null;
			this.callback = null;
			if(callback != null)
				callback();
		}
	}
}
