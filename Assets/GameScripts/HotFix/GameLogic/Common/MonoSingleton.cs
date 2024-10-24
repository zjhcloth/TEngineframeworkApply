//=====================================================
// - FileName: MonoSingleton.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/23 16:53:24
// - Description: 带mono的单例
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T instance;

        public static T Instance
        {
            get { return instance; }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else
            {
                Debug.LogError("Get a second instance of this class" + this.GetType());
            }
        }
    }
}
