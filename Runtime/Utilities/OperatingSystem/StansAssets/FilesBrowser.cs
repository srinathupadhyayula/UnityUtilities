using System;
using UnityEngine;

namespace Utilities.OperatingSystem
{
    /// <summary>
    /// System File browser access point.
    /// </summary>
    public static class FilesBrowser
    {
        /// <summary>
        /// Open new file browser window at given path.
        /// </summary>
        /// <param name="path">absolute system path.</param>
        public static void OpenAtPath(string path)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    OpenAtPathWindows(path);
                    break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    OpenAtPathMacOs(path);
                    break;
                default:
                    throw new PlatformNotSupportedException($"{Application.platform} is not supported.");
            }
        }

        private static void OpenAtPathMacOs(string path)
        {
            var openInsidesOfFolder = false;

            // mac finder doesn't like backward slashes
            string macPath = path.Replace("\\", "/");

            // if path requested is a folder, automatically open insides of that folder
            if (System.IO.Directory.Exists(macPath))
                openInsidesOfFolder = true;

            if (!macPath.StartsWith("\""))
                macPath = "\"" + macPath;

            if (!macPath.EndsWith("\""))
                macPath = macPath + "\"";

            string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;
            System.Diagnostics.Process.Start("open", arguments);
        }

        private static void OpenAtPathWindows(string path)
        {
            var openInsidesOfFolder = false;

            // windows explorer doesn't like forward slashes
            string winPath = path.Replace("/", "\\"); //

            // If path requested is a folder, automatically open insides of that folder
            if (System.IO.Directory.Exists(winPath))
                openInsidesOfFolder = true;

            System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
        }
    }
}
