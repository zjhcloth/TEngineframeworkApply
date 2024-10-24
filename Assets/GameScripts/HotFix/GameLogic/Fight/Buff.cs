//=====================================================
// - FileName: Buff.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/22 14:49:58
// - Description:
//======================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class Buff
    {
        public int MaxCount;//叠加层数
        public int Count;//叠加层数
        public EnumUtil.BuffType Type;//buff类型
        public float KeepTime;//持续时间
        public float EndTime;//结束时间
        public Action OnFinish;//buff结束的回调
        public Buff Clone()
        {
            Buff copy = this.MemberwiseClone() as Buff;
            return copy;
        }
    }
}
