using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
	void Update()
	{
		if(Input.GetMouseButtonUp(0))
		{
			Shoot(Input.mousePosition);
		}
	}

	void Shoot(Vector2 screenPos)
	{
		RaycastHit hit;
		if(Physics.Raycast(Game.Cam.ScreenPointToRay(screenPos), out hit, 1000f, LayerMask.GetMask("Pins")))
			hit.collider.GetComponent<PinCollider>().Pin.Shooted();
	}
}
