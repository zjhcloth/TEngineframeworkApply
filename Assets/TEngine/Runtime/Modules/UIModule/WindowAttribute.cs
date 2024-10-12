﻿using System;

namespace TEngine
{
    /// <summary>
    /// UI层级枚举。
    /// </summary>
    public enum UILayer : int
    {
        Bottom = 0,
        UI = 1,
        Top = 2,
        Tips = 3,
        System = 4,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class WindowAttribute : Attribute
    {
        /// <summary>
        /// 窗口层级
        /// </summary>
        public readonly int WindowLayer;

        /// <summary>
        /// 资源定位地址。
        /// </summary>
        public readonly string Location;

        /// <summary>
        /// 全屏窗口标记。
        /// </summary>
        public readonly bool FullScreen;

        /// <summary>
        /// 是内部资源无需AB加载。
        /// </summary>
        public readonly bool FromResources;

        public readonly int HideTimeToClose;

        public WindowAttribute(int windowLayer, string location = "", bool fullScreen = false, int hideTimeToClose = 10)
        {
            WindowLayer = windowLayer;
            Location = location;
            FullScreen = fullScreen;
            HideTimeToClose = hideTimeToClose;
        }

        public WindowAttribute(UILayer windowLayer, string location = "", bool fullScreen = false, int hideTimeToClose = 10)
        {
            WindowLayer = (int)windowLayer;
            Location = location;
            FullScreen = fullScreen;
            HideTimeToClose = hideTimeToClose;
        }

        public WindowAttribute(UILayer windowLayer, bool fromResources, bool fullScreen = false, int hideTimeToClose = 10)
        {
            WindowLayer = (int)windowLayer;
            FromResources = fromResources;
            FullScreen = fullScreen;
            HideTimeToClose = hideTimeToClose;
        }

        public WindowAttribute(UILayer windowLayer, bool fromResources, string location, bool fullScreen = false, int hideTimeToClose = 10)
        {
            WindowLayer = (int)windowLayer;
            FromResources = fromResources;
            Location = location;
            FullScreen = fullScreen;
            HideTimeToClose = hideTimeToClose;
        }
    }
}