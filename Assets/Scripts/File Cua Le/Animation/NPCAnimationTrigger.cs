using UnityEngine;
using Saus;
public class NPCAnimationTrigger : MonoBehaviour
{
    [Header("=== Kéo OBJECT chứa CraftController vào đây ===")]
    [SerializeField] private CraftController craftController;

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player vào vùng NPC – Bấm E để mở Craft");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Tự đóng Craft nếu đang mở
            if (craftController != null && craftController.IsOpen)
            {
                Debug.Log("Player rời vùng → tự đóng Craft");
                craftController.ToggleCraft();
            }
        }
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Ấn F = Toggle Craft");
            craftController?.ToggleCraft();
        }
    }
}
