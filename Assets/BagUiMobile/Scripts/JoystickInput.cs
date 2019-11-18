using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Bag.Scripts.Generic;
using Bag.Scripts.Extensions;
using Bag.Mobile.UiLite;

public class JoystickInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	public Vector2 Input
	{
		get
		{
			if(rectJoy.rect.size.x * CanvasCoreManager.Singleton.transform.localScale.x != 0)
				return (rect.position - posCenter) / (rectJoy.rect.size.x * CanvasCoreManager.Singleton.transform.localScale.x);
			else
				return Vector2.zero;
		}
	}

	[SerializeField] RectTransform rectJoy;
	[SerializeField] RectTransform rectBkg;
	[SerializeField] float moveLerpTime = .05f;
	[SerializeField] float scaleLerpTime = .1f;

	RectTransform rect;
	InstantiatedCoroutine move;
	InstantiatedCoroutine scale;
	Vector2 joyScale;
	Vector2 bkgScale;
	Vector3 posOld;
	Vector3 posCenter;
	bool dragging;

	void Awake()
	{
		rect = GetComponent<RectTransform>();
		move = new InstantiatedCoroutine(this);
		scale = new InstantiatedCoroutine(this);

		posCenter = rect.position;
		joyScale = rectJoy.sizeDelta;
		bkgScale = rectBkg.sizeDelta;
		rectJoy.sizeDelta = Vector2.zero;
		rectBkg.sizeDelta = Vector2.zero;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(Game.StateCurrent != Game.State.Play)
			return;
		//StartDrag(eventData.position);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if(Game.StateCurrent != Game.State.Play)
			return;

		if(!dragging)
			StartDrag(eventData.position);
		else
		{
			dragging = true;
			DoDrag(eventData.position.ToVector3() - posCenter);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if(Game.StateCurrent != Game.State.Play)
			return;
		if(!dragging)
			return;
		dragging = false;
		DoDrag(Vector3.zero);
	}

	void StartDrag(Vector2 eventPos)
	{
		dragging = true;
		Vector2 screenSize = new Vector2(Screen.width, Screen.height);
		rectBkg.anchorMin = rectBkg.anchorMax = eventPos / screenSize;
		posCenter = eventPos;
		DoDrag(eventPos.ToVector3() - posCenter, true);
	}

	void DoDrag(Vector3 relativeEventPos, bool instant = false)
	{
		posOld = rect.position - posCenter;
		SetAnchoredPos(posOld, Vector2.ClampMagnitude(relativeEventPos, rectJoy.sizeDelta.x * CanvasCoreManager.Singleton.transform.localScale.x), instant);
	}

	void SetAnchoredPos(Vector3 posOld, Vector3 targetPos, bool instant)
	{
		if(rectJoy.sizeDelta == Vector2.zero)
		{
			scale.Start(instant ? 0 : scaleLerpTime, t =>
			{
				rectJoy.sizeDelta = Vector2.Lerp(Vector2.zero, joyScale, t);
				rectBkg.sizeDelta = Vector2.Lerp(Vector2.zero, bkgScale, t);
			}, null, false);
		}
		move.Start(instant ? 0 : moveLerpTime, t =>
		{
			rect.position = Vector3.Lerp(posOld, targetPos, t) + posCenter;
		}, delegate
		{
			if(!dragging)
				scale.Start(instant ? 0 : scaleLerpTime, t =>
				{
					rectJoy.sizeDelta = Vector2.Lerp(joyScale, Vector2.zero, t);
					rectBkg.sizeDelta = Vector2.Lerp(bkgScale, Vector2.zero, t);
				}, null, false);
		}, true);
	}
}
