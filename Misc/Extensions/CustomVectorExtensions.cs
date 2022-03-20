using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomVectorExtension {

	#region VECTOR3
	public static Vector2[] ToVector2(this Vector3[] array)
	{
		return System.Array.ConvertAll<Vector3, Vector2>(array, ToVector2);
	}

	public static Vector2 ToVector2(this Vector3 v)
	{
		return new Vector2(v.x, v.y);
	}

	public static Vector3 AddX(this Vector3 v, float x)
	{
		return new Vector3(v.x+x, v.y, v.z);
	}

	public static Vector3 AddY(this Vector3 v, float y)
	{
		return new Vector3(v.x, v.y+y, v.z);
	}

	public static Vector3 AddZ(this Vector3 v, float z)
	{
		return new Vector3(v.x, v.y, v.z+z);
	}

	public static Vector3 SetX(this Vector3 v, float x)
	{
		return new Vector3(x, v.y, v.z);
	}

	public static Vector3 SetY(this Vector3 v, float y)
	{
		return new Vector3(v.x, y, v.z);
	}

	public static Vector3 SetZ(this Vector3 v, float z)
	{
		return new Vector3(v.x, v.y, z);
	}
	#endregion
	#region VECTOR2
	public static Vector2 Smallest(this Vector2 v)
	{
		v = v.normalized;
		var e = Mathf.Pow(10, -6);
		return v * e;
	}

	public static Vector2 AddX(this Vector2 v, float x)
	{
		return new Vector2(v.x+x, v.y);
	}

	public static Vector2 AddY(this Vector2 v, float y)
	{
		return new Vector2(v.x, v.y+y);
	}

    public static Vector2 SetX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }

    public static Vector2 SetY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static Vector3 ToVector3(this Vector2 v)
	{
		return new Vector3(v.x, v.y, 0f);
	}
	#endregion
}
