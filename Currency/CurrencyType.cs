using UnityEngine;

[System.Serializable]
public partial class CurrencyType : FakeEnum
{
    public static readonly CurrencyType Coins = new CurrencyType("Coins");
    public CurrencyType(string id) : base(id) { }
}