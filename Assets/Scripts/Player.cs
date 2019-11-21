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
	public int Shots { get { return shots; } }

	int score;
	int shots;
	Tower tower;

	public event Action<int, int> onScoreChange = (s, c) => { };
	public event Action<int, int> onShotChange = (s, c) => { };

	public void Init(Tower tower)
	{
		this.tower = tower;
	}

	public void StartLevel(int levelIndex)
	{
		Score = 0;
		shots = tower.Layers + 2;
		onShotChange(shots, shots);

	}

	public void GameStateChanged()
	{

	}

	public bool Shoot()
	{
		if(shots > 0)
		{
			onShotChange(shots--, shots);
			return true;
		}
		else
			return false;
	}
}
