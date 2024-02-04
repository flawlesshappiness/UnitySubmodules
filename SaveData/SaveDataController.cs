using Flawliz.Console;
using Newtonsoft.Json;
using System;
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
            var filename = GetTypeName(typeof(T));
            var local_data = GetLocal<T>();
            var cloud_data = SteamIntegration.Instance.LoadCloudData<T>(filename);
            var use_cloud = cloud_data != null && cloud_data.update_time > local_data.update_time;
            var most_recent = use_cloud ? cloud_data : local_data;
            data_objects.Add(typeof(T), most_recent);
            Save<T>();
            return most_recent;
        }
    }

    private T GetLocal<T>() where T : SaveDataObject, new()
    {
        var name = GetTypeName(typeof(T));
        var json = PlayerPrefs.GetString(name);
        T data = string.IsNullOrEmpty(json) ? new T() : JsonConvert.DeserializeObject<T>(json);
        return data;
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
        if (!data_objects.ContainsKey(typeof(T)))
        {
            GetLocal<T>();
        }

        Save(typeof(T));
    }

    public void Save(System.Type type)
    {
        var name = GetTypeName(type);
        var data = data_objects[type];
        data.update_time = DateTime.UtcNow;
        var json = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(name, json);
        SteamIntegration.Instance.SaveCloudData(name, json);
    }

    public void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
        data_objects.Clear();
    }

    private string GetTypeName(Type type)
    {
        return type.FullName;
    }
}
