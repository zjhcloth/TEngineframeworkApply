using System;

namespace GameLogic
{
    public class BaseMessengerEvent
    {
        public BaseMessengerEvent(string key)
        {
            ChuhaoMessenger.mAllMessengerEvent.Add(key, this);
        }
        public virtual Type EventType()
        {
            return null;
        }
    }

    public class MessengerEvent : BaseMessengerEvent
    {
        public MessengerEvent(string key) : base(key) { }
        public override Type EventType()
        {
            return typeof(Action);
        }
        public void AddListener(Action callBack)
        {
            ChuhaoMessenger.AddListener(this, callBack);
        }
        public void RemoveListener(Action callBack)
        {
            ChuhaoMessenger.RemoveListener(this, callBack);
        }
        //Two parameters
        public void Broadcast()
        {
            ChuhaoMessenger.Broadcast(this);
        }
    }

    public class MessengerEvent<T> : BaseMessengerEvent
    {
        public MessengerEvent(string key) : base(key) { }
        public override Type EventType()
        {
            return typeof(Action<T>);
        }
        public void AddListener(Action<T> callBack)
        {
            ChuhaoMessenger.AddListener(this, callBack);
        }
        public void RemoveListener(Action<T> callBack)
        {
            ChuhaoMessenger.RemoveListener(this, callBack);
        }
        //Two parameters
        public void Broadcast(T arg1)
        {
            ChuhaoMessenger.Broadcast(this, arg1);
        }
    }


    public class MessengerEvent<T, U> : BaseMessengerEvent
    {
        public MessengerEvent(string key) : base(key) { }
        public override Type EventType()
        {
            return typeof(Action<T, U>);
        }
        public void AddListener(Action<T, U> callBack)
        {
            ChuhaoMessenger.AddListener(this, callBack);
        }
        public void RemoveListener(Action<T, U> callBack)
        {
            ChuhaoMessenger.RemoveListener(this, callBack);
        }
        //Two parameters
        public void Broadcast(T arg1, U arg2)
        {
            ChuhaoMessenger.Broadcast(this, arg1, arg2);
        }
    }
    public class MessengerEvent<T, U, V> : BaseMessengerEvent
    {
        public MessengerEvent(string key) : base(key) { }
        public override Type EventType()
        {
            return typeof(Action<T, U, V>);
        }
        public void AddListener(Action<T, U, V> callBack)
        {
            ChuhaoMessenger.AddListener(this, callBack);
        }
        public void RemoveListener(Action<T, U, V> callBack)
        {
            ChuhaoMessenger.RemoveListener(this, callBack);
        }
        //Two parameters
        public void Broadcast(T arg1, U arg2, V arg3)
        {
            ChuhaoMessenger.Broadcast(this, arg1, arg2, arg3);
        }
    }
}