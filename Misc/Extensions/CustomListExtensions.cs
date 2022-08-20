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

    public static List<T> TakeRandom<T>(this IList<T> list, int count)
    {
        var list_random = new List<T>();
        var list_copy = new List<T>(list);
        var max = Mathf.Min(list.Count, count);
        for (int i = 0; i < max; i++)
            list_random.Add(list_copy.PopRandom());
        return list_random;
    }

    public static T PopRandom<T>(this IList<T> list)
    {
        var element = list.Random();
        list.Remove(element);
        return element;
    }

    public static T Pop<T>(this IList<T> list)
    {
        var element = list[0];
        list.RemoveAt(0);
        return element;
    }
}
