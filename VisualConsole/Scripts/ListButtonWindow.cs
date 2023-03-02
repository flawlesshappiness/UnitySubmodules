using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Flawliz.VisualConsole
{
    public class ListButtonWindow : VisualConsoleWindow
    {
        [SerializeField] private ListButton template_button;
        [SerializeField] private TMP_Text template_text;
        private List<GameObject> elements = new List<GameObject>();

        private void Start()
        {
            template_button.gameObject.SetActive(false);
            template_text.gameObject.SetActive(false);
        }

        public override void Clear()
        {
            base.Clear();
            elements.ForEach(e => Destroy(e));
            elements.Clear();
        }

        public ListButton CreateButton(string text)
        {
            var btn = Instantiate(template_button, template_button.transform.parent);
            btn.gameObject.SetActive(true);
            btn.TextCenter = text;
            btn.TextLeft = "";
            btn.TextRight = "";
            elements.Add(btn.gameObject);
            return btn;
        }

        public TMP_Text CreateText(string text)
        {
            var t = Instantiate(template_text, template_text.transform.parent);
            t.gameObject.SetActive(true);
            t.text = text;
            elements.Add(t.gameObject);
            return t;
        }
    }
}