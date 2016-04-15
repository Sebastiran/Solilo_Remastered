using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PositionSampler))]
public class PositionHistoryPlotter : MonoBehaviour
{
	public Color lineColor = Color.blue;
	PositionSampler sampler;

	void OnDrawGizmosSelected()
	{
		if (sampler == null)
			sampler = GetComponent<PositionSampler>();

		Vector3[] samples = sampler.CopySamplesToArray();
		Gizmos.color = lineColor;

		for (int i = 1; i < samples.Length; i++)
			Gizmos.DrawLine(samples[i - 1], samples[i]);
	}
}
