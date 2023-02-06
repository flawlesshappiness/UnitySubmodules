using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InternalShopProduct))]
public class InternalShopProductInspector : Editor
{
    private InternalShopProduct product;
    private void OnEnable()
    {
        product = target as InternalShopProduct;
    }
    public override void OnInspectorGUI()
    {
        GUIHelper.DrawDatabaseButtons<InternalShopProductDatabase, InternalShopProduct>(product);
        GUILayout.Space(20);
        base.OnInspectorGUI();
    }
}