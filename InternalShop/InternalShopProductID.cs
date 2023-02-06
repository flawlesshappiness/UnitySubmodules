[System.Serializable]
public partial class InternalShopProductID : FakeEnum
{
	public static readonly InternalShopProductID Default = new InternalShopProductID("default");
	public InternalShopProductID(string id) : base(id) { }
}