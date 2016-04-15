using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEditor;
using Assets.MxUnity;

public static class EditorOps
{
	public static void KeyCodeField(string label, ref KeyCode key)
	{
		key = (KeyCode)EditorGUILayout.EnumPopup(label, key);
	}

	public static void Button(string label, UnityAction firstAction, params UnityAction[] additionalActions)
	{
		if (GUILayout.Button(label))
		{
			firstAction();

			foreach (UnityAction e in additionalActions)
				e();
		}
	}

	public static void FloatField(string labelText, ref float value)
	{
		value = EditorGUILayout.FloatField(labelText, value);
	}

	public static void UnsignedFloatField(string labelText, ref float value)
	{
		FloatField(labelText, ref value);
		MxArithmetic.Unsigned(ref value);
	}

	public static void Toggle(string labelText, ref bool value)
	{
		value = EditorGUILayout.Toggle(labelText, value);
	}

	public static void Warning(string message)
	{
		EditorGUILayout.HelpBox(message, MessageType.Warning);
	}

	public static void Header(string text)
	{
		GUIStyle guiStyle = new GUIStyle(GUIStyle.none);
		guiStyle.fontStyle = FontStyle.Bold;

		EditorGUILayout.Space();
		GUILayout.Label(text, guiStyle);
	}

	public static void TagField(string labelText, ref string selectedTag)
	{
		selectedTag = EditorGUILayout.TagField(labelText, selectedTag);
	}
}
