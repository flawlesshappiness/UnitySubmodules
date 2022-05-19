using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Flawliz.Console
{
    public class ConsoleElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmp_input;
        [SerializeField] private TMP_Text tmp_output;

        public string InputText { set { tmp_input.text = value; } }
        public string OutputText { set { tmp_output.text = value; } }
    }
}