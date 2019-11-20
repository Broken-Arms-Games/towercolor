using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public int Score
	{
		get { return score; }
		set
		{
			if(value != score)
				onScoreChange(score, value);
			score = value;
		}
	}
	public float ScoreFill { get { return Score / (float)Game.Hub.tower.ScoreMax; } }

	int score;

	public event Action<int, int> onScoreChange = (s, c) => { };

	public void Init()
	{
		Score = 0;
	}

	public void GameStateChanged()
	{

	}
}
