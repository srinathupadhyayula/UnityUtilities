using UnityEngine;

namespace Utilities.Patterns
{
    internal static class SingletonService
    {
        private static Transform s_servicesObjectTransform;
        public static Transform Parent
        {
            get
            {
                if (s_servicesObjectTransform == null)
                {
                    s_servicesObjectTransform = new GameObject("Singletons").transform;
                    Object.DontDestroyOnLoad(s_servicesObjectTransform);
                }

                return s_servicesObjectTransform;
            }
        }
    }
}