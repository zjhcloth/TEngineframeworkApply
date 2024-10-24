//=====================================================
// - FileName: BaseFightData.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 14:55:47
// - Description:
//======================================================

using System;
using System.Collections.Generic;

namespace GameLogic
{
       public class BaseFightData
       {
              public static SRandom mRandom;//本次战斗随机数
              private static long mRandSeed;//随机种子
              public static Type mFightModeType;
              public static Dictionary<int, BasePlayerProperty> mPlayerDataList = new Dictionary<int, BasePlayerProperty>();
              //表现层的角色控制器
              public static Dictionary<int, FightPlayerCtrl> mPlayerCtrlList = new Dictionary<int, FightPlayerCtrl>();
              public static Dictionary<int, BasePlayerProperty> mEnemyDataList = new Dictionary<int, BasePlayerProperty>();
              public static void SetRandom(long randSeed)
              {
                     mRandSeed = randSeed;
                     mRandom = new SRandom(mRandSeed);
                     //if (StaticData.DebugState)
                     //{
                     //    BuryBattleModel.Instance.AddDebugStr($"|key: ExtFightData_SetRandom|mRandSeed: {mRandSeed}");
                     //}
              }
              
              public static void AddPlayerCtrl(int loc, FightPlayerCtrl ctrl)
              {
                     //GameDebug.Log("AddPlayerCtrl" + loc, DebugColorType.Blue);
                     mPlayerCtrlList.Add(loc, ctrl);
              }
              public static T GetPlayerPro<T>(int loc) where T : BasePlayerProperty
              {
                     if (mPlayerDataList.ContainsKey(loc))
                     {
                            return mPlayerDataList[loc] as T;
                     }
                     return null;
              }
              
              public static T GetEnemyPlayerPro<T>(int loc) where T : BasePlayerProperty
              {
                     if (mEnemyDataList.ContainsKey(loc))
                     {
                            return mEnemyDataList[loc] as T;
                     }
                     return null;
              }
              public static void Update()
              {
                     foreach (var item in mPlayerCtrlList)
                     {
                            item.Value.DoUpdate();
                     }
              }

              public static void FixUpdate()
              {
                     foreach (var item in mPlayerCtrlList)
                     {
                            item.Value.DoFixUpdate();
                     }
              }


              public static void  Clear()
              {
                     //TODO;
              }
       }
}