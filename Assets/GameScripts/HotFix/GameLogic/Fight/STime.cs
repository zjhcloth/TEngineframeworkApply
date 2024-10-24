//=====================================================
// - FileName: STime.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 19:51:32
// - Description: 帧同步用时间常量
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STime
{
       public static float deltaTime = 0.066f;//每帧走的时间（每秒15帧逻辑帧）
       public static float time = 0;//战斗开始走了多少时间
       public static int curFrame = 1; //战斗当前帧
       public static void UpdateFrame(int curMaxFrame)
       {
              curFrame = curMaxFrame;
              time = curMaxFrame * deltaTime;
       }
}