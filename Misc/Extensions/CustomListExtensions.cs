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
            int k = UnityEngine.Random.Range(0, list.Count);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public static T Random<T>(this IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T PopRandom<T>(this IList<T> list)
    {
        var element = list.Random();
        list.Remove(element);
        return element;
    }
}
