using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotBomb : Shot
{
	public override void Init(Action a)
	{
		Material m = renderer.material;
		base.Init(a);
		renderer.material = m;
	}

	protected override void DoTriggerCollision()
	{
		triggerHit = Physics.SphereCastAll(transform.position, collider.radius * 5f, rigidbody.velocity, collider.radius * 5f + 3f, LayerMask.GetMask("Pins"));
		for(int i = 0; i < triggerHit.Length; i++)
		{
			triggerHit[i].collider.GetComponent<PinCollider>().Pin.rigidbody.AddExplosionForce(1000, transform.position, 5f);
			AudioManager.PlaySfx("hit");
			// reset shot and switch off object
			ShotEnd();
		}
	}
}
