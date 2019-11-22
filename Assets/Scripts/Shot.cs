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

	[SerializeField] protected MeshRenderer renderer;
	[SerializeField] protected Rigidbody rigidbody;
	[SerializeField] protected SphereCollider collider;
	[SerializeField] AnimationCurve spawnAnim;

	[HideInInspector] public int num;
	InstantiatedCoroutine initCo;
	bool armed;
	protected RaycastHit[] triggerHit;


	public virtual void Init(Action a)
	{
		num = Game.Hub.tower.GetRandomNum();
		renderer.material = Game.Hub.tower.pinMaterials[num];
		rigidbody.velocity = Vector3.zero;
		rigidbody.isKinematic = true;
		transform.rotation = Quaternion.identity;

		Armed = true;

		if(initCo == null)
			initCo = new InstantiatedCoroutine(this);
		initCo.Start(.3f, f =>
		{
			transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one * 1f, spawnAnim.Evaluate(f));
		},
		delegate
		{
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

	protected virtual void DoTriggerCollision()
	{
		triggerHit = Physics.SphereCastAll(transform.position, collider.radius * 5f, rigidbody.velocity, collider.radius * 5f + 3f, LayerMask.GetMask("Pins"));
		for(int i = 0; i < triggerHit.Length; i++)
		{
			if(triggerHit[i].collider.GetComponent<PinCollider>().Pin.Shooted(num))
			{
				Game.GameInput.Shake(0.1f, 0.2f);
				CanvasManager.Vibrate();
				// reset shot and switch off object
				ShotEnd();
				AudioManager.PlaySfx("hit");
				break;
			}
		}
		if(triggerHit.Length > 0 && Armed)
		{
			// disarm shot
			Armed = false;
			AudioManager.PlaySfx("miss");
		}
	}

	protected void ShotEnd()
	{
		gameObject.SetActive(false);
		Game.GameInput.ShotEnd();
	}
}
