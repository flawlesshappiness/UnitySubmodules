using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton : MonoBehaviour
{
    private static Dictionary<System.Type, Singleton> _singletons = new Dictionary<System.Type, Singleton>();

    private static T CreateInstance<T>() where T : Singleton
    {
        var type = typeof(T);
        var g = new GameObject(typeof(T).ToString() + " (Singleton)");
        DontDestroyOnLoad(g);
        var i = g.AddComponent<T>();
        i.Initialize();
        _singletons.Add(type, i);
        return i;
    }

    public static T Instance<T>() where T : Singleton
    {
        var type = typeof(T);
        return _singletons.ContainsKey(type) ?
            (T)_singletons[type] : CreateInstance<T>();
    }

    public virtual void Initialize() { }
}