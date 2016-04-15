using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CollisionTrigger))]
public class CollisionTriggerEditor : MxEditor<CollisionTrigger>
{
	protected override void HandleOnInspectorGUI()
	{
		DrawDefaultInspector();

		if (Script.filterOnTag)
			EditorOps.TagField("Target tag", ref Script.targetTag);
	}
}
