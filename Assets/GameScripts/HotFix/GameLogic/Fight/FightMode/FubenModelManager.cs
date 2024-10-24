//=====================================================
// - FileName: FubenModelManager.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/23 15:07:36
// - Description: 副本战斗模式
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class FubenModelManager : FrameModeManager
    {
        public override void InitData()
        {
            base.InitData();

            mGameTime = 150;


            MessengerKey.DieEvent.AddListener(Die);

        }

        public override void Clear()
        {
            base.Clear();
            MessengerKey.DieEvent.RemoveListener(Die);
        }

        public override void InitComplete()
        {
            base.InitComplete();
        }

        public override void SFUpdate()
        {
            if (mGameTime > 0)
            {
                mGameTime -= STime.deltaTime;
            }
            else
            {
                CmdManager.Instance.GameOver = true;
                MainFightCtrl.Instance.StartCoroutine(Finish());
            }
        }



        public void Die(int bLoc, int loc)
        {
            if (bLoc == loc)
                return;
        }

        private IEnumerator Finish()
        {
            mStart = false;

            //UIFinishLogic.Show();
            yield return new WaitForSeconds(1); //发完结算之后，1秒之后就离开战斗，而不用等到所有动画都播完再离开战斗服
            //UIFinishLogic.Remove();
            //结算界面展示
            //UIBattleScoreLogic.Show();
        }
    }
}
