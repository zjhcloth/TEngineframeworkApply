//=====================================================
// - FileName: CollisionCheck.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 17:24:54
// - Description: 碰撞检测算法
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class CollisionCheck
    {
        /// <summary>
        /// 距离检查
        /// </summary>
        /// <param name="mStPos">起始点</param>
        /// <param name="mEdPos">结束点</param>
        /// <param name="dis">攻击距离</param>
        /// <returns></returns>
        public static bool CanAttack(Vector3 mStPos, Vector3 mEdPos, float dis)
        {

            float x = mStPos.x - mEdPos.x;
            float z = mStPos.z - mEdPos.z;
            return (x * x + z * z) <= dis * dis;
        }
    }
}
