namespace Utilities.Patterns
{
    /// <summary>
    /// Singleton pattern implementation.
    /// </summary>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T s_instance;

        /// <summary>
        /// Returns a singleton class instance.
        /// </summary>
        public static T Instance => s_instance ??= new T();
        
    }
}