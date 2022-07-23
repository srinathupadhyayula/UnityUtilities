using UnityEditor;
using UnityEngine;

namespace Utilities.Patterns
{
    public class EditorSingleton<T> : Editor where T : Editor
    {
        private static EditorSingleton<T> s_instance;
        
        protected void OnEnable()
        {

            s_instance = this;
        }

        //-----------------------------------------------------------------------------

        protected void OnDestroy()
        {
            s_instance = null;
        }
    }
}