using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	public class GyroPan : MonoBehaviour
	{
		public bool GyroEnabled
		{
			get { return Input.gyro.enabled; }
			set
			{
				Input.gyro.enabled = value;
				if(!value)
					transform.localPosition = panTarget = Vector3.zero;
			}
		}

		[Range(0, 10)] public float maxPan = 0.5f;
		[Range(0, 1)] public float gyroSensitivity = 0.08f;

		Vector3 panTarget;
		Vector3 panVel;


		void Start()
		{
			GyroEnabled = false;
			GyroEnabled = true;
		}

		void Update()
		{
			if(GyroEnabled)
			{
				panTarget = Vector3.ClampMagnitude(panTarget + new Vector3(Input.gyro.rotationRate.y, -Input.gyro.rotationRate.x) * gyroSensitivity, maxPan);
				transform.localPosition = Vector3.SmoothDamp(transform.localPosition, panTarget, ref panVel, 0.05f);
			}
		}
	}
}
