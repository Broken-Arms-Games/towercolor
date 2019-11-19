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
	/// <param name="preventNan">If true prevents NaN error when the target is lower than the start.</param>
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

	/// <summary>
	/// Calculates the initial velocity needed for a launch that starts at 'start' positions and goes through 'target', can set a starting speed and a lerp between the min and max arch to get to the target.
	/// </summary>
	/// <returns>The initial launch velocity, maximum force is returned if the target cannot be reached with this amout of initial speed.</returns>
	/// <param name="start">Start position.</param>
	/// <param name="target">Target position.</param>
	/// <param name="speed">Initial speed of the launch.</param>
	/// <param name="archLerp">Lerp from 0 to 1 to get the desired arch for the launch.</param>
	public static Vector3 BallisticLaunch(Vector3 start, Vector3 target, float speed, float archLerp)
	{
		// calculation parameters
		Vector3 toTarget = target - start;
		float gSquared = Physics.gravity.sqrMagnitude;
		float b = speed * speed + Vector3.Dot(toTarget, Physics.gravity);
		float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

		// target too far to hit at this speed
		if(discriminant < 0)
			return toTarget.normalized * speed;

		float discRoot = Mathf.Sqrt(discriminant);
		// highest shot with the given max speed:
		float tMax = Mathf.Sqrt((b + discRoot) * 2f / gSquared);
		// most direct shot with the given max speed:
		float tMin = Mathf.Sqrt((b - discRoot) * 2f / gSquared);
		// lowest-speed arc available:
		float tLowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));
		float t = Mathf.Lerp(tMin, tMax, Mathf.Clamp01(archLerp));

		// convert from time-to-hit to a launch velocity:
		return toTarget / t - Physics.gravity * t / 2f;
	}
}
