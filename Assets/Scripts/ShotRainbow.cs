using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotRainbow : ShotSpecial
{
	protected override void DoTriggerCollision()
	{
		triggerHit = Physics.SphereCastAll(transform.position, collider.radius * 5f, rigidbody.velocity, collider.radius * 5f + 3f, LayerMask.GetMask("Pins"));
		if(triggerHit.Length > 0)
		{
			Pin p = triggerHit[0].collider.GetComponent<PinCollider>().Pin;
			for(int i = p.Tower.LayerLowestUnlocked.index; i < p.Tower.Layers; i++)
				for(int j = 0; j < p.Tower.LayerGet(i).pins.Count; j++)
					p.Tower.LayerGet(i).pins[j].Shooted(p.num, false);

			Game.GameInput.Shake(0.1f, 0.2f);
			CanvasManager.Vibrate();
			// reset shot and switch off object
			ShotEnd();
			AudioManager.PlaySfx("hit");
		}
	}
}
