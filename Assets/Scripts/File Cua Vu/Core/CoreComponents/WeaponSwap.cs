using System;
using Saus.Interaction;
using Saus.Interaction.Interactables;
using Saus.Weapons;
using UnityEngine;

namespace Saus.CoreSystem
{
    public class WeaponSwap : CoreComponent
    {
        public event Action<WeaponSwapChoiceRequest> OnChoiceRequested;
        public event Action<WeaponDataSO> OnWeaponDiscarded;

        private InteractableDetector interactableDetector;
        private WeaponInventory weaponInventory;

        private WeaponDataSO newWeaponData;
        private WeaponPickup weaponPickup;

        [SerializeField] private GameObject object1;
        [SerializeField] private GameObject object2;
        [SerializeField] private GameObject object3;

        private void HandleTryInteract(IInteractable interactable)
        {
            if (interactable is not WeaponPickup pickup)
                return;

            weaponPickup = pickup;
            newWeaponData = weaponPickup.GetContext();

            if (weaponInventory.TryGetEmptyIndex(out var index))
            {
                SetObjectsActive(false);

                weaponInventory.TrySetWeapon(newWeaponData, index, out _);
                interactable.Interact();
                newWeaponData = null;
                return;
            }

            SetObjectsActive(true);

            OnChoiceRequested?.Invoke(new WeaponSwapChoiceRequest(
                HandleWeaponSwapChoice,
                weaponInventory.GetWeaponSwapChoices(),
                newWeaponData
            ));
        }

        private void HandleWeaponSwapChoice(WeaponSwapChoice choice)
        {
            if (!weaponInventory.TrySetWeapon(newWeaponData, choice.Index, out var oldData))
                return;

            newWeaponData = null;

            OnWeaponDiscarded?.Invoke(oldData);

            if (weaponPickup != null)
                weaponPickup.Interact();

            SetObjectsActive(false);
        }

        protected override void Awake()
        {
            base.Awake();

            interactableDetector = core.GetCoreComponent<InteractableDetector>();
            weaponInventory = core.GetCoreComponent<WeaponInventory>();
            SetObjectsActive(false);
        }

        private void SetObjectsActive(bool active)
        {
            if (object1 != null) object1.SetActive(active);
            if (object2 != null) object2.SetActive(active);
            if (object3 != null) object3.SetActive(active);
        }

        private void OnEnable()
        {
            interactableDetector.OnTryInteract += HandleTryInteract;
        }

        private void OnDisable()
        {
            interactableDetector.OnTryInteract -= HandleTryInteract;
        }
    }
}
