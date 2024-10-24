//=====================================================
// - FileName: FrameModeManager.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/18 11:32:41
// - Description: 帧战斗模式
//======================================================
using System;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class FrameModeManager : BaseModeManager
    {
        public float mGameTime = 1000;
        private DateTime mTime = DateTime.Now;
        public override void InitData()
        {
            PreLoadModel.Instance.Clear(); //清除预加载对象池的内容，让主场景的对象不影响战斗场景的内存
            MessengerKey.SFUpdateEvent.AddListener(SFUpdate);
            base.InitData();
        }

        //清除离开战斗的数据
        public override void Clear()
        {
            Time.timeScale = 1;
            CmdManager.Instance.Clear();


            foreach (var ctrl in BaseFightData.mPlayerCtrlList)
            {
                if (ctrl.Value != null)
                {
                    ctrl.Value.Clear();
                }
            }
            MessengerKey.SFUpdateEvent.RemoveListener(SFUpdate);
            PreLoadModel.Instance.Clear(); //清除预加载对象池的内容，否则会出现内存泄漏
            base.Clear();

            Log.Debug("clear  FramMode");
        }

        public override void InitComplete()
        {
            base.InitComplete();
        }

        protected void InitPlayer()
        {
            
        }

        public override void OnUpdate()
        {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN //添加PC包可以键盘控制移动和跳跃
            MoveSend();
#endif
            base.OnUpdate();
            BaseFightData.Update();
        }

        public override void OnFixUpdate()
        {
            if (!mStart)
            {
                return;
            }

            TimeScaleCtrl();

            CmdManager.Instance.OnFixUpdate();
            base.OnFixUpdate();

            BaseFightData.FixUpdate();
        }

        private void TimeScaleCtrl()
        {
            if (CmdManager.Instance.Quicken())
            {
                Time.timeScale = 2;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public virtual void SFUpdate()
        {
        }

        #region 编辑器、主机上用的快捷键操作

        /// <summary>
        /// 快捷键移动
        /// </summary>
        private void MoveSend()
        {
            Vector2 SendVector = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                SendVector.y = 1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                SendVector.y = -1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                SendVector.x = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                SendVector.x = 1f;
            }

            CmdManager.Instance.CmdMove(SendVector);

            // int cmd = GetMoveCmd(Vector3.up, SendVector);
            // if (cmd != lastMove)
            // {
            //     lastMove = cmd;
            //     if (GameCache.mCmdmanager != null)
            //         GameCache.mCmdmanager.CmdMove(cmd);
            // }
            
        }

        // public int GetMoveCmd(Vector2 from, Vector2 to)
        // {
        //     int cmd = 30;
        //     if (to != Vector2.zero)
        //     {
        //         Vector3 cross = Vector3.Cross(from, to);
        //         float angle = Vector2.Angle(from, to);
        //         angle = cross.z > 0 ? -angle + 360f : angle;
        //         angle = Mathf.Min(angle, 359);
        //         cmd = (int)(angle * 30 / 360);
        //     }
        //
        //     return cmd;
        // }

        #endregion
    }
}