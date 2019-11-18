using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
	public float Height { get { return model.transform.localScale.y; } }

	public MeshRenderer model;
	public Rigidbody rigidbody;

	[HideInInspector] public int num;
	Tower tower;

	public void Init(Tower tower)
	{
		this.tower = tower;
		num = Random.Range(0, tower.pinMaterials.Length);

		model.material = tower.pinMaterials[num];
	}
}
