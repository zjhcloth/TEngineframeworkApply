
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic
{
    public class BasePlayerMessengerEvent
    {
        public PlayerMessenger playerMessenger;
        public virtual Type EventType()
        {
            return null;
        }
        public BasePlayerMessengerEvent(string key, PlayerMessenger playerMessenger)
        {
            this.playerMessenger = playerMessenger;
            playerMessenger.mAllMessengerEvent.Add(key, this);
        }
    }

    public class PlayerMessengerEvent : BasePlayerMessengerEvent
    {
        Dictionary<string, string> childTypeDic = new Dictionary<string, string>();
        string msgTypeKey = "";
        public PlayerMessengerEvent(string key, PlayerMessenger playerMessenger) : base(key, playerMessenger)
        {
            this.msgTypeKey = ToString();
            childTypeDic.Add("", msgTypeKey);
        }
        public override Type EventType()
        {
            return typeof(Action);
        }
        public void AddListener(Action callBack, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.AddListener(childTypeDic[childType], callBack);
        }
        public void RemoveListener(Action callBack, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.RemoveListener(childTypeDic[childType], callBack);
        }
        public void Broadcast(string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.Broadcast(childTypeDic[childType]);
        }
    }

    public class PlayerMessengerEvent<T> : BasePlayerMessengerEvent
    {
        Dictionary<string, string> childTypeDic = new Dictionary<string, string>();
        string msgTypeKey = "";
        public PlayerMessengerEvent(string key, PlayerMessenger playerMessenger) : base(key, playerMessenger)
        {
            this.msgTypeKey = ToString();
            childTypeDic.Add("", msgTypeKey);
        }
        public override Type EventType()
        {
            return typeof(Action<T>);
        }
        public void AddListener(Action<T> callBack, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.AddListener(childTypeDic[childType], callBack);
        }
        public void RemoveListener(Action<T> callBack, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.RemoveListener(childTypeDic[childType], callBack);
        }
        //Two parameters
        public void Broadcast(T arg1, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.Broadcast(childTypeDic[childType], arg1);
        }
    }

    public class PlayerMessengerEvent<T, U> : BasePlayerMessengerEvent
    {
        Dictionary<string, string> childTypeDic = new Dictionary<string, string>();
        string msgTypeKey = "";
        public PlayerMessengerEvent(string key, PlayerMessenger playerMessenger) : base(key, playerMessenger) 
        { 
            this.msgTypeKey = ToString(); 
            childTypeDic.Add("", msgTypeKey); 
        }
        public override Type EventType()
        {
            return typeof(Action<T, U>);
        }
        public void AddListener(Action<T, U> callBack, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.AddListener(childTypeDic[childType], callBack);


        }
        public void RemoveListener(Action<T, U> callBack, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.RemoveListener(childTypeDic[childType], callBack);

        }
        //Two parameters
        public void Broadcast(T arg1, U arg2, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.Broadcast(childTypeDic[childType], arg1, arg2);
        }
    }

    public class PlayerMessengerEvent<T, U, V> : BasePlayerMessengerEvent
    {
        Dictionary<string, string> childTypeDic = new Dictionary<string, string>();
        string msgTypeKey = "";
        public PlayerMessengerEvent(string key, PlayerMessenger playerMessenger) : base(key, playerMessenger) { 
            this.msgTypeKey = ToString(); 
            childTypeDic.Add("", msgTypeKey); 
        }
        public override Type EventType()
        {
            return typeof(Action<T, U, V>);
        }
        public void AddListener(Action<T, U, V> callBack, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.AddListener(childTypeDic[childType], callBack);
        }
        public void RemoveListener(Action<T, U, V> callBack, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.RemoveListener(childTypeDic[childType], callBack);
        }
        //Two parameters
        public void Broadcast(T arg1, U arg2, V arg3, string childType = "")
        {
            if (!childTypeDic.ContainsKey(childType))
                childTypeDic[childType] = msgTypeKey + childType;
            playerMessenger.Broadcast(childTypeDic[childType], arg1, arg2, arg3);
        }
    }
}