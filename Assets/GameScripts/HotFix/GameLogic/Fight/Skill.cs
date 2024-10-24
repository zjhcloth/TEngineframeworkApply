//=====================================================
// - FileName: Skill.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/22 15:26:10
// - Description: 技能基础
//======================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class Skill
    {
        public int Id;//技能id
        public int Effect;//技能播放特效
        public int Act;//动作
        public float EndTime;//技能结束时间
        public float CD;//持续时间
        public bool NeedPause;//是否需要暂停
        public float Hurt = 10;//技能伤害
        public float AtkRange;//攻击范围
        public bool NeedTarget;//需要攻击目标
    }
}