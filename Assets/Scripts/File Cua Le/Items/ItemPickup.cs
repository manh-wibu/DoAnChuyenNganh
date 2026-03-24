using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Dữ liệu vật phẩm (ScriptableObject)")]
    public SO_Item itemData;

    private bool isPickedUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPickedUp) return;

        if (other.CompareTag("Player"))
        {
            if (itemData == null)
            {
                Debug.LogError($"❌ {gameObject.name} chưa được gán SO_Item!");
                return;
            }

            isPickedUp = true;
            GetComponent<Collider2D>().enabled = false;

            // ✅ Nếu là vàng → Cộng vào GoldManager thay vì Inventory
            if (itemData.itemID == "Gold")
            {
                GoldManager.Instance.AddGold(1); // hoặc itemData.healAmount nếu bạn muốn set số vàng trong SO_Item
                Destroy(gameObject);
                return;
            }

            // ✅ Còn lại thì thêm vào inventory như bình thường
            bool added = InventoryManager.Instance.AddItem(itemData, 1);
            if (added)
            {
                Debug.Log($"✅ Player nhặt {itemData.itemName} (+1)");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("⚠️ Túi đồ đầy hoặc lỗi khi thêm item!");
                isPickedUp = false;
                GetComponent<Collider2D>().enabled = true;
            }
        }
    }
}
