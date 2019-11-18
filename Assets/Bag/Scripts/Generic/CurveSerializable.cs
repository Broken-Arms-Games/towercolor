using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	[System.Serializable]
	public class CurveId
	{
		[System.Serializable]
		public class KeyFrameSerializable
		{
			public float time;
			public float value;
			public float inTangent;
			public float outTangent;
			public float inWeight;
			public float outWeight;

			public KeyFrameSerializable(Keyframe keyframe)
			{
				time = keyframe.time;
				value = keyframe.value;
				inTangent = keyframe.inTangent;
				outTangent = keyframe.outTangent;
				inWeight = keyframe.inWeight;
				outWeight = keyframe.outWeight;
			}

			public Keyframe ToKeyFrame()
			{
				return new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
			}
		}

		public string id;
		public AnimationCurve curve;
	}
}
