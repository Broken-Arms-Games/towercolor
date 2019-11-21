using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PinCollider : PinComponent
{
	public BoxCollider Collider { get { return collider; } }

	[SerializeField] BoxCollider collider;

	RaycastHit[] triggerHit;

	public override void Init(Pin pin)
	{
		gameObject.SetActive(true);
		base.Init(pin);
		gameObject.layer = pin.gameObject.layer;
		collider.enabled = true;
	}

	public void ShootNeighbours()
	{
		triggerHit = Physics.BoxCastAll(collider.bounds.center, collider.size / 2f + Vector3.one * .08f, Vector3.up, transform.rotation, 0f, LayerMask.GetMask("Pins"));
		if(triggerHit.Length > 0)
		{
			PinCollider pc = null;
			for(int i = 0; i < triggerHit.Length; i++)
			{
				pc = triggerHit[i].collider.GetComponent<PinCollider>();
				if(pc.Pin != Pin)
					pc.Pin.Shooted(Pin.num);
			}
		}
	}
}
