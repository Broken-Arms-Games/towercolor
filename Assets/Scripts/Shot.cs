using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Generic;
using Bag.Mobile.UiLite;

public class Shot : MonoBehaviour
{
	public bool Armed
	{
		get { return armed; }
		set
		{
			armed = value;
		}
	}

	[SerializeField] MeshRenderer renderer;
	[SerializeField] Rigidbody rigidbody;
	[SerializeField] SphereCollider collider;

	[HideInInspector] public int num;
	InstantiatedCoroutine initCo;
	bool armed;
	RaycastHit[] triggerHit;
	RaycastHit triggerHitNear;


	public void Init(Action a)
	{
		num = Game.Hub.tower.GetRandomNum();
		renderer.material = Game.Hub.tower.pinMaterials[num];
		rigidbody.velocity = Vector3.zero;
		rigidbody.isKinematic = true;
		transform.rotation = Quaternion.identity;

		Armed = true;
		//collider.enabled = false;

		if(initCo == null)
			initCo = new InstantiatedCoroutine(this);
		initCo.Start(.3f, f =>
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1f, f);
		},
		delegate
		{
			//collider.enabled = true;
			a();
		});
	}

	public void Shoot(Vector3 target)
	{
		rigidbody.isKinematic = false;
		rigidbody.velocity = Physicf.BallisticLaunch(transform.position, target, Game.GameInput.ShotSpeed, Game.GameInput.ShotArch);
	}

	void FixedUpdate()
	{
		if(Armed)
			DoTriggerCollision();
	}

	void Update()
	{
		if(!Armed && rigidbody.velocity.magnitude < Game.GameInput.ShotSpeed / 3f)
			rigidbody.velocity = rigidbody.velocity.normalized * (Game.GameInput.ShotSpeed / 3f);
		if(transform.position.y < 0)
			ShotEnd();
	}

	void DoTriggerCollision()
	{
		triggerHit = Physics.SphereCastAll(transform.position, collider.radius * 10f, rigidbody.velocity, 1f, LayerMask.GetMask("Pins"));
		//for(int i = 0; i < triggerHit.Length; i++)
		//	if(i == 0 || triggerHit[i].distance < triggerHitNear.distance)
		//		triggerHitNear = triggerHit[i];
		//if(triggerHit.Length > 0)
		for(int i = 0; i < triggerHit.Length; i++)
		{
			if(triggerHit[i].collider.GetComponent<PinCollider>().Pin.Shooted(num))
			{
				Game.GameInput.Shake(0.1f, 0.2f);
				CanvasManager.Vibrate();
				// reset shot and switch off object
				ShotEnd();
				break;
			}
		}
		if(triggerHit.Length > 0 && Armed)
		{
			// disarm shot
			Armed = false;
			// SUPER-BOUNCE!
			//SuperBounce(other);
			//collider.enabled = false;
		}
	}

	void SuperBounce(Collider other)
	{
		rigidbody.velocity = Physicf.BallisticLaunch(transform.position, Vector3.ProjectOnPlane(transform.position - transform.position, Vector3.up).normalized * 100, 20f);
	}

	void ShotEnd()
	{
		gameObject.SetActive(false);
	}
}
