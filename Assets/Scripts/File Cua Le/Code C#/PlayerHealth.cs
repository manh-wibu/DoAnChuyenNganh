using Saus.CoreSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Vector3 checkpointPos;
    [SerializeField] private int maxHealth = 100;

    [Header("UI")]
    public HealthBar healthBar;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Stats stats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        stats = GetComponentInChildren<Stats>();
    }

    private void Start()
    {
        LoadCheckpoint();
        // 🆕 Load health từ PlayerPrefs nếu có
        LoadHealth();
        UpdateHealthUI();
    }

    // 🆕 Load health from PlayerPrefs
    private void LoadHealth()
    {
        if (PlayerPrefs.HasKey("PlayerHealth") && stats != null)
        {
            int savedHealth = PlayerPrefs.GetInt("PlayerHealth", (int)stats.Health.MaxValue);
            stats.Health.CurrentValue = savedHealth;
            Debug.Log($"[PlayerHealth] Loaded health: {savedHealth}");
        }
    }

    // 🆕 Save health to PlayerPrefs
    public void SaveHealth()
    {
        if (stats != null)
        {
            int currentHealth = (int)stats.Health.CurrentValue;
            PlayerPrefs.SetInt("PlayerHealth", currentHealth);
            PlayerPrefs.Save();
            Debug.Log($"[PlayerHealth] Saved health: {currentHealth}");
        }
    }

    private void LoadCheckpoint()
    {
        string nextScene = PlayerPrefs.GetString("NextScene", SceneManager.GetActiveScene().name);
        string checkpointScene = PlayerPrefs.GetString("CheckpointScene", "");

        if (!string.IsNullOrEmpty(checkpointScene) && checkpointScene == nextScene &&
            PlayerPrefs.HasKey("CheckpointX") &&
            PlayerPrefs.HasKey("CheckpointY") &&
            PlayerPrefs.HasKey("CheckpointZ"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            checkpointPos = new Vector3(x, y, z);
            transform.position = checkpointPos;
            Debug.Log("Loaded checkpoint for map: " + checkpointScene);
        }
        else
        {
            checkpointPos = transform.position;
            Debug.Log("No checkpoint for this map. Spawn at default position: " + checkpointPos);
        }
    }

    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        checkpointPos = newCheckpoint;
        PlayerPrefs.SetFloat("CheckpointX", checkpointPos.x);
        PlayerPrefs.SetFloat("CheckpointY", checkpointPos.y);
        PlayerPrefs.SetFloat("CheckpointZ", checkpointPos.z);
        PlayerPrefs.SetString("CheckpointScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("HasCheckpoint", 1);
        PlayerPrefs.Save();
        Debug.Log("Checkpoint updated and saved at: " + checkpointPos);
    }

    public void RespawnAtCheckpoint()
    {
        transform.position = checkpointPos;
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
        if (sr != null) sr.enabled = true;

        UpdateHealthUI();
        Debug.Log("Player respawned at checkpoint: " + checkpointPos);
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null && stats != null)
            healthBar.UpdateBar((int)stats.Health.CurrentValue, (int)stats.Health.MaxValue);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || stats == null) return;
        stats.Health.CurrentValue -= damage;
        UpdateHealthUI();
    }

    // 🆕 Get current health value
    public float GetCurrentHealth()
    {
        return stats != null ? stats.Health.CurrentValue : 0f;
    }
}
