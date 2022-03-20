using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomIntegerExtensions {

    public static int Abs(this int i)
    {
        return Mathf.Abs(i);
    }

	public static int AbsMod(this int i, int m)
	{
		i = i % m;
		return i < 0 ? i+m : i;
	}

    public static bool IsDivisbleBy(this int i, int m)
    {
        return (i % m) == 0;
    }

    public static int IncrementOverArrayLength(this int i, int arrayLength)
    {
        if (arrayLength == 0) return 0;
        return (i + 1) % arrayLength;
    }

    public static float PercOf(this int i, float f)
    {
        return ((float)i) / f;
    }

    public static float PercOf(this int i, int x)
    {
        return i.PercOf((float)x);
    }
}
