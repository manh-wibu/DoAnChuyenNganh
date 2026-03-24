using Saus.Interaction;
using UnityEngine;

namespace Saus.Utilities
{
    public static class ComponentUtilities
    {
        public static bool IsInteractable(this Component component, out IInteractable interactable)
        {
            return component.TryGetComponent(out interactable);
        }
    }
}