using UnityEngine;
using UnityEngine.UI;
using Saus.CoreSystem.StatsSystem;
using Saus.CoreSystem;
public class BossHealthBar : MonoBehaviour
{
    public Stats bossStats;
    public Image fillBar;

    // 🔥 khung viền chứa thanh máu (Image)
    public Image flameBar;

    void Start()
    {
        if (bossStats == null)
            bossStats = GetComponentInParent<Stats>();

        bossStats.Health.OnValueChanged += OnHealthChanged;
        bossStats.Health.OnCurrentValueZero += OnBossDie;

        OnHealthChanged(bossStats.Health.CurrentValue, bossStats.Health.MaxValue);
    }

    /// <summary>
    /// Called whenever the boss's health changes. Updates the fill bar
    /// to reflect the current health value.
    /// </summary>
    /// <param name="current">The current health value.</param>
    /// <param name="max">The maximum health value.</param>
    private void OnHealthChanged(float current, float max)
    {
        if (fillBar != null)
            fillBar.fillAmount = current / max;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    private void OnBossDie()
    {
        // Ẩn toàn bộ khung HealthBar
        gameObject.SetActive(false);

        // Nếu flameBar không nằm trong gameObject này → ẩn riêng
        if (flameBar != null)
            flameBar.gameObject.SetActive(false);
    }
}
