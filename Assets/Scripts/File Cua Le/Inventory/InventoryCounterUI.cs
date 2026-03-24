using UnityEngine;
using TMPro;

public class InventoryCounterUI : MonoBehaviour
{
    [Header("ID vật phẩm cần đếm (trùng với SO_Item.itemID)")]
    public string targetItemID = "potion"; // ví dụ: “potion”, “key”, “gem”, …

    [Header("Text hiển thị số lượng vật phẩm")]
    public TextMeshProUGUI counterText;

    private void Start()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += UpdateCounter;
            UpdateCounter(); // cập nhật ban đầu
        }
        else
        {
            Debug.LogError("❌ Không tìm thấy InventoryManager trong scene!");
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= UpdateCounter;
        }
    }

    /// <summary>
    /// Cập nhật text mỗi khi thay đổi số lượng item.
    /// </summary>
    private void UpdateCounter()
    {
        if (InventoryManager.Instance == null || counterText == null) return;

        int count = InventoryManager.Instance.GetItemCountByID(targetItemID);
        counterText.text = count.ToString();
    }
}
