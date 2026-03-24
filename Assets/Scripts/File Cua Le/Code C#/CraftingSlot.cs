using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer icon;
    public SO_Item storedItem;

    public void SetItem(SO_Item item)
    {
        storedItem = item;

        if (icon == null) return;

        if (item != null && item.icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;       // bắt buộc bật
            icon.color = Color.white;  // tránh trong suốt
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public void Clear()
    {
        storedItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CraftingBook.Instance != null)
            CraftingBook.Instance.OnSlotClicked(this);
    }
}
