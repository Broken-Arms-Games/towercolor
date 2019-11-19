using UnityEngine;

/// <summary>
/// Has useful physic functions.
/// </summary>
public static class Physicf
{
	/// <summary>
	/// Calculates the initial velocity needed for a launch that starts at 'start' positions and goes through 'target'.
	/// </summary>
	/// <returns>The initial launch velocity.</returns>
	/// <param name="start">Start position.</param>
	/// <param name="target">Target position.</param>
	/// <param name="launchHeight">Launch height.</param>
	public static Vector3 BallisticLaunch(Vector3 start, Vector3 target, float launchHeight, bool preventNan = true)
	{
		// get gravity
		float gravity = Physics.gravity.y;
		// positions differences
		float dispY = target.y - start.y;

		if(preventNan)
			launchHeight = Mathf.Max(launchHeight, -(target.y - start.y));

		launchHeight += dispY;
		Vector3 dispXZ = new Vector3(target.x - start.x, 0, target.z - start.z);
		// vertical & horizontal velocity calculation
		Vector3 velY = Vector3.up * Mathf.Sqrt(-2f * gravity * launchHeight);
		Vector3 velXZ = dispXZ / (Mathf.Sqrt(-2f * launchHeight / gravity) + Mathf.Sqrt(2f * (dispY - launchHeight) / gravity));
		// multiplying by the sign of gravity prevents problems with positive gravity scenarios
		return velXZ + velY * -Mathf.Sign(gravity);
	}
}
