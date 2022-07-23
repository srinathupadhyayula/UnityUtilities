using UnityEngine;

namespace Utilities.Types.Layer
{
    /// <summary>
    /// LayerMap ScriptableObject that wraps an int array of layers
    /// </summary>
    /// <remarks>
    /// <para>This class is heavily based on a similar class in <see cref="https://github.com/YondernautsGames">YondernautGames</see>'
    /// <see cref="https://github.com/YondernautsGames/LayerManager">LayerManager</see> repository.</para>
    /// </remarks>
    public class LayerMap : ScriptableObject
    {
        [SerializeField] private int[] m_map;

        public int TransformLayer (int old)
        {
            // Check value is 0-31 (default if not)
            if (old < 0)
                old = 0;
            if (old >= 32)
                old = 0;

            // Return new index
            return m_map[old];
        }

        public int TransformMask (int old)
        {
            var result = 0;

            // Iterate through each old layer
            for (var i = 0; i < 32; ++i)
            {
                // Get old flag for layer
                int flag = (old >> i) & 1;
                // Assign flag to new layer
                result |= flag << TransformLayer(i);
            }

            return result;
        }
    }
}