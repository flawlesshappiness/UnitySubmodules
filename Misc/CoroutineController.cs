using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineController : MonoBehaviour
{
    #region INSTANCE
    private static CoroutineController _instance;
    public static CoroutineController Instance { get { return _instance ?? Create(); } }

    private static CoroutineController Create()
    {
        var g = new GameObject(nameof(CoroutineController));
        DontDestroyOnLoad(g);
        _instance = g.AddComponent<CoroutineController>();
        return _instance;
    }
    #endregion

    private Dictionary<string, CustomCoroutine> coroutines = new Dictionary<string, CustomCoroutine>();

    public CustomCoroutine Run(IEnumerator enumerator, string id)
    {
        var c = new CustomCoroutine();
        c.ID = id;
        c.Enumerator = enumerator;

        if (coroutines.ContainsKey(id))
        {
            var _c = coroutines[id];
            coroutines.Remove(id);
            if(_c.Coroutine != null)
            {
                StopCoroutine(_c.Coroutine);
            }
        }

        coroutines.Add(id, c);
        c.Coroutine = StartCoroutine(RunCoroutineCr(c));
        return c;
    }

    private IEnumerator RunCoroutineCr(CustomCoroutine c)
    {
        yield return c.Enumerator;
        coroutines.Remove(c.ID);
        c.OnEndAction?.Invoke();
    }
}

public class CustomCoroutine
{
    public string ID { get; set; }
    public IEnumerator Enumerator { get; set; }
    public Coroutine Coroutine { get; set; }
    public System.Action OnEndAction { get; private set; }

    public CustomCoroutine OnEnd(System.Action action)
    {
        OnEndAction = action;
        return this;
    }
}
