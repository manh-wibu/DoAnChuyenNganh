using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Saus.CoreSystem.StatsSystem;
using Saus.CoreSystem;

/// <summary>
/// PoiseBar - Hiển thị thanh Poise của player/enemy
/// 
/// Cơ chế Poise:
/// - Poise là khả năng chống lại knockback/stun
/// - Khi bị damage, Poise giảm (damage lượng poise tùy thuộc vào lực tấn công)
/// - Khi Poise = 0 → Break Guard (bị stun, không thể hành động)
/// - Poise sẽ tự hồi theo thời gian (poiseRecoveryRate)
/// - Mục đích: Khuyến khích player block/parry thay vì tank damage
/// </summary>
public class PoiseBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1.2f, 0); // Phía dưới health bar
    public Image fillBar;
    public TextMeshProUGUI valueText;

    private Stats targetStats;
    private RectTransform rectTransform;
    private Canvas parentCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        Debug.Log($"[PoiseBar] Start - Target: {target}, Canvas: {parentCanvas}, RectTransform: {rectTransform}");

        if (target == null)
        {
            Debug.LogError("⚠ PoiseBar: Target NOT ASSIGNED!");
            return;
        }

        // Tìm Stats từ children (Player) hoặc Core (Enemy)
        targetStats = target.GetComponentInChildren<Stats>();
        
        if (targetStats == null)
        {
            Core core = target.GetComponentInChildren<Core>();
            if (core != null)
                targetStats = core.GetCoreComponent<Stats>();
        }

        if (targetStats == null)
        {
            Debug.LogError("⚠ PoiseBar: Không tìm thấy Stats trong target!");
            return;
        }

        if (fillBar == null)
        {
            Debug.LogError("⚠ PoiseBar: Fill Bar NOT ASSIGNED!");
            return;
        }

        // Lắng nghe sự kiện thay đổi Poise
        targetStats.Poise.OnValueChanged += OnPoiseChanged;

        // Cập nhật ngay lần đầu
        OnPoiseChanged(targetStats.Poise.CurrentValue, targetStats.Poise.MaxValue);
        
        Debug.Log($"[PoiseBar] ✓ Initialized! Poise: {targetStats.Poise.CurrentValue}/{targetStats.Poise.MaxValue}");
    }

    void LateUpdate()
    {
        // Nếu Offset = (0,0,0) thì là UI cố định (merged vào Canvas)
        // Không cần update position mỗi frame
        if (offset == Vector3.zero)
            return;

        // Nếu offset khác (0,0,0) thì là UI follows world position
        if (target != null && parentCanvas != null && rectTransform != null)
        {
            Vector3 worldPos = target.position + offset;
            
            Camera cam = parentCanvas.worldCamera != null ? parentCanvas.worldCamera : Camera.main;
            if (cam != null)
            {
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
                rectTransform.position = screenPos;
            }
        }
    }

    private void OnPoiseChanged(float current, float max)
    {
        UpdateBar((int)current, (int)max);
    }

    public void UpdateBar(int currentValue, int maxValue)
    {
        if (fillBar != null)
            fillBar.fillAmount = Mathf.Clamp01((float)currentValue / maxValue);

        if (valueText != null)
            valueText.text = $"{currentValue} / {maxValue}";
    }

    private void OnDestroy()
    {
        if (targetStats != null)
            targetStats.Poise.OnValueChanged -= OnPoiseChanged;
    }
}
