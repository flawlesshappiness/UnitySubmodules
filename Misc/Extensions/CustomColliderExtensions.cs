using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomColliderExtensions {

    #region BOX COLLIDER
    public static Collider2D GetRaycastHit(this BoxCollider2D col)
    {
        return Physics2D.OverlapBox(col.transform.position.ToVector2() + col.offset, col.bounds.size, 0f);
    }

    public static Collider2D GetRaycastHit(this BoxCollider2D col, LayerMask lm)
    {
        return Physics2D.OverlapBox(col.transform.position.ToVector2() + col.offset, col.bounds.size, 0f, lm);
    }

    public static Collider2D[] GetRaycastHits(this BoxCollider2D col)
    {
        return Physics2D.OverlapBoxAll(col.transform.position.ToVector2() + col.offset, col.bounds.size, 0f);
    }

    public static Collider2D[] GetRaycastHits(this BoxCollider2D col, LayerMask lm)
    {
        return Physics2D.OverlapBoxAll(col.transform.position.ToVector2() + col.offset, col.bounds.size, 0f, lm);
    }
    #endregion
    #region CIRCLE COLLIDER
    public static Vector2 GetRandomPosition(this CircleCollider2D col)
	{
		return col.GetRandomPosition(1f);
	}

	public static Vector2 GetRandomPosition(this CircleCollider2D col, float mult)
	{
		return col.transform.position.ToVector2() + Random.insideUnitCircle * (col.radius * mult);
	}

	public static int GetAreaAmount(this CircleCollider2D col, float amountPerUnit)
	{
		return (int)(amountPerUnit * (Mathf.PI * Mathf.Pow(col.radius, 2f)));
	}

    public static Collider2D GetRaycastHit(this CircleCollider2D col)
    {
        return Physics2D.OverlapCircle(col.transform.position.ToVector2() + col.offset, col.radius);
    }

    public static Collider2D GetRaycastHit(this CircleCollider2D col, LayerMask lm)
    {
        return Physics2D.OverlapCircle(col.transform.position.ToVector2() + col.offset, col.radius, lm);
    }

    public static Collider2D[] GetRaycastHits(this CircleCollider2D col)
    {
        return Physics2D.OverlapCircleAll(col.transform.position.ToVector2() + col.offset, col.radius);
    }

    public static Collider2D[] GetRaycastHits(this CircleCollider2D col, LayerMask lm)
    {
        return Physics2D.OverlapCircleAll(col.transform.position.ToVector2() + col.offset, col.radius, lm);
    }
	#endregion
}
