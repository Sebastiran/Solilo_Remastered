using MxUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CourageBarBehaviour : MonoBehaviour
{
	public GameObject filledSegmentObject;
	public GameObject emptySegmentObject;
	//[Range(0, Courage.MaxValue)]
	//public int initialValue = Courage.MaxValue;

	int initialValue;
	int valueAtLastUpdate = 0;
	GameObject[] segmentInstances = new GameObject[Courage.MaxValue];

	static int NumberOfSegments
	{
		get { return Courage.MaxValue; }
	}

	void Awake()
	{
		this.BreakIfNull(filledSegmentObject);
		this.BreakIfNull(emptySegmentObject);
	}

	void Start()
	{
		//valueAtLastUpdate = Courage.MaxValue;
		initialValue = Courage.CurrentValue;
	}

	void FixedUpdate()
	{
		if (valueAtLastUpdate != Courage.CurrentValue)
			RebuildBar();

		valueAtLastUpdate = Courage.CurrentValue;
	}

	void OnDrawGizmosSelected()
	{
		Vector3 offset = transform.position;
		Vector3 originalScale = transform.localScale;
		Vector3 segmentScale = VectorOps.Multiply(originalScale, new Vector3(1f / NumberOfSegments, 1f, 1f));
		Gizmos.color = Color.yellow;

		for (int i = 0; i < NumberOfSegments; i++)
		{
			Vector3 position;

			position = originalScale / NumberOfSegments;
			position *= (.5f + i);
			position += -(.5f * originalScale);
			position = VectorOps.Multiply(position, new Vector3(1f, 0f, 0f));
			position += offset;

			Gizmos.DrawWireCube(position, segmentScale);
		}
	}

	void RebuildBar()
	{
		/** Scenario: New value is lower than previous value. */
		for (int i = valueAtLastUpdate; i > Courage.CurrentValue; --i)
		{
			i--;

			if (segmentInstances[i] != null)
				Destroy(segmentInstances[i]);

			segmentInstances[i] = (GameObject)Instantiate(emptySegmentObject, ComputePosition(i), Quaternion.identity);
			segmentInstances[i].transform.localScale = ComputeSegmentScale();
			segmentInstances[i].transform.SetParent(this.transform);

			i++;
		}

		/** Scenario: New value is higher than previous value. */
		for (int i = valueAtLastUpdate; i < Courage.CurrentValue; ++i)
		{
			if (segmentInstances[i] != null)
				Destroy(segmentInstances[i]);

			segmentInstances[i] = (GameObject)Instantiate(filledSegmentObject, ComputePosition(i), Quaternion.identity);
			segmentInstances[i].transform.localScale = ComputeSegmentScale();
			segmentInstances[i].transform.SetParent(this.transform);
		}
	}

	Vector3 ComputePosition(int segmentIndex)
	{
		Vector3 position;
		Vector3 offset = transform.position;
		Vector3 originalScale = transform.localScale;

		position = originalScale / NumberOfSegments;
		position *= (-.5f + segmentIndex);
		position += (-.5f * originalScale);
		position = VectorOps.Multiply(position, new Vector3(1f, 0f, 0f));
		position += offset;

		return position;
	}

	Vector3 ComputeSegmentScale()
	{
		return VectorOps.Multiply(transform.localScale, new Vector3(1f / NumberOfSegments, 1f, 1f));
	}
}
