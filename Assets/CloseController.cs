using UnityEngine;

namespace Saus
{
    public class ShopController : MonoBehaviour
    {
        [Header("Shop Item Slot Root (tất cả slot nằm ngoài canvas)")]
        public GameObject shopContent;

        [Header("Animator + SpriteRenderer")]
        public Animator animator;
        public SpriteRenderer shopSprite;

        [Header("Objects sẽ đóng shop khi cả 3 xuất hiện")]
        [SerializeField] private GameObject blockObject1;
        [SerializeField] private GameObject blockObject2;
        [SerializeField] private GameObject blockObject3;

        private Animator[] shopAnimators;

        public bool IsOpen { get; private set; } = false;

        private void Start()
        {
            shopContent.SetActive(false);
            shopSprite.enabled = false;

            // Animation của cửa hàng không bị ảnh hưởng bởi timeScale
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;

            // Tìm tất cả Animator bên trong shopContent
            shopAnimators = shopContent.GetComponentsInChildren<Animator>(true);

            // Đặt toàn bộ animator UI trong shopContent chạy theo UnscaledTime
            foreach (var anim in shopAnimators)
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;

            animator.SetBool("open", false);
            animator.SetBool("close", false);
        }

        private void Update()
        {
            if (IsOpen && blockObject1 != null && blockObject2 != null && blockObject3 != null)
            {
                if (blockObject1.activeSelf && blockObject2.activeSelf && blockObject3.activeSelf)
                {
                    CloseShopImmediate();
                }
            }
        }

        public void ToggleShop()
        {
            if (!IsOpen)
                OpenShop();
            else
                CloseShop();
        }

        private void OpenShop()
        {
            Time.timeScale = 0;

            shopSprite.enabled = true;
            animator.SetBool("open", true);
            animator.SetBool("close", false);

            IsOpen = true;
        }

        private void CloseShop()
        {
            Time.timeScale = 1;

            shopContent.SetActive(false);
            shopSprite.enabled = true;

            animator.SetBool("close", true);
            animator.SetBool("open", false);

            IsOpen = false;
        }

        private void CloseShopImmediate()
        {

            animator.SetBool("open", false);
            animator.SetBool("close", false);

            shopContent.SetActive(false);
            shopSprite.enabled = false;

            IsOpen = false;
        }

        public void OnOpenAnimationFinished()
        {
            animator.SetBool("open", false);
            shopContent.SetActive(true);
            shopSprite.enabled = false;
        }

        public void OnCloseAnimationFinished()
        {
            animator.SetBool("close", false);
            shopSprite.enabled = false;
            Time.timeScale = 1;
        }
    }
}
