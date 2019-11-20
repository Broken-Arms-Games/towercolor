using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Generic;
using UnityEngine.UI;
using TMPro;

namespace Bag.Mobile.UiLite
{
	public class CanvasCoreManager : CanvasManager
	{
		public TextMeshProUGUI numberBallsText;
		public PopIn[] popIns;
		public TextMeshProUGUI[] textPowers;
		public TextMeshProUGUI levelNumberText;

		new public static CanvasCoreManager Singleton { get; set; }

		[SerializeField] Image goalBar;

		float goalBarTarget;
		InstantiatedCoroutine goalBarCO;
		Queue<Action> goalBarQueue = new Queue<Action>();

		public event Action restartGame;

		protected override void Init()
		{
			Singleton = this;
			base.Init();
			goalBarCO = new InstantiatedCoroutine(this);

			//onInfoPanelUpdate = delegate { Debug.Log("[CANVAS CORE MANAGER] Event onInfoPanelUpdate."); };
			restartGame = delegate { Debug.Log("[CANVAS CORE MANAGER] restart game."); };

			//onPanelChanged += PauseChanged;
			//onPanelOpen += InfoOpen;

			//AudioManager.StopAllMusic();

			//switch(Game.LevelData.scenery)
			//{
			//	case LevelData.Scenery.Desert:

			//		AudioManager.PlayMusic("desert");
			//		break;
			//	case LevelData.Scenery.City:
			//		AudioManager.PlayMusic("city");
			//		break;
			//	case LevelData.Scenery.Industrial:
			//		AudioManager.PlayMusic("industrial");
			//		break;
			//	default:
			//		AudioManager.PlayMusic("industrial");
			//		break;
			//}

		}

		public void InitGameGraphics()
		{
			Debug.Log("[CANVAS CORE MANAGER] Init game graphics.");

			goalBar.fillAmount = goalBarTarget = 0;

			// init game canvas graphics here
			//Game.Player.onTimeChange += t => { timeTxt.text = t > 60 ? TimerStrings.GetTimerMinutes(t, msLength: 1) : TimerStrings.GetTimerSeconds(t, msLength: 1); };
			//Game.Player.onScore += (f, s, n) =>
			//Game.Player.onScore += (f, s, n) => { ScoreUpdateUI(s, n); };
		}

		#region PANELS

		protected override void OpenPanel(string name)
		{
			if(PanelOpen)
				StartCoroutine(OpenPanelCo(""));// base.OpenPanel("");
			else
				StartCoroutine(OpenPanelCo(name));// base.OpenPanel(name);
		}

		IEnumerator OpenPanelCo(string name)
		{

			for(int i = 0; i < popIns.Length; i++)
			{
				if(name != "")
				{
					popIns[i].Open();
				}
				else
				{
					popIns[i].Close();
				}
				yield return popIns[i].c.Corutine;
			}

			base.OpenPanel(name);
		}

		#endregion

		protected override void Update()
		{
			base.Update();
			if(goalBarTarget != Game.Player.ScoreFill)
			{
				goalBarTarget = Game.Player.ScoreFill;
				AdvanceBar(goalBarCO, goalBarQueue, goalBarTarget, goalBar, delegate { });
			}
		}

		static void AdvanceBar(InstantiatedCoroutine co, Queue<Action> barQueue, float a, Image filler, Action setText)
		{
			bool s = barQueue.Count == 0;
			barQueue.Enqueue(delegate
			{
				if(a <= 0)
				{
					filler.fillAmount = 0;
					setText();
				}
				else
				{
					float start = filler.fillAmount;
					co.Start(.2f, t =>
					{
						filler.fillAmount = Mathf.Lerp(start, a, t);
						setText();
					}, delegate
					{
						if(barQueue.Count >= 1)
							barQueue.Dequeue().Invoke();
					}, true);
				}
			});
			if(!co.IsPlaying)
				barQueue.Dequeue().Invoke();
		}
	}
}
