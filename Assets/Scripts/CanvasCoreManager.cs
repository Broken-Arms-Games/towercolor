﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Generic;
using UnityEngine.UI;

namespace Bag.Mobile.UiLite
{
	public class CanvasCoreManager : CanvasManager
	{
		public Animation settings_anim;

		new public static CanvasCoreManager Singleton { get; set; }
		public static JoystickInput Joystick { get { return Singleton.joystick; } }

		[SerializeField] JoystickInput joystick;

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
				base.OpenPanel("");
			else
				base.OpenPanel(name);
		}

	}
}
