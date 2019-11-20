﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
	public float Height { get { return model.transform.localScale.y; } }
	public bool Locked
	{
		get { return locked; }
		set
		{
			if(locked != value)
			{
				locked = value;
				if(locked)
				{
					model.material = tower.pinMaterialLock;
					rigidbody.isKinematic = true;
					tower.OnPinUnlock(this);
				}
				else
				{
					model.material = tower.pinMaterials[num];
					rigidbody.isKinematic = false;
				}
			}
		}
	}

	public MeshRenderer model;
	public Rigidbody rigidbody;
	public PinCollider[] pinColliders;
	public ParticleSystem particle;

	[HideInInspector] public int num;
	Tower tower;
	bool shooted;
	bool locked = true;


	public void Init(Tower tower, Tower.LayerData layer)
	{
		this.tower = tower;
		num = tower.GetRandomNum();
		shooted = false;

		gameObject.layer = LayerMask.NameToLayer("Pins");

		model.enabled = true;
		model.material = tower.pinMaterialLock;
		rigidbody.isKinematic = true;
		Locked = layer.index < tower.Layers - tower.layersUnlocked;
		for(int i = 0; i < pinColliders.Length; i++)
		{
			pinColliders[i].enabled = true;
			pinColliders[i].Init(this);
		}
	}

	public bool Shooted(int num = -1)
	{
		if(!locked && !shooted && (num < 0 || num == this.num))
		{
			shooted = true;
			for(int i = 0; i < pinColliders.Length; i++)
			{
				pinColliders[i].enabled = false;
				pinColliders[i].ShootNeighbours();
			}
			model.enabled = false;
			rigidbody.isKinematic = true;
			StartCoroutine(Disable(2));
			tower.OnPinShoot(this);
			return true;
		}
		else
			return false;
	}

	IEnumerator Disable(float wait)
	{
		yield return new WaitForSeconds(wait);
		gameObject.SetActive(false);
	}

	public void PlayParticle()
	{
		particle.GetComponent<ParticleSystemRenderer>().material = tower.pinMaterials[num];
		particle.gameObject.SetActive(true);
	}

	public void PlayParticleLocket()
	{
		particle.GetComponent<ParticleSystemRenderer>().material = tower.pinMaterialLock;
		particle.gameObject.SetActive(true);
	}
}
