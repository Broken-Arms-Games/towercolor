using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotBomb : Shot
{
	[SerializeField] GameObject expolosion;

	public override void Init(Action a)
	{
		Material m = renderer.material;
		base.Init(a);
		renderer.material = m;

		// TODO explosion effect initialization
	}

	protected override void DoTriggerCollision()
	{
		triggerHit = Physics.SphereCastAll(transform.position, collider.radius * 5f, rigidbody.velocity, collider.radius * 5f + 3f, LayerMask.GetMask("Pins"));
		if(triggerHit.Length > 0)
		{
			triggerHit = Physics.SphereCastAll(transform.position, 2f, rigidbody.velocity, 2f, LayerMask.GetMask("Pins"));
			for(int i = 0; i < triggerHit.Length; i++)
				triggerHit[i].collider.GetComponent<PinCollider>().Pin.rigidbody.AddExplosionForce(100f, transform.position, 4f, 1.5f, ForceMode.VelocityChange);
			AudioManager.PlaySfx("hit");
			// reset shot and switch off object
			ShotEnd();

			// TODO play explosion effect
			expolosion.SetActive(true);
		}
	}
}
