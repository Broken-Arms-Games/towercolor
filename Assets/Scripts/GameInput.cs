using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Mobile.UiLite;

public class GameInput : MonoBehaviour
{
	[SerializeField] Transform cameraHolder;
	[SerializeField] float cameraRotSpeed = 50;

	/// <summary>
	/// Joystick 2-axis input.
	/// </summary>
	Vector2 joyInput;
	/// <summary>
	/// Pixel coord position of shoot input.
	/// </summary>
	Vector2 shootInput;

	void Update()
	{
		if(Game.StateCurrent != Game.State.Play)
			return;

		joyInput = CanvasCoreManager.Joystick.Input;
		shootInput = ShootInput();

		// input camera rotation
		if(joyInput != Vector2.zero)
			cameraHolder.localRotation = Quaternion.Euler(cameraHolder.localRotation.eulerAngles + Vector3.down * cameraRotSpeed * Time.deltaTime * joyInput.x);
		// input shooting
		else if(shootInput != -Vector2.one)
			Shoot(Input.mousePosition);
	}

	void Shoot(Vector2 screenPos)
	{
		RaycastHit hit;
		if(Physics.Raycast(Game.Cam.ScreenPointToRay(screenPos), out hit, 1000f, LayerMask.GetMask("Pins")))
			hit.collider.GetComponent<PinCollider>().Pin.Shooted();
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
}
