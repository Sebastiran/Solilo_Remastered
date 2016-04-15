using UnityEngine;
using System.Collections;
using Assets.MxUnity;

[RequireComponent(typeof(PositionSampler))]
[RequireComponent(typeof(Courage))]
public class GhostPlatformSpawner : MonoBehaviour
{
	public GameObject platformPrefab;
	public GameObject projectionPrefab;
	public Material projectionValidMaterial;
	public Material projectionInvalidMaterial;
	public KeyCode keyBinding;
	public int courageCost;

	[Header("Anti-Collision Settings")]
	public float minSpawnDistance;
	public int MaxCollisionChecks = 100;
	public float antiCollisionIncrement = .01f;

	[Header("Gizmos")]
	public bool visualizeSpawnSpheres;

	GameObject projectionPrefabClone;

	void Start()
	{
		projectionPrefabClone = (GameObject)Instantiate(projectionPrefab);
	}

	void FixedUpdate()
	{
		HandlePlatformSpawning();
		HandlePlatformProjection();
	}

	//void Update()
	//{
	//	/*
	//	 * Invoke FixedUpdate() again, to stop some visual issues (with the
	//	 * platform projection) from happening.
	//	 */
	//	FixedUpdate();
	//}

	void OnDrawGizmosSelected()
	{
		if (visualizeSpawnSpheres)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, minSpawnDistance);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, antiCollisionIncrement * MaxCollisionChecks);
		}
	}

	Vector3 EstimatedVelocity
	{
		get { return GetComponent<PositionSampler>().EstimateVelocity(); }
	}

	bool IsMovingUp
	{
		get { return EstimatedVelocity.y > 0f; }
	}

	bool IsMovingDown
	{
		get { return EstimatedVelocity.y < 0f; }
	}

	Courage Courage
	{
		get { return GetComponent<Courage>(); }
	}

	/// <summary>
	/// <para>Uses the object's ballistic trajectory to (attempt to) predict its position at a given height.</para>
	/// <para>Returns 'null' when unable to make a prediction.</para>
	/// </summary>
	Vector2? PredictPositionAtHeight(float height)
	{
		Vector2? prevPoint = null;

		foreach (Vector2 point in MxArithmetic.CurvePoints(Physics2D.gravity, EstimatedVelocity, transform.position, 5f, 100))
		{
			if (prevPoint != null && MxArithmetic.IsValueInRange(height, point.y, prevPoint.Value.y))
				return .5f * (point + prevPoint);

			prevPoint = point;
		}

		return null;
	}

	void HandlePlatformSpawning()
	{
		if (Input.GetKeyUp(keyBinding) && Courage.CurrentValue > courageCost)
		{
			bool? actionPerformed = null;

			if (IsMovingUp)
				SpawnPlatformAheadOfSelf(out actionPerformed);
			else
				SpawnPlatformBeneathSelf(out actionPerformed);

			if (actionPerformed == true)
				Courage.SubtractCourage(courageCost);
		}
	}

	void HandlePlatformProjection()
	{
		if (!Input.GetKey(keyBinding))
		{
			projectionPrefabClone.SetActive(false);
			return;
		}

		projectionPrefabClone.SetActive(true);

		if (IsMovingUp)
		{
			Vector3? point = PredictPositionAtHeight(transform.position.y);

			if (point == null)
				return;

			Vector3 newPosition = point.Value;

			newPosition.z = transform.position.z;
			projectionPrefabClone.transform.position = newPosition;

			if (GetComponent<Collider2D>().bounds.Intersects(projectionPrefabClone.GetComponent<Collider2D>().bounds))
				projectionPrefabClone.GetComponent<Renderer>().material = projectionInvalidMaterial;
			else
				projectionPrefabClone.GetComponent<Renderer>().material = projectionValidMaterial;
		}
		else
		{
			projectionPrefabClone.transform.position = transform.position + minSpawnDistance * Vector3.down;
			projectionPrefabClone.GetComponent<Renderer>().material = projectionValidMaterial;
		}
	}

	void SpawnPlatformAheadOfSelf(out bool? actionPerformed)
	{
		if (EstimatedVelocity.x == 0)
		{
			actionPerformed = false;
			return;
		}

		Vector3? spawnPosition = PredictPositionAtHeight(transform.position.y);

		if (spawnPosition == null)
		{
			actionPerformed = false;
			return;
		}

		Vector3 spawnOffset = minSpawnDistance * Vector3.down;
		GameObject spawnedPlatform = (GameObject)Instantiate(platformPrefab, spawnPosition.Value + spawnOffset, Quaternion.identity);

		if (GetComponent<Collider2D>().bounds.Intersects(spawnedPlatform.GetComponent<Collider2D>().bounds))
		{
			Destroy(spawnedPlatform);
			actionPerformed = false;
		}
		else
			actionPerformed = true;
	}

	void SpawnPlatformBeneathSelf(out bool? performedAction)
	{
		Vector3 spawnPosition = transform.position + minSpawnDistance * EstimatedVelocity.normalized;
		GameObject spawnedPlatform = (GameObject)Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

		ShiftPositionUntilNoCollision(spawnedPlatform, EstimatedVelocity);
		performedAction = true;
	}

	void ShiftPositionUntilNoCollision(GameObject obj, Vector3 direction)
	{
		direction.Normalize();

		for (int i = 0; i < MaxCollisionChecks; i++)
		{
			if (GetComponent<Collider2D>().bounds.Intersects(obj.GetComponent<Collider2D>().bounds))
				break;

			obj.transform.position += antiCollisionIncrement * direction;
		}
	}
}
