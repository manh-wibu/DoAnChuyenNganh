using UnityEngine;

namespace Saus
{
    public class Task_Effect : MonoBehaviour
    {
        [Header("=== HIỆU ỨNG KHI NHẶT ITEM ===")]
        [SerializeField] private GameObject effectObject;

        [Header("Object cần kiểm tra Sprite")]
        [SerializeField] private GameObject targetObject;

        [Header("Sprite cần kiểm tra để hiện hiệu ứng")]
        [SerializeField] private Sprite targetSprite;

        private bool effectActive = false;

        private void Awake()
        {
            if (effectObject != null)
                effectObject.SetActive(false);
        }

        private void Update()
        {
            if (targetObject == null || effectObject == null || targetSprite == null)
                return;

            SpriteRenderer sr = targetObject.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == targetSprite)
            {
                if (!effectActive)
                {
                    effectObject.SetActive(true);
                    effectActive = true;
                }
            }
            else
            {
                if (effectActive)
                {
                    effectObject.SetActive(false);
                    effectActive = false;
                }
            }
        }
    }
}
