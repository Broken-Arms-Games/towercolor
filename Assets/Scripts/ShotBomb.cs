using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotBomb : ShotSpecial
{
	[SerializeField] GameObject explosion;

	public override void Init(Action a)
	{
		renderer.enabled = true;
		collider.enabled = true;
		rigidbody.isKinematic = false;
		explosion.SetActive(false);
		base.Init(a);
	}

	protected override void DoTriggerCollision()
	{
		triggerHit = Physics.SphereCastAll(transform.position, collider.radius * 8f, rigidbody.velocity, collider.radius * 8f, LayerMask.GetMask("Pins"));
		if(triggerHit.Length > 0)
		{
			triggerHit = Physics.SphereCastAll(transform.position, 2f, rigidbody.velocity, 2f, LayerMask.GetMask("Pins"));
			for(int i = 0; i < triggerHit.Length; i++)
				triggerHit[i].collider.GetComponent<PinCollider>().Pin.rigidbody.AddExplosionForce(100f, transform.position, 4f, 1.5f, ForceMode.VelocityChange);
			AudioManager.PlaySfx("hit");
			// reset shot and switch off object
			StartCoroutine(ShotEndCo());
		}
	}

	IEnumerator ShotEndCo()
	{
		renderer.enabled = false;
		collider.enabled = false;
		rigidbody.isKinematic = true;
		explosion.SetActive(true);
		yield return new WaitForSeconds(2f);
		ShotEnd();
	}
}
