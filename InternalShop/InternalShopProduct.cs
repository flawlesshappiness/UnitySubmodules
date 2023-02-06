using UnityEngine;

[CreateAssetMenu(fileName = nameof(InternalShopProduct), menuName = "Game/Internal Shop/" + nameof(InternalShopProduct), order = 1)]
public class InternalShopProduct : ScriptableObject
{
    public InternalShopProductID id;
    public CurrencyAmount price;

    public bool IsUnlocked() => InternalShopController.Instance.IsPurchased(this);
}