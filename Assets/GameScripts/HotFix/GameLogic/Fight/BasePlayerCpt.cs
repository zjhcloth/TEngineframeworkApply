using System;

namespace GameLogic
{
    public class BasePlayerCpt<T> where T : BaseCptMono, new()
    {
        protected T mMono;
        protected BasePlayerProperty MBasePlayerPro;
        protected int mLoc;
        bool isRemoveEvent = false;
        public BasePlayerCpt(int loc)
        {
            mLoc = loc;
            MBasePlayerPro = BaseFightData.GetPlayerPro<BasePlayerProperty>(loc);
            if (MBasePlayerPro == null)
                return;
            isRemoveEvent = false;
            MBasePlayerPro.mPlayerMessenger.AddListener(this);
            ChuhaoMessenger.AddListener(this);
        }

        public virtual void Init(Action<T> config)
        {
            if (config != null)
            {
                mMono = new T();
                config(mMono);
            }
        }

        public virtual void Clear()
        {
            if (MBasePlayerPro == null || isRemoveEvent)
                return;
            MBasePlayerPro.mPlayerMessenger.RemoveListener(this);
            ChuhaoMessenger.RemoveListener(this);
            isRemoveEvent = true;
        }
    }
}
