using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class BaseModeManager : IBaseMode
    {
        protected bool mStart = false;

        /// <summary>
        /// 初始化数据
        /// </summary>
        public virtual void InitData()
        {
          
        }

        public virtual void CreateGameObject()
        {
        }

        public virtual void ShowUI()
        {
        }

        public virtual void InitComplete()
        {
            mStart = true;
        }

        public virtual void Clear()
        {
        }

        public virtual void OnUpdate()
        {


        }

        public virtual void OnFixUpdate()
        {

        }

        public bool IsStart()
        {
            return mStart;
        }
    }
}
