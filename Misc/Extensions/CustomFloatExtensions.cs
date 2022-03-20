using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomFloatExtensions
{
    public static bool IsDivisbleBy(this float f, float m)
    {
        return (f % m) == 0;
    }

    public static float Abs(this float f)
    {
        return Mathf.Abs(f);
    }

    public static float Min(this float f, float min)
    {
        return Mathf.Min(f, min);
    }

    public static float Max(this float f, float max)
    {
        return Mathf.Max(f, max);
    }

    public static float Range(this float f, float min, float max)
    {
        return f.Min(max).Max(min);
    }
}
