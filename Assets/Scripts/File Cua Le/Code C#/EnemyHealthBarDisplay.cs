using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Saus.CoreSystem;
using Saus.CoreSystem.StatsSystem;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                 // Enemy transform
    public Vector3 offset = new Vector3(0, 0.5f, 0);

    [Header("UI")]
    public Image fillBar;
    public TextMeshProUGUI valueText;

    private Stats enemyStats;
    private RectTransform rectTransform;
    private Canvas parentCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("⚠ EnemyHealthBar: Target chưa được gán!");
            return;
        }

        enemyStats = target.GetComponentInChildren<Stats>();

        if (enemyStats == null)
        {
            Debug.LogWarning("⚠ EnemyHealthBar: Không tìm thấy Stats trong Enemy!");
            return;
        }

        // Lắng nghe khi máu enemy thay đổi
        enemyStats.Health.OnValueChanged += OnHealthChanged;

        // Update ngay lúc start
        UpdateBar(
            (int)enemyStats.Health.CurrentValue,
            (int)enemyStats.Health.MaxValue
        );
    }

    private void LateUpdate()
    {
        if (target == null || parentCanvas == null) return;

        // Follow enemy position
        Vector3 worldPos = target.position + offset;

        if (parentCanvas.renderMode == RenderMode.WorldSpace)
        {
            rectTransform.position = worldPos;
        }
        else
        {
            rectTransform.position = Camera.main.WorldToScreenPoint(worldPos);
        }
    }

    private void OnHealthChanged(float current, float max)
    {
        // Update health bar
        UpdateBar((int)current, (int)max);
        
        // 🔥 Enemy chết → fade out health bar (không ẩn ngay)
        if (current <= 0)
        {
            StartCoroutine(FadeOutHealthBar());
        }
    }

    private System.Collections.IEnumerator FadeOutHealthBar()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        float fadeDuration = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
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
        if (enemyStats != null)
            enemyStats.Health.OnValueChanged -= OnHealthChanged;
    }
}
