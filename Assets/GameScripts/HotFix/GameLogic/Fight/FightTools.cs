//=====================================================
// - FileName: FightTools.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 15:02:16
// - Description:
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class FightTools
    {
        public static EnumUtil.CampPosType GetCampPosTypeByIdx(int idx)
        {
            if (idx <= 3)
            {
                return EnumUtil.CampPosType.前排;
            }
            else if (idx <= 6)
            {
                return EnumUtil.CampPosType.中排;
            }
            else
            {
                return EnumUtil.CampPosType.后排;
            }
        }
    }
}