using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomMountainGenerator : MonoBehaviour
{
	public GameObject mountainPrefab;
	public float numberOfMountains = 10;
	public AnimationCurve distanceHeightGraph;

	[SerializeField]
	[HideInInspector]
	List<GameObject> spawnedMountains = new List<GameObject>();

	Vector3 RandomSpawnBoxPosition
	{
		get
		{
			Vector3 output;

			float x = Random.Range(-.5f, .5f);
			float y = Random.Range(-.5f, .5f);
			float z = Random.Range(0f, 1f);

			output = new Vector3(x, y, z);
			output.Scale(transform.localScale);
			output += transform.position;

			return output;
		}
	}

	Vector3 EvaluateScale(GameObject mountain)
	{
		return distanceHeightGraph.Evaluate(mountain.transform.localPosition.z / transform.localScale.z) * Vector3.one;
	}

	AnimationCurve DefaultGraph
	{
		get
		{
			AnimationCurve output;
			
			output = AnimationCurve.EaseInOut(0f, 2.5f, 1f, 10f);
			//output.preWrapMode = WrapMode.

			return output;
		}
	}

	public void Regenerate()
	{
		ClearExistingMountains();

		for (int i = 0; i < numberOfMountains; i++)
		{
			GameObject mountain;

			mountain = (GameObject)Instantiate(mountainPrefab);
			mountain.transform.position = RandomSpawnBoxPosition;
			mountain.transform.localScale = EvaluateScale(mountain);
			mountain.transform.parent = transform;

			spawnedMountains.Add(mountain);
		}
	}

	public void ClearExistingMountains()
	{
		foreach (GameObject e in spawnedMountains)
			try
			{
				DestroyImmediate(e);
			}
			catch (System.NullReferenceException) { }

		spawnedMountains.Clear();
	}

	public void ResetGraph()
	{
		distanceHeightGraph = DefaultGraph;
	}

	void OnValidate()
	{
		Vector3 localScale;
		
		localScale = transform.localScale;
		localScale.y = 1f;

		transform.localScale = localScale;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(new Vector3(0f, 0f, .5f), Vector3.one);
	}
}
