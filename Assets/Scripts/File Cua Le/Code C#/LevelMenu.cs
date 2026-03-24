using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
    {
        buttons[i].interactable = (i < unlockedLevel);
    }
    }
    public void OpenLevel(int levelId)
    {
        string levelName = "LV" + levelId;
        SceneManager.LoadScene(levelName);
    }
}
