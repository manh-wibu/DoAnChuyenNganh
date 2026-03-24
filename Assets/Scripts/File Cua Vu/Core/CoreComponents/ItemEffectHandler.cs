using System.Collections;
using UnityEngine;
using Saus.CoreSystem.StatsSystem; // ⚠ import Stats

namespace Saus.CoreSystem
{
    /// <summary>
    /// Xử lý các hiệu ứng item (hồi máu, buff, v.v.) mà không sửa Stats
    /// </summary>
    public class ItemEffectHandler : MonoBehaviour
    {
        /// <summary>
        /// Hồi máu theo thời gian
        /// </summary>
        public void HealOverTime(Stats stats, float amount, float duration)
        {
            if (stats == null)
            {
                Debug.LogWarning("⚠ Không có Stats để hồi máu!");
                return;
            }

            // Nếu duration <= 0 → hồi ngay lập tức
            if (duration <= 0f)
            {
                HealInstant(stats, amount);
                return;
            }

            StartCoroutine(HealCoroutine(stats, amount, duration));
        }

        private IEnumerator HealCoroutine(Stats stats, float amount, float duration)
        {
            float elapsed = 0f;
            float rate = amount / duration;

            while (elapsed < duration)
            {
                stats.Heal(rate * Time.deltaTime); // dùng method Heal của Stats
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Hồi máu ngay lập tức
        /// </summary>
        public void HealInstant(Stats stats, float amount)
        {
            if (stats == null) return;
            stats.Heal(amount);
        }
    }
}
