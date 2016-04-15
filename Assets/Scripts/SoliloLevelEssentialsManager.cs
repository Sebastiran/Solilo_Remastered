using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CourageController))]
public class SoliloLevelEssentialsManager : MonoBehaviour
{
	public GameObject player;
	public OutOfCameraViewIndicatorBehaviour outOfBoundsIndicator;
	public CameraBoundsHandler cameraBoundsHandler;

	[Header("Null Reference Auto-Search")]
	public string playerObjectName = "";

	void Awake()
	{
		if (player == null)
			player = GameObject.Find(playerObjectName);

		outOfBoundsIndicator.trackedObject = player;
		cameraBoundsHandler.player = player;
	}
}
