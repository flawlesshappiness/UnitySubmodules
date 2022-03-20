using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CustomColorExtensions {
	public static string ToHex(this Color32 color)
	{
		return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
	}

    #region A
    public static Color SetA(this Color color, float a)
	{
		return new Color(color.r, color.g, color.b, a);
	}

	public static Color32 SetA(this Color32 color, float a)
	{
		return new Color(color.r, color.g, color.b, a);
	}
    #endregion
    #region R
    public static Color SetR(this Color color, float r)
    {
        return new Color(r, color.g, color.b, color.a);
    }

    public static Color32 SetR(this Color32 color, float r)
    {
        return new Color(r, color.g, color.b, color.a);
    }
    #endregion
    #region G
    public static Color SetG(this Color color, float g)
    {
        return new Color(color.r, g, color.b, color.a);
    }

    public static Color32 SetG(this Color32 color, float g)
    {
        return new Color(color.r, g, color.b, color.a);
    }
    #endregion
    #region B
    public static Color SetB(this Color color, float b)
    {
        return new Color(color.r, color.g, b, color.a);
    }

    public static Color32 SetB(this Color32 color, float b)
    {
        return new Color(color.r, color.g, b, color.a);
    }
    #endregion

    public static Color32 To32(this Color color)
    {
        byte r = (byte)(255 * color.r);
        byte g = (byte)(255 * color.g);
        byte b = (byte)(255 * color.b);
        byte a = (byte)(255 * color.a);
        return new Color32(r, g, b, a);
    }

    public static Color32 Invert(this Color color)
    {
        var c = color.To32();
        byte r = (byte)~c.r;
        byte g = (byte)~c.g;
        byte b = (byte)~c.b;
        return new Color32(r, g, b, c.a);
    }

    public static Color SetBrightness(this Color color, float perc)
	{
		return new Color(color.r*perc, color.g*perc, color.b*perc);
	}

	public static float DistanceTo(this Color c1, Color c2)
	{
		float r = Mathf.Abs(c1.r - c2.r);
		float g = Mathf.Abs(c1.g - c2.g);
		float b = Mathf.Abs(c1.b - c2.b);
		float a = Mathf.Abs(c1.a - c2.a);
		return (r+g+b+a) / 4f;
	}

	public static float DistanceTo(this Color32 c1, Color32 c2)
	{
		float r = Mathf.Abs(c1.r - c2.r);
		float g = Mathf.Abs(c1.g - c2.g);
		float b = Mathf.Abs(c1.b - c2.b);
		float a = Mathf.Abs(c1.a - c2.a);
		return (r+g+b+a) / 4f;
	}
}
