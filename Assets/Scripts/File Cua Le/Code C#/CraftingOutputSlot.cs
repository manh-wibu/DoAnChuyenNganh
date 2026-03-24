using UnityEngine;

public class CraftingOutputSlot : MonoBehaviour
{
    [Header("=== OUTPUT ICON ===")]
    public SpriteRenderer icon;      // SpriteRenderer
    public SO_Item storedItem;

    private void Awake()
    {
        if (icon != null)
        {
            icon.enabled = true;                       // luôn hiển thị
            icon.sprite = null;                        // chưa có item → rỗng
            icon.color = new Color(1f, 1f, 1f, 0.3f); // mờ để phân biệt chưa có item
        }
    }

    // ================================
    // GÁN ITEM KẾT QUẢ CRAFT
    // ================================
    public void SetItem(SO_Item item)
    {
        storedItem = item;

        if (icon == null) return;

        if (item != null && item.icon != null)
        {
            icon.sprite = item.icon;
            icon.color = Color.white;   // hiện rõ icon
        }
        else
        {
            Clear(); // rỗng → mờ
        }
    }

    // ================================
    // XÓA SLOT
    // ================================
    public void Clear()
    {
        storedItem = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.color = new Color(1f, 1f, 1f, 0.3f); // mờ icon
        }
    }

    // ================================
    // HIỂN THỊ KHI CRAFT THÀNH CÔNG
    // ================================
    public void ShowResult(SO_Item craftedItem)
    {
        SetItem(craftedItem);
    }
}
