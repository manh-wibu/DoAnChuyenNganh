using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saus.Weapons;
using Saus.CoreSystem;
using Saus.Interaction.Interactables; // <- Thêm dòng này

public class ChestWeapon : MonoBehaviour
{
    public Animator animator;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    public GameObject item6;
    public GameObject item7;

    public WeaponInventory playerInventory;

    private bool playerInRange = false;
    private bool isOpened = false;

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            isOpened = true;

            if (animator != null)
            {
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                animator.SetBool("open", true);
                StartCoroutine(WaitForOpenAnimation());
            }
            else
            {
                Debug.LogError("Animator chưa được gán!");
            }
        }
    }

    private IEnumerator WaitForOpenAnimation()
    {
        yield return null;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float duration = info.length;
        yield return new WaitForSecondsRealtime(duration);

        SpawnRandomItem();
    }

    private void SpawnRandomItem()
    {
        GameObject[] allItems = new GameObject[] { item1, item2, item3, item4, item5, item6, item7 };
        var availableItems = new List<GameObject>();

        foreach (var item in allItems)
        {
            if (item == null) continue;

            WeaponPickup wp = item.GetComponent<WeaponPickup>();
            if (wp == null)
            {
                availableItems.Add(item);
                continue;
            }

            WeaponDataSO itemData = wp.GetContext();
            bool hasWeapon = false;

            for (int i = 0; i < playerInventory.weaponData.Length; i++)
            {
                if (playerInventory.weaponData[i] == itemData)
                {
                    hasWeapon = true;
                    break;
                }
            }

            if (!hasWeapon)
                availableItems.Add(item);
        }

        if (availableItems.Count == 0)
        {
            Debug.Log("Người chơi đã có tất cả các vũ khí, không spawn item nào.");
        }
        else
        {
            int index = Random.Range(0, availableItems.Count);
            GameObject selected = availableItems[index];

            Instantiate(selected, transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
