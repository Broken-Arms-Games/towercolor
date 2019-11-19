using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Generic;

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

	[HideInInspector] public int num;
	InstantiatedCoroutine initCo;


	public void Init(Action a)
	{
		num = UnityEngine.Random.Range(0, Game.Hub.tower.pinMaterials.Length);
		renderer.material = Game.Hub.tower.pinMaterials[num];
		rigidbody.isKinematic = true;

		ShootArmed = true;

		if(initCo == null)
			initCo = new InstantiatedCoroutine(this);
		initCo.Start(.3f, f =>
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * .5f, f);
		}, a);
	}

	public void Shoot(Vector3 target)
	{
		rigidbody.isKinematic = false;
		rigidbody.velocity = Physicf.BallisticLaunch(transform.position, target, 1f);
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
				// reset shot and switch off object
				ShotEnd();
			else
			{
				// disarm shot
				ShootArmed = false;
				// SUPER-BOUNCE!
				rigidbody.velocity = Physicf.BallisticLaunch(transform.position, Vector3.ProjectOnPlane(transform.position - other.transform.position, Vector3.up).normalized * 100, 20f);
				// reset shot
				Game.GameInput.ShotEnd();
			}
		}
	}

	void ShotEnd()
	{
		if(ShootArmed)
			Game.GameInput.ShotEnd();
		gameObject.SetActive(false);
	}
}
