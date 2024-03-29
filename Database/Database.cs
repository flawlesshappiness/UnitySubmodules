using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Database : ScriptableObject
{
    private static Dictionary<System.Type, Database> databases = new Dictionary<System.Type, Database>();

    public static T Load<T>() where T : Database
    {
        var type = typeof(T);
        if (!databases.ContainsKey(type))
        {
            var name = type.Name;
            var db = Resources.Load<T>("Databases/" + name);
            if(db == null)
            {
                Debug.LogError($"Unable to locate database of type: {typeof(T)}");
                return null;
            }
            else
            {
                databases.Add(type, db);
            }
        }

        return databases[type] as T;
    }
}

public abstract class Database<T> : Database
{
    public List<T> collection = new List<T>();

    protected virtual void OnValidate()
    {
        collection = collection.Where(item => item != null).ToList();
    }
}