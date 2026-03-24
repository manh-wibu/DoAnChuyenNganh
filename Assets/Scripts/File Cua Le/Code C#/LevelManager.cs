using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject selectLevelPanel;

    public GameObject map1;
    public GameObject map2;
    public GameObject map3;

    public void OnPlayButton()
    {
        selectLevelPanel.SetActive(true);
    }

    public void LoadLevel(int level)
    {
        // Tắt hết map
        map1.SetActive(false);
        map2.SetActive(false);
        map3.SetActive(false);

        // Bật map tương ứng
        switch (level)
        {
            case 1:
                map1.SetActive(true);
                break;
            case 2:
                map2.SetActive(true);
                break;
            case 3:
                map3.SetActive(true);
                break;
        }

        // Ẩn panel chọn level
        selectLevelPanel.SetActive(false);
    }
}
