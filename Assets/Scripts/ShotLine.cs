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
		if(triggerHit.Length > 0)
		{
			triggerHit = Physics.BoxCastAll(triggerHit[0].collider.transform.position, Vector3.up * .5f + Vector3.right * 100f + Vector3.forward * 100f, rigidbody.velocity, Quaternion.identity, 0.1f, LayerMask.GetMask("Pins"));
			for(int i = 0; i < triggerHit.Length; i++)
			{
				Pin p = triggerHit[i].collider.GetComponent<PinCollider>().Pin;
				p.Shooted(p.num, false);
			}

			Game.GameInput.Shake(0.1f, 0.2f);
			CanvasManager.Vibrate();
			// reset shot and switch off object
			ShotEnd();
			AudioManager.PlaySfx("hit");
		}
	}
}
