using UnityEngine;
using TMPro;
using System.Collections;
using Saus.Interaction.Interactables;
using Saus.Weapons;
using Saus.CoreSystem;

public class ShopWeaponSlot : MonoBehaviour
{
    [Header("UI Components")]
    public SpriteRenderer iconRenderer;
    public TMP_Text priceText;
    public SpriteRenderer coinRenderer;

    [Header("Preset for Shop Display")]
    public GameObject presetItem;       // Prefab vũ khí (có script nhặt đồ bất kỳ)
    public int presetPrice;
    public Sprite coinSprite;

    [Header("Inventory & Swap Reference")]
    [SerializeField] private WeaponInventory weaponInventory; // Inventory của player
    [SerializeField] private WeaponSwap weaponSwap;           // Reference tới WeaponSwap

    private GameObject itemPrefab;
    private int price;

    private void Start()
    {
        if (presetItem != null)
            Setup(presetItem, presetPrice);
    }

    public void Setup(GameObject newItemPrefab, int itemPrice)
    {
        itemPrefab = newItemPrefab;
        price = itemPrice;

        // Tự động lấy icon từ SpriteRenderer đầu tiên trong prefab
        Sprite icon = null;
        if (itemPrefab != null)
        {
            var sr = itemPrefab.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) icon = sr.sprite;
        }

        if (iconRenderer != null && icon != null)
            iconRenderer.sprite = icon;

        if (priceText != null)
            priceText.text = price.ToString();

        if (coinSprite != null && coinRenderer != null)
        {
            coinRenderer.sprite = coinSprite;
            coinRenderer.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        BuyItem();
    }

    private void BuyItem()
    {
        if (itemPrefab == null)
        {
            Debug.LogError("Shop: Chưa gán prefab vũ khí!");
            return;
        }

        if (GoldManager.Instance.GetGold() < price)
        {
            Debug.Log("Không đủ vàng để mua!");
            return;
        }

        GoldManager.Instance.SpendGold(price);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Không tìm thấy Player (cần tag 'Player')");
            return;
        }

        // Spawn gần player + rơi đẹp
        Vector3 spawnPos = player.transform.position + new Vector3(
            Random.Range(-1.5f, 1.5f),
            Random.Range(1.2f, 2f),
            0
        );

        GameObject dropped = Instantiate(itemPrefab, spawnPos, Quaternion.identity);

        // Auto pickup sau 0.5 giây (có hiệu ứng rơi)
        StartCoroutine(AutoPickupAfterDelay(dropped));
    }

    private IEnumerator AutoPickupAfterDelay(GameObject droppedObj)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        if (droppedObj == null) yield break;

        WeaponPickup wp = droppedObj.GetComponent<WeaponPickup>();
        if (wp != null)
        {
            if (weaponSwap != null)
            {
                weaponSwap.SendMessage("HandleTryInteract", wp, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                WeaponDataSO weaponData = wp.GetContext();
                if (weaponInventory.TryGetEmptyIndex(out int index))
                {
                    weaponInventory.TrySetWeapon(weaponData, index, out _);
                    wp.Interact();
                }
            }

            Debug.Log($"ĐÃ MUA & NHẶT TỰ ĐỘNG: {itemPrefab.name}");
        }
        else
        {
            Debug.LogWarning($"Spawn thành công nhưng không tìm thấy WeaponPickup: {itemPrefab.name}");
        }
    }
}
