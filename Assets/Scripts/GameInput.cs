using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Scripts.Extensions;
using Bag.Mobile.UiLite;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
	[SerializeField] CameraDrag cameraDrag;
	[SerializeField] Transform cameraHolder;
	[SerializeField] float cameraRotSpeed = 10;
	[SerializeField] Shot shot;
	[SerializeField] Transform shotHolder;
	[SerializeField] Transform shootingHolder;

	Vector3 shotPos { get { return shot.transform.position; } }
	Shot shotReady;
	List<Shot> shotsPool;

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
			cameraHolder.localRotation = Quaternion.Euler(cameraHolder.localRotation.eulerAngles - Vector3.down * cameraRotSpeed * Time.deltaTime * camInput.x);
		// input shooting
		else if(shootInput != -Vector2.one)
			Shoot(Input.mousePosition);
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
		}
	}

	public void ShotEnd()
	{
		if(shotReady == null)
			ShotSpawn();
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

	#region CAMERA_MOVE



	#endregion
}
