using UnityEngine;

namespace Bag.Scripts.Generic
{
	/// <summary>
	/// Allow to display an attribute in inspector without allow editing
	/// </summary>
	public class DisplayWithoutEdit : PropertyAttribute
	{
		public DisplayWithoutEdit() { }
	}
}
