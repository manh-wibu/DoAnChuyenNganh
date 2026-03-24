using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameObject[] maps; // Mảng chứa Grid, Grid(1), Grid(2)...

    void Start()
    {
        int level = PlayerPrefs.GetInt("SelectedLevel", 1); // Lấy level, mặc định 1

        // Tắt hết các map
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }

        // Bật đúng map nếu hợp lệ
        if (level >= 1 && level <= maps.Length)
        {
            maps[level - 1].SetActive(true); // Bật map tương ứng
        }
    }
}
