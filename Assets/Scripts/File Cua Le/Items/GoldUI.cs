using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Start()
    {
        UpdateGoldUI(GoldManager.Instance.GetGold());
        GoldManager.Instance.OnGoldChanged += UpdateGoldUI;
    }

    private void OnDestroy()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= UpdateGoldUI;
    }

    private void UpdateGoldUI(int gold)
    {
        goldText.text = gold.ToString();
    }
}
