//=====================================================
// - FileName: MonsterProperty.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 10:02:09
// - Description: 怪物属性
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class MonsterPlayerProperty : BasePlayerProperty
    {
        public MonsterPlayerProperty(int loc) : base(loc)
        {
            HeroType = EnumUtil.HeroType.万变;
            
            Atk = 100; //攻击
            Def = 100; //防御
            Hp = 10000; //气血
            Mp = 100; //内力
            //特殊属性
            Energy = 100; //怒气
            Shield = 100; //护体罡气
            //进阶属性
            Hurt = 100; //伤害
            HurtDef = 100; //伤害抗性
            EnergyRate = 100; //怒气效率
            Avo = 100; //闪避率
            AvoDis = 100; //忽视闪避率
            CritRate = 100; //暴击率
            CritRateDis = 100; //忽视暴击率
            CritHurt = 100; //暴伤
            CritHurtDis = 100; //忽视爆伤
            AtkSpd = 100; //攻速
            Speed = 1;//移动速度
            NorHurt = 100; //普攻伤害
            NorHurtDef = 100; //普攻抗性
            SkillHurt = 100; //技能伤害
            SkillHurtDef = 100; //技能伤害抗性
            HurtDur = 100; //持续伤害
            HurtDurDef = 100; //持续伤害抗性
            AtkRange = 5;//攻击距离
            Position = new Vector3(0, 10, 0);
            MoveStatus = EnumUtil.MoveStatus.移动;
            Buffs = new Dictionary<EnumUtil.BuffType, Buff>();
        }

        
    }
}