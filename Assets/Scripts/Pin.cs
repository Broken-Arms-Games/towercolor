using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
	public float Height { get { return model.transform.localScale.y; } }

	public MeshRenderer model;
	public Rigidbody rigidbody;
	public PinCollider[] pinColliders;

	[HideInInspector] public int num;
	Tower tower;
	bool shooted;

	public void Init(Tower tower)
	{
		this.tower = tower;
		num = tower.GetRandomNum();
		shooted = false;

		gameObject.layer = LayerMask.NameToLayer("Pins");
		model.material = tower.pinMaterials[num];
		for(int i = 0; i < pinColliders.Length; i++)
			pinColliders[i].Init(this);
	}

	public bool Shooted(int num = -1)
	{
		if(!shooted && (num < 0 || num == this.num))
		{
			shooted = true;
			for(int i = 0; i < pinColliders.Length; i++)
				pinColliders[i].ShootNeighbours();
			gameObject.SetActive(false);
			return true;
		}
		else
			return false;
	}
}
