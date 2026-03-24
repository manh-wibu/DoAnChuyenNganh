using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Saus.Weapons
{
    public class WeaponDatabase : MonoBehaviour
    {
        public static WeaponDatabase Instance { get; private set; }

        [Header("Tất cả WeaponDataSO trong game")]
        public List<WeaponDataSO> allWeapons = new List<WeaponDataSO>();

        private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    // Kiểm tra trùng tên
    var duplicates = allWeapons
        .Where(w => w != null)
        .GroupBy(w => w.Name)
        .Where(g => g.Count() > 1)
        .ToList();

    if (duplicates.Count > 0)
    {
        foreach (var d in duplicates)
        {
            Debug.LogError($"WeaponDatabase: Weapon name bị TRÙNG: {d.Key}");
        }
    }
}
    }
}
