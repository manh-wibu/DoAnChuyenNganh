using UnityEngine;
using Saus.Weapons;

public class WeaponInventoryGlobal : MonoBehaviour
{
    public static WeaponInventoryGlobal Instance;

    public WeaponDataSO[] weaponData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Tự khởi tạo 2 slot (Primary / Secondary)
        if (weaponData == null || weaponData.Length != 2)
            weaponData = new WeaponDataSO[2];
    }
}
