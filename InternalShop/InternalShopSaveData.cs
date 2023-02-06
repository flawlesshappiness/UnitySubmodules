using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InternalShopSaveData : SaveDataObject
{
    public List<InternalShopPurchase> purchases = new List<InternalShopPurchase>();
}