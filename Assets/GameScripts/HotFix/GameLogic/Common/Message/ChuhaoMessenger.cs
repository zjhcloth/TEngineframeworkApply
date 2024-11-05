/*
 * Advanced C# messenger by Ilya Suzdalnitski. V1.0
 * 
 * Based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended".
 * 
 * Features:
 	* Prevents a MissingReferenceException because of a reference to a destroyed message handler.
 	* Option to log all messages
 	* Extensive error detection, preventing silent bugs
 * 
 * Usage examples:
 	1. Messenger.AddListener<GameObject>("prop collected", PropCollected);
 	   Messenger.Broadcast<GameObject>("prop collected", prop);
 	2. Messenger.AddListener<float>("speed changed", SpeedChanged);
 	   Messenger.Broadcast<float>("speed changed", 0.5f);
 * 
 * Messenger cleans up its evenTable automatically upon loading of a new level.
 * 
 * Don't forget that the messages that should survive the cleanup, should be marked with Messenger.MarkAsPermanent(string)
 * 
 */

//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
#define REQUIRE_LISTENER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameLogic
{
    static public class ChuhaoMessenger
    {

        #region Internal variables

        static public Dictionary<BaseMessengerEvent, Delegate> eventTable = new Dictionary<BaseMessengerEvent, Delegate>();

        public static Dictionary<string, BaseMessengerEvent> mAllMessengerEvent = new Dictionary<string, BaseMessengerEvent>();
        //Message handlers that should never be removed, regardless of calling Cleanup
        static public List<BaseMessengerEvent> permanentMessages = new List<BaseMessengerEvent>();
        #endregion
        #region Helper methods
        //Marks a certain message as permanent.
        static public void MarkAsPermanent(BaseMessengerEvent eventType)
        {
#if LOG_ALL_MESSAGES
		Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif

            permanentMessages.Add(eventType);
        }

        static public bool IsClean = false;

        static public void Cleanup()
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif

            List<BaseMessengerEvent> messagesToRemove = new List<BaseMessengerEvent>();

            foreach (KeyValuePair<BaseMessengerEvent, Delegate> pair in eventTable)
            {
                bool wasFound = false;

                foreach (BaseMessengerEvent message in permanentMessages)
                {
                    if (pair.Key == message)
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                    messagesToRemove.Add(pair.Key);
            }

            foreach (BaseMessengerEvent message in messagesToRemove)
            {
                eventTable.Remove(message);
            }
            eventTable.Clear();
            IsClean = true;
        }

        static public void PrintEventTable()
        {
            Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (KeyValuePair<BaseMessengerEvent, Delegate> pair in eventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log("\n");
        }
        #endregion

        #region Message logging and exception throwing
        static public void OnListenerAdding(BaseMessengerEvent eventType, Delegate listenerBeingAdded)
        {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		Debug.Log("MESSENGER OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif

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

        static public void OnListenerRemoving(BaseMessengerEvent eventType, Delegate listenerBeingRemoved)
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif

            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];

                if (d == null)
                {
                    throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
            }
            else
            {
                throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
            }
        }

        static public void OnListenerRemoved(BaseMessengerEvent eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }

        static public void OnBroadcasting(BaseMessengerEvent eventType)
        {
#if REQUIRE_LISTENER
            if (!eventTable.ContainsKey(eventType))
            {
                //throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
                return;
            }
#endif
        }

        static public BroadcastException CreateBroadcastSignatureException(BaseMessengerEvent eventType)
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
        static public void AddListener(BaseMessengerEvent eventType, Delegate handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = Delegate.Combine(eventTable[eventType], handler);
        }
        //No parameters
        static public void AddListener(BaseMessengerEvent eventType, Action handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action)eventTable[eventType] + handler;
        }
       
        //Single parameter
        static public void AddListener<T>(BaseMessengerEvent eventType, Action<T> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T>)eventTable[eventType] + handler;
        }

        //Two parameters
        static public void AddListener<T, U>(BaseMessengerEvent eventType, Action<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U>)eventTable[eventType] + handler;
        }

        //Three parameters
        static public void AddListener<T, U, V>(BaseMessengerEvent eventType, Action<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U, V>)eventTable[eventType] + handler;
        }
        #endregion

        #region RemoveListener
        static public void RemoveListener(BaseMessengerEvent eventType, Delegate handler)
        {
            if (IsClean)
            {
                return;
            }
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = Delegate.Remove(eventTable[eventType], handler);
            OnListenerRemoved(eventType);
        }
        //No parameters
        static public void RemoveListener(BaseMessengerEvent eventType, Action handler)
        {
            if (IsClean)
            {
                return;
            }
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Single parameter
        static public void RemoveListener<T>(BaseMessengerEvent eventType, Action<T> handler)
        {
            if (IsClean)
            {
                return;
            }
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Two parameters
        static public void RemoveListener<T, U>(BaseMessengerEvent eventType, Action<T, U> handler)
        {
            if (IsClean)
            {
                return;
            }
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Three parameters
        static public void RemoveListener<T, U, V>(BaseMessengerEvent eventType, Action<T, U, V> handler)
        {
            if (IsClean)
            {
                return;
            }
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U, V>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
        #endregion

        #region Broadcast
        //No parameters
        static public void Broadcast(BaseMessengerEvent eventType)
        {
            if (IsClean)
            {
                return;
            }
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
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
        static public void Broadcast<T>(BaseMessengerEvent eventType, T arg1)
        {
            if (IsClean)
            {
                return;
            }
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
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
        static public void Broadcast<T, U>(BaseMessengerEvent eventType, T arg1, U arg2)
        {
            if (IsClean)
            {
                return;
            }
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
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
        static public void Broadcast<T, U, V>(BaseMessengerEvent eventType, T arg1, U arg2, V arg3)
        {
            if (IsClean)
            {
                return;
            }
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
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
                public static T AddListener<T>(T target) where T : class
        {
            if (target == null)
                target = Activator.CreateInstance(typeof(T)) as T;
            Type targetType = target.GetType();
            MethodInfo[] methodInfos = targetType.GetMethods();
            MethodInfo[] methodInfos1 = targetType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.IsDefined(typeof(MessengerAttribute), false))
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(MessengerAttribute), false).OfType<MessengerAttribute>();
                    BaseMessengerEvent messengerEvent = mAllMessengerEvent[attribute.First().mMsgEvent];
                    Delegate evt = Delegate.CreateDelegate(messengerEvent.EventType(), target, methodInfo);
                    AddListener(messengerEvent, evt);
                }
            }
            foreach (MethodInfo methodInfo in methodInfos1)
            {
                if (methodInfo.IsDefined(typeof(MessengerAttribute), false))
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(MessengerAttribute), false).OfType<MessengerAttribute>();
                    BaseMessengerEvent messengerEvent = mAllMessengerEvent[attribute.First().mMsgEvent];
                    Delegate evt = Delegate.CreateDelegate(messengerEvent.EventType(), target, methodInfo);
                    AddListener(messengerEvent, evt);
                }
            }
            return target;
        }

        public static void RemoveListener<T>(T target) where T : class
        {
            if (target == null)
                return;
            Type targetType = target.GetType();
            MethodInfo[] methodInfos = targetType.GetMethods();
            MethodInfo[] methodInfos1 = targetType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.IsDefined(typeof(MessengerAttribute), false))
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(MessengerAttribute), false).OfType<MessengerAttribute>();
                    BaseMessengerEvent messengerEvent = mAllMessengerEvent[attribute.First().mMsgEvent];
                    Delegate evt = Delegate.CreateDelegate(messengerEvent.EventType(), target, methodInfo);
                    RemoveListener(messengerEvent, evt);
                }
            }
            foreach (MethodInfo methodInfo in methodInfos1)
            {
                if (methodInfo.IsDefined(typeof(MessengerAttribute), false))
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(MessengerAttribute), false).OfType<MessengerAttribute>();
                    BaseMessengerEvent messengerEvent = mAllMessengerEvent[attribute.First().mMsgEvent];
                    Delegate evt = Delegate.CreateDelegate(messengerEvent.EventType(), target, methodInfo);
                    RemoveListener(messengerEvent, evt);
                }
            }
        }

    }


   

}