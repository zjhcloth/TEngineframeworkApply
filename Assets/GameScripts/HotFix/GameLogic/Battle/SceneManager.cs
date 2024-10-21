//=====================================================
// - FileName: SceneManager.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/18 11:32:41
// - Description:
//======================================================
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    /// <summary>
    /// 游戏场景管理
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// 进入主场景
        /// </summary>
        public static void EnterMain()
        {
            //资源清理
            //TODO
            var sence = GameModule.Scene.LoadScene("FubenCenter");
            GameModule.UI.ShowUIAsync<UILoading>(sence);
            //GameModule.Scene.LoadScene("FubenCenter");
        }
        
        public static void EnterBattle()
        {
            //资源清理
            BaseBattleData.Clear();
            //TODO
            var sence = GameModule.Scene.LoadScene("FubenCenter");
            GameModule.UI.ShowUIAsync<UILoading>(sence);
            BaseBattleData.mPlayerDataList.Add(1,new PlayerProperty(1));
            BaseBattleData.mEnemyDataList.Add(1,new MonsterProperty(1));
        }
    }
}
