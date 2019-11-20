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

		new public static CanvasCoreManager Singleton { get; set; }

		[SerializeField] Image barraLivello;

		InstantiatedCoroutine goalBarCO;

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

			//goalBarHolder.SetActive(!Game.LevelIsSandbox);

			// init game canvas graphics here
			//Game.Player.onTimeChange += t => { timeTxt.text = t > 60 ? TimerStrings.GetTimerMinutes(t, msLength: 1) : TimerStrings.GetTimerSeconds(t, msLength: 1); };
			//Game.Player.onScore += (f, s, n) =>
			//Game.Player.onScore += (f, s, n) => { ScoreUpdateUI(s, n); };
		}

		protected override void OpenPanel(string name)
		{
			if(PanelOpen)
				StartCoroutine(OpenPanelCo(""));// base.OpenPanel("");
			else
				StartCoroutine(OpenPanelCo(name));// base.OpenPanel(name);
		}

		public void SetBar(float f)
		{
			barraLivello.fillAmount = f;
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

	}
}
