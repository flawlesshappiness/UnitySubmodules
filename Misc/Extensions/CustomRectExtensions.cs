using UnityEngine;

public static class CustomRectExtensions
{
    public static Vector2 GetNormalizedPosition(this Rect rect, Vector2 position)
    {
        var max = Mathf.Min(rect.xMax, rect.yMax);
        var x = (position.x - rect.xMin) / (max - rect.xMin);
        var y = (position.y - rect.yMin) / (max - rect.yMin);
        return new Vector2(x, y);
    }
}