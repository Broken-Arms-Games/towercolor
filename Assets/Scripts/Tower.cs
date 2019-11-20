using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Extensions;

public class Tower : MonoBehaviour
{
	[Serializable]
	public class LayerData
	{
		public int index;
		public List<Pin> pins;
		public float posHeight;
		public bool locked;

		bool active;

		public LayerData(int index, float posHeight)
		{
			this.index = index;
			this.posHeight = posHeight;
			pins = new List<Pin>();
			locked = true;
			active = true;
		}

		public bool Active
		{
			get
			{
				if(active)
				{
					for(int i = 0; i < pins.Count; i++)
						if(pins[i].gameObject.activeSelf && (pins[i].transform.localPosition.y + 1) > posHeight)
							return true;
					active = false;
				}
				return false;
			}
		}

		public void Unlock()
		{
			locked = false;
			for(int i = 0; i < pins.Count; i++)
				pins[i].Locked = false;
		}
	}

	public int Layers { get { return layers; } }
	public LayerData LayerLowestUnlocked { get { return layerList[layerTop - layersUnlocked]; } }

	[SerializeField] Transform spawnBase;
	[SerializeField] Pin pinPrefab;
	public Material pinMaterialLock;
	public Material[] pinMaterials;
	public int layersUnlocked = 8;

	List<Pin> pinPool;
	LayerData[] layerList;
	int layers;
	int layerAmount = 15;
	float layerRadius = 3.6f;
	System.Random rand;
	int layerTop = -1;

	public event Action<LayerData> onLayerUnlock = l => { Debug.Log("[TOWER] Layer unlock at index " + l.index + "."); };
	public event Action<Pin> onPinUnlock = p => { };
	public event Action<Pin> onPinShoot = p => { };


	public void Init()
	{
		onPinUnlock += p => { /*particelle*/ };
		onPinShoot += p => { /*particelle*/ };
	}

	public void SpawnLevel(int layers = 10)
	{
		this.layers = layers;
		for(int i = 0; i < spawnBase.childCount; i++)
			spawnBase.GetChild(i).gameObject.SetActive(false);

		float angle = 360f / layerAmount;
		layerList = new LayerData[layers];
		for(int i = 0; i < layers; i++)
		{
			layerList[i] = new LayerData(i, pinPrefab.Height * 2f * i + pinPrefab.Height);
			spawnBase.AddPoolList(pinPrefab, layerAmount, ref pinPool, (p, j) =>
			{
				p.transform.localRotation = Quaternion.identity;
				p.transform.localPosition =
					Quaternion.Euler(0, angle * j + (i % 2 == 0 ? 0 : angle / 2f), 0) * (Vector3.forward * layerRadius) +
					(Vector3.up * layerList[i].posHeight);
				p.Init(this, layerList[i]);
				layerList[i].pins.Add(p);
			});
		}
		layerTop = layers - 1;
		LayerUnlock();
	}

	public int GetRandomNum()
	{
		if(rand == null)
			rand = new System.Random(layerAmount + UnityEngine.Random.Range(0, int.MaxValue - layers));
		return rand.Next(pinMaterials.Length);
	}

	void Update()
	{
		if(layerTop >= 0)
		{
			for(int i = layerTop; i >= 0; i--)
			{
				if(!layerList[i].Active)
				{
					layerTop = i - 1;
					LayerUnlock();
					break;
				}
			}
		}
	}

	void LayerUnlock()
	{
		for(int i = layerTop; i >= Mathf.Max(0, layerTop + 1 - layersUnlocked); i--)
		{
			if(layerList[i].locked)
			{
				layerList[i].Unlock();
				onLayerUnlock(layerList[i]);
			}
		}
	}

	#region PIN_EVENTS

	public void OnPinShoot(Pin pin)
	{
		onPinShoot(pin);
	}

	public void OnPinUnlock(Pin pin)
	{
		onPinUnlock(pin);
	}

	#endregion
}
