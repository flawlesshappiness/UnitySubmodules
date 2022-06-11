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

    public CustomCoroutine Run(IEnumerator enumerator, string id) => Run(enumerator, this, id);

    public CustomCoroutine Run(IEnumerator enumerator, MonoBehaviour connection, string id)
    {
        var c = new CustomCoroutine();
        c.ID = id;
        c.Connection = connection;

        Kill(id);
        coroutines.Add(id, c);
        c.Coroutine = StartCoroutine(RunCr());
        return c;

        IEnumerator RunCr()
        {
            yield return enumerator;
            coroutines.Remove(c.ID);
            c.OnEndAction?.Invoke();
        }
    }

    public void Kill(CustomCoroutine cr)
    {
        if(cr != null)
        {
            Kill(cr.ID);
        }
    }

    private void Kill(string id)
    {
        if (coroutines.ContainsKey(id))
        {
            var _c = coroutines[id];
            coroutines.Remove(id);
            if (_c.Coroutine != null)
            {
                _c.Connection.StopCoroutine(_c.Coroutine);
            }
        }
    }

    public bool Has(string id)
    {
        return coroutines.ContainsKey(id);
    }
}

public class CustomCoroutine
{
    public string ID { get; set; }
    public Coroutine Coroutine { get; set; }
    public MonoBehaviour Connection { get; set; }
    public System.Action OnEndAction { get; private set; }

    public CustomCoroutine OnEnd(System.Action action)
    {
        OnEndAction = action;
        return this;
    }
}