using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Flawliz.VisualConsole
{
    public class ListButtonWindow : VisualConsoleWindow
    {
        [SerializeField] private ListButton template_button;
        private List<ListButton> btns = new List<ListButton>();

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

        public ListButton CreateButton(string text, UnityAction onClick)
        {
            var btn = Instantiate(template_button, template_button.transform.parent);
            btn.gameObject.SetActive(true);
            btn.TextCenter = text;
            btn.TextLeft = "";
            btn.TextRight = "";
            btn.onClick.AddListener(onClick);
            btns.Add(btn);
            return btn;
        }
    }
}