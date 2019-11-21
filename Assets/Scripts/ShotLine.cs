using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotLine : Shot
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
			List<Pin> p = triggerHit[i].collider.GetComponent<PinCollider>().Pin.Layer.pins;
			for(int j = 0; j < p.Count; j++)
			{
				if(p[j].Shooted(p[j].num, false))
				{
					Game.GameInput.Shake(0.1f, 0.2f);
					CanvasManager.Vibrate();
					// reset shot and switch off object
					ShotEnd();
					AudioManager.PlaySfx("hit");
					break;
				}
			}
		}
	}
}
