using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomRigidbodyExtensions
{
    #region 2D
    /// <summary>
    /// Calculates a point on the rigidbody's trajectory
    /// </summary>
    /// <param name="point">Starting point</param>
    /// <param name="velocity">Velocity</param>
    /// <param name="timeStep">Time step</param>
    /// <returns>The calculated point</returns>
    public static Vector2 TrajectoryPoint(this Rigidbody2D rig, Vector2 point, Vector2 velocity, float timeStep)
    {
        float gs = -rig.gravityScale * 5f; // -scale * 10f / 2f;
        float g = gs * (timeStep * timeStep);
        return point + new Vector2(velocity.x * timeStep, velocity.y * timeStep + g);
    }
    #endregion
}
