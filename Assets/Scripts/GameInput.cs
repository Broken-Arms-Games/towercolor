﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Extensions;
using Bag.Scripts.Generic;
using Bag.Mobile.UiLite;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
	public float ShotSpeed { get { return shotSpeed; } }
	public float ShotArch { get { return shotArch; } }
	public bool TapToStart { get { return tapToStart; } }
	public bool InitLevelCameraEnded { get { return initLevelCameraCo == null; } }

	Vector3 CamHolderTarget { get { return Vector3.up * camHeight; } }

	[SerializeField] CameraDrag cameraDrag;
	[SerializeField] Transform cameraHolder;
	[SerializeField] float cameraRotSpeed = 10;
	[SerializeField] float cameraHeightOffset = -5;
	[SerializeField] Shot shot;
	[SerializeField] Shot[] shotSpecials;
	[SerializeField] Transform shotHolder;
	[SerializeField] Transform shootingHolder;
	[SerializeField] float shotSpeed = 200;
	[SerializeField, Range(0, 1)] float shotArch = .02f;

	Transform camTransf;
	Vector3 shotPos { get { return shot.transform.position; } }
	Shot shotReady;
	Vector3 shotReadyRotateAxis;
	List<Shot> shotsPool;
	List<Shot>[] shotSpecialsPool;
	float camHeight;
	Vector3 camHeightVel;
	float shakeForce = -1;
	float shakeTime = -1;
	InstantiatedCoroutine shakeCo;
	Vector3 camShakeOffset;
	Vector3 camShakeOffsetVel;
	Vector3 camOffset;
	bool tapToStart;

	/// <summary>
	/// Joystick 2-axis input.
	/// </summary>
	Vector2 camInput;
	/// <summary>
	/// Pixel coord position of shoot input.
	/// </summary>
	Vector2 shootInput;


	public void Init()
	{
		shot.gameObject.SetActive(false);
		for(int i = 0; i < shotSpecials.Length; i++)
			shotSpecials[i].gameObject.SetActive(false);
		shotSpecialsPool = new List<Shot>[shotSpecials.Length];
		camTransf = Game.Cam.transform;
		camOffset = camTransf.localPosition;
		camHeight = 0;
		Game.Hub.tower.onLayerUnlock += l =>
		{
			if(Game.StateCurrent == Game.State.Play)
				camHeight = l.posHeight + cameraHeightOffset;
		};
	}

	void Update()
	{
		if(Game.StateCurrent == Game.State.LevelInit)
		{
			if(!tapToStart)
				CameraRotate(2);
#if UNITY_EDITOR
			if(Input.GetMouseButtonDown(0))
			{
				tapToStart = true;
				return;
			}
#else
			if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				tapToStart = true;
				return;
			}
#endif
		}

		camInput = cameraDrag.CamInput;
		shootInput = ShootInput();

		// input camera rotation
		if(cameraDrag.Dragging || cameraDrag.InputNotZero)
			CameraRotate(camInput.x);

		if(Game.StateCurrent != Game.State.Play)
			return;
		// input shooting
		if(!cameraDrag.Dragging && shootInput != -Vector2.one)
			Shoot(Input.mousePosition);
		else if(shootInput != -Vector2.one)
		{
			Debug.LogError("DRAGGING " + cameraDrag.Dragging);
		}

		CameraUpdatePos();

		if(shotReady != null)
			shotReady.transform.Rotate(shotReadyRotateAxis, 2f * Time.deltaTime);
	}

	#region SHOOT

	void Shoot(Vector2 screenPos)
	{
		RaycastHit hit;
		if(Physics.Raycast(Game.Cam.ScreenPointToRay(screenPos), out hit, 1000f, LayerMask.GetMask("Pins")))
			ShotStart(hit.point);
	}

	public void ShotSpawn()
	{
		int ss = -1;
		for(int i = 0; i < shotSpecials.Length; i++)
		{
			if(Game.Player.SpecialUse(i))
			{
				ss = i;
				break;
			}
		}

		if(ss < 0)
			shot.transform.parent.AddPoolList(shot, 1, ref shotsPool, InitPooledShot);
		//shotSpecials[3].transform.parent.AddPoolList(shotSpecials[3], 1, ref shotSpecialsPool[3], InitPooledShot);
		else
			shotSpecials[ss].transform.parent.AddPoolList(shotSpecials[ss], 1, ref shotSpecialsPool[ss], InitPooledShot);
	}

	void InitPooledShot(Shot shot, int index)
	{
		shot.transform.SetParent(shotHolder);
		shot.transform.position = shotPos;
		shot.Init(delegate
		{
			shotReadyRotateAxis = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
			shotReady = shot;
		});
	}

	void ShotStart(Vector3 target)
	{
		if(shotReady != null && Game.Player.Shoot())
		{
			shotReady.transform.SetParent(shootingHolder);
			shotReady.Shoot(target);
			shotReady = null;
			ShotSpawn();
		}
	}

	Vector2 ShootInput()
	{
#if UNITY_EDITOR
		if(Input.GetMouseButtonUp(0))
			return Input.mousePosition;
#else
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
			return Input.GetTouch(0).position;
#endif
		else
			return -Vector2.one;
	}

	#endregion

	#region CAMERA

	IEnumerator initLevelCameraCo;

	public void InitLevelCamera()
	{
		cameraHolder.transform.localPosition = Vector3.zero;
		camHeight = Game.Hub.tower.LayerLowestUnlocked.posHeight + cameraHeightOffset;
		initLevelCameraCo = InitLevelCameraCo();
		StartCoroutine(initLevelCameraCo);
	}

	IEnumerator InitLevelCameraCo()
	{
		while((cameraHolder.transform.localPosition - CamHolderTarget).magnitude >= 0.5f)
		{
			CameraRotate(-Mathf.Min(5, (cameraHolder.transform.localPosition - CamHolderTarget).magnitude));
			CameraUpdatePos();
			yield return null;
		}
		initLevelCameraCo = null;
	}

	void CameraUpdatePos()
	{
		cameraHolder.localPosition = Vector3.SmoothDamp(cameraHolder.transform.localPosition, CamHolderTarget, ref camHeightVel, 1);
		camTransf.localPosition = camOffset + camTransf.rotation * camShakeOffset;
	}

	void CameraRotate(float deg)
	{
		cameraHolder.localRotation = Quaternion.Euler(cameraHolder.localRotation.eulerAngles - Vector3.down * cameraRotSpeed * Time.deltaTime * deg);
	}

	public void Shake(float duration, float force)
	{
		if(force < shakeForce * (1 - shakeTime))
			return;
		shakeForce = force;
		Vector3 offsetTarget = new Vector3(0, Random.Range(-force, force), Random.Range(-force, force));
		if(shakeCo == null)
			shakeCo = new InstantiatedCoroutine(this);
		shakeCo.Start(duration, t =>
		{
			shakeTime = t;
			camShakeOffset = Vector3.SmoothDamp(camShakeOffset, offsetTarget, ref camShakeOffsetVel, 0.01f);
			if(Vector3.Distance(camShakeOffset, offsetTarget) < 0.05f)
				offsetTarget = new Vector3(Random.Range(-force, force) * (1 - t / 2f), 0, 0);
		}, delegate
		{
			shakeForce = -1;
			shakeCo.Start(.5f, t =>
			{
				camShakeOffset = Vector3.Lerp(camShakeOffset, Vector3.zero, t);
			}, null);
		}, true);
	}

	#endregion
}
