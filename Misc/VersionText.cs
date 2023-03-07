using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    [SerializeField] private TMP_Text tmp;

    private void Reset()
    {
        FindText();
        UpdateText();
    }

    private void OnValidate()
    {
        UpdateText();
    }

    private void Start()
    {
        FindText();
        UpdateText();
    }

    private void FindText()
    {
        tmp = tmp ?? GetComponent<TMP_Text>();
    }

    private void UpdateText()
    {
        if(tmp != null)
        {
            tmp.text = Application.version;
        }
    }
}