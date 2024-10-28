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
            BaseFightData.Clear();
            BaseFightData.mFightModeType = typeof(FrameModeManager);
            //TODO
            var sence = GameModule.Scene.LoadScene("City1");
            GameModule.UI.ShowUIAsync<UILoading>(sence);
            CmdManager.Instance.InitData();
            //添加角色和怪物
            // BaseFightData.mPlayerDataList.Add(1,new PlayerProperty(1));
            //BaseFightData.mEnemyDataList.Add(1,new MonsterPlayerProperty(1));
        }
    }
}
