using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SmoothTransformLock))]
public class SoliloCameraBehaviour : MonoBehaviour
{
	public GameObject boy;
	public GameObject girl;
	public AttachMode attachTo = AttachMode.Girl;
	public bool autoAttachOnAwake = true;

	SmoothTransformLock positionLockScript;

	void Awake()
	{
		positionLockScript = GetComponent<SmoothTransformLock>();

		if (autoAttachOnAwake)
			AttachToTarget();
	}

	public void AttachToBoy()
	{
		SwitchAttachMode(AttachMode.Boy);
	}

	public void AttachToGirl()
	{
		SwitchAttachMode(AttachMode.Girl);
	}

	public void AttachToAverage()
	{
		SwitchAttachMode(AttachMode.Average);
	}

	public void SetSampleCount(int value)
	{
		positionLockScript.SampleCount = value;
	}

	void AttachToTarget()
	{
		if (positionLockScript == null)
			positionLockScript = GetComponent<SmoothTransformLock>();

		switch (attachTo)
		{
			case AttachMode.Boy:
				positionLockScript.target = boy.transform;
				break;

			case AttachMode.Girl:
				positionLockScript.target = girl.transform;
				break;

			case AttachMode.Average:
				positionLockScript.target = girl.transform;
				positionLockScript.optionalOthers = new Transform[] { boy.transform };
				break;

			default:
				throw new ArgumentException();
		}

		positionLockScript.InvokeOnValidate();
	}

	void SwitchAttachMode(AttachMode value)
	{
		attachTo = value;
		AttachToTarget();
	}

	public enum AttachMode
	{
		Boy,
		Girl,
		Average
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(SoliloCameraBehaviour))]
	class InspectorEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			SoliloCameraBehaviour script = (SoliloCameraBehaviour)target;

			DrawDefaultInspector();

			if (GUILayout.Button("Attach Now"))
				script.AttachToTarget();
		}
	}
#endif
}
