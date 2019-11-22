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

		public int Actives
		{
			get
			{
				int actives = 0;
				if(active)
				{
					for(int i = 0; i < pins.Count; i++)
						if(pins[i].gameObject.activeSelf && (pins[i].transform.localPosition.y + 1) > posHeight)
							++actives;
					if(actives == 0)
						active = false;
				}
				return actives;
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
	public LayerData LayerLowestUnlocked { get { return layerList[Mathf.Max(0, layerTop - layersUnlocked)]; } }
	public int ScoreMax { get { return scoreMax - layerAmount * 1; } }
	public bool LevelStarted { get; private set; }

	[SerializeField] Transform spawnBase;
	[SerializeField] Pin pinPrefab;
	[SerializeField] ParticleSystem pinParticle;

	public Material pinMaterialLock;
	public Material[] pinMaterials;
	public Material[] particleMaterials;
	public int layersUnlocked = 8;
	public GameObject winParticle;

	[HideInInspector] public WaitForSeconds wait = new WaitForSeconds(.08f);
	WaitForSeconds waitParticle = new WaitForSeconds(2);

	List<Pin> pinPool;
	List<ParticleSystem> pinParticlePool;
	LayerData[] layerList;
	Queue<LayerData> unlockQueue;
	IEnumerator unlockQueueCo;
	int layers;
	int layerAmount = 15;
	float layerRadius = 3.6f;
	System.Random rand;
	int layerTop = -1;
	int score;
	int scoreMax;

	public event Action<LayerData> onLayerUnlock = l => { Debug.Log("[TOWER] Layer unlock at index " + l.index + "."); };
	public event Action<Pin> onPinUnlock = p => { };
	public event Action<Pin> onPinShoot = p => { };


	public void Init()
	{
		unlockQueue = new Queue<LayerData>();

		onPinUnlock += p => { PinParticleEnable(p, particleMaterials[4]); };
		onPinShoot += p => { PinParticleEnable(p, particleMaterials[p.num]); };
	}

	public void LevelSpawn(int layers = 10)
	{
		this.layers = layers;
		for(int i = 0; i < spawnBase.childCount; i++)
			spawnBase.GetChild(i).gameObject.SetActive(false);

		score = 0;
		scoreMax = layerAmount * layers;
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
	}

	public void LevelStart()
	{
		LevelStarted = false;
		StartCoroutine(LevelStartCo());
	}

	IEnumerator LevelStartCo()
	{
		WaitForSeconds wait = new WaitForSeconds(0.5f / 10f);
		for(int i = 0; i < layerList.Length; i++)
		{
			for(int j = 0; j < layerList[i].pins.Count; j++)
			{
				layerList[i].pins[j].SetLockedState(layerList[i].pins[j].Locked);
			}
			if(i >= (layerList.Length - 10 - layersUnlocked))
				yield return wait;
		}
		LevelStarted = true;
	}

	public int GetRandomNum()
	{
		if(rand == null)
			rand = new System.Random(layerAmount + UnityEngine.Random.Range(0, int.MaxValue - layers));
		return rand.Next(pinMaterials.Length);
	}

	void Update()
	{
		if(Game.StateCurrent != Game.State.Play)
			return;

		int score = 0;
		int actives = 0;
		for(int i = layerList.Length - 1; i >= 0; i--)
		{
			actives = layerList[i].Actives;
			if(i <= layerTop && actives <= 0)
			{
				layerTop = i - 1;
				LayerUnlock();
			}
			score += layerAmount - actives;
		}
		Game.Player.Score = score;

		if(unlockQueue.Count > 0 && unlockQueueCo == null)
		{
			unlockQueueCo = UnlockQueue();
			StartCoroutine(unlockQueueCo);
		}
	}

	void LayerUnlock()
	{
		for(int i = layerTop; i >= Mathf.Max(0, layerTop + 1 - layersUnlocked); i--)
		{
			if(layerList[i].locked && !unlockQueue.Contains(layerList[i]))
			{
				unlockQueue.Enqueue(layerList[i]);
			}
		}
	}

	public LayerData LayerGet(int index)
	{
		return layerList[index];
	}

	IEnumerator UnlockQueue()
	{
		LayerData layer = null;
		while(unlockQueue.Count > 0)
		{
			yield return wait;
			layer = unlockQueue.Dequeue();
			layer.Unlock();
			onLayerUnlock(layer);
		}
		unlockQueueCo = null;
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

	#region EFFECTS

	void PinParticleEnable(Pin pin, Material mat)
	{
		pin.transform.parent.AddPoolList(pinParticle, 1, ref pinParticlePool, (p, i) =>
		{
			p.GetComponent<ParticleSystemRenderer>().material = mat;
			p.transform.position = pin.transform.position;
			p.gameObject.SetActive(true);
			StartCoroutine(PinParticleDisable(p));
		});
	}

	IEnumerator PinParticleDisable(ParticleSystem particle)
	{
		yield return waitParticle;
		particle.gameObject.SetActive(false);
	}

	#endregion
}
