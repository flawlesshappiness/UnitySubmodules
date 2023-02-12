using System.Globalization;
using System.Linq;
using UnityEngine;

public class CurrencyController : Singleton
{
    public static CurrencyController Instance { get { return Instance<CurrencyController>(); } }

    public CurrencySaveData Data { get { return SaveDataController.Instance.Get<CurrencySaveData>(); } }

    public event System.Action<CurrencyType> onCurrencyChanged;
    public event System.Action<CurrencyType> onCurrencyGained;
    public event System.Action<CurrencyType> onCurrencySpent;

    public int GetAmount(CurrencyType type)
    {
        var amount = GetCurrencyAmount(type);
        return amount.amount;
    }

    public void Gain(CurrencyType type, int amount)
    {
        Adjust(type, Mathf.Abs(amount));
        onCurrencyGained?.Invoke(type);
    }

    public void Spend(CurrencyType type, int amount)
    {
        Adjust(type, -Mathf.Abs(amount));
        onCurrencySpent?.Invoke(type);
    }

    private void Adjust(CurrencyType type, int amount)
    {
        var currency_amount = GetCurrencyAmount(type);
        currency_amount.amount = Mathf.Max(0, currency_amount.amount + amount);
        onCurrencyChanged?.Invoke(type);
    }

    public bool CanAfford(CurrencyAmount price)
    {
        var amount = GetCurrencyAmount(price.type);
        return amount.amount >= price.amount;
    }

    private CurrencyAmount GetCurrencyAmount(CurrencyType type)
    {
        var amount = Data.currencies.FirstOrDefault(a => a.type == type);
        if(amount == null)
        {
            amount = new CurrencyAmount { type = type, amount = 0 };
            Data.currencies.Add(amount);
        }
        return amount;
    }

    public static string FormatCurrencyString(int amount)
    {
        var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = " ";
        return amount.ToString("#,0", nfi);
    }
}