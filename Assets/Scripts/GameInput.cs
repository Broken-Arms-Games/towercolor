using System.Collections;
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
	public bool InitLevelCameraEnded { get { return initLevelCameraCo == null; } }

	Vector3 CamHolderTarget { get { return Vector3.up * camHeight; } }

	[SerializeField] CameraDrag cameraDrag;
	[SerializeField] Transform cameraHolder;
	[SerializeField] float cameraRotSpeed = 10;
	[SerializeField] float cameraHeightOffset = -5;
	[SerializeField] Shot shot;
	[SerializeField] Transform shotHolder;
	[SerializeField] Transform shootingHolder;
	[SerializeField] float shotSpeed = 200;
	[SerializeField, Range(0, 1)] float shotArch = .02f;

	Transform camTransf;
	Vector3 shotPos { get { return shot.transform.position; } }
	Shot shotReady;
	List<Shot> shotsPool;
	float camHeight;
	Vector3 camHeightVel;
	float shakeForce = -1;
	float shakeTime = -1;
	InstantiatedCoroutine shakeCo;
	Vector3 camShakeOffset;
	Vector3 camShakeOffsetVel;
	Vector3 camOffset;

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
		camTransf = Game.Cam.transform;
		camOffset = camTransf.localPosition;
		Game.Hub.tower.onLayerUnlock += l => { camHeight = l.posHeight + cameraHeightOffset; };

		ShotSpawn();
	}

	void Update()
	{
		if(Game.StateCurrent != Game.State.Play)
			return;

		camInput = cameraDrag.CamInput;
		shootInput = ShootInput();

		// input camera rotation
		if(cameraDrag.Dragging)
			CameraRotate(camInput.x);
		// input shooting
		else if(shootInput != -Vector2.one)
			Shoot(Input.mousePosition);

		CameraUpdatePos();
	}

	#region SHOOT

	void Shoot(Vector2 screenPos)
	{
		RaycastHit hit;
		if(Physics.Raycast(Game.Cam.ScreenPointToRay(screenPos), out hit, 1000f, LayerMask.GetMask("Pins")))
			ShotStart(hit.point);
	}

	void ShotSpawn()
	{
		shot.transform.parent.AddPoolList(shot, 1, ref shotsPool, (p, i) =>
		{
			//Debug.LogError("X");
			p.transform.SetParent(shotHolder);
			p.transform.position = shotPos;
			p.Init(delegate { shotReady = p; });
		});
	}

	void ShotStart(Vector3 target)
	{
		if(shotReady != null)
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
