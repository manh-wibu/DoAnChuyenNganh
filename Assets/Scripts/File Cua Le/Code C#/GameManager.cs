using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 lastCheckpointPos;
    private bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Không bị xóa khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("LV1");
    }
}