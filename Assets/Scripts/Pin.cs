using System.Collections;
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
				SetLockedState(locked);
			}
		}
	}
	public Tower Tower { get; private set; }
	public Tower.LayerData Layer { get; private set; }

	public MeshRenderer model;
	public Rigidbody rigidbody;
	public PinCollider[] pinColliders;

	[HideInInspector] public int num;
	bool shooted;
	bool locked = true;


	public void Init(Tower tower, Tower.LayerData layer)
	{
		this.Tower = tower;
		this.Layer = layer;
		num = tower.GetRandomNum();
		shooted = false;

		gameObject.layer = LayerMask.NameToLayer("Pins");

		model.enabled = true;
		model.material = tower.pinMaterials[num];
		rigidbody.isKinematic = true;
		for(int i = 0; i < pinColliders.Length; i++)
			pinColliders[i].Init(this);
		locked = Layer.index < tower.Layers - tower.layersUnlocked;
	}

	public void SetLockedState(bool locked)
	{
		if(locked)
		{
			model.material = Tower.pinMaterialLock;
			rigidbody.isKinematic = true;
		}
		else
		{
			model.material = Tower.pinMaterials[num];
			rigidbody.isKinematic = false;
			if(Game.StateCurrent == Game.State.Play)
				Tower.OnPinUnlock(this);
		}
		for(int i = 0; i < pinColliders.Length; i++)
			pinColliders[i].SetLayer(locked);
	}

	public bool Shooted(int num = -1, bool chain = true)
	{
		if(!locked && !shooted && (num < 0 || num == this.num))
		{
			if(chain)
				StartCoroutine(ShootChainCo());
			else
				gameObject.SetActive(false);
			shooted = true;
			model.enabled = false;
			rigidbody.isKinematic = true;
			Tower.OnPinShoot(this);
			Game.Player.SpecialAdd(this.num);
			return true;
		}
		else
			return false;
	}

	IEnumerator ShootChainCo()
	{
		yield return Tower.wait;
		gameObject.SetActive(false);
		for(int i = 0; i < pinColliders.Length; i++)
			pinColliders[i].ShootNeighbours();
	}
}
