//=====================================================
// - FileName: FightManager.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 14:50:12
// - Description: 战斗管理
//======================================================

using System.Collections.Generic;
using GameBase;
using TEngine;
using UnityEngine;


namespace GameLogic
{
    public class CmdManager : Singleton<CmdManager>
    {
        public bool GameOver = false;
        public int mCurMaxFrame = -1;// 当前执行到的最大帧步进
        private float mLocFpsTime = 0;//本地帧时间
        List<EnumUtil.BuffType> removeBuffs = new List<EnumUtil.BuffType>();
        protected Vector2 mCurMove = Vector2.zero;//当前移动方向
        public  void InitData()
        {
            Log.Debug("Start");
            mCurMaxFrame = 0;
            STime.time = mCurMaxFrame * STime.deltaTime;
        }

        void OnLocFixUpdate()
        {
            mLocFpsTime -= Time.fixedDeltaTime;
            if (mLocFpsTime <= 0)
            {
                mLocFpsTime += STime.deltaTime;
                mCurMaxFrame++;
                UpdateData();
            }
        }
        

        /// <summary>
        /// 帧更新
        /// </summary>
        private void UpdateData()
        {
            if (GameOver)
            {
                Debug.Log("GameOver");
                return;
            }
            MessengerKey.SFUpdateEvent.Broadcast();
            BasePlayerProperty pro;
            for (int i = 1; i <= 9; i++)
            {
                if (BaseFightData.mPlayerDataList.TryGetValue(i, out pro))
                {
                    pro.mTime += STime.deltaTime;
                    UpdateTarget(pro);//找目标
                    UpdateBuff(pro);//更新buff
                    UpdateSkill(pro);//更新技能
                    UpdateMove(pro);//移动
                    
                }
            }
            for (int i = 1; i <= 9; i++)
            {
                if (BaseFightData.mEnemyDataList.TryGetValue(i, out pro))
                {
                    pro.mTime += STime.deltaTime;
                    UpdateTarget(pro);//找目标
                    UpdateBuff(pro);//更新buff

                    UpdateSkill(pro);//更新技能
                    UpdateMove(pro);//移动
                }
            }

            STime.UpdateFrame(mCurMaxFrame);
        }
        private void UpdateTarget(BasePlayerProperty pro)
        {
            BasePlayerProperty targetPro = null;
            if (pro.Target == null)
            {
                if (pro.Loc == 1)
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        BasePlayerProperty tmpPro;
                        if (BaseFightData.mEnemyDataList.TryGetValue(i, out tmpPro))
                        {
                            if (targetPro == null)
                            {
                                targetPro = tmpPro;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        BasePlayerProperty tmpPro;
                        if (BaseFightData.mPlayerDataList.TryGetValue(i, out tmpPro))
                        {
                            if (targetPro == null)
                            {
                                targetPro = tmpPro;
                            }
                        }
                    }
                }

                pro.Target = targetPro == null ? pro : targetPro;
                Debug.Log($" 当前帧{mCurMaxFrame}-----{GetPropertyStr(pro)}坐标是：{pro.Position}  目标是{pro.Target.Position} ");
            }
        }
        private void UpdateBuff(BasePlayerProperty pro)
        {
            if (pro.Buffs == null || pro.Buffs.Count == 0)
            {
                return;
            }
            removeBuffs.Clear();
            foreach (Buff buff in pro.Buffs.Values)
            {
                if (buff.EndTime < pro.mTime)
                {
                    removeBuffs.Add(buff.Type);
                }
            }
            //需要更新用户信息
            bool needUpdate = false;
            foreach (var type in removeBuffs)
            {
                if(type == EnumUtil.BuffType.禁疗 
                   || type == EnumUtil.BuffType.虚弱 
                   || type == EnumUtil.BuffType.迟缓 
                   || type == EnumUtil.BuffType.寒意 
                   || type == EnumUtil.BuffType.易伤 
                   )
                pro.RemoveBuff(type);
                //TODO 特殊buff处理
            }
            foreach (var type in pro.Buffs.Values)
            {
                //TODO buff处理
            }
            
        }

