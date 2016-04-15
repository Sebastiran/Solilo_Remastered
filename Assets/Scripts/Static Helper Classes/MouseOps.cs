using UnityEngine;
using System.Collections;

public static class MouseOps
{
	public static Vector2 CurrentInput
	{
		get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
	}
}
