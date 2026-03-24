using Saus.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Saus.UI
{
    public class WeaponInfoUI : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private SpriteRenderer weaponIcon;

        private WeaponDataSO weaponData;

        public void PopulateUI(WeaponDataSO data)
        {
            if(data is null)
                return;

            weaponData = data;

            weaponIcon.sprite = weaponData.Icon;
        }
    }
}