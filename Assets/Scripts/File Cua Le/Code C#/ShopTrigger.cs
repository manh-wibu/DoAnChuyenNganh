using UnityEngine;
using Saus;

public class ShopTrigger : MonoBehaviour
{
    [Header("=== Kéo OBJECT chứa ShopController vào đây ===")]
    [SerializeField] private ShopController shopController;

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player vào vùng shop – Bấm E để mở shop");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // TỰ ĐÓNG SHOP ĐÚNG CÁCH
            if (shopController != null && shopController.IsOpen)
            {
                Debug.Log("Rời vùng → Tự đóng shop");
                shopController.ToggleShop(); // chỉ đóng, vì đang mở
            }

            Debug.Log("Player rời vùng shop");
        }
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Ấn F = Toggle Shop");
            shopController?.ToggleShop();
        }
    }
}
