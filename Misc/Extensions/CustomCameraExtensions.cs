using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomCameraExtensions {

	public static float GetScreenHeight(this Camera cam)
	{
		return cam.orthographicSize * 2f;
	}

	public static float GetScreenWidth(this Camera cam)
	{
		return GetScreenHeight(cam) / Screen.height * Screen.width;
	}

	public static Vector2 ScreenToWorldDimensions(this Camera cam)
	{
		float height = GetScreenHeight(cam);
		float width = height / Screen.height * Screen.width;
		return new Vector2(width, height);
	}
}
