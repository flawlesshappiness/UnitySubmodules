using System.Collections.Generic;

[System.Serializable]
public class InternalShopSaveData : SaveDataObject
{
    public List<InternalShopPurchase> purchases = new List<InternalShopPurchase>();

    public override void Clear()
    {
        purchases.Clear();
    }
}