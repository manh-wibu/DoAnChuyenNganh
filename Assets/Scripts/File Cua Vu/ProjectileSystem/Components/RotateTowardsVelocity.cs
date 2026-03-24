using UnityEngine;

namespace Saus.ProjectileSystem.Components
{
    /// <summary>
    /// This class rotates the current GameObject such that transform.Right points in the same direction as the velocity vector
    /// </summary>
    public class RotateTowardsVelocity : ProjectileComponent
    {
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            Vector2 velocity = rb.velocity;
            if (velocity == Vector2.zero)
                return;

            // Nếu đi sang phải
            if (velocity.x >= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else // Nếu đi sang trái
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}