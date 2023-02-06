using UnityEngine;

[CreateAssetMenu(fileName = nameof(CurrencyInfoDatabase), menuName = "Game/Currency/" + nameof(CurrencyInfoDatabase), order = 1)]
public class CurrencyInfoDatabase : Database<CurrencyInfo>
{
    
}