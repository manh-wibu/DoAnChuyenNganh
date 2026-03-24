using UnityEngine;
using System.Collections.Generic;

public class PotionQuickSlot : MonoBehaviour
{
    public static PotionQuickSlot Instance;

    private InventorySlot displaySlot;

    // Danh sách item ID theo thứ tự người chơi nhặt
    private List<string> pickedOrder = new List<string>();

    private int currentIndex = 0;

    // Chỉ định các ID potion hợp lệ
    private readonly string[] potionIDs = new string[]
    {
        "PotionSmall",
        "Mana",
        "PotionBig",
        "PotionRegen"
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        FindDisplaySlot();
        SubscribeToInventory();
        RebuildPickedOrder();
        UpdateDisplay();
    }

    private void FindDisplaySlot()
    {
        var taskWeapon = GameObject.Find("TaskWeapon");
        if (taskWeapon == null) return;

        Transform slot = taskWeapon.transform.Find("Potion/Slot");
        if (slot != null)
            displaySlot = slot.GetComponent<InventorySlot>();
    }

    private void SubscribeToInventory()
    {
        var inv = InventoryManager.Instance;
        if (inv != null)
            inv.OnInventoryChanged += OnInventoryChanged;
    }

    // ============================
    // Khi inventory đổi → cập nhật thứ tự
    // ============================
    private void OnInventoryChanged()
    {
        RebuildPickedOrder();
        UpdateDisplay();
    }

    private void RebuildPickedOrder()
    {
        var inv = InventoryManager.Instance;
        if (inv == null) return;

        // Xóa các potion đã hết
        pickedOrder.RemoveAll(id => inv.GetItemCountByID(id) <= 0);

        // Tìm xem có potion mới không
        foreach (string id in potionIDs)
        {
            if (inv.GetItemCountByID(id) > 0 && !pickedOrder.Contains(id))
            {
                pickedOrder.Add(id);  // nhặt cái nào → thêm cái đó vào cuối
            }
        }

        // Nếu index hiện tại vượt quá list → chỉnh lại
        if (currentIndex >= pickedOrder.Count)
            currentIndex = Mathf.Max(0, pickedOrder.Count - 1);
    }

    // ============================
    // HIỂN THỊ ITEM
    // ============================
    public void UpdateDisplay()
    {
        if (displaySlot == null) return;

        var inv = InventoryManager.Instance;
        if (inv == null || pickedOrder.Count == 0)
        {
            displaySlot.ClearSlot();
            return;
        }

        string id = pickedOrder[currentIndex];
        int count = inv.GetItemCountByID(id);
        SO_Item item = inv.GetItemByID(id);

        if (count > 0 && item != null)
        {
            displaySlot.Setup(item, count);
        }
        else
        {
            displaySlot.ClearSlot();
        }
    }

    private void Update()
    {
        if (pickedOrder.Count == 0) return;

        // Nhấn C dùng potion hiện tại
        if (Input.GetKeyDown(KeyCode.C))
            UseCurrentPotion();

        // Lăn chuột đổi item
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0.1f) ChangePotion(-1);
        else if (scroll < -0.1f) ChangePotion(+1);
    }

    private void ChangePotion(int direction)
    {
        if (pickedOrder.Count == 0) return;

        currentIndex += direction;

        if (currentIndex < 0)
            currentIndex = pickedOrder.Count - 1;
        else if (currentIndex >= pickedOrder.Count)
            currentIndex = 0;

        UpdateDisplay();
    }

    private void UseCurrentPotion()
    {
        var inv = InventoryManager.Instance;
        if (inv == null || pickedOrder.Count == 0) return;

        string id = pickedOrder[currentIndex];
        SO_Item item = inv.GetItemByID(id);
        if (item == null) return;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            item.Use(player);
            inv.RemoveItemByID(id, 1);
            OnInventoryChanged(); // cập nhật danh sách potion
        }
    }

    private void OnDestroy()
    {
        var inv = InventoryManager.Instance;
        if (inv != null)
            inv.OnInventoryChanged -= OnInventoryChanged;
    }
}
