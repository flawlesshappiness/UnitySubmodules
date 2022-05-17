using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    private static ConsoleController _instance;
    public static ConsoleController Instance { get { return _instance ?? Create(); } }
    private ConsoleView View { get; set; }
    private bool VisibleView { get; set; }

    private static ConsoleController Create()
    {
        var g = new GameObject(nameof(ConsoleController));
        DontDestroyOnLoad(g);
        _instance = g.AddComponent<ConsoleController>();
        _instance.Initialize();
        return _instance;
    }

    public void EnsureExistence()
    {

    }

    private void Initialize()
    {
        var prefab = Resources.Load<ConsoleView>(nameof(ConsoleView));
        View = Instantiate(prefab).GetComponent<ConsoleView>();
        View.SetVisible(false);
        DontDestroyOnLoad(View.gameObject);
        VisibleView = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            ToggleView();
        }
    }

    private void ToggleView()
    {
        VisibleView = !VisibleView;
        View.SetVisible(VisibleView);
    }
}
