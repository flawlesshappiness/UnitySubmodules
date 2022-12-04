using System.Collections;
using UnityEditor;
using UnityEngine;

public static class CustomMonoBehaviourExtensions
{
    public static CustomCoroutine StartCoroutineWithID(this MonoBehaviour mb, IEnumerator enumerator, string id = "")
    {
        if (string.IsNullOrEmpty(id))
        {
            id = new GUID().ToString();
        }

        return CoroutineController.Instance.Run(enumerator, mb, id);
    }
}