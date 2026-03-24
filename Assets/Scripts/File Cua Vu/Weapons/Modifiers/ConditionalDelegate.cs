using Saus.Weapons.Components;
using UnityEngine;

namespace Saus.Weapons.Modifiers
{
    public delegate bool ConditionalDelegate(Transform source, out DirectionalInformation directionalInformation);
}