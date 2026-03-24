using UnityEngine;

public class EnemyRegister : MonoBehaviour
{
    public GameObject originalPrefab;

    private void Start()
    {
        if (EnemyManager.instance != null)
        {
            var entity = GetComponent<Entity>();
            if (entity != null)
            {
                //entity.SetInitialPosition(transform.position);
                EnemyManager.instance.RegisterEnemy(gameObject, originalPrefab);
                Debug.Log($"Đã đăng ký Enemy: {gameObject.name} với EnemyManager");
            }
            else
            {
                Debug.LogError($"Không tìm thấy Entity component trên {gameObject.name}!", this);
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy EnemyManager instance!", this);
        }
    }
}