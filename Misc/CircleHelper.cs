using UnityEngine;

public static class CircleHelper
{
    public static Vector2 Point(float radius, float angleInDegrees)
    {
        var rad = angleInDegrees * Mathf.Deg2Rad;
        var x = radius * Mathf.Cos(rad);
        var y = radius * Mathf.Sin(rad);
        return new Vector2(x, y);
    }

    public static Vector2[] Points(float radius, int count, float offset = 0)
    {
        var points = new Vector2[count];
        var angle_delta = 360f / count;
        for (int i = 0; i < count; i++)
        {
            var angle = i * angle_delta + angle_delta * offset;
            points[i] = Point(radius, angle);
        }
        return points;
    }
}