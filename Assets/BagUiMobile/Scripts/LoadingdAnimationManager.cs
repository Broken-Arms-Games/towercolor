using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingdAnimationManager : MonoBehaviour
{
	public static LoadingdAnimationManager Singleton;
	[SerializeField] RectTransform c_Sx;
	[SerializeField] RectTransform c_Dx;
	[SerializeField] Image i;
	[SerializeField] RectTransform baloon;
	[SerializeField] AnimationCurve anim_curve;
	[SerializeField] Color a;
	[SerializeField] Color b;
	InstantiatedCoroutine c;



	void Awake()
	{
		if(Singleton == null)
		{
			Singleton = this;
			DontDestroyOnLoad(this);
			Init();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Init()
	{
		c = new InstantiatedCoroutine(this);
	}



	public static void StartLoading(Action action)
	{
		if(Singleton)
			Singleton._StartLoading(action);
	}

	void _StartLoading(Action action)
	{
		c.Start(2f, (t) =>
		{
			c_Sx.anchoredPosition = Vector2.Lerp(c_Sx.anchoredPosition, new Vector2(c_Sx.sizeDelta.x, 0), anim_curve.Evaluate(t));
			c_Dx.anchoredPosition = Vector2.Lerp(c_Dx.anchoredPosition, new Vector2(-c_Dx.sizeDelta.x, 0), anim_curve.Evaluate(t));
			baloon.localScale = Vector3.Lerp(baloon.localScale, new Vector3(1, 1, 0), anim_curve.Evaluate(t));
			i.color = Color.Lerp(i.color, a, anim_curve.Evaluate(t));
		},
		delegate
		{
			action();
			//Invoke("CloseLoading", 1f);
		});
	}

	public static void CloseLoading()
	{
		if(Singleton)
			Singleton._CloseLoading();
	}

	void _CloseLoading()
	{
		c.Start(1.5f, (t) =>
		{
			baloon.localScale = Vector3.Lerp(baloon.localScale, new Vector3(0, 0, 0), anim_curve.Evaluate(t));
			c_Sx.anchoredPosition = Vector2.Lerp(c_Sx.anchoredPosition, new Vector2(0, 0), anim_curve.Evaluate(t));
			c_Dx.anchoredPosition = Vector2.Lerp(c_Dx.anchoredPosition, new Vector2(0, 0), anim_curve.Evaluate(t));
			i.color = Color.Lerp(i.color, b, anim_curve.Evaluate(t));
		}, null);
	}
}
