using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
namespace GameLogic
{
    public class BaseAnimator
    {
        //人物动作
        protected Animator mAnimator = null;
        protected string mCtrName = "";
        protected Dictionary<string,int> mDicNameId = new Dictionary<string, int>();
        public BaseAnimator(Animator animator)
        {
            mAnimator = animator;
        }

        /// <summary>
        /// 加载角色动作，通过AnimatorOverrideController
        /// </summary>
        /// <param name="assetNameString"></param>
        /// <param name="loadBack"></param>
        /// <returns></returns>
        public async void SetOverrideController(string assetNameString, Action<RuntimeAnimatorController> loadBack)
        {
            try
            {
                var result = await GameModule.Resource.LoadAssetAsync<RuntimeAnimatorController>(assetNameString);

                loadBack(result);
            }
            catch (Exception ex)
            {
                Debug.LogError("Get Anim Asset Error; " + assetNameString);
            }
        }

        public int GetInt(string name) 
        {
            if (mAnimator == null) return -1;
            return mAnimator.GetInteger(name);
        }

        public bool IsAnimName(string name)
        {
            if (mAnimator == null) return false;
            AnimatorStateInfo stateinfo = mAnimator.GetCurrentAnimatorStateInfo(0);
            return stateinfo.IsName(name);
        }

        public bool IsAnimTag(string tag)
        {
            AnimatorStateInfo stateinfo = mAnimator.GetCurrentAnimatorStateInfo(0);
            return stateinfo.IsTag(tag);
        }
        /// <summary>
        /// 改变动作控制器
        /// </summary>
        /// <param name="ctrName"></param>
        protected void ChangeAnimatorCtr(string ctrName, Action callback)
        {
            if (mCtrName == ctrName) return;
            mCtrName = ctrName;
            SetOverrideController(mCtrName, (pTimeController) =>
            {
                this.mAnimator.runtimeAnimatorController = pTimeController;
                if (callback != null)
                {
                    callback();
                }
            });
        }

        public void SetAniSpeed(float speed)
        {
            mAnimator.speed = speed;
        }

        public virtual void SetFloat(string name, float value)
        {
            if (this.mAnimator != null && mAnimator.runtimeAnimatorController != null && this.mAnimator.gameObject.activeInHierarchy)
            {
                if (!mDicNameId.TryGetValue(name,out int value1))
                {
                    mDicNameId.Add(name, Animator.StringToHash(name));
                }
                this.mAnimator.SetFloat(mDicNameId[name], value);
            }
        }
        public virtual void SetInt(string name, int value)
        {
            if (this.mAnimator != null && mAnimator.runtimeAnimatorController != null && this.mAnimator.gameObject.activeInHierarchy)
            {
                if (!mDicNameId.TryGetValue(name, out int value1))
                {
                    mDicNameId.Add(name, Animator.StringToHash(name));
                }
                this.mAnimator.SetInteger(mDicNameId[name], value);
            }
        }
        public virtual void SetBool(string name, bool value)
        {
            if (this.mAnimator != null && mAnimator.runtimeAnimatorController != null && this.mAnimator.gameObject.activeInHierarchy)
            {
                if (!mDicNameId.TryGetValue(name, out int value1))
                {
                    mDicNameId.Add(name, Animator.StringToHash(name));
                }
                this.mAnimator.SetBool(mDicNameId[name], value);
            }
        }
        public virtual bool GetBool(string name)
        {
            if (this.mAnimator != null && mAnimator.runtimeAnimatorController != null && this.mAnimator.gameObject.activeInHierarchy)
            {
                if (!mDicNameId.TryGetValue(name, out int value1))
                {
                    mDicNameId.Add(name, Animator.StringToHash(name));
                }
                return this.mAnimator.GetBool(mDicNameId[name]);
            }
            return false;
        }
        public virtual void SetTrigger(string name)
        {
            if (this.mAnimator != null && mAnimator.runtimeAnimatorController != null && this.mAnimator.gameObject.activeInHierarchy)
            {
                if (!mDicNameId.TryGetValue(name, out int value1))
                {
                    mDicNameId.Add(name, Animator.StringToHash(name));
                }
                this.mAnimator.SetTrigger(mDicNameId[name]);
            }
        }

        public virtual void SetEnable(bool enable)
        {
            this.mAnimator.enabled = enable;
        }
        public virtual void Clear()
        {
            this.mAnimator = null;
        }
    }
}