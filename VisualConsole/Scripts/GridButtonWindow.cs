using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Flawliz.VisualConsole
{
    public class GridButtonWindow : VisualConsoleWindow
    {
        [SerializeField] private GridButton template_button;

        private List<GridButton> btns = new List<GridButton>();

        private void Start()
        {
            template_button.gameObject.SetActive(false);
        }

        public override void Clear()
        {
            base.Clear();
            btns.ForEach(btn => Destroy(btn.gameObject));
            btns.Clear();
        }

        public GridButton CreateButton(string text, UnityAction onClick)
        {
            var btn = Instantiate(template_button, template_button.transform.parent);
            btn.gameObject.SetActive(true);
            btns.Add(btn);
            btn.Text = text;
            btn.onClick.AddListener(onClick);
            return btn;
        }
    }
}