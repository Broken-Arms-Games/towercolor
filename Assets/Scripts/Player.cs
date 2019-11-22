using System;
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
	public int SpecialsCount { get { return specials.Length; } }

	int score;
	int shots;
	Tower tower;
	float[] specials;

	public event Action<int, int> onScoreChange = (s, c) => { };
	public event Action<int, int> onShotChange = (s, c) => { };
	public event Action<int, float> onSpecialChange = (i, s) => { };

	public void Init(Tower tower)
	{
		this.tower = tower;
		specials = new float[tower.pinMaterials.Length];
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

	public void SpecialAdd(int num)
	{
		specials[num] = Mathf.Clamp01(specials[num] + 1f / 15);
		onSpecialChange(num, specials[num]);
	}

	public float SpecialGet(int num)
	{
		return specials[num];
	}

	public bool SpecialUse(int num)
	{
		if(specials[num] >= 1)
		{
			specials[num] = Mathf.Clamp01(specials[num] - 1f);
			onSpecialChange(num, specials[num]);
			return true;
		}
		else
			return false;
	}
}
