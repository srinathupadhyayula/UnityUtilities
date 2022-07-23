using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    public static class HelperFunctions
    {
        private static Camera              s_camera;
        private static PointerEventData    s_eventDataCurrentPosition;
        private static List<RaycastResult> s_results;
        
        private static readonly Dictionary<float, WaitForSeconds> WaitForSecondsMap = new();
        

        /// <summary>
        /// This static function caches the main camera on first call.
        /// This will reduce the cost of calling Camera.main.
        /// </summary>
        /// <remarks>
        /// <para>Recent Unity versions have reduced the cost of Camera.main so this may not be as useful</para>
        /// <para>Author: Matthew-J-Spencer</para>
        /// <see cref="https://github.com/Matthew-J-Spencer"/>
        /// <seealso cref="https://www.youtube.com/c/Tarodev"/>
        /// </remarks>
        public static Camera Camera
        {
            get
            {
                if (s_camera == null)
                {
                    s_camera = Camera.main;
                }

                return s_camera;
            }
        }

        /// <summary>
        /// This static function caches WaitForSeconds objects in a dictionary
        /// This reduces garbage allocations when WaitForSeconds is used frequently
        /// </summary>
        /// <param name="seconds"> Time in seconds to create/get WaitForSeconds object</param>
        /// <remarks>
        /// <para>Author: Matthew-J-Spencer</para>
        /// <see cref="https://github.com/Matthew-J-Spencer"/>
        /// <seealso cref="https://www.youtube.com/c/Tarodev"/>
        /// </remarks>
        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (WaitForSecondsMap.TryGetValue(seconds, out var waitForSeconds))
            {
                return waitForSeconds;
            }

            WaitForSecondsMap[seconds] = new WaitForSeconds(seconds);
            return WaitForSecondsMap[seconds];
        }


        /// <summary>
        /// This static function checks if the mouse pointer is over a UI object
        /// </summary>
        /// <returns> Returns true if pointer is over a UI/canvas object</returns>
        /// <remarks>
        /// <para>This method uses some old Unity concepts like the old input system
        /// Should look into updating or having a different version for New Input System, UI Elements etc.</para>
        /// <para>Author: Matthew-J-Spencer</para>
        /// <see cref="https://github.com/Matthew-J-Spencer"/>
        /// <seealso cref="https://www.youtube.com/c/Tarodev"/>
        /// </remarks> 
        public static bool IsPointerOverUI()
        {
            s_eventDataCurrentPosition = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
            s_results                  = new List<RaycastResult>();
            EventSystem.current.RaycastAll(s_eventDataCurrentPosition, s_results);

            bool overUI = s_results.Count > 0;
            s_results.Clear();
            
            return overUI;
        }


        /// <summary>
        /// This static function calculates and returns world position of a given canvas element
        /// </summary>
        /// <param name="elementTransform"> The transform of the canvas element to calculate world position for.</param>
        /// <returns> Vector2 world position of the canvas element </returns>
        /// <remarks>
        /// <para>Author: Matthew-J-Spencer</para>
        /// <see cref="https://github.com/Matthew-J-Spencer"/>
        /// <seealso cref="https://www.youtube.com/c/Tarodev"/>
        /// </remarks> 
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform elementTransform)
        {
            UnityEngine.RectTransformUtility.ScreenPointToWorldPointInRectangle(elementTransform, elementTransform.position, Camera, out Vector3 result);
            return result;
        }


    }
}