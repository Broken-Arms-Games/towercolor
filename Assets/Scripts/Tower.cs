using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Extensions;

public class Tower : MonoBehaviour
{
	[SerializeField] Transform spawnBase;
	[SerializeField] Pin pinPrefab;
	public Material[] pinMaterials;

	List<Pin> pinPool;
	int layerAmount = 15;
	float layerRadius = 3.6f;

	public void SpawnLevel(int layers = 10)
	{
		for(int i = 0; i < spawnBase.childCount; i++)
			spawnBase.GetChild(i).gameObject.SetActive(false);

		float angle = 360f / layerAmount;
		for(int i = 0; i < layers; i++)
			spawnBase.AddPoolList(pinPrefab, layerAmount, ref pinPool, (p, j) =>
			{
				p.transform.localRotation = Quaternion.identity;
				p.transform.localPosition =
					Quaternion.Euler(0, angle * j + (i % 2 == 0 ? 0 : angle / 2f), 0) * (Vector3.forward * layerRadius) +
					(Vector3.up * (pinPrefab.Height * 2f * i + pinPrefab.Height));
				p.Init(this);
			});
	}
}
