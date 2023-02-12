using System.Linq;

public class InternalShopController : Singleton
{
    public static InternalShopController Instance { get { return Instance<InternalShopController>(); } }

    private InternalShopSaveData Data { get { return SaveDataController.Instance.Get<InternalShopSaveData>(); } }

    public event System.Action<InternalShopPurchase> onPurchase;

    public bool IsPurchased(InternalShopProduct product) => IsPurchased(product.id);
    public bool IsPurchased(InternalShopProductID id)
    {
        return GetPurchase(id).amount > 0;
    }

    public InternalShopPurchase GetPurchase(InternalShopProductID id)
    {
        var purchase = Data.purchases.FirstOrDefault(p => p.id == id);
        if(purchase == null)
        {
            purchase = new InternalShopPurchase { id = id, amount = 0 };
            Data.purchases.Add(purchase);
        }

        return purchase;
    }

    public int GetPurchaseAmount(InternalShopProductID id) => GetPurchase(id).amount;

    public bool TryPurchaseProduct(InternalShopProduct product, int amount = 1)
    {
        if (CanAfford(product))
        {
            var purchase = new InternalShopPurchase { id = product.id, amount = amount };
            PurchaseProduct(purchase);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PurchaseProduct(InternalShopPurchase active_purchase)
    {
        var product = Database.Load<InternalShopProductDatabase>().collection.FirstOrDefault(p => p.id == active_purchase.id);
        if (product == null) return;

        CurrencyController.Instance.Spend(product.price.type, product.price.amount);

        var saved_purchase = GetPurchase(active_purchase.id);
        saved_purchase.amount += active_purchase.amount;
        onPurchase?.Invoke(active_purchase);
    }

    public bool CanAfford(InternalShopProduct product) => CurrencyController.Instance.CanAfford(product.price);
}