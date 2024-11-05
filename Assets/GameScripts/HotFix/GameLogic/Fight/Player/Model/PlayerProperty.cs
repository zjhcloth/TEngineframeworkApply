//=====================================================
// - FileName: PlayerProperty.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 10:02:09
// - Description: 角色属性
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class PlayerProperty : BasePlayerProperty
    {

        //进阶属性（加成）
        private float _hurtAdd; //伤害加成 加成
        private float _hurtDefAdd; //伤害抗性 加成
        private float _energyRateAdd; //怒气效率 加成
        private float _avoAdd; //闪避率 加成
        private float _avoDisAdd; //忽视闪避率 加成
        private float _critRateAdd; //暴击率 加成
        private float _critRateDisAdd; //忽视暴击率 加成
        private float _critHurtAdd; //暴伤 加成
        private float _critHurtDisAdd; //忽视爆伤 加成
        private float _atkSpdAdd; //攻速 加成
        private float _norHurtAdd; //普攻伤害 加成
        private float _norHurtDefAdd; //普攻抗性 加成
        private float _skillHurtAdd; //技能伤害 加成
        private float _skillHurtDefAdd; //技能伤害抗性 加成
        private float _hurtDurAdd; //持续伤害 加成
        private float _hurtDurDefAdd; //持续伤害抗性 加成
        //战报面板上用的数据
        public float Damage; //输出
        public float Cure; //治疗
        public float UnderHurt; //承伤
        
        public PlayerProperty(int loc) : base(loc)
        {
            HeroType = EnumUtil.HeroType.万变;

            Atk = 100; //攻击
            Def = 100; //防御
            Hp = 100000; //气血
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
            Speed = 2;//移动速度
            NorHurt = 100; //普攻伤害
            NorHurtDef = 100; //普攻抗性
            SkillHurt = 100; //技能伤害
            SkillHurtDef = 100; //技能伤害抗性
            HurtDur = 100; //持续伤害
            HurtDurDef = 100; //持续伤害抗性
            AtkRange = 10;//攻击距离
            Position = new Vector3(2.5f, 0, -7);
            MoveStatus = EnumUtil.MoveStatus.移动;
            Buffs = new Dictionary<EnumUtil.BuffType, Buff>();
        }
    }
}