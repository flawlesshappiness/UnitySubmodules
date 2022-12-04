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

        c.Coroutine = connection.StartCoroutine(RunCr());
        return c;

        IEnumerator RunCr()
        {
            c.Completed = false;
            yield return enumerator;
            c.Completed = true;
            coroutines.Remove(c.ID);
        }
    }

    public void Kill(CustomCoroutine cr)
    {
        if(cr != null)
        {
            Kill(cr.ID);
        }
    }

    public void Kill(string id)
    {
        if (coroutines.ContainsKey(id))
        {
            var _c = coroutines[id];
            coroutines.Remove(id);

            if(_c.Coroutine != null)
            {
                _c.Completed = true;
                _c.Connection.StopCoroutine(_c.Coroutine);
            }
        }
    }

    public bool HasID(string id)
    {
        return coroutines.ContainsKey(id);
    }
}

public class CustomCoroutine : CustomYieldInstruction
{
    public string ID { get; set; }
    public Coroutine Coroutine { get; set; }
    public MonoBehaviour Connection { get; set; }
    public bool Completed { get; set; }

    public void Kill() => CoroutineController.Instance.Kill(ID);

    // CustomYieldInstruction
    public override bool keepWaiting => !Completed;

}