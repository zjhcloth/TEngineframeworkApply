using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ChuhaoBridge
{
    public class ChuhaoBridge
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern double getStartTime();
#endif

        private static ChuhaoBridge _instance;

        public static ChuhaoBridge Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new ChuhaoBridge();
                }
                return _instance;
            }
        }

        public double GetStartTime()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return getStartTime();
#endif
            return 0;
        }

    }
    
    
}
