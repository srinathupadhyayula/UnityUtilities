using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Texture2D Utility methods.
    /// </summary>
    public static class Texture2DUtility
    {
        /// <summary>
        /// Texture scale mode options.
        /// </summary>
        public enum TextureScaleMode
        {
            /// <summary>
            /// Nearest.
            /// </summary>
            Nearest = 0,

            /// <summary>
            /// Bilinear
            /// </summary>
            Bilinear = 1,

            /// <summary>
            /// Average
            /// </summary>
            Average = 2,
        }

        private static readonly Dictionary<float, Texture2D> ColorIcons = new();

        /// <summary>
        /// Generates plane color <see cref="Texture2D"/> object with given size.
        /// You can also choose if you would like to cache generated image, so the next time you ask for the image with the same color,
        /// cache result will be returned instead of generating new image.
        /// </summary>
        /// <param name="colorHtmlString">
        /// Case insensitive html string to be converted into a color.
        /// <see cref="ColorHelper.MakeColorFromHtml(string)"/> for more info about <see cref="colorHtmlString"/> format.
        /// </param>
        /// <param name="width"> Texture width.</param>
        /// <param name="height"> Texture height.</param>
        /// <param name="cacheGeneratedImage">When set to `true` the generated image is cached.</param>
        public static Texture2D MakePlainColorImage(string colorHtmlString, int width = 1, int height = 1, bool cacheGeneratedImage = true)
        {
            Color color = ColorHelper.MakeColorFromHtml(colorHtmlString);
            return MakePlainColorImage(color, width, height, cacheGeneratedImage);
        }

        /// <summary>
        /// Generates plane color <see cref="Texture2D"/> object with given size.
        /// You can also choose if you would like to cache generated image, so the next time you ask for the image with the same color,
        /// cache result will be returned instead of generating new image.
        /// </summary>
        /// <param name="color">Texture color.</param>
        /// <param name="width"> Texture width.</param>
        /// <param name="height"> Texture height.</param>
        /// <param name="cacheGeneratedImage">When set to `true` the generated image is cached.</param>
        public static Texture2D MakePlainColorImage(Color color, int width = 1, int height = 1, bool cacheGeneratedImage = true)
        {
            float colorId = color.r * 100000 + color.g * 10000f + color.b * 1000f + color.a * 100f + width * 10f + height;
            if (ColorIcons.ContainsKey(colorId) && ColorIcons[colorId] != null)
            {
                return ColorIcons[colorId];
            }

            Texture2D plainColorImage = GeneratePlainColorImage(color, width, height);
            if (cacheGeneratedImage)
            {
                ColorIcons[colorId] = plainColorImage;
            }

            return plainColorImage;
        }

        private static Texture2D GeneratePlainColorImage(Color color, int width, int height)
        {
            var plainColorImage = new Texture2D(width, height);
            for (var w = 0; w < width; w++)
            {
                for (var h = 0; h < height; h++)
                {
                    plainColorImage.SetPixel(w, h, color);
                }
            }

            plainColorImage.Apply();

            return plainColorImage;
        }


        /// <summary>
        /// Rotates <see cref="Texture2D"/> pixels to a specified angle.
        /// </summary>
        /// <param name="tex">Source texture to rotate.</param>
        /// <param name="angle">Rotate angle.</param>
        public static Texture2D Rotate(Texture2D tex, float angle)
        {
            var rotImage = new Texture2D(tex.width, tex.height);
            int x;

            int w = tex.width;
            int h = tex.height;
            float x0 = rot_x(angle, -w / 2.0f, -h / 2.0f) + w / 2.0f;
            float y0 = rot_y(angle, -w / 2.0f, -h / 2.0f) + h / 2.0f;

            float dxX = rot_x(angle, 1.0f, 0.0f);
            float dxY = rot_y(angle, 1.0f, 0.0f);
            float dyX = rot_x(angle, 0.0f, 1.0f);
            float dyY = rot_y(angle, 0.0f, 1.0f);

            float x1 = x0;
            float y1 = y0;

            for (x = 0; x < tex.width; x++)
            {
                float x2 = x1;
                float y2 = y1;
                int y;
                for (y = 0; y < tex.height; y++)
                {
                    x2 += dxX;
                    y2 += dxY;
                    rotImage.SetPixel((int)Mathf.Floor(x), (int)Mathf.Floor(y), GetPixel(tex, x2, y2));
                }

                x1 += dyX;
                y1 += dyY;
            }

            rotImage.Apply();
            return rotImage;
        }

        /// <summary>
        /// Resize Texture2D.
        /// </summary>
        /// <param name="source">Source texture to resize.</param>
        /// <param name="newWidth">New texture width.</param>
        /// <param name="newHeight">New texture height. </param>
        /// <param name="filterMode">The filtering mode to use during resize.</param>
        /// <returns></returns>
        public static Texture2D Resize(Texture2D source, int newWidth, int newHeight, FilterMode filterMode)
        {
            source.filterMode = filterMode;
            var rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            var nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newWidth), 0, 0);
            nTex.Apply();
            RenderTexture.active = null;
            return nTex;
        }

        /// <summary>
        /// Scale Texture.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="scale"></param>
        /// <param name="textureScaleMode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Texture2D ScaleTexture(Texture2D source, float scale, TextureScaleMode textureScaleMode)
        {
            int i;

            // Get All the source pixels
            Color[] aSourceColor = source.GetPixels(0);
            var vSourceSize = new Vector2(source.width, source.height);

            // Calculate New Size
            float xWidth = Mathf.RoundToInt(source.width * scale);
            float xHeight = Mathf.RoundToInt(source.height * scale);

            // Make New
            var oNewTex = new Texture2D((int)xWidth, (int)xHeight, TextureFormat.RGBA32, false);

            // Make destination array
            int xLength = (int)xWidth * (int)xHeight;
            var aColor = new Color[xLength];

            var vPixelSize = new Vector2(vSourceSize.x / xWidth, vSourceSize.y / xHeight);

            // Loop through destination pixels and process
            var vCenter = new Vector2();
            for (i = 0; i < xLength; i++)
            {
                // Figure out x&y
                float xX = i % xWidth;
                float xY = Mathf.Floor(i / xWidth);

                // Calculate Center
                vCenter.x = xX / xWidth * vSourceSize.x;
                vCenter.y = xY / xHeight * vSourceSize.y;

                // Do Based on mode
                // Nearest neighbour (testing)
                switch (textureScaleMode)
                {
                    case TextureScaleMode.Nearest:
                    {
                        // Nearest neighbour (testing)
                        vCenter.x = Mathf.Round(vCenter.x);
                        vCenter.y = Mathf.Round(vCenter.y);

                        // Calculate source index
                        var xSourceIndex = (int)(vCenter.y * vSourceSize.x + vCenter.x);

                        // Copy Pixel
                        aColor[i] = aSourceColor[xSourceIndex];
                        break;
                    }

                    case TextureScaleMode.Bilinear:
                    {
                        // Get Ratios
                        float xRatioX = vCenter.x - Mathf.Floor(vCenter.x);
                        float xRatioY = vCenter.y - Mathf.Floor(vCenter.y);

                        // Get Pixel index's
                        var xIndexTl = (int)(Mathf.Floor(vCenter.y) * vSourceSize.x + Mathf.Floor(vCenter.x));
                        var xIndexTr = (int)(Mathf.Floor(vCenter.y) * vSourceSize.x + Mathf.Ceil(vCenter.x));
                        var xIndexBl = (int)(Mathf.Ceil(vCenter.y) * vSourceSize.x + Mathf.Floor(vCenter.x));
                        var xIndexBr = (int)(Mathf.Ceil(vCenter.y) * vSourceSize.x + Mathf.Ceil(vCenter.x));

                        // Calculate Color
                        aColor[i] = Color.Lerp(
                            Color.Lerp(aSourceColor[xIndexTl], aSourceColor[xIndexTr], xRatioX),
                            Color.Lerp(aSourceColor[xIndexBl], aSourceColor[xIndexBr], xRatioX),
                            xRatioY
                        );
                        break;
                    }
                    case TextureScaleMode.Average:
                    {
                        // Calculate grid around point
                        var xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - vPixelSize.x * 0.5f), 0);
                        var xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + vPixelSize.x * 0.5f), vSourceSize.x);
                        var xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - vPixelSize.y * 0.5f), 0);
                        var xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + vPixelSize.y * 0.5f), vSourceSize.y);

                        // Loop and accumulate
                        var oColorTemp = new Color();
                        float xGridCount = 0;
                        for (int iy = xYFrom; iy < xYTo; iy++)
                        {
                            for (int ix = xXFrom; ix < xXTo; ix++)
                            {
                                // Get Color
                                oColorTemp += aSourceColor[(int)(iy * vSourceSize.x + ix)];

                                // Sum
                                xGridCount++;
                            }
                        }

                        // Average Color
                        aColor[i] = oColorTemp / xGridCount;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(textureScaleMode), textureScaleMode, null);
                }
            }

            oNewTex.SetPixels(aColor);
            oNewTex.Apply();
            return oNewTex;
        }

        private static Color GetPixel(Texture2D tex, float x, float y)
        {
            Color pix;
            var x1 = (int)Mathf.Floor(x);
            var y1 = (int)Mathf.Floor(y);

            if (x1 > tex.width || x1 < 0 ||
                y1 > tex.height || y1 < 0)
                pix = Color.clear;
            else
                pix = tex.GetPixel(x1, y1);

            return pix;
        }

        private static float rot_x(float angle, float x, float y)
        {
            float cos = Mathf.Cos(angle / 180.0f * Mathf.PI);
            float sin = Mathf.Sin(angle / 180.0f * Mathf.PI);
            return x * cos + y * -sin;
        }

        private static float rot_y(float angle, float x, float y)
        {
            float cos = Mathf.Cos(angle / 180.0f * Mathf.PI);
            float sin = Mathf.Sin(angle / 180.0f * Mathf.PI);
            return x * sin + y * cos;
        }
    }
}
