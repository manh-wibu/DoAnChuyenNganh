using UnityEngine;

public enum ItemType { Consumable, Key, Equipment, Material }

[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class SO_Item : ScriptableObject
{
    [Header("=== Shop ===")]
    public bool canSell = true;
    public int sellPrice = 5;

    [Header("=== Info ===")]
    public string itemID;              // BẮT BUỘC ĐẶT: PotionSmall → "PotionSmall", Mana → "Mana"
    public string itemName;
    public Sprite icon;
    public ItemType itemType = ItemType.Consumable;
    public bool stackable = true;
    public int maxStack = 99;

    [Header("=== Heal ===")]
    public bool isHealItem = false;
    public float healAmount = 0f;

    [TextArea] public string description;

    public virtual void Use(GameObject target)
    {
        if (target == null) return;

        // ĐÚNG NAMESPACE CỦA MÀY
        var stats = target.GetComponentInChildren<Saus.CoreSystem.Stats>();
        if (stats == null)
        {
            Debug.LogWarning("[SO_Item] Không tìm thấy Stats trên Player!");
            return;
        }

        if (itemID == "PotionSmall")
        {
            stats.Heal(20f);
            Debug.Log($"[Potion] Hồi 20 HP từ {itemName}");
            return;
        }

        if (itemID == "Mana")
        {
            var method = stats.GetType().GetMethod("HealMana");
            if (method != null)
                method.Invoke(stats, new object[] { 30f });
            else
                stats.Heal(15f);
            Debug.Log($"[Mana Potion] Hồi mana hoặc HP từ {itemName}");
            return;
        }

        if (isHealItem && healAmount > 0f)
        {
            stats.Heal(healAmount);
            Debug.Log($"[Item] Hồi {healAmount} HP từ {itemName}");
        }
    }
}