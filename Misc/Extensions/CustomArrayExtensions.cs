using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomArrayExtensions
{
    public static T IndexOrMax<T>(this T[] array, int idx)
    {
        return array[Mathf.Min(idx, array.Length-1)];
    }

    public static T Random<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }
}