using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while(n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }
}
