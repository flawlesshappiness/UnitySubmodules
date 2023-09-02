using Flawliz.Console;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataController : Singleton
{
    public static SaveDataController Instance { get { return Instance<SaveDataController>(); } }
    protected override void Initialize()
    {
        ConsoleController.Instance.RegisterCommand("ClearSaveData", ClearSaveData);
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }

    private Dictionary<System.Type, SaveDataObject> data_objects = new Dictionary<System.Type, SaveDataObject>();

    public T Get<T>() where T : SaveDataObject, new()
    {
        if (data_objects.ContainsKey(typeof(T)))
        {
            return (T)data_objects[typeof(T)];
        }
        else
        {
            var json = PlayerPrefs.GetString(typeof(T).AssemblyQualifiedName);
            T data = string.IsNullOrEmpty(json) ? new T() : JsonConvert.DeserializeObject<T>(json);
            data_objects.Add(typeof(T), data);
            Save<T>();
            return data;
        }
    }

    public void SaveAll()
    {
        foreach (var kvp in data_objects)
        {
            Save(kvp.Key);
        }
    }

    public void Save<T>() where T : SaveDataObject, new()
    {
        var data = Get<T>();
        Save(typeof(T));
    }

    public void Save(System.Type type)
    {
        var data = data_objects[type];
        var json = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(type.AssemblyQualifiedName, json);
    }

    public void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
        data_objects.Clear();
    }
}
