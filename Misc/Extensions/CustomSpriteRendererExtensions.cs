using UnityEngine;

public static class CustomSpriteRendererExtensions
{
    public static void SetColor(this SpriteRenderer spr, Color color)
    {
        spr.color = color;
    }

    public static void SetAlpha(this SpriteRenderer spr, float alpha)
    {
        var c = spr.color;
        spr.color = new Color(c.r, c.g, c.b, alpha);
    }
}