using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;

    void Start()
    {
        HandleFullReset();
        StartCoroutine(LoadSceneAsync());
    }

        private void HandleFullReset()
    {
        if (PlayerPrefs.GetInt("FullReset", 0) == 1)
        {
            string nextScene = PlayerPrefs.GetString("NextScene", "LV1");

            // Xóa chỉ các dữ liệu game cũ không liên quan đến player/respawn map mới
            PlayerPrefs.DeleteKey("SomeOldKey"); // ví dụ checkpoint map cũ
            // Không DeleteAll()

            // Lưu lại các key cần thiết
            PlayerPrefs.SetString("NextScene", nextScene);
            PlayerPrefs.SetInt("FullReset", 0); 
            PlayerPrefs.Save();
        }
}

    IEnumerator LoadSceneAsync()
    {
        string sceneToLoad = PlayerPrefs.GetString("NextScene", "LV1");
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
        op.allowSceneActivation = false;

        float fakeProgress = 0f;

        while (!op.isDone)
        {
            op.allowSceneActivation = true;
            float realProgress = Mathf.Clamp01(op.progress / 0.9f);
            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, Time.deltaTime * 1.5f);
            progressBar.value = fakeProgress;

            if (fakeProgress >= 0.99f)
                break;

            yield return null;
        }
    }
}
