using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameLogic
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MessengerAttribute : Attribute
    {
        public string mMsgEvent { get; set; }
               
        public MessengerAttribute(string msgEvent)
        {
            this.mMsgEvent = msgEvent;
        }
    }
}