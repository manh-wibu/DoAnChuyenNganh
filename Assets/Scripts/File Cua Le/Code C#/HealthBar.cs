using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Saus.CoreSystem.StatsSystem;
using Saus.CoreSystem;

public class HealthBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);
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
        if (target != null)
        {
            targetStats = target.GetComponentInChildren<Stats>();

            if (targetStats == null)
            {
                Debug.LogWarning("⚠ HealthBar: Không tìm thấy Stats trong target!");
                return;
            }

            // Lắng nghe sự kiện thay đổi HP
            targetStats.Health.OnValueChanged += OnHealthChanged;

            // Cập nhật ngay lần đầu
            OnHealthChanged(targetStats.Health.CurrentValue, targetStats.Health.MaxValue);
        }
    }

    private void OnHealthChanged(float current, float max)
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
}
