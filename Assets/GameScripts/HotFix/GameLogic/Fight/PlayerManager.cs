//=====================================================
// - FileName: PlayerManager.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/24 10:02:09
// - Description: 角色管理器,统一实例化，缓存和销毁
//======================================================
using System.Collections.Generic;
using UnityEngine;
using static GameLogic.PlayerProperty;

namespace GameLogic
{
    

public class PlayerManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    public static void CreatePlayer()
    {
        ProcessAddPlayer(1);
        ProcessAddPlayerEnemy(2);
        BaseFightData.mLeaderLoc = 1;
    }

    public static PlayerProperty ProcessAddPlayer(int loc)
    {
        PlayerProperty pro = BaseFightData.GetPlayerPro<PlayerProperty>(loc);
        if (pro != null)
        {
            return pro;
        }
        pro = new PlayerProperty(loc);

        pro.Atk = 100; //攻击
        pro.Def = 100; //防御
        pro. Hp = 10000; //气血
        pro.Mp = 100; //内力
        //特殊属性
        pro.Energy = 100; //怒气
        pro.Shield = 100; //护体罡气
        //进阶属性
        pro.Hurt = 100; //伤害
        pro.HurtDef = 100; //伤害抗性
        pro.EnergyRate = 100; //怒气效率
        pro.Avo = 100; //闪避率
        pro.AvoDis = 100; //忽视闪避率
        pro.CritRate = 100; //暴击率
        pro.CritRateDis = 100; //忽视暴击率
        pro.CritHurt = 100; //暴伤
        pro.CritHurtDis = 100; //忽视爆伤
        pro.AtkSpd = 100; //攻速
        pro.Speed = 1;//移动速度
        pro.StimeSpeed = 3 * STime.deltaTime;//初始化每帧的速度。避免每帧计算
        pro.NorHurt = 100; //普攻伤害
        pro.NorHurtDef = 100; //普攻抗性
        pro.SkillHurt = 100; //技能伤害
        pro.SkillHurtDef = 100; //技能伤害抗性
        pro.HurtDur = 100; //持续伤害
        pro.HurtDurDef = 100; //持续伤害抗性
        pro.AtkRange = 3;//攻击距离
        pro.Position = new Vector3(-3, 7, -10);
        pro.MoveStatus = EnumUtil.MoveStatus.移动;
        pro.Buffs = new Dictionary<EnumUtil.BuffType, Buff>();
        pro.IsDie = false;

        BaseFightData.mPlayerDataList.Add(pro.Loc, pro);
        MessengerKey.AddPlayerEvent.Broadcast(pro);

        CreateOneGameObject(pro);

        return pro;
    }

    public static PlayerProperty ProcessAddPlayerEnemy(int loc)
    {
        PlayerProperty pro = BaseFightData.GetEnemyPlayerPro<PlayerProperty>(loc);
        if (pro != null)
        {
            return pro;
        }
        pro = new PlayerProperty(loc);

        pro.Atk = 100; //攻击
        pro.Def = 100; //防御
        pro. Hp = 10000; //气血
        pro.Mp = 100; //内力
        //特殊属性
        pro.Energy = 100; //怒气
        pro.Shield = 100; //护体罡气
        //进阶属性
        pro.Hurt = 100; //伤害
        pro.HurtDef = 100; //伤害抗性
        pro.EnergyRate = 100; //怒气效率
        pro.Avo = 100; //闪避率
        pro.AvoDis = 100; //忽视闪避率
        pro.CritRate = 100; //暴击率
        pro.CritRateDis = 100; //忽视暴击率
        pro.CritHurt = 100; //暴伤
        pro.CritHurtDis = 100; //忽视爆伤
        pro.AtkSpd = 100; //攻速
        pro.Speed = 1;//移动速度
        pro.StimeSpeed = 1 * STime.deltaTime;//初始化每帧的速度。避免每帧计算
        pro.NorHurt = 100; //普攻伤害
        pro.NorHurtDef = 100; //普攻抗性
        pro.SkillHurt = 100; //技能伤害
        pro.SkillHurtDef = 100; //技能伤害抗性
        pro.HurtDur = 100; //持续伤害
        pro.HurtDurDef = 100; //持续伤害抗性
        pro.AtkRange = 2f;//攻击距离
        pro.Position = new Vector3(3, 7, -10);
        pro.MoveStatus = EnumUtil.MoveStatus.移动;
        pro.Buffs = new Dictionary<EnumUtil.BuffType, Buff>();
        pro.IsDie = false;

        BaseFightData.mEnemyDataList.Add(pro.Loc, pro);
        MessengerKey.AddPlayerEvent.Broadcast(pro);

        CreateOneGameObject(pro, true);

        return pro;
    }
    

    /// <summary>
    /// 创建角色实体
    /// </summary>
    /// <param name="playerPro">角色信息</param>
    public static void CreateOneGameObject(PlayerProperty playerPro, bool isMonster = false)
    {
        string prefab = "hero";//"card";
        GameObject obj = FightMemoryManager.Instance.Allocate(prefab, false);
        string rob = isMonster ? "monster" : "player";
        obj.name = rob + "_" + playerPro.Loc;

        obj.transform.position = Vector3.zero;
        FightPlayerCtrl playerCtrl = new FightPlayerCtrl(playerPro.Loc);
        BaseFightData.AddPlayerCtrl(playerPro.Loc, playerCtrl);

        playerCtrl.InitAsync(playerPro.Loc, obj);


    }



}

}
