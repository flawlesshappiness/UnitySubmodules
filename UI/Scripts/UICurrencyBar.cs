using Flawliz.Lerp;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICurrencyBar : MonoBehaviour
{
    [SerializeField] private Image img_icon;
    [SerializeField] private TMP_Text tmp_value;

    public CurrencyType currency;
    public bool update_on_start;

    private int current_text_value;

    public void Start()
    {
        UpdateCurrencyInfo();
        if (update_on_start) UpdateValue();
    }

    private void OnEnable()
    {
        CurrencyController.Instance.onCurrencyChanged += OnCurrencyChanged;
    }

    private void OnDisable()
    {
        CurrencyController.Instance.onCurrencyChanged -= OnCurrencyChanged;
    }

    private void OnValidate()
    {
        UpdateCurrencyInfo();
    }

    private void OnCurrencyChanged(CurrencyType type)
    {
        if(type == currency)
        {
            AnimateUpdateValue(0.5f);
        }
    }

    public void SetCurrencyType(CurrencyType currency)
    {
        this.currency = currency;
        UpdateCurrencyInfo();
        UpdateValue();
    }

    private void UpdateCurrencyInfo()
    {
        var info = CurrencyInfo.Load(currency);
        if(info != null)
        {
            img_icon.sprite = info.sprite;
        }
    }

    private int GetAmount() => CurrencyController.Instance.GetAmount(currency);

    public void UpdateValue()
    {
        SetValueText(GetAmount());
    }

    public CustomCoroutine AnimateUpdateValue(float duration, AnimationCurve curve = null)
    {
        return this.StartCoroutineWithID(Cr(), "AnimateUpdateValue_" + GetInstanceID());
        IEnumerator Cr()
        {
            curve = curve ?? EasingCurves.Linear;
            var start = current_text_value;
            var end = GetAmount();
            yield return LerpEnumerator.Value(duration, f =>
            {
                var t = curve.Evaluate(f);
                var v = (int)Mathf.Lerp(start, end, t);
                SetValueText(v);
            }).UnscaledTime();
            SetValueText(GetAmount());
        }
    }

    public void SetValueText(int value)
    {
        value = Mathf.Min(value, 9999999);
        tmp_value.text = CurrencyController.FormatCurrencyString(value);
        current_text_value = value;
    }
}