        private void UpdateSkill(BasePlayerProperty pro)
        {
            if (pro.CurrSkill == null)
            {
                //优先判断大招
                if (pro.Skill2 != null && GameCache.AutoFight)
                {
                    if (pro.Buffs.ContainsKey(EnumUtil.BuffType.沉默)
                        || pro.Buffs.ContainsKey(EnumUtil.BuffType.咒魂箓)
                        || pro.Buffs.ContainsKey(EnumUtil.BuffType.击飞)
                        || pro.Buffs.ContainsKey(EnumUtil.BuffType.眩晕))
                    {
                        return; //这些状态无法施放技能
                    }

                    if (pro.Skill2.NeedTarget && pro.Target == null)
                    {
                        Debug.Log("当前没有目标！");
                        return;
                    }

                    pro.CurrSkill = pro.Skill2;
                    pro.Energy = 0; //怒气直接消耗完
                    pro.Skill2.EndTime = pro.mTime + pro.Skill2.CD;
                }

                //然后判断普攻
                if (pro.Skill1 != null)
                {
                    if (pro.Buffs.ContainsKey(EnumUtil.BuffType.缴械)
                        || pro.Buffs.ContainsKey(EnumUtil.BuffType.咒魂箓)
                        || pro.Buffs.ContainsKey(EnumUtil.BuffType.击飞)
                        || pro.Buffs.ContainsKey(EnumUtil.BuffType.眩晕))
                    {
                        return; //缴械无法普通攻击
                    }

                    pro.CurrSkill = pro.Skill1;
                    pro.Skill1.EndTime = pro.mTime + pro.Skill1.CD;
                }

                //再加一层判断
                if (pro.CurrSkill != null)
                {
                    if (pro.Target != null
                        && !pro.Target.IsDie
                        && pro.MoveStatus == EnumUtil.MoveStatus.攻击)
                    {
                        pro.Target.Hp -= pro.CurrSkill.Hurt;
                        if (pro.Target.IsDie)
                        {
                            GameOver = true;
                        }

                        Debug.Log(
                            $"当前帧{mCurMaxFrame}-----{GetPropertyStr(pro)}  攻击  {GetPropertyStr(pro.Target)}  造成伤害{pro.Atk} ,目标剩余血量 {pro.Target.Hp}");
                    }
                }
            }

        }

        private void UpdateMove(BasePlayerProperty pro)
        {
            if (pro.Buffs.ContainsKey(EnumUtil.BuffType.冰冻) 
                || pro.Buffs.ContainsKey(EnumUtil.BuffType.定身)
                || pro.Buffs.ContainsKey(EnumUtil.BuffType.击飞)
                || pro.Buffs.ContainsKey(EnumUtil.BuffType.眩晕)
                || pro.Buffs.ContainsKey(EnumUtil.BuffType.咒魂箓))
            {
                return;//这些buff状态无法移动
            }
            if (pro.Target != null && pro.MoveStatus == EnumUtil.MoveStatus.移动)
            {
                Vector3 lastPos = pro.Position;
                if (pro.IsLeader && GameCache.UserCtrlMove)
                {
                    pro.Position += pro.MoveDir * pro.StimeSpeed;
                }
                else
                {
                    Vector3 dir = (pro.Target.Position - pro.Position).normalized;
                    
                    float x = pro.Position.x - pro.Target.Position.x;
                    float z = pro.Position.z - pro.Target.Position.z;
                    pro.Position += dir * pro.Speed;
                    Debug.Log($" 当前帧{mCurMaxFrame}-----{GetPropertyStr(pro)}  移动 从 {lastPos}  移动到{pro.Position}  dis：{(x * x + z * z)}   range：{pro.AtkRange * pro.AtkRange}");
                    if (CollisionCheck.CanAttack(pro.Position, pro.Target.Position, pro.AtkRange))
                    {
                        pro.MoveStatus = EnumUtil.MoveStatus.攻击;
                        Debug.Log($" 当前帧{mCurMaxFrame}-----{GetPropertyStr(pro)}  移动 状态改为 攻击");
                    }
                    
                }
            }
        }

        private string GetPropertyStr(BasePlayerProperty pro)
        {
            if (pro is PlayerProperty)
                return "Player" + pro.Loc;
            else
                return "Monster" + pro.Loc;
        }

        /// <summary>
        /// 是否要加速处理
        /// </summary>
        /// <returns></returns>
        public bool Quicken()
        {
            bool quicken = GameCache.DoubleSpeed;
            return quicken;
        }

        public void OnFixUpdate()
        {
            OnLocFixUpdate();
        }
        
        /// <summary>
        /// 设置当前移动变化
        /// </summary>
        /// <param name="move"></param>
        public void CmdMove(Vector2 move)
        {
            if (mCurMaxFrame < 0)
                return;
            mCurMove = move;
        }

        public void Clear()
        {
            
        }
    }
}