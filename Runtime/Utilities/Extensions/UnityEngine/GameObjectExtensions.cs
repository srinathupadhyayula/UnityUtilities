using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class GameObjectExtensions
    {
        /// <summary>
        /// Checks if the gameobject has a component of type T 
        /// </summary>
        /// <param name="gameObject">The gameobject to check for component</param>
        /// <param name="componentToReturn">If the object has component then it will fill this variable</param>
        /// <typeparam name="TComponentType">Component type to check</typeparam>
        /// <returns>True if gameobject has Component of type T, or else false</returns>
        /// <remarks><para>This method is heavily based on the blog post at
        /// <see cref="https://monoflauta.com/2021/07/27/11-useful-unity-c-extension-methods/">Monoflauta</see></para>
        /// </remarks>
        public static bool HasComponent<TComponentType>(this GameObject gameObject, out TComponentType componentToReturn)
            where TComponentType : Component
        {
            return gameObject.TryGetComponent(out componentToReturn);
        }

        /// <summary>
        /// <para>Returns a component of type 'T', if the gameobject already has one. If the gameobject does not have a component of type 'T',
        /// the method adds the component of type 'T' and returns the newly added component.</para> 
        /// </summary>
        /// <param name="gameObject">The gameobject to get component from or add component to.</param>
        /// <typeparam name="T">Component type to get or add</typeparam>
        /// <returns>Component of type 'T' that is attached to this gameobject.</returns>
        /// <remarks><para>This method is heavily based on the blog post:
        /// <see cref="https://monoflauta.com/2021/07/27/11-useful-unity-c-extension-methods/"/></para>
        /// <para>This method is heavily based on the blog post at
        /// <see cref="https://monoflauta.com/2021/07/27/11-useful-unity-c-extension-methods/">Monoflauta</see> and
        /// a similar method in <see cref="https://github.com/DapperDino">DapperDino</see>'s
        /// <see cref="https://github.com/DapperDino/Dapper-Tools">Dapper-Tools</see> package.</para>
        /// </remarks>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.HasComponent<T>(out T requestedComponent) ? requestedComponent : gameObject.AddComponent<T>();
        }
        
        /// <summary>
        /// Destroys all children of the game object.
        /// </summary>
        /// <param name="thisGameObject">GameObject to use</param>
        public static void DestroyChildren(this GameObject thisGameObject)
        {
            thisGameObject.transform.DestroyAllChildren();
        }

        /// <summary>
        /// Resets transform's position, scale and rotation
        /// </summary>
        /// <param name="thisGameObject">GameObject to use</param>
        public static void ResetTransformation(this GameObject thisGameObject)
        {
            thisGameObject.transform.Reset();
        }
    }
}