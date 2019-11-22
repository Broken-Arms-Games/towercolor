using System;
using UnityEngine;

public class ShotSpecial : Shot
{
	public override void Init(Action a)
	{
		Material m = renderer.material;
		base.Init(a);
		renderer.material = m;
	}
}
