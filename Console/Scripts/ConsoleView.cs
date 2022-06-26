using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Flawliz.Console
{
    public class ConsoleView : MonoBehaviourExtended
    {
        [SerializeField] private TMP_InputField input;
        [SerializeField] private TMP_Text tmp_autofill;
        [SerializeField] private ConsoleElement element;
        private CanvasGroup cvg { get { return GetComponentOnce<CanvasGroup>(); } }

        public string Autofill { set { tmp_autofill.text = value; } }
        public string Input { get { return input.text; } set { input.text = value; } }

        private void Start()
        {
            element.gameObject.SetActive(false);
            input.onValueChanged.AddListener(OnInputValueChanged);
        }

        public void SetVisible(bool visible)
        {
            cvg.alpha = visible ? 1 : 0;
            cvg.blocksRaycasts = visible;
            cvg.interactable = visible;

            if (visible)
            {
                input.text = "";
                FocusInputField();
            }
        }

        public void FocusInputField()
        {
            EventSystem.current.SetSelectedGameObject(input.gameObject, null);
            input.ActivateInputField();
        }

        public void SetInput(string input)
        {
            this.input.text = input;
            this.input.caretPosition = input.Length;
        }

        public void WriteMessage(string input, string output)
        {
            var e = Instantiate(element.gameObject, element.transform.parent).GetComponent<ConsoleElement>();
            e.gameObject.SetActive(true);
            e.InputText = input;
            e.OutputText = output;
        }

        private void OnInputValueChanged(string value)
        {
            if(value.Length > 0)
            {
                var suggestion = ConsoleController.Instance.GetSuggestion(value);
                Autofill = suggestion.Length > 0 ? value + suggestion.Substring(value.Length, suggestion.Length-value.Length) : "";
            }
            else
            {
                Autofill = "";
            }
        }
    }
}