using UnityEngine;
using Saus.CoreSystem;

[CreateAssetMenu(fileName = "NewArmorPotion", menuName = "Items/Armor Boost Potion")]
public class SO_Item_ArmorBoost : SO_Item
{
    [Header("Armor Boost Info")]
    public float duration = 10f;

    /// <summary>
    /// Dùng item Armor Boost: giảm 30% damage trong <see cref="duration"/> giây.
    /// </summary>
    /// <param name="player">Player sử dụng item</param>
    public override void Use(GameObject player)
    {
        if (player == null)
        {
            Debug.LogWarning("⚠ Player null khi dùng Armor Boost!");
            return;
        }

        // Lấy ArmorBuffHandler từ Player
        //ArmorBuffHandler armorHandler = player.GetComponentInChildren<ArmorBuffHandler>();
        //if (armorHandler == null)
        //{
        //    Debug.LogWarning("⚠ Không tìm thấy ArmorBuffHandler trên Player!");
        //    return;
        //}

        //// Bắt đầu Armor Boost, cộng dồn thời gian
        //armorHandler.StartArmorBoost(duration);

        //Debug.Log($"🛡 {player.name} dùng Armor Buff: -30% damage trong {duration}s.");
    }
}
