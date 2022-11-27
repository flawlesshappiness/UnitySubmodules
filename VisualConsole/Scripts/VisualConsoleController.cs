using UnityEngine;

namespace Flawliz.VisualConsole
{
    public class VisualConsoleController : Singleton
    {
        public static VisualConsoleController Instance { get { return Instance<VisualConsoleController>(); } }

        private VisualConsoleView view;
        private bool IsViewVisible { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            view = Instantiate(Resources.Load<VisualConsoleView>(nameof(VisualConsoleView)));
            view.SetVisible(false);
            DontDestroyOnLoad(view.gameObject);
            IsViewVisible = false;
        }

        public bool ToggleView(out VisualConsoleView view)
        {
            IsViewVisible = !IsViewVisible;
            view = this.view;
            view.SetVisible(IsViewVisible);
            return IsViewVisible;
        }

        public void HideView()
        {
            IsViewVisible = false;
            view.SetVisible(false);
        }
    }
}