using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton : MonoBehaviour, IComparable<Singleton>
{
    private static Dictionary<Type, Singleton> _singletons = new Dictionary<Type, Singleton>();

    public static void CreateAllSingletons()
    {
        var singletons = ReflectiveEnumerator.GetEnumerableOfType<Singleton>();
        foreach(var singleton in singletons)
        {
            var type = singleton.GetType();
            CreateInstance(type);
        }
        InitializeAllSingletons();
    }

    private static void InitializeAllSingletons()
    {
        foreach(var singleton in _singletons.Values)
        {
            singleton.Initialize();
        }
    }

    private static Singleton CreateInstance(Type type)
    {
        var g = new GameObject(type.Name + " (Singleton)");
        DontDestroyOnLoad(g);
        var s = (Singleton)g.AddComponent(type);
        _singletons.Add(type, s);
        return s;
    }

    public static T Instance<T>() where T : Singleton
    {
        var type = typeof(T);
        return (T)_singletons[type];
    }

    protected virtual void Initialize() { }

    public int CompareTo(Singleton other)
    {
        return GetType().Name.CompareTo(other.GetType().Name);
    }
}