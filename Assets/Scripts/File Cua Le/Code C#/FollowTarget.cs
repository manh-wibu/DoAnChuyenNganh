using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Object sẽ hiện/ẩn khi Player vào vùng")]
    public GameObject target;

    [Header("Khoảng cách phát hiện Player")]
    public float detectRadius = 5f;

    [Header("Layer của Player")]
    public LayerMask playerLayer;

    private bool playerActivated = false; // Player đã kích hoạt chưa

    void Start()
    {
        // Ẩn target lúc bắt đầu
        if (target != null)
            target.SetActive(false);
    }

    void Update()
    {
        // Nếu đã kích hoạt trước đó thì không cần detect nữa
        if (!playerActivated)
        {
            DetectPlayer();
        }
    }

    void DetectPlayer()
    {
        // Tìm Player trong vùng
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, playerLayer);

        if (hit != null)
        {
            // Player vào vùng → bật target VÀ không tắt nữa
            playerActivated = true;
            target.SetActive(true);
        }
    }

    // Khi object này bị destroy → tắt target
    private void OnDestroy()
    {
        if (target != null)
            target.SetActive(false);
    }
}
