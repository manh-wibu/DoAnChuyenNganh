using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad; // nhập tên map cần load trong Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // chỉ nhân vật mới được chuyển map
        {
            Debug.Log("Player đã tới cuối map! Đang chuyển scene...");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

