using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
	public static CanvasManager Singleton;
	public static Camera Camera { get { return Singleton.cam; } }


	[SerializeField] Text scoreText;
	[SerializeField] Camera cam;

	[Header("GAME OVER")]
	[SerializeField] GameObject gameOverPnl;
	[SerializeField] Text gameOverScoreTxt;
	[SerializeField] Text gameOverScoreTopTxt;

	[Header("PAUSE")]
	[SerializeField] Button pauseBtn;
	[SerializeField] GameObject pausaPopUp;

	[Header("OPTIONS")]
	[SerializeField] OptionsManager optionsManager;
	[SerializeField] Button replayBtn;

	[Header("Info panel")]
	[SerializeField] GameObject infoPanel;

	public event System.Action onInfoPanelUpdate = delegate { Debug.Log("[CANVAS CORE MANAGER] Event onInfoPanelUpdate."); };


	#region UNITY

	void Awake()
	{
		if(Singleton == null)
			Singleton = this;

		LoadingdAnimationManager.CloseLoading();
	}

	void Update()
	{
		optionsManager.BatteryAutoSet();
	}

	#endregion


	#region CLASS

	public void Init()
	{
		optionsManager.Init();
	}

	public static void SoundClick()
	{
		AudioManager.PlaySfx("ClickMenu");
	}

	#endregion


	#region UI

	public void GameStateUpdateUi(Game.State gameState)
	{
		switch(gameState)
		{
			case Game.State.End:
				GameOverOpen();
				break;
		}
	}

	void GameOverOpen()
	{
		gameOverPnl.SetActive(true);
		gameOverScoreTxt.text = "<b>SCORE: " + Game.Player.Score + "</b>";
		gameOverScoreTopTxt.text = "RECORD: " + Game.Player.ScoreTop;
	}

	public void ScoreUpdateUI(int score)
	{
		scoreText.text = score.ToString();
	}

	#endregion


	#region PAUSE

	public void Pause()
	{
		SoundClick();
		pausaPopUp.SetActive(!pausaPopUp.activeSelf);
		pauseBtn.gameObject.SetActive(!pausaPopUp.activeSelf);
		if(pausaPopUp.activeSelf)
			optionsManager.Init();
	}

	public void ReplayClick()
	{
		SoundClick();
		Game.Hub.Restart();
	}

	public void ReplayGameOverClick()
	{
		SoundClick();
		Game.Hub.Restart();
	}

	public void HomeClick()
	{
		SoundClick();
		LoadingdAnimationManager.StartLoading(() => { SceneManager.LoadScene(0); });
	}

	public void HomeGameOverClick()
	{
		SoundClick();
		LoadingdAnimationManager.StartLoading(() => { SceneManager.LoadScene(0); });
	}

	#endregion


	#region INFO_PANEL

	public void InfoPanelOpen()
	{
		SoundClick();
		pausaPopUp.SetActive(false);
		infoPanel.SetActive(true);
		onInfoPanelUpdate();
	}

	public void InfoPanelClose()
	{
		SoundClick();
		pausaPopUp.SetActive(true);
		infoPanel.SetActive(false);
	}

	#endregion
}
