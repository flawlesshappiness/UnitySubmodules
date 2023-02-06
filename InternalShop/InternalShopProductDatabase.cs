using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(InternalShopProductDatabase), menuName = "Game/Internal Shop/" + nameof(InternalShopProductDatabase), order = 1)]
public class InternalShopProductDatabase : Database<InternalShopProduct>
{
}