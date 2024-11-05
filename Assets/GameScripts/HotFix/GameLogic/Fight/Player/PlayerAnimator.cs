using UnityEngine;

namespace GameLogic
{
    public class PlayerAnimator : BaseAnimator
    {
        public PlayerAnimator(Animator animator) : base(animator)
        {
        }

        /// <summary>
        /// 最终触发改变动作
        /// </summary>
        public void DoTrigger()
        {
            this.SetTrigger("IsTrigger");
        }
        
        /// <summary>
        /// 设置武器类型,读取角色控制器
        /// </summary>
        /// <param name="weaponType"></param>
        public void DoChangeWeaponType(int sex, int weaponType)
        {
            string animCtrlKey = "Wp_" + sex + "_" + weaponType;
            if (mAnimator == null) return; //Mono.mAnimator应该在角色加载之后,从加载后的OBJ上取得
            ChangeAnimatorCtr(animCtrlKey, null);
            // this.SetInt("Weapon", weaponType);
        }

        private float mDirX = 0;
        private float mDirZ = 0;
        private float mCurDirX = 0;
        private float mCurDirZ = 0;
        private float mSpeed = 5;
        private float spped = 1;

        public void DoChangeMoveSpeedDir(float x, float z)
        {
            mDirX = Mathf.Clamp(x, -0.99f, 0.99f) * spped;
            mDirZ = Mathf.Clamp(z, -0.99f, 0.99f) * spped;
        }

        public void DoChangeSpeed(float speed)
        {
            spped = (speed / 8);

            mAnimator.speed = spped > 1 ? 1.2f : 1;

            spped = Mathf.Clamp(spped, 0.8f, 1);
            if (spped >= 0.99f)
            {
                mDirX *= 100;
                mDirZ *= 100;
            }

            mDirX = Mathf.Clamp(mDirX, -0.99f, 0.99f) * spped;
            mDirZ = Mathf.Clamp(mDirZ, -0.99f, 0.99f) * spped;
        }

        public void Update()
        {
            if (Mathf.Abs(mCurDirX - mDirX) > mSpeed * Time.deltaTime)
                mCurDirX += (mCurDirX < mDirX ? 1 : -1) * mSpeed * Time.deltaTime;
            else
                mCurDirX = mDirX;
            if (Mathf.Abs(mCurDirZ - mDirZ) > mSpeed * Time.deltaTime)
                mCurDirZ += (mCurDirZ < mDirZ ? 1 : -1) * mSpeed * Time.deltaTime;
            else
                mCurDirZ = mDirZ;
            this.SetFloat("xSpeed", mCurDirX);
            this.SetFloat("zSpeed", mCurDirZ);
        }
    }
}