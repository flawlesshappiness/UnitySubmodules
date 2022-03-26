using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    #region INSTANCE
    private static ViewController _instance;
    public static ViewController Instance { get { return _instance ??  Create(); } private set { _instance = value; } }

    private static ViewController Create()
    {
        var g = new GameObject(nameof(ViewController));
        Instance = g.AddComponent<ViewController>();
        return Instance;
    }
    #endregion
    #region VIEW
    private Dictionary<string, View> _views = new Dictionary<string, View>();
    public T ShowView<T>(float time = 0.5f, string tag = "") where T : View
    {
        // Close current view
        CloseView(time, tag);

        // Create new view
        var path = string.Format("Views/{0}", typeof(T).ToString());
        var view = Instantiate(Resources.Load<GameObject>(path)).GetComponent<T>();
        view.Show(time);

        // Add view to dictionary
        if (!_views.ContainsKey(tag))
        {
            _views.Add(tag, view);
        }
        else
        {
            _views[tag] = view;
        }

        return view;
    }

    public void CloseView(float time = 0.5f, string tag = "")
    {
        if (_views.ContainsKey(tag))
        {
            var view_prev = _views[tag];
            view_prev.Close(time);
            _views.Remove(tag);
        }
    }
    #endregion
}
