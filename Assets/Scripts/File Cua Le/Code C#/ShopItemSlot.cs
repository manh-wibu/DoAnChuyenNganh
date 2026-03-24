using UnityEngine;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI Components")]
    public SpriteRenderer iconRenderer;
    public TMP_Text priceText;
    public SpriteRenderer coinRenderer;   // 🆕 dùng SpriteRenderer thay Image

    [Header("Preset for Shop Display")]
    public SO_Item presetItem;
    public int presetPrice;
    public Sprite coinSprite;

    private SO_Item item;
    private int price;

    private void Start()
    {
        if (presetItem != null)
        {
            Setup(presetItem, presetPrice);
        }
    }

    public void Setup(SO_Item newItem, int itemPrice)
    {
        item = newItem;
        price = itemPrice;

        // Icon
        if (iconRenderer != null)
            iconRenderer.sprite = item.icon;

        // Price
        priceText.text = price.ToString();

        // Coin sprite
        if (coinSprite != null && coinRenderer != null)
        {
            coinRenderer.sprite = coinSprite;
            coinRenderer.enabled = true;
        }
    }

    // ======================
    // 🔥 ẤN VÀO LÀ MUA NGAY
    // ======================
    private void OnMouseDown()
    {
        BuyItem();
    }

    private void BuyItem()
    {
        if (GoldManager.Instance.GetGold() >= price)
        {
            GoldManager.Instance.SpendGold(price);

            bool added = InventoryManager.Instance.AddItem(item, 1);

            if (added)
                Debug.Log($"🛒 Mua {item.itemName} thành công và đã thêm vào túi đồ!");
            else
                Debug.Log($"⚠️ Mua {item.itemName} thành công nhưng inventory đầy!");
        }
        else
        {
            Debug.Log("❌ Không đủ vàng để mua!");
        }
    }
}
