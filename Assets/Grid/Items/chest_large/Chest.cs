using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public GameObject itemA;
    public GameObject itemB;
    public GameObject itemC;
    public float damageAmount = 10f;

    private bool playerInRange = false;
    private bool isOpened = false;

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            isOpened = true;

            if (animator != null)
            {
                animator.SetBool("open", true);
                Debug.Log("Đã kích hoạt animation Open");

                // ⏳ Bắt đầu theo dõi thời lượng animation
                StartCoroutine(WaitForAnimationToEnd());
            }
            else
            {
                Debug.LogError("Animator chưa được gán!");
            }
        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        // Lấy clip hiện tại đang chạy
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // Nếu clip chưa phát (chuyển từ Idle → Open), ta đợi một frame để cập nhật
        yield return null;
        info = animator.GetCurrentAnimatorStateInfo(0);

        // Đợi đúng thời lượng clip Open
        float duration = info.length;
        Debug.Log("Thời lượng animation Open: " + duration + " giây");
        yield return new WaitForSeconds(duration);

        // Sau khi animation chạy xong → Gọi OnChestOpened()
        OnChestOpened();
    }

    private void OnChestOpened()
    {
        Debug.Log("OnChestOpened được gọi - Rương sẽ bị ẩn và vật phẩm sẽ xuất hiện!");
        float random = Random.Range(0f, 100f);

        if (random < 40f)
        {
            if (itemA != null)
                Instantiate(itemA, transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
            else
                Debug.LogWarning("itemA chưa được gán!");
        }
        else if (random < 70f)
        {
            if (itemB != null)
                Instantiate(itemB, transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
            else
                Debug.LogWarning("itemB chưa được gán!");
        }
        else if (random < 90f)
        {
            if (itemC != null)
                Instantiate(itemC, transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
            else
                Debug.LogWarning("itemC chưa được gán!");
        }
        else
        {
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage((int)damageAmount);
                Debug.Log("Gây sát thương cho Player!");
            }
            else
            {
                Debug.LogWarning("Không tìm thấy PlayerHealth!");
            }
        }

        // Ẩn rương sau khi mở
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player trong vùng rương!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player rời vùng rương!");
        }
    }
}
