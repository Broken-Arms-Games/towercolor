using Bag.Scripts.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopIn : MonoBehaviour
{
	public float time = 0.8f;
	public AnimationCurve anim_curve;

	public InstantiatedCoroutine c;
	// Start is called before the first frame update

	public void Open()
	{
		this.transform.localScale = Vector3.zero;
		AnimationPlay(Vector3.zero, Vector3.one);

	}

	public void Close()
	{
		this.transform.localScale = Vector3.one;
		AnimationPlay(Vector3.one, Vector3.zero);

	}

	void AnimationPlay(Vector3 from, Vector3 to)
	{
		if(c == null)
			c = new InstantiatedCoroutine(this);

		c.StartRealtime(time, (t) =>
		{
			this.transform.localScale = Vector3.Slerp(from, to, anim_curve.Evaluate(t));
		}, null);
	}
}
