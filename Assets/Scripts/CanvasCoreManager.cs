﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Generic;
using UnityEngine.UI;
using TMPro;
using Bag.Scripts.Extensions;

namespace Bag.Mobile.UiLite
{
	public class CanvasCoreManager : CanvasManager
	{
		public TextMeshProUGUI numberBallsText;
		public AnimationCurve numberBallsCurve;
		public PopIn[] popIns;
		public TextMeshProUGUI[] textPowers;
		public GameObject[] nextBalls;
		public Image[] imgPowers;
		public Image[] iconPowers;
		public AnimationCurve iconPowerAnim;
		public TextMeshProUGUI levelNumberText;
		public Image tapToStart;
		public Image gameOver;
		public Image levelComplete;
		public Button btnReplay;
		public Button btnNextLevel;
		public Image whiteFadeInOut;

		new public static CanvasCoreManager Singleton { get; set; }

		[SerializeField] Image goalBar;

		float goalBarTarget;
		InstantiatedCoroutine whiteInOutCo;
		InstantiatedCoroutine goalBarCo;
		InstantiatedCoroutine[] powerBarCo;
		InstantiatedCoroutine[] powerIconCo;
		InstantiatedCoroutine initCo;
		Queue<Action> goalBarQueue = new Queue<Action>();
		Queue<Action>[] powerQueue;

		public event Action restartGame;

		protected override void Init()
		{
			Singleton = this;
			base.Init();
			goalBarCo = new InstantiatedCoroutine(this);
			powerBarCo = new InstantiatedCoroutine[imgPowers.Length];
			for(int i = 0; i < powerBarCo.Length; i++)
				powerBarCo[i] = new InstantiatedCoroutine(this);
			powerIconCo = new InstantiatedCoroutine[imgPowers.Length];
			for(int i = 0; i < powerIconCo.Length; i++)
				powerIconCo[i] = new InstantiatedCoroutine(this);
			powerQueue = new Queue<Action>[imgPowers.Length];
			for(int i = 0; i < powerQueue.Length; i++)
				powerQueue[i] = new Queue<Action>();
		}

		public void InitGameGraphics()
		{
			goalBar.fillAmount = goalBarTarget = 0;
			Game.Player.onScoreChange += GoalBarUpdate;
			Game.Player.onShotChange += ShotCountUpdate;
			Game.Player.onSpecialChange += PowerUpdate;

			Game.GameInput.onShotSpawn += ShotSpawnDisplay;

			if(whiteInOutCo == null)
				whiteInOutCo = new InstantiatedCoroutine(this);
			whiteInOutCo.Start(1, t =>
			{
				whiteFadeInOut.color = Color.white.ToAlpha(1 - t);
			}, null);
		}

		public void InitLevel(int levelIndex)
		{
			levelNumberText.text = (levelIndex + 1).ToString("00");
		}

		public void GameOver(bool win)
		{
			gameOver.gameObject.SetActive(!win);
			levelComplete.gameObject.SetActive(win);
			btnReplay.gameObject.SetActive(!win);
			btnNextLevel.gameObject.SetActive(win);
			btnReplay.onClick.RemoveAllListeners();
			btnReplay.onClick.AddListener(ReloadScene);
			btnNextLevel.onClick.RemoveAllListeners();
			btnNextLevel.onClick.AddListener(ReloadScene);
			OpenPanel("gameover");
		}

		public void SetTapToStart(bool value)
		{
			tapToStart.gameObject.SetActive(value);
		}

		void ReloadScene()
		{
			btnReplay.gameObject.SetActive(false);
			btnNextLevel.gameObject.SetActive(false);

			if(whiteInOutCo == null)
				whiteInOutCo = new InstantiatedCoroutine(this);
			whiteInOutCo.Start(1, t =>
			{
				whiteFadeInOut.color = Color.white.ToAlpha(t);
			}, delegate
			{
				Game.Hub.Restart();
			});
		}

		#region PANELS

		protected override void OpenPanel(string name)
		{
			if(name == "pnl_pause")
			{
				if(PanelOpen)
					StartCoroutine(OpenPanelCo(""));
				else
					StartCoroutine(OpenPanelCo(name));
			}
			else
				base.OpenPanel(name);

		}

		IEnumerator OpenPanelCo(string name)
		{
			if(name == "") //panel -> Close
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
				AdvanceBar(goalBarCo, goalBarQueue, goalBarTarget, goalBar, delegate { });
			}
		}

		void PowerUpdate(int i, float f)
		{
			//Update power ups bar
			nextBalls[i].SetActive(f >= 1);
			AdvanceBar(powerBarCo[i], powerQueue[i], f, imgPowers[i], delegate
			{
			});
			PowerIconFillAnim(i);
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
					co.Start(.1f, t =>
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

		void PowerIconFillAnim(int i)
		{
			if(!powerIconCo[i].IsPlaying)
			{
				powerIconCo[i].Start(.2f, t =>
				{
					iconPowers[i].transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, iconPowerAnim.Evaluate(t));
				}, null);
			}
		}

		void ShotSpawnDisplay(Shot s)
		{
			if(s is ShotBomb || s is ShotSteel)
				numberBallsText.color = Color.white.ToAlpha(0.6f);
			else
				numberBallsText.color = Color.black.ToAlpha(0.6f);


			if(initCo == null)
				initCo = new InstantiatedCoroutine(this);

			initCo.Start(.3f, f =>
			{
				numberBallsText.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one * 1f, numberBallsCurve.Evaluate(f));
			}, null);
		}

		#region BUTTONS

		public void ResetOkClick()
		{
			PlayerPrefs.DeleteKey("levelIndex");
			ReloadScene();
		}

		#endregion
	}
}
