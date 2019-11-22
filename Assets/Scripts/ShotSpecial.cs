using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotSpecial : Shot
{
	public override void Init(Action a)
	{
		Material m = renderer.material;
		base.Init(a);
		renderer.material = m;
	}
}
