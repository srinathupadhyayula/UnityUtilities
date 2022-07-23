using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class RigidbodyExtensions
    {
        /// <summary>
        /// This method changes the direction of the <see cref="Rigidbody"/> while keeping the velocity unchanged.
        /// </summary>
        /// <param name="thisRigidbody">The current <see cref="Rigidbody"/> to change the direction of.</param>
        /// <param name="newDirection">The new Direction to set the <see cref="Rigidbody"/> to.</param>
        /// <remarks><para>This method is heavily based on the blog post:
        /// <see cref="https://monoflauta.com/2021/07/27/11-useful-unity-c-extension-methods/"/></para>
        /// <para>This method is modified from the original post to use expression body"=>..." instead of a statement body"{...}".
        /// <br/> The parameter names are also modified to be more descriptive.</para>
        /// </remarks>
        public static void ChangeDirection(this Rigidbody thisRigidbody, Vector3 newDirection) =>
            thisRigidbody.velocity = newDirection.normalized * thisRigidbody.velocity.magnitude;

        /// <summary>
        /// This method normalizes the velocity of the <see cref="Rigidbody"/> to match the new magnitude or speed.
        /// </summary>
        /// <param name="thisRigidbody">The current <see cref="Rigidbody"/> to change the direction of.</param>
        /// <param name="newMagnitude">The new magnitude or "speed" to set the velocity to.</param>
        /// <remarks><para>This method is heavily based on the blog post:
        /// <see cref="https://monoflauta.com/2021/07/27/11-useful-unity-c-extension-methods/"/></para>
        /// <para>This method is modified from the original post to use expression body"=>..." instead of a statement body"{...}".
        /// <br/> The parameter names are also modified to be more descriptive.</para>
        /// </remarks>
        public static void NormalizeVelocity(this Rigidbody thisRigidbody, float newMagnitude = 1) =>
            thisRigidbody.velocity = thisRigidbody.velocity.normalized * newMagnitude;
    }
}