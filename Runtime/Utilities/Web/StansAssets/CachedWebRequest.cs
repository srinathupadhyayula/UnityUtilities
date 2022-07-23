using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Utilities.Async;

namespace Utilities.Web
{
    /// <summary>
    /// Helper class that works with <see cref="UnityWebRequest"/> under the hood.
    /// All the result of the requests via this class will be stored in the runtime cache.
    ///
    /// So for example if class A whats to get an image from https://web/my-image.png and while request in progress
    /// class B wants to do that same, only 1 request will be sent, and image will be stored in cache in case you would need it later.
    ///
    /// You can clear all the cached results via ClearCache method.
    /// WARNING: only use it for endpoints with static result since every request will only be made once.
    ///
    /// It also contains shortcut methods to get Images & Audio clips via http.
    /// </summary>
    public static class CachedWebRequest
    {
        private static readonly Dictionary<string, UnityWebRequest>               Cache              = new();
        private static readonly Dictionary<string, List<Action<UnityWebRequest>>> ThumbnailLoadQueue = new();

        /// <summary>
        /// Send a UnityWebRequest configured for HTTP GET.
        /// </summary>
        /// <param name="uri">The URI of the resource to retrieve via HTTP GET.</param>
        /// <param name="callback">Callback will be fired once request is completed</param>
        public static void Get(string uri, Action<UnityWebRequest> callback)
        {
            //TODO a proper validation would be nice
            if (string.IsNullOrEmpty(uri))
            {
                callback.Invoke(null);
                return;
            }

            if (ThumbnailLoadQueue.ContainsKey(uri))
            {
                var callbacks = ThumbnailLoadQueue[uri];
                if (callback != null) callbacks.Add(callback);
            }
            else
            {
                var callbacks = new List<Action<UnityWebRequest>>();
                if (callback != null) callbacks.Add(callback);
                ThumbnailLoadQueue.Add(uri, callbacks);

                var request = UnityWebRequest.Get(uri);
                SendRequest(request, (requestResult) =>
                {
                    var registeredCallbacks = ThumbnailLoadQueue[request.url];
                    ThumbnailLoadQueue.Remove(request.url);

                    foreach (var cb in registeredCallbacks) cb.Invoke(requestResult);
                });
            }
        }

        /// <summary>
        /// Send a UnityWebRequest configured for HTTP GET.
        /// </summary>
        /// <param name="uri">The URI of the resource to retrieve via HTTP GET.</param>
        /// <param name="callback"> Returns the downloaded Texture2D, or null.</param>
        public static void GetTexture2D(string uri, Action<Texture2D> callback)
        {
            Get(uri, (unityWebRequest) =>
            {
                if (unityWebRequest == null)
                {
                    callback.Invoke(null);
                }
                else
                {
                    var texture = new Texture2D(1, 1);
                    texture.LoadImage(unityWebRequest.downloadHandler.data);
                    callback.Invoke(texture);
                }
            });
        }

        /// <summary>
        /// Send a UnityWebRequest configured for HTTP GET.
        /// </summary>
        /// <param name="uri">The URI of the resource to retrieve via HTTP GET.</param>
        /// <param name="callback"> Returns the downloaded AudioClip, or null.</param>
        public static void GetAudioClip(string uri, Action<AudioClip> callback)
        {
            Get(uri, (request) =>
            {
                var result = DownloadHandlerAudioClip.GetContent(request);
                callback.Invoke(result);
            });
        }

        /// <summary>
        /// Removes all elements from the cache
        /// </summary>
        public static void ClearCache()
        {
            Cache.Clear();
        }

        private static void SendRequest(UnityWebRequest request, Action<UnityWebRequest> callback)
        {
            if (Cache.ContainsKey(request.url))
            {
                callback.Invoke(Cache[request.url]);
                return;
            }

            CoroutineUtility.Start(SendRequestCoroutine(request, (result) =>
            {
                Cache.Add(result.url, result);
                callback.Invoke(result);
            }));
        }

        private static IEnumerator SendRequestCoroutine(UnityWebRequest request, Action<UnityWebRequest> callback)
        {
            yield return request.SendWebRequest();
            callback.Invoke(request);
        }
    }
}
