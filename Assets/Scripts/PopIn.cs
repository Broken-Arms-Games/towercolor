using Bag.Scripts.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopIn : MonoBehaviour
{
	public float startAfter = 0f;
	public float time = 0.8f;
	public AnimationCurve anim_curve;

	InstantiatedCoroutine c;
	// Start is called before the first frame update
	void Start()
	{
		c = new InstantiatedCoroutine(this);
		this.transform.localScale = Vector3.zero;
	}
	void OnEnable()
	{
		if(this.transform.localScale == Vector3.one)
			this.transform.localScale = Vector3.zero;
		Invoke("AnimationPlay", startAfter);

	}

	void AnimationPlay()
	{
		c.StartRealtime(time, (t) =>
		{
			this.transform.localScale = Vector3.Slerp(Vector3.zero, Vector3.one, anim_curve.Evaluate(t));
		}, null);
	}
}
