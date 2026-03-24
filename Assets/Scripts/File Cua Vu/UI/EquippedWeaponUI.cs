using System;
using Saus.CoreSystem;
using Saus.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Saus.UI
{
    public class EquippedWeaponWorldIcon : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer iconRenderer; 
        [SerializeField] private CombatInputs input;         
        [SerializeField] private WeaponInventory weaponInventory;

        private WeaponDataSO currentWeapon;

        private void Start()
        {
            RefreshIcon();
        }

        private void OnEnable()
        {
            weaponInventory.OnWeaponDataChanged += OnWeaponChanged;
            // Cập nhật icon ngay lập tức khi component enable (khi qua scene mới)
            RefreshIcon();
        }

        private void OnDisable()
        {
            weaponInventory.OnWeaponDataChanged -= OnWeaponChanged;
        }

        private void OnWeaponChanged(int inputIndex, WeaponDataSO data)
        {
            if (inputIndex != (int)input) return;

            currentWeapon = data;
            RefreshIcon();
        }

        private void RefreshIcon()
        {
            // Lấy weapon hiện tại từ inventory
            if (weaponInventory.TryGetWeapon((int)input, out WeaponDataSO weapon))
            {
                currentWeapon = weapon;
                if (currentWeapon != null && currentWeapon.Icon != null)
                {
                    iconRenderer.sprite = currentWeapon.Icon;
                    iconRenderer.enabled = true;
                }
                else
                {
                    iconRenderer.sprite = null;
                    iconRenderer.enabled = false;
                }
            }
            else
            {
                iconRenderer.sprite = null;
                iconRenderer.enabled = false;
            }
        }
    }
}