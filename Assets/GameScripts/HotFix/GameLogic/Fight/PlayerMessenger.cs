using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameLogic
{
    public class PlayerMessenger
    {
        public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
        public Dictionary<string, BasePlayerMessengerEvent> mAllMessengerEvent = new Dictionary<string, BasePlayerMessengerEvent>();

        #region Message logging and exception throwing
        public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
            }
        }

        public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
        {
            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];

                if (d == null)
                {
                    Debug.LogError(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Debug.LogError(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
            }
            else
            {
                Debug.LogError(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
            }
        }

        public void OnListenerRemoved(string eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }

        public void OnBroadcasting(string eventType)
        {
#if REQUIRE_LISTENER
        if (!eventTable.ContainsKey(eventType))
        {
            //throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
            return;
        }
#endif
        }

        static public BroadcastException CreateBroadcastSignatureException(string eventType)
        {
            return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
        }

        public class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg)
            {
            }
        }

        public class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg)
            {
            }
        }
        #endregion

        #region AddListener
        public void AddListener(string eventType, Delegate handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = Delegate.Combine(eventTable[eventType], handler);
        }
        //No parameters
        public void AddListener(string eventType, Action handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action)eventTable[eventType] + handler;
        }

        //Single parameter
        public void AddListener<T>(string eventType, Action<T> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T>)eventTable[eventType] + handler;
        }

        //Two parameters
        public void AddListener<T, U>(string eventType, Action<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U>)eventTable[eventType] + handler;
        }

        //Three parameters
        public void AddListener<T, U, V>(string eventType, Action<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U, V>)eventTable[eventType] + handler;
        }
        #endregion

        #region ChgListener
        //No parameters
        public void ChgListener(string eventType, Action handler, int type = 1)
        {
            if (type == 1)
                AddListener(eventType, handler);
            else
                RemoveListener(eventType, handler);
        }

        //Single parameter
        public void ChgListener<T>(string eventType, Action<T> handler, int type = 1)
        {
            if (type == 1)
                AddListener(eventType, handler);
            else
                RemoveListener(eventType, handler);
        }

        //Two parameters
        public void ChgListener<T, U>(string eventType, Action<T, U> handler, int type = 1)
        {
            if (type == 1)
                AddListener(eventType, handler);
            else
                RemoveListener(eventType, handler);
        }

        //Three parameters
        public void ChgListener<T, U, V>(string eventType, Action<T, U, V> handler, int type = 1)
        {
            if (type == 1)
                AddListener(eventType, handler);
            else
                RemoveListener(eventType, handler);
        }

        public T AddListener<T>(T target) where T : class
        {
            if (target == null)
                target = Activator.CreateInstance(typeof(T)) as T;
            Type targetType = target.GetType();
            MethodInfo[] methodInfos = targetType.GetMethods();
            MethodInfo[] methodInfos1 = targetType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.IsDefined(typeof(PlayerMessengerAttribute), false))
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(PlayerMessengerAttribute), false).OfType<PlayerMessengerAttribute>();
                    BasePlayerMessengerEvent playerMessengerEvent = mAllMessengerEvent[attribute.First().key];
                    Delegate evt = Delegate.CreateDelegate(playerMessengerEvent.EventType(), target, methodInfo);
                    AddListener(playerMessengerEvent + attribute.First().childType, evt);
                }
            }
            foreach (MethodInfo methodInfo in methodInfos1)
            {
                if (methodInfo.IsDefined(typeof(PlayerMessengerAttribute), false))
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(PlayerMessengerAttribute), false).OfType<PlayerMessengerAttribute>();
                    BasePlayerMessengerEvent playerMessengerEvent = mAllMessengerEvent[attribute.First().key];
                    Delegate evt = Delegate.CreateDelegate(playerMessengerEvent.EventType(), target, methodInfo);
                    AddListener(playerMessengerEvent + attribute.First().childType, evt);
                }
            }
            return target;
        }

        public void RemoveListener<T>(T target) where T : class
        {
            if (target == null)
                return;
            Type targetType = target.GetType();
            MethodInfo[] methodInfos = targetType.GetMethods();
            MethodInfo[] methodInfos1 = targetType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.IsDefined(typeof(PlayerMessengerAttribute), false))
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(PlayerMessengerAttribute), false).OfType<PlayerMessengerAttribute>();
                    BasePlayerMessengerEvent playerMessengerEvent = mAllMessengerEvent[attribute.First().key];
                    Delegate evt = Delegate.CreateDelegate(playerMessengerEvent.EventType(), target, methodInfo);
                    RemoveListener(playerMessengerEvent + attribute.First().childType, evt);
                }
            }
            foreach (MethodInfo methodInfo in methodInfos1)
            {
                if (methodInfo.IsDefined(typeof(PlayerMessengerAttribute), false))
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(PlayerMessengerAttribute), false).OfType<PlayerMessengerAttribute>();
                    BasePlayerMessengerEvent playerMessengerEvent = mAllMessengerEvent[attribute.First().key];
                    Delegate evt = Delegate.CreateDelegate(playerMessengerEvent.EventType(), target, methodInfo);
                    RemoveListener(playerMessengerEvent + attribute.First().childType, evt);
                }
            }
        }
        #endregion

        #region RemoveListener
        public void RemoveListener(string eventType, Delegate handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = Delegate.Remove(eventTable[eventType], handler);
            OnListenerRemoved(eventType);
        }
        //No parameters
        public void RemoveListener(string eventType, Action handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Single parameter
        public void RemoveListener<T>(string eventType, Action<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Two parameters
        public void RemoveListener<T, U>(string eventType, Action<T, U> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Three parameters
        public void RemoveListener<T, U, V>(string eventType, Action<T, U, V> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U, V>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
        #endregion

        #region Broadcast
        //No parameters
        public void Broadcast(string eventType)
        {
            OnBroadcasting(eventType);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action callback = d as Action;

                if (callback != null)
                {
                    callback();
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Single parameter
        public void Broadcast<T>(string eventType, T arg1)
        {
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T> callback = d as Action<T>;

                if (callback != null)
                {
                    callback(arg1);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Two parameters
        public void Broadcast<T, U>(string eventType, T arg1, U arg2)
        {
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U> callback = d as Action<T, U>;

                if (callback != null)
                {
                    callback(arg1, arg2);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Three parameters
        public void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
        {
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U, V> callback = d as Action<T, U, V>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }
        #endregion


        //public delegate void Callback();
        //public delegate void Callback<T>(T arg1);
        //public delegate void Callback<T, U>(T arg1, U arg2);
        //public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

    }
}