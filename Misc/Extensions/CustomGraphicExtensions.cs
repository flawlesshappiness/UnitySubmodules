using UnityEngine;
using UnityEngine.UI;

public static class CustomGraphicExtensions
{
    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        var c = graphic.color;
        graphic.color = new Color(c.r, c.g, c.b, alpha);
    }
}