using System.Collections.Generic;
using UnityEngine;

public class Database : ScriptableObject
{
    private static Dictionary<System.Type, Database> databases = new Dictionary<System.Type, Database>();

    public static T Load<T>() where T : Database
    {
        var type = typeof(T);
        if (!databases.ContainsKey(type))
        {
            var name = type.Name;
            var db = Resources.Load<T>("Databases/" + name);
            databases.Add(type, db);
        }

        return databases[type] as T;
    }
}