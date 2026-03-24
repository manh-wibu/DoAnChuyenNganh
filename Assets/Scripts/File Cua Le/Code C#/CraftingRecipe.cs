using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Items/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Nguyên liệu (ID item)")]
    public List<string> inputItemIDs = new List<string>();

    [Header("Kết quả")]
    public SO_Item outputItem;

    [Header("Số lượng kết quả")]
    public int outputQuantity = 1;
}
