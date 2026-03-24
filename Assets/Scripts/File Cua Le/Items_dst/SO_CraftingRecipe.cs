using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class SO_CraftingRecipe : ScriptableObject
{
    [System.Serializable]
    public class ItemRequirement
    {
        public string itemID;
        public int amount = 1;
    }

    public List<ItemRequirement> inputItems = new List<ItemRequirement>();

    public string outputItemID;
    public int outputAmount = 1;
}
