using System.Collections;
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
				}
				else
				{
					model.material = tower.pinMaterials[num];
					rigidbody.isKinematic = false;
					tower.OnPinUnlock(this);
				}
			}
		}
	}
	public Tower.LayerData Layer { get { return layer; } }

	public MeshRenderer model;
	public Rigidbody rigidbody;
	public PinCollider[] pinColliders;

	[HideInInspector] public int num;
	Tower tower;
	Tower.LayerData layer;
	bool shooted;
	bool locked = true;


	public void Init(Tower tower, Tower.LayerData layer)
	{
		this.tower = tower;
		this.layer = layer;
		num = tower.GetRandomNum();
		shooted = false;

		gameObject.layer = LayerMask.NameToLayer("Pins");

		model.enabled = true;
		model.material = tower.pinMaterialLock;
		rigidbody.isKinematic = true;
		Locked = Layer.index < tower.Layers - tower.layersUnlocked;
		for(int i = 0; i < pinColliders.Length; i++)
			pinColliders[i].Init(this);
	}

	public bool Shooted(int num = -1, bool chain = true)
	{
		if(!locked && !shooted && (num < 0 || num == this.num))
		{
			shooted = true;
			for(int i = 0; chain && i < pinColliders.Length; i++)
				pinColliders[i].ShootNeighbours();
			model.enabled = false;
			rigidbody.isKinematic = true;
			gameObject.SetActive(false);
			tower.OnPinShoot(this);
			Game.Player.SpecialAdd(this.num);
			return true;
		}
		else
			return false;
	}
}
