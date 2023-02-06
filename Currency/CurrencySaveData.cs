using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrencySaveData : SaveDataObject
{
    public List<CurrencyAmount> currencies = new List<CurrencyAmount>();
}