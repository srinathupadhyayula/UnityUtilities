using UnityEngine;

namespace Utilities.Patterns
{
    /// <summary>
    /// Singleton pattern implementation.
    /// Can be used with classes extended from a MonoBehaviour.
    /// Once instance is found or created, game object will be marked as DontDestroyOnLoad.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T    s_instance;
        private static bool s_applicationIsQuitting;
        private static bool s_isDestroyed;


        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            gameObject.transform.SetParent(SingletonService.Parent);
        }

        /// <summary>
        /// Returns a singleton class instance
        /// If current instance is not assigned it will try to find an object of the instance type,
        /// in case instance already exists on a scene. If not, new instance will be created
        /// </summary>
        public static T Instance
        {
            get
            {
                if (s_applicationIsQuitting || s_isDestroyed)
                {
                    Debug.LogError($"{typeof(T)} [MonoSingleton] is already destroyed. "
                                 + $"Please check {nameof(HasInstance)} or {nameof(IsDestroyed)} before accessing instance in the destructor.");
                    return null;
                }

                if (s_instance != null)
                    return s_instance;
                
                s_instance = FindObjectOfType(typeof(T)) as T;
                if (s_instance == null)
                    Instantiate();

                return s_instance;
            }
        }

        /// <summary>
        /// Methods will create new object Instantiate
        /// Normally method is called automatically when you referring to and Instance getter
        /// for a first time.
        /// But it may be useful if you want manually control when the instance is created,
        /// even if you do not this specific instance at the moment
        /// </summary>
        public static void Instantiate()
        {
            string name = typeof(T).FullName;
            s_instance = new GameObject(name).AddComponent<T>();
        }

        /// <summary>
        /// Returns `true` if Singleton Instance exists.
        /// </summary>
        public static bool HasInstance => s_instance != null;

        /// <summary>
        /// If this property returns `true` it means that object with explicitly destroyed.
        /// This could happen if Destroy function  was called for this object or if it was
        /// automatically destroyed during the `ApplicationQuit`.
        /// </summary>
        public static bool IsDestroyed => s_isDestroyed;

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed,
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected virtual void OnDestroy()
        {
            s_instance    = null;
            s_isDestroyed = true;
        }

        protected virtual void OnApplicationQuit()
        {
            s_instance              = null;
            s_isDestroyed           = true;
            s_applicationIsQuitting = true;
        }
    }
}
