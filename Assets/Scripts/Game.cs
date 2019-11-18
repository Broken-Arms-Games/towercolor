using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static Game Hub { get; private set; }
	public static int Turns { get { return Hub.turns; } set { Hub.turns = value; } }
	public static State StateCurrent { get { return Hub.state; } }
	public static bool Finish
	{
		get
		{
			if(Hub.endCondition == null)
				return false;
			if(!Hub.end)
			{
				bool[] r = Hub.endCondition();
				Hub.win = r[1];
				return r[0];
			}
			else
				return true;
		}
	}

	//public static Player Player { get { return Hub.player; } }
	//public static GameInput GameInput { get { return Hub.gameInput; } }

	public enum State
	{
		Awake,
		Start,
		Play,
		Pause,
		End
	}

	class ActionTimed
	{
		public Func<float> time;
		public Func<bool> hold;
		public Action action;
	}

	// inspector
	[Header("References")]
	public Camera cam;
	//[SerializeField] GameInput gameInput;
	[Header("DEBUG")]
	[Tooltip("VISUALIZATION ONLY! DO NOT CHANGE!")]
	[SerializeField] State state;
	[SerializeField] Player player;


	[Header("SCORE")]
	public int score;

	List<ActionTimed>[] onStateActions;
	/// <summary>
	/// Length must be 2. First bool is true if the game is ended, second is true if the player wins.
	/// </summary>
	Func<bool[]> endCondition;
	int turns = 0;
	bool start;
	float removeCardDelay = 0.25f;
	bool end = false;
	bool win = false;
	bool hold;
	bool worldBuilded = false;


	IEnumerator gameCo;


	#region UNITY

	void Awake()
	{
		InitHub();
	}

	void Start()
	{
		StateAdvanceAutomatic();
	}

	void Update()
	{
		if(state == State.Play)
		{
			//player.Time -= Time.deltaTime;
			StateNext();
		}
	}

	#endregion


	#region INITIALIZATION

	void InitHub()
	{
		Debug.Log("[GAME] Init game hub.");

		Hub = this;
		onStateActions = new List<ActionTimed>[(int)State.End + 1];
		InitEvents();

		// leveldata here

		//gameCo = SetState(State.Awake);
		//StartCoroutine(gameCo);
		StartCoroutine(SetState(State.Awake));
	}

	public void Restart()
	{
		//StopCoroutine(gameCo);
		//OverrideState(State.Start);


		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	void WorldInit()
	{
	}

	void PlayerInstantiate()
	{
		Debug.Log("[GAME] Load player.");
		player = new Player();

		//if(!string.IsNullOrEmpty(PlayerSave.loading) && FileLoader.SaveExists(PlayerSave.loading))
		//{
		//	player = PlayerSave.LoadPlayer(PlayerSave.loading);
		//}
		//else
		//{
		//	player = new Player();
		//	player.InitSaveData();
		//}
	}

	void PlayerInit()
	{
		Debug.Log("[GAME] Init player.");
		player.Init();
	}

	void InitEvents()
	{
		SetStateAction(State.Awake, "[GAME] Awake event.");
		// json data loading
		AddStateAction(State.Awake, GameData.Init);
		// player instancing
		AddStateAction(State.Awake, PlayerInstantiate);
		// awake calls end
		Debug.Log("[GAME] Game has " + Hub.onStateActions[(int)State.Awake].Count + " " + State.Awake + " events.");

		SetStateAction(State.Start, "[GAME] Start event.");
		// ingame graphics initialization
		AddStateAction(State.Start, delegate
		{
			CanvasCoreManager.Singleton.InitGameGraphics();
			CanvasCoreManager.Singleton.restartGame += Restart;
		});
		// here
		// player initialization
		AddStateAction(State.Start, "[GAME] Start world initialization.");
		AddStateAction(State.Start, WorldInit);
		AddStateAction(State.Start, PlayerInit);
		AddStateAction(State.Start, delegate
		{
			//endCondition = delegate { return new bool[] { Player.Horde + 1 > LevelData.hordes.Length || Player.Time <= 0, Player.Horde + 1 > LevelData.hordes.Length }; };
		});
		AddStateAction(State.Start, "[GAME] Waiting world initialization...", delegate
		{
			return true; // TODO return when world initialization ready
		});
		AddStateAction(State.Start, delegate
		{
			// init game input
		});
		AddStateAction(State.Start, "[GAME] Initialization done.");
		// ingame data initialization
		// ui initialization
		// start game
		AddStateAction(State.Start, delegate
		{
			hold = true;
			LoadingdAnimationManager.CloseLoading(delegate { hold = false; });
		}, delegate { return LoadingdAnimationManager.Singleton == null || !hold; });
		// TODO scommentare le prossime 4 righe
		//AddStateAction(State.Start, delegate { CanvasCoreManager.ShowMessage("3"); }, delegate { return CanvasCoreManager.Msg == 0; });
		//AddStateAction(State.Start, delegate { CanvasCoreManager.ShowMessage("2"); }, delegate { return CanvasCoreManager.Msg == 0; });
		//AddStateAction(State.Start, delegate { CanvasCoreManager.ShowMessage("1"); }, delegate { return CanvasCoreManager.Msg == 0; });
		//AddStateAction(State.Start, delegate { CanvasCoreManager.ShowMessage("SUCK!"); }, delegate { return CanvasCoreManager.Msg == 0; });
		AddStateAction(State.Start, StateAdvanceAutomatic);
		//Debug.Log("[GAME] Game has " + Hub.onStateActions[(int)State.Start].Count + " " + State.Start + " events.");

		SetStateAction(State.Play, "[GAME] play event.");
		AddStateAction(State.Play, delegate { player.GameStateChanged(); });

		SetStateAction(State.End, delegate { Debug.Log("[GAME] End event."); });
		AddStateAction(State.End, delegate { player.GameStateChanged(); });
		AddStateAction(State.End, delegate { Debug.Log("[GAME] GAME ENDED! Player: " + (win ? "WINS!" : "loses.")); });
		AddStateAction(State.End, delegate
		{
			//if(PlayerPrefs.GetInt(LevelData.name + "-score_top") < Player.ScoreEnd)
			//	PlayerPrefs.SetInt(LevelData.name + "-score_top", Player.ScoreEnd);
		});
		AddStateAction(State.End, delegate
		{
			bool unlocked = false;
			if(win)
			{
				//unlocked = PlayerPrefs.GetInt(LevelData.name + "-complete", 0) == 0;
				//PlayerPrefs.SetInt(LevelData.name + "-complete", 1);
			}
			//CanvasCoreManager.Singleton.GameOver(win, Player.ScoreEnd, PlayerPrefs.GetInt(LevelData.name + "-score_top"), unlocked);
		});
		AddStateAction(State.End, delegate
		{

			// QUI METTI QUELLO CHE DEVI FARE QUANDO C'é IL GAMEOVER
			AudioManager.PlaySfx("GameOver");
			//TODO PLAYOND
			/*Playond.PLManager.GameOver(() =>
			{
				Debug.LogError("GameOver");
			});*/

		});
		//Debug.Log("[GAME] Game has " + Hub.onStateActions[(int)State.End].Count + " " + State.End + " events.");

		//Debug.Log("[GAME] Events inited.");
	}

	#endregion


	#region STATES

	static void SetStateAction(State state, Action action)
	{
		Hub.onStateActions[(int)state] = new List<ActionTimed>() { new ActionTimed() { action = action } };
	}

	static void SetStateAction(State state, string log, bool asError = false)
	{
		Hub.onStateActions[(int)state] = new List<ActionTimed>() { new ActionTimed() { action = delegate { if(!asError) Debug.Log(log); else Debug.LogError(log); }, time = null } };
	}

	public static void AddStateAction(State state, Action action)
	{
		Hub.onStateActions[(int)state].Add(new ActionTimed() { action = action });
	}

	public static void AddStateAction(State state, Action action, Func<float> time)
	{
		Hub.onStateActions[(int)state].Add(new ActionTimed() { action = action, time = time });
	}

	public static void AddStateAction(State state, Action action, Func<bool> hold)
	{
		Hub.onStateActions[(int)state].Add(new ActionTimed() { action = action, hold = hold });
	}

	public static void AddStateAction(State state, string log, bool asError = false)
	{
		Hub.onStateActions[(int)state].Add(new ActionTimed() { action = delegate { if(!asError) Debug.Log(log); else Debug.LogError(log); } });
	}

	public static void AddStateAction(State state, string log, Func<float> time, bool asError = false)
	{
		Hub.onStateActions[(int)state].Add(new ActionTimed() { action = delegate { if(!asError) Debug.Log(log); else Debug.LogError(log); }, time = time });
	}

	public static void AddStateAction(State state, string log, Func<bool> hold, bool asError = false)
	{
		Hub.onStateActions[(int)state].Add(new ActionTimed() { action = delegate { if(!asError) Debug.Log(log); else Debug.LogError(log); }, hold = hold });
	}

	public static void RemoveStateAction(State state, Action action)
	{
		ActionTimed toRemove = null;
		for(int i = 0; i < Hub.onStateActions[(int)state].Count; i++)
		{
			if(Hub.onStateActions[(int)state][i].action == action)
			{
				toRemove = Hub.onStateActions[(int)state][i];
				break;
			}
		}
		Hub.onStateActions[(int)state].Remove(toRemove);
	}

	void StateAdvanceAutomatic()
	{
		StartCoroutine(StateNextCO());
	}

	IEnumerator StateNextCO()
	{
		Debug.Log("[GAME HUB] Turn " + Turns + " state " + state.ToString() + " next animation.");

		//if(state == State.TurnPRE && Plyr.HandRemove.Count > 0)
		//	yield return new WaitForSeconds(CanvasManager.Singleton.cardUiMoveTime + RemoveCardDelay * Plyr.HandRemove.Count);
		/*else*/
		if((int)state > (int)State.Start)
			yield return new WaitForSeconds(0.01f);

		StateNext();
	}

	public static void StateNext()
	{
		if(Finish)
			Hub.StartCoroutine(SetState(State.End));
		else if(Hub.state == State.Pause)
			Hub.StartCoroutine(SetState(State.Play));
		else if(Hub.state == State.Play)
			return;
		else if(Hub.state != State.End)
			Hub.StartCoroutine(SetState((State)((int)Hub.state + 1)));
		else
		{
			Debug.LogError("[GAME] Game cycle ended.");
			return;
		}
	}

	public static void OverrideState(State state)
	{
		Hub.StartCoroutine(SetState(state));
	}

	static IEnumerator SetState(State state)
	{
		Hub.state = state;
		int stateIndex = (int)state;
		for(int i = 0; i < Hub.onStateActions[stateIndex].Count; i++)
		{
			Hub.onStateActions[stateIndex][i].action();
			//if(Hub.onStateActions[stateIndex][i].time != null && Hub.onStateActions[stateIndex][i].time() > 0 && (int)state > (int)State.Start)
			if(Hub.onStateActions[stateIndex][i].time != null && Hub.onStateActions[stateIndex][i].time() > 0)
				yield return new WaitForSeconds(Hub.onStateActions[stateIndex][i].time());
			if(Hub.onStateActions[stateIndex][i].hold != null)
				while(!Hub.onStateActions[stateIndex][i].hold())
					yield return null;
		}
	}

	#endregion

	static void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
