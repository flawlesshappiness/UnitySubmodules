using Flawliz.Console;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
            var json = PlayerPrefs.GetString(typeof(T).ToString());
            T data = string.IsNullOrEmpty(json) ? new T() : JsonUtility.FromJson<T>(json);
            data_objects.Add(typeof(T), data);
            Save<T>();
            return data;
        }
    }

    public void SaveAll()
    {
        foreach(var kvp in data_objects)
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
        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(type.ToString(), json);
    }

    private void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
        data_objects.Clear();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
