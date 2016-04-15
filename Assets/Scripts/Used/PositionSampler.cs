using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// <para>Used to keep track of position values and allow a velocity to be estimated from these.</para>
/// <para>Useful for when displacement is not done via rigidbody's velocity, but by changing the transform's position value.</para>
/// <para>A position-sample is taken every FixedUpdate().</para>
/// </summary>
public class PositionSampler : MonoBehaviour
{
	[Range(1, 1000)]
	public int maxSamples = 10;
	Queue<Vector3> samples = new Queue<Vector3>();

	public Vector3[] CopySamplesToArray()
	{
		return samples.ToArray();
	}

	/// <summary>
	/// NOTE: Velocity will appear to be zero, if the sampler has less than 2 stored position-samples.
	/// </summary>
	public Vector3 EstimateVelocity()
	{
		if (samples.Count < 2)
			return Vector3.zero;

		Vector3[] array = CopySamplesToArray();
		Vector3 p1 = array[array.Length - 2];
		Vector3 p2 = array[array.Length - 1];
		Vector3 displacement = p2 - p1;

		return displacement / Time.deltaTime;
	}

	void FixedUpdate()
	{
		// Take sample.
		samples.Enqueue(transform.position);

		// Keep number of samples limited to specified maximum.
		while (samples.Count > maxSamples)
			samples.Dequeue();
	}
}
