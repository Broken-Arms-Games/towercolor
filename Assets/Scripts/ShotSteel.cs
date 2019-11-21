using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotSteel : Shot
{
	public override void Init(Action a)
	{
		Material m = renderer.material;
		base.Init(a);
		renderer.material = m;
	}

	protected override void DoTriggerCollision()
	{
		triggerHit = Physics.SphereCastAll(transform.position, collider.radius * 2.2f, rigidbody.velocity, 0f, LayerMask.GetMask("Pins"));
		if(triggerHit.Length > 0 && Armed)
		{
			AudioManager.PlaySfx("hit");
		}
	}
}
