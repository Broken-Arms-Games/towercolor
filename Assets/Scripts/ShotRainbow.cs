using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotRainbow : Shot
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
			Pin p = triggerHit[i].collider.GetComponent<PinCollider>().Pin;
			if(p.Shooted(p.num))
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
