using System.Collections;
using UnityEngine;

public static class CustomMonoBehaviourExtensions
{
    public static CustomCoroutine StartCoroutineWithID(this MonoBehaviour mb, IEnumerator enumerator, string id = "")
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }

        return CoroutineController.Instance.Run(enumerator, mb, id);
    }

    public static CustomCoroutine StartCoroutineWithID(this MonoBehaviour mb, IEnumerator enumerator, Object id_object) => 
        StartCoroutineWithID(mb, enumerator, id_object.GetInstanceID().ToString());

    public static CustomCoroutine StartCoroutineWithID(this MonoBehaviour mb, IEnumerator enumerator) =>
        StartCoroutineWithID(mb, enumerator, mb.GetInstanceID().ToString());
}