using System.Collections;
using UnityEngine;

public static class CustomMonoBehaviourExtensions
{
    public static CustomCoroutine StartCoroutineWithID(this MonoBehaviour mb, IEnumerator enumerator, string id = "", bool use_instance_id = true)
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
            id += use_instance_id ? mb.GetInstanceID().ToString() : "";
        }

        return CoroutineController.Instance.Run(enumerator, mb, id);
    }

    public static CustomCoroutine StartCoroutineWithID(this MonoBehaviour mb, IEnumerator enumerator, Object id_object) => 
        StartCoroutineWithID(mb, enumerator, id_object.GetInstanceID().ToString(), false);

    public static void StopCoroutineWithID(this MonoBehaviour mb, string id, bool use_instance_id = true)
    {
        if (string.IsNullOrEmpty(id)) return;
        id += use_instance_id ? mb.GetInstanceID().ToString() : "";
        CoroutineController.Instance.Kill(id);
    }
}