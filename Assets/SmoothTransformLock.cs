using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.MxUnity;

public class SmoothTransformLock : MonoBehaviour
{
	const int MaxSamples = 100;

	public Transform target;
	public Transform[] optionalOthers = new Transform[0];
	public BoolVector3 lockedAxes = new BoolVector3(true, true, true);
	public Vector3 offset;
	[SerializeField]
	[Range(1, MaxSamples)]
	int sampleCount = 25;

	Queue<Vector3> positionSamples = new Queue<Vector3>();

	public int SampleCount
	{
		get { return sampleCount; }
		set { sampleCount = Mathf.Clamp(value, 1, MaxSamples); }
	}

	public void InvokeOnValidate()
	{
		OnValidate();
	}

	void OnValidate()
	{
		if (enabled && target != null)
			transform.position = target.position + offset;
	}

	void FixedUpdate()
	{
		if (target == null)
			return;

		TakeSample();
		ApplyPositionChange();
	}

	void Update()
	{
		if (target == null)
			return;

		// Note: Additional position update is done here on purpose (enhances smoothness).
		ApplyPositionChange();
	}

	void OnDrawGizmosSelected()
	{
		if (target == null)
			return;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, .25f);
		Gizmos.DrawLine(transform.position, target.position);
		Gizmos.DrawWireSphere(target.position, .125f);
	}

	void TakeSample()
	{
		positionSamples.Enqueue(target.position);

		foreach (Transform e in optionalOthers)
			positionSamples.Enqueue(e.position);

		int currentCap = sampleCount + sampleCount * optionalOthers.Length;

		while (positionSamples.Count > currentCap)
			positionSamples.Dequeue();
	}

	Vector3 CalculateSampleAverage()
	{
		Vector3 sum = Vector3.zero;

		foreach (Vector3 sample in positionSamples)
			sum += sample;

		return sum / positionSamples.Count;
	}

	void ApplyPositionChange()
	{
		transform.position = lockedAxes.MaskedCopy(CalculateSampleAverage() + offset, transform.position);
	}
}
