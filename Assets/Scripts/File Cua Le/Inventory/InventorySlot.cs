using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Saus.CoreSystem;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    public SpriteRenderer background;
    public SpriteRenderer icon;
    public TextMeshPro countText;

    private SO_Item currentItem;
    private int currentCount = 0;

    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (background != null) background.enabled = true;
    }

    // ===============================
    // SETUP SLOT
    // ===============================
    public void Setup(SO_Item item, int count)
    {
        currentItem = item;
        currentCount = count;

        if (item != null && count > 0)
        {
            icon.enabled = true;
            icon.sprite = item.icon;
            icon.color = Color.white;

            countText.text = count > 1 ? count.ToString() : "";
            countText.gameObject.SetActive(true);
        }
        else
        {
            ClearSlot();
        }
    }

    // ===============================
    // CLEAR SLOT
    // ===============================
    public void ClearSlot()
    {
        currentItem = null;
        currentCount = 0;
        icon.enabled = false;
        countText.gameObject.SetActive(false);
    }

    // ===============================
    // CLICK SLOT
    // ===============================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PutToCrafting();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    private void PutToCrafting()
    {
        if (CraftingManager.Instance != null)
            CraftingManager.Instance.PutItemToCraftingSlot(currentItem);
    }

    // ===============================
    // USE ITEM
    // ===============================
    private void UseItem()
    {
        if (currentItem == null) return;

        var p = player ? player : GameObject.FindGameObjectWithTag("Player");
        if (!p) return;

        currentItem.Use(p);

        string currentMap = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        InventoryManager.Instance.RemoveItemByID(currentItem.itemID, 1, currentMap);

        Refresh(); // CHỈ GỌI 1 LẦN — KHÔNG TỰ TÍNH LẠI SAI
    }

    // ===============================
    // REFRESH SLOT (CHUẨN NHẤT)
    // ===============================
    public void Refresh()
    {
        if (currentItem == null)
        {
            ClearSlot();
            return;
        }

        // ✓ Crafting slot hiển thị tổng count từ tất cả map, không lọc theo map hiện tại
        int count = InventoryManager.Instance.GetItemCountByID(currentItem.itemID);

        if (count <= 0)
        {
            ClearSlot();
        }
        else
        {
            Setup(currentItem, count);
        }
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged += Refresh;
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
    }
}
