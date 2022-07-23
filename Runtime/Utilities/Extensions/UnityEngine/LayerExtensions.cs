using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class LayerExtensions
    {
        public static LayerMask ToLayerMask(int layer)
        {
            return 1 << layer;
        }

        public static bool LayerInMask(this LayerMask mask, int layer)
        {
            return ((1 << layer) & mask) != 0;
        }
    }
}