using UnityEngine;

public class TaskWeapon : MonoBehaviour
{
    public static TaskWeapon Instance;

    [Header("Chỉ 1 ô Potion duy nhất")]
    public InventorySlot potionSlot; // Kéo đúng 1 cái InventorySlot vào đây (ô dưới màn hình)

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        InventoryManager.Instance.OnInventoryChanged += UpdateSingleSlot;
        UpdateSingleSlot(); // Update ngay
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance)
            InventoryManager.Instance.OnInventoryChanged -= UpdateSingleSlot;
    }

    public void UpdateSingleSlot()
    {
        if (potionSlot == null) return;

        SO_Item potionSmall = InventoryManager.Instance.GetItemByID("PotionSmall");
        SO_Item manaPotion = InventoryManager.Instance.GetItemByID("ManaPotion");

        int countSmall = InventoryManager.Instance.GetItemCountByID("PotionSmall");
        int countMana = InventoryManager.Instance.GetItemCountByID("ManaPotion");

        // Ưu tiên: PotionSmall trước → nếu có thì hiện nó
        if (countSmall > 0)
        {
            potionSlot.Setup(potionSmall, countSmall);
            return;
        }

        // Nếu không có PotionSmall → hiện Mana Potion
        if (countMana > 0)
        {
            potionSlot.Setup(manaPotion, countMana);
            return;
        }

        // Không có cái nào → xóa slot
        potionSlot.ClearSlot();
    }
}