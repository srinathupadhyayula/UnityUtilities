using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class RigidbodyExtensions
    {
        /// <summary>
        /// This method changes the direction of the <see cref="Rigidbody2D"/> while keeping the velocity unchanged.
        /// </summary>
        /// <param name="thisRigidbody">The current <see cref="Rigidbody2D"/> to change the direction of.</param>
        /// <param name="newDirection">The new Direction to set the <see cref="Rigidbody2D"/> to.</param>
        /// <remarks>
        /// <para>This is the Rigidbody2D version of the <see cref="RigidbodyExtensions">Rigidbody extension</see> method of the same name.</para>
        /// </remarks>
        public static void ChangeDirection(this Rigidbody2D thisRigidbody, Vector2 newDirection) =>
            thisRigidbody.velocity = newDirection.normalized * thisRigidbody.velocity.magnitude;

        /// <summary>
        /// This method normalizes the velocity of the <see cref="Rigidbody2D"/>.
        /// </summary>
        /// <param name="thisRigidbody"></param>
        /// <param name="newMagnitude"></param>
        /// <remarks>
        /// <para>This is the Rigidbody2D version of the <see cref="RigidbodyExtensions">Rigidbody extension</see> method of the same name.</para>
        /// </remarks>
        public static void NormalizeVelocity(this Rigidbody2D thisRigidbody, float newMagnitude = 1) =>
            thisRigidbody.velocity = thisRigidbody.velocity.normalized * newMagnitude;
    }
}