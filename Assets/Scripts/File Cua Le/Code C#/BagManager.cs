using UnityEngine;

namespace Saus
{
    public class BagManager : MonoBehaviour
    {
        [Header("Bag UI Root (UI chính trong Canvas)")]
        [SerializeField] private GameObject bagContent;

        [Header("Animator + SpriteRenderer của Bag")]
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer bagSprite;

        private bool isOpen = false;

        private void Start()
        {
            // Ban đầu túi đóng hoàn toàn
            bagContent.SetActive(false);
            bagSprite.enabled = false;

            animator.SetBool("open", false);
            animator.SetBool("close", false);

            // Mặc định animator chạy scaled time
            animator.updateMode = AnimatorUpdateMode.Normal;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                ToggleBag();
            }
        }

        public void ToggleBag()
        {
            if (!isOpen)
            {
                OpenBag();
            }
            else
            {
                CloseBag();
            }

            isOpen = !isOpen;
        }

        private void OpenBag()
        {
            // Tắt UI trước
            bagContent.SetActive(false);

            // Hiện sprite để chạy animation
            bagSprite.enabled = true;

            // Animator chạy bằng thời gian không scale để animation không bị freeze
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;

            // Pause game
            Time.timeScale = 0f;

            // Animation mở
            animator.SetBool("open", true);
            animator.SetBool("close", false);
        }

        private void CloseBag()
        {
            // Tắt UI ngay lập tức
            bagContent.SetActive(false);

            // Hiện sprite túi để chạy animation đóng
            bagSprite.enabled = true;

            // Animator vẫn chạy UnscaledTime để animation đóng chạy bình thường
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;

            // Resume game sau khi animation đóng xong (gọi từ event)
            // Không set timeScale = 1 ở đây!

            // Animation đóng
            animator.SetBool("close", true);
            animator.SetBool("open", false);
        }

        // ====== GỌI TỪ EVENT Ở CUỐI ANIMATION OPEN ======
        public void OnOpenAnimationFinished()
        {
            animator.SetBool("open", false);

            // Bật UI túi sau khi mở xong
            bagContent.SetActive(true);

            // Ẩn sprite túi
            bagSprite.enabled = false;
        }

        // ====== GỌI TỪ EVENT Ở CUỐI ANIMATION CLOSE ======
        public void OnCloseAnimationFinished()
        {
            animator.SetBool("close", false);

            // Ẩn sprite túi
            bagSprite.enabled = false;

            // GAME CHỈ MỞ LẠI SAU KHI TÚI ĐÓNG XONG
            Time.timeScale = 1f;

            // Trả animator về chế độ bình thường
            animator.updateMode = AnimatorUpdateMode.Normal;
        }
    }
}
