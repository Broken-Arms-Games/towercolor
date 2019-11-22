using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour
{
	public Vector2 CamInput { get { return input; } }
	public bool Dragging { get { return dragging; } }
	public bool InputNotZero { get { return input != Vector2.zero; } }

	bool dragging;
	bool dragStart;
	Vector2 dragPos;
	Vector2 input;
	Vector2 inputTarget;
	Vector2 inputVel;

	void Update()
	{
		if(Game.StateCurrent < Game.State.Play)
			return;

#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
			StartDrag(Input.mousePosition);
		else if(Input.GetMouseButton(0))
			DoDrag(Input.mousePosition);
		else if(inputTarget.magnitude > 0 || dragging)
			EndDrag();
#else
		if(Input.touchCount == 1)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
				StartDrag(Input.GetTouch(0).position);
			else if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
				DoDrag(Input.GetTouch(0).position);
			else if(dragging)
				EndDrag();
		}
		else if(dragging)
			EndDrag();
#endif

		input = Vector2.SmoothDamp(input, inputTarget, ref inputVel, 0.2f);
	}


	void StartDrag(Vector2 pointerPos)
	{
		inputTarget = Vector2.zero;
		dragPos = pointerPos;
	}

	void DoDrag(Vector2 pointerPos)
	{
		inputTarget = pointerPos - dragPos;
		dragPos = pointerPos;
		if(inputTarget.magnitude > 10f)
			dragging = true;
	}

	void EndDrag()
	{
		StartCoroutine(EndDragCo());
		inputTarget = Vector2.zero;
		dragPos = Vector2.zero;
	}

	IEnumerator EndDragCo()
	{
		yield return null;
		dragging = false;
	}
}
