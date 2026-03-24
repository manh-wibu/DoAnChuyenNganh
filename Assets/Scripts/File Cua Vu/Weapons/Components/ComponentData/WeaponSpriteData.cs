using Saus.Weapons.Components;
using UnityEngine;

namespace Saus.Weapons.Components
{
    public class WeaponSpriteData : ComponentData<AttackSprites>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(WeaponSprite);
        }
    }
}