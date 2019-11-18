using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Generic;

public class GameData
{
	const string fileName = "gameData";
	public static GameData data;
	public static int IntData { get { return data.intData; } }

	[SerializeField] string debug = "fail";
	[SerializeField] public int intData;

	public static void Init()
	{
		if(FileLoader.JsonExists("gameData"))
		{
			data = JsonUtility.FromJson<GameData>(FileLoader.LoadJson(fileName));
			Debug.Log("[GAME DATA] Game data loaded from database json file '" + fileName + "'. Load result is '" + data.debug + "'.");
		}
		else
			data = new GameData();
	}
}
