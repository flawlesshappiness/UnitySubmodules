using System.Collections.Generic;

[System.Serializable]
public class CurrencySaveData : SaveDataObject
{
    public List<CurrencyAmount> currencies = new List<CurrencyAmount>();

    public override void Clear()
    {
        currencies.Clear();
    }
}