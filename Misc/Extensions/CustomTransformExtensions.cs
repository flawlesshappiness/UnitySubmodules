using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomTransformExtensions
{
	public static void LookAt2D(this Transform transform, Vector3 position, float defaultAngle)
	{
		var dir = position - transform.position;
		var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(defaultAngle - angle, Vector3.forward);
	}

    public static Vector3 DirectionTo(this Transform transform, Vector3 pos)
    {
        return pos - transform.position;
    }

    public static Vector3 DirectionTo(this Transform transform, Transform target)
    {
        return transform.DirectionTo(target.position);
    }

    public static void SetGlobalScale(this Transform transform, Vector3 scale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(scale.x / transform.lossyScale.x, scale.y / transform.lossyScale.y, scale.z / transform.lossyScale.z);
    }
}