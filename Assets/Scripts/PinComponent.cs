using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinComponent : MonoBehaviour
{
	public Pin Pin { get { return pin; } }

	Pin pin;

	public virtual void Init(Pin pin)
	{
		this.pin = pin;
	}
}
