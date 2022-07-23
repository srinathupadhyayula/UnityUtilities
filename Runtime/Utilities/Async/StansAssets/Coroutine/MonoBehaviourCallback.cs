using System;
using Utilities.Patterns;

namespace Utilities.Async
{
    /// <summary>
    /// The MonoBehaviourCallback helper class is meant to be used when you need to have MonoBehaviour default Unity callbacks,
    /// but your model is not a MonoBehaviour and you dont want to convert it to the MonoBehaviour by design.
    ///
    /// Please note, that you will subscribe to the global MonoBehaviour singleton instance. Other parts of code may also use it.
    /// In case other callback users will throw and unhandled exception you may not received the callback you subscribed for.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    public class MonoBehaviourCallback : MonoSingleton<MonoBehaviourCallback>
    {
        /// <summary>
        /// Update is called every frame.
        /// Learn more: [MonoBehaviour.Update](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html)
        /// </summary>
        public static event Action OnUpdate;

        /// <summary>
        /// LateUpdate is called after all Update functions have been called. This is useful to order script execution.
        /// For example a follow camera should always be implemented in LateUpdate because it tracks objects that might have moved inside Update.
        /// Learn more: [MonoBehaviour.LateUpdate](https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html)
        /// </summary>
        public static event Action OnLateUpdate;
        
        /// <summary>
        /// In the editor this is called when the user stops playmode.
        /// Learn more: [MonoBehaviour.OnApplicationQuit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationQuit.html)
        /// </summary>
        public static event Action ApplicationOnQuit;
        
        /// <summary>
        /// Sent to all GameObjects when the application pauses.
        /// Learn more: [MonoBehaviour.OnApplicationPause](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationPause.html)
        /// </summary>
        public static event Action<bool> ApplicationOnPause;
        
        /// <summary>
        /// Sent to all GameObjects when the player gets or loses focus.
        /// Learn more: [MonoBehaviour.OnApplicationFocus](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationFocus.html)
        /// </summary>
        public static event Action<bool> ApplicationOnFocus;

        /// <summary>
        /// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
        /// Learn more: [MonoBehaviour.FixedUpdate](https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html)
        /// </summary>
        public static event Action OnFixedUpdate;

        private void Update()                             => OnUpdate?.Invoke();
        private void LateUpdate()                         => OnLateUpdate?.Invoke();
        private void FixedUpdate()                        => OnFixedUpdate?.Invoke();
        private void OnApplicationPause(bool pauseStatus) => ApplicationOnPause?.Invoke(pauseStatus);
        private void OnApplicationFocus(bool hasFocus)    => ApplicationOnFocus?.Invoke(hasFocus);

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            ApplicationOnQuit?.Invoke();
        }
    }
}
