using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	public class RendererReference : MonoBehaviour
	{
		public MeshRenderer MeshRenderer
		{
			get
			{
				if(meshRenderer == null)
					meshRenderer = GetComponent<MeshRenderer>();
				return meshRenderer;
			}
		}
		public MeshFilter MeshFilter
		{
			get
			{
				if(meshFilter == null)
					meshFilter = GetComponent<MeshFilter>();
				return meshFilter;
			}
		}

		[SerializeField] MeshRenderer meshRenderer;
		[SerializeField] MeshFilter meshFilter;
	}
}
