using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class MxEditor<T> : Editor
		where T : MonoBehaviour
{
	protected T Script
	{
		get { return (T)target; }
	}

	public override sealed void OnInspectorGUI()
	{
		HandleOnInspectorGUI();

		if (GUI.changed)
			EditorUtility.SetDirty(Script);
	}

	protected abstract void HandleOnInspectorGUI();
}
