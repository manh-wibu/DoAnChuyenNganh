using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Saus.CoreSystem;

public class PlayerRespawnManager : MonoBehaviour
{
    [Header("=== Cài đặt Respawn ===")]
    public string firstMapName = "LV1"; // Tên scene map đầu tiên
    public Vector3 spawnPositionInFirstMap = Vector3.zero; // Vị trí spawn chính xác trong LV1

    private GameObject playerRoot;
    private Stats stats;
    private PlayerHealth playerHealth;
    private bool isRespawning = false;

    private void Awake()
    {
        // Đảm bảo chỉ có 1 instance
        if (FindObjectsOfType<PlayerRespawnManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TryFindAndSetupPlayer();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UnsubscribeFromPlayerDeath();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[RespawnManager] Scene loaded: {scene.name}");

        // Tìm player bất kỳ trong scene mới
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            playerRoot = players[0];
            SetupPlayerReferences();
            Debug.Log("[RespawnManager] Player references setup for new scene.");
        }

        // Nếu là map 1, lưu vị trí spawn
        if (scene.name == firstMapName && spawnPositionInFirstMap == Vector3.zero && playerRoot != null)
        {
            spawnPositionInFirstMap = playerRoot.transform.position;
            Debug.Log($"[RespawnManager] Lưu spawn map 1: {spawnPositionInFirstMap}");
        }
    }

    private void TryFindAndSetupPlayer()
    {
        StartCoroutine(FindPlayerCoroutine());
    }

    private IEnumerator FindPlayerCoroutine()
    {
        float timer = 0f;
        while (timer < 3f)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var p in players)
            {
                if (p.scene.name == firstMapName || p.scene.name == SceneManager.GetActiveScene().name)
                {
                    playerRoot = p;
                    SetupPlayerReferences();
                    yield break;
                }
            }
            timer += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
        Debug.LogError("[RespawnManager] Không tìm thấy Player sau 3 giây!");
    }

    private void SetupPlayerReferences()
    {
        if (playerRoot == null) return;

        stats = playerRoot.GetComponentInChildren<Stats>();
        playerHealth = playerRoot.GetComponentInChildren<PlayerHealth>();

        if (stats != null)
        {
            UnsubscribeFromPlayerDeath();
            stats.Health.OnCurrentValueZero += OnPlayerDeath;
            Debug.Log("[RespawnManager] Đã hook sự kiện chết của player.");
        }
    }

    private void UnsubscribeFromPlayerDeath()
    {
        if (stats != null)
            stats.Health.OnCurrentValueZero -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        if (isRespawning) return;
        Debug.Log("[RespawnManager] Player chết! Reset toàn bộ inventory và respawn...");
        StartCoroutine(RespawnToFirstMapRoutine());
    }

    private IEnumerator RespawnToFirstMapRoutine()
{
    isRespawning = true;

    if (playerRoot != null)
        playerRoot.SetActive(false);

    yield return new WaitForSeconds(1f);

    // Xóa toàn bộ inventory
    InventoryManager.Instance?.ClearInventory();
    GoldManager.Instance?.ResetGold();

    // Reset vũ khí khi chết
    WeaponInventory.Instance?.ResetWeaponData();

    // Xóa checkpoint cũ → ẩn nút Continue
    PlayerPrefs.SetInt("HasCheckpoint", 0);
    PlayerPrefs.DeleteKey("CheckpointScene");
    PlayerPrefs.Save();

    // Load map đầu tiên
    SceneManager.LoadScene(firstMapName);

    yield return new WaitUntil(() => SceneManager.GetActiveScene().name == firstMapName);
    yield return new WaitForSeconds(0.1f);

    // Tìm lại player
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach (var p in players)
    {
        if (p.scene.name == firstMapName)
        {
            playerRoot = p;
            break;
        }
    }

    if (playerRoot == null)
    {
        Debug.LogError("Không tìm thấy player sau khi load LV1!");
        isRespawning = false;
        yield break;
    }

    // Đặt vị trí spawn
    playerRoot.transform.position = spawnPositionInFirstMap;

    // Reset HP
    stats = playerRoot.GetComponentInChildren<Stats>();
    if (stats != null)
        stats.Health.CurrentValue = stats.Health.MaxValue;

    // Bật lại player
    playerRoot.SetActive(true);

    var rb = playerRoot.GetComponentInChildren<Rigidbody2D>();
    if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;

    var sr = playerRoot.GetComponentInChildren<SpriteRenderer>();
    if (sr != null) sr.enabled = true;

    var movement = playerRoot.GetComponentInChildren<PlayerMovement>();
    if (movement != null) movement.enabled = true;

    // Stop particles nếu có
    foreach (var ps in playerRoot.GetComponentsInChildren<ParticleSystem>())
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

    SetupPlayerReferences();

    Debug.Log("[RespawnManager] Player đã respawn về LV1 và checkpoint cũ đã bị xóa!");
    isRespawning = false;
}

    // Test respawn bằng phím
    public void ForceRespawnPlayer()
    {
        StartCoroutine(RespawnToFirstMapRoutine());
    }
}
