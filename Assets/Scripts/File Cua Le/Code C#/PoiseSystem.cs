using UnityEngine;
using Saus.CoreSystem;

/// <summary>
/// PoiseSystem - Quản lý cơ chế Poise Break Guard
/// 
/// Khi Poise = 0:
/// - Player/Enemy bị Break Guard (bị stun, không thể hành động)
/// - Hiển thị hiệu ứng visual (flash, particle)
/// - Cho enemy: chuyển vào Stun State
/// - Cho player: trigger animation bị knock back
/// </summary>
public class PoiseSystem : MonoBehaviour
{
    private Stats stats;
    private float lastPoiseCheckValue = -1f;

    void Start()
    {
        // Tìm Stats
        stats = GetComponentInChildren<Stats>();
        if (stats == null)
        {
            Core core = GetComponentInChildren<Core>();
            if (core != null)
                stats = core.GetCoreComponent<Stats>();
        }

        if (stats != null)
        {
            lastPoiseCheckValue = stats.Poise.CurrentValue;
            stats.Poise.OnValueChanged += OnPoiseChanged;
        }
    }

    private void OnPoiseChanged(float current, float max)
    {
        // Nếu poise vừa về 0
        if (current <= 0 && lastPoiseCheckValue > 0)
        {
            OnBreakGuard();
        }
        // Nếu poise vừa được hồi lại từ 0
        else if (current > 0 && lastPoiseCheckValue <= 0)
        {
            OnRecoverFromBreak();
        }

        lastPoiseCheckValue = current;
    }

    /// <summary>
    /// Gọi khi Break Guard (Poise = 0)
    /// </summary>
    private void OnBreakGuard()
    {
        Debug.Log($"[PoiseSystem] {gameObject.name} - BREAK GUARD! Poise = 0");

        // Hiệu ứng visual
        PlayBreakGuardEffect();

        // Nếu là enemy, chuyển vào Stun State
        Entity entity = GetComponent<Entity>();
        if (entity != null)
        {
            // Xử lý stun (depend on enemy's state machine)
            Debug.Log($"[PoiseSystem] Enemy stunned!");
        }

        // Nếu là player, có thể trigger hiệu ứng knock back animation
        // (implement tùy theo player controller)
    }

    /// <summary>
    /// Gọi khi hồi lại từ Break Guard
    /// </summary>
    private void OnRecoverFromBreak()
    {
        Debug.Log($"[PoiseSystem] {gameObject.name} - Recovered from Break Guard");

        // Clear hiệu ứng stun
        PlayRecoverEffect();
    }

    /// <summary>
    /// Hiệu ứng khi Break Guard
    /// </summary>
    private void PlayBreakGuardEffect()
    {
        // Hiệu ứng chớp tắt (bên ngoài có thể tạo separate flash effect)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            StartCoroutine(FlashEffect(sr, 0.1f, 4)); // Flash 4 lần, 0.1s mỗi lần
        }

        // Particle effect (optional)
        // ParticleSystem breakEffect = GetComponentInChildren<ParticleSystem>();
        // if (breakEffect != null) breakEffect.Play();
    }

    /// <summary>
    /// Hiệu ứng hồi lại
    /// </summary>
    private void PlayRecoverEffect()
    {
        // Có thể tạo hiệu ứng khôi phục (glow effect, etc)
    }

    /// <summary>
    /// Coroutine để tạo hiệu ứng flash
    /// </summary>
    private System.Collections.IEnumerator FlashEffect(SpriteRenderer sr, float duration, int flashCount)
    {
        Color originalColor = sr.color;
        Color flashColor = new Color(1f, 1f, 1f, 0.5f);

        for (int i = 0; i < flashCount; i++)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(duration);
            sr.color = originalColor;
            yield return new WaitForSeconds(duration);
        }

        sr.color = originalColor;
    }

    /// <summary>
    /// Hàm public để apply poise damage từ attack
    /// Được gọi từ DamageReceiver khi nhận damage
    /// </summary>
    public void TakePoiseDamage(float poiseDamage)
    {
        if (stats != null && stats.Poise.CurrentValue > 0)
        {
            stats.Poise.Decrease(poiseDamage);
            Debug.Log($"[PoiseSystem] Poise damaged by {poiseDamage}. Current: {stats.Poise.CurrentValue}");
        }
    }

    private void OnDestroy()
    {
        if (stats != null)
            stats.Poise.OnValueChanged -= OnPoiseChanged;
    }
}
