using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CurrencyInfo), menuName = "Game/Currency/" + nameof(CurrencyInfo), order = 1)]
public class CurrencyInfo : ScriptableObject
{
    public CurrencyType currency;
    public Sprite sprite;

    public static CurrencyInfo Load(CurrencyType currency)
    {
        var db = Database.Load<CurrencyInfoDatabase>();
        if (db == null) return null;
        var info = db.collection.FirstOrDefault(i => i.currency == currency);
        return info;
    }
}