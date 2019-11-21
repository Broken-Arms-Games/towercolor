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
		public Image[] imgPowers;
		public TextMeshProUGUI levelNumberText;
		public Image tapToStart;
		public Image gameOver;
		public Image levelComplete;

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
			Game.Player.onScoreChange += GoalBarUpdate;
			Game.Player.onShotChange += ShotCountUpdate;

			// init game canvas graphics here
			//Game.Player.onTimeChange += t => { timeTxt.text = t > 60 ? TimerStrings.GetTimerMinutes(t, msLength: 1) : TimerStrings.GetTimerSeconds(t, msLength: 1); };
			//Game.Player.onScore += (f, s, n) =>
			//Game.Player.onScore += (f, s, n) => { ScoreUpdateUI(s, n); };
		}

		public void InitLevel(int levelIndex)
		{
			levelNumberText.text = (levelIndex + 1).ToString("00");
		}

		public void GameOver(bool win)
		{
			gameOver.gameObject.SetActive(!win);
			levelComplete.gameObject.SetActive(win);
			OpenPanel("gameover");
		}

		#region PANELS

		protected override void OpenPanel(string name)
		{
			if(PanelOpen)
				StartCoroutine(OpenPanelCo(""));
			else
				StartCoroutine(OpenPanelCo(name));
		}

		IEnumerator OpenPanelCo(string name)
		{
			//panel -> Close
			if(name == "")
			{
				for(int i = popIns.Length - 1; i > 0; i--)
				{
					popIns[i].Close();
					yield return popIns[i].c.Corutine;
				}

				base.OpenPanel(name);
			}
			else  //panel -> Open
			{
				base.OpenPanel(name);
				for(int i = 0; i < popIns.Length; i++)
				{
					popIns[i].Open();
					yield return popIns[i].c.Corutine;
				}
			}
		}

		#endregion

		void ShotCountUpdate(int o, int n)
		{
			numberBallsText.text = n.ToString("00");
		}

		void GoalBarUpdate(int o, int n)
		{
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

		public void SetTapToStart(bool value)
		{
			tapToStart.gameObject.SetActive(value);
		}
	}
}
