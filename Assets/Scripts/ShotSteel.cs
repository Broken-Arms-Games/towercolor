using UnityEngine;
using Bag.Mobile.UiLite;

public class ShotSteel : ShotSpecial
{
	protected override void DoTriggerCollision()
	{
		triggerHit = Physics.SphereCastAll(transform.position, collider.radius * 2.2f, rigidbody.velocity, 0f, LayerMask.GetMask("Pins"));
		if(triggerHit.Length > 0 && Armed)
		{
			AudioManager.PlaySfx("hit");
		}
	}
}
