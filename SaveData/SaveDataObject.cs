using System;

[System.Serializable]
public abstract class SaveDataObject
{
    public DateTime update_time;
    public bool from_cloud;

    public abstract void Clear();
}
