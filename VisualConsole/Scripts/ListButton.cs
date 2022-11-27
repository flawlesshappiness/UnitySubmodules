using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flawliz.VisualConsole
{
    public class ListButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmp_middle, tmp_left, tmp_right;
        [SerializeField] private Button btn;

        public string TextCenter { set { tmp_middle.text = value; } }
        public string TextLeft { set { tmp_left.text = value; } }
        public string TextRight { set { tmp_right.text = value; } }
        public Button.ButtonClickedEvent onClick { get { return btn.onClick; } }
    }
}