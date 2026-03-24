using UnityEngine;

namespace Saus
{
    public class CraftController : MonoBehaviour
    {
        [Header("Craft UI Content (UI chính trong Canvas)")]
        public GameObject craftContent;

        [Header("Animator + SpriteRenderer của NPC")]
        public Animator animator;
        public SpriteRenderer npcSprite;

        public bool IsOpen { get; private set; } = false;

        private void Start()
        {
            craftContent.SetActive(false);
            npcSprite.enabled = false;

            // Animator chạy bất kể timeScale
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;

            animator.SetBool("open", false);
            animator.SetBool("close", false);
        }

        // Gọi từ NPCTrigger khi bấm E
        public void ToggleCraft()
        {
            if (!IsOpen)
            {
                OpenCraft();
            }
            else
            {
                CloseCraft();
            }
        }

        private void OpenCraft()
        {
            Time.timeScale = 0;  // ⭐ Pause game

            npcSprite.enabled = true;
            animator.SetBool("open", true);
            animator.SetBool("close", false);

            IsOpen = true;
        }

        private void CloseCraft()
        {
            // Không trả timeScale ở đây
            // chỉ trả sau khi animation close chạy xong

            craftContent.SetActive(false);
            npcSprite.enabled = true;

            animator.SetBool("close", true);
            animator.SetBool("open", false);

            IsOpen = false;
        }

        // ======== EVENT CUỐI ANIMATION OPEN ========
        public void OnOpenAnimationFinished()
        {
            animator.SetBool("open", false);

            craftContent.SetActive(true);
            npcSprite.enabled = false;

            CraftingBook.Instance?.OpenBook();
        }

        // ======== EVENT CUỐI ANIMATION CLOSE ========
        public void OnCloseAnimationFinished()
        {
            animator.SetBool("close", false);

            npcSprite.enabled = false;

            CraftingBook.Instance?.CloseBook();

            Time.timeScale = 1;   // ⭐ Chỉ trả timeScale tại đây
        }
    }
}
