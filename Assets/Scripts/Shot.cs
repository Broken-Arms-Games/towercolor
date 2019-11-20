using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Generic;
using Bag.Mobile.UiLite;

public class Shot : MonoBehaviour
{
	public bool ShootArmed
	{
		get { return trigger.enabled; }
		set
		{
			trigger.enabled = value;
		}
	}

	[SerializeField] MeshRenderer renderer;
	[SerializeField] Rigidbody rigidbody;
	[SerializeField] SphereCollider trigger;
	[SerializeField] SphereCollider collider;

	[HideInInspector] public int num;
	InstantiatedCoroutine initCo;


	public void Init(Action a)
	{
		num = Game.Hub.tower.GetRandomNum();
		renderer.material = Game.Hub.tower.pinMaterials[num];
		rigidbody.isKinematic = true;

		ShootArmed = true;
		collider.enabled = false;

		if(initCo == null)
			initCo = new InstantiatedCoroutine(this);
		initCo.Start(.3f, f =>
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1f, f);
		},
		delegate
		{
			collider.enabled = true;
			a();
		});
	}

	public void Shoot(Vector3 target)
	{
		rigidbody.isKinematic = false;
		rigidbody.AddForce(Physicf.BallisticLaunch(transform.position, target, Game.GameInput.ShotSpeed, Game.GameInput.ShotArch), ForceMode.VelocityChange);
	}

	void Update()
	{
		if(transform.position.y < 0)
			ShotEnd();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(ShootArmed && other.gameObject.layer == LayerMask.NameToLayer("Pins"))
		{
			if(other.GetComponent<PinCollider>().Pin.Shooted(num))
			{
				Game.GameInput.Shake(0.1f, 0.2f);
				CanvasManager.Vibrate();
				// reset shot and switch off object
				ShotEnd();
			}
			else
			{
				// disarm shot
				ShootArmed = false;
				// SUPER-BOUNCE!
				//SuperBounce(other);
				//collider.enabled = false;
			}
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
