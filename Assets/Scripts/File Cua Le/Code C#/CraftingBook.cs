using UnityEngine;

public class CraftingBook : MonoBehaviour
{
    public static CraftingBook Instance;

    [Header("UI Right Side (Input Slots)")]
    public CraftingSlot slotA;
    public CraftingSlot slotB;

    [Header("UI Output")]
    public GameObject resultPanel;
    public CraftingOutputSlot outputSlot;

    private void Awake()
    {
        Instance = this;

        if (resultPanel != null)
            resultPanel.SetActive(false);

        gameObject.SetActive(false);
    }

    public void OpenBook()
    {
        gameObject.SetActive(true);
        ClearRightSide();
        UpdatePreview();
    }

    public void CloseBook()
    {
        ClearRightSide();
        gameObject.SetActive(false);
    }

    void ClearRightSide()
    {
        slotA.Clear();
        slotB.Clear();
        if (outputSlot != null) outputSlot.Clear();
        if (resultPanel != null) resultPanel.SetActive(false);
    }

    public void OnSlotClicked(CraftingSlot clicked)
    {
        if (clicked.storedItem == null) return;

        if (clicked == slotA || clicked == slotB)
        {
            clicked.Clear();
        }
        else
        {
            if (slotA.storedItem == null) slotA.SetItem(clicked.storedItem);
            else if (slotB.storedItem == null) slotB.SetItem(clicked.storedItem);
        }

        UpdatePreview();
    }

    void UpdatePreview()
    {
        if (outputSlot != null) outputSlot.icon.enabled = true;

        if (slotA.storedItem == null || slotB.storedItem == null)
        {
            if (outputSlot != null) outputSlot.Clear();
            if (resultPanel != null) resultPanel.SetActive(false);
            return;
        }

        CraftingManager.Instance.TryPreviewResult();

        if (resultPanel != null)
            resultPanel.SetActive(outputSlot != null && outputSlot.storedItem != null);
    }

    public void TryCraft()
    {
        if (slotA.storedItem == null || slotB.storedItem == null) return;

        bool success = CraftingManager.Instance.CraftItem();

        if (success)
        {
            ClearRightSide();
        }
    }
}
