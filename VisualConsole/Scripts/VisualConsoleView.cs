using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Flawliz.VisualConsole
{
    public class VisualConsoleView : View
    {
        [SerializeField] private Button btn_back;

        private List<VisualConsoleWindow> windows;
        private VisualConsoleWindow active_window;

        private void Awake()
        {
            windows = GetComponentsInChildren<VisualConsoleWindow>(true).ToList();
            windows.ForEach(w => w.SetVisible(false));
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        private T GetWindow<T>() where T : VisualConsoleWindow
        {
            return windows.FirstOrDefault(w => w.GetType() == typeof(T)) as T;
        }

        public T ShowWindow<T>() where T : VisualConsoleWindow
        {
            btn_back.gameObject.SetActive(false);

            if(active_window != null)
            {
                active_window.SetVisible(false);
            }

            var window = GetWindow<T>();
            window.SetVisible(true);
            active_window = window;
            return window;
        }

        public GridButtonWindow ShowGrid() => ShowWindow<GridButtonWindow>();
        public ListButtonWindow ShowList() => ShowWindow<ListButtonWindow>();

        public void ShowBackButton(UnityAction action)
        {
            btn_back.gameObject.SetActive(true);
            btn_back.onClick.RemoveAllListeners();
            btn_back.onClick.AddListener(action);
        }
    }
}