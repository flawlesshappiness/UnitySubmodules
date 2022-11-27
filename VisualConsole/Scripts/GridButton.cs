using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flawliz.VisualConsole
{
    public class GridButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmp;
        [SerializeField] private Button btn;

        public string Text { set { tmp.text = value; } }
        public Button.ButtonClickedEvent onClick { get { return btn.onClick; } }
    }
}