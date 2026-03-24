/*using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UseItemPanel : MonoBehaviour
{
    public static UseItemPanel Instance;

    public TextMeshProUGUI itemNameText;
    public Button useButton;
    public Button closeButton;

    private string currentItemID;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        useButton.onClick.AddListener(UseSelectedItem);
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void Show(string itemID, string itemName)
    {
        currentItemID = itemID;
        itemNameText.text = itemName;
        gameObject.SetActive(true);
        Debug.Log($"✅ Hiển thị panel dùng vật phẩm: {itemName}");
    }

    private void UseSelectedItem()
    {
        if (string.IsNullOrEmpty(currentItemID))
        {
            Debug.LogWarning("⚠️ currentItemID trống — chưa chọn vật phẩm.");
            return;
        }

        bool used = InventoryManager.Instance.UseItem(currentItemID);
        if (used)
        {
            Debug.Log($"🧪 Đã dùng vật phẩm: {currentItemID}");
            gameObject.SetActive(false);
        }
    }
}*/
