using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RandomMountainGenerator))]
public class RandomMountainGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Reset Graph"))
			((RandomMountainGenerator)target).ResetGraph();

		if (GUILayout.Button("Clear All"))
			((RandomMountainGenerator)target).ClearExistingMountains();

		if (GUILayout.Button("Regenerate"))
			((RandomMountainGenerator)target).Regenerate();

		EditorGUILayout.EndHorizontal();
	}
}
