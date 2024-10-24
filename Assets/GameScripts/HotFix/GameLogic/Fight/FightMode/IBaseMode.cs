//=====================================================
// - FileName: IBaseMode.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/18 11:32:41
// - Description: 战斗模式
//======================================================
using System;
using UnityEngine;

namespace GameLogic
{
    public interface IBaseMode
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData();
  
        /// <summary>
        /// 战斗对象创建
        /// </summary>
        void CreateGameObject();

        /// <summary>
        /// 战斗UI加载
        /// </summary>
        void ShowUI();

        /// <summary>
        /// 初始化完成调用
        /// </summary>
        void InitComplete();

        /// <summary>
        /// 每帧更新
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 每帧更新
        /// </summary>
        void OnFixUpdate();
        /// <summary>
        /// 是否开始
        /// </summary>
        /// <returns></returns>
        bool IsStart();
        /// <summary>
        /// 退出战斗的清理
        /// </summary>
        void Clear();
    }
}
